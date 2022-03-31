﻿-- DrugID;Segment;Year;Month;Series;TotDrugQn;MnfPrice;PrcPrice;RtlPrice;Funds;VendorID;Remark;SrcO
select 
DrugID,
Segment = convert(varchar,1), --                                         +';'+   --Segment
Year = dbo.formatdatetime('yyyy',getdate()), --                       +';'+   --Year
Month = dbo.formatdatetime('mm',getdate()-6), --                         +';'+   --Month
Series = series_number,   --Series
TotDrugQn= remain ,      --                +';'+   --TotDrugOn
MnfPrice = PRICE_PROD*1.1, --                   +';'+   --MnfPrice
PrcPrice = PRICE_SUP, --                        +';'+   --PrcPrice
RtlPrice = PRICE_Sal, --                        +';'+   --RtlPrice
Funds = '0.00' , --                                                     +';'+   --Funds
VendorID,
Remark = '', --                                                         +';'+   --Remark
SrcOrg =   ''                                                                 --SrcOrg

--v_remains_roszdrav.* 
from v_remains_roszdrav , goods
where v_remains_roszdrav.id_goods = goods.id_goods
and  price_prod <= dbo.FN_ACTUAL_REGISTER_PRICE_GOODS(id_goods_global, GETDATE())
and PRICE_prod <= (select min(d2a_zhnvls_roszdrav.MaxMnfPrice) from d2a_zhnvls_roszdrav where d2a_zhnvls_roszdrav.DrugID = v_remains_roszdrav.drugid)