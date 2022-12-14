CREATE TABLE [dbo].[Transactions]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
	[Amount] DECIMAL(18,2) NOT NULL,
	[CardId] NVARCHAR(100) NOT NULL,
	[TransactionDate] SMALLDATETIME NOT NULL,
	[Result] BIT NOT NULL,
	[TransactionId] AS 'TRX' + RIGHT('00000000000000000000' + CAST(ID AS VARCHAR(20)), 20) PERSISTED NOT NULL
)
