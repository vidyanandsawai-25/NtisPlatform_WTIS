using System.ComponentModel.DataAnnotations;

namespace NtisPlatform.Application.DTOs.WTIS;

/// <summary>
/// Connection Type DTO (response)
/// </summary>
public class ConnectionTypeDto
{
    public int ConnectionTypeID { get; set; }
    public string ConnectionTypeName { get; set; } = string.Empty;
    public bool? IsActive { get; set; }
}

/// <summary>
/// Create Connection Type DTO
/// </summary>
public class CreateConnectionTypeDto
{
    [Required(ErrorMessage = "Connection Type Name is required")]
    [StringLength(100, ErrorMessage = "Connection Type Name cannot exceed 100 characters")]
    public string ConnectionTypeName { get; set; } = string.Empty;

    public bool? IsActive { get; set; } = true;
}

/// <summary>
/// Update Connection Type DTO
/// </summary>
public class UpdateConnectionTypeDto
{
    [StringLength(100, ErrorMessage = "Connection Type Name cannot exceed 100 characters")]
    public string? ConnectionTypeName { get; set; }

    public bool? IsActive { get; set; }
}
