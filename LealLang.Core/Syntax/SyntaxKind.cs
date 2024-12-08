namespace LealLang.Core.Syntax;

public enum SyntaxKind
{
	#region [ Special Tokens ]
	BadToken,
	EndOfFileToken,
	WhiteSpaceToken,
	#endregion
	
	#region [ Tokens ]
	NumberToken,
	PlusToken,
	MinusToken,
	StarToken,
	SlashToken,
	#endregion
	
	#region [ Expressions ]
	LiteralExpression,
	UnaryExpression,
	BinaryExpression,
	#endregion
}