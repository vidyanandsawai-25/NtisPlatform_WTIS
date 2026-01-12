# WTIS API - Complete Documentation & Testing Guide

## ?? Master Tables Overview

WTIS module consists of **6 master tables** with full CRUD operations:

| Table | Endpoint | Records | Description |
|-------|----------|---------|-------------|
| ZoneMaster | `/api/wtis/zone-master` | Zones | Geographic zones |
| WardMaster | `/api/wtis/ward-master` | Wards | Wards within zones |
| ConnectionTypeMaster | `/api/wtis/connection-type` | Connection types | Water connection types |
| ConnectionCategoryMaster | `/api/wtis/connection-category` | Categories | Connection categories |
| PipeSizeMaster | `/api/wtis/pipe-size` | Pipe sizes | Available pipe diameters |
| RateMaster | `/api/wtis/rate-master` | Rates | Water rates by configuration |

---

## ?? API Endpoints (30 Total)

All masters support 5 CRUD operations:
- `GET /api/wtis/{master}` - List with pagination, filtering, sorting
- `GET /api/wtis/{master}/{id}` - Get single record by ID
- `POST /api/wtis/{master}` - Create new record
- `PUT /api/wtis/{master}/{id}` - Update existing record
- `DELETE /api/wtis/{master}/{id}` - Delete record

---

## 1?? Zone Master

**Endpoint:** `/api/wtis/zone-master`

### Database Columns
```sql
ZoneID          INT (PK, Identity)
ZoneName        NVARCHAR(100) NOT NULL
ZoneCode        NVARCHAR(50) NOT NULL UNIQUE
IsActive        BIT NOT NULL (Default: 1)
CreatedBy       INT NOT NULL
CreatedDate     DATETIME NOT NULL (Default: GETDATE())
UpdatedBy       INT NULL
UpdatedDate     DATETIME NULL
```

### GET List
```http
GET /api/wtis/zone-master?pageNumber=1&pageSize=10&sortBy=zoneName&sortOrder=asc&isActive=true
```

**Query Parameters:**
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| pageNumber | int | No | Page number (default: 1) |
| pageSize | int | No | Records per page (default: 10, max: 100) |
| sortBy | string | No | Field to sort (zoneName, zoneCode) |
| sortOrder | string | No | Sort direction (asc, desc) |
| isActive | bool | No | Filter by active status |

**Response:**
```json
{
  "items": [
    {
      "zoneID": 1,
      "zoneName": "Zone A",
      "zoneCode": "Z001",
      "isActive": true,
      "createdBy": 1,
      "createdDate": "2024-01-15T10:00:00",
      "updatedBy": null,
      "updatedDate": null
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 1,
  "totalPages": 1,
  "hasPreviousPage": false,
  "hasNextPage": false
}
```

### GET By ID
```http
GET /api/wtis/zone-master/1
```

**Response:**
```json
{
  "zoneID": 1,
  "zoneName": "Zone A",
  "zoneCode": "Z001",
  "isActive": true,
  "createdBy": 1,
  "createdDate": "2024-01-15T10:00:00",
  "updatedBy": null,
  "updatedDate": null
}
```

### POST Create
```http
POST /api/wtis/zone-master
Content-Type: application/json

{
  "zoneName": "Zone A",
  "zoneCode": "Z001",
  "isActive": true,
  "createdBy": 1
}
```

**Response:**
```json
{
  "success": true,
  "message": "Record inserted successfully",
  "items": {
    "zoneID": 1,
    "zoneName": "Zone A",
    "zoneCode": "Z001",
    "isActive": true,
    "createdBy": 1,
    "createdDate": "2024-01-15T10:00:00",
    "updatedBy": null,
    "updatedDate": null
  }
}
```

### PUT Update
```http
PUT /api/wtis/zone-master/1
Content-Type: application/json

{
  "zoneName": "Zone A Updated",
  "isActive": true,
  "updatedBy": 1
}
```

### DELETE
```http
DELETE /api/wtis/zone-master/1
```

**Response:**
```json
{
  "success": true,
  "message": "Record deleted"
}
```

---

## 2?? Ward Master (with Zone JOIN)

**Endpoint:** `/api/wtis/ward-master`

### Database Columns
```sql
WardID          INT (PK, Identity)
WardName        NVARCHAR(100) NOT NULL
WardCode        NVARCHAR(50) NOT NULL UNIQUE
ZoneID          INT NOT NULL (FK -> ZoneMaster)
IsActive        BIT NOT NULL (Default: 1)
CreatedBy       INT NOT NULL
CreatedDate     DATETIME NOT NULL (Default: GETDATE())
UpdatedBy       INT NULL
UpdatedDate     DATETIME NULL
```

### GET List (Returns Zone Name)
```http
GET /api/wtis/ward-master?pageNumber=1&pageSize=10&wardName=Ward&isActive=true
```

**Query Parameters:**
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| pageNumber | int | No | Page number |
| pageSize | int | No | Records per page |
| sortBy | string | No | Field to sort (wardName, zoneName) |
| sortOrder | string | No | Sort direction |
| wardName | string | No | Filter by ward name (contains) |
| isActive | bool | No | Filter by active status |

**Response (with Zone info):**
```json
{
  "items": [
    {
      "wardID": 1,
      "wardName": "Ward 1",
      "wardCode": "W001",
      "zoneID": 1,
      "zoneName": "Zone A",
      "isActive": true,
      "createdBy": 1,
      "createdDate": "2024-01-15T10:00:00",
      "updatedBy": null,
      "updatedDate": null
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 1,
  "totalPages": 1
}
```

### POST Create (Send ZoneID, Get Zone Name)
```http
POST /api/wtis/ward-master
Content-Type: application/json

{
  "wardName": "Ward 1",
  "wardCode": "W001",
  "zoneID": 1,
  "isActive": true,
  "createdBy": 1
}
```

**Response (enriched with Zone):**
```json
{
  "success": true,
  "message": "Record inserted successfully",
  "items": {
    "wardID": 1,
    "wardName": "Ward 1",
    "wardCode": "W001",
    "zoneID": 1,
    "zoneName": "Zone A",
    "isActive": true,
    "createdBy": 1,
    "createdDate": "2024-01-15T10:00:00"
  }
}
```

---

## 3?? Connection Type Master

**Endpoint:** `/api/wtis/connection-type`

### Database Columns
```sql
ConnectionTypeID    INT (PK, Identity)
ConnectionTypeName  NVARCHAR(100) NOT NULL UNIQUE
Description         NVARCHAR(500) NULL
IsActive            BIT NOT NULL (Default: 1)
CreatedBy           INT NOT NULL
CreatedDate         DATETIME NOT NULL (Default: GETDATE())
UpdatedBy           INT NULL
UpdatedDate         DATETIME NULL
```

### POST Create
```http
POST /api/wtis/connection-type
Content-Type: application/json

{
  "connectionTypeName": "Domestic",
  "description": "Residential water connection",
  "isActive": true,
  "createdBy": 1
}
```

**Response:**
```json
{
  "success": true,
  "message": "Record inserted successfully",
  "items": {
    "connectionTypeID": 1,
    "connectionTypeName": "Domestic",
    "description": "Residential water connection",
    "isActive": true,
    "createdBy": 1,
    "createdDate": "2024-01-15T10:00:00"
  }
}
```

---

## 4?? Connection Category Master

**Endpoint:** `/api/wtis/connection-category`

### Database Columns
```sql
CategoryID      INT (PK, Identity)
CategoryName    NVARCHAR(100) NOT NULL UNIQUE
Description     NVARCHAR(500) NULL
IsActive        BIT NOT NULL (Default: 1)
CreatedBy       INT NOT NULL
CreatedDate     DATETIME NOT NULL (Default: GETDATE())
UpdatedBy       INT NULL
UpdatedDate     DATETIME NULL
```

### POST Create
```http
POST /api/wtis/connection-category
Content-Type: application/json

{
  "categoryName": "Residential",
  "description": "Residential category",
  "isActive": true,
  "createdBy": 1
}
```

---

## 5?? Pipe Size Master

**Endpoint:** `/api/wtis/pipe-size`

### Database Columns
```sql
PipeSizeID      INT (PK, Identity)
SizeName        NVARCHAR(50) NOT NULL UNIQUE
DiameterMM      DECIMAL(10,2) NOT NULL
IsActive        BIT NOT NULL (Default: 1)
CreatedBy       INT NOT NULL
CreatedDate     DATETIME NOT NULL (Default: GETDATE())
UpdatedBy       INT NULL
UpdatedDate     DATETIME NULL
```

### POST Create
```http
POST /api/wtis/pipe-size
Content-Type: application/json

{
  "sizeName": "15mm",
  "diameterMM": 15.00,
  "isActive": true,
  "createdBy": 1
}
```

**Response:**
```json
{
  "success": true,
  "message": "Record inserted successfully",
  "items": {
    "pipeSizeID": 1,
    "sizeName": "15mm",
    "diameterMM": 15.00,
    "isActive": true,
    "createdBy": 1,
    "createdDate": "2024-01-15T10:00:00"
  }
}
```

---

## 6?? Rate Master (with ALL JOINs)

**Endpoint:** `/api/wtis/rate-master`

### Database Columns
```sql
RateID                  INT (PK, Identity)
ZoneID                  INT NOT NULL (FK -> ZoneMaster)
WardID                  INT NOT NULL (FK -> WardMaster)
TapSizeID               INT NOT NULL (FK -> PipeSizeMaster)
ConnectionTypeID        INT NOT NULL (FK -> ConnectionTypeMaster)
ConnectionCategoryID    INT NOT NULL (FK -> ConnectionCategoryMaster)
MinReading              DECIMAL(18,2) NOT NULL
MaxReading              DECIMAL(18,2) NOT NULL
PerLiter                DECIMAL(18,4) NOT NULL
MinimumCharge           DECIMAL(18,2) NOT NULL
MeterOffPenalty         DECIMAL(18,2) NOT NULL
Rate                    DECIMAL(18,2) NOT NULL
Year                    INT NOT NULL
Remark                  NVARCHAR(500) NULL
IsActive                BIT NOT NULL (Default: 1)
CreatedBy               INT NOT NULL
CreatedDate             DATETIME NOT NULL (Default: GETDATE())
UpdatedBy               INT NULL
UpdatedDate             DATETIME NULL
```

### GET List (Returns ALL Master Names)
```http
GET /api/wtis/rate-master?zoneID=1&year=2024&isActive=true&pageNumber=1&pageSize=10
```

**Query Parameters:**
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| pageNumber | int | No | Page number |
| pageSize | int | No | Records per page |
| sortBy | string | No | Field (year, zoneName, wardName) |
| sortOrder | string | No | Sort direction |
| zoneID | int | No | Filter by zone |
| wardID | int | No | Filter by ward |
| tapSizeID | int | No | Filter by pipe size |
| connectionTypeID | int | No | Filter by connection type |
| connectionCategoryID | int | No | Filter by category |
| year | int | No | Filter by year |
| isActive | bool | No | Filter by active status |

**Response (enriched with all master names):**
```json
{
  "items": [
    {
      "rateID": 1,
      "zoneID": 1,
      "zoneName": "Zone A",
      "zoneCode": "Z001",
      "wardID": 1,
      "wardName": "Ward 1",
      "wardCode": "W001",
      "tapSizeID": 1,
      "tapSize": "15mm",
      "diameterMM": 15.00,
      "connectionTypeID": 1,
      "connectionTypeName": "Domestic",
      "connectionCategoryID": 1,
      "categoryName": "Residential",
      "minReading": 0.00,
      "maxReading": 99999.00,
      "perLiter": 1.0000,
      "minimumCharge": 120.00,
      "meterOffPenalty": 300.00,
      "rate": 120.00,
      "year": 2026,
      "remark": "Test",
      "isActive": true,
      "createdBy": 5,
      "createdDate": "2024-01-15T10:00:00",
      "updatedBy": null,
      "updatedDate": null
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 1,
  "totalPages": 1
}
```

### POST Create (Send IDs, Get All Names)
```http
POST /api/wtis/rate-master
Content-Type: application/json

{
  "zoneID": 1,
  "wardID": 1,
  "tapSizeID": 1,
  "connectionTypeID": 1,
  "connectionCategoryID": 1,
  "minReading": 0,
  "maxReading": 99999,
  "perLiter": 1,
  "minimumCharge": 120,
  "meterOffPenalty": 300,
  "rate": 120,
  "year": 2026,
  "remark": "Test rate for 2026",
  "isActive": true,
  "createdBy": 5
}
```

**Response (with all master names populated):**
```json
{
  "success": true,
  "message": "Record inserted successfully",
  "items": {
    "rateID": 1,
    "zoneID": 1,
    "zoneName": "Zone A",
    "zoneCode": "Z001",
    "wardID": 1,
    "wardName": "Ward 1",
    "wardCode": "W001",
    "tapSizeID": 1,
    "tapSize": "15mm",
    "diameterMM": 15.00,
    "connectionTypeID": 1,
    "connectionTypeName": "Domestic",
    "connectionCategoryID": 1,
    "categoryName": "Residential",
    "minReading": 0.00,
    "maxReading": 99999.00,
    "perLiter": 1.0000,
    "minimumCharge": 120.00,
    "meterOffPenalty": 300.00,
    "rate": 120.00,
    "year": 2026,
    "remark": "Test rate for 2026",
    "isActive": true,
    "createdBy": 5,
    "createdDate": "2024-01-15T10:00:00",
    "updatedBy": null,
    "updatedDate": null
  }
}
```

### PUT Update
```http
PUT /api/wtis/rate-master/1
Content-Type: application/json

{
  "perLiter": 1.5,
  "minimumCharge": 150,
  "year": 2027,
  "remark": "Updated for 2027",
  "updatedBy": 5
}
```

---

## ?? Complete Test Sequence

### Step 1: Create Master Data (Prerequisites)

```bash
# 1. Create Zone
POST /api/wtis/zone-master
{
  "zoneName": "Zone A",
  "zoneCode": "Z001",
  "isActive": true,
  "createdBy": 1
}
# Note: ZoneID = 1

# 2. Create Ward (uses ZoneID = 1)
POST /api/wtis/ward-master
{
  "wardName": "Ward 1",
  "wardCode": "W001",
  "zoneID": 1,
  "isActive": true,
  "createdBy": 1
}
# Note: WardID = 1

# 3. Create Connection Type
POST /api/wtis/connection-type
{
  "connectionTypeName": "Domestic",
  "description": "Residential connection",
  "isActive": true,
  "createdBy": 1
}
# Note: ConnectionTypeID = 1

# 4. Create Connection Category
POST /api/wtis/connection-category
{
  "categoryName": "Residential",
  "description": "Residential category",
  "isActive": true,
  "createdBy": 1
}
# Note: CategoryID = 1

# 5. Create Pipe Size
POST /api/wtis/pipe-size
{
  "sizeName": "15mm",
  "diameterMM": 15.00,
  "isActive": true,
  "createdBy": 1
}
# Note: PipeSizeID = 1
```

### Step 2: Create Rate (Uses All Master IDs)

```bash
POST /api/wtis/rate-master
{
  "zoneID": 1,
  "wardID": 1,
  "tapSizeID": 1,
  "connectionTypeID": 1,
  "connectionCategoryID": 1,
  "minReading": 0,
  "maxReading": 1000,
  "perLiter": 2.50,
  "minimumCharge": 100,
  "meterOffPenalty": 500,
  "rate": 150,
  "year": 2024,
  "remark": "Standard rate for 2024",
  "isActive": true,
  "createdBy": 1
}
```

### Step 3: Verify Data

```bash
# Get Rate with all JOINs
GET /api/wtis/rate-master/1

# Expected: All master names populated
# - zoneName: "Zone A"
# - wardName: "Ward 1"
# - tapSize: "15mm"
# - connectionTypeName: "Domestic"
# - categoryName: "Residential"
```

---

## ?? Error Responses

### 400 Bad Request (Validation Error)
```json
{
  "success": false,
  "message": "Validation failed",
  "errors": {
    "ZoneName": ["Zone Name is required"]
  }
}
```

### 404 Not Found
```json
{
  "success": false,
  "message": "Record not found for Update",
  "items": null
}
```

### 409 Conflict (Duplicate)
```json
{
  "success": false,
  "message": "A record with the same details already exists."
}
```

### 500 Internal Server Error
```json
{
  "success": false,
  "message": "An error occurred while processing your request."
}
```

---

## ?? Postman Collection Examples

### Environment Variables
```
baseUrl = https://localhost:44346
zoneId = 1
wardId = 1
tapSizeId = 1
connectionTypeId = 1
categoryId = 1
```

### Test Script (Post-Request)
```javascript
// Save IDs for subsequent requests
if (pm.response.code === 200) {
    const response = pm.response.json();
    if (response.success && response.items) {
        if (response.items.zoneID) {
            pm.environment.set("zoneId", response.items.zoneID);
        }
        if (response.items.wardID) {
            pm.environment.set("wardId", response.items.wardID);
        }
    }
}
```

---

## ? Validation Rules

### All Masters
- Required fields must not be empty
- String fields: No leading/trailing whitespace
- Numeric fields: Must be >= 0
- Year: Between 1900-9999
- IsActive: Boolean (true/false)
- CreatedBy/UpdatedBy: Must be valid user ID

### Unique Constraints
- ZoneMaster: ZoneCode
- WardMaster: WardCode
- ConnectionTypeMaster: ConnectionTypeName
- ConnectionCategoryMaster: CategoryName
- PipeSizeMaster: SizeName

### Foreign Key Validation
- WardMaster.ZoneID ? Must exist in ZoneMaster
- RateMaster.ZoneID ? Must exist in ZoneMaster
- RateMaster.WardID ? Must exist in WardMaster
- RateMaster.TapSizeID ? Must exist in PipeSizeMaster
- RateMaster.ConnectionTypeID ? Must exist in ConnectionTypeMaster
- RateMaster.ConnectionCategoryID ? Must exist in ConnectionCategoryMaster

---

## ?? Quick Start Testing

### Minimal Test Data

```bash
# Zone
POST /api/wtis/zone-master
{"zoneName":"Z1","zoneCode":"Z1","isActive":true,"createdBy":1}

# Ward
POST /api/wtis/ward-master
{"wardName":"W1","wardCode":"W1","zoneID":1,"isActive":true,"createdBy":1}

# Connection Type
POST /api/wtis/connection-type
{"connectionTypeName":"Domestic","isActive":true,"createdBy":1}

# Category
POST /api/wtis/connection-category
{"categoryName":"Residential","isActive":true,"createdBy":1}

# Pipe Size
POST /api/wtis/pipe-size
{"sizeName":"15mm","diameterMM":15,"isActive":true,"createdBy":1}

# Rate
POST /api/wtis/rate-master
{"zoneID":1,"wardID":1,"tapSizeID":1,"connectionTypeID":1,"connectionCategoryID":1,"minReading":0,"maxReading":1000,"perLiter":2.5,"minimumCharge":100,"meterOffPenalty":500,"rate":150,"year":2024,"isActive":true,"createdBy":1}
```

---

## ?? Summary

| Feature | Status |
|---------|--------|
| Total Masters | 6 |
| Total Endpoints | 30 |
| Simple Masters | 4 (Zone, ConnectionType, Category, PipeSize) |
| Complex Masters (JOINs) | 2 (Ward, Rate) |
| CRUD Operations | All supported |
| Pagination | ? |
| Filtering | ? |
| Sorting | ? |
| Validation | ? |
| Error Handling | ? |
| Audit Fields | ? (CreatedBy, CreatedDate, UpdatedBy, UpdatedDate) |

---

**All WTIS endpoints are ready for testing and production use!** ??
