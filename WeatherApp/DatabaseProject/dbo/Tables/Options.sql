CREATE TABLE [dbo].[Options] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [Default]     NVARCHAR (MAX) NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

