using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebLibrary;

namespace eActForm.BusinessLayer
{
	public class ImageAppCodeBudget
	{

		public static List<TB_Bud_Image_Model.BudImageModel> getImageBudgetByApproveId( string budgetApproveId)
		{
			try
			{
				DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetImageByApproveId"
					, new SqlParameter("@budgetApproveId", budgetApproveId)
					//, new SqlParameter("@activityNo", activityNo)
					//, new SqlParameter("@createdByUserId", createdByUserId)
					);
				var lists = (from DataRow d in ds.Tables[0].Rows
							 select new TB_Bud_Image_Model.BudImageModel()
							 {
								 id = d["imageId"].ToString(),

								 invoiceNo = (d["invoiceNo"].ToString() == null || d["invoiceNo"] is DBNull) ? "" : d["invoiceNo"].ToString(),
								 imageType = d["imageType"].ToString(),
								 _image = (d["_image"] == null || d["_image"] is DBNull) ? new byte[0] : (byte[])d["_image"],
								 _fileName = d["_fileName"].ToString(),
								 extension = d["extension"].ToString(),
								 remark = d["remark"].ToString(),
								 delFlag = bool.Parse(d["delFlag"].ToString()),
								 createdDate = DateTime.Parse(d["createdDate"].ToString()),
								 createdByUserId = d["createdByUserId"].ToString(),
								 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
								 updatedByUserId = d["updatedByUserId"].ToString(),
							 });
				return lists.ToList();
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError("getImage getImageBudgetByApproveId => " + ex.Message);
				return new List<TB_Bud_Image_Model.BudImageModel>();
			}
		}


		public static List<TB_Bud_Image_Model.BudImageModel> getImageBudget(string imageId , string imageInvoiceNo, string budgetApproveId, String activityNo, String createdByUserId)
		{
			try
			{
				DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getBudgetImage"
					, new SqlParameter("@imageId", imageId)
					, new SqlParameter("@imageInvoiceNo", imageInvoiceNo)
					, new SqlParameter("@budgetApproveId", budgetApproveId)
					, new SqlParameter("@activityNo", activityNo)
					, new SqlParameter("@createdByUserId", createdByUserId)
					);
				var lists = (from DataRow d in ds.Tables[0].Rows
							 select new TB_Bud_Image_Model.BudImageModel()
							 {
								 id = d["imageId"].ToString(),

								 count_budgetActivityId = int.Parse(d["count_budgetActivityId"].ToString()),
								 count_activityNo = int.Parse(d["count_activityNo"].ToString()),
								 count_budgetApproveId = int.Parse(d["count_budgetApproveId"].ToString()),

								 invoiceNo = (d["invoiceNo"].ToString() == null || d["invoiceNo"] is DBNull) ? "" : d["invoiceNo"].ToString(),
								 imageType = d["imageType"].ToString(),
								 _image = (d["_image"] == null || d["_image"] is DBNull) ? new byte[0] : (byte[])d["_image"],
								 _fileName = d["_fileName"].ToString(),
								 extension = d["extension"].ToString(),
								 remark = d["remark"].ToString(),
								 delFlag = bool.Parse(d["delFlag"].ToString()),
								 createdDate = DateTime.Parse(d["createdDate"].ToString()),
								 createdByUserId = d["createdByUserId"].ToString(),
								 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
								 updatedByUserId = d["updatedByUserId"].ToString(),

								 //budgetActivityId = d["budgetActivityId"].ToString(),
								 //budgetApproveId = d["budgetApproveId"].ToString(),
								 //activityNo = (d["activityNo"].ToString() == null || d["activityNo"] is DBNull) ? "" : d["activityNo"].ToString(),

							 });
				return lists.ToList();
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError("getImage budget => " + ex.Message);
				return new List<TB_Bud_Image_Model.BudImageModel>();
			}
		}

		public static int insertImageBudget(TB_Bud_Image_Model.BudImageModel model)
		{
			int result = 0;
			try
			{
				result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertBudgetImage"
					, new SqlParameter[] {new SqlParameter("@imageType",model.imageType)
					,new SqlParameter("@image",model._image)
					,new SqlParameter("@fileName",model._fileName)
					,new SqlParameter("@extension",model.extension)
					,new SqlParameter("@remark",model.remark)
					,new SqlParameter("@delFlag",model.delFlag)
					,new SqlParameter("@createdDate",model.createdDate)
					,new SqlParameter("@createdByUserId",model.createdByUserId)
					,new SqlParameter("@updatedDate",model.updatedDate)
					,new SqlParameter("@updatedByUserId",model.updatedByUserId)

					});
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError(ex.Message + ">> insertImageBudget");
			}

			return result;
		}

		public static int updateImageBudget(TB_Bud_Image_Model.BudImageModel model)
		{
			int result = 0;
			try
			{
				result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateBudgetImage"
					, new SqlParameter[] {new SqlParameter("@id",model.id)
					,new SqlParameter("@invoiceNo",model.invoiceNo)
					,new SqlParameter("@remark",model.remark)
					,new SqlParameter("@updatedByUserId",model.updatedByUserId)

					});
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError(ex.Message + ">> insertImageBudget");
			}

			return result;
		}

		public static int deleteImageBudgetById(string fileId,string empId)
		{

			int result = 0;
			try
			{
				result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteBudgetImagebyId"
					, new SqlParameter[] {new SqlParameter("@Id",fileId)
					,new SqlParameter("@updatedByUserId",empId)
					});
			}
			catch (Exception ex)
			{
				ExceptionManager.WriteError(ex.Message + ">> deleteImageBudget");
			}

			return result;
		}

	}
}