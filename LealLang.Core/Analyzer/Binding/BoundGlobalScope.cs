using System.Collections.Immutable;
using LealLang.Core.Analyzer.Binding.Expressions;
using LealLang.Core.Analyzer.Diagnostics;
using LealLang.Core.Analyzer.Text;

namespace LealLang.Core.Analyzer.Binding;

internal sealed class BoundGlobalScope
{
	public BoundGlobalScope(BoundGlobalScope? previousScope, DiagnosticManager diagnostics, ImmutableArray<VariableSymbol> variables, BoundExpression expression)
	{
        PreviousScope = previousScope;
        Diagnostics = diagnostics;
        Variables = variables;
        Expression = expression;
    }

    public BoundGlobalScope? PreviousScope { get; }
    public DiagnosticManager Diagnostics { get; }
    public ImmutableArray<VariableSymbol> Variables { get; }
    public BoundExpression Expression { get; }
}
