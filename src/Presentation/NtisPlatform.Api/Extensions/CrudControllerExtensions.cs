using Microsoft.AspNetCore.Mvc;
using NtisPlatform.Application.DTOs.Queries;
using NtisPlatform.Application.Exceptions;
using NtisPlatform.Application.Interfaces;
using NtisPlatform.Application.Models;

namespace NtisPlatform.Api.Extensions;

/// <summary>
/// Generic CRUD controller extension methods - Reusable by all modules
/// </summary>
public static class CrudControllerExtensions
{
    /// <summary>
    /// Execute GetAll with pagination, filtering, and sorting
    /// </summary>
    public static async Task<IActionResult> ExecuteGetAllPaged<TEntity, TDto, TCreateDto, TUpdateDto, TQueryParams, TKey>(
        this ControllerBase controller,
        ICommonCrudService<TEntity, TDto, TCreateDto, TUpdateDto, TQueryParams, TKey> service,
        TQueryParams queryParameters,
        ILogger logger,
        CancellationToken cancellationToken = default)
        where TQueryParams : BaseQueryParameters
    {
        try
        {
            var result = await service.GetAllAsync(queryParameters, cancellationToken);
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
    /// Execute GetById operation
    /// </summary>
    public static async Task<IActionResult> ExecuteGetById<TEntity, TDto, TCreateDto, TUpdateDto, TQueryParams, TKey>(
        this ControllerBase controller,
        ICommonCrudService<TEntity, TDto, TCreateDto, TUpdateDto, TQueryParams, TKey> service,
        TKey id,
        ILogger logger,
        CancellationToken cancellationToken = default)
        where TQueryParams : BaseQueryParameters
    {
        try
        {
            var result = await service.GetByIdAsync(id, cancellationToken);
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
    /// Execute Create operation with duplicate detection
    /// </summary>
    public static async Task<IActionResult> ExecuteCreate<TEntity, TDto, TCreateDto, TUpdateDto, TQueryParams, TKey>(
        this ControllerBase controller,
        ICommonCrudService<TEntity, TDto, TCreateDto, TUpdateDto, TQueryParams, TKey> service,
        TCreateDto createDto,
        ILogger logger,
        CancellationToken cancellationToken = default)
        where TQueryParams : BaseQueryParameters
    {
        try
        {
            var result = await service.CreateAsync(createDto, cancellationToken);
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
    /// Execute Update operation with duplicate detection
    /// </summary>
    public static async Task<IActionResult> ExecuteUpdate<TEntity, TDto, TCreateDto, TUpdateDto, TQueryParams, TKey>(
        this ControllerBase controller,
        ICommonCrudService<TEntity, TDto, TCreateDto, TUpdateDto, TQueryParams, TKey> service,
        TKey id,
        TUpdateDto updateDto,
        ILogger logger,
        CancellationToken cancellationToken = default)
        where TQueryParams : BaseQueryParameters
    {
        try
        {
            var result = await service.UpdateAsync(id, updateDto, cancellationToken);

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
    public static async Task<IActionResult> ExecuteDelete<TEntity, TDto, TCreateDto, TUpdateDto, TQueryParams, TKey>(
        this ControllerBase controller,
        ICommonCrudService<TEntity, TDto, TCreateDto, TUpdateDto, TQueryParams, TKey> service,
        TKey id,
        ILogger logger,
        CancellationToken cancellationToken = default)
        where TQueryParams : BaseQueryParameters
    {
        try
        {
            var result = await service.DeleteAsync(id, cancellationToken);
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

    /// <summary>
    /// Execute universal search - Generic method for any service with UniversalSearchAsync method
    /// Uses reflection to invoke UniversalSearchAsync(string, CancellationToken) on any service
    /// </summary>
    /// <typeparam name="TService">Service type (any class with UniversalSearchAsync method)</typeparam>
    /// <param name="controller">Controller instance</param>
    /// <param name="service">Service instance</param>
    /// <param name="search">Search parameter</param>
    /// <param name="logger">Logger instance</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Search results or error response</returns>
    public static async Task<IActionResult> ExecuteUniversalSearch<TService>(
        this ControllerBase controller,
        TService service,
        string? search,
        ILogger logger,
        CancellationToken cancellationToken = default)
        where TService : class
    {
        try
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return controller.BadRequest(new
                {
                    message = "Search parameter is required",
                    hint = "Provide a search value"
                });
            }

            var methodInfo = typeof(TService).GetMethod("UniversalSearchAsync");
            if (methodInfo == null)
            {
                logger.LogError("UniversalSearchAsync method not found on service type {ServiceType}", typeof(TService).Name);
                return controller.StatusCode(500, new { message = "Search not supported" });
            }

            var task = methodInfo.Invoke(service, new object[] { search, cancellationToken }) as Task<object>;
            if (task == null)
            {
                logger.LogError("Failed to invoke UniversalSearchAsync");
                return controller.StatusCode(500, new { message = "Search execution failed" });
            }

            var result = await task;
            
            var resultType = result.GetType();
            var messageProperty = resultType.GetProperty("message");
            if (messageProperty != null)
            {
                var message = messageProperty.GetValue(result)?.ToString();
                if (message?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return controller.NotFound(result);
                }
            }

            return controller.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Universal search error: {Search}", search);
            return controller.StatusCode(500, new { message = "An error occurred while searching" });
        }
    }
}
