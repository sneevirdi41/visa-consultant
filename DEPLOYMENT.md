# Deployment Guide - Guru Kirpa Visa Consultant

## Free Hosting Options

### 1. Railway.app (Recommended)

#### Prerequisites:
- GitHub account
- Railway account (https://railway.app)

#### Steps:
1. **Push your code to GitHub**
   ```bash
   git init
   git add .
   git commit -m "Initial commit"
   git branch -M main
   git remote add origin https://github.com/yourusername/visa-consultant.git
   git push -u origin main
   ```

2. **Deploy on Railway**
   - Go to https://railway.app
   - Sign in with GitHub
   - Click "New Project"
   - Select "Deploy from GitHub repo"
   - Choose your repository
   - Railway will automatically detect the Dockerfile and deploy

3. **Add PostgreSQL Database**
   - In your Railway project, click "New"
   - Select "Database" → "PostgreSQL"
   - Railway will automatically link it to your app

4. **Set Environment Variables**
   - Go to your app's "Variables" tab
   - Add these environment variables:
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

### 2. Render.com

#### Steps:
1. **Push code to GitHub** (same as above)

2. **Deploy on Render**
   - Go to https://render.com
   - Sign in with GitHub
   - Click "New" → "Web Service"
   - Connect your GitHub repository
   - Set build command: `dotnet publish -c Release -o out`
   - Set start command: `dotnet out/visa_consulatant.dll`
   - Add PostgreSQL database from Render's database section

### 3. Heroku

#### Steps:
1. **Install Heroku CLI**
2. **Create Heroku app**
   ```bash
   heroku create your-app-name
   heroku addons:create heroku-postgresql:hobby-dev
   ```
3. **Deploy**
   ```bash
   git push heroku main
   ```

## Database Migration

After deployment, run database migrations:

```bash
# For Railway/Render (using their CLI or web console)
dotnet ef database update

# For Heroku
heroku run dotnet ef database update
```

## Environment Variables

Make sure to set these environment variables in your hosting platform:

### Required:
- `DB_HOST` - PostgreSQL host
- `DB_NAME` - Database name
- `DB_USER` - Database username
- `DB_PASSWORD` - Database password
- `JWT_SECRET_KEY` - Secret key for JWT tokens

### Optional (for email functionality):
- `SMTP_SERVER` - SMTP server (e.g., smtp.gmail.com)
- `SMTP_PORT` - SMTP port (e.g., 587)
- `SMTP_USERNAME` - Email username
- `SMTP_PASSWORD` - Email password/app password
- `FROM_EMAIL` - From email address
- `ADMIN_EMAIL` - Admin email for notifications

## Testing Your Deployment

1. **Check Health**: Visit `https://your-app-url/swagger`
2. **Test API**: Use Swagger UI to test endpoints
3. **Check Logs**: Monitor application logs in your hosting platform

## Default Admin Account

The application creates a default admin account:
- **Username**: admin
- **Password**: Admin@123

**Important**: Change this password after first login!

## Troubleshooting

### Common Issues:
1. **Database Connection**: Ensure PostgreSQL is properly configured
2. **Environment Variables**: Check all required variables are set
3. **Port Configuration**: Ensure port 80/443 is exposed
4. **Build Errors**: Check Dockerfile and .dockerignore

### Logs:
- Check application logs in your hosting platform
- Look for database connection errors
- Verify environment variables are loaded correctly 