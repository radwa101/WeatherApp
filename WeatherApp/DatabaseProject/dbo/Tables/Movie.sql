CREATE TABLE [dbo].[Movie] (
    [id]           INT           IDENTITY (1, 1) NOT NULL,
    [title]        VARCHAR (100) NOT NULL,
    [overview]     VARCHAR (50)  NOT NULL,
    [vote_average] FLOAT (53)    NULL,
    [release_date] DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

