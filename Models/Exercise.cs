using System;

namespace AvaloniaEjercicios.Models;

/// <summary>
/// Represents one of the 28 exercises. Each exercise declares the input
/// fields it needs (rendered dynamically) and a Compute function that turns
/// the raw text the user typed into a formatted result string.
/// </summary>
public interface IExercise
{
    string Id { get; }
    string Title { get; }
    string Description { get; }
    string[] InputLabels { get; }
    string[] Placeholders { get; }
    Func<string[], string> Compute { get; }
    string MenuHeader { get; }
}

public class Exercise : IExercise
{
    public string Id { get; }
    public string Title { get; }
    public string Description { get; }
    public string[] InputLabels { get; }
    public string[] Placeholders { get; }
    public Func<string[], string> Compute { get; }

    public Exercise(
        string id,
        string title,
        string description,
        string[] inputLabels,
        Func<string[], string> compute,
        string[]? placeholders = null)
    {
        Id = id;
        Title = title;
        Description = description;
        InputLabels = inputLabels;
        Compute = compute;
        Placeholders = placeholders ?? new string[inputLabels.Length];
    }

    public string MenuHeader => $"{Id} — {Title}";

    public override string ToString() => MenuHeader;
}
