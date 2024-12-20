namespace LealLang.Core.Analyzer.Binding.Expressions;

public abstract class BoundExpression : BoundNode
{
	public abstract Type Type { get; }
}
