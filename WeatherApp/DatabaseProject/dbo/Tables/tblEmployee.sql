CREATE TABLE [dbo].[tblEmployee] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50) NULL,
    [Gender]      NVARCHAR (50) NULL,
    [DateOfBirth] DATETIME      NULL,
    [AddressId]   INT           NULL,
    [Active]      BIT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

