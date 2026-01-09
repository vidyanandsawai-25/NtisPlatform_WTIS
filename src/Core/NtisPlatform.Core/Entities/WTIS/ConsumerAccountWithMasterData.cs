namespace NtisPlatform.Core.Entities.WTIS;

/// <summary>
/// Consumer Account with master table data (result from SQL JOINs - read-only)
/// </summary>
public class ConsumerAccountWithMasterData
{
    // Identity
    public int ConsumerID { get; set; }
    public string ConsumerNumber { get; set; } = string.Empty;
    public string? OldConsumerNumber { get; set; }

    // Location hierarchy
    public string? ZoneNo { get; set; }
    public string? WardNo { get; set; }
    public string? PropertyNumber { get; set; }
    public string? PartitionNumber { get; set; }

    // Consumer details
    public string ConsumerName { get; set; } = string.Empty;
    public string? ConsumerNameEnglish { get; set; }
    public string? MobileNumber { get; set; }
    public string? EmailID { get; set; }
    public string? Address { get; set; }
    public string? AddressEnglish { get; set; }

    // Connection details (master table IDs)
    public int ConnectionTypeID { get; set; }
    public int CategoryID { get; set; }
    public int PipeSizeID { get; set; }

    // Master table names (from SQL LEFT JOINs)
    public string? ConnectionTypeName { get; set; }
    public string? CategoryName { get; set; }
    public string? PipeSize { get; set; }

    // Additional info
    public DateTime? ConnectionDate { get; set; }
    public bool? IsActive { get; set; }
    public string? Remark { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
