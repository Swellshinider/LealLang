using LealLang.Core.Analyzer.Text;

namespace LealLang.Core.Analyzer.Binding.Expressions;

internal sealed class BoundAssignmentExpression : BoundExpression 
{
    public BoundAssignmentExpression(VariableSymbol variableSymbol, BoundExpression expression)
    {
        VariableSymbol = variableSymbol;
        Expression = expression;
    }

    public VariableSymbol VariableSymbol { get; }
    public BoundExpression Expression { get; }

    public override Type Type => VariableSymbol.Type;
    public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;
} 