# Railway Deployment Guide for Visa Consultant App

## 🚀 Quick Deployment Steps

### 1. Database Setup
- **Service Name**: `visa-consultant-db`
- **Type**: PostgreSQL
- **Status**: ✅ Connected and working

### 2. Application Setup
- **Service Name**: `visa-consultant-app`
- **Type**: Web Service
- **Status**: ✅ Deployed and running

### 3. Environment Variables (CRITICAL FIX NEEDED)

**Current Issue**: `JWT_SECRET_KEY` is missing, causing login failures.

**Required Environment Variables for `visa-consultant-app`:**

```bash
# Database Connection (✅ Already Set)
PGHOST=visa-consultant-db.railway.internal
PGPORT=5432
PGDATABASE=railway
PGUSER=postgres
PGPASSWORD=YLNMKzOXotAikdYDuXVUVFsTgUrbpivA

# Application Settings (❌ MISSING - NEED TO ADD)
JWT_SECRET_KEY=your-super-secret-jwt-key-2024-your-super-secret-jwt-key-2024
ADMIN_EMAIL=admin@gurukirpa.com
SMTP_SERVER=smtp.gmail.com
SMTP_PORT=587

# Environment (✅ Already Set)
ASPNETCORE_ENVIRONMENT=Production
```

### 4. Add Missing Environment Variables

**Go to Railway Dashboard → `visa-consultant-app` → Variables → New Variable:**

1. **JWT_SECRET_KEY** = `your-super-secret-jwt-key-2024-your-super-secret-jwt-key-2024`
2. **ADMIN_EMAIL** = `admin@gurukirpa.com`
3. **SMTP_SERVER** = `smtp.gmail.com`
4. **SMTP_PORT** = `587`

### 5. Test Application

After adding the environment variables, test:

```bash
# Test health
curl https://visa-consultant-app-production-07b2.up.railway.app/health

# Test environment variables
curl -X POST https://visa-consultant-app-production-07b2.up.railway.app/api/Auth/test-connection

# Test login (should work after adding JWT_SECRET_KEY)
curl -X POST https://visa-consultant-app-production-07b2.up.railway.app/api/Auth/login \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "Admin@123"}'
```

## 🔧 Current Status

### ✅ Working Components:
- Database connection: **CONNECTED**
- Application deployment: **SUCCESSFUL**
- Database tables: **CREATED**
- User data: **SEEDED** (2 users in database)

### ❌ Issues to Fix:
- **JWT_SECRET_KEY missing** → Login fails with 500 error
- **SMTP settings missing** → Email features won't work

## 🎯 Expected Results After Fix

**Environment Variables Test:**
```json
{
  "jwtSecretKey": "set",
  "environment": "Production",
  "databaseUrl": "postgresql://postgres:...",
  "pgHost": "visa-consultant-db.railway.internal"
}
```

**Login Test:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin",
  "role": "Admin",
  "expiresAt": "2025-08-03T18:XX:XX.XXXXXXXZ"
}
```

## 🚨 Immediate Action Required

**Add these environment variables to Railway dashboard:**

1. Go to `visa-consultant-app` service
2. Click "Variables" tab
3. Add each variable:
   - `JWT_SECRET_KEY` = `your-super-secret-jwt-key-2024-your-super-secret-jwt-key-2024`
   - `ADMIN_EMAIL` = `admin@gurukirpa.com`
   - `SMTP_SERVER` = `smtp.gmail.com`
   - `SMTP_PORT` = `587`

**After adding variables, Railway will automatically redeploy and login will work!**

## 📊 Current Test Results

**Health Check:** ✅ Healthy
**Database:** ✅ Connected (2 users)
**Environment Variables:** ❌ JWT_SECRET_KEY missing
**Login:** ❌ Fails due to missing JWT_SECRET_KEY

## 🔗 Application URLs

- **Main App**: https://visa-consultant-app-production-07b2.up.railway.app
- **Health Check**: https://visa-consultant-app-production-07b2.up.railway.app/health
- **API Base**: https://visa-consultant-app-production-07b2.up.railway.app/api

## 👤 Default Admin User

- **Username**: `admin`
- **Password**: `Admin@123`
- **Email**: `admin@gurukirpa.com`
- **Role**: `Admin`

## 🛠️ Troubleshooting

### If login still fails after adding JWT_SECRET_KEY:
1. Check Railway logs for errors
2. Test environment variables: `/api/Auth/test-connection`
3. Test database: `/api/Auth/test-db`
4. Test JWT service: `/api/Auth/test-jwt`

### If database connection fails:
1. Verify PostgreSQL service is running
2. Check environment variables are correct
3. Restart the application service

## 📝 Notes

- The application uses Entity Framework Core with PostgreSQL
- Database migrations run automatically on startup
- JWT tokens expire after 60 minutes
- All API endpoints are prefixed with `/api`
- Health checks are available at `/health` and `/health/db` 