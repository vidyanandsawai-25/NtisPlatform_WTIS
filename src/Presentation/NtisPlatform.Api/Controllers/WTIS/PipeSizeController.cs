using Microsoft.AspNetCore.Mvc;
using NtisPlatform.Api.Extensions;
using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Application.Interfaces.WTIS;

namespace NtisPlatform.Api.Controllers.WTIS;

/// <summary>
/// Pipe Size API - Standard CRUD operations
/// </summary>
[ApiController]
[Route("api/wtis/pipe-size")]
public class PipeSizeController : ControllerBase
{
    private readonly IPipeSizeService _service;
    private readonly ILogger<PipeSizeController> _logger;

    public PipeSizeController(IPipeSizeService service, ILogger<PipeSizeController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all pipe sizes with filtering, sorting, and pagination
    /// </summary>
    [HttpGet]
    public Task<IActionResult> GetAll([FromQuery] PipeSizeQueryParameters queryParams, CancellationToken ct)
        => this.ExecuteGetAllPaged(_service, queryParams, _logger, ct);

    /// <summary>
    /// Get pipe size by ID
    /// </summary>
    [HttpGet("{id:int}")]
    public Task<IActionResult> GetById(int id, CancellationToken ct)
        => this.ExecuteGetById(_service, id, _logger, ct);

    /// <summary>
    /// Create new pipe size
    /// </summary>
    [HttpPost]
    public Task<IActionResult> Create([FromBody] CreatePipeSizeDto createDto, CancellationToken ct)
        => this.ExecuteCreate(_service, createDto, _logger, ct);

    /// <summary>
    /// Update existing pipe size
    /// </summary>
    [HttpPut("{id:int}")]
    public Task<IActionResult> Update(int id, [FromBody] UpdatePipeSizeDto updateDto, CancellationToken ct)
        => this.ExecuteUpdate(_service, id, updateDto, _logger, ct);

    /// <summary>
    /// Delete pipe size
    /// </summary>
    [HttpDelete("{id:int}")]
    public Task<IActionResult> Delete(int id, CancellationToken ct)
        => this.ExecuteDelete(_service, id, _logger, ct);
}
