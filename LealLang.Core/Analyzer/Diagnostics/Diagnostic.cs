using LealLang.Core.Analyzer.Text;

namespace LealLang.Core.Analyzer.Diagnostics;

public sealed class Diagnostic
{
	public Diagnostic(DiagnosticType diagnosticType, TextSpan span, string message) 
	{
        DiagnosticType = diagnosticType;
        Span = span;
		Message = message;
	}

    public DiagnosticType DiagnosticType { get; }
    public TextSpan Span { get; }
	public string Message { get; }

	public override string ToString() => Message;
}
