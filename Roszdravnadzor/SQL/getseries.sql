create function [dbo].[get_series] (@par integer)
returns  VARCHAR(256)
 as
begin 
DECLARE @NAME VARCHAR(256)
DECLARE @NAME2 VARCHAR(256)
DECLARE cur CURSOR FOR 
	select series_number from series s,lot lot where s.id_series=lot.id_series and lot.id_goods=@par and quantity_rem not in(0)

set @NAME=''
set @NAME2=''
OPEN cur
WHILE 1 = 1 BEGIN
	FETCH NEXT FROM cur INTO @NAME
IF @@FETCH_STATUS <> 0 BREAK
/*IF @@fetch_status=-1 
      BREAK
  IF @@fetch_status=-2
*/
select  @NAME2=@NAME2+@NAME+','
END
CLOSE cur 
DEALLOCATE cur
return @NAME2
end   
