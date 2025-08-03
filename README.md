# Guru Kirpa Visa Consultancy

A comprehensive visa consultancy web application built with ASP.NET Core 6.0, featuring user authentication, admin-only content management, and a beautiful responsive frontend.

## Features

### 🏠 **Homepage Management**
- Beautiful, responsive homepage with Guru Kirpa branding
- Admin-only content management (images, text, logo)
- Dynamic content loading from database
- Modern UI with Bootstrap 5 and custom styling

### 🔐 **Authentication & Authorization**
- JWT-based authentication system
- Role-based access control (Admin/User)
- Secure password hashing with BCrypt
- Default admin account: `admin` / `Admin@123`

### 📋 **Visa Services Management**
- CRUD operations for visa services
- Admin-only service management
- Service details including pricing and processing times
- Dynamic service display on homepage

### 📞 **Contact Management**
- Contact inquiry form for potential clients
- Admin dashboard to view and manage inquiries
- Mark inquiries as read/unread
- Email and phone validation

### 🗄️ **Database**
- SQLite database (file-based, no server required)
- Entity Framework Core with Code-First approach
- Seeded data for immediate testing
- Automatic database creation

## Technology Stack

- **Backend**: ASP.NET Core 6.0 Web API
- **Database**: SQLite (file-based)
- **ORM**: Entity Framework Core 6.0
- **Authentication**: JWT Bearer Tokens
- **Frontend**: HTML5, CSS3, JavaScript, Bootstrap 5
- **Password Hashing**: BCrypt.Net-Next

## Prerequisites

- .NET 6.0 SDK or later
- Visual Studio 2022 or VS Code (SQLite is included with .NET)

## Installation & Setup

### 1. Clone the Repository
```bash
git clone <repository-url>
cd visa_consulatant
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Database Setup
The application uses SQLite by default, which creates a file-based database automatically. The database file `GuruKirpaVisaConsultancy.db` will be created in the project root when you first run the application.

If you need to change the database location, update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=GuruKirpaVisaConsultancy.db"
  }
}
```

### 4. Run the Application
```bash
dotnet run
```

The application will:
- Create the database automatically
- Seed initial data (admin user, homepage content, sample services)
- Start the web server

### 5. Access the Application
- **Homepage**: http://localhost:5000 or https://localhost:5001
- **API Documentation**: http://localhost:5000/swagger or https://localhost:5001/swagger

## Default Admin Account

- **Username**: `admin`
- **Password**: `Admin@123`
- **Email**: `admin@gurukirpa.com`

## API Endpoints

### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration

### Homepage Content (Admin Only)
- `GET /api/homepage` - Get homepage content
- `PUT /api/homepage` - Update homepage content (Admin only)

### Visa Services
- `GET /api/visaservices` - Get all active services
- `GET /api/visaservices/{id}` - Get specific service
- `POST /api/visaservices` - Create new service (Admin only)
- `PUT /api/visaservices/{id}` - Update service (Admin only)
- `DELETE /api/visaservices/{id}` - Delete service (Admin only)

### Contact Inquiries
- `POST /api/contact/inquiry` - Submit contact inquiry
- `GET /api/contact/inquiries` - Get all inquiries (Admin only)
- `GET /api/contact/inquiries/{id}` - Get specific inquiry (Admin only)
- `PUT /api/contact/inquiries/{id}/mark-read` - Mark inquiry as read (Admin only)
- `DELETE /api/contact/inquiries/{id}` - Delete inquiry (Admin only)

## Database Schema

### Users
- Id, Username, Email, PasswordHash, Role, CreatedAt, IsActive

### HomePageContent
- Id, Title, Description, HeroImageUrl, LogoUrl, BannerImageUrl, WelcomeMessage, ServicesOverview, LastUpdated, UpdatedBy

### VisaServices
- Id, Name, Description, ImageUrl, Price, ProcessingTime, Country, IsActive, CreatedAt, UpdatedAt

### ContactInquiries
- Id, Name, Email, Phone, Message, Subject, VisaType, CreatedAt, IsRead, ReadAt

## Admin Features

### Homepage Management
1. Login with admin credentials
2. Use the API endpoint `PUT /api/homepage` to update content
3. Include JWT token in Authorization header: `Bearer <token>`

### Service Management
1. Create, update, or delete visa services
2. All operations require admin authentication
3. Services are automatically displayed on the homepage

### Contact Management
1. View all contact inquiries
2. Mark inquiries as read/unread
3. Delete inquiries when no longer needed

## Frontend Features

### Responsive Design
- Mobile-first approach
- Bootstrap 5 for responsive layout
- Custom CSS for branding

### Dynamic Content
- Services loaded from API
- Contact form integration
- Smooth scrolling navigation

### User Experience
- Modern, professional design
- Fast loading times
- Intuitive navigation

## Security Features

- JWT token authentication
- Role-based authorization
- Password hashing with BCrypt
- Input validation and sanitization
- CORS configuration

## Development

### Adding New Features
1. Create models in the `Models` folder
2. Add DbSet to `ApplicationDbContext`
3. Create controllers in the `Controllers` folder
4. Update database with migrations if needed

### Database Migrations
```bash
# Create a new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update
```

## Deployment

### Local Deployment
The application is ready to run locally with the provided configuration.

### Production Deployment
1. Update connection string for production database
2. Configure HTTPS certificates
3. Set up proper CORS policies
4. Use environment variables for sensitive data
5. Configure logging and monitoring

## Support

For support or questions, please contact the development team.

## License

This project is proprietary software for Guru Kirpa Visa Consultancy.

---

**Guru Kirpa Visa Consultancy** - Your Gateway to Global Opportunities 