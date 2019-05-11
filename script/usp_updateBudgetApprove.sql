USE [EActDB-Dev]
GO
/****** Object:  StoredProcedure [dbo].[usp_updateApprove]    Script Date: 03/05/2019 12:08:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		surachj
-- Create date: 20190503
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[usp_updateBudgetApprove]
	@actFormId nvarchar(36),
	@empId nvarchar(36),
	@statusId nvarchar(36),
	@remark nvarchar(1000),
	@updateDate Datetime,
	@updateBy nvarchar(20)
AS
BEGIN

	declare @approveId nvarchar(36)
	declare @nextRangNo int
	declare @countAppDetailWithNextRangNO int

	select @approveId = id from dbo.TB_Reg_Approve where actFormId=@actFormId
	
	update dbo.TB_Reg_ApproveDetail
	set statusId = @statusId
	,remark = @remark
	,updatedByUserId = @updateBy
	,updatedDate = @updateDate
	where empId=@empId 
	and approveId=@approveId
	and statusId = '2'

	if( @@ROWCOUNT > 0 and @statusId = '3')
	begin

		select  top 1 @nextRangNo = (rangNo+1) from dbo.TB_Reg_ApproveDetail 
		where approveId = @approveId and empId=@empId and statusId='3' order by rangNo desc

		declare @countSuccessRang int
		declare @countAllRang int

		select @countAllRang = count(statusId) from dbo.TB_Reg_ApproveDetail  
		where approveId = @approveId and rangNo=@nextRangNo 

		select @countSuccessRang = count(statusId) from dbo.TB_Reg_ApproveDetail  
		where approveId = @approveId and rangNo=@nextRangNo and statusId='3'

		if( @countAllRang <> @countSuccessRang )
		update dbo.TB_Reg_ApproveDetail
		set statusId = '2'--(select id from dbo.TB_Reg_ApproveStatus aps where aps.nameEN='WaitApprove')
			,updatedByUserId = @updateBy
			,updatedDate = @updateDate
		where approveId=@approveId and rangNo=@nextRangNo
	end

END
