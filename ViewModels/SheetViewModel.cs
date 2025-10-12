using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Pexel.models;
namespace Pexel.ViewModels

{
    public class SheetViewModel
    {
        public Sheet CurrentSheet { get; set; }
        public ICommand AddRowCommand { get; }
        public ICommand AddColumnCommand { get; }
        private void AddNewRow()
        {
            CurrentSheet.AddRow();
        }

        private void AddColumn()
        {
            CurrentSheet.AddColumn();
        }
        public SheetViewModel()
        {
            CurrentSheet = new Sheet(5, 5);
            AddRowCommand = new Command(AddNewRow);
            AddColumnCommand = new Command(AddColumn);
        }
    }
}