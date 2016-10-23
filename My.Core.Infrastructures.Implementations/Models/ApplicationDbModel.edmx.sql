
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/19/2016 23:21:21
-- Generated from EDMX file: C:\Users\ediux\Documents\專案文件\自己的\1_SourceCode\OpenCoreWeb\My.Core.Infrastructures.Implementations\Models\ApplicationDbModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [OpenWebSite];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ApplicationGroupTree_ToApplicationGroup_Child]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApplicationGroupTree] DROP CONSTRAINT [FK_ApplicationGroupTree_ToApplicationGroup_Child];
GO
IF OBJECT_ID(N'[dbo].[FK_ApplicationGroupTree_ToApplicationGroup_Current]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApplicationGroupTree] DROP CONSTRAINT [FK_ApplicationGroupTree_ToApplicationGroup_Current];
GO
IF OBJECT_ID(N'[dbo].[FK_ApplicationGroupTree_ToApplicationGroup_Parent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApplicationGroupTree] DROP CONSTRAINT [FK_ApplicationGroupTree_ToApplicationGroup_Parent];
GO
IF OBJECT_ID(N'[dbo].[FK_ApplicationUserClaim_ToApplicationUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApplicationUserClaim] DROP CONSTRAINT [FK_ApplicationUserClaim_ToApplicationUser];
GO
IF OBJECT_ID(N'[dbo].[FK_ApplicationUserGroup_ToApplicationGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApplicationUserGroup] DROP CONSTRAINT [FK_ApplicationUserGroup_ToApplicationGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_ApplicationUserGroup_ToApplicationUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApplicationUserGroup] DROP CONSTRAINT [FK_ApplicationUserGroup_ToApplicationUser];
GO
IF OBJECT_ID(N'[dbo].[FK_ApplicationUserLogin_ToApplicationUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApplicationUserLogin] DROP CONSTRAINT [FK_ApplicationUserLogin_ToApplicationUser];
GO
IF OBJECT_ID(N'[dbo].[FK_ApplicationUserProfileRef_ToApplicationUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApplicationUserProfileRef] DROP CONSTRAINT [FK_ApplicationUserProfileRef_ToApplicationUser];
GO
IF OBJECT_ID(N'[dbo].[FK_ApplicationUserProfileRef_ToApplicationUserProfile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApplicationUserProfileRef] DROP CONSTRAINT [FK_ApplicationUserProfileRef_ToApplicationUserProfile];
GO
IF OBJECT_ID(N'[dbo].[FK_ApplicationUserRole_ToApplicationRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApplicationUserRole] DROP CONSTRAINT [FK_ApplicationUserRole_ToApplicationRole];
GO
IF OBJECT_ID(N'[dbo].[FK_ApplicationUserRole_ToApplicationUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApplicationUserRole] DROP CONSTRAINT [FK_ApplicationUserRole_ToApplicationUser];
GO
IF OBJECT_ID(N'[dbo].[FK_UserOperationLog_ToApplicationUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserOperationLog] DROP CONSTRAINT [FK_UserOperationLog_ToApplicationUser];
GO
IF OBJECT_ID(N'[dbo].[FK_UserOperationLog_ToUserOperationCodeDefine]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserOperationLog] DROP CONSTRAINT [FK_UserOperationLog_ToUserOperationCodeDefine];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ApplicationGroup]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApplicationGroup];
GO
IF OBJECT_ID(N'[dbo].[ApplicationGroupTree]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApplicationGroupTree];
GO
IF OBJECT_ID(N'[dbo].[ApplicationRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApplicationRole];
GO
IF OBJECT_ID(N'[dbo].[ApplicationUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApplicationUser];
GO
IF OBJECT_ID(N'[dbo].[ApplicationUserClaim]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApplicationUserClaim];
GO
IF OBJECT_ID(N'[dbo].[ApplicationUserGroup]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApplicationUserGroup];
GO
IF OBJECT_ID(N'[dbo].[ApplicationUserLogin]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApplicationUserLogin];
GO
IF OBJECT_ID(N'[dbo].[ApplicationUserProfile]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApplicationUserProfile];
GO
IF OBJECT_ID(N'[dbo].[ApplicationUserProfileRef]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApplicationUserProfileRef];
GO
IF OBJECT_ID(N'[dbo].[ApplicationUserRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApplicationUserRole];
GO
IF OBJECT_ID(N'[dbo].[UserOperationCodeDefine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserOperationCodeDefine];
GO
IF OBJECT_ID(N'[dbo].[UserOperationLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserOperationLog];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ApplicationGroup'
CREATE TABLE [dbo].[ApplicationGroup] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Void] bit  NOT NULL,
    [CreateUserId] int  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [LastUpdateUserId] int  NOT NULL,
    [LastUpdateTime] datetime  NOT NULL
);
GO

-- Creating table 'ApplicationGroupTree'
CREATE TABLE [dbo].[ApplicationGroupTree] (
    [Id] int  NOT NULL,
    [ParentId] int  NOT NULL,
    [ChildId] int  NOT NULL,
    [Level] int  NOT NULL,
    [Void] bit  NOT NULL
);
GO

-- Creating table 'ApplicationRole'
CREATE TABLE [dbo].[ApplicationRole] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [CreateUserId] int  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [LastUpdateUserId] int  NOT NULL,
    [LastUpdateTime] datetime  NOT NULL
);
GO

-- Creating table 'ApplicationUser'
CREATE TABLE [dbo].[ApplicationUser] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(50)  NOT NULL,
    [Password] nvarchar(50)  NULL,
    [PasswordHash] nvarchar(max)  NULL,
    [SecurityStamp] nvarchar(max)  NULL,
    [TwoFactorEnabled] bit  NOT NULL,
    [Void] bit  NOT NULL,
    [CreateUserId] int  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [LastUpdateUserId] int  NOT NULL,
    [LastUpdateTime] datetime  NOT NULL,
    [LastActivityTime] datetime  NULL,
    [LastUnlockedTime] datetime  NULL,
    [LastLoginFailTime] datetime  NULL,
    [AccessFailedCount] int  NOT NULL,
    [LockoutEnabled] bit  NULL,
    [LockoutEndDate] datetime  NULL,
    [ResetPasswordToken] nvarchar(512)  NULL
);
GO

-- Creating table 'ApplicationUserGroup'
CREATE TABLE [dbo].[ApplicationUserGroup] (
    [UserId] int  NOT NULL,
    [GroupId] int  NOT NULL,
    [Void] bit  NOT NULL
);
GO

-- Creating table 'ApplicationUserProfile'
CREATE TABLE [dbo].[ApplicationUserProfile] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Address] nvarchar(max)  NULL,
    [EMail] nvarchar(512)  NOT NULL,
    [EMailConfirmed] bit  NOT NULL,
    [PhoneNumber] nvarchar(10)  NOT NULL,
    [PhoneConfirmed] bit  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [LastUpdateTime] datetime  NOT NULL,
    [DisplayName] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'ApplicationUserProfileRef'
CREATE TABLE [dbo].[ApplicationUserProfileRef] (
    [UserId] int  NOT NULL,
    [ProfileId] int  NOT NULL,
    [Void] bit  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [LastUpdateTime] datetime  NOT NULL
);
GO

-- Creating table 'ApplicationUserRole'
CREATE TABLE [dbo].[ApplicationUserRole] (
    [UserId] int  NOT NULL,
    [RoleId] int  NOT NULL,
    [Void] bit  NOT NULL
);
GO

-- Creating table 'UserOperationCodeDefine'
CREATE TABLE [dbo].[UserOperationCodeDefine] (
    [OpreationCode] int  NOT NULL,
    [Description] nvarchar(512)  NOT NULL,
    [MessageResourceKey] nvarchar(256)  NULL
);
GO

-- Creating table 'UserOperationLog'
CREATE TABLE [dbo].[UserOperationLog] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Body] nvarchar(max)  NULL,
    [LogTime] datetime  NOT NULL,
    [OpreationCode] int  NOT NULL,
    [URL] nvarchar(max)  NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'ApplicationUserClaim'
CREATE TABLE [dbo].[ApplicationUserClaim] (
    [UserId] int  NOT NULL,
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClaimType] nvarchar(256)  NULL,
    [ClaimValue] nvarchar(max)  NULL
);
GO

-- Creating table 'ApplicationUserLogin'
CREATE TABLE [dbo].[ApplicationUserLogin] (
    [UserId] int  NOT NULL,
    [LoginProvider] nvarchar(512)  NOT NULL,
    [ProviderKey] nvarchar(512)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'ApplicationGroup'
ALTER TABLE [dbo].[ApplicationGroup]
ADD CONSTRAINT [PK_ApplicationGroup]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id], [ParentId], [ChildId], [Level] in table 'ApplicationGroupTree'
ALTER TABLE [dbo].[ApplicationGroupTree]
ADD CONSTRAINT [PK_ApplicationGroupTree]
    PRIMARY KEY CLUSTERED ([Id], [ParentId], [ChildId], [Level] ASC);
GO

-- Creating primary key on [Id] in table 'ApplicationRole'
ALTER TABLE [dbo].[ApplicationRole]
ADD CONSTRAINT [PK_ApplicationRole]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ApplicationUser'
ALTER TABLE [dbo].[ApplicationUser]
ADD CONSTRAINT [PK_ApplicationUser]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [UserId], [GroupId] in table 'ApplicationUserGroup'
ALTER TABLE [dbo].[ApplicationUserGroup]
ADD CONSTRAINT [PK_ApplicationUserGroup]
    PRIMARY KEY CLUSTERED ([UserId], [GroupId] ASC);
GO

-- Creating primary key on [Id] in table 'ApplicationUserProfile'
ALTER TABLE [dbo].[ApplicationUserProfile]
ADD CONSTRAINT [PK_ApplicationUserProfile]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [UserId], [ProfileId] in table 'ApplicationUserProfileRef'
ALTER TABLE [dbo].[ApplicationUserProfileRef]
ADD CONSTRAINT [PK_ApplicationUserProfileRef]
    PRIMARY KEY CLUSTERED ([UserId], [ProfileId] ASC);
GO

-- Creating primary key on [UserId], [RoleId] in table 'ApplicationUserRole'
ALTER TABLE [dbo].[ApplicationUserRole]
ADD CONSTRAINT [PK_ApplicationUserRole]
    PRIMARY KEY CLUSTERED ([UserId], [RoleId] ASC);
GO

-- Creating primary key on [OpreationCode] in table 'UserOperationCodeDefine'
ALTER TABLE [dbo].[UserOperationCodeDefine]
ADD CONSTRAINT [PK_UserOperationCodeDefine]
    PRIMARY KEY CLUSTERED ([OpreationCode] ASC);
GO

-- Creating primary key on [Id] in table 'UserOperationLog'
ALTER TABLE [dbo].[UserOperationLog]
ADD CONSTRAINT [PK_UserOperationLog]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [UserId] in table 'ApplicationUserClaim'
ALTER TABLE [dbo].[ApplicationUserClaim]
ADD CONSTRAINT [PK_ApplicationUserClaim]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [LoginProvider], [ProviderKey] in table 'ApplicationUserLogin'
ALTER TABLE [dbo].[ApplicationUserLogin]
ADD CONSTRAINT [PK_ApplicationUserLogin]
    PRIMARY KEY CLUSTERED ([LoginProvider], [ProviderKey] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Id] in table 'ApplicationGroupTree'
ALTER TABLE [dbo].[ApplicationGroupTree]
ADD CONSTRAINT [FK_ApplicationGroupTree_ToApplicationGroup_Child]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[ApplicationGroup]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'ApplicationGroupTree'
ALTER TABLE [dbo].[ApplicationGroupTree]
ADD CONSTRAINT [FK_ApplicationGroupTree_ToApplicationGroup_Current]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[ApplicationGroup]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ParentId] in table 'ApplicationGroupTree'
ALTER TABLE [dbo].[ApplicationGroupTree]
ADD CONSTRAINT [FK_ApplicationGroupTree_ToApplicationGroup_Parent]
    FOREIGN KEY ([ParentId])
    REFERENCES [dbo].[ApplicationGroup]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApplicationGroupTree_ToApplicationGroup_Parent'
CREATE INDEX [IX_FK_ApplicationGroupTree_ToApplicationGroup_Parent]
ON [dbo].[ApplicationGroupTree]
    ([ParentId]);
GO

-- Creating foreign key on [GroupId] in table 'ApplicationUserGroup'
ALTER TABLE [dbo].[ApplicationUserGroup]
ADD CONSTRAINT [FK_ApplicationUserGroup_ToApplicationGroup]
    FOREIGN KEY ([GroupId])
    REFERENCES [dbo].[ApplicationGroup]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApplicationUserGroup_ToApplicationGroup'
CREATE INDEX [IX_FK_ApplicationUserGroup_ToApplicationGroup]
ON [dbo].[ApplicationUserGroup]
    ([GroupId]);
GO

-- Creating foreign key on [RoleId] in table 'ApplicationUserRole'
ALTER TABLE [dbo].[ApplicationUserRole]
ADD CONSTRAINT [FK_ApplicationUserRole_ToApplicationRole]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[ApplicationRole]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApplicationUserRole_ToApplicationRole'
CREATE INDEX [IX_FK_ApplicationUserRole_ToApplicationRole]
ON [dbo].[ApplicationUserRole]
    ([RoleId]);
GO

-- Creating foreign key on [UserId] in table 'ApplicationUserGroup'
ALTER TABLE [dbo].[ApplicationUserGroup]
ADD CONSTRAINT [FK_ApplicationUserGroup_ToApplicationUser]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[ApplicationUser]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserId] in table 'ApplicationUserProfileRef'
ALTER TABLE [dbo].[ApplicationUserProfileRef]
ADD CONSTRAINT [FK_ApplicationUserProfileRef_ToApplicationUser]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[ApplicationUser]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserId] in table 'ApplicationUserRole'
ALTER TABLE [dbo].[ApplicationUserRole]
ADD CONSTRAINT [FK_ApplicationUserRole_ToApplicationUser]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[ApplicationUser]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ProfileId] in table 'ApplicationUserProfileRef'
ALTER TABLE [dbo].[ApplicationUserProfileRef]
ADD CONSTRAINT [FK_ApplicationUserProfileRef_ToApplicationUserProfile]
    FOREIGN KEY ([ProfileId])
    REFERENCES [dbo].[ApplicationUserProfile]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApplicationUserProfileRef_ToApplicationUserProfile'
CREATE INDEX [IX_FK_ApplicationUserProfileRef_ToApplicationUserProfile]
ON [dbo].[ApplicationUserProfileRef]
    ([ProfileId]);
GO

-- Creating foreign key on [UserId] in table 'UserOperationLog'
ALTER TABLE [dbo].[UserOperationLog]
ADD CONSTRAINT [FK_UserOperationLog_ToApplicationUser]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[ApplicationUser]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserOperationLog_ToApplicationUser'
CREATE INDEX [IX_FK_UserOperationLog_ToApplicationUser]
ON [dbo].[UserOperationLog]
    ([UserId]);
GO

-- Creating foreign key on [OpreationCode] in table 'UserOperationLog'
ALTER TABLE [dbo].[UserOperationLog]
ADD CONSTRAINT [FK_UserOperationLog_ToUserOperationCodeDefine]
    FOREIGN KEY ([OpreationCode])
    REFERENCES [dbo].[UserOperationCodeDefine]
        ([OpreationCode])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserOperationLog_ToUserOperationCodeDefine'
CREATE INDEX [IX_FK_UserOperationLog_ToUserOperationCodeDefine]
ON [dbo].[UserOperationLog]
    ([OpreationCode]);
GO

-- Creating foreign key on [UserId] in table 'ApplicationUserClaim'
ALTER TABLE [dbo].[ApplicationUserClaim]
ADD CONSTRAINT [FK_ApplicationUserClaim_ToApplicationUser]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[ApplicationUser]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserId] in table 'ApplicationUserLogin'
ALTER TABLE [dbo].[ApplicationUserLogin]
ADD CONSTRAINT [FK_ApplicationUserLogin_ToApplicationUser]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[ApplicationUser]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApplicationUserLogin_ToApplicationUser'
CREATE INDEX [IX_FK_ApplicationUserLogin_ToApplicationUser]
ON [dbo].[ApplicationUserLogin]
    ([UserId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------