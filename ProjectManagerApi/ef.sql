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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830161803_InitialCreate'
)
BEGIN
    CREATE TABLE [Projects] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(200) NOT NULL,
        [Description] nvarchar(2000) NULL,
        [ImageUrl] nvarchar(max) NULL,
        [IsCompleted] bit NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [DueDateUtc] datetime2 NULL,
        CONSTRAINT [PK_Projects] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830161803_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250830161803_InitialCreate', N'9.0.8');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902105707_AddClientOwnerStartBudget'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250902105707_AddClientOwnerStartBudget', N'9.0.8');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902134324_UpdateProject_ClientsOwnersCurrency'
)
BEGIN
    ALTER TABLE [Projects] ADD [Budget] decimal(18,2) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902134324_UpdateProject_ClientsOwnersCurrency'
)
BEGIN
    ALTER TABLE [Projects] ADD [ClientId] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902134324_UpdateProject_ClientsOwnersCurrency'
)
BEGIN
    ALTER TABLE [Projects] ADD [ClientName] nvarchar(200) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902134324_UpdateProject_ClientsOwnersCurrency'
)
BEGIN
    ALTER TABLE [Projects] ADD [Currency] nvarchar(10) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902134324_UpdateProject_ClientsOwnersCurrency'
)
BEGIN
    ALTER TABLE [Projects] ADD [OwnerName] nvarchar(200) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902134324_UpdateProject_ClientsOwnersCurrency'
)
BEGIN
    ALTER TABLE [Projects] ADD [ProjectOwnerId] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902134324_UpdateProject_ClientsOwnersCurrency'
)
BEGIN
    ALTER TABLE [Projects] ADD [StartDateUtc] datetime2 NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902134324_UpdateProject_ClientsOwnersCurrency'
)
BEGIN
    CREATE TABLE [Clients] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(200) NOT NULL,
        CONSTRAINT [PK_Clients] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902134324_UpdateProject_ClientsOwnersCurrency'
)
BEGIN
    CREATE TABLE [ProjectOwners] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(200) NOT NULL,
        CONSTRAINT [PK_ProjectOwners] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902134324_UpdateProject_ClientsOwnersCurrency'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Clients]'))
        SET IDENTITY_INSERT [Clients] ON;
    EXEC(N'INSERT INTO [Clients] ([Id], [Name])
    VALUES (1, N''Acme AB''),
    (2, N''Globex''),
    (3, N''Initech'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Clients]'))
        SET IDENTITY_INSERT [Clients] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902134324_UpdateProject_ClientsOwnersCurrency'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[ProjectOwners]'))
        SET IDENTITY_INSERT [ProjectOwners] ON;
    EXEC(N'INSERT INTO [ProjectOwners] ([Id], [Name])
    VALUES (1, N''Natasja Kauppi''),
    (2, N''Knut Hansson''),
    (3, N''David Hemler'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[ProjectOwners]'))
        SET IDENTITY_INSERT [ProjectOwners] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902134324_UpdateProject_ClientsOwnersCurrency'
)
BEGIN
    CREATE INDEX [IX_Projects_ClientId] ON [Projects] ([ClientId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902134324_UpdateProject_ClientsOwnersCurrency'
)
BEGIN
    CREATE INDEX [IX_Projects_ProjectOwnerId] ON [Projects] ([ProjectOwnerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902134324_UpdateProject_ClientsOwnersCurrency'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Clients_Name] ON [Clients] ([Name]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902134324_UpdateProject_ClientsOwnersCurrency'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ProjectOwners_Name] ON [ProjectOwners] ([Name]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902134324_UpdateProject_ClientsOwnersCurrency'
)
BEGIN
    ALTER TABLE [Projects] ADD CONSTRAINT [FK_Projects_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902134324_UpdateProject_ClientsOwnersCurrency'
)
BEGIN
    ALTER TABLE [Projects] ADD CONSTRAINT [FK_Projects_ProjectOwners_ProjectOwnerId] FOREIGN KEY ([ProjectOwnerId]) REFERENCES [ProjectOwners] ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250902134324_UpdateProject_ClientsOwnersCurrency'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250902134324_UpdateProject_ClientsOwnersCurrency', N'9.0.8');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250904121935_RenameOwnerColumns'
)
BEGIN
    EXEC sp_rename N'[Projects].[OwnerId]', N'ProjectOwnerId', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250904121935_RenameOwnerColumns'
)
BEGIN
    EXEC sp_rename N'[Projects].[OwnerName]', N'ProjectOwnerName', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250904121935_RenameOwnerColumns'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250904121935_RenameOwnerColumns', N'9.0.8');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250906122024_SyncModel_20250906'
)
BEGIN
    EXEC sp_rename N'[Projects].[ProjectOwnerName]', N'OwnerName', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250906122024_SyncModel_20250906'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250906122024_SyncModel_20250906', N'9.0.8');
END;

COMMIT;
GO

