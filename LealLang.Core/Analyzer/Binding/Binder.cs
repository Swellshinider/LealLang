using LealLang.Core.Analyzer.Binding.Expressions;
using LealLang.Core.Analyzer.Syntax;
using LealLang.Core.Analyzer.Syntax.Expressions;

namespace LealLang.Core.Analyzer.Binding;

public sealed class Binder
{
	private readonly List<string> _diagnostics = [];

	public List<string> Diagnostics => _diagnostics;

	public BoundExpression? BindExpression(ExpressionSyntax syntax) => syntax.Kind switch
	{
		SyntaxKind.LiteralExpression => BindLiteralExpression((LiteralExpressionSyntax)syntax),
		SyntaxKind.BinaryExpression => BindBinaryExpression((BinaryExpressionSyntax)syntax),
		SyntaxKind.UnaryExpression => BindUnaryExpression((UnaryExpressionSyntax)syntax),
		SyntaxKind.ParenthesizedExpression => BindExpression(((ParenthesizedExpressionSyntax)syntax).Expression),
		_ => AddErrorAndReturnNull($"Unexpected syntax <{syntax.Kind}>"),
	};

	private BoundExpression? AddErrorAndReturnNull(string message)
	{
		Diagnostics.Add(message);
		return null;
	}

	private BoundExpression BindUnaryExpression(UnaryExpressionSyntax unarySyntax)
	{
		var boundOperand = BindExpression(unarySyntax.Operand)!;
		var unaryOperator = BoundUnaryOperator.Bind(unarySyntax.OperatorToken.Kind, boundOperand.Type);
		
		if (unaryOperator == null) 
		{
			Diagnostics.Add($"Cannot execute unary operation '{unarySyntax.OperatorToken.Text}' for type <{boundOperand.Type}>");
			return boundOperand;
		}

		return new BoundUnaryExpression(unaryOperator, boundOperand);
	}

	private BoundExpression BindBinaryExpression(BinaryExpressionSyntax binarySyntax)
	{
		var leftExpression = BindExpression(binarySyntax.Left)!;
		var rightExpression = BindExpression(binarySyntax.Right)!;
		var binaryOperator = BoundBinaryOperator.Bind(binarySyntax.OperatorToken.Kind, leftExpression.Type, rightExpression.Type);
			
		if (binaryOperator == null) 
		{
			Diagnostics.Add($"Cannot execute binary operation '{binarySyntax.OperatorToken.Text}' between types <{leftExpression.Type}> and <{rightExpression.Type}>");
			return leftExpression;
		}

		return new BoundBinaryExpression(leftExpression, binaryOperator, rightExpression);
	}

	private static BoundLiteralExpression BindLiteralExpression(LiteralExpressionSyntax literalSyntax)
		=> new(literalSyntax.Value);
}