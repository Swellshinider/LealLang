using LealLang.Core.Analyzer.Binding.Expressions;
using LealLang.Core.Analyzer.Diagnostics;
using LealLang.Core.Analyzer.Syntax;
using LealLang.Core.Analyzer.Syntax.Expressions;
using LealLang.Core.Analyzer.Text;

namespace LealLang.Core.Analyzer.Binding;

internal sealed class Binder
{
	private readonly DiagnosticManager _diagnostics = new();
	private readonly BoundScope _scope;

	public Binder(BoundScope? scope = null)
	{
		_scope = new BoundScope(scope);
	}

	public DiagnosticManager Diagnostics => _diagnostics;

	public static BoundGlobalScope BindGlobalScope(BoundGlobalScope? previous, CompilationUnitSyntax compilationUnit) 
	{
		var parentScope = CreateParentScopes(previous);
		var binder = new Binder(parentScope);
		var expression = binder.BindExpression(compilationUnit.Expression);
		var diagnostics = binder._diagnostics;
		var variables = binder._scope?.DeclaredVariables ?? [];
		
		return new(previous, diagnostics, variables, expression);
	}

	public static BoundScope? CreateParentScopes(BoundGlobalScope? previous) 
	{
		var stack = new Stack<BoundGlobalScope>();
		
		while (previous != null) 
		{
			stack.Push(previous);
			previous = previous.PreviousScope;
		}
		
		BoundScope? parent = null;
		
		while (stack.Count > 0) 
		{
			previous = stack.Pop();		
			var scope = new BoundScope(parent);
			
			foreach (var variable in previous.Variables)
				scope.TryDeclare(variable);
			
			parent = scope;
		}
			
		return parent;
	}

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

		if (!_scope.TryLookup(name, out var variable))
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
		var variable = new VariableSymbol(name, boundExpression.Type);
		
		if(!_scope.TryDeclare(variable))
		{
			_diagnostics.ReportVariableAlreadyDeclared(assignmentSyntax.IdentifierToken.Span, name);
		}
		
		return new BoundAssignmentExpression(variable, boundExpression);
	}
}