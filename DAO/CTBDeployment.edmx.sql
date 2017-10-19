
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 10/19/2017 10:02:13
-- Generated from EDMX file: D:\MyWork\Gits\CTBDeployment\DAO\CTBDeployment.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [CTBDeployment];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[D_Project]', 'U') IS NOT NULL
    DROP TABLE [dbo].[D_Project];
GO
IF OBJECT_ID(N'[dbo].[D_ProjectCodeServer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[D_ProjectCodeServer];
GO
IF OBJECT_ID(N'[dbo].[D_ProjectPackage]', 'U') IS NOT NULL
    DROP TABLE [dbo].[D_ProjectPackage];
GO
IF OBJECT_ID(N'[dbo].[D_SqlServer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[D_SqlServer];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'D_Project'
CREATE TABLE [dbo].[D_Project] (
    [ProjectID] varchar(50)  NOT NULL,
    [ProjectName] nvarchar(50)  NOT NULL,
    [ProjectCode] nvarchar(50)  NOT NULL,
    [Remark] nvarchar(250)  NULL,
    [CreateTime] datetime  NOT NULL,
    [CreateUser] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'D_ProjectCodeServer'
CREATE TABLE [dbo].[D_ProjectCodeServer] (
    [ServerID] varchar(50)  NOT NULL,
    [ServerIndex] int  NOT NULL,
    [DeploymentType] int  NOT NULL,
    [ProjectID] nvarchar(50)  NOT NULL,
    [Path] nvarchar(250)  NULL,
    [FtpLoginUser] nvarchar(150)  NULL,
    [FtpLoginPwd] nvarchar(50)  NULL,
    [Remark] nvarchar(250)  NULL,
    [CreateTime] datetime  NOT NULL,
    [CreateUser] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'D_ProjectPackage'
CREATE TABLE [dbo].[D_ProjectPackage] (
    [PackageID] varchar(50)  NOT NULL,
    [ProjectID] varchar(50)  NOT NULL,
    [Version] nchar(50)  NOT NULL,
    [PackageName] nvarchar(50)  NOT NULL,
    [PackagePath] nvarchar(250)  NOT NULL,
    [Status] int  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [CreateUser] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'D_SqlServer'
CREATE TABLE [dbo].[D_SqlServer] (
    [ServerID] nvarchar(50)  NOT NULL,
    [ProjectID] varchar(50)  NOT NULL,
    [ConnectionStr] nvarchar(150)  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [CreateUser] nvarchar(50)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ProjectID] in table 'D_Project'
ALTER TABLE [dbo].[D_Project]
ADD CONSTRAINT [PK_D_Project]
    PRIMARY KEY CLUSTERED ([ProjectID] ASC);
GO

-- Creating primary key on [ServerID] in table 'D_ProjectCodeServer'
ALTER TABLE [dbo].[D_ProjectCodeServer]
ADD CONSTRAINT [PK_D_ProjectCodeServer]
    PRIMARY KEY CLUSTERED ([ServerID] ASC);
GO

-- Creating primary key on [PackageID] in table 'D_ProjectPackage'
ALTER TABLE [dbo].[D_ProjectPackage]
ADD CONSTRAINT [PK_D_ProjectPackage]
    PRIMARY KEY CLUSTERED ([PackageID] ASC);
GO

-- Creating primary key on [ServerID] in table 'D_SqlServer'
ALTER TABLE [dbo].[D_SqlServer]
ADD CONSTRAINT [PK_D_SqlServer]
    PRIMARY KEY CLUSTERED ([ServerID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------