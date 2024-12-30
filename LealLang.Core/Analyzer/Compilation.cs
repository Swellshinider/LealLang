using LealLang.Core.Analyzer.Binding;
using LealLang.Core.Analyzer.Syntax;
using LealLang.Core.Analyzer.Text;

namespace LealLang.Core.Analyzer;

public sealed class Compilation
{
	private BoundGlobalScope? _globalScope;

	public Compilation(SyntaxTree syntaxTree) : this(null, syntaxTree) { }
	
	private Compilation(Compilation? previous, SyntaxTree syntaxTree)
	{
        Previous = previous;
        SyntaxTree = syntaxTree;
	}

    public Compilation? Previous { get; }
    public SyntaxTree SyntaxTree { get; }

	internal BoundGlobalScope GlobalScope 
	{
		get 
		{
			if (_globalScope == null) 
			{
				var globalScope = Binder.BindGlobalScope(Previous?.GlobalScope, SyntaxTree.Root);
				Interlocked.CompareExchange(ref _globalScope, globalScope, null);
			}
			
			return _globalScope;
		}
	}
	
	public Compilation ContinueWith(SyntaxTree syntaxTree) => new(this, syntaxTree);

	public EvaluationResult Evaluate(Dictionary<VariableSymbol, object?> variables)
	{
		if (SyntaxTree.Diagnostics.Any())
			return new([.. SyntaxTree.Diagnostics], null);
		var diagnostics = SyntaxTree.Diagnostics.Concat(GlobalScope.Diagnostics);

		if (diagnostics.Any())
			return new([.. diagnostics], null);

		var evaluator = new Evaluator(GlobalScope.Expression, variables);
		var value = evaluator.Evaluate();

		return new([], value);
	}
}