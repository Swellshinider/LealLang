namespace LealLang.Core.Syntax.Expressions;

public sealed class UnaryExpressionSyntax : ExpressionSyntax
{
	public UnaryExpressionSyntax(SyntaxToken operatorToken, ExpressionSyntax operand)
	{
		OperatorToken = operatorToken;
		Operand = operand;
	}

	public ExpressionSyntax Operand { get; }
	public SyntaxToken OperatorToken { get; }

	public override SyntaxKind Kind => SyntaxKind.UnaryExpression;
}