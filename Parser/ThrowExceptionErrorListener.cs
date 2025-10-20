using Antlr4.Runtime;
using System;
using System.IO;

public class ThrowExceptionErrorListener : BaseErrorListener, IAntlrErrorListener<int>
{
    public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
    {
        throw new ArgumentException($"Невірний вираз: {msg} у позиції {charPositionInLine}", e);
    }

    public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
    {
        throw new ArgumentException($"Невірний вираз: {msg} у позиції {charPositionInLine}", e);
    }
}