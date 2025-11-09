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

        public List<string> DependentCells { get; } = new List<string>(); // Список комірок, які залежать від даної комірки.
        public List<string> Dependencies { get; } = new List<string>();  // Список комірок, від яких залежить дана комірка.

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
                // Очищаємо залежності
                foreach (var dep in Dependencies)
                {
                    var depCell = sheet.GetCellById(dep);
                    if (depCell != null)
                    {
                        depCell.DependentCells.Remove(Id);
                    }
                }
                Dependencies.Clear();

                // Перераховуємо всі залежні клітинки
                RecalculateDependents(sheet);
                return;
            }

            Expression = content;
            UpdateDependencies(sheet);
            CalculateValue(sheet);
            RecalculateDependents(sheet);
        }

        public void CalculateValue(Sheet sheet, HashSet<string>? visited = null)
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
            visited ??= new HashSet<string>();
            string upperId = Id.ToUpper();

            try
            {
                if (visited.Contains(upperId))
                {
                    Value = "#ERR: Circular Reference";
                    return;
                }

                visited.Add(upperId);

                // Перевірка на циклічні посилання через рекурсивний обхід залежностей
                foreach (var dep in Dependencies)
                {
                    var depCell = sheet.GetCellById(dep);
                    if (depCell != null)
                    {
                        var chainVisited = new HashSet<string> { upperId };
                        if (HasCircularReference(depCell, sheet, chainVisited))
                        {
                            Value = "#ERR: Circular Reference";
                            return;
                        }
                    }
                }

                var calculator = new ExpressionCalculator(sheet);
                double result = calculator.Evaluate(formula, visited);
                Value = ConvertResultToDisplayValue(result, formula);
            }
            catch (Exception ex)
            {
                Value = "#ERR: " + ex.Message;
            }
            finally
            {
                visited.Remove(upperId);
            }
        }

        private bool HasCircularReference(Cell cell, Sheet sheet, HashSet<string> chainVisited)
        {
            if (string.IsNullOrWhiteSpace(cell.Expression) || !cell.Expression.StartsWith("="))
                return false;

            foreach (var dep in cell.Dependencies)
            {
                if (chainVisited.Contains(dep))
                    return true;

                chainVisited.Add(dep);
                var nextCell = sheet.GetCellById(dep);
                if (nextCell != null && HasCircularReference(nextCell, sheet, chainVisited))
                    return true;
                chainVisited.Remove(dep);
            }

            return false;
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
