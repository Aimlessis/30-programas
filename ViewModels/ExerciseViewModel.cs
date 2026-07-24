using System;
using System.Collections.ObjectModel;
using System.Linq;
using AvaloniaEjercicios.Models;

namespace AvaloniaEjercicios.ViewModels;

public class ExerciseViewModel : ViewModelBase
{
    private readonly IExercise _exercise;
    private string _result = string.Empty;
    private bool _hasError;

    public string Title => _exercise.MenuHeader;
    public string Description => _exercise.Description;

    public ObservableCollection<InputFieldViewModel> Inputs { get; }

    public string Result
    {
        get => _result;
        private set => SetField(ref _result, value);
    }

    public bool HasError
    {
        get => _hasError;
        private set => SetField(ref _hasError, value);
    }

    public RelayCommand CalculateCommand { get; }

    public ExerciseViewModel(IExercise exercise)
    {
        _exercise = exercise;
        Inputs = new ObservableCollection<InputFieldViewModel>(
            exercise.InputLabels.Select((label, i) =>
                new InputFieldViewModel(label, exercise.Placeholders.ElementAtOrDefault(i) ?? string.Empty)));
        CalculateCommand = new RelayCommand(Calculate);
        Result = "Completa los campos y presiona \"Calcular\".";
    }

    private void Calculate()
    {
        try
        {
            var values = Inputs.Select(i => i.Value).ToArray();
            Result = _exercise.Compute(values);
            HasError = false;
        }
        catch (Exception ex)
        {
            Result = $"⚠ {ex.Message}";
            HasError = true;
        }
    }
}
