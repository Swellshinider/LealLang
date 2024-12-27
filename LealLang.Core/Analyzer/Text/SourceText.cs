
using System.Collections.Immutable;

namespace LealLang.Core.Analyzer.Text;

public sealed class SourceText
{
	private readonly string _text;

	public static SourceText From(string text) => new(text);

	private SourceText(string text)
	{
		_text = text;
		Lines = ParseLines(this, text);
	}

	public ImmutableArray<TextLine> Lines { get; private set; }
	
	public char this[int index] => _text[index];
	
	public int Length => _text.Length;
	
	public string Text => _text;

	public int FindLineIndex(int position)
	{
		var lower = 0;
		var upper = Lines.Length - 1;

		while (lower <= upper)
		{
			var index = lower + (upper - lower) / 2;
			var start = Lines[index].Start;

			if (position == start)
				return index;

			if (position < start)
				upper = index - 1;
			else
				lower = index + 1;
		}

		return lower - 1;
	}

	public override string ToString() => _text;
	
	public string ToString(int start, int len) => _text[start..len];
	
	public string ToString(TextSpan span) => ToString(span.Start, span.Length);

	private static ImmutableArray<TextLine> ParseLines(SourceText sourceText, string text)
	{
		var result = ImmutableArray.CreateBuilder<TextLine>();
		var position = 0;
		var lineStart = 0;

		while (position < text.Length)
		{
			var lineWidthWithBreaks = GetLineLenWithBreaks(text, position);

			if (lineWidthWithBreaks == 0)
				position++;
			else
			{
				result.Add(NewLine(sourceText, position, lineStart, lineWidthWithBreaks));
				position += lineWidthWithBreaks;
				lineStart = position;
			}
		}

		if (position > lineStart)
			result.Add(NewLine(sourceText, position, lineStart, 0));

		return result.ToImmutable();
	}

	private static TextLine NewLine(SourceText sourceText, int position, int lineStart, int lineWidthWithBreaks)
	{
		var lineLen = position - lineStart;
		var lineLenWithBreaks = lineLen + lineWidthWithBreaks;
		return new(sourceText, lineStart, lineLen, lineLenWithBreaks);
	}

	private static int GetLineLenWithBreaks(string text, int pos)
	{
		var curr = text[pos];
		var next = pos + 1 >= text.Length ? '\0' : text[pos + 1];

		if (curr == '\r' && next == '\n')
			return 2;

		if (curr == '\r' || next == '\n')
			return 1;

		return 0;
	}
}