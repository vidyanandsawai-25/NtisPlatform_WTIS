using System.ComponentModel.DataAnnotations;

namespace NtisPlatform.Application.DTOs.WTIS;

/// <summary>
/// Connection Category DTO (response)
/// </summary>
public class ConnectionCategoryDto
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public bool? IsActive { get; set; }
}

/// <summary>
/// Create Connection Category DTO
/// </summary>
public class CreateConnectionCategoryDto
{
    [Required(ErrorMessage = "Category Name is required")]
    [StringLength(100, ErrorMessage = "Category Name cannot exceed 100 characters")]
    public string CategoryName { get; set; } = string.Empty;

    public bool? IsActive { get; set; } = true;
}

/// <summary>
/// Update Connection Category DTO
/// </summary>
public class UpdateConnectionCategoryDto
{
    [StringLength(100, ErrorMessage = "Category Name cannot exceed 100 characters")]
    public string? CategoryName { get; set; }

    public bool? IsActive { get; set; }
}
