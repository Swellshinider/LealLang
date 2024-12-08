using LealLang.Core.Syntax;
using LealLang.Core.Syntax.Expressions;

namespace LealLang.Core.Analyzer;

public sealed class Evaluator
{
	private readonly ExpressionSyntax _expression;
	
	public Evaluator(ExpressionSyntax expression)
	{
		_expression = expression;
	}
	
	public object Evaluate()
	{
		return EvaluateExpression(_expression);
	}

	private object EvaluateExpression(ExpressionSyntax expression) => expression switch
	{
		LiteralExpressionSyntax literal => literal.Value,
		BinaryExpressionSyntax binary => EvaluateBinaryExpression(binary),
		UnaryExpressionSyntax unary => EvaluateUnaryExpression(unary),
		_ => throw new InvalidOperationException($"Unexpected expression {expression}")
	};

	private object EvaluateUnaryExpression(UnaryExpressionSyntax unary)
	{
		var value = EvaluateExpression(unary.Operand);
		
		return unary.OperatorToken.Kind switch
		{
			SyntaxKind.PlusToken => (int)value,
			SyntaxKind.MinusToken => -(int)value,
			_ => throw new InvalidOperationException($"Unexpected unary operator {unary.OperatorToken.Kind}")
		};
	}

	private object EvaluateBinaryExpression(BinaryExpressionSyntax binary)
	{
		var left = EvaluateExpression(binary.Left);
		var right = EvaluateExpression(binary.Right);
		
		return binary.OperatorToken.Kind switch
		{
			SyntaxKind.PlusToken => (int)left + (int)right,
			SyntaxKind.MinusToken => (int)left - (int)right,
			SyntaxKind.StarToken => (int)left * (int)right,
			SyntaxKind.SlashToken => (int)left / (int)right,
			_ => throw new InvalidOperationException($"Unexpected binary operator {binary.OperatorToken.Kind}")
		};
	}
}