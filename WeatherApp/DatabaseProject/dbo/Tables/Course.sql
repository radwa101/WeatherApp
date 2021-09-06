CREATE TABLE [dbo].[Course] (
    [CourseId]    INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (100) NULL,
    [Description] VARCHAR (100) NULL,
    CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED ([CourseId] ASC),
    CONSTRAINT [FK_Course_Course] FOREIGN KEY ([CourseId]) REFERENCES [dbo].[Course] ([CourseId])
);

