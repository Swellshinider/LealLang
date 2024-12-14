namespace LealLang.Core.Analyzer.Syntax;

public static class SyntaxDefinitions 
{
	public static int GetUnaryPrecedence(this SyntaxKind kind) => kind switch 
	{
		SyntaxKind.MinusToken or
		SyntaxKind.PlusToken => 3,
		
		_ => 0,	
	};
	
	public static int GetBinaryPrecedence(this SyntaxKind kind) => kind switch 
	{
		SyntaxKind.MinusToken or
		SyntaxKind.PlusToken => 1,
		
		SyntaxKind.SlashToken => 2,
		SyntaxKind.StarToken => 2,
		
		_ => 0,
	};
}