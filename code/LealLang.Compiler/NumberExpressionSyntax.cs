
namespace LealLang.Compiler;

public sealed class NumberExpressionSyntax : ExpressionSyntax
{
	public NumberExpressionSyntax(SyntaxToken numberToken)
	{
		NumberToken = numberToken;
	}
	
	public SyntaxToken NumberToken { get; }
	
	public override SyntaxKind Kind => SyntaxKind.NumberExpression;

	public override IEnumerable<SyntaxNode> GetChildren()
	{
		yield return NumberToken;
	}
}
