using Microsoft.AspNetCore.Mvc;
using NtisPlatform.Api.Extensions;
using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Application.Interfaces.WTIS;

namespace NtisPlatform.Api.Controllers.WTIS;

/// <summary>
/// Ward Master - Returns enriched data with Zone information via JOINs
/// </summary>
[ApiController]
[Route("api/wtis/ward-master")]
public class WardMasterController : ControllerBase
{
    private readonly IWardMasterService _service;
    private readonly ILogger<WardMasterController> _logger;

    public WardMasterController(IWardMasterService service, ILogger<WardMasterController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public Task<IActionResult> GetAll([FromQuery] WardMasterQueryParameters queryParams, CancellationToken ct)
        => this.ExecuteGetAllEnriched(_service.GetAllAsync, queryParams, _logger, ct);

    [HttpGet("{id:int}")]
    public Task<IActionResult> GetById(int id, CancellationToken ct)
        => this.ExecuteGetByIdEnriched(_service.GetByIdAsync, id, _logger, ct);

    [HttpPost]
    public Task<IActionResult> Create([FromBody] CreateWardMasterDto createDto, CancellationToken ct)
        => this.ExecuteCreateEnriched(_service.CreateAsync, createDto, _logger, ct);

    [HttpPut("{id:int}")]
    public Task<IActionResult> Update(int id, [FromBody] UpdateWardMasterDto updateDto, CancellationToken ct)
        => this.ExecuteUpdateEnriched(_service.UpdateAsync, id, updateDto, _logger, ct);

    [HttpDelete("{id:int}")]
    public Task<IActionResult> Delete(int id, CancellationToken ct)
        => this.ExecuteDeleteEnriched<WardMasterDto>(_service.DeleteAsync, id, _logger, ct);
}
