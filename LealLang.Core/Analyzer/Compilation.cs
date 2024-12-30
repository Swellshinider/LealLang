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
			
		var binder = new Binder(variables);
		var boundExpression = binder.BindExpression(SyntaxTree.Root.Expression);
		
		var diagnostics = SyntaxTree.Diagnostics.Concat(binder.Diagnostics);
		
		if (diagnostics.Any()) 
			return new([.. diagnostics], null);
		
		var evaluator = new Evaluator(boundExpression!, variables);
		var value = evaluator.Evaluate();
		
		return new([], value);
	}
}