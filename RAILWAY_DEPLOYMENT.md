# 🚀 Railway Deployment Guide (Cheapest Option!)

## Why Railway?

### 💰 **Cost Benefits:**
- **Free**: $5 credit monthly (enough for small apps)
- **Paid**: Only $5/month for 1GB RAM, 1 CPU
- **Database**: PostgreSQL included in price
- **No hidden costs**: Everything included

### ⚡ **Performance:**
- **Global CDN**: Fast worldwide access
- **Auto-scaling**: Handles traffic spikes
- **99.9% uptime**: Reliable hosting
- **SSL included**: HTTPS automatically

## Quick Deployment (5 minutes!)

### Step 1: Push to GitHub
```bash
git init
git add .
git commit -m "Initial commit"
git branch -M main
git remote add origin https://github.com/yourusername/visa-consultant.git
git push -u origin main
```

### Step 2: Deploy on Railway
1. Go to https://railway.app
2. Sign in with GitHub
3. Click "New Project"
4. Select "Deploy from GitHub repo"
5. Choose your repository
6. Railway will auto-detect and deploy!

### Step 3: Add Database
1. In your Railway project, click "New"
2. Select "Database" → "PostgreSQL"
3. Railway auto-links it to your app

### Step 4: Set Environment Variables
In your app's "Variables" tab, add:
```
DB_HOST=your-postgres-host
DB_NAME=your-database-name
DB_USER=your-username
DB_PASSWORD=your-password
JWT_SECRET_KEY=your-secret-key-here
SMTP_SERVER=smtp.gmail.com
SMTP_PORT=587
SMTP_USERNAME=your-email@gmail.com
SMTP_PASSWORD=your-app-password
FROM_EMAIL=your-email@gmail.com
ADMIN_EMAIL=admin@gurukirpa.com
```

### Step 5: Test Your App
- **URL**: `https://your-app-name.railway.app`
- **Swagger**: `https://your-app-name.railway.app/swagger`
- **Admin**: `admin` / `Admin@123`

## Cost Breakdown

### Free Tier ($5 credit/month):
- ✅ **Web App**: 1GB RAM, 1 CPU
- ✅ **PostgreSQL**: 1GB storage
- ✅ **Custom Domain**: Included
- ✅ **SSL Certificate**: Included
- ✅ **Global CDN**: Included

### Paid Tier ($5/month):
- ✅ **Always-on**: No sleep mode
- ✅ **More resources**: 2GB RAM, 2 CPU
- ✅ **Priority support**: Included

## Alternative: Render.com (Free 750 hours)

If you want completely free hosting:

### Step 1: Deploy on Render
1. Go to https://render.com
2. Sign in with GitHub
3. Click "New" → "Web Service"
4. Connect your repository
5. Set build command: `dotnet publish -c Release -o out`
6. Set start command: `dotnet out/visa_consulatant.dll`

### Step 2: Add Database
1. Click "New" → "PostgreSQL"
2. Render auto-links it to your app

### Step 3: Configure
- **Free tier**: 750 hours/month (31 days)
- **Sleep mode**: App sleeps after 15 minutes of inactivity
- **Wake time**: ~30 seconds to wake up

## Database Migration

After deployment, run migrations:

### Railway:
```bash
# In Railway dashboard → Deployments → View Logs
dotnet ef database update
```

### Render:
```bash
# In Render dashboard → Shell
dotnet ef database update
```

## Monitoring

### Railway:
- **Logs**: Real-time in dashboard
- **Metrics**: CPU, memory, requests
- **Alerts**: Automatic notifications

### Render:
- **Logs**: Real-time in dashboard
- **Health checks**: Automatic monitoring
- **Uptime**: 99.9% guaranteed

## Scaling Options

### Railway:
- **Free**: $5 credit/month
- **Paid**: $5/month for always-on
- **Pro**: $20/month for more resources

### Render:
- **Free**: 750 hours/month
- **Paid**: $7/month for always-on
- **Pro**: $25/month for more resources

## Troubleshooting

### Common Issues:
1. **App won't start**: Check startup command
2. **Database connection**: Verify environment variables
3. **Build fails**: Check Dockerfile
4. **Sleep mode**: Upgrade to paid plan

### Support:
- **Railway**: Discord community, email support
- **Render**: Documentation, email support
- **Both**: Excellent free support

## Recommendation

**For absolute cheapest**: Use **Railway** with free tier ($5 credit/month)
**For completely free**: Use **Render** with 750 free hours/month

Both are excellent choices and much cheaper than Azure! 