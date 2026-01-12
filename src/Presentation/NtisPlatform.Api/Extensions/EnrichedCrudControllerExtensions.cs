using Microsoft.AspNetCore.Mvc;
using NtisPlatform.Application.DTOs.Queries;
using NtisPlatform.Application.Exceptions;
using NtisPlatform.Application.Models;

namespace NtisPlatform.Api.Extensions;

/// <summary>
/// Extended CRUD controller methods for masters that return enriched data with JOINs
/// </summary>
public static class EnrichedCrudControllerExtensions
{
    /// <summary>
    /// Execute GetAll with pagination for services that return enriched DTOs (with JOINs)
    /// </summary>
    public static async Task<IActionResult> ExecuteGetAllEnriched<TDto, TQueryParams>(
        this ControllerBase controller,
        Func<TQueryParams, CancellationToken, Task<PagedResult<TDto>>> serviceMethod,
        TQueryParams queryParameters,
        ILogger logger,
        CancellationToken cancellationToken = default)
        where TQueryParams : BaseQueryParameters
    {
        try
        {
            var result = await serviceMethod(queryParameters, cancellationToken);
            return controller.Ok(result);
        }
        catch (FilterValidationException ex)
        {
            logger.LogWarning(ex, "Filter validation failed: {Message}", ex.Message);
            return controller.BadRequest(new
            {
                message = ex.Message,
                errors = ex.Errors
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GetAll operation failed");
            return controller.StatusCode(500, new ApiResponse<TDto>
            {
                Success = false,
                Message = "An error occurred while processing your request."
            });
        }
    }

    /// <summary>
    /// Execute GetById for services that return enriched DTOs (with JOINs)
    /// </summary>
    public static async Task<IActionResult> ExecuteGetByIdEnriched<TDto>(
        this ControllerBase controller,
        Func<int, CancellationToken, Task<TDto?>> serviceMethod,
        int id,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await serviceMethod(id, cancellationToken);
            return result == null ? controller.NotFound() : controller.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "GetById operation failed for id: {Id}", id);
            return controller.StatusCode(500, new ApiResponse<TDto>
            {
                Success = false,
                Message = "An error occurred while processing your request."
            });
        }
    }

    /// <summary>
    /// Execute Create for services that return enriched DTOs (with JOINs)
    /// </summary>
    public static async Task<IActionResult> ExecuteCreateEnriched<TCreateDto, TDto>(
        this ControllerBase controller,
        Func<TCreateDto, CancellationToken, Task<TDto>> serviceMethod,
        TCreateDto createDto,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await serviceMethod(createDto, cancellationToken);
            return controller.Ok(new ApiResponse<TDto>
            {
                Success = true,
                Message = "Record inserted successfully",
                Items = result
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Create operation failed");
            var errorMessage = ex.InnerException?.Message ?? ex.Message;

            if (errorMessage.Contains("duplicate", StringComparison.OrdinalIgnoreCase) ||
                errorMessage.Contains("unique", StringComparison.OrdinalIgnoreCase) ||
                errorMessage.Contains("constraint", StringComparison.OrdinalIgnoreCase))
            {
                return controller.Conflict(new ApiResponse<TDto>
                {
                    Success = false,
                    Message = "A record with the same details already exists."
                });
            }
            
            return controller.StatusCode(500, new ApiResponse<TDto>
            {
                Success = false,
                Message = "An error occurred while processing your request."
            });
        }
    }

    /// <summary>
    /// Execute Update for services that return enriched DTOs (with JOINs)
    /// </summary>
    public static async Task<IActionResult> ExecuteUpdateEnriched<TUpdateDto, TDto>(
        this ControllerBase controller,
        Func<int, TUpdateDto, CancellationToken, Task<TDto?>> serviceMethod,
        int id,
        TUpdateDto updateDto,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await serviceMethod(id, updateDto, cancellationToken);

            if (result == null)
            {
                return controller.Ok(new ApiResponse<TDto>
                {
                    Success = false,
                    Message = "Record not found for Update",
                    Items = result
                });
            }

            return controller.Ok(new ApiResponse<TDto>
            {
                Success = true,
                Message = "Record updated successfully",
                Items = result
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Update operation failed for id: {Id}", id);
            var errorMessage = ex.InnerException?.Message ?? ex.Message;

            if (errorMessage.Contains("duplicate", StringComparison.OrdinalIgnoreCase) ||
                errorMessage.Contains("unique", StringComparison.OrdinalIgnoreCase) ||
                errorMessage.Contains("constraint", StringComparison.OrdinalIgnoreCase))
            {
                return controller.Conflict(new ApiResponse<TDto>
                {
                    Success = false,
                    Message = "A record with the same details already exists."
                });
            }
            
            return controller.StatusCode(500, new ApiResponse<TDto>
            {
                Success = false,
                Message = "An error occurred while processing your request."
            });
        }
    }

    /// <summary>
    /// Execute Delete operation
    /// </summary>
    public static async Task<IActionResult> ExecuteDeleteEnriched<TDto>(
        this ControllerBase controller,
        Func<int, CancellationToken, Task<bool>> serviceMethod,
        int id,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await serviceMethod(id, cancellationToken);
            return result 
                ? controller.Ok(new ApiResponse<TDto>
                {
                    Success = true,
                    Message = "Record deleted"
                }) 
                : controller.Ok(new ApiResponse<TDto>
                {
                    Success = false,
                    Message = "Record not found to delete"
                });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Delete operation failed for id: {Id}", id);
            return controller.StatusCode(500, new ApiResponse<TDto>
            {
                Success = false,
                Message = "An error occurred while processing your request."
            });
        }
    }
}
