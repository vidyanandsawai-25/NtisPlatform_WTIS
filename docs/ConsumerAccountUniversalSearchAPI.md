# Consumer Account Universal Search API

## Overview
A streamlined API for Consumer Accounts with a powerful universal search endpoint that allows you to find consumer accounts using **any single parameter**.

## ?? Available Endpoints

### 1. Universal Search (Primary Endpoint) ?
```
GET /api/wtis/consumeraccount/find-consumer?search={value}
```

### 2. Get by ID
```
GET /api/wtis/consumeraccount/{id}
```

### 3. Get All with Filtering & Pagination
```
GET /api/wtis/consumeraccount?[filters]
```

---

## ?? Universal Search Endpoint

### Endpoint
```
GET /api/wtis/consumeraccount/find-consumer?search={value}
```

### Description
The intelligent universal search endpoint that automatically determines what type of search you're performing based on the input.

### Supported Search Types

#### 1. **Consumer Number**
Search by exact consumer number.

**Example:**
```
GET /api/wtis/consumeraccount/find-consumer?search=CON001
```

#### 2. **Mobile Number**
Search by exact mobile number.

**Example:**
```
GET /api/wtis/consumeraccount/find-consumer?search=9876543210
```

#### 3. **Consumer Name**
Search by exact consumer name (Hindi or English).

**Examples:**
```
GET /api/wtis/consumeraccount/find-consumer?search=Raj Sharma
GET /api/wtis/consumeraccount/find-consumer?search=??? ?????
```

#### 4. **Email Address**
Search by email.

**Example:**
```
GET /api/wtis/consumeraccount/find-consumer?search=raj@example.com
```

#### 5. **Ward-Property-Partition Pattern** ? **SPECIAL**
Search using the composite pattern: `WardNo-PropertyNumber-PartitionNumber`

This is useful when you have multiple consumers in the same property/building.

**Examples:**

Full Pattern (Ward-Property-Partition):
```
GET /api/wtis/consumeraccount/find-consumer?search=1-PROP001-PART001
```

Partial Pattern (Ward-Property):
```
GET /api/wtis/consumeraccount/find-consumer?search=1-PROP011
```

Finding Apartment Units:
```
GET /api/wtis/consumeraccount/find-consumer?search=1-PROP011-FLAT-101
GET /api/wtis/consumeraccount/find-consumer?search=1-PROP011-FLAT-102
GET /api/wtis/consumeraccount/find-consumer?search=1-PROP011-FLAT-201
```

#### 6. **Property Number**
Search by property number alone.

**Example:**
```
GET /api/wtis/consumeraccount/find-consumer?search=PROP001
```

#### 7. **Partition Number**
Search by partition/flat number.

**Example:**
```
GET /api/wtis/consumeraccount/find-consumer?search=FLAT-101
```

### Response Format

#### Success Response (200 OK)
```json
{
  "consumerID": 41236,
  "consumerNumber": "CON011",
  "oldConsumerNumber": "OLD011",
  "zoneNo": "1",
  "wardNo": "1",
  "propertyNumber": "PROP011",
  "partitionNumber": "FLAT-101",
  "consumerName": "????? ???????",
  "consumerNameEnglish": "Rajesh Gaikwad",
  "mobileNumber": "9123456789",
  "emailID": "rajesh@example.com",
  "address": "????? ?????????? ????? 101",
  "addressEnglish": "Sundar Apartment Flat 101",
  "connectionTypeID": "1",
  "categoryID": "2",
  "pipeSizeID": "1",
  "connectionDate": "2024-11-01T00:00:00",
  "isActive": true,
  "remark": "Apartment Unit",
  "createdDate": "2026-01-07T15:03:48.58",
  "updatedDate": "2026-01-07T15:03:48.58"
}
```

#### Not Found Response (404)
```json
{
  "message": "Consumer account not found.",
  "searchValue": "CON999",
  "hint": "Try searching with: Consumer Number, Mobile Number, Consumer Name, or Ward-Property-Partition pattern (e.g., 1-PROP001-PART001)"
}
```

#### Bad Request (400)
```json
{
  "message": "Search parameter is required."
}
```

#### Server Error (500)
```json
{
  "message": "An error occurred while searching for the consumer account."
}
```

### Search Priority

When searching with the Ward-Property-Partition pattern:
1. First tries to match the pattern (e.g., `1-PROP011-FLAT-101`)
2. If no match, falls back to individual field matching

When searching with a simple value:
- Checks: Consumer Number ? Mobile Number ? Consumer Name ? Consumer Name (English) ? Old Consumer Number ? Email ? Property Number ? Partition Number
- Returns the **first exact match** found

---

## ?? Get by ID Endpoint

### Endpoint
```
GET /api/wtis/consumeraccount/{id}
```

### Description
Retrieve a specific consumer account by its unique ID.

### Example
```
GET /api/wtis/consumeraccount/41226
```

### Response (200 OK)
Same format as universal search response.

---

## ?? Get All with Filtering Endpoint

### Endpoint
```
GET /api/wtis/consumeraccount?[parameters]
```

### Description
Get a paginated list of consumer accounts with optional filtering and sorting.

### Query Parameters

| Parameter | Type | Description | Example |
|-----------|------|-------------|---------|
| `pageNumber` | int | Page number (default: 1) | `1` |
| `pageSize` | int | Items per page (default: 10, max: 100) | `20` |
| `searchTerm` | string | Search across multiple fields | `Raj` |
| `consumerNumber` | string | Filter by consumer number | `CON001` |
| `mobileNumber` | string | Filter by mobile number | `9876543210` |
| `wardNo` | string | Filter by ward number | `1` |
| `zoneNo` | string | Filter by zone number | `1` |
| `propertyNumber` | string | Filter by property | `PROP011` |
| `consumerName` | string | Filter by consumer name | `Raj` |
| `isActive` | bool | Filter by active status | `true` |
| `connectionDateFrom` | date | Connection date from | `2024-01-01` |
| `connectionDateTo` | date | Connection date to | `2024-12-31` |
| `sortBy` | string | Sort field | `ConsumerName` |
| `sortOrder` | string | Sort direction (asc/desc) | `asc` |

### Examples

Get all consumers in Ward 1:
```
GET /api/wtis/consumeraccount?wardNo=1&pageSize=50
```

Search by name and sort:
```
GET /api/wtis/consumeraccount?searchTerm=Raj&sortBy=ConsumerName&sortOrder=asc
```

Filter by zone and ward:
```
GET /api/wtis/consumeraccount?zoneNo=1&wardNo=1&isActive=true
```

Get active consumers with pagination:
```
GET /api/wtis/consumeraccount?isActive=true&pageNumber=1&pageSize=20
```

### Response (200 OK)
```json
{
  "items": [
    {
      "consumerID": 41226,
      "consumerNumber": "CON001",
      "consumerName": "??? ?????",
      "consumerNameEnglish": "Raj Sharma",
      "mobileNumber": "9876543210",
      "isActive": true
    }
  ],
  "totalCount": 150,
  "pageNumber": 1,
  "pageSize": 20,
  "totalPages": 8,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

---

## ?? Real-World Examples

### Example 1: Customer Service Lookup
Customer calls: "My consumer number is CON001"
```bash
curl "http://localhost:5000/api/wtis/consumeraccount/find-consumer?search=CON001"
```

### Example 2: Mobile Number Search
Customer: "I forgot my consumer number, but my mobile is 9876543210"
```bash
curl "http://localhost:5000/api/wtis/consumeraccount/find-consumer?search=9876543210"
```

### Example 3: Apartment Building Management
Find a specific flat in Sundar Apartment:
```bash
curl "http://localhost:5000/api/wtis/consumeraccount/find-consumer?search=1-PROP011-FLAT-101"
```

### Example 4: List All Consumers in a Ward
Get all consumers in Ward 1:
```bash
curl "http://localhost:5000/api/wtis/consumeraccount?wardNo=1&pageSize=100"
```

### Example 5: Active Consumers Report
Get active consumers sorted by name:
```bash
curl "http://localhost:5000/api/wtis/consumeraccount?isActive=true&sortBy=ConsumerName&pageSize=50"
```

---

## ?? Usage in Code

### JavaScript/TypeScript
```javascript
// Universal Search
async function findConsumer(searchValue) {
  const response = await fetch(
    `/api/wtis/consumeraccount/find-consumer?search=${encodeURIComponent(searchValue)}`
  );
  
  if (!response.ok) {
    if (response.status === 404) {
      console.log('Consumer not found');
      return null;
    }
    throw new Error('Search failed');
  }
  
  return await response.json();
}

// Get by ID
async function getConsumerById(id) {
  const response = await fetch(`/api/wtis/consumeraccount/${id}`);
  return await response.json();
}

// Get all with filters
async function getConsumers(filters) {
  const params = new URLSearchParams(filters);
  const response = await fetch(`/api/wtis/consumeraccount?${params}`);
  return await response.json();
}

// Usage
const consumer = await findConsumer('CON001');
const byId = await getConsumerById(41226);
const list = await getConsumers({ wardNo: '1', isActive: true, pageSize: 20 });
```

### C# (.NET)
```csharp
using var httpClient = new HttpClient { BaseAddress = new Uri("https://your-api-url/") };

// Universal Search
var searchValue = "CON001";
var searchResponse = await httpClient.GetAsync(
    $"api/wtis/consumeraccount/find-consumer?search={Uri.EscapeDataString(searchValue)}"
);
var consumer = await searchResponse.Content.ReadFromJsonAsync<ConsumerAccountDto>();

// Get by ID
var byIdResponse = await httpClient.GetAsync("api/wtis/consumeraccount/41226");
var consumerById = await byIdResponse.Content.ReadFromJsonAsync<ConsumerAccountDto>();

// Get all with filters
var listResponse = await httpClient.GetAsync(
    "api/wtis/consumeraccount?wardNo=1&isActive=true&pageSize=20"
);
var pagedResult = await listResponse.Content.ReadFromJsonAsync<PagedResult<ConsumerAccountDto>>();
```

### cURL
```bash
# Universal search
curl "http://localhost:5000/api/wtis/consumeraccount/find-consumer?search=CON001"

# Get by ID
curl "http://localhost:5000/api/wtis/consumeraccount/41226"

# Get all with filters
curl "http://localhost:5000/api/wtis/consumeraccount?wardNo=1&isActive=true&pageSize=20"
```

---

## ?? URL Encoding

Remember to URL-encode special characters:
- Spaces: `%20` or `+`
- Hindi characters: UTF-8 encoded

**Examples:**
```
Raj Sharma ? Raj%20Sharma
??? ????? ? %E0%A4%B0%E0%A4%BE%E0%A4%9A%20%E0%A4%B6%E0%A4%B0%E0%A5%8D%E0%A4%AE%E0%A4%BE
```

---

## ? Performance Tips

1. **Universal Search (`find-consumer`)** - Fastest for single lookups
   - Consumer Number: ~20ms
   - Mobile Number: ~20ms
   - Pattern search: ~30ms

2. **Get by ID** - Fastest (direct primary key lookup)
   - Response time: ~10ms

3. **Get All with Filters** - Good for reports/lists
   - With indexes: ~50-200ms depending on result set
   - Use pagination for better performance

---

## ?? Testing

### Testing with Swagger/OpenAPI
Navigate to: `http://localhost:5000/swagger`

1. Find `ConsumerAccount` section
2. Test the three available endpoints
3. Try different search values and filters

### Testing with HTML Test Page
Open: `docs/ConsumerSearchTestPage.html`

### Testing with Postman
Import the API and test:
1. Universal Search: GET `{{baseUrl}}/api/wtis/consumeraccount/find-consumer?search=CON001`
2. Get by ID: GET `{{baseUrl}}/api/wtis/consumeraccount/41226`
3. Get All: GET `{{baseUrl}}/api/wtis/consumeraccount?wardNo=1&pageSize=20`

---

## ?? API Summary

| Endpoint | Method | Purpose | Response Time |
|----------|--------|---------|---------------|
| `/find-consumer` | GET | Universal search by any parameter | ~20-50ms |
| `/{id}` | GET | Get specific consumer by ID | ~10ms |
| `/` | GET | List/filter consumers with pagination | ~50-200ms |

---

## ? Best Practices

### For Single Lookups
? **Use Universal Search** (`find-consumer`)
```
GET /api/wtis/consumeraccount/find-consumer?search=CON001
```

### For Known ID
? **Use Get by ID**
```
GET /api/wtis/consumeraccount/41226
```

### For Lists/Reports
? **Use Get All with Filters**
```
GET /api/wtis/consumeraccount?wardNo=1&isActive=true&pageSize=50
```

---

## ?? Summary

### Three Clean Endpoints:

1. **?? Universal Search** - Find by anything
   ```
   GET /api/wtis/consumeraccount/find-consumer?search={anything}
   ```

2. **?? Get by ID** - Direct lookup
   ```
   GET /api/wtis/consumeraccount/{id}
   ```

3. **?? Get All** - Lists with filters
   ```
   GET /api/wtis/consumeraccount?[filters]
   ```

**Simple, Clean, Powerful!** ??
