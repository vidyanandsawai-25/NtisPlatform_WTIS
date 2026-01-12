namespace NtisPlatform.Core.Entities.WTIS;

public class ConnectionTypeMasterEntity
{
    public int ConnectionTypeID { get; set; }
    public string ConnectionTypeName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public class ConnectionCategoryMasterEntity
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public class PipeSizeMasterEntity
{
    public int PipeSizeID { get; set; }
    public string SizeName { get; set; } = string.Empty;
    public decimal DiameterMM { get; set; }
    public bool IsActive { get; set; } = true;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public class ZoneMasterEntity
{
    public int ZoneID { get; set; }
    public string ZoneName { get; set; } = string.Empty;
    public string ZoneCode { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public class WardMasterEntity
{
    public int WardID { get; set; }
    public string WardName { get; set; } = string.Empty;
    public string WardCode { get; set; } = string.Empty;
    public int ZoneID { get; set; }
    public bool IsActive { get; set; } = true;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public class RateMasterEntity
{
    public int RateID { get; set; }
    public int ZoneID { get; set; }
    public int WardID { get; set; }
    public int TapSizeID { get; set; }
    public int ConnectionTypeID { get; set; }
    public int ConnectionCategoryID { get; set; }
    public decimal MinReading { get; set; }
    public decimal MaxReading { get; set; }
    public decimal PerLiter { get; set; }
    public decimal MinimumCharge { get; set; }
    public decimal MeterOffPenalty { get; set; }
    public decimal Rate { get; set; }
    public int Year { get; set; }
    public string? Remark { get; set; }
    public bool IsActive { get; set; } = true;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

/// <summary>
/// Rate Master with enriched data from related master tables
/// </summary>
public class RateMasterWithJoins
{
    public int RateID { get; set; }
    public int ZoneID { get; set; }
    public string? ZoneName { get; set; }
    public string? ZoneCode { get; set; }
    public int WardID { get; set; }
    public string? WardName { get; set; }
    public string? WardCode { get; set; }
    public int TapSizeID { get; set; }
    public string? TapSize { get; set; }
    public decimal? DiameterMM { get; set; }
    public int ConnectionTypeID { get; set; }
    public string? ConnectionTypeName { get; set; }
    public int ConnectionCategoryID { get; set; }
    public string? CategoryName { get; set; }
    public decimal MinReading { get; set; }
    public decimal MaxReading { get; set; }
    public decimal PerLiter { get; set; }
    public decimal MinimumCharge { get; set; }
    public decimal MeterOffPenalty { get; set; }
    public decimal Rate { get; set; }
    public int Year { get; set; }
    public string? Remark { get; set; }
    public bool IsActive { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
