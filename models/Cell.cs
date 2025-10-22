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
                return; // щоб уникнути циклу
            visited.Add(Id);

            foreach (var dependentId in DependentCells)
            {
                var depCell = sheet.GetCellById(dependentId);
                if (depCell == null) continue;

                depCell.CalculateValue(sheet);
                depCell.RecalculateDependents(sheet, visited); // рекурсія
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
