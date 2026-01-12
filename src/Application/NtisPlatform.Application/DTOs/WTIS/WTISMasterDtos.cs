using System.ComponentModel.DataAnnotations;

namespace NtisPlatform.Application.DTOs.WTIS;

#region ConnectionType Master

public class ConnectionTypeDto
{
    public int ConnectionTypeID { get; set; }
    public string ConnectionTypeName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public class CreateConnectionTypeDto
{
    [Required]
    [StringLength(100)]
    public string ConnectionTypeName { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    [Required]
    public int CreatedBy { get; set; }
}

public class UpdateConnectionTypeDto
{
    [StringLength(100)]
    public string? ConnectionTypeName { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    [Required]
    public int UpdatedBy { get; set; }
}

#endregion

#region ConnectionCategory Master

public class ConnectionCategoryDto
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public class CreateConnectionCategoryDto
{
    [Required]
    [StringLength(100)]
    public string CategoryName { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    [Required]
    public int CreatedBy { get; set; }
}

public class UpdateConnectionCategoryDto
{
    [StringLength(100)]
    public string? CategoryName { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    [Required]
    public int UpdatedBy { get; set; }
}

#endregion

#region PipeSize Master

public class PipeSizeDto
{
    public int PipeSizeID { get; set; }
    public string SizeName { get; set; } = string.Empty;
    public decimal DiameterMM { get; set; }
    public bool IsActive { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public class CreatePipeSizeDto
{
    [Required]
    [StringLength(50)]
    public string SizeName { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 1000)]
    public decimal DiameterMM { get; set; }

    public bool IsActive { get; set; } = true;

    [Required]
    public int CreatedBy { get; set; }
}

public class UpdatePipeSizeDto
{
    [StringLength(50)]
    public string? SizeName { get; set; }

    [Range(0.01, 1000)]
    public decimal? DiameterMM { get; set; }

    public bool? IsActive { get; set; }

    [Required]
    public int UpdatedBy { get; set; }
}

#endregion

#region ZoneMaster

public class ZoneMasterDto
{
    public int ZoneID { get; set; }
    public string ZoneName { get; set; } = string.Empty;
    public string ZoneCode { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public class CreateZoneMasterDto
{
    [Required]
    [StringLength(100)]
    public string ZoneName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string ZoneCode { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    [Required]
    public int CreatedBy { get; set; }
}

public class UpdateZoneMasterDto
{
    [StringLength(100)]
    public string? ZoneName { get; set; }

    [StringLength(50)]
    public string? ZoneCode { get; set; }

    public bool? IsActive { get; set; }

    [Required]
    public int UpdatedBy { get; set; }
}

#endregion

#region WardMaster

public class WardMasterDto
{
    public int WardID { get; set; }
    public string WardName { get; set; } = string.Empty;
    public string WardCode { get; set; } = string.Empty;
    public int ZoneID { get; set; }
    public string? ZoneName { get; set; }
    public bool IsActive { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public class CreateWardMasterDto
{
    [Required]
    [StringLength(100)]
    public string WardName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string WardCode { get; set; } = string.Empty;

    [Required]
    public int ZoneID { get; set; }

    public bool IsActive { get; set; } = true;

    [Required]
    public int CreatedBy { get; set; }
}

public class UpdateWardMasterDto
{
    [StringLength(100)]
    public string? WardName { get; set; }

    [StringLength(50)]
    public string? WardCode { get; set; }

    public int? ZoneID { get; set; }
    public bool? IsActive { get; set; }

    [Required]
    public int UpdatedBy { get; set; }
}

#endregion

#region RateMaster

/// <summary>
/// Rate Master DTO with enriched data from all related master tables
/// </summary>
public class RateMasterDto
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

public class CreateRateMasterDto
{
    [Required]
    public int ZoneID { get; set; }

    [Required]
    public int WardID { get; set; }

    [Required]
    public int TapSizeID { get; set; }

    [Required]
    public int ConnectionTypeID { get; set; }

    [Required]
    public int ConnectionCategoryID { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal MinReading { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal MaxReading { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal PerLiter { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal MinimumCharge { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal MeterOffPenalty { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Rate { get; set; }

    [Required]
    [Range(1900, 9999)]
    public int Year { get; set; }

    [StringLength(500)]
    public string? Remark { get; set; }

    public bool IsActive { get; set; } = true;

    [Required]
    public int CreatedBy { get; set; }
}

public class UpdateRateMasterDto
{
    public int? ZoneID { get; set; }
    public int? WardID { get; set; }
    public int? TapSizeID { get; set; }
    public int? ConnectionTypeID { get; set; }
    public int? ConnectionCategoryID { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? MinReading { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? MaxReading { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? PerLiter { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? MinimumCharge { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? MeterOffPenalty { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? Rate { get; set; }

    [Range(1900, 9999)]
    public int? Year { get; set; }

    [StringLength(500)]
    public string? Remark { get; set; }

    public bool? IsActive { get; set; }

    [Required]
    public int UpdatedBy { get; set; }
}

#endregion
