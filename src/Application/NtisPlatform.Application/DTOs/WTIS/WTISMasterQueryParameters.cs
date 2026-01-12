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

/// <summary>
/// Query parameters for Zone Master
/// </summary>
public class ZoneMasterQueryParameters : BaseQueryParameters
{
    public string? ZoneName { get; set; }
    public bool? IsActive { get; set; }
}

/// <summary>
/// Query parameters for Ward Master
/// </summary>
public class WardMasterQueryParameters : BaseQueryParameters
{
    public string? WardName { get; set; }
    public bool? IsActive { get; set; }
}

/// <summary>
/// Query parameters for Rate Master
/// </summary>
public class RateMasterQueryParameters : BaseQueryParameters
{
    public int? ZoneID { get; set; }
    public int? WardID { get; set; }
    public int? TapSizeID { get; set; }
    public int? ConnectionTypeID { get; set; }
    public int? ConnectionCategoryID { get; set; }
    public int? Year { get; set; }
    public bool? IsActive { get; set; }
}
