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

        public List<string> DependentCells { get; } = new List<string>();
        public List<string> Dependencies { get; } = new List<string>();

        public Cell(string id)
        {
            Id = id;
            Expression = string.Empty;
            Value = string.Empty;
        }

        // Встановити вираз або число в клітинку
        public void Write(string content, Sheet sheet)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                Expression = null;
                Value = null;
                Dependencies.Clear();
                RecalculateDependents(sheet);
                return;
            }

            Expression = content;

            UpdateDependencies(sheet);
            CalculateValue(sheet);
            RecalculateDependents(sheet);
        }

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
                var visited = new HashSet<string> { Id.ToUpper() };
                double result = calculator.Evaluate(formula, visited);
                Value = ConvertResultToDisplayValue(result, formula);
            }
            catch (Exception ex)
            {
                Value = "#ERR: " + ex.Message;
            }

            RecalculateDependents(sheet);
        }

        private string ConvertResultToDisplayValue(double result, string formula)
        {
            if (double.IsNaN(result))
                return "0";

            if (formula.Contains("<") || formula.Contains(">") || formula.Contains("="))
                return Math.Abs(result - 1.0) < 1e-9 ? "True" : "False";

            return result.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
        public void UpdateDependencies(Sheet sheet)
        {
            // видаляємо старі залежності
            foreach (var dep in Dependencies)
            {
                var depCell = sheet.GetCellById(dep);
                depCell?.DependentCells.Remove(Id);
            }
            Dependencies.Clear();

            if (string.IsNullOrWhiteSpace(Expression) || !Expression.StartsWith("="))
                return;

            var matches = System.Text.RegularExpressions.Regex.Matches(Expression, @"[A-Z]+\d+");

            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                string depId = match.Value;
                if (depId.Equals(Id, StringComparison.OrdinalIgnoreCase))
                {
                    Value = "#ERR: Self reference";
                    return;
                }
                Dependencies.Add(depId);

                var depCell = sheet.GetCellById(depId);
                if (depCell != null && !depCell.DependentCells.Contains(Id))
                    depCell.DependentCells.Add(Id);
            }
        }

        public void RecalculateDependents(Sheet sheet, HashSet<string>? visited = null)
        {
            visited ??= new HashSet<string>();
            if (visited.Contains(Id))
                return;
            visited.Add(Id);

            foreach (var dependentId in DependentCells)
            {
                var depCell = sheet.GetCellById(dependentId);
                if (depCell == null) continue;

                depCell.CalculateValue(sheet);
                depCell.RecalculateDependents(sheet, visited);
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
