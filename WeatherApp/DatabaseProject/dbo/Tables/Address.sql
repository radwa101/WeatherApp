CREATE TABLE [dbo].[Address] (
    [addressId]    INT           IDENTITY (1, 1) NOT NULL,
    [addressLine1] VARCHAR (100) NULL,
    [addressLine2] VARCHAR (100) NULL,
    [county]       VARCHAR (50)  NULL,
    [purchaseDate] VARCHAR (50)  NULL
);

