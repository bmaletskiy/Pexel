using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Pexel.models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.IO;

namespace Pexel.ViewModels

{
    public class SheetViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public Sheet CurrentSheet { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<string> ColumnHeaders { get; set; } = new System.Collections.ObjectModel.ObservableCollection<string>();
        public System.Collections.ObjectModel.ObservableCollection<string> RowHeaders { get; set; } = new System.Collections.ObjectModel.ObservableCollection<string>();
        public ICommand AddRowCommand { get; }
        public ICommand AddColumnCommand { get; }

        private void AddNewRow()
        {
            CurrentSheet.AddRow();
            UpdateHeaders();
            RecalculateAll(); 
        }

        private void AddColumn()
        {
            CurrentSheet.AddColumn();
            UpdateHeaders();
            RecalculateAll(); 
        }

        public SheetViewModel()
        {
            CurrentSheet = new Sheet(5, 5);
            AddRowCommand = new Command(AddNewRow);
            AddColumnCommand = new Command(AddColumn);
            UpdateHeaders();
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
                    if (!string.IsNullOrEmpty(cell.Expression))
                        simple.Add(new { Row = r, Col = c, Expression = cell.Expression });
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

            if (items.Count == 0)
            {

                UpdateHeaders();
                RecalculateAll();
                return;
            }

            int maxRow = items.Max(i => i.Row) + 1;
            int maxCol = items.Max(i => i.Col) + 1;
            
            if (maxRow > CurrentSheet.RowCount)
            {
                while (CurrentSheet.RowCount < maxRow) CurrentSheet.AddRow();
            }
            if (maxCol > CurrentSheet.ColumnCount)
            {
                while (CurrentSheet.ColumnCount < maxCol) CurrentSheet.AddColumn();
            }
            
            foreach (var sc in items)
            {
                CurrentSheet.Cells[sc.Row][sc.Col].Expression = sc.Expression;
            }
            
            UpdateHeaders();
            RecalculateAll(); 
        }

        public void UpdateHeaders()
        {
            // Update column headers (ObservableCollection will notify UI)
            ColumnHeaders.Clear();
            int cols = CurrentSheet.ColumnCount;
            for (int i = 0; i < cols; i++)
            {
                ColumnHeaders.Add(GetColumnName(i));
            }

            // Update row headers
            RowHeaders.Clear();
            int rows = CurrentSheet.RowCount;
            for (int r = 0; r < rows; r++)
            {
                RowHeaders.Add((r + 1).ToString());
            }
        }

        private static string GetColumnName(int index)
        {
            // 0 -> A, 25 -> Z, 26 -> AA
            var s = string.Empty;
            int col = index + 1;
            while (col > 0)
            {
                int rem = (col - 1) % 26;
                s = (char)('A' + rem) + s;
                col = (col - 1) / 26;
            }
            return s;
        }

        private class SimpleCell { public int Row { get; set; } public int Col { get; set; } public string Expression { get; set; } = string.Empty; }
    }
}