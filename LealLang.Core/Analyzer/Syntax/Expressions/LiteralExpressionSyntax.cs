namespace LealLang.Core.Analyzer.Syntax.Expressions;

public sealed class LiteralExpressionSyntax : ExpressionSyntax
{
	public LiteralExpressionSyntax(SyntaxToken numberToken)
	{
		NumberToken = numberToken;
	}
	
	public SyntaxToken NumberToken { get; }
	
	public override SyntaxKind Kind => SyntaxKind.NumberExpression;
}