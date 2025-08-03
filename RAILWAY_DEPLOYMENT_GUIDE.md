# Railway Deployment Guide for Visa Consultant App

## Overview
This guide will help you deploy your Visa Consultant application to Railway with proper database setup and configuration.

## Prerequisites
- Railway account
- Git repository with your code
- PostgreSQL database service on Railway

## Step 1: Database Setup

### 1.1 Create PostgreSQL Database Service
1. In your Railway project, click "New Service"
2. Select "Database" → "PostgreSQL"
3. Name it `visa-consultant-db`
4. Wait for the database to be provisioned

### 1.2 Configure Database Variables
The following variables should be automatically set by Railway:
- `DATABASE_URL` - PostgreSQL connection string
- `PGDATABASE` - Database name
- `PGHOST` - Database host
- `PGPORT` - Database port
- `PGUSER` - Database username
- `PGPASSWORD` - Database password

## Step 2: Application Service Setup

### 2.1 Deploy Application
1. In your Railway project, click "New Service"
2. Select "GitHub Repo" and connect your repository
3. Name it `visa-consultant-app`
4. Railway will automatically detect it's a .NET application

### 2.2 Configure Application Variables
Add these environment variables to your application service:

```
ASPNETCORE_ENVIRONMENT=Production
JWT_SECRET_KEY=your-super-secret-jwt-key-here
ADMIN_EMAIL=your-admin-email@example.com
SMTP_SERVER=smtp.gmail.com
SMTP_PORT=587
```

### 2.3 Link Database to Application
1. In your application service settings
2. Go to "Variables" tab
3. Add a new variable with:
   - Name: `DATABASE_URL`
   - Value: Reference the database service variable
   - Click the database icon to reference the PostgreSQL service

## Step 3: Deployment Configuration

### 3.1 Build Configuration
Railway will automatically:
- Detect the Dockerfile
- Build the Docker image
- Run the deployment script

### 3.2 Health Checks
The application includes health check endpoints:
- `/health` - Basic health status
- `/health/db` - Database connection status

## Step 4: Verify Deployment

### 4.1 Check Application Logs
1. Go to your application service
2. Click "Logs" tab
3. Look for:
   - "Database migrations completed successfully!"
   - "Starting the application..."

### 4.2 Test Health Endpoints
Visit these URLs:
- `https://your-app-url.railway.app/health`
- `https://your-app-url.railway.app/health/db`

### 4.3 Verify Database Tables
1. Go to your database service
2. Click "Data" tab
3. You should see tables like:
   - Users
   - HomePageContents
   - VisaServices
   - ContactInquiries
   - etc.

## Step 5: Access Your Application

### 5.1 Main Application
- URL: `https://your-app-url.railway.app`
- Admin Panel: `https://your-app-url.railway.app/admin.html`

### 5.2 API Documentation
- Swagger UI: `https://your-app-url.railway.app/swagger`

### 5.3 Default Admin Credentials
- Username: `admin`
- Password: `Admin@123`
- Email: `admin@gurukirpa.com`

## Troubleshooting

### Database Connection Issues
1. Check if `DATABASE_URL` is properly set
2. Verify database service is running
3. Check application logs for connection errors

### Migration Issues
1. Check if Entity Framework tools are installed
2. Verify connection string format
3. Check for pending migrations

### Build Issues
1. Ensure all required packages are in `.csproj`
2. Check Dockerfile syntax
3. Verify `.dockerignore` excludes unnecessary files

## Environment Variables Reference

### Required Variables
| Variable | Description | Example |
|----------|-------------|---------|
| `DATABASE_URL` | PostgreSQL connection string | `postgresql://user:pass@host:port/db` |
| `ASPNETCORE_ENVIRONMENT` | Environment name | `Production` |
| `JWT_SECRET_KEY` | JWT signing key | `your-secret-key` |

### Optional Variables
| Variable | Description | Default |
|----------|-------------|---------|
| `ADMIN_EMAIL` | Admin email for notifications | `admin@gurukirpa.com` |
| `SMTP_SERVER` | SMTP server for emails | `smtp.gmail.com` |
| `SMTP_PORT` | SMTP port | `587` |

## Security Notes
1. Change default admin password after first login
2. Use strong JWT secret key
3. Configure proper CORS settings for production
4. Enable HTTPS (automatic on Railway)

## Monitoring
- Use Railway's built-in metrics
- Monitor application logs
- Set up alerts for health check failures
- Monitor database performance

## Backup Strategy
- Railway provides automatic PostgreSQL backups
- Configure backup retention in database settings
- Test restore procedures regularly 