CREATE PROCEDURE [dbo].[AnonymiseClientDataPerClient] @Client nvarchar(30)
AS
	BEGIN TRY
		BEGIN TRANSACTION
			SET NOCOUNT ON
			print 'Anonymising client data for client: ' + @Client
			IF OBJECT_ID('tempdb..#tmpUsers') IS NOT NULL
				DROP TABLE #tmpUsers

			SELECT username INTO #tmpUsers FROM [Users] --WHERE id between 1016 and 1020

			Declare @total int
			set @total = (Select Count(*) From #tmpUsers)
			Declare @index int
			set @index = 0
			Declare @Username varchar(50)
			While (@total > @index)
			Begin
				Select Top 1 @Username = username From #tmpUsers
				print @Username
				--Do some processing here
				Delete #tmpUsers Where username = @Username
				set @index = @index + 1
			End
			SET NOCOUNT OFF
			COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK
	END CATCH





