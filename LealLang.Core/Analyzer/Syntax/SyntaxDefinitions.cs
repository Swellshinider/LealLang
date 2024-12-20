namespace LealLang.Core.Analyzer.Syntax;

public static class SyntaxDefinitions 
{
	public static int GetAdvanceQuantity(this SyntaxKind kind)
	{
		return kind switch
		{
			SyntaxKind.EqualsEqualsToken or
			SyntaxKind.ExclamationEqualsToken or
			SyntaxKind.AmpersandAmpersandToken or
			SyntaxKind.PipePipeToken => 2,
			_ => 1
		};
	}
	
	public static int GetUnaryPrecedence(this SyntaxKind kind) => kind switch 
	{
		SyntaxKind.MinusToken or
		SyntaxKind.PlusToken or
		SyntaxKind.ExclamationToken => 4,
		
		_ => 0,	
	};
	
	public static int GetBinaryPrecedence(this SyntaxKind kind) => kind switch 
	{
		SyntaxKind.OpenParenthesisToken => 3,
		
		SyntaxKind.SlashToken or
		SyntaxKind.StarToken or 
		SyntaxKind.AmpersandAmpersandToken or 
		SyntaxKind.EqualsEqualsToken or 
		SyntaxKind.PipePipeToken => 2,
		
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