using LealLang.Core.Analyzer.Binding.Expressions;
using LealLang.Core.Analyzer.Diagnostics;
using LealLang.Core.Analyzer.Syntax;
using LealLang.Core.Analyzer.Syntax.Expressions;
using LealLang.Core.Analyzer.Text;

namespace LealLang.Core.Analyzer.Binding;

internal sealed class Binder
{
	private readonly DiagnosticManager _diagnostics = new();
	private readonly Dictionary<VariableSymbol, object?> _variables;

	public Binder(Dictionary<VariableSymbol, object?> variables)
	{
		_variables = variables;
	}

	public DiagnosticManager Diagnostics => _diagnostics;

	public BoundExpression BindExpression(ExpressionSyntax syntax) => syntax.Kind switch
	{
		SyntaxKind.LiteralExpression => BindLiteralExpression((LiteralExpressionSyntax)syntax),
		SyntaxKind.UnaryExpression => BindUnaryExpression((UnaryExpressionSyntax)syntax),
		SyntaxKind.BinaryExpression => BindBinaryExpression((BinaryExpressionSyntax)syntax),
		SyntaxKind.ParenthesizedExpression => BindParenthesizedExpression((ParenthesizedExpressionSyntax)syntax),
		SyntaxKind.NameExpression => BindNameExpression((NameExpressionSyntax)syntax),
		SyntaxKind.AssignmentExpression => BindAssignmentExpression((AssignmentExpressionSyntax)syntax),
		_ => throw new($"Unexpected syntax expression <{syntax.Kind}>"),
	};

	private static BoundLiteralExpression BindLiteralExpression(LiteralExpressionSyntax literalSyntax)
		=> new(literalSyntax.Value);

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

	private BoundExpression BindParenthesizedExpression(ParenthesizedExpressionSyntax parenthesizedSyntax)
		=> BindExpression(parenthesizedSyntax.Expression);

	private BoundExpression BindNameExpression(NameExpressionSyntax nameSyntax)
	{
		var name = nameSyntax.IdentifierToken.Text ?? "";
		var variable = _variables.Keys.FirstOrDefault(v => v.Name == name);

		if (variable.Equals(default(VariableSymbol)))
		{
			_diagnostics.ReportUndefinedName(nameSyntax.IdentifierToken.Span, name);
			return new BoundLiteralExpression(0);
		}

		return new BoundVariableExpression(variable);
	}

	private BoundAssignmentExpression BindAssignmentExpression(AssignmentExpressionSyntax assignmentSyntax)
	{
		var name = assignmentSyntax.IdentifierToken.Text ?? "";
		var boundExpression = BindExpression(assignmentSyntax.Expression);
		
		var existVariable = _variables.Keys.FirstOrDefault(v => v.Name == name);
		
		if (!existVariable.Equals(default(VariableSymbol)))
			_variables.Remove(existVariable);
			
		var variable = new VariableSymbol(name, boundExpression.Type);
		_variables.Add(variable, null);
		
		return new BoundAssignmentExpression(variable, boundExpression);
	}
}