use checkout;

CREATE TABLE [dbo].[Card](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[CardNumber] [nvarchar](20) NOT NULL,
	[CardHolder] [nvarchar](100) NOT NULL,
	[ExpiryMonth] [nvarchar](2) NOT NULL,
	[ExpiryYear] [nvarchar](2) NOT NULL,
 CONSTRAINT [PK_Card] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Card] ADD  CONSTRAINT [DF_Card_ID]  DEFAULT (newid()) FOR [ID]
GO
