using LealLang.Core.Analyzer.Syntax;

namespace LealLang.Tests.Analyzer.Syntax;

public class SyntaxRulesTests
{
	[Theory]
	[MemberData(nameof(GetSyntaxKindData))]
	public void SyntaxRules_GetText(SyntaxKind kind) 
	{
		var text = kind.GetText();
		
		if (text == null)
			return;
			
		var tokens = SyntaxTree.ParseTokens(text);
		var token = Assert.Single(tokens);
		
		Assert.Equal(kind, token.Kind);
		Assert.Equal(text, token.Text);
	}
	
	public static TheoryData<SyntaxKind> GetSyntaxKindData()
	{
		var data = new TheoryData<SyntaxKind>();
		var kinds = Enum.GetValues<SyntaxKind>();
		
		foreach (var kind in kinds)
			data.Add(kind);
			
		return data;
	}
}