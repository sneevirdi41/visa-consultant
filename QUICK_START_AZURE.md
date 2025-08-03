# 🚀 Quick Start: Deploy to Azure (FREE!)

## Step 1: Get Azure Free Account
1. Go to: https://azure.microsoft.com/free/
2. Sign up with your email
3. Get **$200 credit for 30 days** + **12 months of free services**

## Step 2: Install Azure CLI
```powershell
# Windows
winget install Microsoft.AzureCLI

# Or download from: https://docs.microsoft.com/en-us/cli/azure/install-azure-cli
```

## Step 3: Login to Azure
```powershell
az login
```

## Step 4: Deploy with One Command!
```powershell
# Run the automated deployment script
.\azure-deploy.ps1
```

## Step 5: Test Your App
- **URL**: `https://visa-consultant-app.azurewebsites.net`
- **Swagger**: `https://visa-consultant-app.azurewebsites.net/swagger`
- **Admin Login**: 
  - Username: `admin`
  - Password: `Admin@123`

## 🆓 What's Free?

### Azure App Service (F1)
- ✅ 1 GB RAM
- ✅ 60 minutes/day CPU time
- ✅ HTTPS included
- ✅ Custom domain support

### Azure Database for PostgreSQL
- ✅ 32 MB storage
- ✅ 4 connections
- ✅ SSL encryption
- ✅ Automatic backups

### Azure Container Registry
- ✅ 500 MB storage
- ✅ 2,000 pulls/month

## 💰 Cost Breakdown
- **Month 1**: $200 credit (covers everything!)
- **Months 2-12**: FREE tier services
- **After 12 months**: ~$5-10/month for small app

## 🔧 Customization
```powershell
# Custom names and location
.\azure-deploy.ps1 -ResourceGroupName "my-visa-rg" -AppServiceName "my-visa-app" -Location "West US"
```

## 📞 Need Help?
- **Azure Documentation**: https://docs.microsoft.com/azure/
- **Free Support**: Available with Azure account
- **Community**: Stack Overflow, Azure forums

## 🎯 Next Steps
1. **Update Email Settings** in Azure portal
2. **Change Admin Password** after first login
3. **Add Custom Domain** (optional)
4. **Set up Monitoring** with Application Insights

---
**Your application will be live in 5-10 minutes!** 🎉 