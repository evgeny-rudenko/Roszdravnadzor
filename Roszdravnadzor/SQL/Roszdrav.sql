
select x.data--+convert(varchar, (ROW_NUMBER() OVER(ORDER BY data DESC))) 
as data from 
(
--select 'DrugID;PackNx;MnfID;PckID;Segment;Year;Month;IRECID;Series;Quantity;Funds;VendorID;MnfPrice;PrcPrice;RtlPrice;Remark;SrcOrg'
--DrugID;Segment;Year;Month;Series;TotDrugOn;MnfPrice;PrcPrice;RtlPrice;Funds;VendorID;Remark;SrcOrg
SELECT 
--(select top(1) UniqNx from d2a_temp_zhnvls where replace(barcode,' ','') in (select code from bar_code where id_goods=goo.id_goods)) +';'+   --PackNx
(select top(1) DrugID from d2a_zhnvls_roszdrav where replace(EAN,' ','') in (select code from bar_code where id_goods=goo.id_goods) and EAN<>'~') +';'+   --DrugID
convert(varchar,1)                                         +';'+   --Segment
dbo.formatdatetime('yyyy',getdate())                       +';'+   --Year
dbo.formatdatetime('mm',getdate()-6)                         +';'+   --Month
isnull(dbo.get_series(goo.id_goods),'-')                   +';'+   --Series
convert(varchar,sum (OST.AMOUNT_OST))                      +';'+   --TotDrugOn
convert(varchar,max(lot.PRICE_PROD)*1.1)                   +';'+   --MnfPrice
convert(varchar,max(lot.PRICE_SUP))                        +';'+   --PrcPrice
convert(varchar,max(lot.PRICE_Sal))                        +';'+   --RtlPrice
'0.00'                                                     +';'+   --Funds
convert(varchar, 
(select id_contractor from d2a_roszdrav_contractor
where id_contractor_efarma=lot.id_supplier))               +';'+   --VendorID
''                                                         +';'+   --Remark
''                                                                 --SrcOrg

                                              as  data

FROM 
(SELECT LM.ID_GOODS, LM.ID_STORE,
	LM.ID_LOT_GLOBAL,
	AMOUNT_OST = SUM(CASE WHEN DATE_OP <= convert(datetime,getdate(),104) THEN LM.QUANTITY_REM ELSE 0 END),
	LM.PRICE_SUP,
	LM.PRICE_SAL	

FROM  MV_LOT_MOVEMENT LM 
WHERE ( LM.ID_GOODS IN (SELECT ID_GOODS FROM GOODS)) AND
      ( LM.ID_STORE IN (SELECT ID_STORE FROM STORE))
GROUP BY LM.ID_GOODS, LM.ID_STORE, LM.PRICE_SUP, LM.PRICE_SAL, LM.ID_LOT_GLOBAL)
 OST
	INNER JOIN LOT LOT ON LOT.ID_LOT_GLOBAL = OST.ID_LOT_GLOBAL
 	INNER JOIN DBO.GOODS goo ON Goo.ID_GOODS = OST.ID_GOODS
	LEFT JOIN MV_DOCUMENTS MV ON MV.ID_DOCUMENT_GLOBAL = LOT.ID_DOCUMENT
	INNER JOIN DBO.CONTRACTOR C ON C.ID_CONTRACTOR= LOT.ID_SUPPLIER
	LEFT JOIN DBO.PRODUCER P ON P.ID_PRODUCER = Goo.ID_PRODUCER
	LEFT OUTER JOIN DBO.COUNTRY CNR ON P.ID_COUNTRY = CNR.ID_COUNTRY
WHERE AMOUNT_OST > 0
and convert(datetime,lot.incoming_date,104)>=convert(datetime,'01.04.2010',104)
and convert(datetime,lot.incoming_date,104) between convert(datetime,'15.'+dbo.formatdatetime('mm',getdate()-30)+'.'+dbo.formatdatetime('yyyy',getdate()-30),104) and convert(datetime,'15.'+dbo.formatdatetime('mm',getdate())+'.'+dbo.formatdatetime('yyyy',getdate()),104)

and lot.price_sup<>lot.price_sal
and lot.id_supplier in (select id_contractor_efarma from d2a_roszdrav_contractor)
and lot.id_scaling_ratio in (select id_scaling_ratio from scaling_ratio where id_unit=87)
--and goo.id_goods in (select distinct id_goods from goods_code where id_contractor in (select id_contractor from contractor where upper(name) not like upper('%Росздравнадзор%')))
and goo.id_goods in (select id_goods from bar_code where code in (select replace(EAN,' ','') from d2a_zhnvls_roszdrav where EAN not in ('','~')))
group by goo.id_goods,lot.id_supplier
) x

