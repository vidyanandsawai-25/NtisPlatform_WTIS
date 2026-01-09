# Quick Start Guide - Consumer Account Universal Search

## ?? Getting Started in 5 Minutes

### Step 1: Start the API
```bash
cd D:\ntis-platform-feature-master-demo\src\Presentation\NtisPlatform.Api
dotnet run
```

Wait for: `Application started. Press Ctrl+C to shut down.`

---

### Step 2: Test the API

#### Option A: Use the HTML Test Page (Easiest)
1. Open `docs/ConsumerSearchTestPage.html` in your browser
2. Click any example button
3. See the results instantly!

#### Option B: Use Your Browser
Open: `http://localhost:5000/api/wtis/consumeraccount/find-consumer?search=CON001`

#### Option C: Use cURL
```bash
curl "http://localhost:5000/api/wtis/consumeraccount/find-consumer?search=CON001"
```

#### Option D: Use Swagger UI
Open: `http://localhost:5000/swagger`
- Find `ConsumerAccount` section
- Try the `find-consumer` endpoint

---

### Step 3: Try Different Searches

```bash
# By Consumer Number
http://localhost:5000/api/wtis/consumeraccount/find-consumer?search=CON001

# By Mobile Number
http://localhost:5000/api/wtis/consumeraccount/find-consumer?search=9876543210

# By Name
http://localhost:5000/api/wtis/consumeraccount/find-consumer?search=Raj%20Sharma

# By Apartment Pattern (Ward-Property-Flat)
http://localhost:5000/api/wtis/consumeraccount/find-consumer?search=1-PROP011-FLAT-101

# By Property Number
http://localhost:5000/api/wtis/consumeraccount/find-consumer?search=PROP011

# By Email
http://localhost:5000/api/wtis/consumeraccount/find-consumer?search=raj@example.com
```

---

## ?? Test with Your Sample Data

Your database has these test consumers:

| Consumer | Mobile | Pattern | Use For Testing |
|----------|--------|---------|-----------------|
| CON001 | 9876543210 | 1-PROP001-PART001 | Simple search |
| CON002 | 9876543211 | 1-PROP002-PART002 | Different ward |
| CON011 | 9123456789 | 1-PROP011-FLAT-101 | Apartment unit 1 |
| CON012 | 9123456789 | 1-PROP011-FLAT-102 | Apartment unit 2 |
| CON013 | 9123456789 | 1-PROP011-FLAT-201 | Apartment unit 3 |

---

## ?? Common Scenarios

### Scenario 1: Customer calls with their mobile number
```bash
curl "http://localhost:5000/api/wtis/consumeraccount/find-consumer?search=9876543210"
```
? Returns: Raj Sharma's complete details

### Scenario 2: Customer knows their consumer number
```bash
curl "http://localhost:5000/api/wtis/consumeraccount/find-consumer?search=CON001"
```
? Returns: Complete consumer account

### Scenario 3: Finding a specific apartment unit
```bash
curl "http://localhost:5000/api/wtis/consumeraccount/find-consumer?search=1-PROP011-FLAT-101"
```
? Returns: Rajesh Gaikwad in Flat 101

### Scenario 4: Customer only knows their name
```bash
curl "http://localhost:5000/api/wtis/consumeraccount/find-consumer?search=Raj%20Sharma"
```
? Returns: Raj Sharma's account

---

## ?? Pro Tips

### Tip 1: URL Encoding
Always encode special characters:
- Spaces: Use `%20` or `+`
- Example: `Raj Sharma` ? `Raj%20Sharma`

### Tip 2: Pattern Format
For apartment/complex searches:
```
Format: {WardNo}-{PropertyNumber}-{PartitionNumber}
Example: 1-PROP011-FLAT-101
```

### Tip 3: Check Connection String
Make sure your `appsettings.json` has:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=192.168.1.21;Database=Online_NTIS;User Id=vidyanand.s;Password=Ram@2508;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

---

## ?? Expected Response

### Success (200 OK)
```json
{
  "consumerID": 41226,
  "consumerNumber": "CON001",
  "consumerName": "??? ?????",
  "consumerNameEnglish": "Raj Sharma",
  "mobileNumber": "9876543210",
  "emailID": "raj@example.com",
  "zoneNo": "1",
  "wardNo": "1",
  "propertyNumber": "PROP001",
  "partitionNumber": "PART001",
  "address": "????? ???",
  "addressEnglish": "Mumbai Road",
  "isActive": true,
  "remark": "Residential Consumer"
}
```

### Not Found (404)
```json
{
  "message": "Consumer account not found.",
  "searchValue": "CON999",
  "hint": "Try searching with: Consumer Number, Mobile Number, Consumer Name, or Ward-Property-Partition pattern"
}
```

---

## ? Performance Check

Test response time:
```bash
time curl "http://localhost:5000/api/wtis/consumeraccount/find-consumer?search=CON001"
```

Expected: < 100ms for first request, < 50ms for subsequent requests

---

## ?? Troubleshooting

### Error: "Cannot connect to API"
**Fix:** Make sure API is running
```bash
# Check if running
curl http://localhost:5000/health

# Or check this URL in browser
http://localhost:5000
```

### Error: "Consumer not found" but you know it exists
**Fix:** Check exact spelling and format
- Consumer numbers are case-sensitive
- Names must match exactly
- Try with consumer ID directly: `/api/wtis/consumeraccount/41226`

### Error: Database connection failed
**Fix:** Verify connection string and SQL Server is accessible
```bash
# Test SQL connection
sqlcmd -S 192.168.1.21 -U vidyanand.s -P Ram@2508 -d Online_NTIS -Q "SELECT TOP 1 * FROM WTIS.ConsumerAccount"
```

---

## ?? Next Steps

1. ? Test the API with sample data
2. ? Try all search patterns
3. ? Open the HTML test page for interactive testing
4. ? Read full documentation in `ConsumerAccountUniversalSearchAPI.md`
5. ? Integrate into your application

---

## ?? You're Ready!

You now have a powerful, flexible consumer search API. Try it out!

**Main Endpoint:**
```
GET /api/wtis/consumeraccount/find-consumer?search={anything}
```

**Test Page:**
```
docs/ConsumerSearchTestPage.html
```

**Documentation:**
```
docs/ConsumerAccountUniversalSearchAPI.md
docs/IMPLEMENTATION_SUMMARY.md
```

Happy Coding! ??
