namespace LealLang.Core.Analyzer.Syntax;

public enum SyntaxKind
{
	#region [ Tokens ]
	LiteralToken,
	EndOfFileToken,
	WhitespaceToken,
	IdentifierToken,
	BadToken,
	PlusToken,
	MinusToken,
	StarToken,
	SlashToken,
	OpenParenthesisToken,
	CloseParenthesisToken,
	#endregion
	
	#region [ Keywords ]
    TrueKeyword,
    FalseKeyword,
	#endregion
	
	#region [ Expressions ]
	LiteralExpression,
	BinaryExpression,
	UnaryExpression,
	ParenthesizedExpression,
	#endregion
}