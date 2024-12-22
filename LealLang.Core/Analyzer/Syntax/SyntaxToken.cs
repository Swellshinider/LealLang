using LealLang.Core.Analyzer.Text;

namespace LealLang.Core.Analyzer.Syntax;

public sealed class SyntaxToken : SyntaxNode
{
	public SyntaxToken(SyntaxKind kind, int position, string? text, object? value = null)
	{
		Kind = kind;
		Position = position;
		Text = text;
		Value = value;
	}
	
	public int Position { get; set; }
	public string? Text { get; set; }
	public object? Value { get; set; }
	public TextSpan Span => new(Position, (Text ?? "").Length);
	
	public override SyntaxKind Kind { get; }
}