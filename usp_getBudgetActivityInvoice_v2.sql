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

ALTER PROCEDURE [dbo].[usp_getBudgetActivityInvoice]
	@activityID nvarchar(500),
	@activityOfEstimateID nvarchar(500),
	@invoiceID nvarchar(500)
AS
BEGIN
----------------------------------------------------------------------------------------------
select
 t.activityId as act_activityId
,t.activityNo as act_activityNo
,t.act_EstimateId
,t.productId as prd_productId
,t.act_typeTheme
,t.prd_productDetail
,t.normalCost
,t.themeCost
,t.totalCost
,t.[productStandBath] as productStandBath
,isnull(t.invoiceId,'') as invoiceId
,isnull(t.invoiceNo,'') as invoiceNo
,t.invTotalBath as invoiceTotalBath
,t.productBalanceBath as productBalanceBath
,t.productBudgetStatusId
,t.productBudgetStatusNameTH
,t.invoiceActionDate
,t.invoiceBudgetStatusId
,t.invoiceBudgetStatusNameTH
,t.invoiceSeq
,t.delFlag
from
(
	select inv.[activityId] as activityId
	
	,act.activityNo as activityNo
	,ae.id as act_EstimateId
	,inv.[productId]
	,ae.typeTheme as act_typeTheme
	,rtrim(ltrim(pg.groupName)) +' / '+ isnull(pd.productName,'') as prd_productDetail
	,isnull(ae.normalCost,0) as normalCost
	,isnull(ae.themeCost,0) as themeCost
	,isnull(ae.total,0) as totalCost 
	,inv.[productStandBath]
	,inv.Id as invoiceId
	,inv.invoiceNo
	,inv.invTotalBath
	,inv.productBalanceBath
	,ap.[budgetStatusId] as productBudgetStatusId
	,st.nameTH as productBudgetStatusNameTH
	,inv.actionDate as invoiceActionDate
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
and (t.activityId = @activityID OR @activityID IS NULL)
and (t.act_EstimateId = @activityOfEstimateID OR @activityOfEstimateID IS NULL)
and (t.invoiceId = @invoiceID OR @invoiceID IS NULL)
order by t.activityId,t.act_EstimateId,t.invoiceSeq

END
  --------------------------------------------------------------------------------------

  --EXEC dbo.usp_getBudgetActivityInvoice 'f91d5f98-0443-4761-9ea2-eb6b8a58f94a', 'b51628df-7c52-438e-8462-7db8ee483e57','3c166c63-2fc2-401e-98a7-4787ccd53001'
  
  --EXEC dbo.usp_getBudgetActivityInvoice null, null, null
  
  --EXEC dbo.usp_getBudgetActivityInvoice 'f91d5f98-0443-4761-9ea2-eb6b8a58f94a', 'b51628df-7c52-438e-8462-7db8ee483e57',null

  --EXEC dbo.usp_getBudgetActivityInvoice 'f91d5f98-0443-4761-9ea2-eb6b8a58f94a','2c7f7d46-dd4a-4c84-9f4c-a26221270eb4',null

