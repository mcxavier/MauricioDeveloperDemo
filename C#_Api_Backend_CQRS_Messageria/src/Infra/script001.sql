IF SCHEMA_ID(N'marketing') IS NULL EXEC(N'CREATE SCHEMA [marketing];');

GO

CREATE TABLE [marketing].[CustomerNotification] (
    [Id] uniqueidentifier NOT NULL,
    [StoreCode] nvarchar(25) NULL,
    [StockKeepingUnit] nvarchar(40) NULL,
    [CustomerName] nvarchar(max) NULL,
    [CustomerEmail] nvarchar(max) NULL,
    [Notified] bit NOT NULL,
    [NotifiedAt] datetime2 NULL,
    CONSTRAINT [PK_CustomerNotification] PRIMARY KEY ([Id])
);

GO

CREATE INDEX [IX_CustomerNotification_StoreCode] ON [marketing].[CustomerNotification] ([StoreCode]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201218165200_v0010', N'3.1.5');

GO

