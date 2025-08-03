# Azure Deployment Script for Guru Kirpa Visa Consultant
# Prerequisites: Azure CLI installed and logged in

param(
    [string]$ResourceGroupName = "visa-consultant-rg",
    [string]$AppServiceName = "visa-consultant-app",
    [string]$DatabaseName = "visa-consultant-db",
    [string]$Location = "East US"
)

Write-Host "Starting Azure deployment..." -ForegroundColor Green

# 1. Create Resource Group
Write-Host "Creating Resource Group..." -ForegroundColor Yellow
az group create --name $ResourceGroupName --location $Location

# 2. Create App Service Plan (Free F1 tier)
Write-Host "Creating App Service Plan..." -ForegroundColor Yellow
az appservice plan create --name "visa-consultant-plan" --resource-group $ResourceGroupName --sku F1 --is-linux

# 3. Create Web App
Write-Host "Creating Web App..." -ForegroundColor Yellow
az webapp create --resource-group $ResourceGroupName --plan "visa-consultant-plan" --name $AppServiceName --runtime "DOTNETCORE:9.0"

# 4. Create PostgreSQL Database (Free tier)
Write-Host "Creating PostgreSQL Database..." -ForegroundColor Yellow
az postgres flexible-server create --resource-group $ResourceGroupName --name $DatabaseName --location $Location --admin-user postgres --admin-password "YourStrongPassword123!" --sku-name Standard_B1ms --tier Burstable --storage-size 32

# 5. Configure database firewall
Write-Host "Configuring database firewall..." -ForegroundColor Yellow
az postgres flexible-server firewall-rule create --resource-group $ResourceGroupName --name $DatabaseName --rule-name "AllowAzureServices" --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0

# 6. Create database
Write-Host "Creating database..." -ForegroundColor Yellow
az postgres flexible-server db create --resource-group $ResourceGroupName --server-name $DatabaseName --database-name "visa_consultant"

# 7. Get database connection details
Write-Host "Getting database connection details..." -ForegroundColor Yellow
$dbHost = az postgres flexible-server show --resource-group $ResourceGroupName --name $DatabaseName --query "fullyQualifiedDomainName" -o tsv
$dbName = "visa_consultant"
$dbUser = "postgres"
$dbPassword = "YourStrongPassword123!"

# 8. Set application settings
Write-Host "Setting application settings..." -ForegroundColor Yellow
az webapp config appsettings set --resource-group $ResourceGroupName --name $AppServiceName --settings @(
    "DB_HOST=$dbHost",
    "DB_NAME=$dbName", 
    "DB_USER=$dbUser",
    "DB_PASSWORD=$dbPassword",
    "JWT_SECRET_KEY=YourSuperSecretJWTKey2024!",
    "SMTP_SERVER=smtp.gmail.com",
    "SMTP_PORT=587",
    "SMTP_USERNAME=your-email@gmail.com",
    "SMTP_PASSWORD=your-app-password",
    "FROM_EMAIL=your-email@gmail.com",
    "ADMIN_EMAIL=admin@gurukirpa.com"
)

# 9. Configure startup command
Write-Host "Configuring startup command..." -ForegroundColor Yellow
az webapp config set --resource-group $ResourceGroupName --name $AppServiceName --startup-file "dotnet visa_consulatant.dll"

# 10. Enable logging
Write-Host "Enabling application logging..." -ForegroundColor Yellow
az webapp log config --resource-group $ResourceGroupName --name $AppServiceName --web-server-logging filesystem

Write-Host "Deployment completed successfully!" -ForegroundColor Green
Write-Host "Your application URL: https://$AppServiceName.azurewebsites.net" -ForegroundColor Cyan
Write-Host "Database Host: $dbHost" -ForegroundColor Cyan
Write-Host "Database Name: $dbName" -ForegroundColor Cyan
Write-Host "Database User: $dbUser" -ForegroundColor Cyan

Write-Host "`nNext steps:" -ForegroundColor Yellow
Write-Host "1. Update your email settings in the Azure portal" -ForegroundColor White
Write-Host "2. Run database migrations: az webapp ssh --resource-group $ResourceGroupName --name $AppServiceName" -ForegroundColor White
Write-Host "3. Test your application at: https://$AppServiceName.azurewebsites.net/swagger" -ForegroundColor White 