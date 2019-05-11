/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [id]
      ,[subjectId]
      ,[customerId]
      ,[productCatId]
      ,[productTypeId]
      ,[flowLimitId]
      ,[delFlag]
      ,[createdDate]
      ,[createdByUserId]
      ,[updatedDate]
      ,[updatedByUserId]
  FROM [EActDB-DEV].[dbo].[TB_Reg_Flow]
  where subjectId = '639C73A8-328E-433E-8B12-19B04AC8D61A'

  select * 
  --delete
  from [dbo].[TB_Reg_FlowApprove]
  --where createdByUserId = '70016911'
  where flowId in 
  (
  select id
  --delete
  FROM [EActDB-DEV].[dbo].[TB_Reg_Flow]
  where subjectId = '639C73A8-328E-433E-8B12-19B04AC8D61A'
  )
  -----------------------------------------------------------------------------
  -- import import_MTM_Flow and import_MTM_FlowDetal

  select * from [dbo].[import_MTM_Flow]
  select * from [dbo].[import_MTM_FlowDetal]

   --insert into [EActDB-DEV].[dbo].[TB_Reg_Flow]
  SELECT [id]
      ,[subjectId]
      ,[customerId]
      ,[productCatId]
      ,[productTypeId]
      ,[approveLimitId] as [flowLimitId]
      ,0 as [delFlag]
	  ,getdate() as [createdDate]
	  ,'70016911' as [createdByUserId]
	  ,getdate() as [updatedDate]
	  ,'70016911'[updatedByUserId]
  FROM [EActDB-DEV].[dbo].[import_MTM_Flow]
  where id is not null

  --insert into [EActDB-DEV].[dbo].[TB_Reg_FlowApprove]
  select [id]
      ,[flowid]
      ,[companyId]
      ,[employeeCode]
      ,[ApproveGroup]
      ,[RankNo]
      ,[showInDoc]
      ,'' [description]
  ,[deleteFlag]
 ,getdate() as [createdDate]
 ,'70016911' as [createdByUserId]
 ,getdate() as [updatedDate]
 ,'70016911'[updatedByUserId]
  FROM [dbo].[import_MTM_FlowDetal]
  where id is not null
  -------------------------------------------------------------------------
  select * from [dbo].[TB_Reg_Flow] where createdByUserId = '70016911'
  select * from [dbo].[TB_Reg_FlowApprove] where createdByUserId = '70016911'


  ---- clone to prd ------

  select count(*) from [EActDB].[dbo].[TB_Reg_Flow] where createdByUserId = '70016911'
  select count(*) from [EActDB].[dbo].[TB_Reg_FlowApprove] where createdByUserId = '70016911'

  --insert into  [EActDB].[dbo].[TB_Reg_Flow] 
  select * from  [EActDB-DEV].[dbo].[TB_Reg_Flow] where createdByUserId = '70016911'

  --insert into [EActDB].[dbo].[TB_Reg_FlowApprove]
  select * from  [EActDB-DEV].[dbo].[TB_Reg_FlowApprove] where createdByUserId = '70016911'

