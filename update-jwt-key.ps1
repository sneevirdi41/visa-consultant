# Railway JWT Secret Key Update Script
# This script helps you update the JWT_SECRET_KEY in Railway

Write-Host "🚀 Railway JWT Secret Key Update Helper" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Green

Write-Host "`n📋 Current Status:" -ForegroundColor Yellow
Write-Host "✅ Application: Running and healthy" -ForegroundColor Green
Write-Host "✅ Database: Connected" -ForegroundColor Green
Write-Host "❌ JWT_SECRET_KEY: Too short (136 bits, needs 256+ bits)" -ForegroundColor Red

Write-Host "`n🔧 Required Action:" -ForegroundColor Yellow
Write-Host "1. Go to Railway Dashboard" -ForegroundColor Cyan
Write-Host "2. Navigate to: visa-consultant-app → Variables" -ForegroundColor Cyan
Write-Host "3. Find JWT_SECRET_KEY variable" -ForegroundColor Cyan
Write-Host "4. Update its value to:" -ForegroundColor Cyan
Write-Host "   your-super-secret-jwt-key-2024-your-super-secret-jwt-key-2024" -ForegroundColor White -BackgroundColor DarkBlue

Write-Host "`n🧪 Test Commands:" -ForegroundColor Yellow
Write-Host "After updating, test with these commands:" -ForegroundColor Cyan

Write-Host "`n# Test JWT Configuration:" -ForegroundColor Gray
Write-Host "Invoke-WebRequest -Uri 'https://visa-consultant-app-production-07b2.up.railway.app/api/Auth/test-jwt' -Method GET" -ForegroundColor White

Write-Host "`n# Test Login:" -ForegroundColor Gray
Write-Host '$body = @{username="admin";password="Admin@123"} | ConvertTo-Json' -ForegroundColor White
Write-Host 'Invoke-WebRequest -Uri "https://visa-consultant-app-production-07b2.up.railway.app/api/Auth/login" -Method POST -Body $body -ContentType "application/json"' -ForegroundColor White

Write-Host "`n⏰ Expected Timeline:" -ForegroundColor Yellow
Write-Host "• Update variable in Railway" -ForegroundColor Cyan
Write-Host "• Wait 1-2 minutes for redeployment" -ForegroundColor Cyan
Write-Host "• Test with commands above" -ForegroundColor Cyan

Write-Host "`n🎯 Expected Results:" -ForegroundColor Yellow
Write-Host "✅ JWT Test: Should return a valid token" -ForegroundColor Green
Write-Host "✅ Login Test: Should return JWT token for admin user" -ForegroundColor Green

Write-Host "`nPress any key to continue..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown") 