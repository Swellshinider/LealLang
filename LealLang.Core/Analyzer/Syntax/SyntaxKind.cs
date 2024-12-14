namespace LealLang.Core.Analyzer.Syntax;

public enum SyntaxKind
{
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
    NumberExpression,
    BinaryExpression,
    UnaryExpression
}