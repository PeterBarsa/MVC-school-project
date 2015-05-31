
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/31/2015 11:36:45
-- Generated from EDMX file: C:\Users\Peter\Documents\GitHub\MVC-school-project\DatingSite_Projekt\Dating_data\Repository\MainDBContext.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MainDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Descriptions'
CREATE TABLE [dbo].[Descriptions] (
    [DescriptionId] int  NOT NULL,
    [Description1] nvarchar(50)  NULL,
    [UserId] int  NOT NULL,
    [Email] varchar(50)  NULL,
    [City] varchar(20)  NULL,
    [Country] varchar(20)  NULL,
    [AboutMe] nvarchar(255)  NULL,
    [Age] int  NULL
);
GO

-- Creating table 'Friends'
CREATE TABLE [dbo].[Friends] (
    [User1] int  NOT NULL,
    [User2] int  NOT NULL,
    [FriendId] int  NOT NULL,
    [status] bit  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int  NOT NULL,
    [Username] nvarchar(25)  NOT NULL,
    [Password] nvarchar(25)  NOT NULL,
    [Searchable] bit  NOT NULL
);
GO

-- Creating table 'Messages'
CREATE TABLE [dbo].[Messages] (
    [MessageId] int  NOT NULL,
    [SenderId] int  NOT NULL,
    [ReceiverId] int  NOT NULL,
    [Messages] nvarchar(255)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [DescriptionId] in table 'Descriptions'
ALTER TABLE [dbo].[Descriptions]
ADD CONSTRAINT [PK_Descriptions]
    PRIMARY KEY CLUSTERED ([DescriptionId] ASC);
GO

-- Creating primary key on [FriendId] in table 'Friends'
ALTER TABLE [dbo].[Friends]
ADD CONSTRAINT [PK_Friends]
    PRIMARY KEY CLUSTERED ([FriendId] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [MessageId] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [PK_Messages]
    PRIMARY KEY CLUSTERED ([MessageId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'Descriptions'
ALTER TABLE [dbo].[Descriptions]
ADD CONSTRAINT [FK__Descripti__UserI__0697FACD]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Descripti__UserI__0697FACD'
CREATE INDEX [IX_FK__Descripti__UserI__0697FACD]
ON [dbo].[Descriptions]
    ([UserId]);
GO

-- Creating foreign key on [User1] in table 'Friends'
ALTER TABLE [dbo].[Friends]
ADD CONSTRAINT [FK_User1]
    FOREIGN KEY ([User1])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_User1'
CREATE INDEX [IX_FK_User1]
ON [dbo].[Friends]
    ([User1]);
GO

-- Creating foreign key on [User2] in table 'Friends'
ALTER TABLE [dbo].[Friends]
ADD CONSTRAINT [FK_User2]
    FOREIGN KEY ([User2])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_User2'
CREATE INDEX [IX_FK_User2]
ON [dbo].[Friends]
    ([User2]);
GO

-- Creating foreign key on [ReceiverId] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [FK_ReceiverId]
    FOREIGN KEY ([ReceiverId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReceiverId'
CREATE INDEX [IX_FK_ReceiverId]
ON [dbo].[Messages]
    ([ReceiverId]);
GO

-- Creating foreign key on [SenderId] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [FK_SenderId]
    FOREIGN KEY ([SenderId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SenderId'
CREATE INDEX [IX_FK_SenderId]
ON [dbo].[Messages]
    ([SenderId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------