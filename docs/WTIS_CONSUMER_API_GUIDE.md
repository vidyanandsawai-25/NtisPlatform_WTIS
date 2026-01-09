# WTIS Consumer Account API - Complete Guide

## ?? Overview

A clean, focused read-only API for querying WTIS Consumer Account data with intelligent search capabilities and master table joins.

## ?? Base URL
```
/api/wtis/consumer
```

## ? Key Features

? **Universal Search** - Find consumers using any identifier  
? **Pattern Matching** - Support for Ward-Property-Partition format  
? **Master Table Joins** - Returns ConnectionTypeName, CategoryName, PipeSize  
? **Active Consumers Only** - Filters by IsActive=1 by default  
? **Flexible Filtering** - Multiple search criteria supported  
? **Pagination** - Efficient data retrieval  

---

## ?? API Endpoints

### 1. Universal Find (Primary Search)

**Endpoint:**
```
GET /api/wtis/consumer/find?search={value}
```

**Description:**  
Find a single consumer using ANY identifier.

**Supported Search Formats:**

| Format | Example | Description |
|--------|---------|-------------|
| Consumer Number | `CON001` | Exact consumer number |
| Mobile Number | `9876543210` | Consumer's mobile |
| Consumer Name | `Raj Sharma` or `??? ?????` | Hindi or English name |
| Email | `raj@example.com` | Email address |
| Property Number | `PROP001` | Property identifier |
| **Ward-Property-Partition** | `1-PROP001-PART001` | ? Special pattern |

**Examples:**

```bash
# By Consumer Number
GET /api/wtis/consumer/find?search=CON001

# By Mobile
GET /api/wtis/consumer/find?search=9876543210

# By Name (Hindi)
GET /api/wtis/consumer/find?search=???%20?????

# By Ward-Property-Partition Pattern
GET /api/wtis/consumer/find?search=1-PROP001-PART001
```

**Response (200 OK):**
```json
{
  "consumerID": 41226,
  "consumerNumber": "CON001",
  "oldConsumerNumber": "OLD001",
  "zoneNo": "1",
  "wardNo": "1",
  "propertyNumber": "PROP001",
  "partitionNumber": "PART001",
  "consumerName": "??? ?????",
  "consumerNameEnglish": "Raj Sharma",
  "mobileNumber": "9876543210",
  "emailID": "raj@example.com",
  "address": "????? ???",
  "addressEnglish": "Mumbai Road",
  "connectionTypeID": 1,
  "categoryID": 2,
  "pipeSizeID": 1,
  "connectionTypeName": "Residential",
  "categoryName": "Domestic",
  "pipeSize": "15mm",
  "connectionDate": "2024-01-15",
  "isActive": true,
  "remark": "Residential Consumer",
  "createdDate": "2026-01-07T13:32:33.827",
  "updatedDate": "2026-01-07T13:32:33.827"
}
```

**Response (404 Not Found):**
```json
{
  "message": "Consumer account not found.",
  "searchValue": "CON999",
  "hint": "Try: CON001, 9876543210, Raj Sharma, or 1-PROP001-PART001"
}
```

---

### 2. Multi-Criteria Search with Pagination

**Endpoint:**
```
POST /api/wtis/consumer/search?pageNumber=1&pageSize=10
```

**Description:**  
Search with multiple filters and get paginated results.

**Request Body:**
```json
{
  "consumerNumber": "CON",
  "oldConsumerNumber": null,
  "consumerName": "Raj",
  "mobileNumber": null,
  "wardNo": "1",
  "zoneNo": null,
  "propertyNumber": null,
  "isActive": true
}
```

**Special PropertyNumber Format:**

You can use Ward-Property-Partition pattern in PropertyNumber:
```json
{
  "propertyNumber": "1-PROP011-FLAT-101",
  "isActive": true
}
```

This will search for:
- Ward = 1
- Property = PROP011
- Partition = FLAT-101

**Query Parameters:**

| Parameter | Type | Default | Max | Description |
|-----------|------|---------|-----|-------------|
| pageNumber | int | 1 | - | Page number |
| pageSize | int | 10 | 100 | Items per page |

**Examples:**

```bash
# Search in Ward 1
POST /api/wtis/consumer/search?pageNumber=1&pageSize=20
{
  "wardNo": "1",
  "isActive": true
}

# Search by name pattern
POST /api/wtis/consumer/search
{
  "consumerName": "Sharma"
}

# Search using Ward-Property-Partition in PropertyNumber
POST /api/wtis/consumer/search
{
  "propertyNumber": "1-PROP011-FLAT-101"
}

# Multiple filters
POST /api/wtis/consumer/search?pageSize=50
{
  "wardNo": "1",
  "zoneNo": "1",
  "consumerName": "Raj",
  "isActive": true
}
```

**Response (200 OK):**
```json
{
  "items": [
    {
      "consumerID": 41226,
      "consumerNumber": "CON001",
      "consumerName": "??? ?????",
      "consumerNameEnglish": "Raj Sharma",
      "connectionTypeName": "Residential",
      "categoryName": "Domestic",
      "pipeSize": "15mm",
      "isActive": true
    }
  ],
  "totalCount": 150,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 15,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

---

### 3. Get by ID

**Endpoint:**
```
GET /api/wtis/consumer/{id}
```

**Description:**  
Get a specific consumer by ID.

**Example:**
```bash
GET /api/wtis/consumer/41226
```

**Response:**  
Same format as Universal Find response.

---

### 4. Get All with Query Filters

**Endpoint:**
```
GET /api/wtis/consumer?[filters]
```

**Description:**  
Get paginated list with advanced filtering and sorting.

**Query Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| pageNumber | int | Page number (default: 1) |
| pageSize | int | Items per page (default: 10) |
| searchTerm | string | Search across multiple fields |
| consumerNumber | string | Filter by consumer number |
| wardNo | string | Filter by ward |
| zoneNo | string | Filter by zone |
| consumerName | string | Filter by name |
| isActive | bool | Filter by status |
| sortBy | string | Sort field |
| sortOrder | string | asc/desc |

**Examples:**

```bash
# Active consumers in Ward 1
GET /api/wtis/consumer?wardNo=1&isActive=true&pageSize=50

# Search and sort
GET /api/wtis/consumer?searchTerm=Sharma&sortBy=ConsumerName&sortOrder=asc

# Zone and Ward filter
GET /api/wtis/consumer?zoneNo=1&wardNo=1&pageNumber=2&pageSize=20
```

---

## ?? Ward-Property-Partition Pattern

### Format
```
{WardNo}-{PropertyNumber}-{PartitionNumber}
```

### Usage Scenarios

**1. In Universal Find (`/find`):**
```bash
GET /api/wtis/consumer/find?search=1-PROP011-FLAT-101
```

**2. In Multi-Search (`/search`) - PropertyNumber field:**
```json
POST /api/wtis/consumer/search
{
  "propertyNumber": "1-PROP011-FLAT-101"
}
```

### Pattern Matching Logic

The API automatically detects the pattern and splits it:

```
Input: "1-PROP011-FLAT-101"
       ?
Ward: 1
Property: PROP011  
Partition: FLAT-101
```

### Partial Patterns Supported

```bash
# Full pattern (most specific)
1-PROP011-FLAT-101

# Ward + Property (finds first match)
1-PROP011

# Just Property (finds first match)
PROP011
```

---

## ?? Master Table Joins

The API automatically joins with master tables and returns descriptive names:

### Joined Tables

```sql
SELECT 
    ca.*,
    ct.ConnectionTypeName,      -- From ConnectionTypeMaster
    cc.CategoryName,             -- From ConnectionCategoryMaster
    ps.SizeName AS PipeSize     -- From PipeSizeMaster
FROM WTIS.ConsumerAccount ca
LEFT JOIN WTIS.ConnectionTypeMaster ct ON ca.ConnectionTypeID = ct.ConnectionTypeID
LEFT JOIN WTIS.ConnectionCategoryMaster cc ON ca.CategoryID = cc.CategoryID
LEFT JOIN WTIS.PipeSizeMaster ps ON ca.PipeSizeID = ps.PipeSizeID
WHERE ca.IsActive = 1
```

### Response Fields

**IDs (for reference):**
- `connectionTypeID`
- `categoryID`
- `pipeSizeID`

**Names (human-readable):**
- `connectionTypeName` - e.g., "Residential", "Commercial"
- `categoryName` - e.g., "Domestic", "Industrial"
- `pipeSize` - e.g., "15mm", "20mm", "25mm"

---

## ?? Usage Examples

### JavaScript/TypeScript

```typescript
// Universal Find
async function findConsumer(search: string) {
  const response = await fetch(
    `/api/wtis/consumer/find?search=${encodeURIComponent(search)}`
  );
  return await response.json();
}

// Multi-criteria search
async function searchConsumers(filters: any, page = 1, size = 10) {
  const response = await fetch(
    `/api/wtis/consumer/search?pageNumber=${page}&pageSize=${size}`,
    {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(filters)
    }
  );
  return await response.json();
}

// Usage
const consumer = await findConsumer('CON001');
const list = await searchConsumers({ wardNo: '1', isActive: true }, 1, 20);
```

### C# (.NET)

```csharp
// Universal Find
var response = await httpClient.GetAsync(
    $"/api/wtis/consumer/find?search={Uri.EscapeDataString("CON001")}"
);
var consumer = await response.Content.ReadFromJsonAsync<ConsumerAccountDto>();

// Multi-criteria search
var searchDto = new ConsumerAccountSearchDto
{
    WardNo = "1",
    IsActive = true
};
var response = await httpClient.PostAsJsonAsync(
    "/api/wtis/consumer/search?pageNumber=1&pageSize=20",
    searchDto
);
var result = await response.Content.ReadFromJsonAsync<PagedResult<ConsumerAccountDto>>();
```

### cURL

```bash
# Universal find
curl "http://localhost:5000/api/wtis/consumer/find?search=CON001"

# Pattern search
curl "http://localhost:5000/api/wtis/consumer/find?search=1-PROP011-FLAT-101"

# Multi-search
curl -X POST "http://localhost:5000/api/wtis/consumer/search?pageSize=20" \
  -H "Content-Type: application/json" \
  -d '{"wardNo":"1","isActive":true}'

# Get by ID
curl "http://localhost:5000/api/wtis/consumer/41226"

# Get all with filters
curl "http://localhost:5000/api/wtis/consumer?wardNo=1&isActive=true"
```

---

## ?? Common Use Cases

### 1. Customer Service Portal
Agent searches with whatever customer provides:
```javascript
const consumer = await findConsumer(userInput);
// Works with: CON001, 9876543210, "Raj Sharma", "1-PROP011-FLAT-101"
```

### 2. Apartment Building Management
Find specific units in a building:
```javascript
// List all flats in Sundar Apartment (Ward 1, Property PROP011)
const result = await searchConsumers({
  propertyNumber: "1-PROP011"
});
```

### 3. Ward-wise Reports
Get all consumers in a ward:
```javascript
const consumers = await searchConsumers({
  wardNo: "1",
  isActive: true
}, 1, 100);
```

### 4. Mobile Verification
Check if mobile number exists:
```javascript
const consumer = await findConsumer("9876543210");
if (consumer) {
  console.log(`Found: ${consumer.consumerName}`);
}
```

---

## ?? Default Behaviors

### Active Consumers Only
By default, all endpoints return only active consumers (`IsActive = 1`).

To include inactive consumers:
```json
{
  "isActive": false  // or null to include all
}
```

### Sorting
Default sort order: `ConsumerID ASC`

Override with query parameters:
```
?sortBy=ConsumerName&sortOrder=asc
```

### Pagination
Default page size: 10  
Maximum page size: 100

---

## ? Performance Tips

1. **Use Universal Find** for single lookups - fastest
2. **Use Get by ID** if you know the ConsumerID - direct PK lookup
3. **Ward-Property-Partition pattern** - indexed fields for fast search
4. **Limit page size** - use appropriate pageSize for your needs
5. **Filter by Ward/Zone** - most efficient filters

---

## ?? Troubleshooting

### Error: "Consumer not found"
- Check if consumer is active (`IsActive = 1`)
- Verify exact spelling (case-sensitive for some fields)
- Try different identifiers (mobile, name, etc.)

### Error: "Search parameter is required"
- Ensure `search` query parameter is provided
- Check URL encoding for special characters

### Pattern not matching
- Verify pattern format: `WardNo-PropertyNumber-PartitionNumber`
- Check if all parts exist in database
- Try partial pattern: `WardNo-PropertyNumber`

---

## ?? API Summary

| Endpoint | Method | Purpose | Key Feature |
|----------|--------|---------|-------------|
| `/find` | GET | Find single consumer | Universal search |
| `/search` | POST | Multi-criteria search | Pattern support in PropertyNumber |
| `/{id}` | GET | Get by ID | Direct lookup |
| `/` | GET | List with filters | Advanced filtering |

---

## ? What Was Cleaned Up

### Removed:
? Create endpoint (POST)  
? Update endpoint (PUT)  
? Delete endpoint (DELETE)  
? Duplicate search endpoints  
? Unused DTOs  
? Complex query methods  

### Added:
? Master table joins (ConnectionTypeName, CategoryName, PipeSize)  
? Ward-Property-Partition pattern support  
? IsActive=1 default filter  
? Enhanced search in PropertyNumber field  
? Better error messages  
? Complete API documentation  

---

## ?? Quick Start

**1. Find a consumer:**
```bash
curl "http://localhost:5000/api/wtis/consumer/find?search=CON001"
```

**2. Search in a ward:**
```bash
curl -X POST "http://localhost:5000/api/wtis/consumer/search" \
  -H "Content-Type: application/json" \
  -d '{"wardNo":"1"}'
```

**3. Find apartment unit:**
```bash
curl "http://localhost:5000/api/wtis/consumer/find?search=1-PROP011-FLAT-101"
```

---

**Clean, Simple, Powerful!** ??
