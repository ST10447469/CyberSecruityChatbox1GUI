using System.Globalization;
using System.Windows.Data;

namespace CybersecurityChatbotGUI.Converters;

public sealed class DateTimeToReminderStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (value is not DateTime date)
            return "No reminder set";

        culture ??= CultureInfo.CurrentCulture;
        var today = DateTime.Today;

        return date.Date switch
        {
            _ when date.Date == today => "Today",
            _ when date.Date == today.AddDays(1) => "Tomorrow",
            _ when date.Date == today.AddDays(-1) => "Yesterday",
            _ when date.Year == today.Year => date.ToString("d MMM", culture),
            _ => date.ToString("d MMM yyyy", culture)
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
        => throw new NotSupportedException("One-way conversion only");
}