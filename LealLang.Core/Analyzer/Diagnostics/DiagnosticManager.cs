using System.Collections;
using LealLang.Core.Analyzer.Syntax;
using LealLang.Core.Analyzer.Text;

namespace LealLang.Core.Analyzer.Diagnostics;

public sealed class DiagnosticManager : IEnumerable<Diagnostic>
{
	private readonly List<Diagnostic> _diagnostics = [];
	
	public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
	internal void AddRange(DiagnosticManager diagnostics) 
		=> _diagnostics.AddRange(diagnostics._diagnostics);
	
	private void Report(DiagnosticType diagnosticType, TextSpan span, string message)
		=> _diagnostics.Add(new(diagnosticType, span, message));
	
	internal void ReportInvalidType(int start, int position, string text, Type type)
	{
		var span = new TextSpan(start, start - position);
		var message = $"Cannot implicitly convert '{text}' to type <{type}>";
		Report(DiagnosticType.TypeError, span, message);
	}
	
	internal void ReportBadToken(int start, int position, string text) 
	{
		var span = new TextSpan(start, start - position);
		var message = $"Invalid token '{text}'";
		Report(DiagnosticType.BadToken, span, message);
	}

	internal void ReportTokenDidNotMatched(TextSpan span, SyntaxKind kind, SyntaxKind expectedKind)
	{
		var message = $"Unexpected token <{kind}>, expected <{expectedKind}>";
		Report(DiagnosticType.SyntaxError, span, message);
	}

	internal void ReportInvalidUnaryOperator(TextSpan span, string? operatorText, Type targetType)
	{
		var message = $"Unary operator '{operatorText}' is not defined for type <{targetType}>";
		Report(DiagnosticType.InvalidOperation, span, message);
	}

	internal void ReportInvalidBinaryOperator(TextSpan span, string? operatorText, Type leftType, Type rightType)
	{
		var message = $"Binary operator '{operatorText}' is not defined for types <{leftType}> and <{rightType}>";
		Report(DiagnosticType.InvalidOperation, span, message);
	}

	internal void ReportUndefinedName(TextSpan span, string name)
	{
		var message = $"Variable '{name}' does not exist in the current context";
		Report(DiagnosticType.NameDoesNotExist, span, message);
	}
}