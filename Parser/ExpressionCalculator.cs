using Antlr4.Runtime;
using Pexel.models;
using System;
using System.Collections.Generic;

namespace Pexel.ExpressionLogic
{
    public class ExpressionCalculator
    {
        private readonly Sheet _sheet;

        public ExpressionCalculator(Sheet sheet)
        {
            _sheet = sheet ?? throw new ArgumentNullException(nameof(sheet));
        }

        public double Evaluate(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                return 0.0;

            var inputStream = new AntlrInputStream(expression);
            var lexer = new LabCalculatorLexer(inputStream);
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(new ThrowExceptionErrorListener());

            var tokens = new CommonTokenStream(lexer);
            var parser = new LabCalculatorParser(tokens);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new ThrowExceptionErrorListener());

            var tree = parser.compileUnit();
            var visited = new HashSet<string>();
            var visitor = new PexelExpressionVisitor(_sheet, visited);
            return visitor.Visit(tree);
        }
    }
}
