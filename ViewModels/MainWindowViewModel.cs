using System.Collections.ObjectModel;
using AvaloniaEjercicios.Data;
using AvaloniaEjercicios.Models;

namespace AvaloniaEjercicios.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<IExercise> Exercises { get; }

    private ExerciseViewModel? _currentExercise;
    public ExerciseViewModel? CurrentExercise
    {
        get => _currentExercise;
        private set => SetField(ref _currentExercise, value);
    }

    public RelayCommand<IExercise> SelectExerciseCommand { get; }

    public MainWindowViewModel()
    {
        Exercises = new ObservableCollection<IExercise>(ExerciseCatalog.Exercises);
        SelectExerciseCommand = new RelayCommand<IExercise>(SelectExercise);

        // Show the first exercise by default so the window isn't empty on launch.
        if (Exercises.Count > 0)
            SelectExercise(Exercises[0]);
    }

    private void SelectExercise(IExercise? exercise)
    {
        if (exercise is null) return;
        CurrentExercise = new ExerciseViewModel(exercise);
    }
}
