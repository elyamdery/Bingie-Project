using System;
using Microsoft.Maui.Controls;

namespace Bingie.Views.Components;

public partial class AvatarBodyView : ContentView
{
    public static readonly BindableProperty BodyScoreProperty =
        BindableProperty.Create(nameof(BodyScore), typeof(double), typeof(AvatarBodyView), 0.5d, propertyChanged: OnBodyScoreChanged);

    public static readonly BindableProperty MoodCopyProperty =
        BindableProperty.Create(nameof(MoodCopy), typeof(string), typeof(AvatarBodyView), string.Empty, propertyChanged: OnMoodCopyChanged);

    public AvatarBodyView()
    {
        InitializeComponent();
        UpdateBody(BodyScore);
        UpdateCopy(MoodCopy);
    }

    public double BodyScore
    {
        get => (double)GetValue(BodyScoreProperty);
        set => SetValue(BodyScoreProperty, value);
    }

    public string MoodCopy
    {
        get => (string)GetValue(MoodCopyProperty);
        set => SetValue(MoodCopyProperty, value);
    }

    private static void OnBodyScoreChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AvatarBodyView view && newValue is double value)
        {
            view.UpdateBody(value);
        }
    }

    private static void OnMoodCopyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AvatarBodyView view && newValue is string text)
        {
            view.UpdateCopy(text);
        }
    }

    private void UpdateBody(double score)
    {
        var clamped = Math.Clamp(score, 0.2, 1.2);
        BodyEllipse.WidthRequest = 120 * clamped;
        BodyEllipse.HeightRequest = 120 * clamped;
        GlowEllipse.WidthRequest = 140 * clamped;
        GlowEllipse.Opacity = 0.3 + (1 - clamped) * 0.2;
        BodyEllipse.Fill = clamped < 0.6
            ? Color.FromArgb("#FBD1D1")
            : Color.FromArgb("#A0CFFF");
    }

    private void UpdateCopy(string copy)
    {
        var lines = copy?.Split('|', StringSplitOptions.TrimEntries) ?? Array.Empty<string>();
        MoodLabel.Text = lines.Length > 0 ? lines[0] : "Checking in";
        DetailLabel.Text = lines.Length > 1 ? lines[1] : "Keep caring for yourself.";
    }
}
