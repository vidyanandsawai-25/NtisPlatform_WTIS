using Microsoft.AspNetCore.Mvc;
using NtisPlatform.Application.DTOs.WTIS;
using NtisPlatform.Application.Interfaces.WTIS;

namespace NtisPlatform.Api.Controllers.WTIS;

/// <summary>
/// Consumer Account API - Single unified search endpoint
/// </summary>
[ApiController]
[Route("api/wtis/consumer")]
public class ConsumerAccountController : ControllerBase
{
    #region Fields

    private readonly IConsumerAccountService _service;
    private readonly ILogger<ConsumerAccountController> _logger;

    #endregion

    #region Constructor

    public ConsumerAccountController(IConsumerAccountService service, ILogger<ConsumerAccountController> logger)
    {
        _service = service;
        _logger = logger;
    }

    #endregion

    #region API Endpoints

    /// <summary>
    /// Universal search - ONE parameter for all search types
    /// </summary>
    /// <remarks>
    /// **Hierarchical Pattern Search:**
    /// - Ward only: `?search=1` ? All consumers in Ward 1
    /// - Ward-Property: `?search=1-PROP011` ? All in property (all partitions)
    /// - Ward-Property-Partition: `?search=1-PROP011-FLAT-101` ? Exact consumer
    /// 
    /// **Direct Identifier Search:**
    /// - Consumer Number: `?search=CON001`
    /// - Mobile: `?search=9876543210`
    /// - Name: `?search=Raj Sharma` (Hindi/English)
    /// - Email: `?search=raj@example.com`
    /// - ID: `?search=41226`
    /// </remarks>
    /// <param name="search">Universal search parameter (required)</param>
    /// <param name="pageNumber">Page number for list results (default: 1)</param>
    /// <param name="pageSize">Page size for list results (default: 10, max: 100)</param>
    /// <param name="ct">Cancellation token</param>
    [HttpGet]
    [ProducesResponseType(typeof(ConsumerAccountDto), 200)]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Search(
        [FromQuery] string? search = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(search))
                return BadRequest(CreateErrorResponse(
                    "Search parameter is required.",
                    "Use: ?search=CON001 or ?search=1 or ?search=1-PROP011 or ?search=1-PROP011-FLAT-101"));

            // Validate and normalize pagination
            pageSize = Math.Clamp(pageSize, 1, 100);
            pageNumber = Math.Max(pageNumber, 1);
            search = search.Trim();

            // Pattern detection: Ward-Property-Partition (contains dash)
            if (search.Contains('-'))
            {
                return await HandlePatternSearch(search, pageNumber, pageSize, ct);
            }

            // Number detection: Ward number or Consumer ID
            if (int.TryParse(search, out int number))
            {
                return await HandleNumberSearch(search, number, pageNumber, pageSize, ct);
            }

            // Direct field search: Consumer#, Mobile, Name, Email
            return await HandleDirectSearch(search, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching consumers: {Search}", search);
            return StatusCode(500, new { message = "An error occurred while searching." });
        }
    }

    #endregion

    #region Private Helper Methods

    /// <summary>
    /// Handle pattern search (Ward-Property-Partition)
    /// </summary>
    private async Task<IActionResult> HandlePatternSearch(
        string search,
        int pageNumber,
        int pageSize,
        CancellationToken ct)
    {
        var parts = search.Split('-', StringSplitOptions.TrimEntries);
        if (parts.Length < 2)
            return BadRequest(CreateErrorResponse("Invalid pattern format.", "Use: Ward-Property or Ward-Property-Partition"));

        var wardNo = parts[0];
        var propertyNumber = parts[1];
        var partitionNumber = parts.Length > 2 ? parts[2] : null;

        // Level 3: Ward-Property-Partition (exact consumer)
        if (!string.IsNullOrWhiteSpace(partitionNumber))
        {
            var exactResult = await _service.FindConsumerAsync(search, ct);
            return exactResult != null
                ? Ok(exactResult)
                : NotFound(CreateNotFoundResponse(search, "Exact consumer not found"));
        }

        // Level 2: Ward-Property (all partitions in property)
        var searchDto = new ConsumerAccountSearchDto
        {
            WardNo = wardNo,
            PropertyNumber = propertyNumber
        };

        var pagedResult = await _service.SearchConsumersAsync(searchDto, pageNumber, pageSize, ct);
        return pagedResult.TotalCount > 0
            ? Ok(pagedResult)
            : NotFound(CreateNotFoundResponse(search, $"No consumers in Ward {wardNo}, Property {propertyNumber}"));
    }

    /// <summary>
    /// Handle number search (Ward or Consumer ID)
    /// </summary>
    private async Task<IActionResult> HandleNumberSearch(
        string search,
        int number,
        int pageNumber,
        int pageSize,
        CancellationToken ct)
    {
        // Large numbers (>100) treated as Consumer ID
        if (number > 100)
        {
            var consumerById = await _service.FindConsumerAsync(search, ct);
            if (consumerById != null) return Ok(consumerById);
        }

        // Level 1: Ward number (all consumers in ward)
        var searchDto = new ConsumerAccountSearchDto { WardNo = search };
        var pagedResult = await _service.SearchConsumersAsync(searchDto, pageNumber, pageSize, ct);

        return pagedResult.TotalCount > 0
            ? Ok(pagedResult)
            : NotFound(CreateNotFoundResponse(search, $"No active consumers in Ward {search}"));
    }

    /// <summary>
    /// Handle direct field search (Consumer#, Mobile, Name, Email)
    /// </summary>
    private async Task<IActionResult> HandleDirectSearch(string search, CancellationToken ct)
    {
        var result = await _service.FindConsumerAsync(search, ct);
        return result != null
            ? Ok(result)
            : NotFound(CreateNotFoundResponse(
                search,
                "Try: CON001, 9876543210, Raj Sharma, 1 (Ward), 1-PROP011 (Property), or 1-PROP011-FLAT-101 (Exact)"));
    }

    /// <summary>
    /// Create standardized error response
    /// </summary>
    private static object CreateErrorResponse(string message, string hint) => new
    {
        message,
        hint,
        examples = new[]
        {
            "?search=CON001 (Consumer Number)",
            "?search=9876543210 (Mobile)",
            "?search=1 (Ward - all consumers)",
            "?search=1-PROP011 (Property - all partitions)",
            "?search=1-PROP011-FLAT-101 (Exact consumer)"
        }
    };

    /// <summary>
    /// Create standardized not found response
    /// </summary>
    private static object CreateNotFoundResponse(string searchValue, string hint) => new
    {
        message = "Not found.",
        searchValue,
        hint
    };

    #endregion
}
