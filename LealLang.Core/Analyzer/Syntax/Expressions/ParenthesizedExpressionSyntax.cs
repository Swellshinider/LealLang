namespace LealLang.Core.Analyzer.Syntax.Expressions;

public sealed class ParenthesizedExpressionSyntax : ExpressionSyntax
{
	public ParenthesizedExpressionSyntax(SyntaxToken openParenthesisToken, ExpressionSyntax expression, SyntaxToken closeParenthesisToken) 
	{
		OpenParenthesisToken = openParenthesisToken;
		Expression = expression;
		CloseParenthesisToken = closeParenthesisToken;
	}

	public SyntaxToken OpenParenthesisToken { get; }
	public ExpressionSyntax Expression { get; }
	public SyntaxToken CloseParenthesisToken { get; }

    public override SyntaxKind Kind => SyntaxKind.ParenthesizedExpression;
}