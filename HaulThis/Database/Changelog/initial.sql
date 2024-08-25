CREATE TABLE role (
    Id INT PRIMARY KEY IDENTITY(1,1),
    roleName VARCHAR(255) NOT NULL
);

INSERT INTO role (roleName) VALUES ('Customer');
INSERT INTO role (roleName) VALUES ('Administrator');
INSERT INTO role (roleName) VALUES ('Driver');

CREATE TABLE users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    roleId INT,
    FOREIGN KEY (roleId) REFERENCES role(Id)
);

CREATE TABLE expense (
    Id INT PRIMARY KEY IDENTITY(1,1),
    userId INT,
    amount DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (userId) REFERENCES users(Id)
);

CREATE TABLE bill (
    Id INT PRIMARY KEY IDENTITY(1,1),
    userId INT,
    FOREIGN KEY (userId) REFERENCES users(Id)
);

CREATE TABLE vehicle (
    uniqueId INT PRIMARY KEY IDENTITY(1,1),
    vehicleName VARCHAR(255) NOT NULL
);

CREATE TABLE trip (
    Id INT PRIMARY KEY IDENTITY(1,1),
    vehicleId INT,
    driverId INT,
    FOREIGN KEY (vehicleId) REFERENCES vehicle(uniqueId),
    FOREIGN KEY (driverId) REFERENCES users(Id)
);

CREATE TABLE item (
    Id INT PRIMARY KEY IDENTITY(1,1),
    tripId INT,
    billId INT,
    pickedUpBy INT,
    deliveredBy INT,
    FOREIGN KEY (tripId) REFERENCES trip(Id),
    FOREIGN KEY (billId) REFERENCES bill(Id),
    FOREIGN KEY (pickedUpBy) REFERENCES users(Id),
    FOREIGN KEY (deliveredBy) REFERENCES users(Id)
);

CREATE TABLE event (
    Id INT PRIMARY KEY IDENTITY(1,1),
    tripId INT,
    eventName VARCHAR(255) NOT NULL,
    FOREIGN KEY (tripId) REFERENCES trip(Id)
);

CREATE TABLE waypoint (
    Id INT PRIMARY KEY IDENTITY(1,1),
    tripId INT,
    userId INT,
    location VARCHAR(255) NOT NULL,
    FOREIGN KEY (tripId) REFERENCES trip(Id),
    FOREIGN KEY (userId) REFERENCES users(Id)
);
