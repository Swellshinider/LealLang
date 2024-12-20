namespace LealLang.Core.Analyzer.Syntax.Expressions;

public sealed class LiteralExpressionSyntax : ExpressionSyntax
{
	public LiteralExpressionSyntax(SyntaxToken numberToken)
	{
		LiteralToken = numberToken;
	}
	
	public SyntaxToken LiteralToken { get; }
	
	public override SyntaxKind Kind => SyntaxKind.LiteralExpression;
}