# Railway Deployment Script for Visa Consultant App
# This script helps prepare and deploy the application to Railway

Write-Host "=== Railway Deployment Script ===" -ForegroundColor Green
Write-Host ""

# Check if git is available
try {
    $gitVersion = git --version
    Write-Host "✓ Git found: $gitVersion" -ForegroundColor Green
} catch {
    Write-Host "✗ Git not found. Please install Git first." -ForegroundColor Red
    exit 1
}

# Check if we're in a git repository
if (-not (Test-Path ".git")) {
    Write-Host "✗ Not in a git repository. Please run this script from your project root." -ForegroundColor Red
    exit 1
}

# Check current branch
$currentBranch = git branch --show-current
Write-Host "Current branch: $currentBranch" -ForegroundColor Yellow

# Check for uncommitted changes
$status = git status --porcelain
if ($status) {
    Write-Host "⚠️  You have uncommitted changes:" -ForegroundColor Yellow
    Write-Host $status -ForegroundColor Gray
    $commit = Read-Host "Do you want to commit these changes? (y/n)"
    if ($commit -eq 'y' -or $commit -eq 'Y') {
        $commitMessage = Read-Host "Enter commit message (or press Enter for default)"
        if (-not $commitMessage) {
            $commitMessage = "Deploy to Railway - $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
        }
        git add .
        git commit -m $commitMessage
        Write-Host "✓ Changes committed" -ForegroundColor Green
    } else {
        Write-Host "⚠️  Deploying with uncommitted changes" -ForegroundColor Yellow
    }
}

# Push to remote repository
Write-Host ""
Write-Host "Pushing to remote repository..." -ForegroundColor Yellow
git push origin $currentBranch

if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Code pushed successfully" -ForegroundColor Green
} else {
    Write-Host "✗ Failed to push code" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "=== Deployment Summary ===" -ForegroundColor Green
Write-Host "✓ Code pushed to repository" -ForegroundColor Green
Write-Host "✓ Railway will automatically detect changes and deploy" -ForegroundColor Green
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Go to your Railway dashboard" -ForegroundColor White
Write-Host "2. Check the deployment logs" -ForegroundColor White
Write-Host "3. Verify database migrations ran successfully" -ForegroundColor White
Write-Host "4. Test the health endpoints:" -ForegroundColor White
Write-Host "   - /health" -ForegroundColor Gray
Write-Host "   - /health/db" -ForegroundColor Gray
Write-Host ""
Write-Host "Default admin credentials:" -ForegroundColor Yellow
Write-Host "Username: admin" -ForegroundColor White
Write-Host "Password: Admin@123" -ForegroundColor White
Write-Host ""
Write-Host "Remember to change the default password after first login!" -ForegroundColor Red 