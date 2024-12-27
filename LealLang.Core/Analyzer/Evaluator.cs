using LealLang.Core.Analyzer.Binding;
using LealLang.Core.Analyzer.Binding.Expressions;
using LealLang.Core.Analyzer.Text;

namespace LealLang.Core.Analyzer;

internal sealed class Evaluator
{
	private readonly BoundExpression _expression;
	private readonly Dictionary<VariableSymbol, object?> _variables;

	public Evaluator(BoundExpression expression, Dictionary<VariableSymbol, object?> variables)
	{
		_expression = expression;
		_variables = variables;
	}

	public object Evaluate() => EvaluateExpression(_expression);

	private object EvaluateExpression(BoundExpression expression) => expression.Kind switch
	{
		BoundNodeKind.LiteralExpression => EvaluateLiteralExpression((BoundLiteralExpression)expression),
		BoundNodeKind.BinaryExpression => EvaluateBinaryExpression((BoundBinaryExpression)expression),
		BoundNodeKind.UnaryExpression => EvaluateUnaryExpression((BoundUnaryExpression)expression),
		BoundNodeKind.VariableExpression => EvaluateVariableExpression((BoundVariableExpression)expression),
		BoundNodeKind.AssignmentExpression => EvaluateAssignmentExpression((BoundAssignmentExpression)expression),
		_ => throw new($"Unexpected expression to evaluate '{expression.Kind}'")
	};

	private static object EvaluateLiteralExpression(BoundLiteralExpression literalExpression)
		=> literalExpression.Value;

	private object EvaluateBinaryExpression(BoundBinaryExpression binaryExpression)
	{
		var left = EvaluateExpression(binaryExpression.Left)!;
		var right = EvaluateExpression(binaryExpression.Right)!;

		return binaryExpression.BinaryOperator.Kind switch
		{
			BoundBinaryOperatorKind.Addition => (int)left + (int)right,
			BoundBinaryOperatorKind.Subtraction => (int)left - (int)right,
			BoundBinaryOperatorKind.Multiplication => (int)left * (int)right,
			BoundBinaryOperatorKind.Division => (int)left / (int)right,
			BoundBinaryOperatorKind.LogicalEquality => Equals(left, right),
			BoundBinaryOperatorKind.LogicalInequality => !Equals(left, right),
			BoundBinaryOperatorKind.GreaterThan => (int)left > (int)right,
			BoundBinaryOperatorKind.LessThan => (int)left < (int)right,
			BoundBinaryOperatorKind.GreaterThanOrEqual => (int)left >= (int)right,
			BoundBinaryOperatorKind.LessThanOrEqual => (int)left <= (int)right,
			BoundBinaryOperatorKind.LogicalAnd => (bool)left && (bool)right,
			BoundBinaryOperatorKind.LogicalOr => (bool)left || (bool)right,
		_ => throw new($"Invalid binary operator to evaluate '{binaryExpression.BinaryOperator.Kind}'")
		};
	}

	private object EvaluateUnaryExpression(BoundUnaryExpression unaryExpression)
	{
		var value = EvaluateExpression(unaryExpression.Operand)!;
		
		return unaryExpression.UnaryOperator.Kind switch
		{
			BoundUnaryOperatorKind.Negation => -(int)value,
			BoundUnaryOperatorKind.Identity => (int)value,
			BoundUnaryOperatorKind.LogicalNegation => !(bool)value,
		_ => throw new($"Invalid unary operator to evaluate '{unaryExpression.UnaryOperator.Kind}'")
		};
	}

	private object EvaluateAssignmentExpression(BoundAssignmentExpression assignmentExpression)
	{
		var value = EvaluateExpression(assignmentExpression.Expression);
		_variables[assignmentExpression.VariableSymbol] = value;
		return value;
	}

	private object EvaluateVariableExpression(BoundVariableExpression variableExpression)
		=> _variables[variableExpression.VariableSymbol]!;
}