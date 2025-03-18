IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'TransactionDb')
BEGIN
    CREATE DATABASE TransactionDb;
END;
GO

USE TransactionDb;
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='FinancialTransactions' AND xtype='U')
BEGIN
    CREATE TABLE FinancialTransactions (
        ID INT PRIMARY KEY IDENTITY(1,1),
        TransactionExternalId UNIQUEIDENTIFIER NOT NULL,
        SourceAccountId UNIQUEIDENTIFIER NOT NULL,
        TargetAccountId UNIQUEIDENTIFIER NOT NULL,
        TransferTypeId INT NOT NULL,
        Value DECIMAL(18, 2) NOT NULL,
        CreatedAt DATETIME2 NOT NULL,
        Status INT NOT NULL
    );
END;
GO