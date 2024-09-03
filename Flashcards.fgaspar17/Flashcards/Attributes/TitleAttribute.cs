namespace CodingTracker;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class TitleAttribute : Attribute
{
    public string Title { get; set; }

    public TitleAttribute(string title)
    {
        Title = title;
    }
}