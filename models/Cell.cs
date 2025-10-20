using System;
using System.Collections.Generic;
using System.Linq;

namespace Pexel.models
{
    public class Cell : System.ComponentModel.INotifyPropertyChanged
    {
        private string _expression;
        private string _value;

        public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string? name) => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(name));

        public string Expression
        {
            get => _expression;
            set
            {
                if (_expression != value)
                {
                    _expression = value;
                    OnPropertyChanged(nameof(Expression));
                }
            }
        }

        public string Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        public int Row { get; set; }
        public int Column { get; set; }

        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
            _expression = string.Empty;
            _value = string.Empty;
        }

        public void CalculateValue()
        {
        }

        public void CalculateValue(Sheet sheet)
        {
            if (sheet == null) throw new ArgumentNullException(nameof(sheet));
            string expr = this.Expression ?? string.Empty;

            if (string.IsNullOrWhiteSpace(expr))
            {
                this.Value = string.Empty;
                return;
            }

            if (!expr.StartsWith("="))
            {
                this.Value = expr;
                return;
            }

            string formula = expr.Substring(1);

            try
            {
                var calculator = new Pexel.ExpressionLogic.ExpressionCalculator(sheet);
                double result = calculator.Evaluate(formula);
                this.Value = result.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                this.Value = "#ERR: " + ex.Message;
            }
        }
    }
}
