using System;
using System.Collections.Generic;
using Pexel.ExpressionLogic; 

namespace Pexel.models
{
    public class Cell
    {
        public string Id { get; }
        public string? Expression { get; set; }
        public string? Value { get; set; }

        public Cell(string id)
        {
            Id = id;
            Expression = string.Empty;
            Value = string.Empty;
        }

        // Встановити вираз або число в клітинку
        public void Write(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                Expression = null;
                Value = null;
                return;
            }

            Expression = content;
        }

        // Обчислити значення клітинки на основі Expression
        public void CalculateValue(Sheet sheet)
        {
            if (string.IsNullOrWhiteSpace(Expression))
            {
                Value = string.Empty;
                return;
            }

            if (!Expression.StartsWith("="))
            {
                Value = Expression;
                return;
            }

            string formula = Expression.Substring(1);

            try
            {
                var calculator = new ExpressionCalculator(sheet);
                double result = calculator.Evaluate(formula);
                Value = result.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Value = "#ERR: " + ex.Message;
            }
        }

        // Показати значення під час редагування
        public string ShowFocused()
        {
            return Expression ?? "";
        }

        // Показати значення після редагування
        public string ShowUnfocused()
        {
            return Value ?? "";
        }
    }
}
