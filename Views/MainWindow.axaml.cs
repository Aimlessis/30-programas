using Avalonia.Controls;
using AvaloniaEjercicios.Models;
using AvaloniaEjercicios.ViewModels;

namespace AvaloniaEjercicios.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContextChanged += (_, _) => BuildMenu();
    }

    private void BuildMenu()
    {
        if (DataContext is not MainWindowViewModel vm) return;

        EjerciciosMenu.Items.Clear();
        foreach (IExercise exercise in vm.Exercises)
        {
            var item = new MenuItem { Header = exercise.MenuHeader };
            item.Click += (_, _) => vm.SelectExerciseCommand.Execute(exercise);
            EjerciciosMenu.Items.Add(item);
        }
    }
}
