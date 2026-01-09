# WTIS Consumer API - Smart Hierarchical Search

## ?? ONE Parameter, THREE Levels

**Super intelligent search that understands hierarchy:**

```
GET /api/wtis/consumer?search={anything}
```

---

## ?? How It Works - Hierarchical Pattern Detection

The API automatically detects the level of detail you provide:

### Level 1: Ward Only
**Format:** `?search={WardNo}`

Returns **ALL consumers** in that ward.

```bash
GET /api/wtis/consumer?search=1
GET /api/wtis/consumer?search=3
```

**Response:** List of all consumers in Ward 1 (or Ward 3)

---

### Level 2: Ward + Property
**Format:** `?search={WardNo}-{PropertyNo}`

Returns **ALL consumers** in that property (all partitions/flats).

```bash
GET /api/wtis/consumer?search=1-PROP011
GET /api/wtis/consumer?search=3-PROP009
```

**Response:** List of all consumers in that property (all flats/units)

---

### Level 3: Ward + Property + Partition
**Format:** `?search={WardNo}-{PropertyNo}-{PartitionNo}`

Returns **EXACT consumer** in that specific partition.

```bash
GET /api/wtis/consumer?search=1-PROP011-FLAT-101
GET /api/wtis/consumer?search=3-PROP009-PART009
```

**Response:** Single consumer (exact match)

---

## ?? Complete Search Modes

### 1. Hierarchical Search (Pattern with `-`)

| Pattern | Example | Returns |
|---------|---------|---------|
| `{Ward}` | `?search=1` | **LIST:** All consumers in Ward 1 |
| `{Ward}-{Property}` | `?search=1-PROP011` | **LIST:** All in property |
| `{Ward}-{Property}-{Partition}` | `?search=1-PROP011-FLAT-101` | **ONE:** Exact consumer |

### 2. Direct Identifier Search (No `-`)

| Type | Example | Returns |
|------|---------|---------|
| Consumer Number | `?search=CON001` | **ONE:** Exact consumer |
| Mobile Number | `?search=9876543210` | **ONE:** Exact consumer |
| Consumer Name | `?search=Raj Sharma` | **ONE:** Exact consumer |
| Email | `?search=raj@example.com` | **ONE:** Exact consumer |
| Consumer ID | `?search=41226` | **ONE:** Exact consumer |

---

## ?? Real-World Examples

### Example 1: Find All in Ward 1
**Request:**
```
GET /api/wtis/consumer?search=1&pageSize=50
```

**Response (200 OK):**
```json
{
  "items": [
    {
      "consumerNumber": "CON001",
      "wardNo": "1",
      "propertyNumber": "PROP001",
      "consumerName": "??? ?????"
    },
    {
      "consumerNumber": "CON011",
      "wardNo": "1",
      "propertyNumber": "PROP011",
      "partitionNumber": "FLAT-101",
      "consumerName": "????? ???????"
    }
  ],
  "totalCount": 50,
  "pageNumber": 1,
  "pageSize": 50
}
```

---

### Example 2: Find All in Sundar Apartment
**Request:**
```
GET /api/wtis/consumer?search=1-PROP011
```

**Response (200 OK):**
```json
{
  "items": [
    {
      "consumerNumber": "CON011",
      "wardNo": "1",
      "propertyNumber": "PROP011",
      "partitionNumber": "FLAT-101",
      "consumerName": "????? ???????"
    },
    {
      "consumerNumber": "CON012",
      "wardNo": "1",
      "propertyNumber": "PROP011",
      "partitionNumber": "FLAT-102",
      "consumerName": "????? ????"
    },
    {
      "consumerNumber": "CON013",
      "wardNo": "1",
      "propertyNumber": "PROP011",
      "partitionNumber": "FLAT-201",
      "consumerName": "?????? ????????"
    }
  ],
  "totalCount": 3,
  "pageNumber": 1,
  "pageSize": 10
}
```

---

### Example 3: Find Exact Flat
**Request:**
```
GET /api/wtis/consumer?search=1-PROP011-FLAT-101
```

**Response (200 OK):**
```json
{
  "consumerID": 41236,
  "consumerNumber": "CON011",
  "wardNo": "1",
  "propertyNumber": "PROP011",
  "partitionNumber": "FLAT-101",
  "consumerName": "????? ???????",
  "consumerNameEnglish": "Rajesh Gaikwad",
  "mobileNumber": "9123456789",
  "connectionTypeName": "Residential",
  "categoryName": "Domestic",
  "pipeSize": "15mm",
  "isActive": true
}
```

---

## ?? Smart Detection Logic

```
Input: "1"
       ?
    Is it a number?
       ? YES
    Is it > 100?
       ? NO
    Treat as WARD
       ?
Return ALL consumers in Ward 1
```

```
Input: "1-PROP011"
       ?
    Contains "-"?
       ? YES
    Split by "-"
       ?
    2 parts found
       ?
    Ward=1, Property=PROP011
       ?
Return ALL consumers in that property
```

```
Input: "1-PROP011-FLAT-101"
       ?
    Contains "-"?
       ? YES
    Split by "-"
       ?
    3 parts found
       ?
    Ward=1, Property=PROP011, Partition=FLAT-101
       ?
Try to find EXACT match first
       ?
    Found? Return ONE consumer
    Not found? Return ALL in property
```

---

## ?? Usage Scenarios

### Scenario 1: Admin Needs Ward Report
**User:** "Show me all consumers in Ward 1"
```sh
curl "http://localhost:5000/api/wtis/consumer?search=1&pageSize=100"
```

### Scenario 2: Building Manager
**User:** "Show me all consumers in Sundar Apartment"
```sh
curl "http://localhost:5000/api/wtis/consumer?search=1-PROP011"
```

### Scenario 3: Maintenance Visit
**User:** "I need details for Flat 101 in Sundar Apartment"
```sh
curl "http://localhost:5000/api/wtis/consumer?search=1-PROP011-FLAT-101"
```

### Scenario 4: Customer Service
**User:** "Customer says their number is CON011"
```sh
curl "http://localhost:5000/api/wtis/consumer?search=CON011"
```

### Scenario 5: Mobile Verification
**User:** "Mobile number: 9123456789"
```sh
curl "http://localhost:5000/api/wtis/consumer?search=9123456789"
```

---

## ?? Code Examples

### JavaScript/TypeScript

```typescript
// ONE function for all searches
async function search(value: string, page = 1, size = 10) {
  const params = new URLSearchParams({
    search: value,
    pageNumber: page.toString(),
    pageSize: size.toString()
  });
  
  const response = await fetch(`/api/wtis/consumer?${params}`);
  return await response.json();
}

// Usage - same function, different results based on input
const ward = await search('1', 1, 100);              // All in Ward 1
const property = await search('1-PROP011');          // All in property
const exact = await search('1-PROP011-FLAT-101');    // Exact consumer
const consumer = await search('CON001');             // By number
const mobile = await search('9876543210');           // By mobile
```

### C# (.NET)

```csharp
async Task<object> Search(string searchValue, int page = 1, int size = 10)
{
    var query = $"?search={Uri.EscapeDataString(searchValue)}&pageNumber={page}&pageSize={size}";
    var response = await httpClient.GetAsync($"/api/wtis/consumer{query}");
    return await response.Content.ReadFromJsonAsync<object>();
}

// Usage - same method, different results
var ward = await Search("1", 1, 100);              // All in Ward 1
var property = await Search("1-PROP011");          // All in property
var exact = await Search("1-PROP011-FLAT-101");    // Exact consumer
var consumer = await Search("CON001");             // By number
var mobile = await Search("9876543210");           // By mobile
```

### Python

```python
def search(value, page=1, size=10):
    params = {
        'search': value,
        'pageNumber': page,
        'pageSize': size
    }
    response = requests.get('http://localhost:5000/api/wtis/consumer', params=params)
    return response.json()

# Usage - same function, different results
ward = search('1', page=1, size=100)              # All in Ward 1
property = search('1-PROP011')                    # All in property
exact = search('1-PROP011-FLAT-101')              # Exact consumer
consumer = search('CON001')                       # By number
mobile = search('9876543210')                     # By mobile
```

---

## ?? Complete Search Reference

| Input | Format | Returns | Pagination |
|-------|--------|---------|------------|
| `1` | Ward only | **LIST** of all in ward | ? Yes |
| `3` | Ward only | **LIST** of all in ward | ? Yes |
| `1-PROP011` | Ward-Property | **LIST** of all in property | ? Yes |
| `3-PROP009` | Ward-Property | **LIST** of all in property | ? Yes |
| `1-PROP011-FLAT-101` | Ward-Property-Partition | **ONE** exact consumer | ? No |
| `CON001` | Consumer Number | **ONE** exact consumer | ? No |
| `9876543210` | Mobile | **ONE** exact consumer | ? No |
| `Raj Sharma` | Name | **ONE** exact consumer | ? No |
| `raj@example.com` | Email | **ONE** exact consumer | ? No |

---

## ? Performance

| Search Type | Speed | Reason |
|-------------|-------|--------|
| Ward only | Fast | Indexed WardNo field |
| Ward-Property | Fast | Indexed WardNo + PropertyNumber |
| Ward-Property-Partition | Fastest | Indexed fields + exact match |
| Consumer Number | Fastest | Unique index |
| Mobile | Fast | Indexed field |
| Name | Moderate | Full-text search on 2 fields |

---

## ? Advantages

### Before (Multiple APIs)
```
? /api/consumer/ward/{wardNo}
? /api/consumer/property/{wardNo}/{propertyNo}
? /api/consumer/{wardNo}/{propertyNo}/{partitionNo}
? /api/consumer/find?consumerNumber=CON001
? /api/consumer/find?mobile=9876543210
```
?? **Which API should I use?**

### After (ONE API)
```
? /api/wtis/consumer?search=1
? /api/wtis/consumer?search=1-PROP011
? /api/wtis/consumer?search=1-PROP011-FLAT-101
? /api/wtis/consumer?search=CON001
? /api/wtis/consumer?search=9876543210
```
?? **Just use `?search=` with the value you have!**

---

## ?? Decision Tree

```
Input Received
      ?
  Contains "-"?
      ?
 ???????????
YES        NO
 ?          ?
Split   Is Number?
Parts      ?
 ?     ?????????
2?    YES      NO
3?     ?        ?
 ?    Ward   Try Single
List  List   Search
      or      ?
     Single  Consumer#
             Mobile
             Name
             Email
```

---

## ?? Summary

? **ONE parameter** - `search`  
? **THREE levels** - Ward / Ward-Property / Ward-Property-Partition  
? **SMART detection** - Automatic hierarchical parsing  
? **UNIFIED API** - Same endpoint for everything  
? **MASTER data** - Always includes ConnectionTypeName, CategoryName, PipeSize  
? **PAGINATION** - Available for list results  

---

## ?? Quick Test

```bash
# Ward only (all consumers)
curl "http://localhost:5000/api/wtis/consumer?search=1"

# Ward + Property (all in property)
curl "http://localhost:5000/api/wtis/consumer?search=1-PROP011"

# Ward + Property + Partition (exact)
curl "http://localhost:5000/api/wtis/consumer?search=1-PROP011-FLAT-101"

# Consumer Number
curl "http://localhost:5000/api/wtis/consumer?search=CON001"

# Mobile
curl "http://localhost:5000/api/wtis/consumer?search=9876543210"
```

---

**The smartest API ever - automatically understands the level of detail you need!** ??

Just enter what you have, and the API figures out the rest! ??
