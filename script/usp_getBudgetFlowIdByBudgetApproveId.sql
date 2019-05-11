USE [EActDB-DEV]
GO
/****** Object:  StoredProcedure [dbo].[usp_getBudgetFlowIdByBudgetId]    Script Date: 08/05/2019 9:24:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		surachat j
-- Create date: 20190422
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[usp_getBudgetFlowIdByBudgetApproveId]
	@budgetApproveId nvarchar(36)
AS
BEGIN
	
	select f.* ,l.*
	from dbo.TB_Reg_Flow f
	left join dbo.TB_Reg_FlowLimit l on l.id = f.flowLimitId 
	where 1=1
	and f.id = (select flowId from [dbo].[TB_Reg_Approve] where [actFormId] = @budgetApproveId)

END