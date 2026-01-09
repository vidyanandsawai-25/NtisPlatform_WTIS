# WTIS Consumer API - ONE Universal Endpoint

## ?? Single API for Everything

**ONE endpoint to rule them all:**
```
GET /api/wtis/consumer
```

No confusion. No multiple endpoints. Just ONE API that does it all.

---

## ?? How It Works

The API automatically detects what you want based on the parameters you provide:

### 1. **Single Search** - Use `?search=`
Find ONE consumer by ANY identifier:
```bash
GET /api/wtis/consumer?search={anything}
```

### 2. **Multiple Results** - Use filters
Get a LIST of consumers with pagination:
```bash
GET /api/wtis/consumer?wardNo=1&pageSize=50
```

---

## ?? Usage Examples

### Find Single Consumer

**By Consumer Number:**
```bash
GET /api/wtis/consumer?search=CON001
```

**By Mobile Number:**
```bash
GET /api/wtis/consumer?search=9876543210
```

**By Consumer Name:**
```bash
GET /api/wtis/consumer?search=Raj%20Sharma
```

**By Consumer ID:**
```bash
GET /api/wtis/consumer?search=41226
```

**By Ward-Property-Partition Pattern:**
```bash
GET /api/wtis/consumer?search=1-PROP011-FLAT-101
```

**By Email:**
```bash
GET /api/wtis/consumer?search=raj@example.com
```

**By Property:**
```bash
GET /api/wtis/consumer?search=PROP001
```

---

### Get Multiple Consumers (List)

**All active consumers in Ward 1:**
```bash
GET /api/wtis/consumer?wardNo=1
```

**By Ward and Zone:**
```bash
GET /api/wtis/consumer?wardNo=1&zoneNo=1&pageSize=50
```

**By Property (returns all consumers in that property):**
```bash
GET /api/wtis/consumer?propertyNumber=PROP011
```

**By Ward-Property pattern (in propertyNumber):**
```bash
GET /api/wtis/consumer?propertyNumber=1-PROP011
```

**By name pattern (partial match):**
```bash
GET /api/wtis/consumer?consumerName=Sharma&pageSize=20
```

**By mobile number:**
```bash
GET /api/wtis/consumer?mobileNumber=9876543210
```

**Include inactive consumers:**
```bash
GET /api/wtis/consumer?wardNo=1&isActive=false
```

**All consumers (no filter on active status):**
```bash
GET /api/wtis/consumer?wardNo=1&isActive=
```

---

## ?? All Parameters

| Parameter | Type | Description | Example |
|-----------|------|-------------|---------|
| `search` | string | Universal search (returns ONE result) | `CON001`, `9876543210`, `1-PROP011-FLAT-101` |
| `consumerNumber` | string | Filter by consumer number (partial) | `CON` |
| `oldConsumerNumber` | string | Filter by old number | `OLD001` |
| `consumerName` | string | Filter by name (partial, Hindi/English) | `Sharma` |
| `mobileNumber` | string | Filter by mobile | `9876543210` |
| `wardNo` | string | Filter by ward | `1` |
| `zoneNo` | string | Filter by zone | `1` |
| `propertyNumber` | string | Filter by property (supports pattern) | `PROP011` or `1-PROP011` |
| `isActive` | bool | Filter by status (default: true) | `true`, `false` |
| `pageNumber` | int | Page number (default: 1) | `1`, `2`, `3` |
| `pageSize` | int | Items per page (default: 10, max: 100) | `10`, `50`, `100` |

---

## ?? Response Formats

### Single Consumer (when using `?search=`)

**Request:**
```
GET /api/wtis/consumer?search=CON001
```

**Response (200 OK):**
```json
{
  "consumerID": 41226,
  "consumerNumber": "CON001",
  "consumerName": "??? ?????",
  "consumerNameEnglish": "Raj Sharma",
  "wardNo": "1",
  "propertyNumber": "PROP001",
  "mobileNumber": "9876543210",
  "connectionTypeName": "Residential",
  "categoryName": "Domestic",
  "pipeSize": "15mm",
  "isActive": true
}
```

### Multiple Consumers (when using filters)

**Request:**
```
GET /api/wtis/consumer?wardNo=1&pageSize=2
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
      "isActive": true
    },
    {
      "consumerID": 41227,
      "consumerNumber": "CON002",
      "consumerName": "?????? ?????",
      "consumerNameEnglish": "Sunita Patil",
      "connectionTypeName": "Residential",
      "isActive": true
    }
  ],
  "totalCount": 150,
  "pageNumber": 1,
  "pageSize": 2,
  "totalPages": 75,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

### Not Found (404)

```json
{
  "message": "Consumer not found.",
  "searchValue": "CON999",
  "hint": "Try: CON001, 9876543210, Raj Sharma, or 1-PROP001-PART001"
}
```

### Bad Request (400)

```json
{
  "message": "At least one search parameter is required.",
  "hint": "Use: ?search=CON001 for single lookup, or ?wardNo=1 for filtered list"
}
```

---

## ?? Common Scenarios

### Scenario 1: Customer Service Lookup
**Customer says:** "My consumer number is CON001"
```
GET /api/wtis/consumer?search=CON001
```

### Scenario 2: Mobile Verification
**Customer provides mobile:**
```
GET /api/wtis/consumer?search=9876543210
```

### Scenario 3: Apartment Search
**Customer says:** "I'm in Ward 1, Property PROP011, Flat 101"
```
GET /api/wtis/consumer?search=1-PROP011-FLAT-101
```

### Scenario 4: Ward Report
**Admin needs all consumers in Ward 1:**
```
GET /api/wtis/consumer?wardNo=1&pageSize=100
```

### Scenario 5: Building List
**Admin needs all units in a building:**
```
GET /api/wtis/consumer?propertyNumber=1-PROP011
```

### Scenario 6: Search by Name
**Admin searches for "Sharma":**
```
GET /api/wtis/consumer?consumerName=Sharma&pageSize=50
```

### Scenario 7: Zone Report
**Admin needs Ward 1, Zone 1 consumers:**
```
GET /api/wtis/consumer?wardNo=1&zoneNo=1&pageSize=100
```

---

## ?? Smart Features

### 1. **Auto-Detection**
The API automatically knows what you want:
- `?search=` ? Single result
- `?wardNo=` ? Multiple results with pagination

### 2. **Pattern Recognition**
Automatically detects Ward-Property-Partition pattern:
```
Input: "1-PROP011-FLAT-101"
       ?
Ward=1, Property=PROP011, Partition=FLAT-101
```

### 3. **Partial Matching**
Filters support partial matches:
```
?consumerName=Sharma ? Matches "Raj Sharma", "Amit Sharma", etc.
?consumerNumber=CON ? Matches "CON001", "CON002", etc.
```

### 4. **Default Active Filter**
Returns only active consumers by default:
```
?wardNo=1 ? Only active consumers in Ward 1
?wardNo=1&isActive=false ? Include inactive
?wardNo=1&isActive= ? All consumers (active and inactive)
```

### 5. **Master Table Joins**
Always includes descriptive names:
- `connectionTypeName` (e.g., "Residential")
- `categoryName` (e.g., "Domestic")
- `pipeSize` (e.g., "15mm")

---

## ?? Quick Reference

### Single Consumer Search
```
GET /api/wtis/consumer?search={value}
```

**Examples:**
- `?search=CON001`
- `?search=9876543210`
- `?search=1-PROP011-FLAT-101`

### Multiple Consumers (List)
```
GET /api/wtis/consumer?{filters}
```

**Examples:**
- `?wardNo=1`
- `?propertyNumber=PROP011`
- `?consumerName=Sharma&pageSize=50`

---

## ?? Code Examples

### JavaScript/TypeScript

```typescript
// Universal search function
async function searchConsumer(params: any) {
  const query = new URLSearchParams(params).toString();
  const response = await fetch(`/api/wtis/consumer?${query}`);
  return await response.json();
}

// Single consumer lookup
const consumer = await searchConsumer({ search: 'CON001' });

// List by ward
const wardList = await searchConsumer({ 
  wardNo: '1', 
  pageSize: 50 
});

// Pattern search
const apartment = await searchConsumer({ 
  search: '1-PROP011-FLAT-101' 
});

// Multiple filters
const filtered = await searchConsumer({
  wardNo: '1',
  consumerName: 'Sharma',
  pageNumber: 1,
  pageSize: 20
});
```

### C# (.NET)

```csharp
// Single consumer search
var response = await httpClient.GetAsync(
    "/api/wtis/consumer?search=CON001"
);
var consumer = await response.Content
    .ReadFromJsonAsync<ConsumerAccountDto>();

// List by ward
var response = await httpClient.GetAsync(
    "/api/wtis/consumer?wardNo=1&pageSize=50"
);
var result = await response.Content
    .ReadFromJsonAsync<PagedResult<ConsumerAccountDto>>();

// With multiple filters
var query = $"?wardNo=1&zoneNo=1&consumerName=Sharma&pageSize=20";
var response = await httpClient.GetAsync($"/api/wtis/consumer{query}");
```

### cURL

```bash
# Single search
curl "http://localhost:5000/api/wtis/consumer?search=CON001"

# Pattern search
curl "http://localhost:5000/api/wtis/consumer?search=1-PROP011-FLAT-101"

# Ward list
curl "http://localhost:5000/api/wtis/consumer?wardNo=1&pageSize=50"

# Multiple filters
curl "http://localhost:5000/api/wtis/consumer?wardNo=1&zoneNo=1&consumerName=Sharma"

# Property pattern
curl "http://localhost:5000/api/wtis/consumer?propertyNumber=1-PROP011"
```

---

## ?? Decision Logic

The API uses this simple logic:

```
1. Is 'search' parameter provided?
   ?? YES ? Find single consumer (exact match)
   ?? NO ? Continue to step 2

2. Are any filter parameters provided?
   ?? YES ? Search multiple consumers (list with pagination)
   ?? NO ? Return error (parameters required)
```

---

## ? Advantages of Single API

### Before (Multiple Endpoints)
? `/find-consumer?search=CON001`  
? `/search` POST with body  
? `/{id}` for get by ID  
? `/` for list  
?? **Which endpoint should I use?**

### After (ONE Endpoint)
? `/consumer?search=CON001`  
? `/consumer?wardNo=1`  
? `/consumer?search=41226`  
? `/consumer?propertyNumber=1-PROP011`  
?? **Always use the same endpoint!**

---

## ?? Summary

### ONE Endpoint:
```
GET /api/wtis/consumer
```

### TWO Modes:

**1. Single Search (use `?search=`):**
```
?search=CON001
?search=9876543210
?search=1-PROP011-FLAT-101
```

**2. List Search (use filters):**
```
?wardNo=1
?propertyNumber=PROP011
?consumerName=Sharma
```

### THREE Smart Features:
1. ? Auto-detection (single vs list)
2. ? Pattern recognition (Ward-Property-Partition)
3. ? Master table joins (descriptive names)

---

**Simple. Clean. Powerful.** ??
