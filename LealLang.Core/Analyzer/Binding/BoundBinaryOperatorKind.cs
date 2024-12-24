namespace LealLang.Core.Analyzer.Binding;

internal enum BoundBinaryOperatorKind
{
	Addition,
	Subtraction,
	Multiplication,
	Division,
    LogicalAnd,
    LogicalOr,
    LogicalEquality,
    LogicalInequality,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual
}