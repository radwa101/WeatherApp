CREATE TABLE [dbo].[Users] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [UserName] NVARCHAR (320) NOT NULL,
    CONSTRAINT [PK__Users__3214EC2789745283] PRIMARY KEY NONCLUSTERED ([ID] ASC)
);


GO
CREATE CLUSTERED INDEX [ix1]
    ON [dbo].[Users]([UserName] ASC);

