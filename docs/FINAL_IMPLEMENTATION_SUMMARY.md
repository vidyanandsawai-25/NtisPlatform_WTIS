# ? FINAL IMPLEMENTATION - WTIS Consumer API with Master Table Joins

## ?? What Was Accomplished

Successfully implemented a **single universal API endpoint** that returns consumer data **WITH master table joins** (ConnectionTypeName, CategoryName, PipeSize).

---

## ?? The ONE API Endpoint

```
GET /api/wtis/consumer
```

**Returns data with SQL LEFT JOINs:**
```sql
SELECT 
    ca.ConsumerID, ca.ConsumerNumber, ca.OldConsumerNumber, 
    ca.ZoneNo, ca.WardNo, ca.PropertyNumber, ca.PartitionNumber,
    ca.ConsumerName, ca.ConsumerNameEnglish, ca.MobileNumber, 
    ca.EmailID, ca.Address, ca.AddressEnglish,
    ca.ConnectionTypeID, ca.CategoryID, ca.PipeSizeID,
    ca.ConnectionDate, ca.IsActive, ca.Remark, 
    ca.CreatedDate, ca.UpdatedDate,
    ct.ConnectionTypeName,          -- From ConnectionTypeMaster
    cc.CategoryName,                -- From ConnectionCategoryMaster
    ps.SizeName AS PipeSize        -- From PipeSizeMaster
FROM WTIS.ConsumerAccount ca
LEFT JOIN WTIS.ConnectionTypeMaster ct ON ca.ConnectionTypeID = ct.ConnectionTypeID
LEFT JOIN WTIS.ConnectionCategoryMaster cc ON ca.CategoryID = cc.CategoryID
LEFT JOIN WTIS.PipeSizeMaster ps ON ca.PipeSizeID = ps.PipeSizeID
WHERE ca.IsActive = 1
ORDER BY ca.ConsumerID
```

---

## ?? Response Format

### With Master Table Data

```json
{
  "consumerID": 41234,
  "consumerNumber": "CON009",
  "oldConsumerNumber": "OLD009",
  "zoneNo": "3",
  "wardNo": "3",
  "propertyNumber": "PROP009",
  "partitionNumber": "PART009",
  "consumerName": "??? ????????",
  "consumerNameEnglish": "Ravi Kulkarni",
  "mobileNumber": "9876543218",
  "emailID": "ravi@example.com",
  "address": "?????? ???",
  "addressEnglish": "Sangli Road",
  
  "connectionTypeID": 1,
  "connectionTypeName": "Residential",        // ? FROM JOIN
  
  "categoryID": 2,
  "categoryName": "Domestic",                 // ? FROM JOIN
  
  "pipeSizeID": 3,
  "pipeSize": "20mm",                        // ? FROM JOIN
  
  "connectionDate": "2024-09-14",
  "isActive": true,
  "remark": "Residential Consumer",
  "createdDate": "2026-01-07T13:32:33.827",
  "updatedDate": "2026-01-07T13:32:33.827"
}
```

---

## ?? Usage Modes

### Mode 1: Single Search (use `?search=`)

Find ONE consumer by ANY identifier:

```bash
# By Consumer Number
GET /api/wtis/consumer?search=CON009

# By Mobile
GET /api/wtis/consumer?search=9876543218

# By Name (Hindi or English)
GET /api/wtis/consumer?search=??? ????????
GET /api/wtis/consumer?search=Ravi%20Kulkarni

# By Consumer ID
GET /api/wtis/consumer?search=41234

# By Ward-Property-Partition Pattern
GET /api/wtis/consumer?search=3-PROP009-PART009

# By Email
GET /api/wtis/consumer?search=ravi@example.com

# By Property
GET /api/wtis/consumer?search=PROP009
```

### Mode 2: List/Filter (use filter parameters)

Get MULTIPLE consumers with pagination:

```bash
# All in Ward 3
GET /api/wtis/consumer?wardNo=3&pageSize=50

# By Ward and Zone
GET /api/wtis/consumer?wardNo=3&zoneNo=3&pageSize=100

# By Property (all units)
GET /api/wtis/consumer?propertyNumber=PROP009

# By Ward-Property Pattern
GET /api/wtis/consumer?propertyNumber=3-PROP009

# By Name (partial match)
GET /api/wtis/consumer?consumerName=Kulkarni&pageSize=20

# Multiple filters
GET /api/wtis/consumer?wardNo=3&consumerName=Ravi&pageSize=10
```

---

## ? Special Features

### 1. Master Table Joins
**Always includes:**
- `connectionTypeName` - e.g., "Residential", "Commercial", "Industrial"
- `categoryName` - e.g., "Domestic", "Commercial", "Institutional"
- `pipeSize` - e.g., "15mm", "20mm", "25mm"

### 2. Ward-Property-Partition Pattern
**Format:** `{WardNo}-{PropertyNumber}-{PartitionNumber}`

**Works in:**
- `?search=` parameter (single search)
- `?propertyNumber=` parameter (list filter)

**Examples:**
```
3-PROP009-PART009  (Full pattern)
3-PROP009          (Ward + Property)
PROP009            (Property only)
```

### 3. Active-Only Default
**Default behavior:** Returns only active consumers (`WHERE IsActive = 1`)

**Override:**
```
?isActive=false     # Include inactive only
?isActive=          # Include all (active and inactive)
```

### 4. Automatic Detection
API automatically knows what you want:
- `?search=CON009` ? Returns ONE consumer
- `?wardNo=3` ? Returns LIST of consumers

---

## ?? All Parameters

| Parameter | Type | Mode | Description |
|-----------|------|------|-------------|
| `search` | string | Single | Universal search (Consumer Number, Mobile, Name, Pattern, etc.) |
| `consumerNumber` | string | List | Filter by consumer number (partial) |
| `oldConsumerNumber` | string | List | Filter by old number |
| `consumerName` | string | List | Filter by name (partial, Hindi/English) |
| `mobileNumber` | string | List | Filter by mobile |
| `wardNo` | string | List | Filter by ward |
| `zoneNo` | string | List | Filter by zone |
| `propertyNumber` | string | List | Filter by property (supports pattern) |
| `isActive` | bool | Both | Filter by status (default: true) |
| `pageNumber` | int | List | Page number (default: 1) |
| `pageSize` | int | List | Page size (default: 10, max: 100) |

---

## ?? Code Examples

### JavaScript/TypeScript

```typescript
// Single search
async function findConsumer(search: string) {
  const response = await fetch(
    `/api/wtis/consumer?search=${encodeURIComponent(search)}`
  );
  return await response.json();
}

// List with filters
async function listConsumers(filters: any) {
  const params = new URLSearchParams(filters);
  const response = await fetch(`/api/wtis/consumer?${params}`);
  return await response.json();
}

// Usage
const consumer = await findConsumer('CON009');
console.log(`${consumer.consumerName} - ${consumer.connectionTypeName}`);

const list = await listConsumers({ 
  wardNo: '3', 
  pageSize: 50 
});
console.log(`Found ${list.totalCount} consumers`);
```

### C# (.NET)

```csharp
// Single search
var response = await httpClient.GetAsync(
    "/api/wtis/consumer?search=CON009"
);
var consumer = await response.Content
    .ReadFromJsonAsync<ConsumerAccountDto>();
    
Console.WriteLine($"{consumer.ConsumerName} - {consumer.ConnectionTypeName}");

// List with filters
var response = await httpClient.GetAsync(
    "/api/wtis/consumer?wardNo=3&pageSize=50"
);
var result = await response.Content
    .ReadFromJsonAsync<PagedResult<ConsumerAccountDto>>();
    
Console.WriteLine($"Found {result.TotalCount} consumers");
```

### cURL

```bash
# Single search
curl "http://localhost:5000/api/wtis/consumer?search=CON009"

# Pattern search
curl "http://localhost:5000/api/wtis/consumer?search=3-PROP009-PART009"

# List by ward
curl "http://localhost:5000/api/wtis/consumer?wardNo=3&pageSize=50"

# Multiple filters
curl "http://localhost:5000/api/wtis/consumer?wardNo=3&consumerName=Ravi"
```

---

## ??? Technical Implementation

### Files Modified

1. **`ConsumerAccountWithMasterData.cs`** (NEW)
   - Entity class for SQL query results with JOINs

2. **`ConsumerAccountRepository.cs`**
   - Uses raw SQL with LEFT JOINs
   - Returns `ConsumerAccountWithMasterData`

3. **`IConsumerAccountRepository.cs`**
   - Updated interface to return master data

4. **`ConsumerAccountService.cs`**
   - Maps from repository results to DTOs

5. **`ConsumerAccountMappingProfile.cs`**
   - AutoMapper configuration for master data

6. **`ConsumerAccountController.cs`**
   - Single endpoint with dual mode support

7. **`ConsumerAccountDto.cs`**
   - Includes master table fields

### Architecture

```
???????????????????????????????????????????
?  HTTP Request                            ?
?  GET /api/wtis/consumer?search=CON009  ?
???????????????????????????????????????????
             ?
             ?
???????????????????????????????????????????
?  Controller (Single Endpoint)           ?
?  - Detects mode (single vs list)        ?
?  - Validates parameters                  ?
???????????????????????????????????????????
             ?
             ?
???????????????????????????????????????????
?  Service Layer                           ?
?  - FindConsumerAsync()                   ?
?  - SearchConsumersAsync()                ?
???????????????????????????????????????????
             ?
             ?
???????????????????????????????????????????
?  Repository (Raw SQL)                    ?
?  LEFT JOIN ConnectionTypeMaster          ?
?  LEFT JOIN ConnectionCategoryMaster      ?
?  LEFT JOIN PipeSizeMaster               ?
???????????????????????????????????????????
             ?
             ?
???????????????????????????????????????????
?  SQL Server Database                     ?
?  - WTIS.ConsumerAccount                  ?
?  - WTIS.ConnectionTypeMaster            ?
?  - WTIS.ConnectionCategoryMaster        ?
?  - WTIS.PipeSizeMaster                  ?
???????????????????????????????????????????
```

---

## ? What You Get

### Before
? Multiple endpoints (confusing)  
? No master table data (only IDs)  
? Complex query logic  
? Separate APIs for different searches  

### After
? **ONE endpoint** for everything  
? **Master table names** included (ConnectionTypeName, CategoryName, PipeSize)  
? **Smart detection** (single vs list)  
? **Pattern support** (Ward-Property-Partition)  
? **SQL JOINs** built-in  
? **Active-only** default filter  

---

## ?? Quick Start

```bash
# 1. Start the API
cd src/Presentation/NtisPlatform.Api
dotnet run

# 2. Test single search
curl "http://localhost:5000/api/wtis/consumer?search=CON009"

# 3. Test pattern search
curl "http://localhost:5000/api/wtis/consumer?search=3-PROP009-PART009"

# 4. Test list
curl "http://localhost:5000/api/wtis/consumer?wardNo=3&pageSize=50"
```

---

## ?? Summary

? **ONE API endpoint:** `/api/wtis/consumer`  
? **TWO modes:** Single search (`?search=`) or List (`?wardNo=`, etc.)  
? **THREE master table joins:** ConnectionTypeMaster, ConnectionCategoryMaster, PipeSizeMaster  
? **SMART pattern matching:** Ward-Property-Partition support  
? **ACTIVE-ONLY default:** Filters `WHERE IsActive = 1`  
? **CLEAN response:** Includes both IDs and descriptive names  

---

**The API is now complete, clean, and production-ready!** ??

All responses include master table data (ConnectionTypeName, CategoryName, PipeSize) from SQL LEFT JOINs, exactly as requested! ??
