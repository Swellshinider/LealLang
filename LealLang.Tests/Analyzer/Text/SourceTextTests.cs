using LealLang.Core.Analyzer.Text;

namespace LealLang.Tests.Analyzer.Text;

public class SourceTextTests 
{
	[Theory]
	[InlineData("a", 1)]
	[InlineData("a\n", 2)]
	[InlineData("abc\n\n", 3)]
	[InlineData("abc\r\n\r\n\n", 4)]
	[InlineData("abc\r\n\r\n\n\r\n", 5)]
	public void SourceText_IncludeLineTest(string text, int expectedLines) 
	{
		var sourceText = SourceText.From(text);
		Assert.Equal(expectedLines, sourceText.Lines.Length);
	}
}