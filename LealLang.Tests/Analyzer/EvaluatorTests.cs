
using LealLang.Core.Analyzer;
using LealLang.Core.Analyzer.Syntax;
using LealLang.Core.Analyzer.Text;

namespace LealLang.Tests.Analyzer;

public class EvaluatorTests
{
	[Theory]
	[MemberData(nameof(GetEvaluationData))]
	public void Evaluator_EvaluationTests(string text, object expectedResult)
	{
		var syntaxTree = SyntaxTree.Parse(text);
		var compilation = new Compilation(syntaxTree);
		var testVariables = new Dictionary<VariableSymbol, object?>();
		var evaluation = compilation.Evaluate(testVariables);
		
		Assert.Equal(expectedResult, evaluation.Value);
	}

	public static TheoryData<string, object> GetEvaluationData() => new()
	{
		{ "1", 1 },
		{ "1 + 2", 3 },
		{ "1 - 2", -1 },
		{ "1 * 2", 2 },
		{ "4 / 2", 2 },
		{ "1 + 2 * 3", 7 },
		{ "(1 + 2) * 3", 9 },
		{ "5 + 3 * 2", 11 },
		{ "(5 + 3) * 2", 16 },
		{ "10 - 3 * 2", 4 },
		{ "(10 - 3) * 2", 14 },
		{ "10 / 2 + 3", 8 },
		{ "10 / (2 + 3)", 2 },
		{ "-1", -1 },
		{ "-1 + -2", -3 },
		{ "-3 + 2", -1 },
		{ "-3 * -2", 6 },
		{ "(-3 + 2) * -1", 1 },
		{ "-(3 + 2)", -5 },
		{ "true", true },
		{ "false", false },
		{ "true && true", true },
		{ "false && false", false },
		{ "true && false", false },
		{ "false && true", false },
		{ "true || true", true },
		{ "true || false", true },
		{ "false || true", true },
		{ "false || false", false },
		{ "true == true", true },
		{ "true == false", false },
		{ "false == true", false },
		{ "false == false", true },
		{ "true != true", false },
		{ "true != false", true },
		{ "false != true", true },
		{ "false != false", false },
		{ "(true || false) && true", true },
		{ "(true || false) && false", false },
		{ "(true && false) || true", true },
		{ "(true && false) || false", false },
		{ "true && (false || true)", true },
		{ "false && (true || false)", false },
		{ "(true || false) && (true && true)", true },
		{ "(true || false) && (true && false)", false },
		{ "!true", false },
		{ "!false", true },
		{ "!(true && true)", false },
		{ "!(true && false)", true },
		{ "!(false && true)", true },
		{ "!(false && false)", true },
		{ "!(true || true)", false },
		{ "!(true || false)", false },
		{ "!(false || true)", false },
		{ "!(false || false)", true },
		{ "1 < 2", true },
		{ "1 > 2", false },
		{ "1 <= 2", true },
		{ "1 >= 2", false },
		{ "2 <= 2", true },
		{ "2 >= 2", true },
		{ "1 < 1", false },
		{ "1 > 1", false },
		{ "(1 < 2) && (2 > 1)", true },
		{ "(1 > 2) || (2 < 3)", true },
		{ "(1 <= 2) && (2 >= 1)", true },
		{ "(1 >= 2) || (2 <= 1)", false },
		{ "(2 <= 2) && (2 >= 2)", true },
		{ "(1 < 1) || (1 > 1)", false },
		{ "1 < 2 && true", true },
		{ "1 > 2 && true", false },
		{ "1 < 2 || false", true },
		{ "1 > 2 || true", true },
		{ "1 < 2 && false", false },
		{ "1 > 2 && false", false },
		{ "1 < 2 || true", true },
		{ "1 > 2 || false", false },
		{ "(1 < 2) && true", true },
		{ "(1 > 2) && true", false },
		{ "(1 < 2) || false", true },
		{ "(1 > 2) || true", true },
		{ "(1 < 2) && false", false },
		{ "(1 > 2) && false", false },
		{ "(1 < 2) || true", true },
		{ "(1 > 2) || false", false },
		{ "!true && 1 == 1", false },
		{ "!false && 1 == 1", true },
		{ "!true || 1 == 1", true },
		{ "!false || 1 == 1", true },
		{ "!(true && 1 == 1)", false },
		{ "!(false && 1 == 1)", true },
		{ "!(true || 1 == 1)", false },
		{ "!(false || 1 == 1)", false }
	};
}