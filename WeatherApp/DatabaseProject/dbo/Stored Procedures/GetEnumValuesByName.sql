-- =============================================
CREATE PROCEDURE [dbo].[GetEnumValuesByName] 
	@enumerationId int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT EV.[id]
      ,EV.[enumeration_id]
	  ,(SELECT name from [dbo].[Enumeration] WHERE id = @enumerationId) as enumeration_name
      ,EV.[value]
      ,EV.[text]
  FROM [demoDatabase].[dbo].[Enumeration_Value] EV
  WHERE EV.[enumeration_id] = @enumerationId
END
