using LealLang.Core.Analyzer.Diagnostics;

namespace LealLang.Core.Analyzer;

public sealed class EvaluationResult
{
	public EvaluationResult(IEnumerable<Diagnostic> diagnostics, object? value) 
	{
        Diagnostics = diagnostics.ToArray();
        Value = value;
    }

    public IReadOnlyList<Diagnostic> Diagnostics { get; }
    public object? Value { get; }
}