namespace LealLang.Core.Analyzer.Text;

public readonly struct TextSpan
{
	public TextSpan(int start, int length)
	{
		Start = start;
		Length = length;
	}

	public int Start { get; }
	public int Length { get; }
	public int End => Start + Length;

	public static TextSpan FromBounds(int start, int end) => new(start, end - start);
}