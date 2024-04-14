using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace ToDoApp.Model;

internal class DateTasksMemory : FileMemory<TasksCollection>
{
    private static readonly Regex FileNameRegex = 
        new Regex(@"(?<day>\d*)d(?<month>\d*)m(?<year>\d*)y", RegexOptions.Compiled);

    public DateOnly Date { get; }

    public static string ResolveFileName(DateOnly date)
    {
        return $"{date.Day}d{date.Month}m{date.Year}y.tasks";
    }

    public static bool TryResolveDate(string fileName, [NotNullWhen(true)] out DateOnly? date)
    {
        date = null;
        
        if (Path.GetExtension(fileName) != ".tasks")
            return false;

        var bareFileName = Path.GetFileNameWithoutExtension(fileName);
        var match = FileNameRegex.Match(bareFileName);
        
        if (!match.Success)
            return false;

        int day = int.Parse(match.Groups["day"].Value);
        int month = int.Parse(match.Groups["month"].Value);
        int year = int.Parse(match.Groups["year"].Value);

        date = new DateOnly(year, month, day);
        return true;
    }

    public DateTasksMemory(string filePath, DateOnly date, ISerializer<TasksCollection> serializer)
        : base(filePath, serializer)
    {
        Date = date;
    }
}