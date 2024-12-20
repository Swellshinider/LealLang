namespace LealLang.Core.Analyzer.Binding.Expressions;

internal abstract class BoundExpression : BoundNode
{
	public abstract Type Type { get; }
}
