USE [master];
IF EXISTS (SELECT * FROM sys.databases WHERE name = N'TrendyClothesDB')
BEGIN
    ALTER DATABASE TrendyClothesDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE TrendyClothesDB;
END;

CREATE DATABASE TrendyClothesDB;
GO

USE TrendyClothesDB;
GO

CREATE LOGIN trendyuser WITH PASSWORD = 'P@ssw0rd123';
GO

CREATE USER trendyuser FOR LOGIN trendyuser;
GO

-- Darle permisos básicos (CRUD)
ALTER ROLE db_datareader ADD MEMBER trendyuser;
ALTER ROLE db_datawriter ADD MEMBER trendyuser;
GO

CREATE TABLE Addresses (
    Id INT IDENTITY(1, 1) PRIMARY KEY,
    Street VARCHAR(100) NOT NULL,
    ExtNumber VARCHAR(30) NOT NULL,
    IntNumber VARCHAR(30),
    Neighborhood VARCHAR(50) NOT NULL,
    City VARCHAR(50) NOT NULL,
    PostalCode VARCHAR(8) NOT NULL,
    State VARCHAR(80) NOT NULL,
    Country VARCHAR(80) NOT NULL,
);

CREATE TABLE RolesUser ( -- Admin, Seller/Buyer
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Role VARCHAR(25) NOT NULL UNIQUE
);

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    MiddleName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Username VARCHAR(40) NOT NULL UNIQUE,
    Email VARCHAR(100) NOT NULL UNIQUE,
    AreaCode VARCHAR(5) NOT NULL,
    PhoneNumber VARCHAR(10) NOT NULL,
    Password NVARCHAR(200) NOT NULL,
    TwoFactorCode NVARCHAR(6),
    RoleId INT NOT NULL,

    CONSTRAINT FK_RoleUser_User FOREIGN KEY (RoleId) REFERENCES RolesUser(Id)
);

CREATE TABLE User_Address (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT,
    AddressId INT,
    IsActive BIT,

    CONSTRAINT FK_UserAddress_User FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_UserAddress_Address FOREIGN KEY (AddressId) REFERENCES Addresses(Id)
);

CREATE TABLE CategoriesProduct (    -- t-shirt, jeans, pants, trousers, blouse, etc.
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Category VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE TypesProduct (        -- New, used
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Type VARCHAR(20) NOT NULL UNIQUE
);

CREATE TABLE StatusesProduct (        -- Not paused, Paused
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Status VARCHAR(20) NOT NULL UNIQUE
);

CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Price DECIMAL(12,2) NOT NULL,
    Discount DECIMAL(12,2),
    NumberSold INT,
    AverageStars DECIMAL(2,1),
    Description VARCHAR(MAX) NOT NULL,
    StockAvailable INT NOT NULL,
    SellerId INT NOT NULL,
    CategoryId INT NOT NULL,
    TypeId INT NOT NULL,
    StatusId INT NOT NULL,

    CONSTRAINT FK_Seller_Product FOREIGN KEY (SellerId) REFERENCES Users(Id),
    CONSTRAINT FK_CategoryProduct_Product FOREIGN KEY (CategoryId) REFERENCES CategoriesProduct(Id),
    CONSTRAINT FK_TypeProduct_Product FOREIGN KEY (TypeId) REFERENCES TypesProduct(Id),
    CONSTRAINT FK_StatusProduct_Product FOREIGN KEY (StatusId) REFERENCES StatusesProduct(Id)
);

CREATE TABLE PhotosProduct (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Photo VARBINARY(MAX) NOT NULL,
    ProductId INT NOT NULL,
    Mime VARCHAR(25) NOT NULL

    CONSTRAINT FK_PhotoProduct_Product FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE
);

CREATE TABLE QAProduct (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Stars DECIMAL(2,1),
    Description TEXT,
    Date Date NOT NULL,
    ProductId INT NOT NULL,
    UserId INT NOT NULL

    CONSTRAINT FK_QAProduct_Product FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserQA_Product FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
);

CREATE TABLE StatusesAuction (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Status VARCHAR(20) NOT NULL UNIQUE
);

CREATE TABLE AuctionsProduct (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    FirstPrice DECIMAL(12,2),
    Bid DECIMAL(12,2) NOT NULL,
    LastPrice DECIMAL(12,2),
    DateStart DATETIME NOT NULL,
    DateEnd DATETIME NOT NULL,
    Description VARCHAR(MAX) NOT NULL,
    StatusId INT NOT NULL,
    SellerId INT NOT NULL,

    CONSTRAINT FK_StatusAuction_Auction FOREIGN KEY (StatusId) REFERENCES StatusesAuction(Id) ON DELETE CASCADE,
    CONSTRAINT FK_BuyerProduct_Product FOREIGN KEY (SellerId) REFERENCES Users(Id) ON DELETE CASCADE,
);

CREATE TABLE PhotosAuction (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Photo VARBINARY(MAX) NOT NULL,
    AuctionId INT NOT NULL,
    Mime VARCHAR(25) NOT NULL

    CONSTRAINT FK_PhotoAuction_Auction FOREIGN KEY (AuctionId) REFERENCES AuctionsProduct(Id) ON DELETE CASCADE
);

CREATE TABLE BidsAuction (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    BuyerId INT NOT NULL,
    AuctionId INT NOT NULL,

    CONSTRAINT FK_BidUser_Auction FOREIGN KEY (BuyerId) REFERENCES Users(Id),
    CONSTRAINT FK_BidAuction_Auction FOREIGN KEY (AuctionId) REFERENCES AuctionsProduct(Id) ON DELETE CASCADE
);


-- RolesUser
INSERT INTO RolesUser(Role) VALUES ('Administrador');
INSERT INTO RolesUser(Role) VALUES ('Comprador/Vendedor');

-- Users
INSERT INTO Users (FirstName, MiddleName, LastName, Username, Email, AreaCode, PhoneNumber, Password, RoleId) VALUES 
('Juan', 'Carlos', 'Hernández', 'juancho', 'juan@example.com', '+52', '1234567890', '123456', 1),
('Rodolfo', 'Fernández', 'Rodríguez', 'foforfr', 'foforfr007@gmail.com', '+52', '2281856845', '123456', 2),
('Maria', 'Luisa', 'Gonzalez', 'marilu', 'maria@example.com', '+52', '0987654321','abcdef', 2),
('Pedro', 'José', 'Ramirez', 'pedrito', 'pedro@example.com', '+52', '1324354657', 'pass123', 2);


-- Addresses
INSERT INTO Addresses (Street, ExtNumber, IntNumber, Neighborhood, City, PostalCode, State, Country) VALUES 
('Enriquez', '232', '2', 'Centro', 'Xalapa', '91097', 'Veracruz', 'México');

-- User_Address
INSERT INTO User_Address (UserId, AddressId, IsActive) VALUES 
(2, 1, 1);

-- CategoriesProduct
INSERT INTO CategoriesProduct (Category) VALUES 
('Pantalón'), ('Playera'), ('Short'), ('Ropa interior'), 
('Accesorios'), ('Joyería'), ('Gorra'), ('Calzado'), ('Suéter'),
('Chamarra'), ('Relojes');

-- TypesProduct
INSERT INTO TypesProduct (Type) VALUES 
('Nuevo'),
('Usado');

-- StatusesProduct
INSERT INTO StatusesProduct (Status) VALUES 
('Activo'), ('Pausado');

-- Products
INSERT INTO Products (Name, Price, Discount, NumberSold, AverageStars, Description, StockAvailable, SellerId, CategoryId, TypeId, StatusId) VALUES 
('Tenis blanco',        199.99, 0, 15, 4.5,     'Tenis blanco 1.',          10, 2, 8, 1, 1),
('Tenis converse',      299.99, 10,16, 4.3,     'Tenis converse mezclilla.',11, 3, 8, 1, 1),
('Tenis rojos',         399.99, 11,14, 4.0,     'Tenis rojos 3.',           21, 4, 8, 1, 1),
('Pantalón mezclilla',  150.99, 0, 1, 5.0,      'Pantalón mezclilla 1.',    12, 2, 1, 1, 1),
('Pantalón blanco',     151.99, 2, 100, 4.2,    'Pantalón blamco.',         13, 3, 1, 1, 1),
('Pantalón mezclilla',  152.99, 3, 22, 3.0,     'Pantalón mezclilla 2.',    14, 4, 1, 1, 1),
('Lentes',              52.99,  0, 2,  3.0,     'Lentes bonitos.',          15, 2, 5, 1, 1),
('Playera negra',       100.99, 0, 2,  3.0,     'Playera negra 1.',         16, 3, 2, 1, 1),
('Playera blanca',      100.99, 0, 2,  3.0,     'Playera blanca.',          17, 4, 2, 1, 1),
('Playera negra',       100.99, 0, 2,  3.0,     'Playera negra 2.',         18, 2, 2, 1, 1),
('Ropa interior',       100.99, 0, 2,  3.0,     'Ropa interior 1.',         18, 3, 4, 1, 1),
('Ropa interior',       100.99, 0, 2,  3.0,     'Ropa interior 2.',         18, 4, 4, 1, 1),
('Ropa interior',       100.99, 0, 2,  3.0,     'Ropa interior 3.',         18, 2, 4, 1, 1);

-- PhotosProduct (solo insert de ejemplo sin foto real, usa NULL o CONVERT si deseas agregar imágenes reales)
INSERT INTO PhotosProduct (Photo, ProductId, Mime) VALUES 
((SELECT BulkColumn FROM OPENROWSET(BULK '/var/opt/mssql/data/Tenis 1.jpg', SINGLE_BLOB) AS Photo), 1, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK '/var/opt/mssql/data/Tenis 2.jpg', SINGLE_BLOB) AS Photo), 2, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK '/var/opt/mssql/data/Tenis 3.jpg', SINGLE_BLOB) AS Photo), 3, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK '/var/opt/mssql/data/Pantalones 1.jpg', SINGLE_BLOB) AS Photo), 4, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK '/var/opt/mssql/data/Pantalones 2.jpg', SINGLE_BLOB) AS Photo), 5, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK '/var/opt/mssql/data/Pantalones 3.jpg', SINGLE_BLOB) AS Photo), 6, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK '/var/opt/mssql/data/Accesorio 1.jpg', SINGLE_BLOB) AS Photo), 7, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK '/var/opt/mssql/data/Playera 1.jpg', SINGLE_BLOB) AS Photo), 8, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK '/var/opt/mssql/data/Playera 2.jpg', SINGLE_BLOB) AS Photo), 9, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK '/var/opt/mssql/data/Playera 3.jpg', SINGLE_BLOB) AS Photo), 10, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK '/var/opt/mssql/data/Ropa interior 1.jpg', SINGLE_BLOB) AS Photo), 11, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK '/var/opt/mssql/data/Ropa interior 2.jpg', SINGLE_BLOB) AS Photo), 12, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK '/var/opt/mssql/data/Ropa interior 3.jpg', SINGLE_BLOB) AS Photo), 13, 'image/jpg');
/*
((SELECT BulkColumn FROM OPENROWSET(BULK 'C:\Archivos\Example Files\TrendyClothes\Tenis 1.jpg', SINGLE_BLOB) AS Photo), 1, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK 'C:\Archivos\Example Files\TrendyClothes\Tenis 2.jpg', SINGLE_BLOB) AS Photo), 2, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK 'C:\Archivos\Example Files\TrendyClothes\Tenis 3.jpg', SINGLE_BLOB) AS Photo), 3, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK 'C:\Archivos\Example Files\TrendyClothes\Pantalones 1.jpg', SINGLE_BLOB) AS Photo), 4, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK 'C:\Archivos\Example Files\TrendyClothes\Pantalones 2.jpg', SINGLE_BLOB) AS Photo), 5, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK 'C:\Archivos\Example Files\TrendyClothes\Pantalones 3.jpg', SINGLE_BLOB) AS Photo), 6, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK 'C:\Archivos\Example Files\TrendyClothes\Accesorio 1.jpg', SINGLE_BLOB) AS Photo), 7, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK 'C:\Archivos\Example Files\TrendyClothes\Playera 1.jpg', SINGLE_BLOB) AS Photo), 8, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK 'C:\Archivos\Example Files\TrendyClothes\Playera 2.jpg', SINGLE_BLOB) AS Photo), 9, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK 'C:\Archivos\Example Files\TrendyClothes\Playera 3.jpg', SINGLE_BLOB) AS Photo), 10, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK 'C:\Archivos\Example Files\TrendyClothes\Ropa interior 1.jpg', SINGLE_BLOB) AS Photo), 11, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK 'C:\Archivos\Example Files\TrendyClothes\Ropa interior 2.jpg', SINGLE_BLOB) AS Photo), 12, 'image/jpg'),
((SELECT BulkColumn FROM OPENROWSET(BULK 'C:\Archivos\Example Files\TrendyClothes\Ropa interior 3.jpg', SINGLE_BLOB) AS Photo), 13, 'image/jpg');
*/

-- QAProduct
INSERT INTO QAProduct (Stars, Description, Date, ProductId, UserId) VALUES 
(5.0, 'Excelente producto', GETDATE(), 1, 1),
(3.5, 'Calidad regular', GETDATE(), 2, 2),
(4.0, 'Buena compra', GETDATE(), 3, 3);

-- StatusesAuction
INSERT INTO StatusesAuction (Status) VALUES 
('Activo'),
('Pausado'),
('Cancelado');


SELECT * FROM Users;

SELECT * FROM Products LEFT JOIN PhotosProduct ON Products.Id = PhotosProduct.ProductId;