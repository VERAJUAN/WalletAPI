USE WalletDB;
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Wallets') AND type in (N'U'))
BEGIN
    CREATE TABLE Wallets (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        DocumentId NVARCHAR(20) NOT NULL UNIQUE,
        Name NVARCHAR(100) NOT NULL,
        Balance DECIMAL(18,2) NOT NULL DEFAULT 0,
        CreatedAt DATETIME DEFAULT GETDATE()
    );
END;
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'TypeTransaction') AND type in (N'U'))
BEGIN
    CREATE TABLE TypeTransaction (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(50) NOT NULL UNIQUE
    );
END;
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Transactions') AND type in (N'U'))
BEGIN
    CREATE TABLE Transactions (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        WalletId INT NOT NULL,
        Amount DECIMAL(18,2) NOT NULL CHECK (Amount > 0),
        TypeId INT NOT NULL,
        CreatedAt DATETIME DEFAULT GETDATE(),

        CONSTRAINT FK_Transactions_Wallet FOREIGN KEY (WalletId) REFERENCES Wallets(Id) ON DELETE CASCADE,
        CONSTRAINT FK_Transactions_Type FOREIGN KEY (TypeId) REFERENCES TypeTransaction(Id)
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM TypeTransaction WHERE Name = 'Debito')
BEGIN
    INSERT INTO TypeTransaction (Name) VALUES ('Debito');
END;

IF NOT EXISTS (SELECT 1 FROM TypeTransaction WHERE Name = 'Credito')
BEGIN
    INSERT INTO TypeTransaction (Name) VALUES ('Credito');
END;
GO
