-- Table for User Roles

CREATE TABLE Role (

    UniqueId INT PRIMARY KEY,

    Name NVARCHAR(255) NOT NULL,

    Description NVARCHAR(255)

);

 

-- Table for Customers

CREATE TABLE Customer (

    UniqueId INT PRIMARY KEY,

    Name NVARCHAR(255) NOT NULL,

    BillingInfo NVARCHAR(255),

    Address NVARCHAR(255)

);

 

-- Table for Users with extended roles and customers

CREATE TABLE [User] (

    UniqueId INT PRIMARY KEY,

    Username NVARCHAR(255) NOT NULL,

    PasswordHash NVARCHAR(255) NOT NULL,

    RoleId INT,

    CustomerId INT NULL,

    CONSTRAINT FK_User_Role FOREIGN KEY (RoleId) REFERENCES Role(UniqueId),

    CONSTRAINT FK_User_Customer FOREIGN KEY (CustomerId) REFERENCES Customer(UniqueId)

);

 

-- Table for Drivers with qualifications

CREATE TABLE Driver (

    UniqueId INT PRIMARY KEY,

    UserId INT,

    Qualifications NVARCHAR(255),

    CONSTRAINT FK_Driver_User FOREIGN KEY (UserId) REFERENCES [User](UniqueId)

);

 

-- Table for Vehicles

CREATE TABLE Vehicle (

    UniqueId INT PRIMARY KEY,

    Make NVARCHAR(255),

    Model NVARCHAR(255),

    Year INT,

    LicensePlate NVARCHAR(255),

    Status NVARCHAR(255)

);

 

-- Table for Goods Categories

CREATE TABLE GoodsCategory (

    UniqueId INT PRIMARY KEY,

    Name NVARCHAR(255) NOT NULL,

    RequiresInspection BIT,

    RequiresQualifiedDriver BIT

);

 

-- Table for Items with category

CREATE TABLE Item (

    UniqueId INT PRIMARY KEY,

    Description NVARCHAR(255),

    Weight DECIMAL(10, 2),

    CategoryId INT,

    CONSTRAINT FK_Item_GoodsCategory FOREIGN KEY (CategoryId) REFERENCES GoodsCategory(UniqueId)

);

 

-- Table for Pickup and Delivery Requests

CREATE TABLE PickupDeliveryRequest (

    UniqueId INT PRIMARY KEY,

    CustomerId INT,

    PickupLocation NVARCHAR(255),

    DeliveryLocation NVARCHAR(255),

    RequestedPickupDate DATETIME,

    RequestedDeliveryDate DATETIME,

    Status NVARCHAR(255),

    CONSTRAINT FK_PickupDeliveryRequest_Customer FOREIGN KEY (CustomerId) REFERENCES Customer(UniqueId)

);

 

-- Table for Trip

CREATE TABLE Trip (

    UniqueId INT PRIMARY KEY,

    VehicleId INT,

    DriverId INT,

    StartDate DATETIME,

    EndDate DATETIME,

    Status NVARCHAR(255),

    CONSTRAINT FK_Trip_Vehicle FOREIGN KEY (VehicleId) REFERENCES Vehicle(UniqueId),

    CONSTRAINT FK_Trip_Driver FOREIGN KEY (DriverId) REFERENCES Driver(UniqueId)

);

 

-- Table for Trip Manifests to track items in a trip

CREATE TABLE TripManifest (

    UniqueId INT PRIMARY KEY,

    TripId INT,

    ItemId INT,

    PickupRequestId INT,

    DeliveryRequestId INT,

    CONSTRAINT FK_TripManifest_Trip FOREIGN KEY (TripId) REFERENCES Trip(UniqueId),

    CONSTRAINT FK_TripManifest_Item FOREIGN KEY (ItemId) REFERENCES Item(UniqueId),

    CONSTRAINT FK_TripManifest_PickupRequest FOREIGN KEY (PickupRequestId) REFERENCES PickupDeliveryRequest(UniqueId),

    CONSTRAINT FK_TripManifest_DeliveryRequest FOREIGN KEY (DeliveryRequestId) REFERENCES PickupDeliveryRequest(UniqueId)

);

 

-- Table for Waypoints

CREATE TABLE Waypoint (

    UniqueId INT PRIMARY KEY,

    TripId INT,

    Location NVARCHAR(255),

    ArrivalTime DATETIME,

    DepartureTime DATETIME,

    CONSTRAINT FK_Waypoint_Trip FOREIGN KEY (TripId) REFERENCES Trip(UniqueId)

);

 

-- Table for Events

CREATE TABLE Event (

    UniqueId INT PRIMARY KEY,

    TripId INT,

    EventType NVARCHAR(255),

    EventTime DATETIME,

    Description NVARCHAR(255),

    CONSTRAINT FK_Event_Trip FOREIGN KEY (TripId) REFERENCES Trip(UniqueId)

);

 

-- Table for Expenses

CREATE TABLE Expense (

    UniqueId INT PRIMARY KEY,

    TripId INT,

    Amount DECIMAL(10, 2),

    ExpenseType NVARCHAR(255),

    Description NVARCHAR(255),

    CONSTRAINT FK_Expense_Trip FOREIGN KEY (TripId) REFERENCES Trip(UniqueId)

);

 

-- Table for Inspections and Sign-offs for fragile items

CREATE TABLE InspectionSignOff (

    UniqueId INT PRIMARY KEY,

    ItemId INT,

    InspectedBy INT,

    SignOffBy INT,

    InspectionDate DATETIME,

    SignOffDate DATETIME,

    CONSTRAINT FK_InspectionSignOff_Item FOREIGN KEY (ItemId) REFERENCES Item(UniqueId),

    CONSTRAINT FK_InspectionSignOff_InspectedBy FOREIGN KEY (InspectedBy) REFERENCES [User](UniqueId),

    CONSTRAINT FK_InspectionSignOff_SignOffBy FOREIGN KEY (SignOffBy) REFERENCES [User](UniqueId)

);

 

-- Table for Bills

CREATE TABLE Bill (

    UniqueId INT PRIMARY KEY,

    CustomerId INT,

    Amount DECIMAL(10, 2),

    BillDate DATETIME,

    DueDate DATETIME,

    Status NVARCHAR(255),

    CONSTRAINT FK_Bill_Customer FOREIGN KEY (CustomerId) REFERENCES Customer(UniqueId)

);