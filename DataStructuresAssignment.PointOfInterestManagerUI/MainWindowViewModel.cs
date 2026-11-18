using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataStructuresAssignment.PointsOfInterestManager.DTOs;
using DataStructuresAssignment.PointsOfInterestManager.Enums;
using DataStructuresAssignment.PointsOfInterestManager.Interfaces;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;

namespace DataStructuresAssignment.PointOfInterestManagerUI;

public partial class MainWindowViewModel(IPointOfInterestService service) : ObservableObject
{
    private IPointOfInterestService Service { get; } = service;

    public ETreePicked[] TreePicker { get; } = Enum.GetValues<ETreePicked>();
    public ObservableCollection<PointOfInterestDto> Points { get; } = new ObservableCollection<PointOfInterestDto>();

    #region Settings

    private bool _settings = true;
    public bool Settings
    {
        get => _settings;
        set
        {
            if (SetProperty(ref _settings, value))
            {

            }
        }
    }

    private ETreePicked _tree = ETreePicked.Kd;
    public ETreePicked Tree
    {
        get => _tree;
        set
        {
            if (SetProperty(ref _tree, value))
            {

            }
        }
    }

    private string _minLatitude = "-90";
    public string MinLatitude
    {
        get => _minLatitude;
        set
        {
            if (SetProperty(ref _minLatitude, value))
            {

            }
        }
    }

    private string _minLongitude = "-180";
    public string MinLongitude
    {
        get => _minLongitude;
        set
        {
            if (SetProperty(ref _minLongitude, value))
            {

            }
        }
    }

    private string _maxLatitude = "90";
    public string MaxLatitude
    {
        get => _maxLatitude;
        set
        {
            if (SetProperty(ref _maxLatitude, value))
            {

            }
        }
    }

    private string _maxLongitude = "180";
    public string MaxLongitude
    {
        get => _maxLongitude;
        set
        {
            if (SetProperty(ref _maxLongitude, value))
            {

            }
        }
    }

    private string _maxDepth = "50";
    public string MaxDepth
    {
        get => _maxDepth;
        set
        {
            if (SetProperty(ref _maxDepth, value))
            {

            }
        }
    }

    #endregion Settings

    #region Workspace

    private bool _workspace;
    public bool Workspace
    {
        get => _workspace;
        set
        {
            if (SetProperty(ref _workspace, value))
            {

            }
        }
    }

    #endregion Workspace

    #region Data

    private string _generateCount = "100";
    public string GenerateCount
    {
        get => _generateCount;
        set
        {
            if (SetProperty(ref _generateCount, value))
            {

            }
        }
    }

    private string _generateMinLatitude = "";
    public string GenerateMinLatitude
    {
        get => _generateMinLatitude;
        set => SetProperty(ref _generateMinLatitude, value);
    }

    private string _generateMinLongitude = "";
    public string GenerateMinLongitude
    {
        get => _generateMinLongitude;
        set => SetProperty(ref _generateMinLongitude, value);
    }

    private string _generateMaxLatitude = "";
    public string GenerateMaxLatitude
    {
        get => _generateMaxLatitude;
        set => SetProperty(ref _generateMaxLatitude, value);
    }

    private string _generateMaxLongitude = "";
    public string GenerateMaxLongitude
    {
        get => _generateMaxLongitude;
        set => SetProperty(ref _generateMaxLongitude, value);
    }

    #endregion Data

    #region Point

    private string _pointId = "";
    public string PointId
    {
        get => _pointId;
        set
        {
            if (SetProperty(ref _pointId, value))
            {
                UpdateCommand.NotifyCanExecuteChanged();
                DeleteCommand.NotifyCanExecuteChanged();
            }
        }
    }

    private string _pointName = "";
    public string PointName
    {
        get => _pointName;
        set
        {
            if (SetProperty(ref _pointName, value))
            {

            }
        }
    }

    private string _pointLatitude = "";
    public string PointLatitude
    {
        get => _pointLatitude;
        set
        {
            if (SetProperty(ref _pointLatitude, value))
            {

            }
        }
    }

    private string _pointLongitude = "";
    public string PointLongitude
    {
        get => _pointLongitude;
        set
        {
            if (SetProperty(ref _pointLongitude, value))
            {

            }
        }
    }

    private string _pointAddress = "";
    public string PointAddress
    {
        get => _pointAddress;
        set
        {
            if (SetProperty(ref _pointAddress, value))
            {

            }
        }
    }

    private string _pointDescription = "";
    public string PointDescription
    {
        get => _pointDescription;
        set
        {
            if (SetProperty(ref _pointDescription, value))
            {

            }
        }
    }

    private PointOfInterestDto? _selectedPoint;
    public PointOfInterestDto? SelectedPoint
    {
        get => _selectedPoint;
        set
        {
            if (SetProperty(ref _selectedPoint, value))
            {
                if (value is not null)
                {
                    PointId = value.Id.ToString();
                    PointName = value.Name;
                    PointLatitude = value.Latitude.ToString();
                    PointLongitude = value.Longitude.ToString();
                    PointAddress = value.Address ?? "";
                    PointDescription = value.Description;
                }
                else
                {
                    PointId = "";
                }
            }
        }
    }

    #endregion Point

    #region Search

    private string _latitude = "";
    public string Latitude
    {
        get => _latitude;
        set
        {
            if (SetProperty(ref _latitude, value))
            {

            }
        }
    }

    private string _longitude = "";
    public string Longitude
    {
        get => _longitude;
        set
        {
            if (SetProperty(ref _longitude, value))
            {

            }
        }
    }

    private string _radius = "";
    public string Radius
    {
        get => _radius;
        set
        {
            if (SetProperty(ref _radius, value))
            {

            }
        }
    }

    private string _searchMinLatitude = "";
    public string SearchMinLatitude
    {
        get => _searchMinLatitude;
        set
        {
            if (SetProperty(ref _searchMinLatitude, value))
            {

            }
        }
    }

    private string _searchMinLongitude = "";
    public string SearchMinLongitude
    {
        get => _searchMinLongitude;
        set
        {
            if (SetProperty(ref _searchMinLongitude, value))
            {

            }
        }
    }

    private string _searchMaxLatitude = "";
    public string SearchMaxLatitude
    {
        get => _searchMaxLatitude;
        set
        {
            if (SetProperty(ref _searchMaxLatitude, value))
            {

            }
        }
    }

    private string _searchMaxLongitude = "";
    public string SearchMaxLongitude
    {
        get => _searchMaxLongitude;
        set
        {
            if (SetProperty(ref _searchMaxLongitude, value))
            {

            }
        }
    }

    private string _searchName = "";
    public string SearchName
    {
        get => _searchName;
        set
        {
            if (SetProperty(ref _searchName, value))
            {

            }
        }
    }

    #endregion Search

    #region Initialize

    [RelayCommand]
    private void Initialize()
    {
        try
        {
            if (Tree == ETreePicked.Kd)
            {
                Service.Initialize(Tree);
            }
            else
            {
                double minLatitude = double.Parse(MinLatitude);
                double minLongitude = double.Parse(MinLongitude);
                double maxLatitude = double.Parse(MaxLatitude);
                double maxLongitude = double.Parse(MaxLongitude);
                int maxDepth = int.Parse(MaxDepth);

                Service.Initialize(Tree, minLatitude, minLongitude, maxLatitude, maxLongitude, maxDepth);
            }

            Settings = false;

            GenerateMinLatitude = MinLatitude;
            GenerateMinLongitude = MinLongitude;
            GenerateMaxLatitude = MaxLatitude;
            GenerateMaxLongitude = MaxLongitude;

            Workspace = true;
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    #endregion Initialize

    #region Data commands

    [RelayCommand]
    private void Generate()
    {
        try
        {
            int count = int.Parse(GenerateCount);
            double minLatitude = double.Parse(GenerateMinLatitude);
            double minLongitude = double.Parse(GenerateMinLongitude);
            double maxLatitude = double.Parse(GenerateMaxLatitude);
            double maxLongitude = double.Parse(GenerateMaxLongitude);

            Service.Generate(count, minLatitude, minLongitude, maxLatitude, maxLongitude);
            FindAll();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    [RelayCommand]
    private void CsvImport()
    {
        try
        {
            var dialog = new OpenFileDialog
            {
                Title = "Load CSV",
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                Service.CsvImport(dialog.FileName);
                Clear();
                FindAll();
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    [RelayCommand]
    private void CsvExport()
    {
        try
        {
            var dialog = new SaveFileDialog
            {
                Title = "Save CSV",
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                DefaultExt = ".csv",
                AddExtension = true,
                FileName = "points.csv"
            };

            if (dialog.ShowDialog() == true)
            {
                Service.CsvExport(dialog.FileName);
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    [RelayCommand]
    private void Exit()
    {
        Points.Clear();
        Clear();

        Latitude = "";
        Longitude = "";
        Radius = "";
        SearchMinLatitude = "";
        SearchMinLongitude = "";
        SearchMaxLatitude = "";
        SearchMaxLongitude = "";
        SearchName = "";

        Workspace = false;
        Settings = true;
    }

    #endregion Data commands

    #region Point commands

    [RelayCommand]
    private void Insert()
    {
        try
        {
            var point = new PointOfInterestDto
            {
                Name = PointName,
                Description = PointDescription,
                Address = PointAddress,
                Latitude = double.Parse(PointLatitude),
                Longitude = double.Parse(PointLongitude)
            };

            var inserted = Service.Insert(point);

            Points.Add(inserted);
            SelectedPoint = inserted;
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    private bool HasPointId()
    {
        return !string.IsNullOrWhiteSpace(PointId);
    }

    [RelayCommand(CanExecute = nameof(HasPointId))]
    private void Update()
    {
        try
        {
            var pointNew = new PointOfInterestDto
            {
                Id = long.Parse(PointId),
                Name = PointName,
                Description = PointDescription,
                Address = PointAddress,
                Latitude = double.Parse(PointLatitude),
                Longitude = double.Parse(PointLongitude)
            };

            var updated = Service.Update(SelectedPoint!, pointNew);

            for (int i = 0; i < Points.Count; i++)
            {
                if (Points[i].Id == updated.Id)
                {
                    Points[i] = updated;
                    break;
                }
            }

            SelectedPoint = updated;
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    [RelayCommand(CanExecute = nameof(HasPointId))]
    private void Delete()
    {
        try
        {
            var deleted = Service.Delete(SelectedPoint!);

            for (int i = 0; i < Points.Count; i++)
            {
                if (Points[i].Id == deleted.Id)
                {
                    Points.RemoveAt(i);
                    break;
                }
            }

            Clear();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    [RelayCommand]
    private void Clear()
    {
        SelectedPoint = null;
        PointId = "";
        PointName = "";
        PointLatitude = "";
        PointLongitude = "";
        PointAddress = "";
        PointDescription = "";
    }

    #endregion Point commands

    #region Search commands

    [RelayCommand]
    private void FindAll()
    {
        try
        {
            var points = Service.FindAll();

            Points.Clear();

            foreach (var point in points)
            {
                Points.Add(point);
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    [RelayCommand]
    private void FindByCoordinates()
    {
        try
        {
            double latitude = double.Parse(Latitude);
            double longitude = double.Parse(Longitude);

            var points = Service.FindByCoordinates(latitude, longitude);

            Points.Clear();

            foreach (var point in points)
            {
                Points.Add(point);
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    [RelayCommand]
    private void FindByCircle()
    {
        try
        {
            double latitude = double.Parse(Latitude);
            double longitude = double.Parse(Longitude);
            double radius = double.Parse(Radius);

            var points = Service.FindByCircle(latitude, longitude, radius);

            Points.Clear();

            foreach (var point in points)
            {
                Points.Add(point);
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    [RelayCommand]
    private void FindByName()
    {
        try
        {
            double minLatitude = double.Parse(SearchMinLatitude);
            double minLongitude = double.Parse(SearchMinLongitude);
            double maxLatitude = double.Parse(SearchMaxLatitude);
            double maxLongitude = double.Parse(SearchMaxLongitude);

            var points = Service.FindByName(SearchName, minLatitude, minLongitude, maxLatitude, maxLongitude);

            Points.Clear();

            foreach (var point in points)
            {
                Points.Add(point);
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    #endregion Search commands
}