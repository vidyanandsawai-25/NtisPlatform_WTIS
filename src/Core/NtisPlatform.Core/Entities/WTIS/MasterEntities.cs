namespace NtisPlatform.Core.Entities.WTIS;

/// <summary>
/// Connection Type Master entity (maps to WTIS.ConnectionTypeMaster table)
/// </summary>
public class ConnectionTypeMasterEntity
{
    public int ConnectionTypeID { get; set; }
    public string ConnectionTypeName { get; set; } = string.Empty;
    public bool? IsActive { get; set; }
}

/// <summary>
/// Connection Category Master entity (maps to WTIS.ConnectionCategoryMaster table)
/// </summary>
public class ConnectionCategoryMasterEntity
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public bool? IsActive { get; set; }
}

/// <summary>
/// Pipe Size Master entity (maps to WTIS.PipeSizeMaster table)
/// </summary>
public class PipeSizeMasterEntity
{
    public int PipeSizeID { get; set; }
    public string SizeName { get; set; } = string.Empty;
    public bool? IsActive { get; set; }
}
