USE [EActDB-DEV]
GO
/****** Object:  StoredProcedure [dbo].[usp_getBudgetActivity]    Script Date: 29/03/2019 2:58:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		surachat.j
-- Create date: 20190313
-- Description:	<Description,,>
-- =============================================

ALTER PROCEDURE [dbo].[usp_getBudgetActivity]
	@act_approveStatusId nvarchar(50),
	@act_activityNo nvarchar(100)
AS
BEGIN


select 
 t.act_form_id
,t.act_approveStatusId
,t.approve_nameTH
,t.approve_nameEN
,t.act_activityNo 
,t.act_documentDate
,t.act_reference
,t.act_customerId
,t.cus_cusShortName
,t.cus_cusNameTH
,t.cus_cusNameEN
,t.act_activityPeriodSt
,t.act_activityPeriodEnd
,t.act_costPeriodSt
,t.act_costPeriodEnd
,t.act_activityName
,t.act_theme
,t.act_objective
,t.act_trade
,t.act_activityDetail
,t.act_delFlag
,t.act_createdDate
,t.act_createdByUserId
,t.act_updatedDate
,t.act_updatedByUserId
,t.act_normalCost
,t.act_themeCost
,t.act_totalCost
,t.bud_ActivityStatusId
,bcs.nameTH as bud_ActivityStatus
--------------------------------------------------
,act_sum.act_cost
,act_sum.act_inv_total
,act_sum.act_balance
--------------------------------------------------
from
(
SELECT af.[Id] as act_form_id
      ,af.[statusId]  as act_approveStatusId
	  ,ap.[nameTH] as approve_nameTH
	  ,ap.[nameEN] as approve_nameEN
      ,af.[activityNo] as act_activityNo 
      ,af.[documentDate] as act_documentDate
      ,af.[reference] as act_reference
      ,af.[customerId] as act_customerId
	  ,cs.cusShortName as cus_cusShortName
	  ,cs.[cusNameTH] as cus_cusNameTH
	  ,cs.[cusNameEN] as cus_cusNameEN
      ,af.[activityPeriodSt] as act_activityPeriodSt
      ,af.[activityPeriodEnd] as act_activityPeriodEnd
      ,af.[costPeriodSt] as act_costPeriodSt
      ,af.[costPeriodEnd] as act_costPeriodEnd
      ,af.[activityName] as act_activityName
      ,af.[theme] as act_theme
      ,af.[objective] as act_objective
      ,af.[trade] as act_trade
      ,af.[activityDetail] as act_activityDetail
      ,af.[delFlag] as act_delFlag
      ,af.[createdDate] as act_createdDate
      ,af.[createdByUserId] as act_createdByUserId
      ,af.[updatedDate] as act_updatedDate
      ,af.[updatedByUserId] as act_updatedByUserId
	  ,(select sum(ae.normalCost) from dbo.TB_Act_ActivityOfEstimate ae where ae.activityId=af.Id) as act_normalCost
	  ,(select sum(ae.themeCost) from dbo.TB_Act_ActivityOfEstimate ae where ae.activityId=af.Id ) as act_themeCost
	  ,(select sum(total) from dbo.TB_Act_ActivityOfEstimate ae where ae.activityId=af.Id ) as act_totalCost
	  ,Case When bcs.[id] IS NULL THEN '1' ELSE bcs.[id]  END as bud_ActivityStatusId
  FROM [EActDB-DEV].[dbo].[TB_Act_ActivityForm] af 
  left outer join dbo.TB_Reg_ApproveStatus ap on ap.id = af.statusId
  left outer join dbo.TB_Act_Customers cs on cs.id = af.customerId
  left outer join [dbo].[TB_Bud_Activity] ba on ba.activityId=af.id
  left outer join [dbo].[TB_Bud_ActivityStatus] bcs on  bcs.[id] = ba.[budgetActivityStatusId]
  ) as t
  left outer join [dbo].[TB_Bud_ActivityStatus] bcs on  bcs.[id] = t.bud_ActivityStatusId
  left outer join 
  (
	  select 
		[activityId]
		,sum([productCostBath]) as act_cost
		,sum([invTotalBath]) as act_inv_total
		,sum([productCostBath]) - sum([invTotalBath]) as act_balance 
		from [EActDB-DEV].[dbo].[TB_Bud_ActivityInvoice]
		group by [activityId]
  ) act_sum on act_sum.[activityId] = t.act_form_id
  where 1=1
  -- and t.act_form_id ='700ade5e-a6b3-4150-9a81-fc9f53fdae24'
  AND (t.act_approveStatusId = @act_approveStatusId OR @act_approveStatusId IS NULL)
  AND (t.act_activityNo = @act_activityNo OR @act_activityNo IS NULL)
  order by t.act_activityNo

  END

  ----------------------------------------------------------------------------------------

  --    EXEC dbo.[usp_getBudgetActivity] 2,null
  
  ----------------------------------------------------------------------------------------
  -- select 
  -- [activityId]
  -- ,sum([productCostBath]) as act_cost							--ยอดยกมา ?
  -- ,sum([invTotalBath]) as act_inv_total							--ยอดที่ตัดไปแล้ว
  -- ,sum([productCostBath]) - sum([invTotalBath]) as act_balance	--คงเหลือ 
  -- from [EActDB-DEV].[dbo].[TB_Bud_ActivityInvoice]
  -- where [activityId] ='700ade5e-a6b3-4150-9a81-fc9f53fdae24'
  -- group by [activityId]

