--/****** Script for SelectTopNRows command from SSMS  ******/
--SELECT TOP (1000) [Id]
--      ,[activityId]
--      ,[productId]
--      ,[activityOfEstimateId]
--      ,[paymentNo]
--      ,[invoiceNo]
--      ,[productCostBath]
--      ,[productStandBath]
--      ,[invoiceTotalBath]
--      ,[productBalanceBath]
--      ,[actionDate]
--      ,[invoiceSeq]
--      ,[invoiceBudgetStatusId]
--      ,[delFlag]
--      ,[createdDate]
--      ,[createdByUserId]
--      ,[updatedDate]
--      ,[updatedByUserId]
--  FROM [EActDB-DEV].[dbo].[TB_Bud_ActivityInvoice]
--  where  [Id] not in (select [activityInvoiceId] from [dbo].[TB_Bud_InvoiceApprove])
--  order by [activityOfEstimateId] , invoiceSeq


  select i.[Id]
      ,i.[activityId]
      ,i.[productId]
      ,i.[activityOfEstimateId]
      ,i.[paymentNo]
      ,i.[invoiceNo]
      ,i.[productCostBath]
      ,i.[productStandBath]
      ,i.[invoiceTotalBath]
      ,i.[productBalanceBath]
      ,i.[actionDate]
      ,i.[invoiceSeq]
      ,i.[invoiceBudgetStatusId]
	  ,i.updatedDate as inv_updatedDate
	  ,i.createdDate as inv_createdDate
	  ,a.flowId as approve_flowId
 FROM [EActDB-DEV].[dbo].[TB_Bud_ActivityInvoice] i
 left join [dbo].[TB_Bud_InvoiceApprove] a on a.activityInvoiceId = i.Id
 where a.flowId is not null
 --and convert(date,i.createdDate) between '04-05-2019' and '04-05-2019' -- MM-dd-yyyy
 order by i.[activityOfEstimateId] , i.invoiceSeq


 select * 
 from [dbo].[TB_Reg_Flow]
 where id = 'F90D568F-4601-4A5D-B0DF-CB14CBF73B36'


 select * 
 from [dbo].[TB_Reg_FlowApprove]
 where flowid = 'F90D568F-4601-4A5D-B0DF-CB14CBF73B36'
 order by rangNo