namespace LealLang.Core.Syntax;

public sealed class SyntaxToken : SyntaxNode
{
    public SyntaxToken(SyntaxKind syntaxKind, int position, string? text, object? value = null)
    {
        Kind = syntaxKind;
        Position = position;
        Text = text;
        Value = value;
    }

    public int Position { get; }
    public string? Text { get; }
    public object? Value { get; }
    public override SyntaxKind Kind { get; }
}