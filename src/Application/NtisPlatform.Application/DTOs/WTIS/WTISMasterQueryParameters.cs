using NtisPlatform.Application.DTOs.Queries;

namespace NtisPlatform.Application.DTOs.WTIS;

/// <summary>
/// Query parameters for Connection Type master
/// </summary>
public class ConnectionTypeQueryParameters : BaseQueryParameters
{
    public string? ConnectionTypeName { get; set; }
    public bool? IsActive { get; set; }
}

/// <summary>
/// Query parameters for Connection Category master
/// </summary>
public class ConnectionCategoryQueryParameters : BaseQueryParameters
{
    public string? CategoryName { get; set; }
    public bool? IsActive { get; set; }
}

/// <summary>
/// Query parameters for Pipe Size master
/// </summary>
public class PipeSizeQueryParameters : BaseQueryParameters
{
    public string? SizeName { get; set; }
    public bool? IsActive { get; set; }
}
