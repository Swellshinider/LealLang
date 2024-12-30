using LealLang.Core.Analyzer.Binding;
using LealLang.Core.Analyzer.Syntax;
using LealLang.Core.Analyzer.Text;

namespace LealLang.Core.Analyzer;

public sealed class Compilation
{
	public Compilation(SyntaxTree syntaxTree)
	{
		SyntaxTree = syntaxTree;
	}

	public SyntaxTree SyntaxTree { get; }
	
	public EvaluationResult Evaluate(Dictionary<VariableSymbol, object?> variables) 
	{
		if (SyntaxTree.Diagnostics.Any()) 
			return new([.. SyntaxTree.Diagnostics], null);
			
		var boundGlobalScope = Binder.BindGlobalScope(SyntaxTree.Root);
		var diagnostics = SyntaxTree.Diagnostics.Concat(boundGlobalScope.Diagnostics);
		
		if (diagnostics.Any()) 
			return new([.. diagnostics], null);
		
		var evaluator = new Evaluator(boundGlobalScope.Expression, variables);
		var value = evaluator.Evaluate();
		
		return new([], value);
	}
}