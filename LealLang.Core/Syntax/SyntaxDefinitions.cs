namespace LealLang.Core.Syntax;

public static class SyntaxDefinitions
{
	public static int GetUnaryPrecedence(this SyntaxKind kind) => kind switch
	{
		// Unary operators have the highest precedence
		SyntaxKind.PlusToken or
		SyntaxKind.MinusToken => 3,
		_ => 0
	};

	public static int GetBinaryPrecedence(this SyntaxKind kind) => kind switch
	{
		// Addition and subtraction have the lowest precedence
		SyntaxKind.PlusToken => 1,
		SyntaxKind.MinusToken => 1,
		
		// Multiplication and division have higher precedence than addition and subtraction
		SyntaxKind.StarToken or
		SyntaxKind.SlashToken => 2,
		_ => 0
	};
}