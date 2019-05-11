USE [EActDB-DEV]
GO
/****** Object:  StoredProcedure [dbo].[usp_getApproveBudgetByEmpId]    Script Date: 07/05/2019 12:13:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		surachat j
-- Create date: 20190425
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[usp_getBudgetFormByEmpId]
	@empId nvarchar(20)
AS
BEGIN

select 
 af.[Id] as ActivityFormId
,ba.approveStatusId as [statusId]
,ss.nameTH as statusName
,af.[activityNo]
------------------------------------------------------------
,ra.id as regApproveId
,ra.flowId as regApproveFlowId
,ra.actFormId as budgetApproveId
,ra.[createdDate] as documentDate
------------------------------------------------------------
,af.[reference]
,af.[customerId]
,ch.chanelGroup as channelName
,pt.id as productTypeId
,pt.nameEN as productTypeNameEN
,cs.cusShortName
,pc.cateName as productCateText
-----------------------------------------
,af.[productCateId]
,af.[productGroupId]
,pg.groupName as productGroupName
,af.[activityPeriodSt]
,af.[activityPeriodEnd]
,af.[costPeriodSt]
,af.[costPeriodEnd]
,af.[activityName]
,af.[theme]
,af.[objective]
,af.[trade]
,af.[activityDetail]
---------------------------------------
,ba.budgetActivityId
,ba.id as budgetApproveId
,bt.budgetActivityStatusId
,bs.nameTH as budgetActivityStatus
,ba.id  as approveId
--,ad.id as approveDetailId
,ba.[createdDate]
,ba.[createdByUserId]
,ba.[updatedDate]
,ba.[updatedByUserId]
--------------------------------------
,(select sum(ae.normalCost) from dbo.TB_Act_ActivityOfEstimate ae where ae.activityId=af.Id) as normalCost
,(select sum(ae.themeCost) from dbo.TB_Act_ActivityOfEstimate ae where ae.activityId=af.Id ) as themeCost
,(select sum(total) from dbo.TB_Act_ActivityOfEstimate ae where ae.activityId=af.Id ) as totalCost
,(
	select sum(ai.invoiceTotalBath) as total_invoice_bath
	from [dbo].[TB_Bud_ApproveInvoice] bi
	left join [dbo].[TB_Bud_Approve] baa on baa.id = bi.[budgetApproveId]
	left join [dbo].[TB_Bud_ActivityInvoice] ai on ai.id = bi.[budgetActivityInvoiceId]
	left join [dbo].[TB_Bud_Activity] ac on ac.id = ai.budgetActivityId
	where bi.[budgetApproveId] = ba.id --'B277F66F-4CB9-47F9-9405-2176EB9FDE25'
 ) as totalInvoiceApproveBath
from [dbo].[TB_Bud_Approve] ba 
left join dbo.TB_Reg_Approve ra on ba.id = ra.[actFormId] 
left join [dbo].[TB_Reg_Flow] fl on fl.id = ra.flowId
left join [dbo].[TB_Reg_Subject] fs on fs.id = fl.subjectId
left join [dbo].[TB_Bud_Activity] bt on bt.id = ba.[budgetActivityId]
----------------------------------------------------------
left join [dbo].[TB_Act_ActivityForm] af on af.id = bt.activityId
left outer join dbo.TB_Reg_ApproveStatus ss on ss.id = ba.approveStatusId
left outer join dbo.TB_Act_Customers cs on cs.id = af.customerId
left outer join dbo.TB_Act_ProductGroup pg on pg.id=af.productGroupId
left outer join dbo.TB_Act_ProductCate pc on pc.id=af.productCateId 
left outer join dbo.TB_Act_Chanel ch on ch.id=cs.chanel_Id 
left outer join dbo.tb_act_productType pt on pt.id=pc.productTypeId
----------------------------------------------------------
left join [dbo].[TB_Bud_ActivityStatus] bs on bs.id = bt.budgetActivityStatusId
where fs.id = '639C73A8-328E-433E-8B12-19B04AC8D61A' -- budget form only
--and ba.[createdByUserId] ='11017721'
and (ba.[createdByUserId] = @empId OR @empId IS NULL)
order by ba.[createdDate] desc

END

--exec usp_getBudgetFormByEmpId '11017721'

--update [dbo].[TB_Bud_Approve] set 
--createdByUserId = '11017721'
--,updatedByUserId = '11017721'