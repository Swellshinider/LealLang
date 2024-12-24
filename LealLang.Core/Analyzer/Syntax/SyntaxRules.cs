namespace LealLang.Core.Analyzer.Syntax;

public static class SyntaxRules
{
	public static int GetUnaryPrecedence(this SyntaxKind kind) => kind switch
	{
		SyntaxKind.NotToken or
		SyntaxKind.MinusToken or
		SyntaxKind.PlusToken => 5,

		_ => 0,
	};

	public static int GetBinaryPrecedence(this SyntaxKind kind) => kind switch
	{
		SyntaxKind.SlashToken or
		SyntaxKind.StarToken => 4,

		SyntaxKind.MinusToken or
		SyntaxKind.PlusToken => 3,

		SyntaxKind.EqualsEqualsToken => 2,
		SyntaxKind.NotEqualsToken => 2,

		SyntaxKind.AmpersandAmpersandToken or
		SyntaxKind.PipePipeToken => 1,

		_ => 0,
	};

	public static int GetAdvanceQuantity(this SyntaxKind kind) => kind switch
	{
		SyntaxKind.EqualsEqualsToken or
		SyntaxKind.NotEqualsToken or
		SyntaxKind.AmpersandAmpersandToken or
		SyntaxKind.PipePipeToken => 2,

		_ => 1
	};

	public static SyntaxKind GetKeywordKind(this string text) => text switch
	{
		"true" => SyntaxKind.TrueKeyword,
		"false" => SyntaxKind.FalseKeyword,
		_ => SyntaxKind.IdentifierToken
	};

	public static IEnumerable<SyntaxKind> GetUnaryOperatorKinds()
	{
		var kinds = Enum.GetValues<SyntaxKind>();

		foreach (var kind in kinds)
			if (kind.GetUnaryPrecedence() > 0)
				yield return kind;
	}

	public static IEnumerable<SyntaxKind> GetBinaryOperatorKinds()
	{
		var kinds = Enum.GetValues<SyntaxKind>();

		foreach (var kind in kinds)
			if (kind.GetBinaryPrecedence() > 0)
				yield return kind;
	}

	public static string? GetText(this SyntaxKind kind) => kind switch
	{
		SyntaxKind.PlusToken => "+",
		SyntaxKind.MinusToken => "-",
		SyntaxKind.StarToken => "*",
		SyntaxKind.SlashToken => "/",
		SyntaxKind.EqualsToken => "=",
		SyntaxKind.EqualsEqualsToken => "==",
		SyntaxKind.NotToken => "!",
		SyntaxKind.NotEqualsToken => "!=",
		//SyntaxKind.PipeToken => "|",
		SyntaxKind.PipePipeToken => "||",
		//SyntaxKind.AmpersandToken => "&",
		SyntaxKind.AmpersandAmpersandToken => "&&",
		SyntaxKind.OpenParenthesisToken => "(",
		SyntaxKind.CloseParenthesisToken => ")",
		SyntaxKind.TrueKeyword => "true",
		SyntaxKind.FalseKeyword => "false",
		_ => null
	};
}