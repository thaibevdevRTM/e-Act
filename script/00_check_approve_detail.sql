/****** Script for SelectTopNRows command from SSMS  ******/
SELECT fa.id as Reg_flow_aprove_id
	  ,f.id as Reg_Flow_Id
	  ,f.[subjectId]
	  ,s.nameEN as subject_name
      ,f.[customerId]
	  ,c.cusNameEN as customer_name
      ,f.[productCatId]
	  ,pc.cateName as product_cat_name
      ,f.[productTypeId]
	  ,pt.nameEN as prd_type
	  ,ag.id as group_id
	  ,ag.nameTH as approve_group_nameTh
	  ,fa.empId
	  ,fa.rangNo
      ,f.[flowLimitId]
	  ,l.limitBegin
	  ,l.limitto
	  ,f.[createdByUserId]
  FROM [dbo].[TB_Reg_Flow] f
  left join [dbo].[TB_Reg_FlowApprove] fa on fa.flowId = f.id
  left join [dbo].[TB_Reg_ApproveGroup] ag on ag.id = fa.approveGroupId
  left join [dbo].[TB_Reg_Subject] s on s.id = f.subjectId
  left join [dbo].[TB_Act_Customers] c on c.id = f.customerId
  left join [dbo].[TB_Act_ProductCate] pc on pc.id = f.productCatId
  left join [dbo].[TB_Act_ProductType] pt on pt.id = f.productTypeId
  left join [dbo].[TB_Reg_FlowLimit] l on l.id = f.flowLimitId
  where 1=1
  and f.[createdByUserId] = '70016911'
  --and fa.id is null
  and c.cusNameEN= 'Makro'
  and l.limitBegin = 1
  --and ag.id = '5A37A9BF-A4CE-47FC-A442-5FFA2564C995'

  --begin tran

  --delete [dbo].[TB_Reg_Flow]
  --where id in 
  --(
  -- select f.id FROM [dbo].[TB_Reg_Flow] f
  --left join [dbo].[TB_Reg_FlowApprove] fa on fa.flowId = f.id
  --left join [dbo].[TB_Reg_ApproveGroup] ag on ag.id = fa.approveGroupId
  --left join [dbo].[TB_Reg_Subject] s on s.id = f.subjectId
  --left join [dbo].[TB_Act_Customers] c on c.id = f.customerId
  --left join [dbo].[TB_Act_ProductCate] pc on pc.id = f.productCatId
  --left join [dbo].[TB_Act_ProductType] pt on pt.id = f.productTypeId
  --left join [dbo].[TB_Reg_FlowLimit] l on l.id = f.flowLimitId
  --where 1=1
  --and f.[createdByUserId] = '70016911'
  --and fa.id is null
  --)

  --commit



  --SELECT fa.id as Reg_flow_aprove_id
	 -- ,f.id as Reg_Flow_Id
	 -- ,f.[subjectId]
	 -- ,s.nameEN as subject_name
  --    ,f.[customerId]
	 -- ,c.cusNameEN as customer_name
  --    ,f.[productCatId]
	 -- ,pc.cateName as product_cat_name
  --    ,f.[productTypeId]
	 -- ,pt.nameEN as prd_type
	 -- ,ag.id as group_id
	 -- ,ag.nameTH as approve_group_nameTh
	 -- ,fa.empId
	 -- ,fa.rangNo
  --    ,f.[flowLimitId]
	 -- ,l.limitBegin
	 -- ,l.limitto
	 -- ,ad.statusId
	 -- ,st.nameTH as approve_status_nameTh
	 -- ,ad.isSendEmail
	 -- ,ad.remark as approve_remark
  --from [dbo].[TB_Reg_Approve] ap
  --left join [dbo].[TB_Reg_ApproveDetail] ad on ad.approveId = ap.flowId
  --left join [dbo].[TB_Reg_Flow] f on f.id = ap.flowId
  --left join [dbo].[TB_Reg_FlowApprove] fa on fa.flowId = f.id
  --left join [dbo].[TB_Reg_ApproveGroup] ag on ag.id = fa.approveGroupId
  --left join [dbo].[TB_Reg_Subject] s on s.id = f.subjectId
  --left join [dbo].[TB_Act_Customers] c on c.id = f.customerId
  --left join [dbo].[TB_Act_ProductCate] pc on pc.id = f.productCatId
  --left join [dbo].[TB_Act_ProductType] pt on pt.id = f.productTypeId
  --left join [dbo].[TB_Reg_FlowLimit] l on l.id = f.flowLimitId
  --left join [dbo].[TB_Reg_ApproveStatus] st on st.id = ad.statusId
  --where 1=1
  --and ap.actFormId = '0DB04C83-CAB6-41A6-8F11-E9D3A36773D4'


  
  ----- new query ----------------------------
     SELECT
	  ap.actFormId as budget_act_id -- change to new table budget_approve_id
	  ,ap.id as approve_id
	  ,ad.id as approve_detail_id
	  ,f.id as Reg_Flow_Id  
	  ,f.[subjectId]
	  ,s.nameEN as subject_name
      ,f.[customerId]
	  ,c.cusNameEN as customer_name
      ,f.[productCatId]
	  ,pc.cateName as product_cat_name
      ,f.[productTypeId]
	  ,pt.nameEN as prd_type
	  --,ag.id as group_id
	  --,ag.nameTH as approve_group_nameTh
	  ,ad.empId
	  ,ad.rangNo
      ,f.[flowLimitId]
	  ,l.limitBegin
	  ,l.limitto
	  ,ad.statusId
	  ,st.nameTH as approve_status_nameTh
	  ,ad.isSendEmail
	  ,ad.remark as approve_remark
	  from [dbo].[TB_Reg_Approve] ap
  left join [dbo].[TB_Reg_ApproveDetail] ad on ad.approveId = ap.Id
  left join [dbo].[TB_Reg_Flow] f on f.id = ap.flowId
  left join [dbo].[TB_Reg_Subject] s on s.id = f.subjectId
  left join [dbo].[TB_Act_Customers] c on c.id = f.customerId
  left join [dbo].[TB_Act_ProductCate] pc on pc.id = f.productCatId
  left join [dbo].[TB_Act_ProductType] pt on pt.id = f.productTypeId
  left join [dbo].[TB_Reg_FlowLimit] l on l.id = f.flowLimitId
  left join [dbo].[TB_Reg_ApproveStatus] st on st.id = ad.statusId
  --left join [dbo].[TB_Reg_FlowApprove] fa on fa.flowId = ap.flowId
 -- left join [dbo].[TB_Reg_ApproveGroup] ag on ag.id = ad.approveGroupId
 where 1=1
  and ap.actFormId = 'A8DC08BC-1A16-4A51-B71F-80A6F69CEA64'
  order by ap.id,ad.rangNo


  
  ----- clear approve detail data ------------------------------------------------------
select *
--  delete 
from [dbo].[TB_Reg_ApproveDetail] where approveId in 
( 
	select id
	from [dbo].[TB_Reg_Approve] 
	where 1=1
	 and flowId in (SELECT [id]
					FROM [EActDB-Dev].[dbo].[TB_Reg_Flow]
					where flowLimitId is not null)
	--and actFormId = '04251A84-3A42-4E66-BD09-5BB667716BC2'
	--and createdByUserId = '70016911'
)order by createdDate desc , rangno asc


select * 
--  delete
from [dbo].[TB_Reg_Approve]  
where  flowId in (SELECT [id]
					FROM [EActDB-Dev].[dbo].[TB_Reg_Flow]
					where flowLimitId is not null)

					update [dbo].[TB_Reg_Approve]   set
					createdByUserId = updatedByUserId
					where updatedByUserId = '11017721'

---------  approve flow --------------

select * from  [TB_Reg_ApproveDetail] order by createdDate desc

rangNo	empId
1	11025960
2	11019809

Plek : 11017721

update [dbo].[TB_Reg_ApproveDetail]  set
empid = '11017721'
where id = 'e4e135ea-7d5f-486d-9166-e404c23fe2e6'

update [dbo].[TB_Reg_ApproveDetail]  set
empid = '11017721'
where id = 'e4d93c92-21bf-4ac4-8a54-0f05481974d8'


update [dbo].[TB_Reg_ApproveDetail]  set
statusId = 2
where id = 'e4e135ea-7d5f-486d-9166-e404c23fe2e6'




------- delete all -------------------
  select * 
  -- delete 
  from [dbo].[TB_Bud_Activity]


  select * 
  ---  delete 
  from [dbo].[TB_Bud_ActivityProduct]
  
  
  select * 
  --   delete
  from [dbo].[TB_Bud_ActivityInvoice]


select * from  [dbo].[TB_Bud_Approve]

select * from  [dbo].[TB_Bud_ApproveInvoice]