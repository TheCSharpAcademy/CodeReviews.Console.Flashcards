using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FlashCards.Utilities;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum value)
    {
        FieldInfo field = value.GetType().GetField(value.ToString());
        DisplayAttribute attribute = field.GetCustomAttribute<DisplayAttribute>();
        return attribute == null ? value.ToString() : attribute.Name;
    }
}