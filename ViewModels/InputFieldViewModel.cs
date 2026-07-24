namespace AvaloniaEjercicios.ViewModels;

public class InputFieldViewModel : ViewModelBase
{
    private string _value = string.Empty;

    public string Label { get; }
    public string Placeholder { get; }

    public string Value
    {
        get => _value;
        set => SetField(ref _value, value);
    }

    public InputFieldViewModel(string label, string placeholder)
    {
        Label = label;
        Placeholder = placeholder;
    }
}
