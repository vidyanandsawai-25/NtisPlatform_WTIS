using System.ComponentModel.DataAnnotations;
namespace NtisPlatform.Application.DTOs;

public class ConstructionTypeDto
{
    public string ConstructionId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DescriptionEnglish { get; set; } = string.Empty;
    public string GroupID { get; set; } = string.Empty;
    public string KeyboardShortCutKey { get; set; } = string.Empty;
    public int? KeyWiseSequence { get; set; }

    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public class CreateConstructionTypeDto
{
    [Required(
        ErrorMessageResourceType = typeof(NtisPlatform.Application.Resources.ValidationMessages),
        ErrorMessageResourceName = "ConstructionId_Required")]
    [StringLength(7,
        ErrorMessageResourceType = typeof(NtisPlatform.Application.Resources.ValidationMessages),
        ErrorMessageResourceName = "ConstructionId_MaxLen_7")]
    public string ConstructionId { get; set; } = string.Empty;

    [StringLength(100,
        ErrorMessageResourceType = typeof(NtisPlatform.Application.Resources.ValidationMessages),
        ErrorMessageResourceName = "Construction_Description_MaxLen_100")]
    public string Description { get; set; } = string.Empty;

    [StringLength(100,
        ErrorMessageResourceType = typeof(NtisPlatform.Application.Resources.ValidationMessages),
        ErrorMessageResourceName = "Construction_DescriptionEnglish_MaxLen_100")]
    public string DescriptionEnglish { get; set; } = string.Empty;

    [StringLength(50,
        ErrorMessageResourceType = typeof(NtisPlatform.Application.Resources.ValidationMessages),
        ErrorMessageResourceName = "Construction_GroupID_MaxLen_50")]
    public string GroupID { get; set; } = string.Empty;

    [StringLength(20,
        ErrorMessageResourceType = typeof(NtisPlatform.Application.Resources.ValidationMessages),
        ErrorMessageResourceName = "Construction_KeyboardShortCutKey_MaxLen_20")]
    public string KeyboardShortCutKey { get; set; } = string.Empty;

    public int? KeyWiseSequence { get; set; }
    public int? CreatedBy { get; set; }
}

public class UpdateConstructionTypeDto
{
    [Required(
        ErrorMessageResourceType = typeof(NtisPlatform.Application.Resources.ValidationMessages),
        ErrorMessageResourceName = "ConstructionId_Required")]
    [StringLength(7,
        ErrorMessageResourceType = typeof(NtisPlatform.Application.Resources.ValidationMessages),
        ErrorMessageResourceName = "ConstructionId_MaxLen_7")]
    public string ConstructionId { get; set; } = string.Empty;

    [StringLength(100,
        ErrorMessageResourceType = typeof(NtisPlatform.Application.Resources.ValidationMessages),
        ErrorMessageResourceName = "Construction_Description_MaxLen_100")]
    public string Description { get; set; } = string.Empty;

    [StringLength(100,
        ErrorMessageResourceType = typeof(NtisPlatform.Application.Resources.ValidationMessages),
        ErrorMessageResourceName = "Construction_DescriptionEnglish_MaxLen_100")]
    public string DescriptionEnglish { get; set; } = string.Empty;

    [StringLength(50,
        ErrorMessageResourceType = typeof(NtisPlatform.Application.Resources.ValidationMessages),
        ErrorMessageResourceName = "Construction_GroupID_MaxLen_50")]
    public string GroupID { get; set; } = string.Empty;

    [StringLength(20,
        ErrorMessageResourceType = typeof(NtisPlatform.Application.Resources.ValidationMessages),
        ErrorMessageResourceName = "Construction_KeyboardShortCutKey_MaxLen_20")]
    public string KeyboardShortCutKey { get; set; } = string.Empty;

    public int? KeyWiseSequence { get; set; }
    public int? UpdatedBy { get; set; }
}

