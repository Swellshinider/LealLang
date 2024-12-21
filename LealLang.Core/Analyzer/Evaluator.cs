using LealLang.Core.Analyzer.Binding;
using LealLang.Core.Analyzer.Binding.Expressions;

namespace LealLang.Core.Analyzer;

public sealed class Evaluator
{
	private readonly BoundExpression _expression;

	public Evaluator(BoundExpression expression)
	{
		_expression = expression;
	}

	public object? Evaluate()
	{
		return EvaluateExpression(_expression);
	}

	private object EvaluateExpression(BoundExpression expression) => expression.Kind switch
	{
		BoundNodeKind.LiteralExpression => EvaluateLiteralExpression((BoundLiteralExpression)expression),
		BoundNodeKind.BinaryExpression => EvaluateBinaryExpression((BoundBinaryExpression)expression),
		BoundNodeKind.UnaryExpression => EvaluateUnaryExpression((BoundUnaryExpression)expression),
		_ => -1
	};
	
	private static object EvaluateLiteralExpression(BoundLiteralExpression literalExpression)
		=> literalExpression.Value;

	private object EvaluateBinaryExpression(BoundBinaryExpression binaryExpression)
	{
		var left = EvaluateExpression(binaryExpression.Left);
		var right = EvaluateExpression(binaryExpression.Right);

		return binaryExpression.BinaryOperator.Kind switch
		{
			BoundBinaryOperatorKind.Addition => (int)left + (int)right,
			BoundBinaryOperatorKind.Subtraction => (int)left - (int)right,
			BoundBinaryOperatorKind.Multiplication => (int)left * (int)right,
			BoundBinaryOperatorKind.Division => (int)left / (int)right,
			BoundBinaryOperatorKind.LogicalAnd => (bool)left && (bool)right,
			BoundBinaryOperatorKind.LogicalOr => (bool)left || (bool)right,
			BoundBinaryOperatorKind.LogicalEquality => Equals(left, right),
			BoundBinaryOperatorKind.LogicalInequality => !Equals(left, right),
			_ => -1
		};
	}

	private object EvaluateUnaryExpression(BoundUnaryExpression unaryExpression)
	{
		return unaryExpression.UnaryOperator.Kind switch
		{
			BoundUnaryOperatorKind.Negation => -(int)EvaluateExpression(unaryExpression.Operand),
			BoundUnaryOperatorKind.Identity => (int)EvaluateExpression(unaryExpression.Operand),
			BoundUnaryOperatorKind.LogicalNegation => !(bool)EvaluateExpression(unaryExpression.Operand),
			_ => -1
		};
	}
}