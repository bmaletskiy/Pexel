using System;
using System.Collections.Generic;

namespace Pexel.models
{
    public class Sheet
    {
        private List<List<Cell>> _cells = new List<List<Cell>>();
        public List<List<Cell>> Cells => _cells;

        public Sheet(int initialRows, int initialColumns)
        {
            for (int r = 0; r < initialRows; r++)
            {
                var newRow = new List<Cell>();
                for (int c = 0; c < initialColumns; c++)
                {
                    string id = GetCellId(r, c);
                    newRow.Add(new Cell(id));
                }
                _cells.Add(newRow);
            }
        }

        public int RowCount => _cells.Count;
        public int ColumnCount => RowCount > 0 ? _cells[0].Count : 0;

        public void AddRow()
        {
            int newRowIdx = RowCount;
            var newRow = new List<Cell>();
            for (int c = 0; c < ColumnCount; c++)
            {
                string id = GetCellId(newRowIdx, c);
                newRow.Add(new Cell(id));
            }
            _cells.Add(newRow);
        }

        public void AddColumn()
        {
            int newColIdx = ColumnCount;
            for (int r = 0; r < RowCount; r++)
            {
                string id = GetCellId(r, newColIdx);
                _cells[r].Add(new Cell(id));
            }
        }

        public void RemoveRow()
        {
            if (RowCount > 0)
                 _cells.RemoveAt(RowCount - 1); // видаляємо останній рядок
        }

        public void RemoveColumn()
        {
            if (ColumnCount > 0)
            {
                for (int r = 0; r < RowCount; r++)
                    _cells[r].RemoveAt(ColumnCount - 1); // видаляємо останню колонку
            }
        }


        private string GetCellId(int row, int col)
        {
            // Генеруємо id як Excel-стиль: A1, B1, C1...
            string colName = "";
            int c = col + 1;
            while (c > 0)
            {
                int rem = (c - 1) % 26;
                colName = (char)('A' + rem) + colName;
                c = (c - 1) / 26;
            }
            return colName + (row + 1);
        }
    }
}
