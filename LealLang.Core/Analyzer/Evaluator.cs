using LealLang.Core.Analyzer.Binding;
using LealLang.Core.Analyzer.Binding.Expressions;
using LealLang.Core.Analyzer.Syntax;
using LealLang.Core.Analyzer.Syntax.Expressions;

namespace LealLang.Core.Analyzer;

public sealed class Evaluator
{
	private readonly BoundExpression _expression;

	public Evaluator(ExpressionSyntax syntax)
	{
		var binder = new Binder();
		_expression = binder.BindExpression(syntax);
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
		_ => throw new NotImplementedException()
	};
	
	private static int EvaluateLiteralExpression(BoundLiteralExpression literalExpression)
	{
		var value = literalExpression.Value;
		
		if (value!.GetType() != typeof(int))
			throw new Exception("Impossible");

		return (int)value!;
	}

	private int EvaluateBinaryExpression(BoundBinaryExpression binaryExpression)
	{
		var left = EvaluateExpression(binaryExpression.Left);
		var right = EvaluateExpression(binaryExpression.Right);

		return binaryExpression.OperatorKind switch
		{
			BoundBinaryOperatorKind.Addition => (int)left + (int)right,
			BoundBinaryOperatorKind.Subtraction => (int)left - (int)right,
			BoundBinaryOperatorKind.Multiplication => (int)left * (int)right,
			BoundBinaryOperatorKind.Division => (int)left / (int)right,
			_ => throw new NotImplementedException()
		};
	}

	private int EvaluateUnaryExpression(BoundUnaryExpression unaryExpression)
	{
		return unaryExpression.OperatorKind switch
		{
			BoundUnaryOperatorKind.Negation => -(int)EvaluateExpression(unaryExpression.Operand),
			BoundUnaryOperatorKind.Identity => (int)EvaluateExpression(unaryExpression.Operand),
			_ => throw new NotImplementedException()
		};
	}
}