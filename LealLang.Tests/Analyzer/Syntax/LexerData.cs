using LealLang.Core.Analyzer.Syntax;

namespace LealLang.Tests.Analyzer.Syntax;

public static class LexerData 
{
	internal static IEnumerable<(SyntaxKind kind, string text)> Tokens =>
	[
		(SyntaxKind.PlusToken, "+"),
		(SyntaxKind.MinusToken, "-"),
		(SyntaxKind.StarToken, "*"),
		(SyntaxKind.SlashToken, "/"),
		(SyntaxKind.EqualsToken, "="),
		(SyntaxKind.EqualsEqualsToken, "=="),
		(SyntaxKind.NotToken, "!"),
		(SyntaxKind.NotEqualsToken, "!="),
		(SyntaxKind.PipeToken, "|"),
		(SyntaxKind.PipePipeToken, "||"),
		(SyntaxKind.AmpersandToken, "&"),
		(SyntaxKind.AmpersandAmpersandToken, "&&"),
		(SyntaxKind.OpenParenthesisToken, "("),
		(SyntaxKind.CloseParenthesisToken, ")"),
		(SyntaxKind.LiteralToken, "123"),
		(SyntaxKind.LiteralToken, "12345"),
		(SyntaxKind.IdentifierToken, "a"),
		(SyntaxKind.IdentifierToken, "abc"),
		(SyntaxKind.TrueKeyword, "true"),
		(SyntaxKind.FalseKeyword, "false"),
	];
	
	internal static IEnumerable<(SyntaxKind kind, string text)> Separators =>
	[
		(SyntaxKind.WhitespaceToken, " "),
		(SyntaxKind.WhitespaceToken, "  "),
		(SyntaxKind.WhitespaceToken, "\n"),
		(SyntaxKind.WhitespaceToken, "\r"),
		(SyntaxKind.WhitespaceToken, "\t"),
		(SyntaxKind.WhitespaceToken, "\r\n"),
		(SyntaxKind.WhitespaceToken, "\n\r"),
	];
}