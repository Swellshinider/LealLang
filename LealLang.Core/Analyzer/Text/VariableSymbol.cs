namespace LealLang.Core.Analyzer.Text;

public readonly struct VariableSymbol
{
	internal VariableSymbol(string name, Type type)
	{
        Name = name;
        Type = type;
    }

    public string Name { get; }
    public Type Type { get; }
}