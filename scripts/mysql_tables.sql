use querier;

CREATE TABLE Category (
  entityId INT AUTO_INCREMENT NOT NULL,
  categoryName VARCHAR(15) NOT NULL,
  description TEXT NULL,
  picture BLOB NULL,
  PRIMARY KEY (entityId)
  ) ENGINE=INNODB;
CREATE TABLE Region (
  entityId INT AUTO_INCREMENT NOT NULL,
  regiondescription VARCHAR(50) NOT NULL,
  PRIMARY KEY (entityId)
  ) ENGINE=INNODB;

CREATE TABLE Territory (
  entityId INT AUTO_INCREMENT NOT NULL,
  territoryCode VARCHAR(20) NOT NULL,
  territorydescription VARCHAR(50) NOT NULL,
  regionId INT NOT NULL,
  PRIMARY KEY (entityId),
  FOREIGN KEY (regionId) REFERENCES Region(entityId)
  ) ENGINE=INNODB;

CREATE TABLE CustomerDemographics (
  entityId INT AUTO_INCREMENT NOT NULL,
  customerDesc TEXT NULL,
  PRIMARY KEY (entityId)
  ) ENGINE=INNODB;

CREATE TABLE Customer (
  entityId INT AUTO_INCREMENT NOT NULL,
  companyName VARCHAR(40) NOT NULL,
  contactName VARCHAR(30) NULL,
  contactTitle VARCHAR(30) NULL,
  address VARCHAR(60) NULL,
  city VARCHAR(15) NULL,
  region VARCHAR(15) NULL,
  postalCode VARCHAR(10) NULL,
  country VARCHAR(15) NULL,
  phone VARCHAR(24) NULL,
  mobile VARCHAR(24) NULL,
  email VARCHAR(225) NULL,
  fax VARCHAR(24) NULL,
  PRIMARY KEY (entityId)
  ) ENGINE=INNODB;


CREATE TABLE CustomerCustomerDemographics (
  entityId INT AUTO_INCREMENT NOT NULL,
  customerId  INT NOT NULL,
  customerTypeId  INT NOT NULL,
  PRIMARY KEY (entityId),
  FOREIGN KEY (customerId)
      REFERENCES Customer(entityId),
  FOREIGN KEY (customerTypeId)
      REFERENCES CustomerDemographics(entityId)
  ) ENGINE=INNODB;


CREATE TABLE Employee (
  entityId INT AUTO_INCREMENT NOT NULL,
  lastname VARCHAR(20) NOT NULL,
  firstname VARCHAR(10) NOT NULL,
  title VARCHAR(30) NULL,
  titleOfCourtesy VARCHAR(25) NULL,
  birthDate DATETIME NULL,
  hireDate DATETIME NULL,
  address VARCHAR(60) NULL,
  city VARCHAR(15) NULL,
  region VARCHAR(15) NULL,
  postalCode VARCHAR(10) NULL,
  country VARCHAR(15) NULL,
  phone VARCHAR(24) NULL,
  extension VARCHAR(4) NULL,
  mobile VARCHAR(24) NULL,
  email VARCHAR(225) NULL,
  photo BLOB NULL,
  notes BLOB NULL,
  mgrId INT NULL,
  photoPath VARCHAR(255) NULL,
  PRIMARY KEY (entityId)
  ) ENGINE=INNODB;

CREATE TABLE EmployeeTerritory (
  entityId INT AUTO_INCREMENT NOT NULL,
  employeeId INT NOT NULL,
  territoryId INT NOT NULL,
  territoryCode VARCHAR(20) NOT NULL,
  PRIMARY KEY (entityId),
  FOREIGN KEY (employeeId)
  	REFERENCES Employee(entityId),
  FOREIGN KEY (territoryId)
  	REFERENCES Territory(entityId)
  ) ENGINE=INNODB;

CREATE TABLE Supplier (
  entityId INT AUTO_INCREMENT NOT NULL,
  companyName VARCHAR(40) NOT NULL,
  contactName VARCHAR(30) NULL,
  contactTitle VARCHAR(30) NULL,
  address VARCHAR(60) NULL,
  city VARCHAR(15) NULL,
  region VARCHAR(15) NULL,
  postalCode VARCHAR(10) NULL,
  country VARCHAR(15) NULL,
  phone VARCHAR(24) NULL,
  email VARCHAR(225) NULL,
  fax VARCHAR(24) NULL,
  HomePage TEXT NULL,
  PRIMARY KEY (entityId)
  ) ENGINE=INNODB;



CREATE TABLE Product (
  entityId INT AUTO_INCREMENT NOT NULL,
  productName VARCHAR(40) NOT NULL,
  supplierId INT NULL,
  categoryId INT NULL,
  quantityPerUnit VARCHAR(20) NULL,
  unitPrice DECIMAL(10, 2) NULL,
  unitsInStock SMALLINT NULL,
  unitsOnOrder SMALLINT NULL,
  reorderLevel SMALLINT NULL,
  discontinued CHAR(1) NOT NULL,
  PRIMARY KEY (entityId),
  FOREIGN KEY (supplierId) REFERENCES Supplier(entityId),
  FOREIGN KEY (categoryId) REFERENCES Category(entityId)
  ) ENGINE=INNODB;



CREATE TABLE Shipper (
  entityId INT AUTO_INCREMENT NOT NULL,
  companyName VARCHAR(40) NOT NULL,
  phone VARCHAR(44) NULL,
  PRIMARY KEY (entityId)
  ) ENGINE=INNODB;




CREATE TABLE SalesOrder (
  entityId INT AUTO_INCREMENT NOT NULL,
  customerId INT NOT NULL,
  employeeId INT NULL,
  orderDate DATETIME NULL,
  requiredDate DATETIME NULL,
  shippedDate DATETIME NULL,
  shipperId INT NOT NULL,
  freight DECIMAL(10, 2) NULL,
  shipName VARCHAR(40) NULL,
  shipAddress VARCHAR(60) NULL,
  shipCity VARCHAR(15) NULL,
  shipRegion VARCHAR(15) NULL,
  shipPostalCode VARCHAR(10) NULL,
  shipCountry VARCHAR(15) NULL,
  PRIMARY KEY (entityId),
  FOREIGN KEY (shipperId) REFERENCES Shipper(entityId),
  FOREIGN KEY (customerId) REFERENCES Customer(entityId) 

  ) ENGINE=INNODB;



CREATE TABLE OrderDetail (
   entityId INT AUTO_INCREMENT NOT NULL,
   orderId INT NOT NULL,
   productId INT NOT NULL,
   unitPrice DECIMAL(10, 2) NOT NULL,
   quantity SMALLINT NOT NULL,
   discount DECIMAL(10, 2) NOT NULL,
   PRIMARY KEY (entityId),
   FOREIGN KEY (orderId) REFERENCES SalesOrder(entityId),
   FOREIGN KEY (productId) REFERENCES Product(entityId) 
  ) ENGINE=INNODB;
  
  ALTER TABLE Territory ADD UNIQUE INDEX IDX_TerrytoryCode (territoryCode);

ALTER TABLE Territory ADD UNIQUE INDEX IDX_TerritoryCode_RegionId (territoryCode, regionId);

ALTER TABLE CustomerCustomerDemographics ADD UNIQUE INDEX IDX_CustomerId_CustomerTypeId (customerId, customerTypeId);

ALTER TABLE EmployeeTerritory ADD FOREIGN KEY (territoryCode) REFERENCES Territory(territoryCode);

ALTER TABLE EmployeeTerritory ADD UNIQUE INDEX IDX_EmployeeId_TerritoryCode (employeeId, territoryCode);

ALTER TABLE OrderDetail ADD UNIQUE INDEX IDX_OrderId_ProductId (orderId, productId);
