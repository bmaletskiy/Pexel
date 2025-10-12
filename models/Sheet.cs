using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel; //for ObservableCollection

namespace Pexel.models
{
    public class Sheet
    {
        private ObservableCollection<List<Cell>> _cells; // Changed to ObservableCollection for UI binding
        public ObservableCollection<List<Cell>> Cells => _cells;
        public Sheet(int initialRows, int initialColumns)
        {
            _cells = new ObservableCollection<List<Cell>>();
            for (int i = 0; i < initialRows; i++)
            {
                var newRow = new List<Cell>();
                for (int j = 0; j < initialColumns; j++)
                {
                    newRow.Add(new Cell(i, j));
                }
                _cells.Add(newRow);
            }
        }

        public int RowCount => _cells.Count;
        public int ColumnCount => RowCount > 0 ? _cells[0].Count : 0;
        public void AddRow()
        {
            int newRowIdx = this.RowCount;
            var newRow = new List<Cell>();
            int width = this.ColumnCount;
            for (int i = 0; i < width; i++)
            {
                newRow.Add(new Cell(newRowIdx, i));
            }
            _cells.Add(newRow);
        }

        public void AddColumn()
        {
            int newColIdx = this.ColumnCount;
            int length = this.RowCount;
            var newRow = new List<Cell>();
            for (int i = 0; i < length; i++)
            {
               newRow.Add(new Cell(i, newColIdx));
            }
        }
    }
}