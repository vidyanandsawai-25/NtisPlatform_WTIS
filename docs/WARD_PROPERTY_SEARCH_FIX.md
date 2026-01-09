# Ward-Property-Partition Search - FIXED

## ?? The Problem Was

When searching with `?search=1-PROP011`, the repository was receiving:
- `WardNo = "1"`
- `PropertyNumber = "PROP011"`

But the repository's pattern detection was trying to parse `PropertyNumber` as a pattern again, causing issues.

## ? The Fix

Now the flow is:

### Input: `?search=1-PROP011`

**Controller:**
1. Detects pattern with 2 parts (Ward-Property)
2. Splits: `WardNo = "1"`, `PropertyNumber = "PROP011"`
3. Passes BOTH separately to repository

**Repository:**
1. Checks: Is `WardNo` provided AND `PropertyNumber` doesn't contain `-`?
2. YES ? Use exact match on BOTH fields
3. SQL: `WHERE ca.WardNo = '1' AND ca.PropertyNumber = 'PROP011'`

Result: **Returns ALL consumers in Ward 1, Property PROP011 (all partitions/flats)**

---

## ?? Test Cases

### Test 1: Ward Only
```sh
curl "http://localhost:5000/api/wtis/consumer?search=1"
```

**SQL Generated:**
```sql
WHERE ca.IsActive = 1 AND ca.WardNo = '1'
```

**Returns:** ALL consumers in Ward 1

---

### Test 2: Ward + Property
```sh
curl "http://localhost:5000/api/wtis/consumer?search=1-PROP011"
```

**SQL Generated:**
```sql
WHERE ca.IsActive = 1 
  AND ca.WardNo = '1' 
  AND ca.PropertyNumber = 'PROP011'
```

**Returns:** ALL consumers in Ward 1, Property PROP011 (FLAT-101, FLAT-102, FLAT-201, etc.)

---

### Test 3: Ward + Property + Partition
```sh
curl "http://localhost:5000/api/wtis/consumer?search=1-PROP011-FLAT-101"
```

**First Attempt (FindConsumerAsync):**
```sql
WHERE ca.IsActive = 1 
  AND ca.WardNo = '1' 
  AND ca.PropertyNumber = 'PROP011'
  AND ca.PartitionNumber = 'FLAT-101'
```

**Returns:** ONE exact consumer (if found)

---

## ?? Example Data

Assume database has:

```
WardNo | PropertyNumber | PartitionNumber | ConsumerNumber
-------|----------------|-----------------|---------------
1      | PROP011        | FLAT-101        | CON011
1      | PROP011        | FLAT-102        | CON012
1      | PROP011        | FLAT-201        | CON013
1      | PROP011        | FLAT-202        | CON014
```

### Search Results:

**`?search=1`**
```json
{
  "items": [
    { "consumerNumber": "CON001", "wardNo": "1", "propertyNumber": "PROP001" },
    { "consumerNumber": "CON011", "wardNo": "1", "propertyNumber": "PROP011", "partitionNumber": "FLAT-101" },
    { "consumerNumber": "CON012", "wardNo": "1", "propertyNumber": "PROP011", "partitionNumber": "FLAT-102" },
    { "consumerNumber": "CON013", "wardNo": "1", "propertyNumber": "PROP011", "partitionNumber": "FLAT-201" },
    { "consumerNumber": "CON014", "wardNo": "1", "propertyNumber": "PROP011", "partitionNumber": "FLAT-202" }
  ],
  "totalCount": 50
}
```

**`?search=1-PROP011`** ? NOW RETURNS ALL FLATS
```json
{
  "items": [
    { "consumerNumber": "CON011", "wardNo": "1", "propertyNumber": "PROP011", "partitionNumber": "FLAT-101" },
    { "consumerNumber": "CON012", "wardNo": "1", "propertyNumber": "PROP011", "partitionNumber": "FLAT-102" },
    { "consumerNumber": "CON013", "wardNo": "1", "propertyNumber": "PROP011", "partitionNumber": "FLAT-201" },
    { "consumerNumber": "CON014", "wardNo": "1", "propertyNumber": "PROP011", "partitionNumber": "FLAT-202" }
  ],
  "totalCount": 4
}
```

**`?search=1-PROP011-FLAT-101`**
```json
{
  "consumerID": 41236,
  "consumerNumber": "CON011",
  "wardNo": "1",
  "propertyNumber": "PROP011",
  "partitionNumber": "FLAT-101",
  "consumerName": "????? ???????",
  "connectionTypeName": "Residential",
  "isActive": true
}
```

---

## ?? Key Changes

### Before (Buggy):
```csharp
// Controller passed:
PropertyNumber = "PROP011"

// Repository tried to parse "PROP011" as pattern
if (propertyNumber.Contains('-')) { ... }  // FALSE, falls through
sql += " AND ca.PropertyNumber LIKE '%PROP011%'";  // WRONG! Uses LIKE
```

### After (Fixed):
```csharp
// Controller passes:
WardNo = "1"
PropertyNumber = "PROP011"

// Repository detects both provided
if (!string.IsNullOrWhiteSpace(wardNo) && !propertyNumber.Contains('-'))
{
    sql += " AND ca.PropertyNumber = 'PROP011'";  // CORRECT! Exact match
}
```

---

## ? Verification

Test all three levels:

```sh
# Level 1: Ward only (all consumers in ward)
curl "http://localhost:5000/api/wtis/consumer?search=1" | jq '.totalCount'
# Expected: Large number (all in ward)

# Level 2: Ward-Property (all partitions in property)
curl "http://localhost:5000/api/wtis/consumer?search=1-PROP011" | jq '.items | length'
# Expected: 4 (FLAT-101, FLAT-102, FLAT-201, FLAT-202)

# Level 3: Ward-Property-Partition (exact consumer)
curl "http://localhost:5000/api/wtis/consumer?search=1-PROP011-FLAT-101" | jq '.consumerNumber'
# Expected: "CON011" (single result)
```

---

## ?? Summary

? **Ward only** (`1`) - Returns ALL in ward  
? **Ward-Property** (`1-PROP011`) - Returns ALL partitions in property (FIXED!)  
? **Ward-Property-Partition** (`1-PROP011-FLAT-101`) - Returns EXACT consumer  

**The hierarchical search now works perfectly at all three levels!** ??
