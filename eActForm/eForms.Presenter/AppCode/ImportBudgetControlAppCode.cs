using eActForm.Presenter.MasterData;
using eForms.Models.MasterData;
using eForms.Presenter.MasterData;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;
using static eForms.Models.MasterData.ImportBudgetControlModel;

namespace eForms.Presenter.AppCode
{
    public class ImportBudgetControlAppCode
    {
        public static string channel = "channel";
        public static string brand = "brand";


        public static string getLE_No(string strCon, int year, string companyId)
        {
            string result = "";
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_getLE_No"
                     , new SqlParameter[] {new SqlParameter("@year",year)
                      ,new SqlParameter("@companyId",companyId)

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

        public static int InsertBudgetControlTemp(string strCon, ImportBudgetControlModel.BudgetControlModels model, string companyId)
        {
            int result = 0;
            string selectStored = "";
            try
            {
                if (companyId == ConfigurationManager.AppSettings["companyId_TBM"].ToString())
                {
                    selectStored = "usp_insertBudgetControlTemp";
                }
                else
                {
                    selectStored = "usp_insertBudgetControlTemp_ActBeer";
                }

                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, selectStored
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

        public static int InsertBudgetLETemp(string strCon, ImportBudgetControlModel.BudgetControl_LEModel model, string companyId)
        {
            int result = 0;
            try
            {
                string selectStored = "";
                if (companyId == ConfigurationManager.AppSettings["companyId_TBM"].ToString())
                {
                    selectStored = "usp_insertBudgetLETemp";
                }
                else
                {
                    selectStored = "usp_insertBudgetLETemp_ActBeer";
                }
                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, selectStored
                , new SqlParameter[] {new SqlParameter("@id",model.id)
                         ,new SqlParameter("@budgetId",model.budgetId)
                         ,new SqlParameter("@startDate",model.startDate)
                         ,new SqlParameter("@endDate",model.endDate)
                         ,new SqlParameter("@description",model.descripion)
                         ,new SqlParameter("@createdByUserId",model.createdByUserId)
                         ,new SqlParameter("@companyId",companyId)
                  });
                result++;


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("InsertBudgetLETemp => " + ex.Message);
            }

            return result;
        }

        public static int InsertBudgetLE_History(string strCon, string companyId)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertBudgetLE_History"
                     , new SqlParameter[] { new SqlParameter("@companyId", companyId)
                     });
                result++;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> InsertBudgetLE_History");
            }

            return result;
        }

        public static int deleteBudgetTemp_ImportBudgetBG(string strCon, string companyId)
        {
            int result = 0;
            string selectStored = "";

            try
            {
                if (companyId == ConfigurationManager.AppSettings["companyId_TBM"].ToString())
                {
                    selectStored = "usp_deleteBudgetTemp";
                }
                else
                {
                    selectStored = "usp_deleteBudgetTempActBeer";
                }


                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, selectStored);
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


        public static List<BudgetControlModels> getBudgetTemp(string strCon, string companyId)
        {
            string selectStored = "";
            try
            {
                if (companyId == ConfigurationManager.AppSettings["companyId_TBM"].ToString())
                {
                    selectStored = "usp_getBudgetTemp";
                }
                else
                {
                    selectStored = "usp_getBudgetTempActBeer";
                }


                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, selectStored);
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


        public static int confirmImportBudget(string strCon, string companyId)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_confirmImportBudgetTemp"
                    , new SqlParameter[] { new SqlParameter("@companyId",companyId)
                    });
                return result;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getBudgetTemp => " + ex.Message);
                return result;
            }

        }

        public static int InsertBudgetActTypeTemp(string strCon, ImportBudgetControlModel.BudgetControl_ActType model, string companyId)
        {
            int result = 0;
            string selectStored = "";
            try
            {
                if (companyId == ConfigurationManager.AppSettings["companyId_TBM"].ToString())
                {
                    selectStored = "usp_insertBudgetActTypeTemp";
                }
                else
                {
                    selectStored = "usp_insertBudgetActTypeTemp_ActBeer";
                }

                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, selectStored
                , new SqlParameter[] {new SqlParameter("@id",model.id)
                         ,new SqlParameter("@budgetId",model.budgetId)
                         ,new SqlParameter("@budgetLEId",model.budgetLEId)
                         ,new SqlParameter("@actTypeId",model.actTypeId)
                         ,new SqlParameter("@amount",model.amount)
                         ,new SqlParameter("@description",model.description)
                         ,new SqlParameter("@createdByUserId",model.createdByUserId)
                         ,new SqlParameter("@companyId",companyId)
                  });
                result++;


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("InsertBudgetActType => " + ex.Message);
            }

            return result;
        }
        public static int InsertBudgetActType_History(string strCon, string companyId)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_insertBudgetActType_History"
                    , new SqlParameter[] { new SqlParameter("@companyId", companyId)
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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

        public static int delBudgetMonthlyByDate(string strCon, ImportBudgetControlModel model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCon, CommandType.StoredProcedure, "usp_delBudgetMonthlyByDate"
                    , new SqlParameter[] {new SqlParameter("@dateActual",model.budgetReportList.FirstOrDefault().date)
                    ,new SqlParameter("@fiscalYear",model.budgetReportList.FirstOrDefault().fiscalYear)
                      });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("delBudgetRpt_Temp => " + ex.Message);
            }

            return result;
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


        public static AjaxResult PrepareData_ImportBudget(string strCon, string ImportBudgetType, DataTable dt, BudgetControlModels model, string getLE, string empId)
        {
            var resultAjax = new AjaxResult();
            string typeImportCatch = "", genId = "", genIdLE = "";
            try
            {
                int rowImport = 1;

                List<BudgetControlModels> budgetList = new List<BudgetControlModels>();
                List<BudgetControl_LEModel> BudgetLEList = new List<BudgetControl_LEModel>();
                List<BudgetControl_ActType> bgActTypeList = new List<BudgetControl_ActType>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["BRAND"].ToString()))
                    {
                        rowImport = rowImport++;

                        BudgetControlModels modelBudget = new BudgetControlModels();
                        genId = Guid.NewGuid().ToString();
                        modelBudget.id = genId;
                        modelBudget.companyId = model.companyId;
                        typeImportCatch = "/// brand : " + dt.Rows[i]["BRAND"].ToString();

                        if (!QueryGetAllBrand.GetAllBrand(strCon).Where(x => x.id.Equals(dt.Rows[i]["brandId"].ToString())).Any())
                        {
                            resultAjax.Message += "brandId is null &&";
                            throw new Exception();
                        }


                        modelBudget.brandId = dt.Rows[i]["brandId"].ToString();
                        modelBudget.budgetGroupType = ImportBudgetType;
                        //modelBudget.chanelName = dt.Rows[i]["channel"].ToString();
                        modelBudget.startDate = MainAppCode.convertStrToDate(model.startDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                        modelBudget.endDate = MainAppCode.convertStrToDate(model.endDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                        modelBudget.createdByUserId = empId;
                        modelBudget.LE = int.Parse(getLE) + 1;
                        if (model.companyId != ConfigurationManager.AppSettings["companyId_TBM"].ToString())
                        {
                            modelBudget.chanelId = dt.Rows[i]["channel"].ToString();
                        }
                        modelBudget.amount = 0;
                        modelBudget.EO = ImportBudgetControlAppCode.genEO(strCon, modelBudget); //genauto
                        budgetList.Add(modelBudget);


                        if (budgetList.Any())
                        {
                            //Add Model LE
                            BudgetControl_LEModel modelLE = new BudgetControl_LEModel();
                            genIdLE = Guid.NewGuid().ToString();
                            modelLE.id = genIdLE;
                            modelLE.budgetId = genId;
                            modelLE.startDate = MainAppCode.convertStrToDate(model.startDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                            modelLE.endDate = MainAppCode.convertStrToDate(model.endDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                            modelLE.descripion = "";
                            modelLE.createdByUserId = empId;
                            BudgetLEList.Add(modelLE);
                        }
                        if (BudgetLEList.Any())
                        {
                            int setIndex = 2;
                            if (model.companyId != ConfigurationManager.AppSettings["companyId_TBM"].ToString())
                            {
                                setIndex = 3;
                            }

                            for (int ii = setIndex; ii < dt.Columns.Count; ii++)
                            {
                                BudgetControl_ActType bgActTypeModel = new BudgetControl_ActType();
                                bgActTypeModel.id = Guid.NewGuid().ToString();
                                bgActTypeModel.budgetId = genId;
                                bgActTypeModel.budgetLEId = genIdLE;
                                if (model.companyId == ConfigurationManager.AppSettings["companyId_TBM"].ToString())
                                {
                                    typeImportCatch += "/// column : " + dt.Columns[ii].ToString();
                                    if (ImportBudgetType == ImportBudgetControlAppCode.brand)
                                    {
                                        bgActTypeModel.actTypeId = QueryGetAllActivityGroup.getAllActivityGroup(strCon).Where(x => x.activityCondition.Equals("bg") && x.activitySales.ToLower().Equals(dt.Columns[ii].ToString().ToLower())).FirstOrDefault().id;
                                    }
                                    else
                                    {
                                        bgActTypeModel.actTypeId = QueryGetAllActivityGroup.getAllActivityGroup(strCon).Where(x => x.activityCondition.Equals("actTbm") && x.activitySales.Substring(0, 2).Equals(dt.Columns[ii].ToString().Substring(0, 2))).FirstOrDefault().id;
                                    }
                                }
                                else
                                {
                                    typeImportCatch += "/// column : " + dt.Columns[ii].ToString();
                                    bgActTypeModel.actTypeId = QueryGetAllActivityGroup.getAllActivityGroup(strCon).Where(x => x.activityCondition.Equals(ConfigurationManager.AppSettings["conditionActBeer"]) && x.activitySales.Substring(0, 2).Equals(dt.Columns[ii].ToString().Substring(0, 2))).FirstOrDefault().id;
                                }
                                bgActTypeModel.amount = dt.Rows[i][dt.Columns[ii].ToString()].ToString() == "" ? 0 : decimal.Parse(checkNullorEmpty(dt.Rows[i][dt.Columns[ii].ToString()].ToString()));
                                bgActTypeModel.description = "";
                                bgActTypeModel.createdByUserId = empId;
                                bgActTypeList.Add(bgActTypeModel);

                                setIndex++;
                            }
                        }
                    }

                }

                if (budgetList.Any())
                {
                    
                    foreach (var item in budgetList)
                    {
                        int result = +ImportBudgetControlAppCode.InsertBudgetControlTemp(strCon, item, model.companyId);
                    }
                }

                if (BudgetLEList.Any())
                {
                    //var delCount = +ImportBudgetControlAppCode.InsertBudgetLE_History(AppCode.StrCon, BudgetLEList.FirstOrDefault().endDate);
                    foreach (var item in BudgetLEList)
                    {
                        int result = +ImportBudgetControlAppCode.InsertBudgetLETemp(strCon, item, model.companyId);
                    }
                }
                if (bgActTypeList.Any())
                {
                    //var delCount = +ImportBudgetControlAppCode.InsertBudgetActType_History(AppCode.StrCon , BudgetLEList.FirstOrDefault().endDate);
                    foreach (var item in bgActTypeList)
                    {
                        int result = +ImportBudgetControlAppCode.InsertBudgetActTypeTemp(strCon, item, model.companyId);
                    }
                }

                resultAjax.Success = true;
                resultAjax.Message = "Type Import Success : " + ImportBudgetType;
            }
            catch(Exception ex)
            {
                resultAjax.Success = false;
                resultAjax.Message = "Type Import : "+ ImportBudgetType + "Error : " + typeImportCatch + "EX : " + ex;
            }

            return resultAjax;
        }

        public static AjaxResult PrepareData_ImportBudgetChannel(string strCon, string ImportBudgetType, DataTable dt, BudgetControlModels model, string getLE, string empId)
        {
            var resultAjax = new AjaxResult();
            string typeImportCatch = "", genId = "", genIdLE = "";
            int rowImport = 0;
            List<BudgetControlModels> budgetList = new List<BudgetControlModels>();
            List<BudgetControl_LEModel> BudgetLEList = new List<BudgetControl_LEModel>();
            List<BudgetControl_ActType> bgActTypeList = new List<BudgetControl_ActType>();
            try
            {
               // var resultAjax_ = ImportBudgetControlAppCode.PrepareData_ImportBudget(strCon, ImportBudgetControlAppCode.channel, dt, model, getLE, empId);
                //resultAjax.Success = resultAjax_.Success;
                //resultAjax.Message = resultAjax_.Message;
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    typeImportCatch = ImportBudgetControlAppCode.channel;
                    rowImport = rowImport++;
                    for (int ii = 2; ii < dt.Columns.Count; ii++)
                    {
                        BudgetControlModels modelBudget = new BudgetControlModels();
                        genId = Guid.NewGuid().ToString();
                        modelBudget.id = genId;
                        modelBudget.companyId = model.companyId;
                        typeImportCatch = "/// brand : " + dt.Rows[i]["BRAND"].ToString();

                        if (!QueryGetAllBrand.GetAllBrand(strCon).Where(x => x.id.Equals(dt.Rows[i]["brandId"].ToString())).Any())
                        {
                            resultAjax.Message += "brandId is null &&";
                            throw new Exception();
                        }


                        modelBudget.brandId = dt.Rows[i]["brandId"].ToString();
                        modelBudget.budgetGroupType = ImportBudgetControlAppCode.channel;
                        modelBudget.amount = decimal.Parse(checkNullorEmpty(dt.Rows[i][dt.Columns[ii].ToString()].ToString()));
                        modelBudget.chanelName = dt.Columns[ii].ToString();
                        modelBudget.startDate = MainAppCode.convertStrToDate(model.startDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                        modelBudget.endDate = MainAppCode.convertStrToDate(model.endDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                        modelBudget.createdByUserId = empId;
                        modelBudget.LE = int.Parse(getLE) + 1;

                        if (model.companyId == ConfigurationManager.AppSettings["companyId_TBM"].ToString())
                        {
                            typeImportCatch += "//// column name : " + dt.Columns[ii].ToString();
                            modelBudget.chanelId = QueryGetAllChanel.getAllChanel(strCon).Where(x => x.cust.Equals(dt.Columns[ii].ToString())).FirstOrDefault().id;
                            modelBudget.EO = ImportBudgetControlAppCode.genEO(strCon, modelBudget); //genauto
                        }
                        else
                        {
                            modelBudget.chanelId = dt.Columns[ii].ToString();
                            modelBudget.EO = ""; //genauto
                        }

                        budgetList.Add(modelBudget);


                        if (budgetList.Any())
                        {
                            //Add Model LE
                            BudgetControl_LEModel modelLE = new BudgetControl_LEModel();
                            genIdLE = Guid.NewGuid().ToString();
                            modelLE.id = genIdLE;
                            modelLE.budgetId = genId;
                            modelLE.startDate = MainAppCode.convertStrToDate(model.startDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                            modelLE.endDate = MainAppCode.convertStrToDate(model.endDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                            modelLE.descripion = "";
                            modelLE.createdByUserId = empId;
                            BudgetLEList.Add(modelLE);
                        }
                        if (BudgetLEList.Any())
                        {
                            BudgetControl_ActType bgActTypeModel = new BudgetControl_ActType();
                            bgActTypeModel.id = Guid.NewGuid().ToString();
                            bgActTypeModel.budgetId = genId;
                            bgActTypeModel.budgetLEId = genIdLE;
                            bgActTypeModel.actTypeId = "";
                            bgActTypeModel.amount = decimal.Parse(checkNullorEmpty(dt.Rows[i][dt.Columns[ii].ToString()].ToString()));
                            bgActTypeModel.createdByUserId = empId;
                            bgActTypeList.Add(bgActTypeModel);
                        }
                    }

                }

                if (budgetList.Any())
                {

                    foreach (var item in budgetList)
                    {
                        int result = +ImportBudgetControlAppCode.InsertBudgetControlTemp(strCon, item, model.companyId);
                    }
                }

                if (BudgetLEList.Any())
                {
                    //var delCount = +ImportBudgetControlAppCode.InsertBudgetLE_History(AppCode.StrCon, BudgetLEList.FirstOrDefault().endDate);
                    foreach (var item in BudgetLEList)
                    {
                        int result = +ImportBudgetControlAppCode.InsertBudgetLETemp(strCon, item, model.companyId);
                    }
                }
                if (bgActTypeList.Any())
                {
                    //var delCount = +ImportBudgetControlAppCode.InsertBudgetActType_History(AppCode.StrCon , BudgetLEList.FirstOrDefault().endDate);
                    foreach (var item in bgActTypeList)
                    {
                        int result = +ImportBudgetControlAppCode.InsertBudgetActTypeTemp(strCon, item, model.companyId);
                    }
                }

                resultAjax.Success = true;
                resultAjax.Message = "Type Import Success : " + ImportBudgetType;
            }
            catch (Exception ex)
            {
                resultAjax.Success = false;
                resultAjax.Message = "Type Import : " + ImportBudgetType + "Error : " + typeImportCatch + "EX : " + ex;
            }

            return resultAjax;
        }


        public static string checkNullorEmpty(string p)
        {
            return p == "" || p == null || p == "0" || p == "0.00" || p == "0.000" || p == "0.0000" || p == "0.00000" ? "0" : p;
        }
    }
}
