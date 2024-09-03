using Spectre.Console;

namespace Flashcards;
public class ExistingModelValidator<TKey, TModel> : IValidator
{
    public string ErrorMsg { get; set; } = string.Empty;
    public required Func<TKey, TModel> GetModel {  get; set; }
    public ValidationResult Validate(string input)
    {
        try
        {
            System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(TKey));
            if (converter != null && converter.CanConvertFrom(typeof(string)))
            {
                TKey key = (TKey)converter.ConvertFrom(input);

                if (GetModel(key) == null)
                {
                    return ValidationResult.Error($"[red]{ErrorMsg}[/]");
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