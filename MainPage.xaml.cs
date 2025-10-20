using System;
using System.IO;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Pexel.ViewModels;
using Pexel.models;
using PCell = Pexel.models.Cell;
using Microsoft.Maui.Storage;

namespace Pexel;

public partial class MainPage : ContentPage
{
    private SheetViewModel _viewModel;

    // Початкові розміри клітинок
    private const int CellWidth = 120;
    private const int CellHeight = 50;

    public MainPage()
    {
        InitializeComponent();

        _viewModel = new SheetViewModel();
        BindingContext = _viewModel;

        // Підготовка заголовків та обчислень
        _viewModel.UpdateHeaders();
        _viewModel.RecalculateAll();

        BuildGridUI();
    }

    // Побудова таблиці у Grid
    private void BuildGridUI()
    {
        var sheet = _viewModel.CurrentSheet;

        dataGrid.Children.Clear();
        dataGrid.RowDefinitions.Clear();
        dataGrid.ColumnDefinitions.Clear();

        // Додаємо рядок заголовків
        dataGrid.RowDefinitions.Add(new RowDefinition { Height = CellHeight });
        for (int r = 0; r < sheet.RowCount; r++)
            dataGrid.RowDefinitions.Add(new RowDefinition { Height = CellHeight });

        // Додаємо першу колонку для номерів рядків
        dataGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 60 });
        for (int c = 0; c < sheet.ColumnCount; c++)
            dataGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = CellWidth });

        // Заголовки колонок
        for (int c = 0; c < sheet.ColumnCount; c++)
        {
            var headerLabel = new Label
            {
                Text = _viewModel.ColumnHeaders[c],
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Colors.White
            };
            dataGrid.Add(headerLabel, c + 1, 0);
        }

        // Рядки та клітинки
        for (int r = 0; r < sheet.RowCount; r++)
        {
            // Номер рядка
            var rowLabel = new Label
            {
                Text = (r + 1).ToString(),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Colors.White
            };
            dataGrid.Add(rowLabel, 0, r + 1);

            for (int c = 0; c < sheet.ColumnCount; c++)
            {
                var cell = sheet.Cells[r][c];

                // Entry для редагування клітинки
                var entry = new Entry
                {
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Center,
                    IsVisible = false,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    Margin = new Thickness(0)
                };
                entry.BindingContext = cell;
                entry.SetBinding(Entry.TextProperty, new Binding(nameof(PCell.Expression), mode: BindingMode.TwoWay));

                // Label для відображення обчисленого значення
                var valueLabel = new Label
                {
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    LineBreakMode = LineBreakMode.NoWrap,
                    Margin = new Thickness(0)
                };
                valueLabel.BindingContext = cell;
                valueLabel.SetBinding(Label.TextProperty, new Binding(nameof(PCell.Value)));

                // Grid overlay для перемикання між Label і Entry
                var overlay = new Grid();
                overlay.Add(valueLabel);
                overlay.Add(entry);

                // Подія натискання на клітинку для редагування
                var tap = new TapGestureRecognizer();
                tap.Tapped += (s, ea) =>
                {
                    valueLabel.IsVisible = false;
                    entry.IsVisible = true;
                    entry.Focus();
                };
                overlay.GestureRecognizers.Add(tap);

                // Подія виходу з Entry або завершення редагування
                entry.Unfocused += (s, ea) =>
                {
                    if (entry.BindingContext is PCell ccell)
                        ccell.CalculateValue(_viewModel.CurrentSheet);

                    entry.IsVisible = false;
                    valueLabel.IsVisible = true;
                };

                entry.Completed += (s, ea) =>
                {
                    if (entry.BindingContext is PCell ccell)
                        ccell.CalculateValue(_viewModel.CurrentSheet);

                    entry.Unfocus();
                };

                // Додаємо рамку навколо клітинки
                var border = new Border { Stroke = Colors.LightGray, StrokeThickness = 1, Content = overlay };
                dataGrid.Add(border, c + 1, r + 1);
            }
        }
    }

    // Додаємо новий рядок
    private void AddRow_Clicked(object? sender, EventArgs e)
    {
        _viewModel.CurrentSheet.AddRow();
        _viewModel.UpdateHeaders();
        _viewModel.RecalculateAll();
        BuildGridUI();
    }

    // Додаємо нову колонку
    private void AddColumn_Clicked(object? sender, EventArgs e)
    {
        _viewModel.CurrentSheet.AddColumn();
        _viewModel.UpdateHeaders();
        _viewModel.RecalculateAll();
        BuildGridUI();
    }

    // Збереження таблиці у файл
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var vm = BindingContext as SheetViewModel;
        if (vm == null) return;
        string path = Path.Combine(FileSystem.AppDataDirectory, "sheet.json");
        vm.SaveToFile(path);
        await DisplayAlert("Збережено", $"Файл збережено до {path}", "OK");
    }

    // Завантаження таблиці з файлу(не реалізовано)
    private async void OnLoadClicked(object sender, EventArgs e)
    {
        var vm = BindingContext as SheetViewModel;
        if (vm == null) return;
        string path = Path.Combine(FileSystem.AppDataDirectory, "sheet.json");
        vm.LoadFromFile(path);
        vm.UpdateHeaders();
        vm.RecalculateAll();
        BuildGridUI();
        await DisplayAlert("Завантажено", $"Файл завантажено з {path}", "OK");
    }

    // Інформація про програму
    private async void OnAboutClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Про програму", "Pexel — простий редактор електронних таблиць з підтримкою арифметичних виразів. Інтерфейс українською.", "OK");
    }

    // Вихід з програми
    private async void OnExitClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Вихід", "Ви дійсно хочете вийти з програми?", "Так", "Ні");
        if (answer)
        {
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        }
    }
}
