CREATE TABLE [dbo].[Attendee] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [Fname]       VARCHAR (50) NOT NULL,
    [Lname]       VARCHAR (50) NOT NULL,
    [CanAttend]   BIT          NULL,
    [DateOfBirth] DATETIME     NOT NULL,
    [AddressId]   INT          NULL,
    [CourseId]    INT          NULL,
    CONSTRAINT [PK_Attendees] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Attendee_Course] FOREIGN KEY ([CourseId]) REFERENCES [dbo].[Course] ([CourseId])
);

