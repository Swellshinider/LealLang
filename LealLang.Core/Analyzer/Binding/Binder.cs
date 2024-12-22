using LealLang.Core.Analyzer.Binding.Expressions;
using LealLang.Core.Analyzer.Diagnostics;
using LealLang.Core.Analyzer.Syntax;
using LealLang.Core.Analyzer.Syntax.Expressions;

namespace LealLang.Core.Analyzer.Binding;

internal sealed class Binder
{
	private readonly DiagnosticManager _diagnostics = new();

	public DiagnosticManager Diagnostics => _diagnostics;

	public BoundExpression? BindExpression(ExpressionSyntax syntax) => syntax.Kind switch
	{
		SyntaxKind.LiteralExpression => BindLiteralExpression((LiteralExpressionSyntax)syntax),
		SyntaxKind.BinaryExpression => BindBinaryExpression((BinaryExpressionSyntax)syntax),
		SyntaxKind.UnaryExpression => BindUnaryExpression((UnaryExpressionSyntax)syntax),
		SyntaxKind.ParenthesizedExpression => BindExpression(((ParenthesizedExpressionSyntax)syntax).Expression),
		_ => throw new($"Unexpected syntax expression <{syntax.Kind}>"),
	};

	private BoundExpression BindUnaryExpression(UnaryExpressionSyntax unarySyntax)
	{
		var boundOperand = BindExpression(unarySyntax.Operand)!;
		var unaryOperator = BoundUnaryOperator.Bind(unarySyntax.OperatorToken.Kind, boundOperand.Type);
		
		if (unaryOperator == null) 
		{
			_diagnostics.ReportInvalidUnaryOperator(unarySyntax.OperatorToken.Span, unarySyntax.OperatorToken.Text, boundOperand.Type);
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
			_diagnostics.ReportInvalidBinaryOperator(binarySyntax.OperatorToken.Span, binarySyntax.OperatorToken.Text, leftExpression.Type, rightExpression.Type);
			return leftExpression;
		}

		return new BoundBinaryExpression(leftExpression, binaryOperator, rightExpression);
	}

	private static BoundLiteralExpression BindLiteralExpression(LiteralExpressionSyntax literalSyntax)
		=> new(literalSyntax.Value);
}