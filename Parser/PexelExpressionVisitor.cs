using Antlr4.Runtime.Misc;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Pexel.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pexel.ExpressionLogic
{
    public class PexelExpressionVisitor : LabCalculatorBaseVisitor<double>
    {
        private readonly Sheet _sheet;
        private readonly HashSet<string>? _visited;

        public PexelExpressionVisitor(Sheet sheet, HashSet<string>? visited = null)
        {
            _sheet = sheet ?? throw new ArgumentNullException(nameof(sheet));
            _visited = visited ?? new HashSet<string>();
        }

        public override double VisitCompileUnit(LabCalculatorParser.CompileUnitContext context)
        {
            return Visit(context.expression());
        }

        public override double VisitNumberExpr(LabCalculatorParser.NumberExprContext context)
        {
            var result = double.Parse(context.GetText());
            Debug.WriteLine(result);
            return result;
        }

        public override double VisitIdentifierExpr(LabCalculatorParser.IdentifierExprContext context)
        {
            var identifierName = context.GetText().ToUpper();
            try
            {
                int col = identifierName[0] - 'A';
                int row = int.Parse(identifierName.Substring(1)) - 1;

                if (row >= 0 && row < _sheet.RowCount && col >= 0 && col < _sheet.ColumnCount)
                {
                    Pexel.models.Cell targetCell = _sheet.Cells[row][col];
                    string key = $"{row},{col}";

                    if (_visited!.Contains(key))
                        throw new Exception($"Циклічне посилання на клітинку {identifierName}");

                    if (string.IsNullOrWhiteSpace(targetCell.Expression) && string.IsNullOrWhiteSpace(targetCell.Value))
                    {
                        return 0.0;
                    }

                    if (string.IsNullOrEmpty(targetCell.Value) && !string.IsNullOrEmpty(targetCell.Expression))
                    {
                        try
                        {
                            _visited.Add(key);
                            var calculator = new ExpressionCalculator(_sheet);
                            double val = calculator.Evaluate(targetCell.Expression);
                            targetCell.Value = val.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        {
                            targetCell.Value = "#ERR: " + ex.Message;
                            throw;
                        }
                        finally
                        {
                            _visited.Remove(key);
                        }
                    }

                    if (string.IsNullOrWhiteSpace(targetCell.Value))
                        return 0.0;

                    if (targetCell.Value.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
                        return 1.0;
                    if (targetCell.Value.Equals("FALSE", StringComparison.OrdinalIgnoreCase))
                        return 0.0;

                    if (double.TryParse(targetCell.Value, out double cellValue))
                        return cellValue;

                    if (targetCell.Value.StartsWith("#"))
                        throw new Exception($"Посилання на комірку з помилкою {identifierName} ('{targetCell.Value}')");

                    throw new Exception($"Значення комірки {identifierName} ('{targetCell.Value}') не є числом.");
                }
                else
                {
                    throw new Exception($"Посилання на неіснуючу комірку: {identifierName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка отримання значення для {identifierName}: {ex.Message}");
                throw;
            }
        }



        public override double VisitParenthesizedExpr(LabCalculatorParser.ParenthesizedExprContext context)
        {
            return Visit(context.expression());
        }

        public override double VisitUnaryMinusExpr([NotNull] LabCalculatorParser.UnaryMinusExprContext context)
        {
            return -Visit(context.expression());
        }

        public override double VisitUnaryPlusExpr([NotNull] LabCalculatorParser.UnaryPlusExprContext context)
        {
            return Visit(context.expression());
        }

        public override double VisitExponentialExpr(LabCalculatorParser.ExponentialExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);
            Debug.WriteLine("{0} ^ {1}", left, right);
            return Math.Pow(left, right);
        }

        public override double VisitAdditiveExpr(LabCalculatorParser.AdditiveExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);

            if (context.operatorToken.Type == LabCalculatorLexer.ADD)
            {
                Debug.WriteLine("{0} + {1}", left, right);
                return left + right;
            }
            else
            {
                Debug.WriteLine("{0} - {1}", left, right);
                return left - right;
            }
        }

        public override double VisitMultiplicativeExpr(LabCalculatorParser.MultiplicativeExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);

            if (context.operatorToken.Type == LabCalculatorLexer.MULTIPLY)
            {
                Debug.WriteLine("{0} * {1}", left, right);
                return left * right;
            }
            else if (context.operatorToken.Type == LabCalculatorLexer.DIVIDE)
            {
                if (right == 0) throw new DivideByZeroException("Ділення на нуль");
                Debug.WriteLine("{0} / {1}", left, right);
                return left / right;
            }
            else if (context.operatorToken.Type == LabCalculatorLexer.MOD)
            {
                if (right == 0) throw new DivideByZeroException("Ділення на нуль (mod)");
                Debug.WriteLine("{0} mod {1}", left, right);
                return left % right;
            }
            else
            {
                if (right == 0) throw new DivideByZeroException("Ділення на нуль (div)");
                Debug.WriteLine("{0} div {1}", left, right);
                return Math.Truncate(left / right);
            }
        }

        public override double VisitComparisonExpr([NotNull] LabCalculatorParser.ComparisonExprContext context)
        {
            double left = Visit(context.expression(0));
            double right = Visit(context.expression(1));
            int op = context.operatorToken.Type;
            bool result;
            const double epsilon = 0.0000001;

            switch (op)
            {
                case LabCalculatorLexer.EQUAL: result = Math.Abs(left - right) < epsilon; break;
                case LabCalculatorLexer.LESS: result = left < right; break;
                case LabCalculatorLexer.GREATER: result = left > right; break;
                case LabCalculatorLexer.LESSEQUAL: result = left <= right; break;
                case LabCalculatorLexer.GREATEREQUAL: result = left >= right; break;
                case LabCalculatorLexer.NOTEQUAL: result = Math.Abs(left - right) >= epsilon; break;
                default: throw new ArgumentOutOfRangeException("Невідомий оператор порівняння");
            }
            return result ? 1.0 : 0.0;
        }

        private double WalkLeft(LabCalculatorParser.ExpressionContext context)
        {
            return Visit(context.GetRuleContext<LabCalculatorParser.ExpressionContext>(0));
        }

        private double WalkRight(LabCalculatorParser.ExpressionContext context)
        {
            return Visit(context.GetRuleContext<LabCalculatorParser.ExpressionContext>(1));
        }
    }
}
