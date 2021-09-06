CREATE TABLE [dbo].[CursorTest] (
    [CursorTestID] INT            IDENTITY (1, 1) NOT NULL,
    [Filler]       VARCHAR (4000) NULL,
    [RunningTotal] BIGINT         NULL,
    PRIMARY KEY CLUSTERED ([CursorTestID] ASC)
);

