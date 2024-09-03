using System.ComponentModel;
using Spectre.Console;

namespace Flashcards;
public class ExistingModelValidator<TKey, TModel> : IValidator
{
    public string errorMsg { get; set; } = string.Empty;
    public required Func<TKey, TModel> GetModel {  get; set; }
    public ValidationResult Validate(string input)
    {
        try
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(TKey));
            if (converter != null && converter.CanConvertFrom(typeof(string)))
            {
                TKey key = (TKey)converter.ConvertFrom(input);

                if (GetModel(key) == null)
                {
                    return ValidationResult.Error($"[red]{errorMsg}[/]");
                }
                return ValidationResult.Success();
            }
            else
            {
                return ValidationResult.Error("[red]Input cannot be converted to the required key type.[/]");
            }
        }
        catch (Exception ex)
        {
            return ValidationResult.Error("[red]Failed to convert input.[/]");
        }
    }
}