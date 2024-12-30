using System.Collections.Immutable;
using LealLang.Core.Analyzer.Text;

namespace LealLang.Core.Analyzer.Binding;

internal sealed class BoundScope
{
	private Dictionary<string, VariableSymbol> _variables = [];

	public BoundScope(BoundScope? parent)
	{
		Parent = parent;
	}
	
	public BoundScope? Parent { get; }
	
	public ImmutableArray<VariableSymbol> DeclaredVariables => [.. _variables.Values];
	
	public bool TryLookup(string name, out VariableSymbol variableSymbol) 
	{
		if (!_variables.TryGetValue(name, out variableSymbol))
			return false;
			
		if (Parent == null)
			return false;
		
		return Parent.TryLookup(name, out variableSymbol);
	}
	
	public bool TryDeclare(VariableSymbol variableSymbol) 
	{
		if (_variables.ContainsKey(variableSymbol.Name))
			return false;
		
		_variables.Add(variableSymbol.Name, variableSymbol);
		return true;
	}
}
