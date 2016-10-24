
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/23/2016 23:09:48
-- Generated from EDMX file: C:\Users\ediux\Documents\GitHub\CRM\CRM\Models\CRM.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [C:\USERS\EDIUX\DOCUMENTS\GITHUB\CRM\CRM\APP_DATA\客戶資料.MDF];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_客戶銀行資訊_客戶資料]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[客戶銀行資訊] DROP CONSTRAINT [FK_客戶銀行資訊_客戶資料];
GO
IF OBJECT_ID(N'[dbo].[FK_客戶聯絡人_客戶資料]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[客戶聯絡人] DROP CONSTRAINT [FK_客戶聯絡人_客戶資料];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[客戶資料]', 'U') IS NOT NULL
    DROP TABLE [dbo].[客戶資料];
GO
IF OBJECT_ID(N'[dbo].[客戶銀行資訊]', 'U') IS NOT NULL
    DROP TABLE [dbo].[客戶銀行資訊];
GO
IF OBJECT_ID(N'[dbo].[客戶聯絡人]', 'U') IS NOT NULL
    DROP TABLE [dbo].[客戶聯絡人];
GO
IF OBJECT_ID(N'[dbo].[vw_CustomerSummary]', 'V') IS NOT NULL
    DROP VIEW [dbo].[vw_CustomerSummary];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table '客戶資料'
CREATE TABLE [dbo].[客戶資料] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [客戶名稱] nvarchar(50)  NOT NULL,
    [統一編號] char(8)  NOT NULL,
    [電話] nvarchar(50)  NOT NULL,
    [傳真] nvarchar(50)  NULL,
    [地址] nvarchar(100)  NULL,
    [Email] nvarchar(250)  NULL,
    [Void] bit  NOT NULL
);
GO

-- Creating table '客戶銀行資訊'
CREATE TABLE [dbo].[客戶銀行資訊] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [客戶Id] int  NOT NULL,
    [銀行名稱] nvarchar(50)  NOT NULL,
    [銀行代碼] int  NOT NULL,
    [分行代碼] int  NULL,
    [帳戶名稱] nvarchar(50)  NOT NULL,
    [帳戶號碼] nvarchar(20)  NOT NULL,
    [Void] bit  NOT NULL
);
GO

-- Creating table '客戶聯絡人'
CREATE TABLE [dbo].[客戶聯絡人] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [客戶Id] int  NOT NULL,
    [職稱] nvarchar(50)  NOT NULL,
    [姓名] nvarchar(50)  NOT NULL,
    [Email] nvarchar(250)  NOT NULL,
    [手機] nvarchar(50)  NULL,
    [電話] nvarchar(50)  NULL,
    [Void] bit  NOT NULL
);
GO

-- Creating table 'vw_CustomerSummary'
CREATE VIEW [dbo].[vw_CustomerSummary]
	AS 
	SELECT [Id],[客戶名稱],(
	Select COUNT(*)
	FROM [客戶聯絡人]
	Where [客戶Id]=[Id]
	AND [Void]=0
	) as [客戶聯絡人數量],
	(Select COUNT(*)
	FROM [客戶銀行資訊]
	WHERE [客戶Id]=[Id]
	AND [Void]=0
	)as[客戶銀行帳戶數量]
	From [客戶資料]
	WHERE [Void]=0
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table '客戶資料'
ALTER TABLE [dbo].[客戶資料]
ADD CONSTRAINT [PK_客戶資料]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table '客戶銀行資訊'
ALTER TABLE [dbo].[客戶銀行資訊]
ADD CONSTRAINT [PK_客戶銀行資訊]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table '客戶聯絡人'
ALTER TABLE [dbo].[客戶聯絡人]
ADD CONSTRAINT [PK_客戶聯絡人]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [客戶Id] in table '客戶銀行資訊'
ALTER TABLE [dbo].[客戶銀行資訊]
ADD CONSTRAINT [FK_客戶銀行資訊_客戶資料]
    FOREIGN KEY ([客戶Id])
    REFERENCES [dbo].[客戶資料]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_客戶銀行資訊_客戶資料'
CREATE INDEX [IX_FK_客戶銀行資訊_客戶資料]
ON [dbo].[客戶銀行資訊]
    ([客戶Id]);
GO

-- Creating foreign key on [客戶Id] in table '客戶聯絡人'
ALTER TABLE [dbo].[客戶聯絡人]
ADD CONSTRAINT [FK_客戶聯絡人_客戶資料]
    FOREIGN KEY ([客戶Id])
    REFERENCES [dbo].[客戶資料]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_客戶聯絡人_客戶資料'
CREATE INDEX [IX_FK_客戶聯絡人_客戶資料]
ON [dbo].[客戶聯絡人]
    ([客戶Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------