using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;
using Pexel.models;

namespace Pexel.ViewModels
{
    public class SheetViewModel
    {
        public Sheet CurrentSheet { get; set; }

        public ICommand AddRowCommand { get; }
        public ICommand AddColumnCommand { get; }

        public SheetViewModel()
        {
            CurrentSheet = new Sheet(5, 5);
            AddRowCommand = new Command(AddRow);
            AddColumnCommand = new Command(AddColumn);
        }

        private void AddRow()
        {
            CurrentSheet.AddRow();
        }

        private void AddColumn()
        {
            CurrentSheet.AddColumn();
        }

        public void DeleteRow()
        {
            CurrentSheet.RemoveRow();
        }

        public void DeleteColumn()
        {
            CurrentSheet.RemoveColumn();
        }


        public void RecalculateAll()
        {
            for (int r = 0; r < CurrentSheet.RowCount; r++)
            {
                for (int c = 0; c < CurrentSheet.ColumnCount; c++)
                {
                    CurrentSheet.Cells[r][c].CalculateValue(CurrentSheet);
                }
            }
        }

        public void SaveToFile(string path)
        {
            var simple = new List<object>();
            for (int r = 0; r < CurrentSheet.RowCount; r++)
            {
                for (int c = 0; c < CurrentSheet.ColumnCount; c++)
                {
                    var cell = CurrentSheet.Cells[r][c];
                    string content = cell.ShowFocused();
                    if (!string.IsNullOrEmpty(content))
                        simple.Add(new { Row = r, Col = c, Content = content });
                }
            }

            var json = JsonSerializer.Serialize(simple);
            File.WriteAllText(path, json);
        }

        public void LoadFromFile(string path)
        {
            if (!File.Exists(path)) return;

            var json = File.ReadAllText(path);
            var items = JsonSerializer.Deserialize<List<SimpleCell>>(json);
            if (items == null) return;

            int maxRow = items.Count > 0 ? items.Max(i => i.Row) + 1 : CurrentSheet.RowCount;
            int maxCol = items.Count > 0 ? items.Max(i => i.Col) + 1 : CurrentSheet.ColumnCount;

            while (CurrentSheet.RowCount < maxRow) CurrentSheet.AddRow();
            while (CurrentSheet.ColumnCount < maxCol) CurrentSheet.AddColumn();

            foreach (var sc in items)
            {
                CurrentSheet.Cells[sc.Row][sc.Col].Write(sc.Content, CurrentSheet);
            }
        }

        private class SimpleCell
        {
            public int Row { get; set; }
            public int Col { get; set; }
            public string Content { get; set; } = string.Empty;
        }
    }
}
