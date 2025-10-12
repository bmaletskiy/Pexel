using Pexel.ViewModels;

namespace Pexel;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        this.BindingContext = new SheetViewModel();
    }
}