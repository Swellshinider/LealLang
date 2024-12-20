namespace LealLang.Core.Analyzer.Syntax;

public enum SyntaxKind
{
	#region [ Tokens ]
	NumberToken,
	EndOfFileToken,
	WhitespaceToken,
	BadToken,
	PlusToken,
	MinusToken,
	StarToken,
	SlashToken,
	OpenParenthesisToken,
	CloseParenthesisToken,
	#endregion
	
	#region [ Expressions ]
	LiteralExpression,
	BinaryExpression,
	UnaryExpression,
	ParenthesizedExpression,
	#endregion
}