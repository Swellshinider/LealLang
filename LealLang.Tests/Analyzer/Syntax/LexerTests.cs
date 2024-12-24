using LealLang.Core.Analyzer.Syntax;

namespace LealLang.Tests.Analyzer.Syntax;

public class LexerTests
{
	[Theory]
	[MemberData(nameof(GetTokensData))]
	public void Lexer_TokenizeOneToken_ReturnsExpectedTokens(string text, SyntaxKind expectedKind, string expectedText)
	{
		var tokens = SyntaxTree.ParseTokens(text);

		var token = Assert.Single(tokens);
		Assert.Equal(expectedKind, token.Kind);
		Assert.Equal(expectedText, token.Text);
	}

	[Theory]
	[MemberData(nameof(GetTokenPairsData))]
	public void Lexer_TokenizeTwoTokens_ReturnsExpectedTokens(SyntaxKind kind1, string text1, SyntaxKind kind2, string text2)
	{
		var text = text1 + text2;
		var tokens = SyntaxTree.ParseTokens(text).ToArray();

		Assert.Equal(2, tokens.Length);
		Assert.Equal(kind1, tokens[0].Kind);
		Assert.Equal(text1, tokens[0].Text);
		Assert.Equal(kind2, tokens[1].Kind);
		Assert.Equal(text2, tokens[1].Text);
	}

	[Theory]
	[MemberData(nameof(GetTokenPairsWithSeparatorData))]
	public void Lexer_TokenizeTwoTokensWithSeparators_ReturnsExpectedTokens(SyntaxKind kind1, string text1, SyntaxKind separatorKind, string separatorText, SyntaxKind kind2, string text2)
	{
		var text = text1 + separatorText + text2;
		var tokens = SyntaxTree.ParseTokens(text).ToArray();

		Assert.Equal(3, tokens.Length);
		Assert.Equal(kind1, tokens[0].Kind);
		Assert.Equal(text1, tokens[0].Text);
		Assert.Equal(separatorKind, tokens[1].Kind);
		Assert.Equal(separatorText, tokens[1].Text);
		Assert.Equal(kind2, tokens[2].Kind);
		Assert.Equal(text2, tokens[2].Text);
	}

	public static TheoryData<string, SyntaxKind, string> GetTokensData()
	{
		var data = new TheoryData<string, SyntaxKind, string>();

		foreach (var (kind, text) in LexerData.Tokens.Concat(LexerData.Separators))
			data.Add(text, kind, text);

		return data;
	}

	public static TheoryData<SyntaxKind, string, SyntaxKind, string> GetTokenPairsData()
	{
		var data = new TheoryData<SyntaxKind, string, SyntaxKind, string>();

		foreach (var (kind1, text1, kind2, text2) in GetTokenPairs())
			data.Add(kind1, text1, kind2, text2);

		return data;
	}

	public static TheoryData<SyntaxKind, string, SyntaxKind, string, SyntaxKind, string> GetTokenPairsWithSeparatorData()
	{
		var data = new TheoryData<SyntaxKind, string, SyntaxKind, string, SyntaxKind, string>();

		foreach (var (kind1, text1, separatorKind, separatorText, kind2, text2) in GetTokenPairsWithSeparators())
			data.Add(kind1, text1, separatorKind, separatorText, kind2, text2);

		return data;
	}

	private static bool RequireSeparator(SyntaxKind t1Kind, SyntaxKind t2Kind)
	{
		if (t1Kind.ToString().EndsWith("Keyword") && t2Kind.ToString().EndsWith("Keyword"))
			return true;
		
		var incompatiblePairs = new HashSet<(SyntaxKind, SyntaxKind)>
		{
			(SyntaxKind.NotToken, SyntaxKind.EqualsToken),
			(SyntaxKind.NotToken, SyntaxKind.EqualsEqualsToken),
			(SyntaxKind.EqualsToken, SyntaxKind.EqualsToken),
			(SyntaxKind.EqualsToken, SyntaxKind.EqualsEqualsToken),
			(SyntaxKind.PipeToken, SyntaxKind.PipeToken),
			(SyntaxKind.PipeToken, SyntaxKind.PipePipeToken),
			(SyntaxKind.AmpersandToken, SyntaxKind.AmpersandToken),
			(SyntaxKind.AmpersandToken, SyntaxKind.AmpersandAmpersandToken),
			(SyntaxKind.LiteralToken, SyntaxKind.LiteralToken),
			(SyntaxKind.IdentifierToken, SyntaxKind.LiteralToken),
			(SyntaxKind.IdentifierToken, SyntaxKind.IdentifierToken),
		};
		
		foreach (var kind in Enum.GetValues<SyntaxKind>())
		{
			if (kind.ToString().EndsWith("Keyword"))
			{
				incompatiblePairs.Add((kind, SyntaxKind.IdentifierToken));
				incompatiblePairs.Add((kind, SyntaxKind.LiteralToken));
				incompatiblePairs.Add((SyntaxKind.IdentifierToken, kind));
			}
		}
		
		return incompatiblePairs.Contains((t1Kind, t2Kind));
	}

	private static IEnumerable<(SyntaxKind kind1, string text1, SyntaxKind kind2, string text2)> GetTokenPairs()
	{
		return from t1 in LexerData.Tokens
			   from t2 in LexerData.Tokens
			   where !RequireSeparator(t1.kind, t2.kind)
			   select (t1.kind, t1.text, t2.kind, t2.text);
	}

	private static IEnumerable<(SyntaxKind kind1, string text1, SyntaxKind separatorKind, string separatorText, SyntaxKind kind2, string text2)> GetTokenPairsWithSeparators()
	{
		return from t1 in LexerData.Tokens
			   from t2 in LexerData.Tokens
			   from separators in LexerData.Separators
			   where RequireSeparator(t1.kind, t2.kind)
			   select (t1.kind, t1.text, separators.kind, separators.text, t2.kind, t2.text);
	}
}