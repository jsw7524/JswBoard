
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/26/2018 23:42:52
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
IF OBJECT_ID(N'[dbo].[FK_Friends_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Friends] DROP CONSTRAINT [FK_Friends_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_Friends_Person1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Friends] DROP CONSTRAINT [FK_Friends_Person1];
GO
IF OBJECT_ID(N'[dbo].[FK_SendMessage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Messages] DROP CONSTRAINT [FK_SendMessage];
GO
IF OBJECT_ID(N'[dbo].[FK_ReceiveMessage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Messages] DROP CONSTRAINT [FK_ReceiveMessage];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonArticleThumberUp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArticleThumberUpSet] DROP CONSTRAINT [FK_PersonArticleThumberUp];
GO
IF OBJECT_ID(N'[dbo].[FK_ArticleArticleThumberUp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ArticleThumberUpSet] DROP CONSTRAINT [FK_ArticleArticleThumberUp];
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
IF OBJECT_ID(N'[dbo].[Matches]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Matches];
GO
IF OBJECT_ID(N'[dbo].[Messages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Messages];
GO
IF OBJECT_ID(N'[dbo].[ArticleThumberUpSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ArticleThumberUpSet];
GO
IF OBJECT_ID(N'[dbo].[Friends]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Friends];
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
    [PersonId] int  NOT NULL,
    [ThumbUpNumber] int  NOT NULL
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

-- Creating table 'Matches'
CREATE TABLE [dbo].[Matches] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [A_ID] int  NOT NULL,
    [B_ID] int  NOT NULL,
    [A_OK] bit  NULL,
    [B_OK] bit  NULL
);
GO

-- Creating table 'Messages'
CREATE TABLE [dbo].[Messages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SendPersonId] int  NOT NULL,
    [MessageContent] nvarchar(max)  NULL,
    [Time] datetime  NOT NULL,
    [ReceivePerson_Id] int  NOT NULL
);
GO

-- Creating table 'ArticleThumberUpSet'
CREATE TABLE [dbo].[ArticleThumberUpSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PersonId] int  NOT NULL,
    [ArticleId] int  NOT NULL
);
GO

-- Creating table 'NotificationSet'
CREATE TABLE [dbo].[NotificationSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PersonId] int  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [Time] datetime  NOT NULL
);
GO

-- Creating table 'Friends'
CREATE TABLE [dbo].[Friends] (
    [Person1_Id] int  NOT NULL,
    [Person2_Id] int  NOT NULL
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

-- Creating primary key on [Id] in table 'Matches'
ALTER TABLE [dbo].[Matches]
ADD CONSTRAINT [PK_Matches]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [PK_Messages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ArticleThumberUpSet'
ALTER TABLE [dbo].[ArticleThumberUpSet]
ADD CONSTRAINT [PK_ArticleThumberUpSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NotificationSet'
ALTER TABLE [dbo].[NotificationSet]
ADD CONSTRAINT [PK_NotificationSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Person1_Id], [Person2_Id] in table 'Friends'
ALTER TABLE [dbo].[Friends]
ADD CONSTRAINT [PK_Friends]
    PRIMARY KEY CLUSTERED ([Person1_Id], [Person2_Id] ASC);
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

-- Creating foreign key on [Person1_Id] in table 'Friends'
ALTER TABLE [dbo].[Friends]
ADD CONSTRAINT [FK_Friends_Person]
    FOREIGN KEY ([Person1_Id])
    REFERENCES [dbo].[People]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Person2_Id] in table 'Friends'
ALTER TABLE [dbo].[Friends]
ADD CONSTRAINT [FK_Friends_Person1]
    FOREIGN KEY ([Person2_Id])
    REFERENCES [dbo].[People]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Friends_Person1'
CREATE INDEX [IX_FK_Friends_Person1]
ON [dbo].[Friends]
    ([Person2_Id]);
GO

-- Creating foreign key on [SendPersonId] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [FK_SendMessage]
    FOREIGN KEY ([SendPersonId])
    REFERENCES [dbo].[People]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SendMessage'
CREATE INDEX [IX_FK_SendMessage]
ON [dbo].[Messages]
    ([SendPersonId]);
GO

-- Creating foreign key on [ReceivePerson_Id] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [FK_ReceiveMessage]
    FOREIGN KEY ([ReceivePerson_Id])
    REFERENCES [dbo].[People]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReceiveMessage'
CREATE INDEX [IX_FK_ReceiveMessage]
ON [dbo].[Messages]
    ([ReceivePerson_Id]);
GO

-- Creating foreign key on [PersonId] in table 'ArticleThumberUpSet'
ALTER TABLE [dbo].[ArticleThumberUpSet]
ADD CONSTRAINT [FK_PersonArticleThumberUp]
    FOREIGN KEY ([PersonId])
    REFERENCES [dbo].[People]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonArticleThumberUp'
CREATE INDEX [IX_FK_PersonArticleThumberUp]
ON [dbo].[ArticleThumberUpSet]
    ([PersonId]);
GO

-- Creating foreign key on [ArticleId] in table 'ArticleThumberUpSet'
ALTER TABLE [dbo].[ArticleThumberUpSet]
ADD CONSTRAINT [FK_ArticleArticleThumberUp]
    FOREIGN KEY ([ArticleId])
    REFERENCES [dbo].[ArticleSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ArticleArticleThumberUp'
CREATE INDEX [IX_FK_ArticleArticleThumberUp]
ON [dbo].[ArticleThumberUpSet]
    ([ArticleId]);
GO

-- Creating foreign key on [PersonId] in table 'NotificationSet'
ALTER TABLE [dbo].[NotificationSet]
ADD CONSTRAINT [FK_PersonNotification]
    FOREIGN KEY ([PersonId])
    REFERENCES [dbo].[People]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonNotification'
CREATE INDEX [IX_FK_PersonNotification]
ON [dbo].[NotificationSet]
    ([PersonId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------