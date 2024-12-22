using LealLang.Core.Analyzer.Binding;
using LealLang.Core.Analyzer.Syntax;

namespace LealLang.Core.Analyzer;

public sealed class Compilation
{
	public Compilation(SyntaxTree syntaxTree)
	{
		SyntaxTree = syntaxTree;
	}

	public SyntaxTree SyntaxTree { get; }
	
	public EvaluationResult Evaluate() 
	{
		if (SyntaxTree.Diagnostics.Any()) 
			return new EvaluationResult(SyntaxTree.Diagnostics, null);
			
		var binder = new Binder();
		var boundExpression = binder.BindExpression(SyntaxTree.RootExpression);
		
		var diagnostics = SyntaxTree.Diagnostics.Concat(binder.Diagnostics);
		
		if (diagnostics.Any()) 
			return new EvaluationResult(diagnostics, null);
		
		var evaluator = new Evaluator(boundExpression!);
		var value = evaluator.Evaluate();
		
		return new([], value);
	}
}