using System.ComponentModel.DataAnnotations;

namespace CivicHub.Api.Options;

public record LocalizationOptions
{
    public const string SectionName = "Localization";
    
    [Required(ErrorMessage = "DefaultCulture is required")]
    public string DefaultCulture { get; set; }

    [Required(ErrorMessage = "SupportedCultures is required")]
    public List<string> SupportedCultures { get; set; } = [];
};