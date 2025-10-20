using System;
using System.IO;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Pexel.ViewModels;
using Pexel.models;
using Microsoft.Maui.Storage;

namespace Pexel;

public partial class MainPage : ContentPage
{
    private SheetViewModel _viewModel;

    private const int CellWidth = 120;
    private const int CellHeight = 50;

    public MainPage()
    {
        InitializeComponent();

        _viewModel = new SheetViewModel();
        BindingContext = _viewModel;

        _viewModel.RecalculateAll();
        BuildGridUI();
    }

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
                Text = GetColumnName(c),
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

                var entry = new Entry
                {
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Center,
                    IsVisible = false,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    Margin = new Thickness(0)
                };

                var valueLabel = new Label
                {
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    LineBreakMode = LineBreakMode.NoWrap,
                    Margin = new Thickness(0),
                    Text = cell.ShowUnfocused()
                };

                var overlay = new Grid();
                overlay.Add(valueLabel);
                overlay.Add(entry);

                // Подія натискання для редагування
                var tap = new TapGestureRecognizer();
                tap.Tapped += (s, ea) =>
                {
                    valueLabel.IsVisible = false;
                    entry.IsVisible = true;
                    entry.Text = cell.ShowFocused();
                    entry.Focus();
                };
                overlay.GestureRecognizers.Add(tap);

                // Завершення редагування
                entry.Unfocused += (s, ea) =>
                {
                    cell.Write(entry.Text);
                    cell.CalculateValue(_viewModel.CurrentSheet);

                    entry.IsVisible = false;
                    valueLabel.IsVisible = true;
                    valueLabel.Text = cell.ShowUnfocused();
                };

                entry.Completed += (s, ea) =>
                {
                    cell.Write(entry.Text);
                    cell.CalculateValue(_viewModel.CurrentSheet);

                    entry.Unfocus();
                };

                var border = new Border { Stroke = Colors.LightGray, StrokeThickness = 1, Content = overlay };
                dataGrid.Add(border, c + 1, r + 1);
            }
        }
    }

    private string GetColumnName(int index)
    {
        string s = string.Empty;
        int col = index + 1;
        while (col > 0)
        {
            int rem = (col - 1) % 26;
            s = (char)('A' + rem) + s;
            col = (col - 1) / 26;
        }
        return s;
    }

    private void AddRow_Clicked(object? sender, EventArgs e)
    {
        _viewModel.CurrentSheet.AddRow();
        _viewModel.RecalculateAll();
        BuildGridUI();
    }

    private void AddColumn_Clicked(object? sender, EventArgs e)
    {
        _viewModel.CurrentSheet.AddColumn();
        _viewModel.RecalculateAll();
        BuildGridUI();
    }

    private void DeleteRow_Clicked(object sender, EventArgs e)
    {
        _viewModel.DeleteRow();
        _viewModel.RecalculateAll();
        BuildGridUI();
    }

    private void DeleteColumn_Clicked(object sender, EventArgs e)
    {
        _viewModel.DeleteColumn();
        _viewModel.RecalculateAll();
        BuildGridUI();
    }


    private async void OnSaveClicked(object sender, EventArgs e)
    {
        string path = Path.Combine(FileSystem.AppDataDirectory, "sheet.json");
        _viewModel.SaveToFile(path);
        await DisplayAlert("Збережено", $"Файл збережено до {path}", "OK");
    }

    private async void OnLoadClicked(object sender, EventArgs e)
    {
        string path = Path.Combine(FileSystem.AppDataDirectory, "sheet.json");
        _viewModel.LoadFromFile(path);
        _viewModel.RecalculateAll();
        BuildGridUI();
        await DisplayAlert("Завантажено", $"Файл завантажено з {path}", "OK");
    }

    private async void OnAboutClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Про програму", "Pexel — простий редактор електронних таблиць з підтримкою арифметичних виразів. Інтерфейс українською.", "OK");
    }

    private async void OnExitClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Вихід", "Ви дійсно хочете вийти з програми?", "Так", "Ні");
        if (answer)
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
    }
}
