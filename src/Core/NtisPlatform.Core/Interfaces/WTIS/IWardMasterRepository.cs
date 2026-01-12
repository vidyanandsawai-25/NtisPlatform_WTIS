using NtisPlatform.Core.Entities.WTIS;

namespace NtisPlatform.Core.Interfaces.WTIS;

/// <summary>
/// Repository interface for Ward Master with Zone information
/// </summary>
public interface IWardMasterRepository : IRepository<WardMasterEntity, int>
{
    /// <summary>
    /// Get all wards with zone information
    /// </summary>
    Task<IEnumerable<WardMasterWithZone>> GetAllWithZoneAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get ward by ID with zone information
    /// </summary>
    Task<WardMasterWithZone?> GetByIdWithZoneAsync(int id, CancellationToken cancellationToken = default);
}

/// <summary>
/// Ward Master with Zone information (for display)
/// </summary>
public class WardMasterWithZone
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
