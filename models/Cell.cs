using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pexel.models
{
    public class Cell
    {
        public string Expression { get; set; }
        public string Value { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
            Expression = string.Empty;
            Value = string.Empty;
        }

        public void CalculateValue()
        {
          
        }
    }
}