# Consumer Account Entity - Database Schema Mapping

## ?? Exact Database Schema Alignment

This document shows the **exact mapping** between the SQL Server database and the .NET Entity.

## Database Table: `WTIS.ConsumerAccount`

### Column Definition (SQL Server)

```sql
CREATE TABLE [WTIS].[ConsumerAccount](
    [ConsumerID] [int] IDENTITY(1,1) NOT NULL,
    [ConsumerNumber] [varchar](40) NOT NULL,
    [OldConsumerNumber] [varchar](40) NULL,
    [ZoneNo] [varchar](50) NULL,
    [WardNo] [varchar](50) NULL,
    [PropertyNumber] [varchar](50) NULL,
    [PartitionNumber] [varchar](50) NULL,
    [ConsumerName] [nvarchar](200) NOT NULL,
    [ConsumerNameEnglish] [nvarchar](200) NULL,
    [MobileNumber] [varchar](15) NULL,
    [EmailID] [varchar](120) NULL,
    [Address] [nvarchar](250) NULL,
    [AddressEnglish] [nvarchar](250) NULL,
    [ConnectionTypeID] [int] NOT NULL,
    [CategoryID] [int] NOT NULL,
    [PipeSizeID] [int] NOT NULL,
    [ConnectionDate] [date] NULL,
    [IsActive] [bit] NULL,
    [Remark] [nvarchar](max) NULL,
    [CreatedDate] [datetime] NULL,
    [UpdatedDate] [datetime] NULL,
    CONSTRAINT [PK_ConsumerAccount] PRIMARY KEY CLUSTERED ([ConsumerID] ASC)
)
```

## Entity Mapping (C#)

```csharp
public class ConsumerAccountEntity
{
    public int ConsumerID { get; set; }                    // IDENTITY(1,1) NOT NULL
    public string ConsumerNumber { get; set; }             // varchar(40) NOT NULL
    public string? OldConsumerNumber { get; set; }         // varchar(40) NULL
    public string? ZoneNo { get; set; }                    // varchar(50) NULL
    public string? WardNo { get; set; }                    // varchar(50) NULL
    public string? PropertyNumber { get; set; }            // varchar(50) NULL
    public string? PartitionNumber { get; set; }           // varchar(50) NULL
    public string ConsumerName { get; set; }               // nvarchar(200) NOT NULL
    public string? ConsumerNameEnglish { get; set; }       // nvarchar(200) NULL
    public string? MobileNumber { get; set; }              // varchar(15) NULL
    public string? EmailID { get; set; }                   // varchar(120) NULL
    public string? Address { get; set; }                   // nvarchar(250) NULL
    public string? AddressEnglish { get; set; }            // nvarchar(250) NULL
    public int ConnectionTypeID { get; set; }              // int NOT NULL
    public int CategoryID { get; set; }                    // int NOT NULL
    public int PipeSizeID { get; set; }                    // int NOT NULL
    public DateTime? ConnectionDate { get; set; }          // date NULL
    public bool? IsActive { get; set; }                    // bit NULL
    public string? Remark { get; set; }                    // nvarchar(max) NULL
    public DateTime? CreatedDate { get; set; }             // datetime NULL
    public DateTime? UpdatedDate { get; set; }             // datetime NULL
}
```

## Field-by-Field Mapping

| C# Property | SQL Column | SQL Type | Max Length | Nullable | Notes |
|-------------|------------|----------|------------|----------|-------|
| `ConsumerID` | `ConsumerID` | `int` | - | NO (PK, Identity) | Auto-generated |
| `ConsumerNumber` | `ConsumerNumber` | `varchar` | 40 | NO | Required |
| `OldConsumerNumber` | `OldConsumerNumber` | `varchar` | 40 | YES | Optional |
| `ZoneNo` | `ZoneNo` | `varchar` | 50 | YES | Optional |
| `WardNo` | `WardNo` | `varchar` | 50 | YES | Optional |
| `PropertyNumber` | `PropertyNumber` | `varchar` | 50 | YES | Optional |
| `PartitionNumber` | `PartitionNumber` | `varchar` | 50 | YES | Optional |
| `ConsumerName` | `ConsumerName` | `nvarchar` | 200 | NO | Required (Hindi) |
| `ConsumerNameEnglish` | `ConsumerNameEnglish` | `nvarchar` | 200 | YES | Optional (English) |
| `MobileNumber` | `MobileNumber` | `varchar` | 15 | YES | Optional |
| `EmailID` | `EmailID` | `varchar` | 120 | YES | Optional |
| `Address` | `Address` | `nvarchar` | 250 | YES | Optional (Hindi) |
| `AddressEnglish` | `AddressEnglish` | `nvarchar` | 250 | YES | Optional (English) |
| `ConnectionTypeID` | `ConnectionTypeID` | `int` | - | NO | Required (FK) |
| `CategoryID` | `CategoryID` | `int` | - | NO | Required (FK) |
| `PipeSizeID` | `PipeSizeID` | `int` | - | NO | Required (FK) |
| `ConnectionDate` | `ConnectionDate` | `date` | - | YES | Date only |
| `IsActive` | `IsActive` | `bit` | - | YES | Boolean flag |
| `Remark` | `Remark` | `nvarchar(max)` | unlimited | YES | Optional text |
| `CreatedDate` | `CreatedDate` | `datetime` | - | YES | Timestamp |
| `UpdatedDate` | `UpdatedDate` | `datetime` | - | YES | Timestamp |

## Important Notes

### 1. **Required vs Optional Fields**

**Required (NOT NULL):**
- ConsumerID (Primary Key)
- ConsumerNumber
- ConsumerName
- ConnectionTypeID
- CategoryID
- PipeSizeID

**Optional (NULL allowed):**
- All other fields

### 2. **Data Type Differences**

**String Fields:**
- `varchar` - ASCII only (good for numbers, codes)
- `nvarchar` - Unicode (supports Hindi/Marathi characters)

**Integer Fields:**
- `int` - 32-bit integer
- `bit` - Boolean (0 or 1)

**Date Fields:**
- `date` - Date only (no time component)
- `datetime` - Date and time

### 3. **Foreign Key Fields**

These are integer IDs that reference lookup tables:
- `ConnectionTypeID` ? Connection type master
- `CategoryID` ? Category master
- `PipeSizeID` ? Pipe size master

### 4. **Identity Field**

- `ConsumerID` - Auto-incremented by database, don't set manually

### 5. **Unicode Fields**

Fields that support Hindi/Marathi characters:
- `ConsumerName` (nvarchar)
- `ConsumerNameEnglish` (nvarchar)
- `Address` (nvarchar)
- `AddressEnglish` (nvarchar)
- `Remark` (nvarchar)

### 6. **Indexed Fields**

The following fields have indexes for fast searching:
- ConsumerNumber (Unique Index)
- OldConsumerNumber
- ZoneNo
- WardNo
- PropertyNumber
- MobileNumber
- IsActive

## Sample Data

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
  "categoryID": 2,
  "pipeSizeID": 3,
  "connectionDate": "2024-09-14",
  "isActive": true,
  "remark": "Residential Consumer",
  "createdDate": "2026-01-07T13:32:33.827",
  "updatedDate": "2026-01-07T13:32:33.827"
}
```

## Validation Rules (DTOs)

### CreateConsumerAccountDto

**Required Fields:**
- ConsumerNumber (max 40 chars)
- ConsumerName (max 200 chars)
- ConnectionTypeID
- CategoryID
- PipeSizeID

**Optional Fields:**
- All others

**Field Lengths:**
- ConsumerNumber: 40
- OldConsumerNumber: 40
- ZoneNo, WardNo, PropertyNumber, PartitionNumber: 50
- ConsumerName, ConsumerNameEnglish: 200
- MobileNumber: 15
- EmailID: 120
- Address, AddressEnglish: 250
- Remark: unlimited

### UpdateConsumerAccountDto

Same as Create, plus:
- ConsumerID (required for identifying record)

## Common Scenarios

### 1. Creating New Consumer
```csharp
var newConsumer = new CreateConsumerAccountDto
{
    ConsumerNumber = "CON100",
    ConsumerName = "???? ??????",
    ConsumerNameEnglish = "New Consumer",
    MobileNumber = "9876543210",
    ConnectionTypeID = 1,
    CategoryID = 2,
    PipeSizeID = 1,
    IsActive = true
};
```

### 2. Searching Consumer
```csharp
// By Consumer Number
GET /api/wtis/consumeraccount/find-consumer?search=CON009

// By Mobile
GET /api/wtis/consumeraccount/find-consumer?search=9876543218

// By Pattern
GET /api/wtis/consumeraccount/find-consumer?search=3-PROP009-PART009
```

### 3. Filtering List
```csharp
// Active consumers in Ward 3
GET /api/wtis/consumeraccount?wardNo=3&isActive=true

// By Category
GET /api/wtis/consumeraccount?categoryID=2
```

## Troubleshooting

### Error: "Cannot insert NULL into column"
**Cause:** Missing required field  
**Fix:** Ensure these are provided:
- ConsumerNumber
- ConsumerName
- ConnectionTypeID
- CategoryID
- PipeSizeID

### Error: "String truncation"
**Cause:** Value exceeds max length  
**Fix:** Check field lengths:
- ConsumerNumber: max 40
- Address: max 250 (not 500!)
- EmailID: max 120

### Error: "Invalid cast Int32 to String"
**Cause:** Type mismatch  
**Fix:** These are INTs, not strings:
- ConnectionTypeID
- CategoryID
- PipeSizeID

## Summary

? **All fields now match database exactly**  
? **Correct nullable/required constraints**  
? **Proper field lengths**  
? **Correct data types**  
? **Ready for production use**
