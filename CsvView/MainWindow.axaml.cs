using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using CsvLib;

// ReSharper disable UnusedParameter.Local

namespace CsvView;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        RenderReg(0);
    }

    private async void BtnLoad_OnClick(object? sender, RoutedEventArgs e)
    {
        TopLevel? topLevel = GetTopLevel(this);
        if (topLevel == null) { return; }

        IReadOnlyList<IStorageFile> files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open CSV File",
            AllowMultiple = false,
            FileTypeFilter = new List<FilePickerFileType>
            {
                new("CSV Files") { Patterns = new[] { "*.csv", }, },
                new("Any File") { Patterns = new[] { "*", }, },
            },
        });

        if (files.Count <= 0) { return; }

        LoadFile(files[0].Path.LocalPath);
    }

    private void BtnSearch_OnClick(object? sender, RoutedEventArgs e)
    {
        Search(TxtSearch.Text);
    }
    
    private void BtnFirst_OnClick(object? sender, RoutedEventArgs e)
    {
        RenderReg(0);
    }

    private void BtnPrevious_OnClick(object? sender, RoutedEventArgs e)
    {
        RenderReg(_currentReg - 1);
    }

    private void TxtIndex_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        RenderReg(int.TryParse(TxtIndex.Text, out int newReg) ? newReg : _currentReg);
    }

    private void BtnNext_OnClick(object? sender, RoutedEventArgs e)
    {
        RenderReg(_currentReg + 1);
    }

    private void BtnLast_OnClick(object? sender, RoutedEventArgs e)
    {
        RenderReg(_totalRegs - 1);
    }

    private string _loadedFile = string.Empty;
    private long _currentReg;
    private int _totalRegs;
    private List<long> _index = new();

    private void LoadFile(string fileName)
    {
        // TODO: Loading animation
        _loadedFile = fileName;
        TxtFileName.Text = fileName;

        CsvFieldIndexer csvIndexer = new();
        csvIndexer.LoadIndexOfFile(_loadedFile);
        _index = csvIndexer.Index;
        _totalRegs = _index.Count - 1;

        RenderReg(0);
    }

    private void Search(string? textToSearch)
    {
        if (textToSearch == null) { return; }
        
        // TODO: Loading animation
        CsvFieldIndexer csvIndexer = new();
        csvIndexer.LoadIndexOfFile(_loadedFile);

        List<long> newIndexes = csvIndexer.Search(_loadedFile, textToSearch);
        _index = newIndexes;
        _totalRegs = _index.Count - 1;

        RenderReg(0, forceLoad: true);
    }

    private bool _rendering;

    private void RenderReg(long currentReg, bool forceLoad = false)
    {
        if (_rendering) { return; }
        _rendering = true;

        if (_index.Count <= 0)
        {
            _currentReg = -1;
            BtnFirst.IsEnabled = false;
            BtnPrevious.IsEnabled = false;
            TxtIndex.IsReadOnly = true;
            BtnNext.IsEnabled = false;
            BtnLast.IsEnabled = false;

            DataContext = new MainWindowViewModel { Index = 0, Fields = new(), };
            _rendering = false;
            return;
        }

        bool first = false;
        bool last = false;
        if (currentReg <= 0)
        {
            currentReg = 0;
            first = true;
        }
        if (currentReg >= (_totalRegs - 1))
        {
            currentReg = _totalRegs - 1;
            last = true;
        }

        BtnFirst.IsEnabled = (first == false);
        BtnPrevious.IsEnabled = (first == false);
        TxtIndex.IsReadOnly = false;
        BtnNext.IsEnabled = (last == false);
        BtnLast.IsEnabled = (last == false);

        if (_currentReg == currentReg && forceLoad == false)
        {
            _rendering = false;
            return;
        }

        _currentReg = currentReg;

        CsvParser csvParser = new();
        csvParser.ParseFile(_loadedFile, _index[(int)currentReg], 1);
        MainWindowViewModel viewModel = new()
        {
            Index = (int)currentReg,
            MaxIndex = _totalRegs,
            Fields = csvParser.Data[0].Select(f => new FieldViewModel { Text = f, }).ToList(),
        };

        DataContext = viewModel;

        _rendering = false;
    }
}

public class FieldViewModel
{
    public string Text { get; set; } = string.Empty;
}

public class MainWindowViewModel
{
    public int? Index { get; set; }

    public int? MaxIndex { get; set; }

    public List<FieldViewModel>? Fields { get; set; }
}
