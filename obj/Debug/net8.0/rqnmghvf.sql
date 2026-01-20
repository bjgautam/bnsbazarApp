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
    WHERE [MigrationId] = N'20251219070423_Initial'
)
BEGIN
    CREATE TABLE [Advertisements] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(max) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [Category] nvarchar(max) NOT NULL,
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
    WHERE [MigrationId] = N'20251219070423_Initial'
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
    WHERE [MigrationId] = N'20251219070423_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Email', N'PasswordHash', N'Role') AND [object_id] = OBJECT_ID(N'[AppUsers]'))
        SET IDENTITY_INSERT [AppUsers] ON;
    EXEC(N'INSERT INTO [AppUsers] ([Id], [CreatedAt], [Email], [PasswordHash], [Role])
    VALUES (1, ''2025-12-19T07:04:22.9172451Z'', N''bjgautam21@gmail.com'', N''$2a$11$x5l5dnzwCQabwPgupEIREec6b8QqA3l9dRaXFJAShmB9BBsaGTFWy'', N''Admin'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Email', N'PasswordHash', N'Role') AND [object_id] = OBJECT_ID(N'[AppUsers]'))
        SET IDENTITY_INSERT [AppUsers] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219070423_Initial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251219070423_Initial', N'8.0.0');
END;
GO

COMMIT;
GO

