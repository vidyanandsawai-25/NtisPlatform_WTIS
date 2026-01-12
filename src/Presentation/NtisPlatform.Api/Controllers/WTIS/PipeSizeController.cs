using Microsoft.AspNetCore.Mvc;
using NtisPlatform.Api.Extensions;
using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Application.Interfaces.WTIS;

namespace NtisPlatform.Api.Controllers.WTIS;

[ApiController]
[Route("api/wtis")]
public class PipeSizeController : ControllerBase
{
    private readonly IPipeSizeService _service;
    private readonly ILogger<PipeSizeController> _logger;

    public PipeSizeController(IPipeSizeService service, ILogger<PipeSizeController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public Task<IActionResult> GetAll([FromQuery] PipeSizeQueryParameters queryParams, CancellationToken ct)
        => this.ExecuteGetAllPaged(_service, queryParams, _logger, ct);

    [HttpGet("{id:int}")]
    public Task<IActionResult> GetById(int id, CancellationToken ct)
        => this.ExecuteGetById(_service, id, _logger, ct);

    [HttpPost]
    public Task<IActionResult> Create([FromBody] CreatePipeSizeDto createDto, CancellationToken ct)
        => this.ExecuteCreate(_service, createDto, _logger, ct);

    [HttpPut("{id:int}")]
    public Task<IActionResult> Update(int id, [FromBody] UpdatePipeSizeDto updateDto, CancellationToken ct)
        => this.ExecuteUpdate(_service, id, updateDto, _logger, ct);

    [HttpDelete("{id:int}")]
    public Task<IActionResult> Delete(int id, CancellationToken ct)
        => this.ExecuteDelete(_service, id, _logger, ct);
}
