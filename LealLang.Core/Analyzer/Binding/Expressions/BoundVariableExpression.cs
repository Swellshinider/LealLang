using LealLang.Core.Analyzer.Text;

namespace LealLang.Core.Analyzer.Binding.Expressions;

internal sealed class BoundVariableExpression : BoundExpression 
{
	public BoundVariableExpression(VariableSymbol variableSymbol) 
	{
		VariableSymbol = variableSymbol;
	}
	
	public VariableSymbol VariableSymbol { get; }

	public override Type Type => VariableSymbol.Type;
	public override BoundNodeKind Kind => BoundNodeKind.VariableExpression;
}