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
    "Password" NVARCHAR(200) NOT NULL,
    RoleId INT NOT NULL,

    CONSTRAINT FK_RoleUser_User FOREIGN KEY (RoleId) REFERENCES RolesUser(Id)
);

CREATE TABLE CategoriesProduct (    -- t-shirt, jeans, pants, trousers, blouse, etc.
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Category VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE TypesProduct (        -- New, used
    Id INT IDENTITY(1,1) PRIMARY KEY,
    "Type" VARCHAR(20) NOT NULL UNIQUE
);

CREATE TABLE StatusesProduct (        -- Not paused, Paused
    Id INT IDENTITY(1,1) PRIMARY KEY,
    "Status" VARCHAR(20) NOT NULL UNIQUE
);

CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    "Name" VARCHAR(100) NOT NULL,
    Price DECIMAL(12,2) NOT NULL,
    Discount DECIMAL(12,2),
    NumberSold INT NOT NULL,
    AverageStars DECIMAL(2,1),
    "Description" TEXT,
    StockAvailable INT,
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

    CONSTRAINT FK_PhotoProduct_Product FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

CREATE TABLE QAProduct (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Stars DECIMAL(2,1),
    "Description" TEXT,
    "Date" Date NOT NULL,
    ProductId INT NOT NULL,
    UserId INT NOT NULL

    CONSTRAINT FK_QAProduct_Product FOREIGN KEY (ProductId) REFERENCES Products(Id),
    CONSTRAINT FK_UserQA_Product FOREIGN KEY (UserId) REFERENCES Users(Id),
);

CREATE TABLE StatusesAuction (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    "Status" VARCHAR(20) NOT NULL UNIQUE
);

CREATE TABLE AuctionsProduct (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Number INT NOT NULL,
    FirstPrice DECIMAL(12,2),
    MinBid DECIMAL(12,2),
    LastPrice DECIMAL(12,2),
    ProductId INT NOT NULL,
    StatusId INT NOT NULL,
    BuyerId INT,

    CONSTRAINT FK_AuctionProduct_Product FOREIGN KEY (ProductId) REFERENCES Products(Id),
    CONSTRAINT FK_StatusAuction_Auction FOREIGN KEY (StatusId) REFERENCES StatusesAuction(Id),
    CONSTRAINT FK_BuyerProduct_Product FOREIGN KEY (BuyerId) REFERENCES Users(Id),
);

-- RolesUser
INSERT INTO RolesUser(Role) VALUES ('Administrator');
INSERT INTO RolesUser(Role) VALUES ('Seller/Buyer');

-- Users
INSERT INTO Users (FirstName, MiddleName, LastName, Username, Email, Password, RoleId) VALUES 
('Juan', 'Carlos', 'Hernández', 'juancho', 'juan@example.com', '123456', 1),
('Maria', 'Luisa', 'Gonzalez', 'marilu', 'maria@example.com', 'abcdef', 2),
('Pedro', 'José', 'Ramirez', 'pedrito', 'pedro@example.com', 'pass123', 2);


-- CategoriesProduct
INSERT INTO CategoriesProduct (Category) VALUES 
('Playeras'),
('Pantalones'),
('Blusas');

-- TypesProduct
INSERT INTO TypesProduct (Type) VALUES 
('New'),
('Used');

-- StatusesProduct
INSERT INTO StatusesProduct (Status) VALUES 
('Available'),
('Paused'),
('Sold out');

-- Products
INSERT INTO Products (Name, Price, Discount, NumberSold, AverageStars, Description, StockAvailable, SellerId, CategoryId, TypeId, StatusId) VALUES 
('Playera Blanca',      199.99, 0, 15, 4.5,     'Playera 100% algodón',     10, 2, 1, 1, 1),
('Pantalón Jeans Azul', 499.50, 50, 30, 4.0,    'Jeans de mezclilla',       25, 2, 2, 2, 1),
('Blusa Estampada',     349.00, 20, 10, 3.5,    'Blusa con diseño floral',  5, 3, 3, 1, 2),
('Blusa negra',         123.30, 0, 10, 3.5,     'Blusa de color negro',     5, 3, 3, 1, 2);

-- PhotosProduct (solo insert de ejemplo sin foto real, usa NULL o CONVERT si deseas agregar imágenes reales)
INSERT INTO PhotosProduct (Photo, ProductId) VALUES 
(CAST('0x' AS VARBINARY(MAX)), 1),
(CAST('0x' AS VARBINARY(MAX)), 2),
(CAST('0x' AS VARBINARY(MAX)), 3);

-- QAProduct
INSERT INTO QAProduct (Stars, Description, Date, ProductId, UserId) VALUES 
(5.0, 'Excelente producto', GETDATE(), 1, 1),
(3.5, 'Calidad regular', GETDATE(), 2, 2),
(4.0, 'Buena compra', GETDATE(), 3, 3);

-- StatusesAuction
INSERT INTO StatusesAuction (Status) VALUES 
('Active'),
('Paused'),
('Canceled');

-- AuctionsProduct
INSERT INTO AuctionsProduct (Number, FirstPrice, MinBid, LastPrice, ProductId, StatusId, BuyerId) VALUES 
(1, 100.00, 10.00, 130.00, 1, 1, 3),
(2, 200.00, 15.00, 250.00, 2, 2, 2),
(3, 300.00, 20.00, 0.00, 3, 1, NULL);