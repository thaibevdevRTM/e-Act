using eActForm.Presenter.MasterData;
using eForms.Models.MasterData;
using eForms.Presenter.MasterData;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLibrary;
using static eForms.Models.MasterData.ImportBudgetControlModel;

namespace eForms.Presenter.AppCode
{
    public class ImportBudgetControlAppCode
    {
        public static string channel = "channel";
        public static string brand = "brand";


        public static string getLE_No(string strCon)
        {
            string result = "";
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_getLE_No");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new
                             {
                                 LE = d["LE"].ToString(),
                             });
                if (lists.Any())
                {
                    result = lists.FirstOrDefault().LE;
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> getLE_No");
            }

            return result;
        }
        public static int getBudget_No(string strCon)
        {
            int result = 0;
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_getBudget_No");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new
                             {
                                 bgNo = d["budgetNo"].ToString(),
                             });
                if (lists.Any())
                {
                    result = int.Parse(lists.FirstOrDefault().bgNo);
                }
                return result;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> getLE_No");
            }

            return result;
        }
        public static int InsertBudgetControl(string strCon, ImportBudgetControlModel.BudgetControlModels model)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertBudgetControl"
                , new SqlParameter[] {new SqlParameter("@id",model.id)
                         ,new SqlParameter("@budgetNo",model.budgetNo)
                         ,new SqlParameter("@EO",model.EO)
                         ,new SqlParameter("@companyId",model.companyId)
                         ,new SqlParameter("@budgetGroupType",model.budgetGroupType)
                         ,new SqlParameter("@customerId",model.customerId)
                         ,new SqlParameter("@chanelId",model.chanelId)
                         ,new SqlParameter("@brandId",model.brandId)
                         ,new SqlParameter("@startDate",model.startDate)
                         ,new SqlParameter("@endDate",model.endDate)
                         ,new SqlParameter("@amount",model.amount)
                         ,new SqlParameter("@totalChannel",model.totalChannel)
                         ,new SqlParameter("@totalBG",model.totalBG)
                         ,new SqlParameter("@LE",model.LE)
                         ,new SqlParameter("@createdByUserId",model.createdByUserId)
                  });
                result++;


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("InsertBudgetControl => " + ex.Message);
            }

            return result;
        }

        public static int InsertBudgetLE(string strCon, ImportBudgetControlModel.BudgetControl_LEModel model)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertBudgetLE"
                , new SqlParameter[] {new SqlParameter("@id",model.id)
                         ,new SqlParameter("@budgetId",model.budgetId)
                         ,new SqlParameter("@startDate",model.startDate)
                         ,new SqlParameter("@endDate",model.endDate)
                         ,new SqlParameter("@description",model.descripion)
                         ,new SqlParameter("@createdByUserId",model.createdByUserId)
                  });
                result++;


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("InsertBudgetLE => " + ex.Message);
            }

            return result;
        }

        public static int InsertBudgetLE_History(string strCon, DateTime? importDate)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertBudgetLE_History"
              , new SqlParameter[] {new SqlParameter("@importDate",importDate)
                });
                result++;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> InsertBudgetLE_History");
            }

            return result;
        }

        public static int InsertBudgetActType(string strCon, ImportBudgetControlModel.BudgetControl_ActType model)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertBudgetActType"
                , new SqlParameter[] {new SqlParameter("@id",model.id)
                         ,new SqlParameter("@budgetId",model.budgetId)
                         ,new SqlParameter("@budgetLEId",model.budgetLEId)
                         ,new SqlParameter("@actTypeId",model.actTypeId)
                         ,new SqlParameter("@amount",model.amount)
                         ,new SqlParameter("@description",model.description)
                         ,new SqlParameter("@createdByUserId",model.createdByUserId)
                  });
                result++;


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("InsertBudgetActType => " + ex.Message);
            }

            return result;
        }
        public static int InsertBudgetActType_History(string strCon, DateTime? importDate)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertBudgetActType_History"
             , new SqlParameter[] {new SqlParameter("@importDate",importDate)
               });
                result++;

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> InsertBudgetActType_History");
            }

            return result;
        }

        public static string genEO(string strCon, BudgetControlModels modelBudget)
        {
            string result = "";
            try
            {
                var genGroup = "{0}";
                if (!string.IsNullOrEmpty(modelBudget.chanelName))
                {
                    if (modelBudget.chanelName.Equals("ONT"))
                    {
                        genGroup = "B2";
                    }
                    else if (modelBudget.chanelName.Equals("TT") || modelBudget.chanelName.Equals("CVM") || modelBudget.chanelName.Equals("SSC"))
                    {
                        genGroup = "B3";
                    }
                    else if (modelBudget.chanelName.Equals("MT"))
                    {
                        genGroup = "B4";
                    }
                }
                if (!string.IsNullOrEmpty(modelBudget.brandId))
                {
                    result = QueryGetAllBrand.GetAllBrand(strCon).Where(x => x.id.Equals(modelBudget.brandId)).FirstOrDefault().digit_EO;
                }
                result += getDigitDepartment(modelBudget.budgetGroupType);
                result += genGroup; //group
                result += modelBudget.endDate.Value.Year.ToString().Substring(2, 2);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("genEO Presenter => " + ex.Message);
            }
            return result;
        }

        public static string genBudgetNo(string strCon, BudgetControlModels modelBudget, int LE)
        {
            string getCode = "", formatBudgetNo = "{0}-{1}-{2}-";

            try
            {
                getCode = QueryGetAllBrand.GetAllBrand(strCon).Where(x => x.id.Equals(modelBudget.brandId)).FirstOrDefault().brandCode;
                if (!string.IsNullOrEmpty(modelBudget.chanelId))
                {
                    formatBudgetNo = String.Format(formatBudgetNo, "BTL", modelBudget.endDate.Value.Year.ToString().Substring(2, 2), getCode);
                }
                else
                {
                    formatBudgetNo = String.Format(formatBudgetNo, "ATL", modelBudget.endDate.Value.Year.ToString().Substring(2, 2), getCode);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("genBudgetNo Presenter => " + ex.Message);
            }
            return formatBudgetNo;
        }


        public static string getDigitDepartment(string p)
        {
            var result = "";
            if (p == channel)
            {
                result = "11";
            }
            else
            {
                result = "10";
            }
            return result;
        }


        public static int InsertBudgetRpt_Temp(string strCon, BudgetControlModels model)
        {
            int result = 0;
            try
            {


                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertBudgetRpt_Temp"
                      , new SqlParameter[] {new SqlParameter("@EO",model.EO)
                      ,new SqlParameter("@approveNo",model.approveNo)
                      ,new SqlParameter("@orderNo",model.orderNo)
                      ,new SqlParameter("@description",model.description)
                      ,new SqlParameter("@budgetAmount",model.budgetAmount)
                      ,new SqlParameter("@returnAmount",model.returnAmount)
                      ,new SqlParameter("@actual",model.actual)
                      ,new SqlParameter("@accrued",model.accrued)
                      ,new SqlParameter("@commitment",model.commitment)
                      ,new SqlParameter("@prpo_Outstanding",model.PR_PO)
                      ,new SqlParameter("@prepaid",model.prepaid)
                      ,new SqlParameter("@available",model.available)
                      ,new SqlParameter("@replaceEO",model.replaceEO)
                      ,new SqlParameter("@importType",model.typeImport)
                      ,new SqlParameter("@createdByUserId",model.createdByUserId)

                  });
                result++;


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("InsertBudgetRpt_Temp => " + ex.Message);
            }

            return result;
        }

        public static int delBudgetRpt_Temp(string strCon,string EO , string IO)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_delTempBudgetReport"
                     , new SqlParameter[] {new SqlParameter("@EO",EO)
                      ,new SqlParameter("@IO",IO)
                     });

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("delBudgetRpt_Temp => " + ex.Message);
            }

            return result;
        }


        public static string getChannelIdForTxt(string strCon, string p_channelTxt)
        {
            string result = "";
            try
            {
                result = QueryGetAllChanel.getAllChanel(strCon).Where(x => x.cust.Contains(p_channelTxt)).FirstOrDefault().id;
                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string getActivityIdIdForTxt(string strCon, string p_ActTypeTxt)
        {
            string result = "";
            try
            {
                result = QueryGetAllActivityGroup.getAllActivityGroup(strCon).Where(x => x.activityCondition.Equals("bg") && x.activitySales.ToLower().Equals(p_ActTypeTxt.ToLower())).FirstOrDefault().id;
                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }



        public static List<BudgetControlModels> getBudgetList(string strCon)
        {
            List<BudgetControlModels> budgetList = new List<BudgetControlModels>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_getBudgetList");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new BudgetControlModels
                             {
                                 //activityNo = d["activityNo"].ToString(),
                                 date = DateTime.Parse(d["documentDate"].ToString()),
                                 budNum = d["budNum"].ToString(),
                                 b_Code = d["bCode"].ToString(),
                                 brandName = d["bnam_Eng"].ToString(),
                                 type = d["type"].ToString(),
                                 budget_Activity = d["actType"].ToString(),
                                 transaction = d["transactionTxt"].ToString(),
                                 EO = d["EO"].ToString(),
                                 originalBudget = decimal.Parse(d["originalBudget"].ToString()),
                                 LE_Amount = decimal.Parse(d["amountLE"].ToString()),
                                 approve_Amount = decimal.Parse(d["approveAmount"].ToString()),
                                 balanceLEApprove = decimal.Parse(d["balanceLEApprove"].ToString()),
                                 trf_BG = decimal.Parse(d["trf_BG"].ToString()),
                                 actual = decimal.Parse(d["actual"].ToString()),
                                 PR_PO = decimal.Parse(d["pr_po"].ToString()),
                                 actualTotal = decimal.Parse(d["totalActual"].ToString()),
                                 available = decimal.Parse(d["availableAmount"].ToString()),
                                 returnAmount = decimal.Parse(d["returnAmount"].ToString()),
                                 balance = decimal.Parse(d["balanceAmount"].ToString()),
                                 remark = d["remark"].ToString(),
                                 createdByUserId = d["createBy"].ToString(),
                             });

                return lists.ToList(); ;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> getBudgetList");
            }

            return budgetList;
        }

     

        public static int insertDateReportBudgetTBM(string strCon, ImportBudgetControlModel model)
        {
            int result = 0;
            try
            {

                foreach (var item in model.budgetReportList)
                {
                    result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertTo_BGC_Budget_Rpt"
                    , new SqlParameter[] {new SqlParameter("@budNum",item.budNum)
                      ,new SqlParameter("@date",item.date)
                      ,new SqlParameter("@bCode",item.b_Code)
                      ,new SqlParameter("@bNum_En",item.brandName)
                      ,new SqlParameter("@type",item.type)
                      ,new SqlParameter("@bg_Activity",item.budget_Activity)
                      ,new SqlParameter("@transaction",item.transaction)
                      ,new SqlParameter("@EO",item.EO)
                      ,new SqlParameter("@originalBudget",item.originalBudget)
                      ,new SqlParameter("@LE_Amount",item.LE_Amount)
                      ,new SqlParameter("@approve_Amount",item.approve_Amount)
                      ,new SqlParameter("@balanceLEApprove",item.balanceLEApprove)
                      ,new SqlParameter("@trf_BG",item.trf_BG)
                      ,new SqlParameter("@actual",item.actual)
                      ,new SqlParameter("@pr_po",item.PR_PO)
                      ,new SqlParameter("@actualTotal",item.actualTotal)
                      ,new SqlParameter("@available",item.available)
                      ,new SqlParameter("@returnAmount",item.returnAmount)
                      ,new SqlParameter("@balanceAmount",item.balance)
                      ,new SqlParameter("@importType",item.typeImport)
                      ,new SqlParameter("@createdByUserId",item.createdByUserId)

                      });
                }






                return result;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertDateReportBudgetTBM");
            }

            return result;
        }
    }
}
