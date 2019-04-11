USE [EActDB-DEV]
GO
/****** Object:  StoredProcedure [dbo].[usp_getBudgetActivityProduct]    Script Date: 25/03/2019 4:21:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		surachat.j
-- Create date: 20190313
-- Description:	<Description,,>
-- =============================================

CREATE PROCEDURE [dbo].[usp_getBudgetActivityInvoice]
	@activityID nvarchar(500),
	@productID nvarchar(500),
	@activityOfEstimateID nvarchar(500),
	@invoiceID nvarchar(500)
AS
BEGIN
----------------------------------------------------------------------------------------------
select
 t.act_activityId
,t.act_activityNo
,t.prd_productId
,t.act_typeTheme
,t.prd_productDetail
,t.normalCost
,t.themeCost
,t.totalCost 
,isnull(t.productStandBath,t.totalCost ) as productStandBath
,isnull(t.invoiceId,'') as invoiceId
,isnull(t.invoiceNo,'') as invoiceNo
,t.invTotalBath
,isnull(t.productBalanceBath,t.totalCost ) as productBalanceBath
,t.productBudgetStatusId
,t.productBudgetStatusNameTH
,t.invActionDate
,t.invoiceActivityStatusId
,t.invoiceActivityStatusNameTH
,t.invoiceSeq
,t.delFlag
from
(
	select
	   a.act_activityFormId as [act_activityId]
	  ,null as invoiceId
	  ,a.act_activityNo
	  -------------------------------------
	  ,a.prd_productId
	  ,a.act_typeTheme
	  ,a.prd_productDetail
	  ,a.normalCost
	  ,a.themeCost
	  ,a.totalCost 
	  ,null as invoiceNo
	  ,null as invTotalBath
	  ,null as productStandBath
	  ,null as productBalanceBath
	  ,a.productBudgetStatusId  --ID สภานะเงินของรายการ product
	  ,s.nameTH as productBudgetStatusNameTH  --สภานะเงินของรายการ product
	  ,null as invActionDate
	  ,null as invoiceActivityStatusId
	  ,null as invoiceActivityStatusNameTH
	  ,0 as invoiceSeq
	  ,a.delFlag
	from
	(
      select
	  af.id as act_activityFormId
	  ,af.[activityNo] as act_activityNo 
	  -------------------------------------
	  ,ae.productId as prd_productId
	  ,ae.typeTheme as act_typeTheme
	  ,rtrim(ltrim(pg.groupName)) +' / '+ pd.productName as prd_productDetail
	  ,isnull(ae.normalCost,0) as normalCost
	  ,isnull(ae.themeCost,0) as themeCost
	  ,isnull(ae.total,0) as totalCost 
	  ,Case When ai.budgetStatusId IS NULL THEN '1' ELSE ai.budgetStatusId  END as productBudgetStatusId  --สภานะเงินของรายการ product
	  -------------------------------------------
	  ,pc.cateName as prd_cate_productCateText
	  ,pg.groupName as prd_group_groupName
	  ,pd.productCode
	  ,pd.productName
	  ,pd.size
	  ,pd.unit
	  ,pd.smellId
	  ,ps.nameTH as smell_nameTH
	  ,ps.nameEN as smell_nameEN
	  ,pb.brandName as brand_Name
	  ------------------------------
	  ,Case When ai.productSeq IS NULL THEN '1' ELSE ai.productSeq  END as productSeq  --ลำดับรายการ product
	  ,af.delFlag
	  FROM [EActDB-DEV].[dbo].[TB_Act_ActivityForm] af 
	  left outer join dbo.TB_Reg_ApproveStatus ap on ap.id = af.statusId
	  left outer join dbo.TB_Act_ActivityOfEstimate ae on af.Id = ae.activityId 
	  left outer join dbo.TB_Act_Product pd on pd.id = ae.productId
	  left outer join dbo.TB_Act_ProductGroup pg on pg.id=af.productGroupId
	  left outer join dbo.TB_Act_ProductCate pc on pc.id=af.productCateId
	  left outer join dbo.tb_act_productType pt on pt.id=pc.id
	  left outer join dbo.TB_Act_ProductSmell ps on ps.id = pd.smellId
	  left outer join dbo.TB_Act_ProductBrand pb on pb.id = pd.brandId
	  left outer join dbo.[TB_Bud_ActivityProduct] ai on ai.[activityId] = ae.activityId and ai.[productId] = ae.productId
	) a
	left join [dbo].[TB_Bud_ActivityStatus] s on s.id = a.productBudgetStatusId

	union all

	select inv.[activityId] as activityId
	,inv.Id as invoiceId
	,act.activityNo as activityNo
	,ae.id as act_EstimateId
	,inv.[productId]
	,ae.typeTheme as act_typeTheme
	,rtrim(ltrim(pg.groupName)) +' / '+ isnull(pd.productName,'') as prd_productDetail
	,isnull(ae.normalCost,0) as normalCost
	,isnull(ae.themeCost,0) as themeCost
	,isnull(ae.total,0) as totalCost 
	,inv.[productStandBath]
	,inv.invoiceNo
	,inv.invTotalBath
	,inv.productBalanceBath
	,ap.[budgetStatusId] as productBudgetStatusId
	,st.nameTH as productBudgetStatusNameTH
	,inv.actionDate as invActionDate
	,inv.invoiceBudgetStatusId
	,ns.nameTH as invoiceBudgetStatusNameTH
	,inv.invoiceSeq
	,inv.delFlag
	from [dbo].[TB_Bud_ActivityInvoice] inv
	left join [dbo].[TB_Bud_ActivityProduct] ap on ap.[activityId] = inv.activityId and ap.[productId] = inv.[productId]
	left join [dbo].[TB_Bud_Activity] act on act.[activityId] = inv.[activityId]
	left join [dbo].[TB_Act_ActivityForm] af on af.Id = inv.[activityId] 
	left join dbo.TB_Act_ActivityOfEstimate ae on ae.activityId = inv.activityId  and ae.[productId] = inv.[productId]
	left outer join dbo.TB_Act_Product pd on pd.id = ae.productId
	left outer join dbo.TB_Act_ProductGroup pg on pg.id=af.productGroupId
	left join [dbo].[TB_Bud_ActivityStatus] st on st.id = ap.[budgetStatusId]
	left join (select * from [dbo].[TB_Bud_ActivityStatus]) ns on ns.id = inv.invoiceBudgetStatusId
) t
where 1=1
----------------------------------------------------------------------------------------------
and t.delFlag = 0
--and t.act_activityNo = 'WBEHPBC190019'
--and af.[activityNo] = @activityNo
and (t.act_activityId = @activityID OR @activityID IS NULL)
and (t.prd_productId = @productID OR @productID IS NULL)
and (t.invoiceId = @invoiceID OR @invoiceID IS NULL)
order by t.act_activityId,t.prd_productId,t.invoiceSeq

END
  --------------------------------------------------------------------------------------

  --EXEC dbo.usp_getBudgetActivityProduct '5063a8b1-95aa-47b3-a910-8897d37408db', '72C01966-5A53-42BE-92E5-55D8C514EE02','ae7a101d-93e1-424a-8be4-813e6785946c'
  
  --EXEC dbo.usp_getBudgetActivityProduct '5063a8b1-95aa-47b3-a910-8897d37408db', '72C01966-5A53-42BE-92E5-55D8C514EE02', null


  --EXEC dbo.usp_getBudgetActivityProduct null, null, null

