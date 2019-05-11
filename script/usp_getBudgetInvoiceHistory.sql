USE [EActDB-DEV]
GO
/****** Object:  StoredProcedure [dbo].[usp_getBudgetInvoiceHistory]    Script Date: 07/05/2019 3:44:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		surachat.j
-- Create date: 20190410
-- Description:	<Description,,>
-- =============================================

ALTER PROCEDURE [dbo].[usp_getBudgetInvoiceHistory]
	@activityId nvarchar(500)
	,@budgetApproveId nvarchar(500)


AS
BEGIN

----------------------------------------------------------------------------------------------
select 
 t.budgetActivityId
,t.activityId as activityId
,t.activityNo as activityNo
,t.activityName
,t.act_EstimateId as activitEstimateId
,t.productId as productId
,t.act_typeTheme as activityTypeTheme
,t.prd_productDetail as productDetail
,t.normalCost
,t.themeCost
,t.totalCost
,t.[productStandBath] as productStandBath
,isnull(t.invoiceId,'') as invoiceId
,isnull(t.invoiceNo,'') as invoiceNo
,isnull(t.invoiceTotalBath,0) as invoiceTotalBath
,isnull(t.productBalanceBath,t.totalCost) as productBalanceBath
,t.productBudgetStatusId
,t.productBudgetStatusNameTH
,convert(varchar(10),t.invoiceActionDate,103) as invoiceActionDate
,t.invoiceBudgetStatusId
,t.invoiceBudgetStatusNameTH
,t.invoiceSeq
,isnull(t.productCountInvoice,0) as productCountInvoice
,isnull(t.productSumInvoiceBath,0) as productSumInvoiceBath
,a.sum_cost_product_inv -- sum activity cost of product has invoice
,a.sum_total_invoice -- sum activity invoice
,a.sum_cost_product_inv - a.sum_total_invoice as sum_balance_product_inv
,t.invoiceApproveStatusId
,aps.nameTH as invoiceApproveStatusName
,t.budgetApproveId
,t.budgetapproveInvoiceId
from
(

	select 
	ap.[budgetActivityId] as budgetActivityId
	,af.Id as activityId
	,af.activityNo as activityNo
	,af.activityName
	,ae.id as act_EstimateId
	,ae.[productId]
	,ae.typeTheme as act_typeTheme
	,rtrim(ltrim(pg.groupName)) +' / '+ isnull(pd.productName,'') as prd_productDetail
	,isnull(ae.normalCost,0) as normalCost
	,isnull(ae.themeCost,0) as themeCost
	,isnull(ae.total,0) as totalCost 
	,isnull(ae.total,0) as productStandBath
	,'' as invoiceId
	,'' as invoiceNo
	--,si.productSumInvoiceBath as invoiceTotalBath
	,0 as invoiceTotalBath
	,ae.total as productBalanceBath
	,ap.[budgetStatusId] as productBudgetStatusId
	,st.nameTH as productBudgetStatusNameTH
	,af.updatedDate as invoiceActionDate
	,st.id as invoiceBudgetStatusId
	,st.nameTH as invoiceBudgetStatusNameTH
	,0 as invoiceSeq
	,si.productCountInvoice
	,si.productSumInvoiceBath
	,'' as invoiceApproveStatusId
	,'' as approveStatusId
    ,'' as budgetApproveId
	,'' as budgetapproveInvoiceId

	------------------------------
	from dbo.[TB_Act_ActivityForm] af
	left join dbo.TB_Act_ProductGroup pg on pg.id=af.productGroupId
	left join dbo.TB_Act_ActivityOfEstimate ae on ae.activityId = af.Id
	left join dbo.TB_Act_Product pd on pd.id = ae.productId
	left join dbo.TB_Bud_ActivityProduct as ap on ap.activityOfEstimateId = ae.id
	left join dbo.[TB_Bud_ActivityStatus] as st on st.id = ap.budgetStatusId
	left join (	select
				[activityId]
				,[activityOfEstimateId]
				,count(distinct [invoiceNo]) as productCountInvoice
				,sum([invoiceTotalBath]) as productSumInvoiceBath
				from [dbo].[TB_Bud_ActivityInvoice]
				group by [activityId],[activityOfEstimateId]
				) si on si.activityOfEstimateId = ae.id
	--where af.Id = 'b77c8afc-a9e2-4397-94ab-a8132ce916a3'

	union all

	select 
	 act.id as budgetActivityId
	,inv.[activityId] as activityId
	,act.activityNo as activityNo
	,af.activityName
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
	,inv.invoiceTotalBath
	,inv.productBalanceBath
	,ap.[budgetStatusId] as productBudgetStatusId
	,st.nameTH as productBudgetStatusNameTH
	,inv.actionDate as invoiceActionDate
	,inv.invoiceBudgetStatusId
	,ns.nameTH as invoiceBudgetStatusNameTH
	,inv.invoiceSeq
	,si.productCountInvoice
	,si.productSumInvoiceBath
	,CASE
		WHEN bi.id  is null  THEN '1'
		ELSE ba.[approveStatusId]
		END AS invoiceApproveStatusId
	,ba.[approveStatusId]
	,isnull(ba.id,'') as budgetApproveId
	,isnull(bi.id,'') as budgetapproveInvoiceId
	from [dbo].[TB_Bud_ActivityInvoice] inv
	left join [dbo].[TB_Bud_ActivityProduct] ap on ap.[activityId] = inv.activityId and ap.[productId] = inv.[productId]
	left join [dbo].[TB_Bud_Activity] act on act.[activityId] = inv.[activityId]
	left join [dbo].[TB_Act_ActivityForm] af on af.Id = inv.[activityId] 
	left join dbo.TB_Act_ActivityOfEstimate ae on ae.activityId = inv.activityId  and ae.[productId] = inv.[productId]
	left outer join dbo.TB_Act_Product pd on pd.id = ae.productId
	left outer join dbo.TB_Act_ProductGroup pg on pg.id=af.productGroupId
	left join [dbo].[TB_Bud_ActivityStatus] st on st.id = ap.[budgetStatusId]
	left join (select * from [dbo].[TB_Bud_ActivityStatus]) ns on ns.id = inv.invoiceBudgetStatusId
	left join (	select
				[activityId]
				,[activityOfEstimateId]
				,count(distinct [invoiceNo]) as productCountInvoice
				,sum([invoiceTotalBath]) as productSumInvoiceBath
				from [dbo].[TB_Bud_ActivityInvoice]
				group by [activityId],[activityOfEstimateId]
				) si on si.activityOfEstimateId = ae.id
	----------------------------------------------------------------------------------------
	left join [dbo].[TB_Bud_ApproveInvoice] bi on bi.[budgetActivityInvoiceId] = inv.id  
	left join [dbo].[TB_Bud_Approve] ba on ba.id = bi.[budgetApproveId]
	-----------------------------------------------------------------------------------------
	-- where af.Id = '2b39cce9-10d9-40de-86a1-8a069edbcc1c'

) t 
left join
(
	select 
	 es.activityId,count(es.id) as count_product_inv 
	,isnull(sum(total),0) as sum_cost_product_inv 
	,isnull(sum(iv.sum_total_invoice),0) as sum_total_invoice
	from  [dbo].[TB_Act_ActivityOfEstimate] es
	left join (
				select activityOfEstimateId,isnull(sum([invoiceTotalBath]),0) as sum_total_invoice
				from [dbo].[TB_Bud_ActivityInvoice] 
				group by activityOfEstimateId 
				) iv on iv.activityOfEstimateId = es.id
	where 1=1
	and iv.activityOfEstimateId is not null
	group by es.activityId
) a on a.activityId = t.activityId
left join [dbo].[TB_Reg_ApproveStatus] aps on aps.id = t.invoiceApproveStatusId
where 1=1
and t.productCountInvoice > 0
--and t.activityId = '2b39cce9-10d9-40de-86a1-8a069edbcc1c'
and (t.activityId = @activityId OR @activityId IS NULL)
and (t.budgetApproveId = @budgetApproveId OR @budgetApproveId IS NULL)
order by t.activityId,t.act_EstimateId,t.invoiceSeq

END
  --------------------------------------------------------------------------------------

  
  --  EXEC dbo.usp_getBudgetInvoiceHistory null ,null
  --  EXEC dbo.usp_getBudgetInvoiceHistory 'ef07c508-e054-440d-97b3-a773884f2f17' ,null

  --  EXEC dbo.usp_getBudgetInvoiceHistory null, '6C9A92B2-82CB-4C7D-B6C7-773EDF925B98'

 

