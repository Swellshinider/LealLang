using LealLang.Core.Analyzer.Syntax.Expressions;

namespace LealLang.Core.Analyzer.Syntax;

public sealed class Evaluator
{
	private readonly ExpressionSyntax _expression;

	public Evaluator(ExpressionSyntax expression)
	{
		_expression = expression;
	}

	public object? Evaluate()
	{
		return EvaluateExpression(_expression);
	}

	private object EvaluateExpression(ExpressionSyntax expression) => expression.Kind switch
	{
		SyntaxKind.NumberExpression => ParseNumberExpression((LiteralExpressionSyntax)expression),
		SyntaxKind.BinaryExpression => ParseBinaryExpression((BinaryExpressionSyntax)expression),
		SyntaxKind.UnaryExpression => ParseUnaryExpression((UnaryExpressionSyntax)expression),
		SyntaxKind.ParenthesizedExpression => ParseParenthesizedExpression((ParenthesizedExpressionSyntax)expression),
		_ => throw new NotImplementedException()
	};

	private object ParseParenthesizedExpression(ParenthesizedExpressionSyntax expression)
	{
		return EvaluateExpression(expression.Expression);
	}

	private static int ParseNumberExpression(LiteralExpressionSyntax numberExpression)
	{
		var value = numberExpression.NumberToken.Value;
		
		if (value!.GetType() != typeof(int))
			throw new Exception("Impossible");

		return (int)value!;
	}

	private int ParseBinaryExpression(BinaryExpressionSyntax binaryExpression)
	{
		var left = EvaluateExpression(binaryExpression.Left);
		var right = EvaluateExpression(binaryExpression.Right);

		return binaryExpression.OperatorToken.Kind switch
		{
			SyntaxKind.PlusToken => (int)left + (int)right,
			SyntaxKind.MinusToken => (int)left - (int)right,
			SyntaxKind.StarToken => (int)left * (int)right,
			SyntaxKind.SlashToken => (int)left / (int)right,
			_ => throw new NotImplementedException()
		};
	}

	private int ParseUnaryExpression(UnaryExpressionSyntax unaryExpression)
	{
		return unaryExpression.OperatorToken.Kind switch
		{
			SyntaxKind.MinusToken => -(int)EvaluateExpression(unaryExpression.Operand),
			SyntaxKind.PlusToken => (int)EvaluateExpression(unaryExpression.Operand),
			_ => throw new NotImplementedException()
		};
	}
}