namespace LealLang.Core.Analyzer.Syntax.Expressions;

public sealed class NumberExpressionSyntax : ExpressionSyntax
{
	public NumberExpressionSyntax(SyntaxToken numberToken)
	{
		NumberToken = numberToken;
	}
	
	public SyntaxToken NumberToken { get; }
	
	public override SyntaxKind Kind => SyntaxKind.NumberExpression;
}