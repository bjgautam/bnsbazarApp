IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251225223254_Initial'
)
BEGIN
    CREATE TABLE [Advertisements] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(max) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [Category] nvarchar(max) NOT NULL,
        [Module] nvarchar(max) NOT NULL,
        [Location] nvarchar(max) NOT NULL,
        [PostedDate] datetime2 NOT NULL,
        [Image1Url] nvarchar(max) NULL,
        [Image2Url] nvarchar(max) NULL,
        [Image3Url] nvarchar(max) NULL,
        [Image4Url] nvarchar(max) NULL,
        [Image5Url] nvarchar(max) NULL,
        [VideoUrl] nvarchar(max) NULL,
        CONSTRAINT [PK_Advertisements] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251225223254_Initial'
)
BEGIN
    CREATE TABLE [AppUsers] (
        [Id] int NOT NULL IDENTITY,
        [Email] nvarchar(max) NOT NULL,
        [PasswordHash] nvarchar(max) NOT NULL,
        [Role] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_AppUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251225223254_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Email', N'PasswordHash', N'Role') AND [object_id] = OBJECT_ID(N'[AppUsers]'))
        SET IDENTITY_INSERT [AppUsers] ON;
    EXEC(N'INSERT INTO [AppUsers] ([Id], [CreatedAt], [Email], [PasswordHash], [Role])
    VALUES (1, ''2025-12-25T22:32:53.7809982Z'', N''bjgautam21@gmail.com'', N''$2a$11$dWXSe0PMheldfq.JojGlfOteRcY4BktKlgx0vqdKq97RPznlWXAx2'', N''Admin'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Email', N'PasswordHash', N'Role') AND [object_id] = OBJECT_ID(N'[AppUsers]'))
        SET IDENTITY_INSERT [AppUsers] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251225223254_Initial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251225223254_Initial', N'8.0.0');
END;
GO

COMMIT;
GO

