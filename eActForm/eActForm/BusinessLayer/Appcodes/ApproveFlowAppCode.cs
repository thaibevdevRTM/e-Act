using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebLibrary;
using static eActForm.Models.ApproveFlowModel;

namespace eActForm.BusinessLayer
{
    public class ApproveFlowAppCode
    {
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
                model.flowDetail = getFlowDetailWithApproveDetail(model.flowMain.id, actId);
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlowByActFormId >>" + ex.Message);
            }
        }

        public static ApproveFlowModel.approveFlowModel getFlowForReportDetail(string subId, string customerId, string productTypeId)
        {
            ApproveFlowModel.approveFlowModel model = new ApproveFlowModel.approveFlowModel();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getFlowMainForReportDetail"
                    , new SqlParameter[] {new SqlParameter("@subjectId",subId)
                    ,new SqlParameter("@customerId",customerId)
                    ,new SqlParameter("@productTypeId",productTypeId)});
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
                model.flowDetail = getFlowDetail(model.flowMain.id);
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getFlowForReportDetail >> flow detail report not found : " + ex.Message);
            }
            return model;
        }


        public static ApproveFlowModel.approveFlowModel getFlowForReportDetailOMT(string subId, string customerId, string productTypeId)
        {
            ApproveFlowModel.approveFlowModel model = new ApproveFlowModel.approveFlowModel();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getFlowMainForReportDetailOMT"
                    , new SqlParameter[] {new SqlParameter("@subjectId",subId)
                    ,new SqlParameter("@customerId",customerId)
                    ,new SqlParameter("@productTypeId",productTypeId)});
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveFlowModel.flowApprove()
                             {
                                 id = dr["id"].ToString(),
                                 flowNameTH = dr["flowNameTH"].ToString(),
                                 cusNameTH = dr["cusNameTH"].ToString(),
                                 cusNameEN = dr["cusNameEN"].ToString(),
                                 nameTH = dr["nameTH"].ToString(),
                             }).ToList();
                if (lists.Any())
                {
                    model.flowMain = lists[0];
                    model.flowDetail = getFlowDetail(model.flowMain.id);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getFlowForReportDetailOMT >> flow detail report not found : " + ex.Message);
            }
            return model;
        }
        /// <summary> 
        /// get flow for type the activity form
        /// </summary>
        /// <param name="subId"></param>
        /// <param name="actFormId"></param>
        /// <returns></returns>
        public static ApproveFlowModel.approveFlowModel getFlowId(string subId, string actFormId)
        {
            try
            {
                ApproveFlowModel.approveFlowModel model = new ApproveFlowModel.approveFlowModel();

                var getMasterType = QueryGetActivityByIdTBMMKT.getActivityById(actFormId).FirstOrDefault().master_type_form_id;
                string stor = AppCode.expenseForm.Contains(getMasterType)  ? "usp_getFlowIdExpenseByActFormId" : "usp_getFlowIdByActFormId";

                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, stor
                    , new SqlParameter[] {new SqlParameter("@subId",subId)
                    ,new SqlParameter("@actFormId",actFormId)});
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveFlowModel.flowApprove()
                             {
                                 id = dr["id"].ToString(),
                             }).ToList();
                if (lists.Count > 0)
                {
                    model.flowMain = lists[0];
                    string checkFlowApprove = checkFlowBeforeByActId(actFormId);
                    checkFlowApprove = string.IsNullOrEmpty(checkFlowApprove) ? model.flowMain.id : checkFlowApprove;

                    if ((AppCode.hcForm.Contains(getMasterType)) || (AppCode.expenseForm.Contains(getMasterType)))
                    {
                        model.flowDetail = getFlowDetailExpense(checkFlowApprove, actFormId);
                    }
                    else
                    {
                        model.flowDetail = getFlowDetail(checkFlowApprove, actFormId);

                        var estimateList = QueryGetActivityEstimateByActivityId.getByActivityId(actFormId);
                        var getLimitAmount = estimateList.Sum(x => x.total);

                        var purpose = QueryGet_master_purpose.getPurposeByActivityId(actFormId).Where(x => x.id == ConfigurationManager.AppSettings["purposeTravelPlane"] && x.chk == true).ToList() ;
                        if (model.flowDetail.Any() && ConfigurationManager.AppSettings["formTrvTbmId"] == getMasterType)
                        {
                            if (purpose.Any() || getLimitAmount > decimal.Parse(ConfigurationManager.AppSettings["limit300000"]))
                            {

                                if (!model.flowDetail.Where(X => X.empId == "11023182").Any())
                                {
                                    int conutRow = model.flowDetail.Count();
                                    var changeApproveGroup = model.flowDetail.Where(x => x.approveGroupId == AppCode.ApproveGroup.Approveby);
                                    foreach (var item in changeApproveGroup)
                                    {
                                        item.approveGroupId = AppCode.ApproveGroup.Verifyby;
                                        item.approveGroupName = "ผ่าน";
                                        item.approveGroupNameEN = "Verify by";
                                    }

                                    model.flowDetail.Where(x => x.rangNo == conutRow).Select(c => c.rangNo = c.rangNo + 2).ToList();
                                    model.flowDetail.Add(getAddOn_TrvTBM(ConfigurationManager.AppSettings["Kpatama"], conutRow, AppCode.ApproveGroup.Verifyby, false));
                                    model.flowDetail.Add(getAddOn_TrvTBM(ConfigurationManager.AppSettings["Kpaparkorn"], conutRow + 1, AppCode.ApproveGroup.Approveby, true));
                                    model.flowDetail.OrderBy(X => X.rangNo);

                                }
                            }
                                
                        }

                    }

                }

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlow by actFormId >>" + ex.Message);
            }
        }

        public static flowApproveDetail getAddOn_TrvTBM(string empId,int rangNo ,string approveGroupId,bool isShowInDoc)
        {
            var result = new ApproveFlowModel.flowApproveDetail(empId)
            {
                rangNo = rangNo,
                empId = empId,
                //empFNameTH = dr["empFNameTH"].ToString(),
                //empLNameTH = dr["empLNameTH"].ToString(),
                //empPositionTitleTH = dr["empPositionTitleTH"].ToString(), get from AD
                approveGroupId = approveGroupId,
                approveGroupName = QueryGetAllApproveGroup.getAllApproveGroup().Where(X => X.id == approveGroupId).FirstOrDefault().nameTH,
                approveGroupNameEN = QueryGetAllApproveGroup.getAllApproveGroup().Where(X => X.id == approveGroupId).FirstOrDefault().nameEN,
                isShowInDoc = isShowInDoc,
                empGroup ="",
                isApproved =  true,

            };

            return result;
        }


            public static string checkFlowBeforeByActId(string actFormId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_CheckFlowHistoryByActFormId"
                    , new SqlParameter[] { new SqlParameter("@actFormId", actFormId) });

                return ds.Tables[0].Rows[0]["flowId"].ToString();

            }
            catch (Exception ex)
            {
                return null;
                throw new Exception("checkFlowBeforeByActId >>" + ex.Message);
            }
        }

        /// <summary>
        /// get flow for type the activity form
        /// </summary>
        /// <param name="subId"></param>
        /// <param name="customerId"></param>
        /// <param name="productCatId"></param>
        /// <returns></returns>
        public static ApproveFlowModel.approveFlowModel getFlow(string subId, string customerId, string productCatId)
        {
            try
            {
                ApproveFlowModel.approveFlowModel model = new ApproveFlowModel.approveFlowModel();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getFlowMain"
                    , new SqlParameter[] {new SqlParameter("@subjectId",subId)
                    ,new SqlParameter("@customerId",customerId)
                    ,new SqlParameter("@productCatId",productCatId)});
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
                model.flowDetail = getFlowDetail(model.flowMain.id);
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlow >> " + ex.Message);
            }
        }

        public static List<ApproveFlowModel.flowApproveDetail> getFlowDetailWithApproveDetail(string flowId, string actId)
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
                             }).ToList();

                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlowDetailWithApproveDetail >>" + ex.Message);
            }
        }

        public static List<ApproveFlowModel.flowApproveDetail> getFlowDetail(string flowId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "c"
                    , new SqlParameter[] { new SqlParameter("@flowId", flowId) });
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
                                 isApproved = dr["isApproved"] != null ? (bool)dr["isApproved"] : true,
                                 empGroup = dr["empGroup"].ToString(),
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlowDetail >>" + ex.Message);
            }
        }
        public static List<ApproveFlowModel.flowApproveDetail> getFlowDetail(string flowId, string actId)
        {
            try
            {
                List<ActivityForm> getActList = QueryGetActivityById.getActivityById(actId);

                var callStored = getActList.FirstOrDefault().companyId == ("5600") || getActList.FirstOrDefault().companyId == ("5601") ? "usp_getFlowApproveDetailForActFormMT" : "usp_getFlowApproveDetailForActForm";

                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, callStored
                    , new SqlParameter[] { new SqlParameter("@flowId", flowId)
                                            , new SqlParameter("@actFormId",actId)
                    });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveFlowModel.flowApproveDetail(dr["empId"].ToString())
                             {
                                 id = dr["id"].ToString(),
                                 rangNo = int.Parse(dr["rangNo"].ToString()),
                                 empId = dr["empId"].ToString(),
                                 empEmail = dr["empEmail"].ToString(),
                                 //empFNameTH = dr["empFNameTH"].ToString(),
                                 //empLNameTH = dr["empLNameTH"].ToString(),
                                 //empPositionTitleTH = dr["empPositionTitleTH"].ToString(), get position from AD
                                 approveGroupId = dr["approveGroupId"].ToString(),//เฟรมเพิ่ม 20200113 เพิ่มapproveGroupId ไว้ใช้ดึงชือ่ผู้อนุมัติแสดงบนหนังสือฟอร์ม
                                 approveGroupName = dr["approveGroupName"].ToString(),
                                 approveGroupNameEN = dr["approveGroupNameEN"].ToString(),
                                 isShowInDoc = (bool)dr["showInDoc"],
                                 empGroup = dr["empGroup"].ToString(),
                                 isApproved = dr["isApproved"] != null ? (bool)dr["isApproved"] : true,
                                // bu = dr["empDivisionTH"].ToString(),
                                // buEN = dr["empDivisionEN"].ToString(),
                                 //empFNameEN = dr["empFNameEN"].ToString(),
                                 //empLNameEN = dr["empLNameEN"].ToString(),
                                 //empPositionTitleEN = dr["empPositionTitleEN"].ToString()
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlowDetail >>" + ex.Message);
            }
        }

        public static List<ApproveFlowModel.flowApproveDetail> getNewPosition(string empId, string companyId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getPostiForSpectialEmpByCompany"
                    , new SqlParameter[]{ new SqlParameter("@companyId", companyId)
                                   , new SqlParameter("@empId", empId)});
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveFlowModel.flowApproveDetail("")
                             {
                                 empPositionTitleTH = dr["positionTH"].ToString(),
                                 empPositionTitleEN = dr["positionEN"].ToString(),

                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getNewPosition >>" + ex.Message);
            }
        }


        public static List<ApproveFlowModel.flowApproveDetail> getFlowDetailExpense(string flowId, string actId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getFlowApproveDetailExpense"
                    , new SqlParameter[]{ new SqlParameter("@flowId", flowId)
                                   , new SqlParameter("@actFormId", actId)});
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveFlowModel.flowApproveDetail(dr["empId"].ToString())
                             {
                                 id = dr["id"].ToString(),
                                 rangNo = (int)dr["rangNo"],
                                 empId = dr["empId"].ToString(),
                                 empEmail = dr["empEmail"].ToString(),
                                 //empFNameTH = dr["empFNameTH"].ToString(),
                                 //empLNameTH = dr["empLNameTH"].ToString(),
                                 //empPositionTitleTH = dr["empPositionTitleTH"].ToString(), get from AD
                                 approveGroupId = dr["approveGroupId"].ToString(),
                                 approveGroupName = dr["approveGroupName"].ToString(),
                                 approveGroupNameEN = dr["approveGroupNameEN"].ToString(),
                                 isShowInDoc = (bool)dr["showInDoc"],
                                 empGroup = dr["empGroup"].ToString(),
                                 isApproved = dr["isApproved"] != null ? (bool)dr["isApproved"] : true,
                                 bu = dr["empDivisionTH"].ToString(),
                                 buEN = dr["empDivisionEN"].ToString(),
                                 //empFNameEN = dr["empFNameEN"].ToString(),
                                 //empLNameEN = dr["empLNameEN"].ToString(),
                                 //empPositionTitleEN = dr["empPositionTitleEN"].ToString()
                             }).ToList();
                return lists;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlowDetailExpense >>" + ex.Message);
            }
        }

        public static ApproveFlowModel.approveFlowModel getFlowApproveGroupByType(getDataList_Model model,string typeFlow)
        {
            try
            {
                string getStored = typeFlow == Activity_Model.typeFlow.flowAddOn.ToString() ? "usp_getFlowAddOnByType" : "usp_getFlowApproveByAllType";
                ApproveFlowModel.approveFlowModel approveFlow_Model = new ApproveFlowModel.approveFlowModel();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, getStored
                    , new SqlParameter[] {new SqlParameter("@companyId",model.companyId)
                    ,new SqlParameter("@subjectId",model.subjectId)
                    ,new SqlParameter("@customerId",model.customerId)
                    ,new SqlParameter("@regionId",model.regionId)
                    ,new SqlParameter("@productCateId",model.productCateId)
                    ,new SqlParameter("@flowLimitId",model.flowLimitId)
                    ,new SqlParameter("@channelId",model.channelId)
                    ,new SqlParameter("@productBrandId",model.productBrandId)
                    ,new SqlParameter("@productType",model.productTypeId)
                    ,new SqlParameter("@activityGroup",model.activityGroup)
                    ,new SqlParameter("@empId",model.empId)
                    ,new SqlParameter("@deparmentId",model.deparmentId)
                    });
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveFlowModel.flowApproveDetail(dr["empId"].ToString())
                             {
                                 id = dr["id"].ToString(),
                                 companyId = dr["companyId"].ToString(),
                                 flowId = dr["flowId"].ToString(),
                                 empId = dr["empId"].ToString(),
                                 approveGroupId = dr["approveGroupId"].ToString(),
                                 rangNo = int.Parse(dr["rangNo"].ToString()),
                                 empGroup = dr["empGroup"].ToString(),
                                 isShowInDoc = !string.IsNullOrEmpty(dr["showInDoc"].ToString()) ? bool.Parse(dr["showInDoc"].ToString()) : true,
                                 isApproved = !string.IsNullOrEmpty(dr["isApproved"].ToString()) ? bool.Parse(dr["isApproved"].ToString()) : true,
                                 activityGroup = dr["activityTypeId"].ToString(),
                                 delFlag = !string.IsNullOrEmpty(dr["delFlag"].ToString()) ? bool.Parse(dr["delFlag"].ToString()) : true,
                             }).ToList();

                var result = !string.IsNullOrEmpty(model.empId) ? lists.Where(x => x.empGroup == model.empId).ToList() : lists.ToList();
                result = string.IsNullOrEmpty(model.activityGroup) ? lists.Where(x => x.activityGroup == "").ToList() : lists.ToList();
                approveFlow_Model.flowDetail = result;
                return approveFlow_Model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlowApproveGroupByType >>" + ex.Message);
            }
        }

        public static ApproveFlowModel.approveFlowModel getFlowAddOnByType(getDataList_Model model)
        {
            try
            {
                ApproveFlowModel.approveFlowModel approveFlow_Model = new ApproveFlowModel.approveFlowModel();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getFlowAddOnByType"
                    , new SqlParameter[] {new SqlParameter("@companyId",model.companyId)
                    ,new SqlParameter("@subjectId",model.subjectId)
                    ,new SqlParameter("@customerId",model.customerId)
                    ,new SqlParameter("@productCateId",model.productCateId)
                    ,new SqlParameter("@flowLimitId",model.flowLimitId)
                    ,new SqlParameter("@channelId",model.channelId)
                    ,new SqlParameter("@productBrandId",model.productBrandId)
                    ,new SqlParameter("@productType",model.productTypeId)});
                var lists = (from DataRow dr in ds.Tables[0].Rows
                             select new ApproveFlowModel.flowApproveDetail("")
                             {
                                 id = dr["id"].ToString(),
                                 companyId = dr["companyId"].ToString(),
                                 flowId = dr["flowId"].ToString(),
                                 empId = dr["empId"].ToString(),
                                 empFNameTH = dr["empName"].ToString(),
                                 approveGroupId = dr["approveGroupId"].ToString(),
                                 rangNo = int.Parse(dr["rangNo"].ToString()),
                                 empGroup = dr["empGroup"].ToString(),
                                 isShowInDoc = !string.IsNullOrEmpty(dr["showInDoc"].ToString()) ? bool.Parse(dr["showInDoc"].ToString()) : true,
                                 isApproved = !string.IsNullOrEmpty(dr["isApproved"].ToString()) ? bool.Parse(dr["isApproved"].ToString()) : true,
                             }).ToList();

                var result = !string.IsNullOrEmpty(model.empId) ? lists.Where(x => x.empGroup == model.empId).ToList() : lists.ToList();


                approveFlow_Model.flowDetail = result;
                return approveFlow_Model;
            }
            catch (Exception ex)
            {
                throw new Exception("getFlowApproveGroupByType >>" + ex.Message);
            }
        }


        public static List<TB_Reg_FlowModel> getMainFlowByMasterTypeId(string masterTypeId)
        {
            try
            {
                List<TB_Reg_FlowModel> regMainFlow = new List<TB_Reg_FlowModel>();
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_get_FlowMainByMasterTypeId"
                    , new SqlParameter[] { new SqlParameter("@masterTypeId", masterTypeId) });
                regMainFlow = (from DataRow dr in ds.Tables[0].Rows
                               select new TB_Reg_FlowModel()
                               {
                                   id = dr["id"].ToString(),
                                   subjectId = dr["subjectId"].ToString(),
                                   companyId = dr["companyId"].ToString(),
                                   customerId = dr["customerId"].ToString(),
                                   productCatId = dr["productCatId"].ToString(),
                                   productTypeId = dr["productTypeId"].ToString(),
                                   flowLimitId = dr["flowLimitId"].ToString(),
                                   channelId = dr["channelId"].ToString(),
                                   productBrandId = dr["productBrandId"].ToString(),
                               }).ToList();

                return regMainFlow;
            }
            catch (Exception ex)
            {
                throw new Exception("getMainFlowByMasterTypeId >>" + ex.Message);
            }
        }


        public static List<RequestEmpModel> getEmpByConditon(string subjectId, string limitId, string channelId)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_getEmpSettingFlow"
                    , new SqlParameter[] { new SqlParameter("@subjectId", subjectId)
                    ,new SqlParameter("@flowLimitId", limitId)
                    ,new SqlParameter("@channelId", channelId)
                    });
                var result = (from DataRow dr in ds.Tables[0].Rows
                              select new RequestEmpModel(dr["empId"].ToString(),false,false)
                              {
                              }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("getEmpByConditon >>" + ex.Message);
            }
        }
    }
}