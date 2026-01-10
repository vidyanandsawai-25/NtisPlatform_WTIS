using Microsoft.AspNetCore.Mvc;
using NtisPlatform.Api.Extensions;
using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Application.Interfaces.WTIS;

namespace NtisPlatform.Api.Controllers.WTIS;

/// <summary>
/// Connection Category API - Standard CRUD operations
/// </summary>
[ApiController]
[Route("api/wtis/connection-category")]
public class ConnectionCategoryController : ControllerBase
{
    private readonly IConnectionCategoryService _service;
    private readonly ILogger<ConnectionCategoryController> _logger;

    public ConnectionCategoryController(IConnectionCategoryService service, ILogger<ConnectionCategoryController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all connection categories with filtering, sorting, and pagination
    /// </summary>
    [HttpGet]
    public Task<IActionResult> GetAll([FromQuery] ConnectionCategoryQueryParameters queryParams, CancellationToken ct)
        => this.ExecuteGetAllPaged(_service, queryParams, _logger, ct);

    /// <summary>
    /// Get connection category by ID
    /// </summary>
    [HttpGet("{id:int}")]
    public Task<IActionResult> GetById(int id, CancellationToken ct)
        => this.ExecuteGetById(_service, id, _logger, ct);

    /// <summary>
    /// Create new connection category
    /// </summary>
    [HttpPost]
    public Task<IActionResult> Create([FromBody] CreateConnectionCategoryDto createDto, CancellationToken ct)
        => this.ExecuteCreate(_service, createDto, _logger, ct);

    /// <summary>
    /// Update existing connection category
    /// </summary>
    [HttpPut("{id:int}")]
    public Task<IActionResult> Update(int id, [FromBody] UpdateConnectionCategoryDto updateDto, CancellationToken ct)
        => this.ExecuteUpdate(_service, id, updateDto, _logger, ct);

    /// <summary>
    /// Delete connection category
    /// </summary>
    [HttpDelete("{id:int}")]
    public Task<IActionResult> Delete(int id, CancellationToken ct)
        => this.ExecuteDelete(_service, id, _logger, ct);
}
