namespace LealLang.Core.Analyzer.Text;

public sealed class TextLine
{
	public TextLine(SourceText text, int start, int length, int lengthWithLineBreaks)
	{
		Text = text;
		Start = start;
		Length = length;
		LengthWithLineBreaks = lengthWithLineBreaks;
	}

	public SourceText Text { get; }
	public int Start { get; }
	public int Length { get; }
	public int End => Start + Length;
	public int LengthWithLineBreaks { get; }
	public int EndWithLineBreaks => Start + LengthWithLineBreaks;
	public TextSpan Span => new(Start, Length);
	public TextSpan SpanWithLineBreaks => new(Start, LengthWithLineBreaks);
	
	public override string ToString() => Text.ToString(Span);
}