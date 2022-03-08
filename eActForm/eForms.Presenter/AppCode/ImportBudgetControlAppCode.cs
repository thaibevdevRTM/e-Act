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


        public static string getLE_No(string strCon,int year)
        {
            string result = "";
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_getLE_No"
                     , new SqlParameter[] {new SqlParameter("@year",year)
                     });
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

        public static int InsertBudgetControlTemp(string strCon, ImportBudgetControlModel.BudgetControlModels model)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertBudgetControlTemp"
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

        public static int InsertBudgetLETemp(string strCon, ImportBudgetControlModel.BudgetControl_LEModel model)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertBudgetLETemp"
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

        public static int InsertBudgetLE_History(string strCon)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertBudgetLE_History");
                result++;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> InsertBudgetLE_History");
            }

            return result;
        }

        public static int deleteBudgetTemp_ImportBudgetBG(string strCon)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_deleteBudgetTemp");
                result++;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> deleteBudgetTemp");
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


        public static List<BudgetControlModels> getBudgetTemp(string strCon)
        {
           
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_getBudgetTemp");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new BudgetControlModels
                             {
                                 startDate = DateTime.Parse(d["startDate"].ToString()),
                                 endDate = DateTime.Parse(d["endDate"].ToString()),
                                 EO = d["EO"].ToString(),
                                 LE = int.Parse(d["LE"].ToString()),
                                 budgetGroupType = d["budgetGroupType"].ToString(),
                                 brandName = d["brandName"].ToString(),
                                 chanelName = d["chanelGroup"].ToString(),
                                 budget_Activity = d["activitySales"].ToString(),
                                 amount = decimal.Parse(d["amount"].ToString()),
                             });
                if (lists.Any())
                {
                    return lists.ToList();
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getBudgetTemp => " + ex.Message);
                return null;
            }

        }


        public static int confirmImportBudget(string strCon)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_confirmImportBudgetTemp");
                return result;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getBudgetTemp => " + ex.Message);
                return result;
            }

        }

        public static int InsertBudgetActTypeTemp(string strCon, ImportBudgetControlModel.BudgetControl_ActType model)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertBudgetActTypeTemp"
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
        public static int InsertBudgetActType_History(string strCon)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertBudgetActType_History");
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
                      ,new SqlParameter("@nonCommitment",model.nonCommitment)
                      ,new SqlParameter("@replaceEO",model.replaceEO)
                      ,new SqlParameter("@fiscalYear",model.fiscalYear)
                      ,new SqlParameter("@importType",model.typeImport)
                      ,new SqlParameter("@dateActual",model.date)
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


        public static int InsertBudgetTemp_Monthly(string strCon, BudgetControlModels model)
        {
            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertBudgetTemp_Monthly"
                      , new SqlParameter[] {new SqlParameter("@EO",model.EO)
                      ,new SqlParameter("@approveNo",model.approveNo)
                      ,new SqlParameter("@orderNo",model.orderNo)
                      ,new SqlParameter("@description",model.description)
                      ,new SqlParameter("@budgetAmount",model.budgetAmount)
                      ,new SqlParameter("@returnAmount",model.returnAmount)
                      ,new SqlParameter("@actual",model.actual)
                      ,new SqlParameter("@accrued",model.accrued)
                      ,new SqlParameter("@commitment",model.commitment)
                      ,new SqlParameter("@nonCommitment",model.nonCommitment)
                      ,new SqlParameter("@replaceEO",model.replaceEO)
                      ,new SqlParameter("@fiscalYear",model.fiscalYear)
                      ,new SqlParameter("@importType",model.typeImport)
                      ,new SqlParameter("@dateActual",model.date)
                      ,new SqlParameter("@createdByUserId",model.createdByUserId)

                  });
                result++;


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("InsertBudgetTemp_Monthly => " + ex.Message);
            }

            return result;
        }

        public static int delBudgetRpt_Temp(string strCon)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_delTempBudgetReport");

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("delBudgetRpt_Temp => " + ex.Message);
            }

            return result;
        }

        public static int delBudgetRptMonthly_Temp(string strCon)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_delTempBudgetReport_Monthly");

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

                                 transaction = d["transactionTxt"].ToString(),
                                 EO = d["EO"].ToString(),
                                 orderNo = d["orderNo"].ToString(),
                                 approve_Amount = decimal.Parse(d["approveAmount"].ToString()),
                                 budget_Amount2 = decimal.Parse(d["approveAmount_Sap"].ToString()),  
                                 replaceEO = d["replaceEO"].ToString(),
                                 actual = decimal.Parse(d["actual"].ToString()),
                                 accrued = decimal.Parse(d["accrued"].ToString()),
                                 commitment = decimal.Parse(d["commitment"].ToString()),
                                 bnamEng = d["bnam_Eng"].ToString(),
                                 returnAmount = decimal.Parse(d["returnAmount"].ToString()),
                                 fiscalYear = d["fiscalYear"].ToString(),
                                 approveNo = d["approveNo"].ToString(),
                                 date = !string.IsNullOrEmpty(d["dateActual"].ToString()) ? DateTime.Parse(d["dateActual"].ToString()) : (DateTime?)null,
                             });

                return lists.ToList(); ;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> getBudgetList");
            }

            return budgetList;
        }


        public static List<BudgetControlModels> getDataSapMonthlyList(string strCon)
        {
            List<BudgetControlModels> budgetList = new List<BudgetControlModels>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_getBudgetMonthlyList");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new BudgetControlModels
                             {

                                 date = !string.IsNullOrEmpty(d["dateActual"].ToString()) ? DateTime.Parse(d["dateActual"].ToString()) : (DateTime?)null,
                                 fiscalYear = d["fiscalYear"].ToString(),
                                 transaction = d["description"].ToString(),
                                 EO = d["EO"].ToString(),
                                 replaceEO = d["replaceEO"].ToString(),
                                 orderNo = d["IO"].ToString(),
                                 approveNo = d["approveNo"].ToString(),
                                 budgetAmount = decimal.Parse(d["budget"].ToString()),
                                 returnAmount = decimal.Parse(d["returnAmount"].ToString()),
                                 brandName = d["brandName"].ToString(),
                                 actual = decimal.Parse(d["actual"].ToString()),
                                 accrued = decimal.Parse(d["accrued"].ToString()),
                                 commitment = decimal.Parse(d["commitment"].ToString()),
                                 nonCommitment = decimal.Parse(d["nonCommitment"].ToString()),

                             });

                return lists.ToList(); ;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> getBudgetList");
            }

            return budgetList;
        }


        public static List<BudgetControlModels> getBudgetReportTBM_NotEO(string strCon)
        {
            List<BudgetControlModels> budgetList = new List<BudgetControlModels>();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_getBudgetReportTBM_NotEO");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new BudgetControlModels
                             {
                                 //activityNo = d["activityNo"].ToString(),

                                 EO = d["EO"].ToString(),
                                 orderNo = d["orderNo"].ToString(),
                                 description = d["description"].ToString(),
                                
                             });

                return lists.ToList(); ;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> getBudgetReportTBM_NotEO");
            }

            return budgetList;
        }



        public static int insertDateReportMonthly_BudgetTBM(string strCon, ImportBudgetControlModel model)
        {
            int result = 0;
            try
            {

                foreach (var item in model.budgetReportList)
                {
                    result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertTo_TB_BGC_BudgetReporMonthly"
                    , new SqlParameter[] {new SqlParameter("@dateActual",item.date)
                      ,new SqlParameter("@fiscalYear",item.fiscalYear)
                      ,new SqlParameter("@description",item.transaction)
                      ,new SqlParameter("@EO",item.EO)
                      ,new SqlParameter("@replaceEO",item.replaceEO)
                      ,new SqlParameter("@IO",item.orderNo)
                      ,new SqlParameter("@approveNo",item.approveNo)
                      ,new SqlParameter("@budget",item.budgetAmount)
                      ,new SqlParameter("@actual",item.actual)
                      ,new SqlParameter("@accrued",item.accrued)
                      ,new SqlParameter("@returnAmount",item.returnAmount)
                      ,new SqlParameter("@commitment",item.commitment)
                      ,new SqlParameter("@nonCommitment",item.nonCommitment)
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
                      ,new SqlParameter("@IO",item.orderNo)
                      ,new SqlParameter("@originalBudget",item.originalBudget)
                      ,new SqlParameter("@LE_Amount",item.LE_Amount)
                      ,new SqlParameter("@approve_Amount",item.approve_Amount)
                      ,new SqlParameter("@balanceLEApprove",item.balanceLEApprove)
                      ,new SqlParameter("@trf_BG",item.trf_BG)
                      ,new SqlParameter("@actual",item.actual)
                      ,new SqlParameter("@accrued",item.accrued)
                      ,new SqlParameter("@pr_po",item.PR_PO)
                      ,new SqlParameter("@actualTotal",item.actualTotal)
                      ,new SqlParameter("@available",item.available)
                      ,new SqlParameter("@returnAmount",item.returnAmount)
                      ,new SqlParameter("@balanceAmount",item.balance)
                      ,new SqlParameter("@importType",item.typeImport)
                      ,new SqlParameter("@createdByUserId",item.createdByUserId)
                      ,new SqlParameter("@fiscalYear",item.fiscalYear)
                      ,new SqlParameter("@approveNo",item.approveNo)
                      ,new SqlParameter("@commitment",item.commitment)
                      ,new SqlParameter("@nonCommitment",item.nonCommitment)
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
