USE [EActDB-DEV]
GO
/****** Object:  StoredProcedure [dbo].[usp_insertBudgetActivityInvoice]    Script Date: 01/04/2019 11:48:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		surachat j
-- Create date: 2019-03-21
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[usp_insertBudgetActivityInvoice]
    @id nvarchar(50)
   ,@activityId  nvarchar(50)
   ,@activityNo  nvarchar(50)
   ,@productId  nvarchar(50)
   ,@activityOfEstimateId nvarchar(50)
   ,@paymentNo  nvarchar(50)
   ,@invoiceBudgetStatusId int
   ,@invoiceNo  nvarchar(50)
   ,@invTotalBath numeric(11,2)
   ,@actionDate datetime
   ,@createdByUserId nvarchar(10)
   ,@updatedByUserId nvarchar(10)
   --,@invoiceSeq numeric(10,0)
   --,@delFlag bit
AS	
BEGIN
   --insert into dbo.TB_Act_ActivityForm_History
   --select * from dbo.TB_Act_ActivityForm where id =  @id
   --delete from dbo.TB_Act_ActivityForm where id =  @id
   
   	----- cal balance & summary invoice-------------------------------------------------------
	DECLARE @var_total_cost_of_activity decimal(12,2);		--ต้นุทนตั้งต้นของ activity
	DECLARE @var_total_cost_of_product decimal(12,2);		--ต้นุทนตั้งต้นของ product ใน activity

	DECLARE @var_total_inv_of_activity decimal(12,2);		--ยอดรวมทุก invoice ของ activity
	DECLARE @var_total_inv_of_product decimal(12,2);		--ยอดรวมทุก invoice ของ product ใน activity

	DECLARE @var_total_balance_of_activity decimal(12,2);	--ยอด balance invoice ของ activity
	DECLARE @var_total_balance_of_product decimal(12,2);	--ยอด balance invoice ของ product ใน activity

	DECLARE @var_balance_of_product decimal(12,2);			--(ผลต่าง-บาท) เมื่อทำการตัดยอด invoice นี้แล้ว balance ของ product จะเหลือเท่าไร
	DECLARE @var_forward_of_product decimal(12,2);			--ยอดยกมาจากรายการก่อนหน้า
	
	
	--ต้นุทนตั้งต้นของ activity
	SELECT @var_total_cost_of_activity = isnull( sum([total]),0) 
	FROM [EActDB-DEV].[dbo].[TB_Act_ActivityOfEstimate]
	WHERE [activityId] = @activityId;

	--ต้นุทนตั้งต้นของ product ใน activity
	SELECT @var_total_cost_of_product = isnull( sum([total]),0) 
	FROM [EActDB-DEV].[dbo].[TB_Act_ActivityOfEstimate]
	WHERE [activityId] = @activityId 
	and [productId] = @productId
	and [id] = @activityOfEstimateId ;

	--ยอดรวมทุก invoice ของ activity
	SELECT @var_total_inv_of_activity = isnull( sum(invTotalBath),0)
	FROM [EActDB-DEV].[dbo].TB_Bud_ActivityInvoice
	WHERE [activityId] = @activityId;

	--ยอดรวมทุก invoice ของ product ใน activity
	SELECT @var_total_inv_of_product = isnull( sum(invTotalBath),0)
	FROM [EActDB-DEV].[dbo].TB_Bud_ActivityInvoice
	WHERE [activityId] = @activityId 
	and [productId] = @productId
	and [activityOfEstimateId] = @activityOfEstimateId ;

	-- balance product = ต้นทุนของ product - (invoice ก่อนหน้านี้ + invoice ตัวนี้)
	select @var_balance_of_product = @var_total_cost_of_product - (@var_total_inv_of_product + @invTotalBath);

	-- ยอดยกมาจากรายการก่อนหน้า
	select @var_forward_of_product = @invTotalBath + @var_balance_of_product;


	----- insert invoice product-----------------------------------------------------------------
	DECLARE @var_invoiceSeq varchar(30);         
       
	SELECT @var_invoiceSeq = isnull(max([invoiceSeq]),0) + 1   -- get next invoice seq 
	FROM TB_Bud_ActivityInvoice        
	WHERE [activityId] = @activityId 
	and [productId] = @productId
	and [activityOfEstimateId] = @activityOfEstimateId;


	DECLARE @var_standBath_of_product decimal(12,2) = 0;		--จำนวนเงินตั้ง

	--select	@var_standBath_of_product = [productForwardBath] 
	--FROM TB_Bud_ActivityInvoice        
	--WHERE [activityId] = @activityId 
	--and [productId] = @productId
	--and invoiceSeq =  @var_invoiceSeq -1;


	

  IF @var_invoiceSeq > 1  
   BEGIN  
       select @var_standBath_of_product  = @var_total_cost_of_product - @var_total_inv_of_product;
   END 
  ELSE
   BEGIN
	   select @var_standBath_of_product  = @var_total_cost_of_product;
   END


	INSERT INTO [dbo].[TB_Bud_ActivityInvoice]
    ([id]
	,[activityId]
	,[productId]
	,[activityOfEstimateId]
	,[paymentNo]
	,[invoiceNo]
	,[invTotalBath]
	,[productCostBath]
	,[productStandBath]
	,[productForwardBath]
	,[productBalanceBath]
	,[actionDate]
	,[invoiceSeq]
	,[invoiceBudgetStatusId]
    ,[delFlag]
    ,[createdDate]
    ,[createdByUserId]
    ,[updatedDate]
    ,[updatedByUserId]
    )VALUES ( 
	@id
	,@activityId
	,@productId
	,@activityOfEstimateId
	,@paymentNo
	,@invoiceNo
	,@invTotalBath
	,@var_total_cost_of_product
	,@var_standBath_of_product
	,@var_forward_of_product
	,@var_balance_of_product
	,@actionDate
	,@var_invoiceSeq 
	,@invoiceBudgetStatusId
	,0
	,getdate()
	,@createdByUserId
	,getdate()  
	,@updatedByUserId
	);
	

	----- insert activity status -------------------------------------------------------------
	DECLARE @var_count_row int;

	select @var_count_row = isnull(count(*),0)
	from [dbo].TB_Bud_Activity
	where [activityId]= @activityId ;

	IF (@var_count_row = 0)
	BEGIN
		insert into [dbo].[TB_Bud_Activity] 
		(
		 [id]
		,[activityId]
		,[activityNo]
		,[budgetActivityStatusId]
		,[delFlag]
		,[createdDate]
		,[createdByUserId]
		,[updatedDate]
		,[updatedByUserId]
		)values(
		@id
		,@activityId
		,@activityNo
		,2
		,0
		,getdate()
		,@createdByUserId
		,getdate()  
		,@updatedByUserId
		)
	END
	ELSE
	BEGIN
		update [dbo].[TB_Bud_Activity] set
		 [budgetActivityStatusId] = 2
		,[updatedDate] = getdate()
		,[updatedByUserId] = @updatedByUserId
		where [activityId]= @activityId
	END;



	----- insert product status  -------------------------------------------------------------

	select @var_count_row = isnull(count(*),0)
	from [dbo].TB_Bud_ActivityProduct
	where [activityId]= @activityId 
	and [productId] = @productId;


	IF (@var_count_row = 0)
	BEGIN
		insert into [dbo].[TB_Bud_ActivityProduct] 
		(
		 [id]
		,[activityId]
		,[productId]
		,[activityOfEstimateId]
		,[budgetStatusId]
		--,[productSeq] ยังไม่ได้ใช้
		,[delFlag]
		,[createdDate]
		,[createdByUserId]
		,[updatedDate]
		,[updatedByUserId]
		)values(
		 @id
		,@activityId
		,@productId
		,@activityOfEstimateId
		,@invoiceBudgetStatusId
		--,null [productSeq] ยังไม่ได้ใช้
		,0
		,getdate()
		,@createdByUserId
		,getdate()  
		,@updatedByUserId
		) 
	END
	ELSE
	BEGIN
		update [dbo].[TB_Bud_ActivityProduct] set
		 [budgetStatusId] = @invoiceBudgetStatusId
		,[updatedDate] = getdate()
		,[updatedByUserId] = @updatedByUserId
		where [activityId]= @activityId
		and [productId] = @productId
		and [activityOfEstimateId] = @activityOfEstimateId
	END;

	-- if status == จบรายการ ให้ new record เพื่อจบรายการ 1 record
	--if @invoiceBudgetStatusId = 3 
	--BEGIN
	--	INSERT INTO [dbo].[TB_Bud_ActivityInvoice]
	--	([id]
	--	,[activityId]
	--	,[productId]
	--	,[activityOfEstimateId]
	--	,[paymentNo]
	--	,[invoiceNo]
	--	,[invTotalBath]
	--	,[productCostBath]
	--	,[productStandBath]
	--	,[productForwardBath]
	--	,[productBalanceBath]
	--	,[actionDate]
	--	,[invoiceSeq]
	--	,[invoiceBudgetStatusId]
	--	,[delFlag]
	--	,[createdDate]
	--	,[createdByUserId]
	--	,[updatedDate]
	--	,[updatedByUserId]
	--	)VALUES ( 
	--	@id
	--	,@activityId
	--	,@productId
	--	,@activityOfEstimateId
	--	,@paymentNo
	--	,'inv-close'
	--	,0
	--	,@var_total_cost_of_product
	--	,@var_standBath_of_product
	--	,@var_forward_of_product
	--	,@var_balance_of_product
	--	,@actionDate
	--	,@var_invoiceSeq + 1 
	--	,@invoiceBudgetStatusId xxxxxxxxxxxxxxxx
	--	,0
	--	,getdate()
	--	,@createdByUserId
	--	,getdate()  
	--	,@updatedByUserId
	--	);
	--END

END

-- execute usp_insertBudgetActivityInvoice 'asxcsdasdasdgjj','aaaa','activityNo','productId','paymentNo','inv_no',150.50,'03/21/2019','70016911','70016911'



-- note ---------------------------------------------------------------
-- delete invoice อย่าลืม update status ที่ product ถ้า invoice นั้นเป็น จบรายการ