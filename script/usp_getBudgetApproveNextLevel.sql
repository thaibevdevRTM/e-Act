USE [EActDB-Dev]
GO
/****** Object:  StoredProcedure [dbo].[usp_getApproveNextLevel]    Script Date: 13/05/2019 8:02:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		surachat
-- Create date: 20190413
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[usp_getBudgetApproveNextLevel]
	@actFormId nvarchar(36)
AS
BEGIN

	declare @approveId nvarchar(36)
	declare @rangNo int

	select * into #empTemp from DBAuthen.dbo.empMaster
	select @approveId=id from TB_Reg_Approve where actFormId=@actFormId
	select * into #tempAppDetail from TB_Reg_ApproveDetail 
	where approveId=@approveId 
	and statusId='2'
	and empId not in (select empId from dbo.TB_Reg_EmailConfig where emailConfig ='Dialy')
	order by rangNo


	select emp.empEmail 
	,emp.empPrefix 
	, emp.empFNameTH + ' ' + emp.empLNameTH  as empName
	, af.activityName
	, (select activitySales from dbo.TB_Act_ActivityGroup where id=af.theme) as activitySales
	, af.activityNo
	, ( select sum(total) from dbo.TB_Act_ActivityOfEstimate where activityid=@actFormId ) as sumTotal
	, (select empPrefix +' ' +empFNameTH + ' ' + empLNameTH  from #empTemp where empId=af.createdByUserId) as createBy
	, ap.* 
	from #tempAppDetail ap
	left outer join [dbo].[TB_Reg_Approve] ra on ra.id = @actFormId
	left outer join dbo.TB_Act_ActivityForm af on af.id in
	(
	select ba.[activityId] 
	from [dbo].[TB_Bud_Approve] bp
	left join [dbo].[TB_Bud_Activity] ba on ba.id = bp.budgetActivityId
	where bp.id = @actFormId
	)
	left outer join #empTemp emp on emp.empId = ap.empId
	where ap.rangNo=( select distinct rangNo from #tempAppDetail )

drop table #tempAppDetail
drop table #empTemp

END




    select id from TB_Reg_Approve where actFormId='82F3D279-9E01-4E5E-A556-5A04CE01DBAA'
	@approveId = '86e9aebd-ae9f-4767-ae4f-c8b43785ed7d'

	select * into #tempAppDetail from TB_Reg_ApproveDetail 
	where approveId='86e9aebd-ae9f-4767-ae4f-c8b43785ed7d'
	and statusId='2'
	order by rangNo

	select * from #tempAppDetail

	'82F3D279-9E01-4E5E-A556-5A04CE01DBAA'


	select ba.[activityId] 
	from [dbo].[TB_Bud_Approve] bp
	left join [dbo].[TB_Bud_Activity] ba on ba.id = bp.budgetActivityId
	where bp.id = '82F3D279-9E01-4E5E-A556-5A04CE01DBAA'

	select emp.empEmail 
	,emp.empPrefix 
	, emp.empFNameTH + ' ' + emp.empLNameTH  as empName
	, af.activityName
	, (select activitySales from dbo.TB_Act_ActivityGroup where id=af.theme) as activitySales
	, af.activityNo
	, ( select sum(total) from dbo.TB_Act_ActivityOfEstimate where activityid='82F3D279-9E01-4E5E-A556-5A04CE01DBAA' ) as sumTotal
	, (select empPrefix +' ' +empFNameTH + ' ' + empLNameTH  from #empTemp where empId=af.createdByUserId) as createBy
	, ap.* 

	select * 
	from #tempAppDetail ap
	left outer join dbo.TB_Act_ActivityForm af on af.id in 
	(
	select ba.[activityId] 
	from [dbo].[TB_Bud_Approve] bp
	left join [dbo].[TB_Bud_Activity] ba on ba.id = bp.budgetActivityId
	where bp.id = '82F3D279-9E01-4E5E-A556-5A04CE01DBAA'
	)
	left outer join #empTemp emp on emp.empId = ap.empId
	where ap.rangNo=( select distinct rangNo from #tempAppDetail )

	select * from [dbo].[TB_Reg_Approve]

	ra.id = '82F3D279-9E01-4E5E-A556-5A04CE01DBAA'