using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;
using static eActForm.Models.ReportSummaryModels;

namespace eActForm.BusinessLayer
{
    public class ReportSummaryAppCode
    {

        public static List<CompanyMTM> getCompanyMTMList()
        {
            try
            {
                string rtn = ConfigurationManager.AppSettings["companyMTM"];//File.ReadAllText(path);
                JObject json = JObject.Parse(rtn);
                var lists = JsonConvert.DeserializeObject<List<CompanyMTM>>(json.SelectToken("companyMTM").ToString());
                return lists;

            }
            catch (Exception ex)
            {
                throw new Exception("getCompanyMTMList >>" + ex.Message);
            }
        }

        public static int updateSummaryReportWithApproveDetail(string repDetailId)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateStatusActSummaryDetailByApproveSummaryDetail"
                    , new SqlParameter[] { new SqlParameter("@repDetailId", repDetailId)
                    ,new SqlParameter("@updateDate",DateTime.Now)
                    ,new SqlParameter("@updateBy",UtilsAppCode.Session.User.empId)});
            }
            catch (Exception ex)
            {
                throw new Exception("updateSummaryReportWithApproveDetail >> " + ex.Message);
            }
        }
        public static int updateSummaryReportWithApproveReject(string repDetailId)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateStatusSummaryDetailByApproveReject"
                    , new SqlParameter[] { new SqlParameter("@repDetailId", repDetailId)
                    ,new SqlParameter("@updateDate",DateTime.Now)
                    ,new SqlParameter("@updateBy",UtilsAppCode.Session.User.empId)});
            }
            catch (Exception ex)
            {
                throw new Exception("updateSummaryReportWithApproveReject >> " + ex.Message);
            }
        }

        public static ApproveFlowModel.approveFlowModel getFlowByActFormId(string actId)
        {
            try
            {
                ApproveFlowModel.approveFlowModel model = new ApproveFlowModel.approveFlowModel();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getFlowMainByActFormId"
                    , new SqlParameter[] { new SqlParameter("@actFormId", actId) });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveFlowModel.flowApprove()
                             {
                                 id = dr["id"].ToString(),
                                 flowNameTH = dr["flowNameTH"].ToString(),
                                 cusNameTH = dr["cusNameTH"].ToString(),
                                 cusNameEN = dr["cusNameEN"].ToString(),
                                 nameTH = dr["nameTH"].ToString(),
                             }).ToList();
                model.flowMain = lists[0];
                model.flowDetail = getFlowDetailWithApproveSummaryDetail(model.flowMain.id, actId);
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlowByActFormId >>" + ex.Message);
            }
        }


        public static List<ApproveFlowModel.flowApproveDetail> getFlowDetailWithApproveSummaryDetail(string flowId, string actId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getFlowApproveWithStatusDetail"
                    , new SqlParameter[] {
                        new SqlParameter("@flowId", flowId)
                        ,new SqlParameter("@actFormId", actId)
                    });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveFlowModel.flowApproveDetail(dr["empId"].ToString())
                             {
                                 id = dr["id"].ToString(),
                                 rangNo = (int)dr["rangNo"],
                                 empId = dr["empId"].ToString(),
                                 empEmail = dr["empEmail"].ToString(),
                                 empFNameTH = dr["empFNameTH"].ToString(),
                                 empLNameTH = dr["empLNameTH"].ToString(),
                                 //empPositionTitleTH = dr["empPositionTitleTH"].ToString(),
                                 approveGroupName = dr["approveGroupName"].ToString(),
                                 approveGroupNameEN = dr["approveGroupNameEN"].ToString(),
                                 isShowInDoc = (bool)dr["showInDoc"],
                                 empGroup = dr["empGroup"].ToString(),
                                 statusId = dr["statusId"].ToString(),
                                 remark = dr["remark"].ToString(),
                                 imgSignature = string.Format(ConfigurationManager.AppSettings["rootgetSignaURL"], dr["empId"].ToString()),
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlowDetailWithApproveDetail >>" + ex.Message);
            }
        }

        public static List<ReportSummaryModels.ReportSummaryModel> getSummaryDetailReportByDate(string startDate, string endDate)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getDetailSummarybyDate"
                      , new SqlParameter[] {
                        new SqlParameter("@startDate",DateTime.ParseExact(startDate,"MM/dd/yyyy",null))
                        ,new SqlParameter("@endDate",DateTime.ParseExact(endDate,"MM/dd/yyyy",null))
                    });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ReportSummaryModels.ReportSummaryModel()
                             {
                                 id = Guid.NewGuid().ToString(),
                                 productType = dr["productTypeTH"].ToString(),
                                 productTypeId = dr["productTypeId"].ToString(),
                                 repDetailId = dr["repDetailId"].ToString(),
                                 activityNo = dr["activityNo"].ToString(),
                                 customerId = dr["customerId"].ToString(),
                                 customerName = dr["cusNameTH"].ToString(),
                                 createdDate = (DateTime?)dr["createdDate"],
                                 companyId = dr["companyId"].ToString()
                             });

                return lists.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getSummaryDetailReportByDate >>" + ex.Message);
            }
        }

        public static ReportSummaryModels getReportSummaryApprove(string summaryId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getReportSummaryApprove"
                     , new SqlParameter[] {
                        new SqlParameter("@summaryId",summaryId)
                    });

                var list = (from DataRow d in ds.Tables[0].Rows
                            select new ReportSummaryModel()
                            {
                                id = Guid.NewGuid().ToString(),
                                customerName = d["customerName"].ToString(),
                                activitySales = d["activitySales"].ToString(),
                                activityId = d["actid"].ToString(),
                                repDetailId = d["repDetailId"].ToString(),
                                est = decimal.Parse(AppCode.checkNullorEmpty(d["est"].ToString())),
                                crystal = decimal.Parse(AppCode.checkNullorEmpty(d["crystal"].ToString())),
                                wranger = decimal.Parse(AppCode.checkNullorEmpty(d["wranger"].ToString())),
                                plus100 = decimal.Parse(AppCode.checkNullorEmpty(d["100plus"].ToString())),
                                jubjai = decimal.Parse(AppCode.checkNullorEmpty(d["jubjai"].ToString())),
                                oishi = decimal.Parse(AppCode.checkNullorEmpty(d["oishi"].ToString())),
                                soda = decimal.Parse(AppCode.checkNullorEmpty(d["soda"].ToString())),
                                water = decimal.Parse(AppCode.checkNullorEmpty(d["water"].ToString())),
                                createdDate = (DateTime?)d["createdDate"],
                            });
                List<ReportSummaryModel> groupList = new List<ReportSummaryModel>();
                groupList = list
                    .OrderBy(x => x.customerName)
                    .GroupBy(g => new { g.customerName, g.activitySales })
                    .Select((group, index) => new ReportSummaryModel
                    {
                        id = Guid.NewGuid().ToString(),
                        customerName = group.First().customerName,
                        activitySales = group.First().activitySales,
                        est = group.Sum(s => s.est),
                        crystal = group.Sum(s => s.crystal),
                        wranger = group.Sum(s => s.wranger),
                        plus100 = group.Sum(s => s.plus100),
                        jubjai = group.Sum(s => s.jubjai),
                        oishi = group.Sum(s => s.oishi),
                        soda = group.Sum(s => s.soda),
                        water = group.Sum(s => s.water),
                        createdDate = group.First().createdDate,
                    }).ToList();


                List<ReportSummaryModel> groupActivityList = new List<ReportSummaryModel>();
                groupActivityList = list
                    .GroupBy(g => new { g.activitySales })
                    .Select((group, index) => new ReportSummaryModel
                    {
                        id = Guid.NewGuid().ToString(),
                        activitySales = group.First().activitySales,
                        est = group.Sum(s => s.est),
                        crystal = group.Sum(s => s.crystal),
                        wranger = group.Sum(s => s.wranger),
                        plus100 = group.Sum(s => s.plus100),
                        jubjai = group.Sum(s => s.jubjai),
                        oishi = group.Sum(s => s.oishi),
                        soda = group.Sum(s => s.soda),
                        water = group.Sum(s => s.water),
                        createdDate = group.First().createdDate,
                    }).ToList();


                ReportSummaryModels resultModel = new ReportSummaryModels();
                resultModel.activitySummaryGroupList = groupList.OrderBy(x => x.customerName).ToList();
                resultModel.activitySummaryList = list.ToList();
                resultModel.activitySummaryGroupActivityList = groupActivityList;
                return resultModel;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getReportSummary => " + ex.Message);
                return new ReportSummaryModels();
            }
        }

        public static ReportSummaryModels getReportSummary(string repId, string txtDate)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getReportSummary"
                     , new SqlParameter[] {
                        new SqlParameter("@repId",repId)
                    });

                var list = (from DataRow d in ds.Tables[0].Rows
                            select new ReportSummaryModel()
                            {
                                id = Guid.NewGuid().ToString(),
                                productTypeId = AppCode.nonAL,
                                customerName = d["customerName"].ToString(),
                                activitySales = d["activitySales"].ToString(),
                                activityId = d["actid"].ToString(),
                                repDetailId = d["repDetailId"].ToString(),
                                est = decimal.Parse(AppCode.checkNullorEmpty(d["est"].ToString())),
                                crystal = decimal.Parse(AppCode.checkNullorEmpty(d["crystal"].ToString())),
                                wranger = decimal.Parse(AppCode.checkNullorEmpty(d["wranger"].ToString())),
                                plus100 = decimal.Parse(AppCode.checkNullorEmpty(d["100plus"].ToString())),
                                jubjai = decimal.Parse(AppCode.checkNullorEmpty(d["jubjai"].ToString())),
                                oishi = decimal.Parse(AppCode.checkNullorEmpty(d["oishi"].ToString())),
                                soda = decimal.Parse(AppCode.checkNullorEmpty(d["soda"].ToString())),
                                water = decimal.Parse(AppCode.checkNullorEmpty(d["water"].ToString())),
                            }).OrderBy(x => x.activitySales).ToList();
                List<ReportSummaryModel> groupList = new List<ReportSummaryModel>();
                groupList = list
                    .OrderByDescending(x => x.rowNo)
                    .GroupBy(g => new { g.customerName, g.activitySales })
                    .Select((group, index) => new ReportSummaryModel
                    {
                        id = Guid.NewGuid().ToString(),
                        customerName = group.First().customerName,
                        activitySales = group.First().activitySales,
                        est = group.Sum(s => s.est),
                        crystal = group.Sum(s => s.crystal),
                        wranger = group.Sum(s => s.wranger),
                        plus100 = group.Sum(s => s.plus100),
                        jubjai = group.Sum(s => s.jubjai),
                        oishi = group.Sum(s => s.oishi),
                        soda = group.Sum(s => s.soda),
                        water = group.Sum(s => s.water),
                    }).ToList();


                List<ReportSummaryModel> groupActivityList = new List<ReportSummaryModel>();
                groupActivityList = list
                    .GroupBy(g => new { g.activitySales })
                    .Select((group, index) => new ReportSummaryModel
                    {
                        id = Guid.NewGuid().ToString(),
                        activitySales = group.First().activitySales,
                        est = group.Sum(s => s.est),
                        crystal = group.Sum(s => s.crystal),
                        wranger = group.Sum(s => s.wranger),
                        plus100 = group.Sum(s => s.plus100),
                        jubjai = group.Sum(s => s.jubjai),
                        oishi = group.Sum(s => s.oishi),
                        soda = group.Sum(s => s.soda),
                        water = group.Sum(s => s.water),
                    }).ToList();



                string txtMonth = DateTime.ParseExact(txtDate, "MM/dd/yyyy", null).ToString("MMM").ToLower();
                string txtYear = DateTime.ParseExact(txtDate, "MM/dd/yyyy", null).ToString("yyyy");
                DataSet ds1 = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_get_SaleForcast"
                    , new SqlParameter[] {
                        new SqlParameter("@year",txtYear)
                    });
                var listForSale = (from DataRow d in ds1.Tables[0].Rows
                                   select new ReportSummaryModel()
                                   {
                                       id = d["id"].ToString(),
                                       brandId = d["brandId"].ToString(),
                                       month = decimal.Parse(AppCode.checkNullorEmpty(d[txtMonth].ToString())),

                                   }).OrderBy(x => x.activitySales).ToList();



                //add Sales Forecast
                ReportSummaryModels resultModel = new ReportSummaryModels();
                ReportSummaryModel forecastlist = new ReportSummaryModel();

                if (listForSale != null && listForSale.Count > 0)
                {
                    forecastlist = (new ReportSummaryModel
                    {
                        id = Guid.NewGuid().ToString(),
                        activitySales = "Sales Forecast",
                        est = listForSale.Where(x => x.brandId.Equals("9EC1CA68-591D-4B3A-9A65-6509C6ED965E")).FirstOrDefault().month,
                        crystal = listForSale.Where(x => x.brandId.Equals("2395EA4D-5CD5-4DDB-A7B6-48EF819B99BB")).FirstOrDefault().month,
                        wranger = listForSale.Where(x => x.brandId.Equals("BB57DBF4-C281-4F79-9481-2B8A4C53C723")).FirstOrDefault().month,
                        plus100 = listForSale.Where(x => x.brandId.Equals("6B623E91-AE6E-4621-A6CD-3F567D19BC70")).FirstOrDefault().month,
                        jubjai = listForSale.Where(x => x.brandId.Equals("32459970-AACE-4E67-B58B-F6D786F8D7A1")).FirstOrDefault().month,
                        oishi = listForSale.Where(x => x.brandId.Equals("1D8F1409-9A19-46AC-B1D3-D71919351716")).FirstOrDefault().month,
                        soda = listForSale.Where(x => x.brandId.Equals("BC05AADC-A306-4D33-8383-521B8CAB2B2F")).FirstOrDefault().month +
                              listForSale.Where(x => x.brandId.Equals("7CA5340A-747B-486C-81C5-D206B081D96A")).FirstOrDefault().month,
                        water = listForSale.Where(x => x.brandId.Equals("3B936397-55EC-475B-9441-5BE7DE1F80F5")).FirstOrDefault().month,
                    });

                }
                resultModel.activitySummaryGroupList = groupList.OrderBy(x => x.customerName).ToList();
                resultModel.activitySummaryList = list.ToList();
                resultModel.activitySummaryGroupActivityList = groupActivityList;
                resultModel.activitySummaryForecastList.Add(forecastlist);

                return resultModel;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getReportSummary => " + ex.Message);
                return new ReportSummaryModels();
            }
        }


        public static ReportSummaryModels getReportSummaryAlcohol(string repId, string txtDate)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getReportSummaryAlcohol"
                     , new SqlParameter[] {
                        new SqlParameter("@repId",repId)
                    });

                var list = (from DataRow d in ds.Tables[0].Rows
                            select new ReportSummaryModel()
                            {
                                id = Guid.NewGuid().ToString(),
                                productTypeId = AppCode.AL,
                                customerName = d["customerName"].ToString(),
                                activitySales = d["activitySales"].ToString(),
                                activityId = d["actid"].ToString(),
                                repDetailId = d["repDetailId"].ToString(),
                                beer = decimal.Parse(AppCode.checkNullorEmpty(d["beer"].ToString())),
                                changclassic = decimal.Parse(AppCode.checkNullorEmpty(d["changclassic"].ToString())),
                                federbrau = decimal.Parse(AppCode.checkNullorEmpty(d["federbrau"].ToString())),
                                archa = decimal.Parse(AppCode.checkNullorEmpty(d["archa"].ToString())),
                                whitespirits = decimal.Parse(AppCode.checkNullorEmpty(d["whitespirits"].ToString())),
                                brownspirits = decimal.Parse(AppCode.checkNullorEmpty(d["brownspirits"].ToString())),
                                hongthong = decimal.Parse(AppCode.checkNullorEmpty(d["hongthong"].ToString())),
                                blend285 = decimal.Parse(AppCode.checkNullorEmpty(d["blend285"].ToString())),
                                sangsom = decimal.Parse(AppCode.checkNullorEmpty(d["sangsom"].ToString())),
                                readytodrink = decimal.Parse(AppCode.checkNullorEmpty(d["readytodrink"].ToString())),
                            }).OrderBy(x => x.activitySales).ToList();
                List<ReportSummaryModel> groupList = new List<ReportSummaryModel>();
                groupList = list
                    .OrderByDescending(x => x.rowNo)
                    .GroupBy(g => new { g.customerName, g.activitySales })
                    .Select((group, index) => new ReportSummaryModel
                    {
                        id = Guid.NewGuid().ToString(),
                        customerName = group.First().customerName,
                        activitySales = group.First().activitySales,
                        beer = group.Sum(s => s.beer),
                        changclassic = group.Sum(s => s.changclassic),
                        federbrau = group.Sum(s => s.federbrau),
                        archa = group.Sum(s => s.archa),
                        whitespirits = group.Sum(s => s.whitespirits),
                        brownspirits = group.Sum(s => s.brownspirits),
                        hongthong = group.Sum(s => s.hongthong),
                        blend285 = group.Sum(s => s.blend285),
                        sangsom = group.Sum(s => s.sangsom),
                        readytodrink = group.Sum(s => s.readytodrink),
                    }).ToList();


                List<ReportSummaryModel> groupActivityList = new List<ReportSummaryModel>();
                groupActivityList = list
                    .GroupBy(g => new { g.activitySales })
                    .Select((group, index) => new ReportSummaryModel
                    {
                        id = Guid.NewGuid().ToString(),
                        activitySales = group.First().activitySales,
                        beer = group.Sum(s => s.beer),
                        changclassic = group.Sum(s => s.changclassic),
                        federbrau = group.Sum(s => s.federbrau),
                        archa = group.Sum(s => s.archa),
                        whitespirits = group.Sum(s => s.whitespirits),
                        brownspirits = group.Sum(s => s.brownspirits),
                        hongthong = group.Sum(s => s.hongthong),
                        blend285 = group.Sum(s => s.blend285),
                        sangsom = group.Sum(s => s.sangsom),
                        readytodrink = group.Sum(s => s.readytodrink),
                    }).ToList();



                string txtMonth = DateTime.ParseExact(txtDate, "MM/dd/yyyy", null).ToString("MMM").ToLower();
                string txtYear = DateTime.ParseExact(txtDate, "MM/dd/yyyy", null).ToString("yyyy");
                DataSet ds1 = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_get_SaleForcast"
                    , new SqlParameter[] {
                        new SqlParameter("@year",txtYear)
                    });
                var listForSale = (from DataRow d in ds1.Tables[0].Rows
                                   select new ReportSummaryModel()
                                   {
                                       id = d["id"].ToString(),
                                       brandId = d["brandId"].ToString(),
                                       month = decimal.Parse(AppCode.checkNullorEmpty(d[txtMonth].ToString())),

                                   }).OrderBy(x => x.activitySales).ToList();



                //add Sales Forecast
                ReportSummaryModels resultModel = new ReportSummaryModels();
                ReportSummaryModel forecastlist = new ReportSummaryModel();
                if (listForSale != null && listForSale.Count > 0)
                {
                    forecastlist = (new ReportSummaryModel
                    {
                        id = Guid.NewGuid().ToString(),
                        activitySales = "Sales Forecast",
                        est = listForSale.Where(x => x.brandId.Equals("9EC1CA68-591D-4B3A-9A65-6509C6ED965E")).FirstOrDefault().month,
                        crystal = listForSale.Where(x => x.brandId.Equals("2395EA4D-5CD5-4DDB-A7B6-48EF819B99BB")).FirstOrDefault().month,
                        wranger = listForSale.Where(x => x.brandId.Equals("BB57DBF4-C281-4F79-9481-2B8A4C53C723")).FirstOrDefault().month,
                        plus100 = listForSale.Where(x => x.brandId.Equals("6B623E91-AE6E-4621-A6CD-3F567D19BC70")).FirstOrDefault().month,
                        jubjai = listForSale.Where(x => x.brandId.Equals("32459970-AACE-4E67-B58B-F6D786F8D7A1")).FirstOrDefault().month,
                        oishi = listForSale.Where(x => x.brandId.Equals("1D8F1409-9A19-46AC-B1D3-D71919351716")).FirstOrDefault().month,
                        soda = listForSale.Where(x => x.brandId.Equals("BC05AADC-A306-4D33-8383-521B8CAB2B2F")).FirstOrDefault().month +
                              listForSale.Where(x => x.brandId.Equals("7CA5340A-747B-486C-81C5-D206B081D96A")).FirstOrDefault().month,
                        water = listForSale.Where(x => x.brandId.Equals("3B936397-55EC-475B-9441-5BE7DE1F80F5")).FirstOrDefault().month,
                    });

                }
                else
                {

                }

                groupList.Add(forecastlist);
                resultModel.activitySummaryForecastList.Add(forecastlist);
                resultModel.activitySummaryGroupList = groupList;
                resultModel.activitySummaryList = list.ToList();
                resultModel.activitySummaryGroupActivityList = groupActivityList;
                return resultModel;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getReportSummary => " + ex.Message);
                return new ReportSummaryModels();
            }
        }

        public static List<ReportSummaryModels.ReportSummaryModel> getFilterSummaryDetailByProductType(List<ReportSummaryModels.ReportSummaryModel> lists, string producttypeId, string txtCompanyId)
        {
            try
            {
                return lists.Where(r => r.productTypeId == producttypeId && r.companyId.Equals(txtCompanyId)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterSummaryDetailByProductType >>" + ex.Message);
            }
        }

        public static List<ReportSummaryModels.ReportSummaryModel> getFilterSummaryDetailByRepDetailNo(List<ReportSummaryModels.ReportSummaryModel> lists, string txtRepDetailNo, string txtCompanyId)
        {
            try
            {
                return lists.Where(r => r.activityNo.Contains(txtRepDetailNo) && r.companyId.Equals(txtCompanyId)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterSummaryDetailByRepDetailNo >>" + ex.Message);
            }
        }



        public static List<ReportSummaryModels.ReportSummaryModel> getFilterSummaryDetailByCustomer(List<ReportSummaryModels.ReportSummaryModel> lists, string cusId)
        {
            try
            {
                return lists.Where(r => r.customerId == cusId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterSummaryDetailByCustomer >>" + ex.Message);
            }
        }


        public static List<ReportSummaryModels.actApproveSummaryDetailModel> getFilterFormByStatusId(List<ReportSummaryModels.actApproveSummaryDetailModel> lists, int statusId)
        {
            try
            {
                return lists.Where(r => r.statusId == statusId.ToString()).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("getFilterFormByStatusId >> " + ex.Message);
            }
        }

        public static string insertActivitySummaryDetail(string customerId, string productTypeId, string startDate, string endDate, ReportSummaryModels model)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                string docNo = string.Format("{0:0000}", int.Parse(ActivityFormCommandHandler.getActivityDoc("SummaryDetail", "", "").FirstOrDefault().docNo));
                int rtn = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertSummaryDetail"
                    , new SqlParameter[] {
                        new SqlParameter("@id",id)
                        ,new SqlParameter("@activityNo",docNo)
                        ,new SqlParameter("@statusId",(int)AppCode.ApproveStatus.รออนุมัติ)
                        ,new SqlParameter("@startDate",DateTime.ParseExact(startDate,"MM/dd/yyyy",null))
                        ,new SqlParameter("@endDate",DateTime.ParseExact(endDate,"MM/dd/yyyy",null))
                        ,new SqlParameter("@customerId",customerId)
                        ,new SqlParameter("@productTypeId",productTypeId)
                        ,new SqlParameter("@delFlag",false)
                        ,new SqlParameter("@createdDate",DateTime.Now)
                        ,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
                        ,new SqlParameter("@updatedDate",DateTime.Now)
                        ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                    });

                if (rtn > 0)
                {
                    string actIdTemp = "";
                    foreach (var item in model.activitySummaryList)
                    {
                        if (actIdTemp != item.id)
                        {
                            SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertSummaryMapForm"
                            , new SqlParameter[] {
                                new SqlParameter("@id",Guid.NewGuid().ToString())
                                ,new SqlParameter("@repDetailId",item.repDetailId)
                                ,new SqlParameter("@summaryId",id)
                                ,new SqlParameter("@delFlag",false)
                                ,new SqlParameter("@createdDate",DateTime.Now)
                                ,new SqlParameter("@createdByUserId",UtilsAppCode.Session.User.empId)
                                ,new SqlParameter("@updatedDate",DateTime.Now)
                                ,new SqlParameter("@updatedByUserId",UtilsAppCode.Session.User.empId)
                            });
                        }
                        actIdTemp = item.id;
                    }
                }
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception("insertActivitySummaryDetail >>" + ex.Message);
            }
        }


        public static int insertApproveForReportSummaryDetail(string subId, string customerId, string productTypeId, string summaryId)
        {
            try
            {
                int rtn = 0;
                ApproveFlowModel.approveFlowModel flowModel = ApproveFlowAppCode.getFlowForReportDetail(
                    subId
                    , customerId
                    , productTypeId);
                if (ApproveAppCode.insertApproveByFlow(flowModel, summaryId) > 0)
                {
                    rtn = ApproveAppCode.updateApproveWaitingByRangNo(summaryId);
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("insertApproveForReportSummaryDetail >>" + ex.Message);
            }
        }

        public static List<ReportSummaryModels.actApproveSummaryDetailModel> getApproveSummaryDetailListsByEmpId()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getApproveSummaryDetailFormByEmpId"
                    , new SqlParameter[] { new SqlParameter("@empId", UtilsAppCode.Session.User.empId) });
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var lists = (from DataRow dr in ds.Tables[0].Rows
                                 select new ReportSummaryModels.actApproveSummaryDetailModel(dr["createdByUserId"].ToString())
                                 {
                                     id = dr["id"].ToString(),
                                     statusId = dr["statusId"].ToString(),
                                     statusName = dr["statusName"].ToString(),
                                     productTypeName = dr["productTypeName"].ToString(),
                                     startDate = dr["startDate"] is DBNull ? null : (DateTime?)dr["startDate"],
                                     activityNo = dr["actNo"].ToString(),
                                     endDate = dr["endDate"] is DBNull ? null : (DateTime?)dr["endDate"],
                                     delFlag = (bool)dr["delFlag"],
                                     createdDate = dr["createdDate"] is DBNull ? null : (DateTime?)dr["createdDate"],
                                     createdByUserId = dr["createdByUserId"].ToString(),
                                     updatedDate = dr["updatedDate"] is DBNull ? null : (DateTime?)dr["updatedDate"],
                                     updatedByUserId = dr["updatedByUserId"].ToString()
                                 }).ToList();
                    return lists;
                }
                else
                {
                    return new List<ReportSummaryModels.actApproveSummaryDetailModel>();
                }


            }
            catch (Exception ex)
            {
                throw new Exception("getApproveSummaryDetailListsByEmpId >>" + ex.Message);
            }
        }


        public static string getSummaryIdByRepdetail(string repDetailId)
        {

            try
            {
                object obj = SqlHelper.ExecuteScalar(AppCode.StrCon, CommandType.StoredProcedure, "usp_getSummaryIdByRepdetail"
                    , new SqlParameter[] { new SqlParameter("@repDetailId", repDetailId) });

                return obj.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception("getSummaryIdByRepdetail >>" + ex.Message);
            }
        }

        public static List<ReportSummaryModels.actApproveSummaryDetailModel> getDocumentSummaryDetailByDate(DateTime startDate, DateTime endDate)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getDocumentSummaryReportByDate"
                    , new SqlParameter[] { new SqlParameter("@startDate",startDate)
                        ,new SqlParameter("@endDate",endDate) });
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var lists = (from DataRow dr in ds.Tables[0].Rows
                                 select new ReportSummaryModels.actApproveSummaryDetailModel(dr["createdByUserId"].ToString())
                                 {
                                     id = dr["id"].ToString(),
                                     summaryId = dr["summaryDetailId"].ToString(),
                                     statusName = dr["txtstatus"].ToString(),
                                     statusId = dr["statusid"].ToString(),
                                     productTypeName = dr["txtproductType"].ToString(),
                                     productTypeId = dr["productTypeId"].ToString(),
                                     activityNo = dr["activityNo"].ToString(),
                                     createdDate = dr["createdDate"] is DBNull ? null : (DateTime?)dr["createdDate"],
                                     createdByUserId = dr["createdByUserId"].ToString(),
                                 }).ToList();
                    return lists;
                }
                else
                {
                    return new List<ReportSummaryModels.actApproveSummaryDetailModel>();
                }


            }
            catch (Exception ex)
            {
                throw new Exception("getDocumentSummaryDetailByDate >>" + ex.Message);
            }
        }


        public static int getsummaryDetailStatus(string summaryId)
        {
            try
            {

                object obj = SqlHelper.ExecuteScalar(AppCode.StrCon, CommandType.StoredProcedure, "usp_getsummaryDetailStatus"
                    , new SqlParameter[] { new SqlParameter("@summaryId", summaryId) });

                return int.Parse(obj.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("getsummaryDetailStatus >> " + ex.Message);
            }
        }
    }
}

