using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel; //for ObservableCollection
using System.Collections.Specialized;
using System.Numerics;

namespace Pexel.models
{
    public class Sheet
    {
        private ObservableCollection<ObservableCollection<Cell>> _cells; 
        public ObservableCollection<ObservableCollection<Cell>> Cells => _cells;
        public Sheet(int initialRows, int initialColumns)
        {
            _cells = new ObservableCollection<ObservableCollection<Cell>>();
            for (int i = 0; i < initialRows; i++)
            {
                var newRow = new ObservableCollection<Cell>();
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
            var newRow = new ObservableCollection<Cell>();
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
            for(int i = 0; i < this.RowCount; i++)
            {
                _cells[i].Add(new Cell(i, newColIdx));
            }
        }


    }
}
