using LealLang.Core.Analyzer.Syntax;

namespace LealLang.Tests.Analyzer.Syntax;

public static class LexerData
{
	internal static IEnumerable<(SyntaxKind kind, string text)> Tokens()
	{
		var lexicalTokens = Enum.GetValues<SyntaxKind>()
						 .Select(k => (kind: k, text: k.GetText()))
						 .Where(t => t.text != null)
						 .Select(t => (t.kind, t.text!));

		var dynamicTokens = new[]
		{
			(SyntaxKind.LiteralToken, "123"),
			(SyntaxKind.LiteralToken, "12345"),
			(SyntaxKind.IdentifierToken, "a"),
			(SyntaxKind.IdentifierToken, "abc")
		};
		
		return lexicalTokens.Concat(dynamicTokens);
	}

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