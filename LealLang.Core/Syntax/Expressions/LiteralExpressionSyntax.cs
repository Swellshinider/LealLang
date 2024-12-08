namespace LealLang.Core.Syntax.Expressions;

public sealed class LiteralExpressionSyntax : ExpressionSyntax
{
	public LiteralExpressionSyntax(SyntaxToken literalToken)
		: this(literalToken, literalToken.Value!) { }

	public LiteralExpressionSyntax(SyntaxToken literalToken, object value)
	{
		Value = value;
		LiteralToken = literalToken;
	}

	public SyntaxToken LiteralToken { get; }
	public object Value { get; }
	public override SyntaxKind Kind => SyntaxKind.LiteralExpression;
}