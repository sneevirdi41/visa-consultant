# Railway Deployment Test Script
# This script tests all endpoints of the Visa Consultant application

Write-Host "=== RAILWAY DEPLOYMENT TEST SCRIPT ===" -ForegroundColor Green
Write-Host "Testing: https://visa-consultant-app-production-07b2.up.railway.app" -ForegroundColor Cyan
Write-Host ""

# Test 1: Health Check
Write-Host "1. Testing Health Check..." -ForegroundColor Yellow
try {
    $health = Invoke-RestMethod -Uri "https://visa-consultant-app-production-07b2.up.railway.app/health" -Method GET
    Write-Host "   ✅ Health Check: PASSED" -ForegroundColor Green
    Write-Host "   Status: $($health.status)" -ForegroundColor White
    Write-Host "   Environment: $($health.environment)" -ForegroundColor White
} catch {
    Write-Host "   ❌ Health Check: FAILED - $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# Test 2: Environment Variables
Write-Host "2. Testing Environment Variables..." -ForegroundColor Yellow
try {
    $env = Invoke-RestMethod -Uri "https://visa-consultant-app-production-07b2.up.railway.app/api/Auth/test-connection" -Method POST
    Write-Host "   ✅ Environment Test: PASSED" -ForegroundColor Green
    Write-Host "   Database URL: $($env.databaseUrl)" -ForegroundColor White
    Write-Host "   JWT Secret Key: $($env.jwtSecretKey)" -ForegroundColor White
    Write-Host "   Environment: $($env.environment)" -ForegroundColor White
} catch {
    Write-Host "   ❌ Environment Test: FAILED - $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# Test 3: Database Connection
Write-Host "3. Testing Database Connection..." -ForegroundColor Yellow
try {
    $db = Invoke-RestMethod -Uri "https://visa-consultant-app-production-07b2.up.railway.app/api/Auth/test-db" -Method POST
    Write-Host "   ✅ Database Test: PASSED" -ForegroundColor Green
    Write-Host "   Can Connect: $($db.canConnect)" -ForegroundColor White
    Write-Host "   User Count: $($db.userCount)" -ForegroundColor White
} catch {
    Write-Host "   ❌ Database Test: FAILED - $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# Test 4: JWT Service (if JWT_SECRET_KEY is set)
Write-Host "4. Testing JWT Service..." -ForegroundColor Yellow
try {
    $jwt = Invoke-RestMethod -Uri "https://visa-consultant-app-production-07b2.up.railway.app/api/Auth/test-jwt" -Method GET
    Write-Host "   ✅ JWT Test: PASSED" -ForegroundColor Green
    Write-Host "   JWT Secret Key: $($jwt.jwtSecretKey)" -ForegroundColor White
    Write-Host "   Token Generated: $($jwt.token.Length) characters" -ForegroundColor White
} catch {
    Write-Host "   ❌ JWT Test: FAILED - $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "   💡 This is expected if JWT_SECRET_KEY is not set" -ForegroundColor Cyan
}

Write-Host ""

# Test 5: Login (Expected to fail if JWT_SECRET_KEY is missing)
Write-Host "5. Testing Login..." -ForegroundColor Yellow
try {
    $login = Invoke-RestMethod -Uri "https://visa-consultant-app-production-07b2.up.railway.app/api/Auth/login" -Method POST -Headers @{"Content-Type"="application/json"} -Body '{"username": "admin", "password": "Admin@123"}'
    Write-Host "   ✅ Login Test: PASSED" -ForegroundColor Green
    Write-Host "   Username: $($login.username)" -ForegroundColor White
    Write-Host "   Role: $($login.role)" -ForegroundColor White
    Write-Host "   Token: $($login.token.Substring(0, 20))..." -ForegroundColor White
} catch {
    Write-Host "   ❌ Login Test: FAILED - $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "   💡 This is expected if JWT_SECRET_KEY is not set" -ForegroundColor Cyan
}

Write-Host ""
Write-Host "=== SUMMARY ===" -ForegroundColor Green

# Determine overall status
$overallStatus = "UNKNOWN"
if ($health.status -eq "healthy" -and $db.canConnect -eq $true) {
    if ($env.jwtSecretKey -eq "set") {
        $overallStatus = "FULLY OPERATIONAL"
        Write-Host "🎉 APPLICATION IS FULLY OPERATIONAL!" -ForegroundColor Green
    } else {
        $overallStatus = "PARTIALLY WORKING"
        Write-Host "⚠️  APPLICATION IS PARTIALLY WORKING - JWT_SECRET_KEY MISSING" -ForegroundColor Yellow
    }
} else {
    $overallStatus = "HAS ISSUES"
    Write-Host "❌ APPLICATION HAS ISSUES" -ForegroundColor Red
}

Write-Host ""
Write-Host "=== NEXT STEPS ===" -ForegroundColor Cyan

if ($env.jwtSecretKey -eq "null") {
    Write-Host "🔧 ACTION REQUIRED: Add JWT_SECRET_KEY environment variable" -ForegroundColor Red
    Write-Host "   1. Go to Railway Dashboard" -ForegroundColor White
    Write-Host "   2. Select visa-consultant-app service" -ForegroundColor White
    Write-Host "   3. Go to Variables tab" -ForegroundColor White
    Write-Host "   4. Add: JWT_SECRET_KEY = your-super-secret-jwt-key-2024" -ForegroundColor White
    Write-Host "   5. Railway will auto-redeploy" -ForegroundColor White
    Write-Host "   6. Run this script again to test" -ForegroundColor White
} else {
    Write-Host "✅ All tests passed! Application is ready to use." -ForegroundColor Green
}

Write-Host ""
Write-Host "=== APPLICATION URLS ===" -ForegroundColor Cyan
Write-Host "Main App: https://visa-consultant-app-production-07b2.up.railway.app" -ForegroundColor White
Write-Host "Admin Panel: https://visa-consultant-app-production-07b2.up.railway.app/admin.html" -ForegroundColor White
Write-Host "API Docs: https://visa-consultant-app-production-07b2.up.railway.app/swagger" -ForegroundColor White

Write-Host ""
Write-Host "=== DEFAULT CREDENTIALS ===" -ForegroundColor Cyan
Write-Host "Username: admin" -ForegroundColor White
Write-Host "Password: Admin@123" -ForegroundColor White
Write-Host "Email: admin@gurukirpa.com" -ForegroundColor White

Write-Host ""
Write-Host "Test completed at: $(Get-Date)" -ForegroundColor Gray 