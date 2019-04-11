USE [EActDB-DEV]
GO
/****** Object:  StoredProcedure [dbo].[usp_getBudgetActivityProduct]    Script Date: 01/04/2019 3:46:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		surachat.j
-- Create date: 20190401
-- Description:	<Description,,>
-- =============================================

ALTER PROCEDURE [dbo].[usp_getBudgetActivityProduct]
	@activityID nvarchar(500),
	@productID nvarchar(500),
	@activityOfEstimateID nvarchar(500)
AS
BEGIN
----------------------------------------------------------------------------------------------
select
 t.act_activityId
,t.act_activityNo
,t.prd_productId
,t.activityOfEstimateId
,t.act_activityNo
,t.act_typeTheme
,t.prd_productDetail
,t.normalCost
,t.themeCost
,t.totalCost 
,t.invTotalBath
,isnull(t.productBalanceBath,t.totalCost ) as productBalanceBath
,t.budgetStatusId
,t.budgetStatusNameTH
,t.invoiceSeq
,t.delFlag
from
(
	select
	   a.act_activityFormId as [act_activityId]
	  ,null as invoiceId
	  ,a.act_activityNo
	  -------------------------------------
	  ,a.prd_productId
	  ,a.activityOfEstimateId
	  ,a.act_typeTheme
	  ,a.prd_productDetail
	  ,a.normalCost
	  ,a.themeCost
	  ,a.totalCost 
	  ,inv.invTotalBath as invTotalBath
	  ,a.totalCost - inv.invTotalBath as productBalanceBath
	  ,a.budgetStatusId  --ID สภานะเงินของรายการ product
	  ,s.nameTH as budgetStatusNameTH  --สภานะเงินของรายการ product
	  ,0 as invoiceSeq
	  ,a.delFlag
	from
	(
      select
	  af.id as act_activityFormId
	  ,af.[activityNo] as act_activityNo 
	  -------------------------------------
	  ,ae.productId as prd_productId
	  ,ae.id as [activityOfEstimateId]
	  ,ae.typeTheme as act_typeTheme
	  ,rtrim(ltrim(pg.groupName)) +' / '+ isnull(pd.productName,'') as prd_productDetail
	  ,isnull(ae.normalCost,0) as normalCost
	  ,isnull(ae.themeCost,0) as themeCost
	  ,isnull(ae.total,0) as totalCost 
	  ,Case When ai.budgetStatusId IS NULL THEN '1' ELSE ai.budgetStatusId  END as budgetStatusId  --สภานะเงินของรายการ product
	  -------------------------------------------
	  ,pc.cateName as prd_cate_productCateText
	  ,pg.groupName as prd_group_groupName
	  ,pd.productCode
	  ,pd.productName
	  ,pd.size
	  ,pd.unit
	  ,pd.smellId
	  ,ps.nameTH as smell_nameTH
	  ,ps.nameEN as smell_nameEN
	  ,pb.brandName as brand_Name
	  ------------------------------
	  ,Case When ai.productSeq IS NULL THEN '1' ELSE ai.productSeq  END as productSeq  --ลำดับรายการ product
	  ,af.delFlag
	  FROM [EActDB-DEV].[dbo].[TB_Act_ActivityForm] af 
	  left outer join dbo.TB_Reg_ApproveStatus ap on ap.id = af.statusId
	  left outer join dbo.TB_Act_ActivityOfEstimate ae on af.Id = ae.activityId 
	  left outer join dbo.TB_Act_Product pd on pd.id = ae.productId
	  left outer join dbo.TB_Act_ProductGroup pg on pg.id=af.productGroupId
	  left outer join dbo.TB_Act_ProductCate pc on pc.id=af.productCateId
	  left outer join dbo.tb_act_productType pt on pt.id=pc.id
	  left outer join dbo.TB_Act_ProductSmell ps on ps.id = pd.smellId
	  left outer join dbo.TB_Act_ProductBrand pb on pb.id = pd.brandId
	  left outer join dbo.[TB_Bud_ActivityProduct] ai on ai.[activityId] = ae.activityId and ai.[productId] = ae.productId
	) a
	left join [dbo].[TB_Bud_ActivityStatus] s on s.id = a.budgetStatusId
	left join 
	( select 
		[activityId]
		,[activityOfEstimateId]
		,sum([invTotalBath]) as invTotalBath
		from [dbo].[TB_Bud_ActivityInvoice]
		group by [activityId],[activityOfEstimateId]
  ) inv on inv.activityOfEstimateId = a.activityOfEstimateId
) t
where 1=1
----------------------------------------------------------------------------------------------
and t.delFlag = 0
--and t.act_activityNo = 'WBEHPBC190019'
and (t.act_activityId = @activityID OR @activityID IS NULL)
and (t.prd_productId = @productID OR @productID IS NULL)
and (t.activityOfEstimateId = @activityOfEstimateId OR @activityOfEstimateId IS NULL)
order by t.act_activityId,t.prd_productId,t.[activityOfEstimateId],t.invoiceSeq

END
  --------------------------------------------------------------------------------------

  --EXEC dbo.usp_getBudgetActivityProduct '5063a8b1-95aa-47b3-a910-8897d37408db', '72C01966-5A53-42BE-92E5-55D8C514EE02','ae7a101d-93e1-424a-8be4-813e6785946c'
  
  -- EXEC dbo.usp_getBudgetActivityProduct '700ade5e-a6b3-4150-9a81-fc9f53fdae24', null, null,null
  
  -- EXEC dbo.usp_getBudgetActivityProduct 'f91d5f98-0443-4761-9ea2-eb6b8a58f94a',  null,null

  -- EXEC dbo.usp_getBudgetActivityProduct '6ce93ae9-b6ad-4bc5-9a58-f22c10deed41', '40541609-4369-4464-9897-774CA2124F95', null

