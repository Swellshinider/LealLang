namespace LealLang.Core.Analyzer.Binding.Expressions;

internal sealed class BoundUnaryExpression : BoundExpression 
{
	public BoundUnaryExpression(BoundUnaryOperator unaryOperator, BoundExpression operand)
	{
		UnaryOperator = unaryOperator;
		Operand = operand;
	}

	public BoundUnaryOperator UnaryOperator { get; }
	public BoundExpression Operand { get; }
	
	public override Type Type => UnaryOperator.ResultType;
	public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
}
