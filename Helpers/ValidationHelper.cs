using System.ComponentModel.DataAnnotations;

namespace TodoApi.Helpers;
public static class ValidationHelper
{
    public static Dictionary<string, string[]>? ValidateModel<T>(T model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();

        if (!Validator.TryValidateObject(
            model,
            validationContext,
            validationResults,
            validateAllProperties: true))
        {
            return validationResults
                .GroupBy(v => v.MemberNames.FirstOrDefault() ?? "")
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(v => v.ErrorMessage ?? "").ToArray()
                );
        }
        return null;
    }
}
