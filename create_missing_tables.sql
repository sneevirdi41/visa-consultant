USE GuruKirpaVisaConsultancy;

-- Create ImageUploads table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ImageUploads' AND xtype='U')
BEGIN
    CREATE TABLE ImageUploads (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        FileName NVARCHAR(255) NOT NULL,
        ContentType NVARCHAR(100) NOT NULL,
        ImageData VARBINARY(MAX) NOT NULL,
        ImageType NVARCHAR(50) NOT NULL,
        Title NVARCHAR(255),
        Description NVARCHAR(MAX),
        UploadedAt DATETIME2 DEFAULT GETUTCDATE(),
        IsActive BIT DEFAULT 1
    );
    PRINT 'ImageUploads table created successfully.';
END
ELSE
BEGIN
    PRINT 'ImageUploads table already exists.';
END

-- Create CarouselImages table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='CarouselImages' AND xtype='U')
BEGIN
    CREATE TABLE CarouselImages (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(255),
        Description NVARCHAR(MAX),
        ImageUploadId INT NOT NULL,
        DisplayOrder INT DEFAULT 0,
        CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
        IsActive BIT DEFAULT 1,
        FOREIGN KEY (ImageUploadId) REFERENCES ImageUploads(Id)
    );
    PRINT 'CarouselImages table created successfully.';
END
ELSE
BEGIN
    PRINT 'CarouselImages table already exists.';
END

-- Create Offers table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Offers' AND xtype='U')
BEGIN
    CREATE TABLE Offers (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(255) NOT NULL,
        Description NVARCHAR(MAX) NOT NULL,
        ImageUploadId INT,
        DiscountPercentage DECIMAL(5,2),
        ValidFrom DATETIME2,
        ValidUntil DATETIME2,
        CreatedBy NVARCHAR(100),
        CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
        IsActive BIT DEFAULT 1,
        FOREIGN KEY (ImageUploadId) REFERENCES ImageUploads(Id)
    );
    PRINT 'Offers table created successfully.';
END
ELSE
BEGIN
    PRINT 'Offers table already exists.';
END

-- Insert sample offer data if Offers table is empty
IF NOT EXISTS (SELECT * FROM Offers)
BEGIN
    INSERT INTO Offers (Title, Description, DiscountPercentage, ValidUntil, CreatedBy)
    VALUES 
    ('Student Visa Special', 'Get 20% off on student visa processing fees', 20.00, DATEADD(MONTH, 3, GETUTCDATE()), 'Admin'),
    ('Family Visa Package', 'Complete family visa package with special rates', 15.00, DATEADD(MONTH, 2, GETUTCDATE()), 'Admin');
    PRINT 'Sample offers inserted successfully.';
END
ELSE
BEGIN
    PRINT 'Offers table already has data.';
END

PRINT 'Database setup completed successfully!'; 