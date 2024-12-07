namespace LealLang.Core.Analyzer;

public readonly struct Token(TokenType type, string text, int position, int length, object? value)
{
	public TokenType Type { get; } = type;
	public string Text { get; } = text;
	public int Position { get; } = position;
	public int Length { get; } = length;
	public object? Value { get; } = value;
	
	public override string ToString() => $"{Type}: {Text} {(Value != null ? $"-> {Value}" : "")}";
}