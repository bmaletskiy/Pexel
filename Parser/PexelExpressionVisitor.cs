using Antlr4.Runtime.Misc;
using Antlr4.Runtime;
using Pexel.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using CellModel = Pexel.models.Cell;

namespace Pexel.ExpressionLogic
{
    public class PexelExpressionVisitor : LabCalculatorBaseVisitor<double>
    {
        private readonly Sheet _sheet;
        private readonly HashSet<string> _visited; 

        public PexelExpressionVisitor(Sheet sheet, HashSet<string>? visited = null)
        {
            _sheet = sheet ?? throw new ArgumentNullException(nameof(sheet));
            _visited = visited ?? new HashSet<string>();
        }

        public override double VisitCompileUnit(LabCalculatorParser.CompileUnitContext context)
            => Visit(context.expression());

        public override double VisitNumberExpr(LabCalculatorParser.NumberExprContext context)
            => double.Parse(context.GetText());

        public override double VisitIdentifierExpr(LabCalculatorParser.IdentifierExprContext context)
        {
            var identifierName = context.GetText().ToUpper();

            try
            {
                int col = identifierName[0] - 'A';
                int row = int.Parse(identifierName.Substring(1)) - 1;

                if (row < 0 || row >= _sheet.RowCount || col < 0 || col >= _sheet.ColumnCount)
                    throw new Exception($"Посилання на неіснуючу комірку: {identifierName}");

                if (_visited.Contains(identifierName))
                    throw new Exception($"Циклічне посилання на клітинку {identifierName}");

                CellModel targetCell = _sheet.Cells[row][col];

                if (string.IsNullOrWhiteSpace(targetCell.Expression) && string.IsNullOrWhiteSpace(targetCell.Value))
                    return 0.0;

                if (string.IsNullOrEmpty(targetCell.Value) && !string.IsNullOrEmpty(targetCell.Expression))
                {
                    _visited.Add(identifierName);
                    try
                    {
                        var calculator = new ExpressionCalculator(_sheet);
                        double val = calculator.Evaluate(targetCell.Expression, _visited);
                        targetCell.Value = val.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    }
                    finally
                    {
                        _visited.Remove(identifierName);
                    }
                }

                if (string.IsNullOrWhiteSpace(targetCell.Value))
                    return 0.0;

                if (targetCell.Value.Equals("TRUE", StringComparison.OrdinalIgnoreCase))  return 1.0;
                if (targetCell.Value.Equals("FALSE", StringComparison.OrdinalIgnoreCase)) return 0.0;

                if (double.TryParse(targetCell.Value, out double cellValue)) return cellValue;

                if (targetCell.Value.StartsWith("#"))
                    throw new Exception($"Посилання на комірку з помилкою {identifierName} ('{targetCell.Value}')");

                throw new Exception($"Значення комірки {identifierName} ('{targetCell.Value}') не є числом.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка отримання значення для {identifierName}: {ex.Message}");
                throw;
            }
        }

        public override double VisitParenthesizedExpr(LabCalculatorParser.ParenthesizedExprContext context)
            => Visit(context.expression());

        public override double VisitUnaryMinusExpr([NotNull] LabCalculatorParser.UnaryMinusExprContext context)
            => -Visit(context.expression());

        public override double VisitUnaryPlusExpr([NotNull] LabCalculatorParser.UnaryPlusExprContext context)
            => Visit(context.expression());

        public override double VisitExponentialExpr(LabCalculatorParser.ExponentialExprContext context)
            => Math.Pow(Visit(context.expression(0)), Visit(context.expression(1)));

        public override double VisitAdditiveExpr(LabCalculatorParser.AdditiveExprContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));
            return context.operatorToken.Type == LabCalculatorLexer.ADD ? left + right : left - right;
        }

        public override double VisitMultiplicativeExpr(LabCalculatorParser.MultiplicativeExprContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));

            switch (context.operatorToken.Type)
            {
                case LabCalculatorLexer.MULTIPLY: return left * right;
                case LabCalculatorLexer.DIVIDE:
                    if (right == 0) throw new DivideByZeroException("Ділення на нуль");
                    return left / right;
                case LabCalculatorLexer.MOD:
                    if (right == 0) throw new DivideByZeroException("Ділення на нуль (mod)");
                    return left % right;
                default: // DIV
                    if (right == 0) throw new DivideByZeroException("Ділення на нуль (div)");
                    return Math.Truncate(left / right);
            }
        }

        public override double VisitComparisonExpr([NotNull] LabCalculatorParser.ComparisonExprContext context)
        {
            double left = Visit(context.expression(0));
            double right = Visit(context.expression(1));
            const double eps = 1e-7;

            return context.operatorToken.Type switch
            {
                LabCalculatorLexer.EQUAL        => Math.Abs(left - right) < eps ? 1.0 : 0.0,
                LabCalculatorLexer.LESS         => left <  right ? 1.0 : 0.0,
                LabCalculatorLexer.GREATER      => left >  right ? 1.0 : 0.0,
                LabCalculatorLexer.LESSEQUAL    => left <= right ? 1.0 : 0.0,
                LabCalculatorLexer.GREATEREQUAL => left >= right ? 1.0 : 0.0,
                LabCalculatorLexer.NOTEQUAL     => Math.Abs(left - right) >= eps ? 1.0 : 0.0,
                _ => throw new ArgumentOutOfRangeException("Невідомий оператор порівняння")
            };
        }
    }
}
