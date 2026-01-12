using Microsoft.AspNetCore.Mvc;
using NtisPlatform.Api.Extensions;
using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Application.Interfaces.WTIS;

namespace NtisPlatform.Api.Controllers.WTIS;

/// <summary>
/// Rate Master - Returns enriched data with all master table JOINs (Zone, Ward, PipeSize, ConnectionType, Category)
/// </summary>
[ApiController]
[Route("api/wtis")]
public class RateMasterController : ControllerBase
{
    private readonly IRateMasterService _service;
    private readonly ILogger<RateMasterController> _logger;

    public RateMasterController(IRateMasterService service, ILogger<RateMasterController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public Task<IActionResult> GetAll([FromQuery] RateMasterQueryParameters queryParams, CancellationToken ct)
        => this.ExecuteGetAllEnriched(_service.GetAllAsync, queryParams, _logger, ct);

    [HttpGet("{id:int}")]
    public Task<IActionResult> GetById(int id, CancellationToken ct)
        => this.ExecuteGetByIdEnriched(_service.GetByIdAsync, id, _logger, ct);

    [HttpPost]
    public Task<IActionResult> Create([FromBody] CreateRateMasterDto createDto, CancellationToken ct)
        => this.ExecuteCreateEnriched(_service.CreateAsync, createDto, _logger, ct);

    [HttpPut("{id:int}")]
    public Task<IActionResult> Update(int id, [FromBody] UpdateRateMasterDto updateDto, CancellationToken ct)
        => this.ExecuteUpdateEnriched(_service.UpdateAsync, id, updateDto, _logger, ct);

    [HttpDelete("{id:int}")]
    public Task<IActionResult> Delete(int id, CancellationToken ct)
        => this.ExecuteDeleteEnriched<RateMasterDto>(_service.DeleteAsync, id, _logger, ct);
}
