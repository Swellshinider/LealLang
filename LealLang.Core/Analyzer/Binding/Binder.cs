using LealLang.Core.Analyzer.Binding.Expressions;
using LealLang.Core.Analyzer.Syntax;
using LealLang.Core.Analyzer.Syntax.Expressions;

namespace LealLang.Core.Analyzer.Binding;

internal sealed class Binder
{
	private readonly List<string> _diagnostics = [];
	
	public List<string> Diagnostics => _diagnostics;
	
	public BoundExpression BindExpression(ExpressionSyntax syntax) => syntax.Kind switch
	{
		SyntaxKind.LiteralExpression => BindLiteralExpression((LiteralExpressionSyntax)syntax),
		SyntaxKind.BinaryExpression => BindBinaryExpression((BinaryExpressionSyntax)syntax),
		SyntaxKind.UnaryExpression => BindUnaryExpression((UnaryExpressionSyntax)syntax),
		_ => throw new($"Unexpected syntax <{syntax.Kind}>"),
	};

	private BoundUnaryExpression BindUnaryExpression(UnaryExpressionSyntax unarySyntax)
	{
		var boundOperand = BindExpression(unarySyntax.Operand);
		var boundKind = BindUnaryOperatorKind(unarySyntax.OperatorToken.Kind, boundOperand.Type)
			?? throw new($"Cannot execute unary operationr <{unarySyntax.OperatorToken.Kind}> for type <{boundOperand.Type}>");

		return new BoundUnaryExpression(boundKind, boundOperand);
	}

	private BoundBinaryExpression BindBinaryExpression(BinaryExpressionSyntax binarySyntax)
	{
		var leftExpression = BindExpression(binarySyntax.Left);
		var rightExpression = BindExpression(binarySyntax.Right);
		var binaryOperator = BindBinaryOperatorKind(binarySyntax.OperatorToken.Kind, leftExpression.Type, rightExpression.Type) 
			?? throw new($"Cannot execute binary operation <{binarySyntax.OperatorToken.Kind}> between <{leftExpression.Type}> and <{rightExpression.Type}>");

		return new BoundBinaryExpression(leftExpression, binaryOperator, rightExpression);
	}

	private static BoundLiteralExpression BindLiteralExpression(LiteralExpressionSyntax literalSyntax)
	{
		var value = literalSyntax.LiteralToken.Value as int? ?? 0;
		return new BoundLiteralExpression(value);
	}

	private static BoundUnaryOperatorKind? BindUnaryOperatorKind(SyntaxKind kind, Type operantType)
	{
		if (operantType != typeof(int))
			return null;

		return kind switch
		{
			SyntaxKind.PlusToken => BoundUnaryOperatorKind.Identity,
			SyntaxKind.MinusToken => BoundUnaryOperatorKind.Negation,
			_ => throw new("Invalid unary operator <{binarySyntax.OperatorToken.Kind}>")
		};
	}

	private static BoundBinaryOperatorKind? BindBinaryOperatorKind(SyntaxKind kind, Type leftType, Type rightType)
	{
		if (leftType != typeof(int) || rightType != typeof(int))
			return null;

		return kind switch
		{
			SyntaxKind.PlusToken => BoundBinaryOperatorKind.Addition,
			SyntaxKind.MinusToken => BoundBinaryOperatorKind.Subtraction,
			SyntaxKind.StarToken => BoundBinaryOperatorKind.Multiplication,
			SyntaxKind.SlashToken => BoundBinaryOperatorKind.Division,
			_ => throw new("Invalid binary operator <{binarySyntax.OperatorToken.Kind}>")
		};
	}
}