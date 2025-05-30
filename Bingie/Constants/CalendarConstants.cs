namespace Bingie.Constants;

/// <summary>
/// Constants used throughout the calendar functionality
/// </summary>
public static class CalendarConstants
{
    // UI Constants
    public const int DayButtonCornerRadius = 20;
    public const int DayButtonHeight = 40;
    public const int DayButtonWidth = 40;
    public const int DaysPerWeek = 7;
    
    // Colors
    public const string HasBingeEntryColor = "#FF6B6B";
    public const string NormalDayColor = "#E0E0E0";
    public const string HasBingeEntryTextColor = "#FFFFFF";
    public const string NormalDayTextColor = "#333333";
    
    // Date Formats
    public const string MonthYearFormat = "MMMM yyyy";
    
    // Navigation
    public const int MonthNavigationStep = 1; // Navigate by months, not weeks
}