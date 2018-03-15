
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/13/2018 21:09:13
-- Generated from EDMX file: D:\SourceCode\CSharp\MyCard3\MyCard3\Models\MyCard.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MyCard];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_BoardArticle]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArticleSet] DROP CONSTRAINT [FK_BoardArticle];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonArticle]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArticleSet] DROP CONSTRAINT [FK_PersonArticle];
GO
IF OBJECT_ID(N'[dbo].[FK_ArticleComment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CommentSet] DROP CONSTRAINT [FK_ArticleComment];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonComment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CommentSet] DROP CONSTRAINT [FK_PersonComment];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[People]', 'U') IS NOT NULL
    DROP TABLE [dbo].[People];
GO
IF OBJECT_ID(N'[dbo].[ArticleSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArticleSet];
GO
IF OBJECT_ID(N'[dbo].[BoardSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BoardSet];
GO
IF OBJECT_ID(N'[dbo].[CommentSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CommentSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'People'
CREATE TABLE [dbo].[People] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(64)  NOT NULL,
    [Gender] bit  NOT NULL,
    [Birthday] nvarchar(max)  NOT NULL,
    [authenticationId] nvarchar(128)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Picture] varbinary(max)  NULL
);
GO

-- Creating table 'ArticleSet'
CREATE TABLE [dbo].[ArticleSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(64)  NOT NULL,
    [Time] datetime  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [BoardId] int  NOT NULL,
    [PersonId] int  NOT NULL
);
GO

-- Creating table 'BoardSet'
CREATE TABLE [dbo].[BoardSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(64)  NOT NULL
);
GO

-- Creating table 'CommentSet'
CREATE TABLE [dbo].[CommentSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Opinion] nvarchar(max)  NULL,
    [ArticleId] int  NOT NULL,
    [PersonId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'People'
ALTER TABLE [dbo].[People]
ADD CONSTRAINT [PK_People]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ArticleSet'
ALTER TABLE [dbo].[ArticleSet]
ADD CONSTRAINT [PK_ArticleSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BoardSet'
ALTER TABLE [dbo].[BoardSet]
ADD CONSTRAINT [PK_BoardSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CommentSet'
ALTER TABLE [dbo].[CommentSet]
ADD CONSTRAINT [PK_CommentSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [BoardId] in table 'ArticleSet'
ALTER TABLE [dbo].[ArticleSet]
ADD CONSTRAINT [FK_BoardArticle]
    FOREIGN KEY ([BoardId])
    REFERENCES [dbo].[BoardSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BoardArticle'
CREATE INDEX [IX_FK_BoardArticle]
ON [dbo].[ArticleSet]
    ([BoardId]);
GO

-- Creating foreign key on [PersonId] in table 'ArticleSet'
ALTER TABLE [dbo].[ArticleSet]
ADD CONSTRAINT [FK_PersonArticle]
    FOREIGN KEY ([PersonId])
    REFERENCES [dbo].[People]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonArticle'
CREATE INDEX [IX_FK_PersonArticle]
ON [dbo].[ArticleSet]
    ([PersonId]);
GO

-- Creating foreign key on [ArticleId] in table 'CommentSet'
ALTER TABLE [dbo].[CommentSet]
ADD CONSTRAINT [FK_ArticleComment]
    FOREIGN KEY ([ArticleId])
    REFERENCES [dbo].[ArticleSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ArticleComment'
CREATE INDEX [IX_FK_ArticleComment]
ON [dbo].[CommentSet]
    ([ArticleId]);
GO

-- Creating foreign key on [PersonId] in table 'CommentSet'
ALTER TABLE [dbo].[CommentSet]
ADD CONSTRAINT [FK_PersonComment]
    FOREIGN KEY ([PersonId])
    REFERENCES [dbo].[People]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonComment'
CREATE INDEX [IX_FK_PersonComment]
ON [dbo].[CommentSet]
    ([PersonId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------