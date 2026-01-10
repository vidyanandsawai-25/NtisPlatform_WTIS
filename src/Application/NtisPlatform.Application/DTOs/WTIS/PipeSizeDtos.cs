using System.ComponentModel.DataAnnotations;

namespace NtisPlatform.Application.DTOs.WTIS;

/// <summary>
/// Pipe Size DTO (response)
/// </summary>
public class PipeSizeDto
{
    public int PipeSizeID { get; set; }
    public string SizeName { get; set; } = string.Empty;
    public bool? IsActive { get; set; }
}

/// <summary>
/// Create Pipe Size DTO
/// </summary>
public class CreatePipeSizeDto
{
    [Required(ErrorMessage = "Size Name is required")]
    [StringLength(50, ErrorMessage = "Size Name cannot exceed 50 characters")]
    public string SizeName { get; set; } = string.Empty;

    public bool? IsActive { get; set; } = true;
}

/// <summary>
/// Update Pipe Size DTO
/// </summary>
public class UpdatePipeSizeDto
{
    [StringLength(50, ErrorMessage = "Size Name cannot exceed 50 characters")]
    public string? SizeName { get; set; }

    public bool? IsActive { get; set; }
}
