using LealLang.Core.Analyzer.Binding.Expressions;
using LealLang.Core.Analyzer.Syntax;
using LealLang.Core.Analyzer.Syntax.Expressions;

namespace LealLang.Core.Analyzer.Binding;

public sealed class Binder
{
	private readonly List<string> _diagnostics = new();

	public List<string> Diagnostics => _diagnostics;

	public BoundExpression? BindExpression(ExpressionSyntax syntax) => syntax.Kind switch
	{
		SyntaxKind.LiteralExpression => BindLiteralExpression((LiteralExpressionSyntax)syntax),
		SyntaxKind.BinaryExpression => BindBinaryExpression((BinaryExpressionSyntax)syntax),
		SyntaxKind.UnaryExpression => BindUnaryExpression((UnaryExpressionSyntax)syntax),
		SyntaxKind.ParenthesizedExpression => BindExpression(((ParenthesizedExpressionSyntax)syntax).Expression),
		_ => AddErrorAndReturnDefault<BoundExpression>($"Unexpected syntax <{syntax.Kind}>"),
	};

	private BoundUnaryExpression BindUnaryExpression(UnaryExpressionSyntax unarySyntax)
	{
		var boundOperand = BindExpression(unarySyntax.Operand)!;
		var boundKind = BindUnaryOperatorKind(unarySyntax.OperatorToken.Kind, boundOperand.Type)
			?? AddErrorAndReturnDefault<BoundUnaryOperatorKind>($"Cannot execute unary operation <{unarySyntax.OperatorToken.Kind}> for type <{boundOperand.Type}>");

		return new BoundUnaryExpression(boundKind, boundOperand);
	}

	private BoundBinaryExpression BindBinaryExpression(BinaryExpressionSyntax binarySyntax)
	{
		var leftExpression = BindExpression(binarySyntax.Left)!;
		var rightExpression = BindExpression(binarySyntax.Right)!;
		var binaryOperator = BindBinaryOperatorKind(binarySyntax.OperatorToken.Kind, leftExpression.Type, rightExpression.Type)
			?? AddErrorAndReturnDefault<BoundBinaryOperatorKind>($"Cannot execute binary operation <{binarySyntax.OperatorToken.Kind}> between <{leftExpression.Type}> and <{rightExpression.Type}>");

		return new BoundBinaryExpression(leftExpression, binaryOperator, rightExpression);
	}

	private static BoundLiteralExpression BindLiteralExpression(LiteralExpressionSyntax literalSyntax)
		=> new(literalSyntax.Value);

	private BoundUnaryOperatorKind? BindUnaryOperatorKind(SyntaxKind kind, Type operantType)
	{
		if (operantType == typeof(int))
			return kind switch
			{
				SyntaxKind.PlusToken => BoundUnaryOperatorKind.Identity,
				SyntaxKind.MinusToken => BoundUnaryOperatorKind.Negation,
				_ => AddErrorAndReturnDefault<BoundUnaryOperatorKind>($"Invalid unary operator <{kind}>")
			};
		else
		{
			return kind switch
			{
				SyntaxKind.ExclamationToken => BoundUnaryOperatorKind.LogicalNegation,
				_ => AddErrorAndReturnDefault<BoundUnaryOperatorKind>($"Invalid unary operator <{kind}>")
			};
		}
	}

	private BoundBinaryOperatorKind? BindBinaryOperatorKind(SyntaxKind kind, Type leftType, Type rightType)
	{
		if (leftType == typeof(int) && rightType == typeof(int))
		{
			return kind switch
			{
				SyntaxKind.PlusToken => BoundBinaryOperatorKind.Addition,
				SyntaxKind.MinusToken => BoundBinaryOperatorKind.Subtraction,
				SyntaxKind.StarToken => BoundBinaryOperatorKind.Multiplication,
				SyntaxKind.SlashToken => BoundBinaryOperatorKind.Division,
				_ => AddErrorAndReturnDefault<BoundBinaryOperatorKind>($"Invalid binary operator <{kind}>")
			};
		}
		else if (leftType == typeof(bool) && rightType == typeof(bool)) 
		{
			return kind switch 
			{
				SyntaxKind.AmpersandAmpersandToken => BoundBinaryOperatorKind.LogicalAnd,
				SyntaxKind.PipePipeToken => BoundBinaryOperatorKind.LogicalOr,
				_ => AddErrorAndReturnDefault<BoundBinaryOperatorKind>($"Invalid binary operator <{kind}>")
			};
		}
		
		return AddErrorAndReturnDefault<BoundBinaryOperatorKind>($"Unable to cast object of type '{leftType}' to type '<{rightType}>'");
	}

	private T? AddErrorAndReturnDefault<T>(string message)
	{
		_diagnostics.Add(message);
		return default;
	}
}