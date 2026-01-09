using NtisPlatform.Application.Attributes;
using NtisPlatform.Application.DTOs.Queries;
using NtisPlatform.Application.Enums;

namespace NtisPlatform.Application.DTOs.WTIS;

public class ConsumerAccountQueryParameters : BaseQueryParameters
{
    [Filterable(FilterOperator.Contains)]
    [Searchable]
    [Sortable]
    public string? ConsumerNumber { get; set; }

    [Filterable(FilterOperator.Contains)]
    [Searchable]
    public string? OldConsumerNumber { get; set; }

    [Filterable(FilterOperator.Contains)]
    [Searchable]
    public string? MobileNumber { get; set; }

    [Filterable(FilterOperator.Equals)]
    [Sortable]
    public string? WardNo { get; set; }

    [Filterable(FilterOperator.Contains)]
    public string? PropertyNumber { get; set; }

    [Filterable(FilterOperator.Equals)]
    [Sortable]
    public string? ZoneNo { get; set; }

    [Filterable(FilterOperator.Contains)]
    [Searchable]
    [Sortable]
    public string? ConsumerName { get; set; }

    [Filterable(FilterOperator.Equals)]
    [Sortable]
    public bool? IsActive { get; set; }

    [Filterable(FilterOperator.GreaterThanOrEqual, EntityProperty = "ConnectionDate")]
    public DateTime? ConnectionDateFrom { get; set; }

    [Filterable(FilterOperator.LessThanOrEqual, EntityProperty = "ConnectionDate")]
    public DateTime? ConnectionDateTo { get; set; }
}
