namespace LealLang.Core.Analyzer.Binding.Expressions;

internal sealed class BoundBinaryExpression : BoundExpression
{
	public BoundBinaryExpression(BoundExpression left, BoundBinaryOperator binaryOperator, BoundExpression right)
	{
		Left = left;
		BinaryOperator = binaryOperator;
		Right = right;
	}

	public BoundExpression Left { get; }
	public BoundBinaryOperator BinaryOperator { get; }
	public BoundExpression Right { get; }

	public override Type Type => BinaryOperator.ResultType;
	public override BoundNodeKind Kind => BoundNodeKind.BinaryExpression;
}