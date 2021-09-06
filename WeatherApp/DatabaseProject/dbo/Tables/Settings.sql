CREATE TABLE [dbo].[Settings] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [UserID]   INT            NOT NULL,
    [OptionID] INT            NOT NULL,
    [Value]    NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([OptionID]) REFERENCES [dbo].[Options] ([ID]),
    FOREIGN KEY ([OptionID]) REFERENCES [dbo].[Options] ([ID]),
    CONSTRAINT [FK__Settings__UserID__5070F446] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([ID]) ON DELETE CASCADE
);

