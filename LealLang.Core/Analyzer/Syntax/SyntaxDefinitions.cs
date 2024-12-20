namespace LealLang.Core.Analyzer.Syntax;

public static class SyntaxDefinitions 
{
	public static int GetUnaryPrecedence(this SyntaxKind kind) => kind switch 
	{
		SyntaxKind.MinusToken or
		SyntaxKind.PlusToken => 4,
		
		_ => 0,	
	};
	
	public static int GetBinaryPrecedence(this SyntaxKind kind) => kind switch 
	{
		SyntaxKind.OpenParenthesisToken => 3,
		
		SyntaxKind.SlashToken => 2,
		SyntaxKind.StarToken => 2,
		
		SyntaxKind.MinusToken or
		SyntaxKind.PlusToken => 1,
		
		_ => 0,
	};
	
	public static SyntaxKind GetKeywordKind(this string text) => text switch 
	{
		"true" => SyntaxKind.TrueKeyword,
		"false" => SyntaxKind.FalseKeyword,
		_ => SyntaxKind.IdentifierToken	
	};
}