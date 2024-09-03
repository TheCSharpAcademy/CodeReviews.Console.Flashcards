namespace CodingTracker;

public static class EnumHelper
{
    public static string GetTitle<T>(T value) where T : Enum
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);

        if (name != null)
        {
            var field = type.GetField(name);
            var attr = Attribute.GetCustomAttribute(field, typeof(TitleAttribute)) as TitleAttribute;
            if (attr != null)
            {
                return attr.Title;
            }
        }

        return "Unknown";
    }
}