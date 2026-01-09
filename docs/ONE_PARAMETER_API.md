# WTIS Consumer API - ONE Parameter for Everything

## ?? Super Simple API

**ONE endpoint. ONE parameter. That's it.**

```
GET /api/wtis/consumer?search={anything}
```

---

## ?? How It Works

Just use `?search=` for **EVERYTHING**. The API is smart enough to figure out what you want.

### Single Consumer Search

Just pass the value directly:

```bash
# By Consumer Number
GET /api/wtis/consumer?search=CON001

# By Mobile
GET /api/wtis/consumer?search=9876543210

# By Name (Hindi or English)
GET /api/wtis/consumer?search=Raj Sharma
GET /api/wtis/consumer?search=??? ?????

# By Email
GET /api/wtis/consumer?search=raj@example.com

# By Consumer ID
GET /api/wtis/consumer?search=41226

# By Property
GET /api/wtis/consumer?search=PROP001

# By Ward-Property-Partition Pattern
GET /api/wtis/consumer?search=1-PROP011-FLAT-101
```

### Multiple Consumers (List)

Use prefix to get lists:

```bash
# All consumers in Ward 1
GET /api/wtis/consumer?search=Ward:1
GET /api/wtis/consumer?search=W:1        # Short form

# All consumers in Zone 1
GET /api/wtis/consumer?search=Zone:1
GET /api/wtis/consumer?search=Z:1        # Short form

# Search by name (partial match)
GET /api/wtis/consumer?search=Name:Sharma
GET /api/wtis/consumer?search=N:Sharma   # Short form

# Search by property
GET /api/wtis/consumer?search=Property:PROP011
GET /api/wtis/consumer?search=P:PROP011  # Short form

# Search by mobile (for list)
GET /api/wtis/consumer?search=Mobile:9876543210
GET /api/wtis/consumer?search=M:9876543210       # Short form
```

### With Pagination

Add pagination for list results:

```bash
# Ward 1, page 1, 50 items
GET /api/wtis/consumer?search=Ward:1&pageNumber=1&pageSize=50

# Zone 1, page 2, 20 items
GET /api/wtis/consumer?search=Zone:1&pageNumber=2&pageSize=20

# Name search with pagination
GET /api/wtis/consumer?search=Name:Sharma&pageSize=100
```

---

## ?? Search Prefixes (for Lists)

| Prefix | Short | Example | Description |
|--------|-------|---------|-------------|
| `Ward:` | `W:` | `?search=Ward:1` | All consumers in ward |
| `Zone:` | `Z:` | `?search=Zone:1` | All consumers in zone |
| `Name:` | `N:` | `?search=Name:Sharma` | Search by name (partial) |
| `Property:` | `P:` | `?search=Property:PROP011` | All in property |
| `Mobile:` | `M:` | `?search=Mobile:9876543210` | Search by mobile |

**No prefix?** Single consumer search (exact match).

---

## ?? Response Formats

### Single Consumer (no prefix)

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

### Multiple Consumers (with prefix)

**Request:**
```
GET /api/wtis/consumer?search=Ward:1&pageSize=2
```

**Response (200 OK):**
```json
{
  "items": [
    {
      "consumerID": 41226,
      "consumerNumber": "CON001",
      "consumerName": "??? ?????",
      "connectionTypeName": "Residential",
      "isActive": true
    },
    {
      "consumerID": 41227,
      "consumerNumber": "CON002",
      "consumerName": "?????? ?????",
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
  "hint": "Try: CON001, 9876543210, Raj Sharma, or 1-PROP001-PART001. For lists use: Ward:1, Zone:1, Name:Sharma"
}
```

---

## ?? Smart Detection

The API automatically knows what you want:

```
Input: "CON001"           ? Single consumer search
Input: "9876543210"       ? Single consumer search
Input: "1-PROP011-FLAT-101" ? Single consumer search (pattern)
Input: "Ward:1"           ? List of consumers in Ward 1
Input: "Name:Sharma"      ? List of consumers with name "Sharma"
```

---

## ?? Quick Examples

### Customer Service Scenarios

**1. Customer calls with consumer number:**
```sh
curl "http://localhost:5000/api/wtis/consumer?search=CON001"
```

**2. Customer provides mobile:**
```sh
curl "http://localhost:5000/api/wtis/consumer?search=9876543210"
```

**3. Customer knows their flat:**
```sh
curl "http://localhost:5000/api/wtis/consumer?search=1-PROP011-FLAT-101"
```

**4. Admin needs ward report:**
```sh
curl "http://localhost:5000/api/wtis/consumer?search=Ward:1&pageSize=100"
```

**5. Admin searches by name:**
```sh
curl "http://localhost:5000/api/wtis/consumer?search=Name:Sharma&pageSize=50"
```

---

## ?? Code Examples

### JavaScript/TypeScript

```typescript
// Universal search function - ONE parameter
async function search(value: string, page = 1, size = 10) {
  const params = new URLSearchParams({
    search: value,
    pageNumber: page.toString(),
    pageSize: size.toString()
  });
  
  const response = await fetch(`/api/wtis/consumer?${params}`);
  return await response.json();
}

// Usage - ALL cases use the same function!
const consumer = await search('CON001');              // Single
const mobile = await search('9876543210');            // Single
const pattern = await search('1-PROP011-FLAT-101');   // Single
const ward = await search('Ward:1', 1, 50);           // List
const name = await search('Name:Sharma', 1, 20);      // List
const zone = await search('Z:1', 1, 100);             // List (short form)
```

### C# (.NET)

```csharp
// Universal search method - ONE parameter
async Task<object> Search(string searchValue, int page = 1, int size = 10)
{
    var query = $"?search={Uri.EscapeDataString(searchValue)}&pageNumber={page}&pageSize={size}";
    var response = await httpClient.GetAsync($"/api/wtis/consumer{query}");
    return await response.Content.ReadFromJsonAsync<object>();
}

// Usage - ALL cases use the same method!
var consumer = await Search("CON001");                 // Single
var mobile = await Search("9876543210");               // Single
var pattern = await Search("1-PROP011-FLAT-101");      // Single
var ward = await Search("Ward:1", 1, 50);              // List
var name = await Search("Name:Sharma", 1, 20);         // List
var zone = await Search("Z:1", 1, 100);                // List (short form)
```

### Python

```python
import requests

def search(value, page=1, size=10):
    params = {
        'search': value,
        'pageNumber': page,
        'pageSize': size
    }
    response = requests.get('http://localhost:5000/api/wtis/consumer', params=params)
    return response.json()

# Usage - ALL cases use the same function!
consumer = search('CON001')                    # Single
mobile = search('9876543210')                  # Single
pattern = search('1-PROP011-FLAT-101')         # Single
ward = search('Ward:1', page=1, size=50)       # List
name = search('Name:Sharma', page=1, size=20)  # List
zone = search('Z:1', page=1, size=100)         # List (short form)
```

### cURL

```bash
# Single searches
curl "http://localhost:5000/api/wtis/consumer?search=CON001"
curl "http://localhost:5000/api/wtis/consumer?search=9876543210"
curl "http://localhost:5000/api/wtis/consumer?search=1-PROP011-FLAT-101"

# List searches
curl "http://localhost:5000/api/wtis/consumer?search=Ward:1&pageSize=50"
curl "http://localhost:5000/api/wtis/consumer?search=Zone:1&pageSize=100"
curl "http://localhost:5000/api/wtis/consumer?search=Name:Sharma&pageSize=20"

# Short form
curl "http://localhost:5000/api/wtis/consumer?search=W:1&pageSize=50"
curl "http://localhost:5000/api/wtis/consumer?search=Z:1&pageSize=100"
curl "http://localhost:5000/api/wtis/consumer?search=N:Sharma&pageSize=20"
```

---

## ?? All Search Types

### Direct Value (Single Consumer)

| What You Search | Example | Returns |
|-----------------|---------|---------|
| Consumer Number | `?search=CON001` | One consumer |
| Mobile Number | `?search=9876543210` | One consumer |
| Consumer Name | `?search=Raj Sharma` | One consumer |
| Email | `?search=raj@example.com` | One consumer |
| Consumer ID | `?search=41226` | One consumer |
| Property | `?search=PROP001` | One consumer |
| Pattern | `?search=1-PROP011-FLAT-101` | One consumer |

### Prefix Value (Multiple Consumers)

| Prefix | Example | Returns |
|--------|---------|---------|
| `Ward:` or `W:` | `?search=Ward:1` | All in ward 1 |
| `Zone:` or `Z:` | `?search=Zone:1` | All in zone 1 |
| `Name:` or `N:` | `?search=Name:Sharma` | All with name "Sharma" |
| `Property:` or `P:` | `?search=Property:PROP011` | All in property |
| `Mobile:` or `M:` | `?search=Mobile:9876543210` | Filter by mobile |

---

## ? Performance Tips

1. **Direct searches** (no prefix) are fastest - exact match lookup
2. **Ward/Zone searches** use indexed fields - very fast
3. **Name searches** check both Hindi and English - slightly slower
4. **Use pagination** for large result sets

---

## ? Why This is Better

### Before (Multiple Parameters - Confusing)
```
?search=CON001
?consumerNumber=CON
?wardNo=1
?zoneNo=1
?consumerName=Sharma
?mobileNumber=9876543210
?propertyNumber=PROP001
```
?? **Which parameter should I use?**

### After (ONE Parameter - Simple)
```
?search=CON001
?search=Ward:1
?search=Zone:1
?search=Name:Sharma
?search=Mobile:9876543210
?search=Property:PROP001
```
?? **Always use `search`!**

---

## ?? Decision Tree

```
User provides: ?search={value}
                    ?
            Does it have a prefix?
                    ?
        ?????????????????????????
       YES                      NO
        ?                        ?
Parse prefix type          Single search
(Ward:, Zone:, etc.)      (exact match)
        ?                        ?
Return LIST with          Return ONE
    pagination            consumer
```

---

## ?? Complete API Reference

### Endpoint
```
GET /api/wtis/consumer
```

### Parameters

| Parameter | Required | Default | Max | Description |
|-----------|----------|---------|-----|-------------|
| `search` | ? Yes | - | - | Universal search value |
| `pageNumber` | ? No | 1 | - | Page number (for lists) |
| `pageSize` | ? No | 10 | 100 | Items per page (for lists) |

### Response Codes

| Code | Description |
|------|-------------|
| 200 | Success - consumer(s) found |
| 404 | Not found - no matches |
| 400 | Bad request - search parameter missing |
| 500 | Server error |

---

## ?? Summary

? **ONE parameter:** `search`  
? **TWO modes:** Direct value (single) or Prefix (list)  
? **FIVE prefixes:** Ward:, Zone:, Name:, Property:, Mobile:  
? **SHORT forms:** W:, Z:, N:, P:, M:  
? **SMART detection:** Automatic  
? **MASTER data:** Always included (ConnectionTypeName, CategoryName, PipeSize)  

---

## ?? Quick Start

```bash
# Test single search
curl "http://localhost:5000/api/wtis/consumer?search=CON001"

# Test list search
curl "http://localhost:5000/api/wtis/consumer?search=Ward:1"

# Test short form
curl "http://localhost:5000/api/wtis/consumer?search=W:1&pageSize=50"
```

---

**The simplest API ever - just ONE parameter for EVERYTHING!** ??
