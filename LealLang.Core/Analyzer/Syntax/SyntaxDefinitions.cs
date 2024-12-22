namespace LealLang.Core.Analyzer.Syntax;

public static class SyntaxDefinitions 
{
	public static int GetUnaryPrecedence(this SyntaxKind kind) => kind switch 
	{
		SyntaxKind.NotToken or
		SyntaxKind.MinusToken or
		SyntaxKind.PlusToken => 5,
		
		_ => 0,	
	};
	
	public static int GetBinaryPrecedence(this SyntaxKind kind) => kind switch 
	{
		SyntaxKind.SlashToken or
		SyntaxKind.StarToken => 4,
		
		SyntaxKind.MinusToken or
		SyntaxKind.PlusToken => 3,
		
		SyntaxKind.EqualsEqualsToken => 2,
		SyntaxKind.NotEqualsToken => 2,
		 
		SyntaxKind.AmpersandAmpersandToken or 
		SyntaxKind.PipePipeToken => 1,
		
		_ => 0,
	};
	
	public static SyntaxKind GetKeywordKind(this string text) => text switch 
	{
		"true" => SyntaxKind.TrueKeyword,
		"false" => SyntaxKind.FalseKeyword,
		_ => SyntaxKind.IdentifierToken	
	};
	
	public static int GetAdvanceQuantity(this SyntaxKind kind)
	{
		return kind switch
		{
			SyntaxKind.EqualsEqualsToken or
			SyntaxKind.NotEqualsToken or
			SyntaxKind.AmpersandAmpersandToken or
			SyntaxKind.PipePipeToken => 2,
			_ => 1
		};
	}
}