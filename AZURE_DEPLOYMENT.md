# Azure Deployment Guide - Guru Kirpa Visa Consultant

## Azure Free Tier Benefits

### 🆓 **Free Services Available:**
- **Azure App Service (F1)**: Free tier with 1 GB RAM, 60 minutes/day CPU
- **Azure Database for PostgreSQL**: Free tier with 32 MB storage
- **Azure Container Registry**: 500 MB storage, 2,000 pulls/month
- **Azure DevOps**: Free for 5 users
- **Azure Application Insights**: 5 GB data/month free

## Prerequisites

### 1. **Azure Account Setup**
- Create free Azure account: https://azure.microsoft.com/free/
- Get $200 credit for 30 days + free services for 12 months
- Install Azure CLI: https://docs.microsoft.com/en-us/cli/azure/install-azure-cli

### 2. **Local Setup**
```bash
# Install Azure CLI
winget install Microsoft.AzureCLI

# Login to Azure
az login

# Set subscription (if you have multiple)
az account set --subscription "Your Subscription Name"
```

## Deployment Methods

### Method 1: Automated Script (Recommended)

1. **Run the deployment script**:
   ```powershell
   .\azure-deploy.ps1
   ```

2. **Customize parameters** (optional):
   ```powershell
   .\azure-deploy.ps1 -ResourceGroupName "my-visa-rg" -AppServiceName "my-visa-app" -Location "West US"
   ```

### Method 2: Manual Azure Portal Deployment

#### Step 1: Create Resource Group
1. Go to Azure Portal: https://portal.azure.com
2. Click "Create a resource"
3. Search for "Resource group"
4. Create with name: `visa-consultant-rg`

#### Step 2: Create App Service Plan
1. Search for "App Service Plan"
2. Create new with:
   - **Name**: `visa-consultant-plan`
   - **Resource Group**: `visa-consultant-rg`
   - **Operating System**: Linux
   - **Region**: East US
   - **Pricing Plan**: F1 (Free)

#### Step 3: Create Web App
1. Search for "Web App"
2. Create new with:
   - **Name**: `visa-consultant-app`
   - **Resource Group**: `visa-consultant-rg`
   - **Runtime Stack**: .NET 9 (LTS)
   - **Operating System**: Linux
   - **App Service Plan**: `visa-consultant-plan`

#### Step 4: Create PostgreSQL Database
1. Search for "Azure Database for PostgreSQL flexible servers"
2. Create new with:
   - **Name**: `visa-consultant-db`
   - **Resource Group**: `visa-consultant-rg`
   - **Region**: East US
   - **Admin Username**: `postgres`
   - **Password**: `YourStrongPassword123!`
   - **Compute + Storage**: Burstable B1ms (Free tier)

#### Step 5: Configure Database
1. Go to your PostgreSQL server
2. Create database named `visa_consultant`
3. Configure firewall rules to allow Azure services

#### Step 6: Deploy Application
1. Go to your Web App
2. Navigate to "Deployment Center"
3. Choose "GitHub" as source
4. Connect your repository
5. Deploy from main branch

#### Step 7: Configure Environment Variables
In your Web App → Configuration → Application settings, add:

```
DB_HOST=your-postgres-server.postgres.database.azure.com
DB_NAME=visa_consultant
DB_USER=postgres
DB_PASSWORD=YourStrongPassword123!
JWT_SECRET_KEY=YourSuperSecretJWTKey2024!
SMTP_SERVER=smtp.gmail.com
SMTP_PORT=587
SMTP_USERNAME=your-email@gmail.com
SMTP_PASSWORD=your-app-password
FROM_EMAIL=your-email@gmail.com
ADMIN_EMAIL=admin@gurukirpa.com
```

### Method 3: Azure DevOps Pipeline

1. **Create Azure DevOps Project**:
   - Go to https://dev.azure.com
   - Create new project

2. **Set up Pipeline**:
   - Go to Pipelines → New Pipeline
   - Choose "Azure Repos Git" or "GitHub"
   - Select "ASP.NET Core"
   - Use the `azure-pipelines.yml` file

3. **Configure Variables**:
   - Go to Library → Variable Groups
   - Create variable group with all environment variables

## Database Migration

After deployment, run migrations:

### Option 1: Azure CLI
```bash
az webapp ssh --resource-group visa-consultant-rg --name visa-consultant-app
dotnet ef database update
```

### Option 2: Kudu Console
1. Go to your Web App → Advanced Tools → Kudu
2. Open SSH console
3. Navigate to `/home/site/wwwroot`
4. Run: `dotnet ef database update`

### Option 3: Local Migration
```bash
# Set connection string locally
$env:DB_HOST="your-postgres-server.postgres.database.azure.com"
$env:DB_NAME="visa_consultant"
$env:DB_USER="postgres"
$env:DB_PASSWORD="YourStrongPassword123!"

# Run migration
dotnet ef database update
```

## Monitoring and Logging

### Application Insights (Free)
1. Create Application Insights resource
2. Add connection string to app settings
3. Monitor performance and errors

### Log Streaming
```bash
az webapp log tail --resource-group visa-consultant-rg --name visa-consultant-app
```

## Cost Optimization

### Free Tier Limits:
- **App Service F1**: 60 minutes/day CPU time
- **PostgreSQL**: 32 MB storage, 4 connections
- **Container Registry**: 500 MB storage

### Scaling Options:
- **App Service**: F1 → B1 (Basic) when needed
- **Database**: Burstable → General Purpose when needed

## Security Best Practices

1. **Use Azure Key Vault** for secrets
2. **Enable HTTPS** (automatic with App Service)
3. **Configure firewall rules** for database
4. **Use managed identities** for database access
5. **Enable Application Insights** for monitoring

## Troubleshooting

### Common Issues:

1. **Database Connection Failed**:
   - Check firewall rules
   - Verify connection string
   - Ensure SSL is enabled

2. **App Service Won't Start**:
   - Check startup command
   - Review application logs
   - Verify environment variables

3. **Migration Errors**:
   - Check database permissions
   - Verify connection string
   - Run migrations manually

### Useful Commands:
```bash
# Check app status
az webapp show --resource-group visa-consultant-rg --name visa-consultant-app

# View logs
az webapp log download --resource-group visa-consultant-rg --name visa-consultant-app

# Restart app
az webapp restart --resource-group visa-consultant-rg --name visa-consultant-app
```

## Testing Your Deployment

1. **Health Check**: Visit `https://your-app-name.azurewebsites.net/swagger`
2. **API Testing**: Use Swagger UI to test endpoints
3. **Database**: Verify data is being created
4. **Logs**: Monitor application logs in Azure portal

## Default Admin Account

The application creates a default admin account:
- **Username**: admin
- **Password**: Admin@123

**Important**: Change this password after first login!

## Support

- **Azure Documentation**: https://docs.microsoft.com/azure/
- **Azure Support**: Available with free account
- **Community**: Stack Overflow, Azure forums 