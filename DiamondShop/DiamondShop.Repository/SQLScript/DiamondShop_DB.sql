use master;
go
if exists (select name from sys.databases where name = 'PRN231_DiamondShop')
    begin
        drop database PRN231_DiamondShop;
    end;
go
create database PRN231_DiamondShop;
go
use PRN231_DiamondShop;
go
create table [Account]
(
    [Id] uniqueidentifier default newid() primary key,
    [Username] [nvarchar](255) NOT NULL,
    [Password] [nvarchar](255) NOT NULL,
    [Email] [nvarchar](255) NOT NULL,
    [PhoneNumber] [nvarchar](255) NULL,
    [AvatarUrl] [nvarchar](255) NULL,
    [Address] [nvarchar](255) NULL,
    [Role] [nvarchar](255) NOT NULL,
    [Status] [nvarchar](255) NOT NULL default 'available');
go
create table [Category]
(
    Id uniqueidentifier default newid() primary key,
    [Name] nvarchar(50) not null,
    [LastUpdate] datetime default CURRENT_TIMESTAMP,			--for manage history
    [Status] nvarchar(20) not null default 'available'	--available   |   stop-sale   |   deleted
);
go

create table [Certificate]
(
    Id uniqueidentifier default newid() primary key,
    ReportNumber nvarchar(max),
    Origin nvarchar(50) not null,
    Shape nvarchar(100) not null,
    Color nvarchar(50) not null,
    Clarity nvarchar(50) not null,
    Cut nvarchar(50) not null,
    CaratWeight nvarchar(50) not null,
    DateOfIssue datetime not null default CURRENT_TIMESTAMP,
    [Status] nvarchar(20) not null default 'available',	--available   |   not-available   |   deleted
);
go
create table [Diamond]
(
    Id uniqueidentifier default newid() primary key,
    Origin nvarchar(50) not null,
    Shape nvarchar(100) not null,
    Color nvarchar(50) not null,
    Clarity nvarchar(50) not null,
    Cut nvarchar(50) not null,
    CaratWeight nvarchar(50) not null,
    Price money default 0 not null,
    Quantity int default 0 not null,
    WarrantyPeriod int default 0 not null,	--count as month (thoi han bao hanh)
    [LastUpdate] datetime not null default CURRENT_TIMESTAMP,	--for manage history
    [Status] nvarchar(20) not null default 'available',	--available   |   out-of-stock   |   deleted

    CertificateId uniqueidentifier unique not null foreign key references [Certificate](Id)	--1 diamond - 1 certificate
);
go
create table [Product]
(
    Id uniqueidentifier default newid() primary key,
    [Name] nvarchar(50) not null,
    Material nvarchar(100),
    Gender bit,
    Price money not null,
    Point int default 0 not null,
    Quantity int default 0 not null,
    WarrantyPeriod int default 0 not null,	--count as month (thoi han bao hanh)
    [LastUpdate] datetime default CURRENT_TIMESTAMP,
    [Status] nvarchar(20) not null default 'available',	--available   |   out-of-stock   |   deleted
    DiamondId uniqueidentifier unique not null foreign key references [Diamond](Id),
    CategoryId uniqueidentifier not null foreign key references [Category](Id)
);
go
create table [Picture]
(
    Id uniqueidentifier default newid() primary key,
    UrlPath nvarchar(max),

    DiamondId uniqueidentifier foreign key references [Diamond](Id),
    ProductId uniqueidentifier foreign key references [Product](Id),
    constraint CHK_PictureOf_ForeignKey check (
        (ProductId IS NOT NULL AND DiamondId IS NULL)
            OR (ProductId IS NULL AND DiamondId IS NOT NULL)
        )
);
create table [Order]
(
    Id uniqueidentifier default newid() primary key,
    PayMethod nvarchar(20),
    [Status] [nvarchar](255) NOT NULL,
    [CreatedDate] [date] NOT NULL,
    [TotalPrice] [bigint] NOT NULL,
    [Name] nvarchar(255),
    [Address] nvarchar(255),
    CustomerId uniqueidentifier not null foreign key references [Account](Id),
    SalesStaffId uniqueidentifier null foreign key references [Account](Id),
    DeliveryStaffId uniqueidentifier null foreign key references [Account](Id)
);
create table [OrderDetail]
(
    Id uniqueidentifier default newid() primary key,
    OrderId uniqueidentifier not null foreign key references [Order](Id),
    ProductId uniqueidentifier foreign key references [Product](Id),
    Quantity int default 0 not null,
    [Price] [decimal](8, 2) NOT NULL,
);
create table [Promotion]
(
    Id uniqueidentifier default newid() primary key,
    [Name] nvarchar(100) not null,
    [Description] nvarchar(max),
    ExpiredDate datetime not null,
    DiscountPercent int not null,		--int%
    [Status] nvarchar(20) not null default 'available',	--expired   |   deleted
);
go