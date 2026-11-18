using System.Windows;

namespace DataStructuresAssignment.PointOfInterestManagerUI;

public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}