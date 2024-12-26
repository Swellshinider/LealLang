using System.Collections.Immutable;
using LealLang.Core.Analyzer.Diagnostics;

namespace LealLang.Core.Analyzer;

public sealed class EvaluationResult
{
	public EvaluationResult(ImmutableArray<Diagnostic> diagnostics, object? value) 
	{
        Diagnostics = diagnostics;
        Value = value;
    }

    public ImmutableArray<Diagnostic> Diagnostics { get; }
    public object? Value { get; }
}