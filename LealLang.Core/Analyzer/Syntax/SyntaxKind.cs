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
	EqualsToken,
	EqualsEqualsToken,
	NotToken,
	NotEqualsToken,
	PipeToken,
	PipePipeToken,
	AmpersandToken,
	AmpersandAmpersandToken,
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