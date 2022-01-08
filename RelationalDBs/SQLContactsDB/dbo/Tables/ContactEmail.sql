CREATE TABLE [dbo].[ContactEmail]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ContactID] INT NOT NULL, 
    [EmailAddressId] INT NOT NULL
)
