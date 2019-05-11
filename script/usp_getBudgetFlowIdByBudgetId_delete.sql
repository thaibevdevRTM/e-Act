USE [EActDB-Dev]
GO
/****** Object:  StoredProcedure [dbo].[usp_getBudgetFlowIdByBudgetId]    Script Date: 08/05/2019 9:27:01 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		surachat j
-- Create date: 20190422
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[usp_getBudgetFlowIdByBudgetId]
	@subId nvarchar(36),
	@budgetApproveId nvarchar(36)
AS
BEGIN
	
	DECLARE @var_total_invoice_bath decimal(12,2);

	select @var_total_invoice_bath = sum(invoiceTotalBath) 
	from [dbo].[TB_Bud_ApproveInvoice] a
	left join [dbo].[TB_Bud_ActivityInvoice] i on i.Id = a.[budgetActivityInvoiceId]
	where a.[budgetApproveId] = @budgetApproveId


	select a.* into #tempAct
	from [dbo].[TB_Bud_Activity] b
	left join dbo.TB_Act_ActivityForm a on a.id = b.activityId


	select f.* ,l.*
	from dbo.TB_Reg_Flow f
	left join dbo.TB_Reg_FlowLimit l on l.id = f.flowLimitId 
	where f.subjectId=@subId 
	and f.customerId = (select customerId from #tempAct) 
	and f.productCatId=(select productCateId from #tempAct )
	and l.limitto >= @var_total_invoice_bath
    and l.limitBegin <= @var_total_invoice_bath

	--drop table #tempAct
END