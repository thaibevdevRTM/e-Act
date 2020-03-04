using eActForm.BusinessLayer.Appcodes;
using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class ActivityFormTBMMKTCommandHandler
    {
        public static int insertAllActivity(Activity_TBMMKT_Model model, string activityId)
        {
            int rtn = 0;
            try
            {

                if (model.activityFormModel.mode == AppCode.Mode.edit.ToString() && model.activityFormTBMMKT.statusId == 2 && UtilsAppCode.Session.User.isAdminTBM)//ถ้าเป็น บัญชีเข้ามาเพื่อกรอก IO
                {
                    rtn = ProcessInsertEstimate(rtn, model, activityId);
                    rtn = ProcessInsertTB_Act_ActivityForm_DetailOther(rtn, model, activityId);
                }
                else
                {
                    model.activityFormModel.id = activityId;
                    model.activityFormModel.statusId = 1;
                    model.activityFormModel.documentDate = model.activityFormModel.documentDate;
                    model.activityFormModel.activityPeriodSt = string.IsNullOrEmpty(model.activityFormModel.activityPeriodSt.ToString()) ? (DateTime?)null : model.activityFormModel.activityPeriodSt;
                    model.activityFormModel.activityPeriodEnd = string.IsNullOrEmpty(model.activityFormModel.activityPeriodEnd.ToString()) ? (DateTime?)null : model.activityFormModel.activityPeriodEnd;
                    model.activityFormModel.activityNo = string.IsNullOrEmpty(model.activityFormModel.activityNo) ? "---" : model.activityFormModel.activityNo;
                    model.activityFormModel.createdByUserId = model.activityFormModel.createdByUserId != null ? model.activityFormModel.createdByUserId : UtilsAppCode.Session.User.empId;
                    model.activityFormModel.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
                    model.activityFormModel.updatedByUserId = UtilsAppCode.Session.User.empId;
                    model.activityFormModel.updatedDate = DateTime.Now;
                    model.activityFormModel.delFlag = false;
                    model.activityFormModel.companyId = model.activityFormTBMMKT.formCompanyId;
                    model.activityFormModel.remark = model.activityFormModel.remark;
                    model.activityFormModel.master_type_form_id = model.activityFormTBMMKT.master_type_form_id == null ? "" : model.activityFormTBMMKT.master_type_form_id;

                    rtn = insertActivityForm(model.activityFormModel);

                    rtn = ProcessInsertTB_Act_ActivityForm_DetailOther(rtn, model, activityId);

                    rtn = ProcessInsertEstimate(rtn, model, activityId);

                    rtn = ProcessInsertTB_Act_ActivityChoiceSelect(rtn, model, activityId);



                    rtn = ProcessInsertRequestEmp(rtn, model, activityId);
                    rtn = ProcessInsertPlaceDetail(rtn, model, activityId);
                    rtn = ProcessInsertPurpose(rtn, model, activityId);


                }

                return rtn;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertAllActivity");
                return rtn;
            }


        }

        public static int ProcessInsertEstimate(int rtn, Activity_TBMMKT_Model model, string activityId)
        {
            int insertIndex = 1;
            if (model.costThemeDetailOfGroupByPriceTBMMKT != null)
            {
                rtn += deleteActivityOfEstimateByActivityId(activityId);
                foreach (var item in model.costThemeDetailOfGroupByPriceTBMMKT.ToList())
                {

                    CostThemeDetailOfGroupByPriceTBMMKT costThemeDetail = new CostThemeDetailOfGroupByPriceTBMMKT();
                    costThemeDetail.id = Guid.NewGuid().ToString();
                    costThemeDetail.activityId = activityId;
                    costThemeDetail.activityTypeId = item.activityTypeId;
                    costThemeDetail.productDetail = item.productDetail;
                    costThemeDetail.total = item.total == null ? 0 : item.total;
                    costThemeDetail.IO = item.IO;
                    costThemeDetail.rowNo = insertIndex;
                    costThemeDetail.delFlag = false;
                    costThemeDetail.createdByUserId = model.activityFormModel.createdByUserId;
                    costThemeDetail.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
                    costThemeDetail.updatedByUserId = UtilsAppCode.Session.User.empId;
                    costThemeDetail.updatedDate = DateTime.Now;
                    costThemeDetail.unit = item.unit;
                    costThemeDetail.unitPrice = item.unitPriceDisplay == null ? 0 : decimal.Parse(item.unitPriceDisplay.Replace(",", ""));
                    costThemeDetail.QtyName = item.QtyName;
                    costThemeDetail.remark = item.remark == null ? "" : item.remark;
                    costThemeDetail.productId = item.productId == null ? "" : item.productId;
                    costThemeDetail.typeTheme = item.typeTheme;

                    rtn += insertEstimate(costThemeDetail);

                    insertIndex++;
                }
            }
            return rtn;
        }

        public static int ProcessInsertTB_Act_ActivityForm_DetailOther(int rtn, Activity_TBMMKT_Model model, string activityId)
        {
            TB_Act_ActivityForm_DetailOther tB_Act_ActivityForm_DetailOther = new TB_Act_ActivityForm_DetailOther();
            if (model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formTrvTbmId"] || model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formTrvHcmId"])//แบบฟอร์มเดินทางปฏิบัติงานนอกสถานที่
            {
                model.tB_Act_ActivityForm_DetailOther = tB_Act_ActivityForm_DetailOther;
                model.tB_Act_ActivityForm_DetailOther.activityProduct = "";
                model.tB_Act_ActivityForm_DetailOther.activityTel = "";
                model.tB_Act_ActivityForm_DetailOther.IO = "";
                model.tB_Act_ActivityForm_DetailOther.EO = "";
                model.tB_Act_ActivityForm_DetailOther.descAttach = "";
                model.tB_Act_ActivityForm_DetailOther.BudgetNumber = "";
            }

            int insertIndex = 1;
            if (model.tB_Act_ActivityForm_DetailOther != null)
            {
                rtn += deleteusp_deleteTB_Act_ActivityForm_DetailOther(activityId);
                tB_Act_ActivityForm_DetailOther.Id = Guid.NewGuid().ToString();
                tB_Act_ActivityForm_DetailOther.activityId = activityId;

                if (model.activityFormTBMMKT.selectedBrandOrChannel == "Brand")
                {
                    tB_Act_ActivityForm_DetailOther.productBrandId = model.activityFormTBMMKT.BrandlId;
                }
                else
                {
                    tB_Act_ActivityForm_DetailOther.channelId = model.activityFormTBMMKT.channelId;
                }

                tB_Act_ActivityForm_DetailOther.SubjectId = model.activityFormTBMMKT.SubjectId;
                tB_Act_ActivityForm_DetailOther.activityProduct = model.tB_Act_ActivityForm_DetailOther.activityProduct;
                tB_Act_ActivityForm_DetailOther.activityTel = model.tB_Act_ActivityForm_DetailOther.activityTel;
                tB_Act_ActivityForm_DetailOther.IO = model.tB_Act_ActivityForm_DetailOther.IO;
                tB_Act_ActivityForm_DetailOther.EO = model.tB_Act_ActivityForm_DetailOther.EO;
                tB_Act_ActivityForm_DetailOther.descAttach = model.tB_Act_ActivityForm_DetailOther.descAttach;
                tB_Act_ActivityForm_DetailOther.delFlag = false;
                tB_Act_ActivityForm_DetailOther.createdByUserId = model.activityFormModel.createdByUserId;
                tB_Act_ActivityForm_DetailOther.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
                tB_Act_ActivityForm_DetailOther.updatedByUserId = UtilsAppCode.Session.User.empId;
                tB_Act_ActivityForm_DetailOther.updatedDate = DateTime.Now;
                tB_Act_ActivityForm_DetailOther.BudgetNumber = model.tB_Act_ActivityForm_DetailOther.BudgetNumber;
                tB_Act_ActivityForm_DetailOther.brand_select = model.activityFormTBMMKT.brand_select;
                tB_Act_ActivityForm_DetailOther.costCenter = model.tB_Act_ActivityForm_DetailOther.costCenter;
                tB_Act_ActivityForm_DetailOther.channelRegionName = model.tB_Act_ActivityForm_DetailOther.channelRegionName;
                tB_Act_ActivityForm_DetailOther.glNo = model.tB_Act_ActivityForm_DetailOther.glNo;
                tB_Act_ActivityForm_DetailOther.glName = model.tB_Act_ActivityForm_DetailOther.glName;
                tB_Act_ActivityForm_DetailOther.toName = model.tB_Act_ActivityForm_DetailOther.toName;
                tB_Act_ActivityForm_DetailOther.toAddress = model.tB_Act_ActivityForm_DetailOther.toAddress;
                tB_Act_ActivityForm_DetailOther.toContact = model.tB_Act_ActivityForm_DetailOther.toContact;
                tB_Act_ActivityForm_DetailOther.detailContact = model.tB_Act_ActivityForm_DetailOther.detailContact;

                rtn += usp_insertTB_Act_ActivityForm_DetailOther(tB_Act_ActivityForm_DetailOther);

                insertIndex++;
            }
            return rtn;
        }

        public static int ProcessInsertTB_Act_ActivityChoiceSelect(int rtn, Activity_TBMMKT_Model model, string activityId)
        {
            var okProcessInsert = false;
            TB_Act_ActivityChoiceSelectModel tB_Act_ActivityChoiceSelectModel = new TB_Act_ActivityChoiceSelectModel();
            tB_Act_ActivityChoiceSelectModel.delFlag = false;
            tB_Act_ActivityChoiceSelectModel.createdByUserId = model.activityFormModel.createdByUserId;
            tB_Act_ActivityChoiceSelectModel.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
            tB_Act_ActivityChoiceSelectModel.updatedByUserId = UtilsAppCode.Session.User.empId;
            tB_Act_ActivityChoiceSelectModel.updatedDate = DateTime.Now;

            if (model.activityFormTBMMKT.list_0_select != null || model.activityFormTBMMKT.list_1_multi_select != null ||
                 model.activityFormTBMMKT.list_2_select != null || model.activityFormTBMMKT.brand_select != null ||
                 model.activityFormTBMMKT.list_3_select != null)
            {
                rtn += deleteActivityTB_Act_ActivityChoiceSelect(activityId);
                okProcessInsert = true;
            }

            if (okProcessInsert == true)
            {
                if (model.activityFormTBMMKT.list_0_select != "")//สต๊อก
                {
                    tB_Act_ActivityChoiceSelectModel.id = Guid.NewGuid().ToString();
                    tB_Act_ActivityChoiceSelectModel.actFormId = activityId;
                    tB_Act_ActivityChoiceSelectModel.select_list_choice_id = model.activityFormTBMMKT.list_0_select;
                    rtn += insertActivityChoiceSelect(tB_Act_ActivityChoiceSelectModel);
                }

                if (model.activityFormTBMMKT.list_1_multi_select != null)
                {
                    if (model.activityFormTBMMKT.list_1_multi_select.Length > 0)
                    {
                        for (int i = 0; i < model.activityFormTBMMKT.list_1_multi_select.Length; i++)//ขอเบิก
                        {
                            tB_Act_ActivityChoiceSelectModel.id = Guid.NewGuid().ToString();
                            tB_Act_ActivityChoiceSelectModel.actFormId = activityId;
                            tB_Act_ActivityChoiceSelectModel.select_list_choice_id = model.activityFormTBMMKT.list_1_multi_select[i];
                            rtn += insertActivityChoiceSelect(tB_Act_ActivityChoiceSelectModel);
                        }
                    }
                }
                if (model.activityFormTBMMKT.list_2_select != null)
                {
                    if (model.activityFormTBMMKT.list_2_select != "")//เพื่อ
                    {
                        tB_Act_ActivityChoiceSelectModel.id = Guid.NewGuid().ToString();
                        tB_Act_ActivityChoiceSelectModel.actFormId = activityId;
                        tB_Act_ActivityChoiceSelectModel.select_list_choice_id = model.activityFormTBMMKT.list_2_select;
                        rtn += insertActivityChoiceSelect(tB_Act_ActivityChoiceSelectModel);
                    }
                }
                if (model.activityFormTBMMKT.brand_select != null)
                {
                    if (model.activityFormTBMMKT.brand_select != "")//Brand/ผลิตภัณฑ์ 
                    {
                        tB_Act_ActivityChoiceSelectModel.id = Guid.NewGuid().ToString();
                        tB_Act_ActivityChoiceSelectModel.actFormId = activityId;
                        tB_Act_ActivityChoiceSelectModel.select_list_choice_id = model.activityFormTBMMKT.brand_select;
                        rtn += insertActivityChoiceSelect(tB_Act_ActivityChoiceSelectModel);
                    }
                }
                if (model.activityFormTBMMKT.list_3_select != null)
                {
                    if (model.activityFormTBMMKT.list_3_select != "")//Channel+Region
                    {
                        tB_Act_ActivityChoiceSelectModel.id = Guid.NewGuid().ToString();
                        tB_Act_ActivityChoiceSelectModel.actFormId = activityId;
                        tB_Act_ActivityChoiceSelectModel.select_list_choice_id = model.activityFormTBMMKT.list_3_select;
                        rtn += insertActivityChoiceSelect(tB_Act_ActivityChoiceSelectModel);
                    }
                }
            }
            return rtn;
        }

        public static Activity_TBMMKT_Model getDataForEditActivity(string activityId)
        {
            Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
            try
            {
                string sumTxtLabelRequired = "";
                activity_TBMMKT_Model.activityFormTBMMKT = QueryGetActivityByIdTBMMKT.getActivityById(activityId).FirstOrDefault(); // TB_Act_ActivityForm
                activity_TBMMKT_Model.activityFormModel = activity_TBMMKT_Model.activityFormTBMMKT;
                activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther = QueryGetActivityFormDetailOtherByActivityId.getByActivityId(activityId).FirstOrDefault(); // TB_Act_ActivityForm_DetailOther                
                activity_TBMMKT_Model.costThemeDetailOfGroupByPriceTBMMKT = QueryGetActivityEstimateByActivityId.getByActivityId(activityId);  //TB_Act_ActivityOfEstimate
                activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel = QueryGet_TB_Act_ActivityChoiceSelect.get_TB_Act_ActivityChoiceSelectModel(activityId);

                if (activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Count > 0)
                {
                    if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPosTbmId"])
                    {
                        activity_TBMMKT_Model.activityFormTBMMKT.list_0_select = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "in_or_out_stock").FirstOrDefault().select_list_choice_id;
                        activity_TBMMKT_Model.activityFormTBMMKT.labelInOrOutStock = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "in_or_out_stock").FirstOrDefault().name;
                        var countlist_1_multi_select = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "product_pos_premium").Count();
                        activity_TBMMKT_Model.activityFormTBMMKT.list_1_multi_select = new string[countlist_1_multi_select];

                        int index_each = 0;
                        foreach (var item in activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "product_pos_premium").ToList())
                        {
                            activity_TBMMKT_Model.activityFormTBMMKT.list_1_multi_select[index_each] = item.select_list_choice_id;
                            if (index_each == 0)
                            {
                                sumTxtLabelRequired += item.name;
                            }
                            else
                            {
                                sumTxtLabelRequired += ("," + item.name);
                            }
                            index_each++;
                        }
                        activity_TBMMKT_Model.activityFormTBMMKT.labelRequire = sumTxtLabelRequired;
                        activity_TBMMKT_Model.activityFormTBMMKT.list_2_select = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "for").FirstOrDefault().select_list_choice_id;
                        activity_TBMMKT_Model.activityFormTBMMKT.labelFor = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "for").FirstOrDefault().name;
                        activity_TBMMKT_Model.activityFormTBMMKT.brand_select = activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.brand_select;
                        activity_TBMMKT_Model.activityFormTBMMKT.labelBrand = QueryGetAllBrandByForm.GetAllBrand().Where(x => x.id == activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.brand_select).FirstOrDefault().brandName;
                        activity_TBMMKT_Model.activityFormTBMMKT.list_3_select = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "channel_place").FirstOrDefault().select_list_choice_id;
                        activity_TBMMKT_Model.activityFormTBMMKT.labelChannelRegion = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "channel_place").FirstOrDefault().name;
                        if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.productBrandId != "")
                        {
                            activity_TBMMKT_Model.activityFormTBMMKT.labelBrandOrChannel = "Brand";
                        }
                        else
                        {
                            activity_TBMMKT_Model.activityFormTBMMKT.labelBrandOrChannel = "Channel";
                        }
                    }
                    else if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formTrvTbmId"] || activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formTrvHcmId"])
                    {
                        activity_TBMMKT_Model.activityFormTBMMKT.list_0_select = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "travelling").FirstOrDefault().select_list_choice_id;
                        activity_TBMMKT_Model.activityFormTBMMKT.list_0_select_value = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "travelling").FirstOrDefault().name;
                    }
                }


                activity_TBMMKT_Model.requestEmpModel = QueryGet_ReqEmpByActivityId.getReqEmpByActivityId(activityId);
                activity_TBMMKT_Model.purposeModel = QueryGet_master_purpose.getPurposeByActivityId(activityId);
                activity_TBMMKT_Model.placeDetailModel = QueryGet_PlaceDetailByActivityId.getPlaceDetailByActivityId(activityId);
                activity_TBMMKT_Model.expensesDetailModel.costDetailLists = activity_TBMMKT_Model.costThemeDetailOfGroupByPriceTBMMKT;

                Decimal? totalCostThisActivity = 0;
                foreach (var item in activity_TBMMKT_Model.costThemeDetailOfGroupByPriceTBMMKT)
                {
                    if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPosTbmId"])//ใบเบิกผลิตภัณฑ์,POS/PREMIUM
                    {
                        totalCostThisActivity += item.unit;
                    }
                    else
                    {
                        totalCostThisActivity += item.total;
                    }
                }

                activity_TBMMKT_Model.totalCostThisActivity = totalCostThisActivity;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> getDataForEditActivityTBMMKT");
            }
            return activity_TBMMKT_Model;
        }

        public static int updateActivityForm(Activity_Model model, string activityId)
        {
            int rtn = 0;
            try
            {
                model.activityFormModel.id = activityId;
                model.activityFormModel.activityPeriodSt = string.IsNullOrEmpty(model.activityFormModel.str_activityPeriodSt) ? (DateTime?)null :
                 DateTime.ParseExact(model.activityFormModel.str_activityPeriodSt, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                model.activityFormModel.activityPeriodEnd = string.IsNullOrEmpty(model.activityFormModel.str_activityPeriodEnd) ? (DateTime?)null :
                DateTime.ParseExact(model.activityFormModel.str_activityPeriodEnd, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                model.activityFormModel.costPeriodSt = string.IsNullOrEmpty(model.activityFormModel.str_costPeriodSt) ? (DateTime?)null :
                 DateTime.ParseExact(model.activityFormModel.str_costPeriodSt, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                model.activityFormModel.costPeriodEnd = string.IsNullOrEmpty(model.activityFormModel.str_costPeriodEnd) ? (DateTime?)null :
                  DateTime.ParseExact(model.activityFormModel.str_costPeriodEnd, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                model.activityFormModel.updatedByUserId = UtilsAppCode.Session.User.empId;
                model.activityFormModel.updatedDate = DateTime.Now;
                rtn = updateActivityForm(model.activityFormModel);
                return rtn;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> updateActivityForm");
                return rtn;
            }
        }

        public static string[] genNumberActivity(string activityId)
        {
            try
            {

                String[] result = new String[2];
                List<ActivityForm> getActList = QueryGetActivityById.getActivityById(activityId);
                if (getActList.Any())
                {
                    if (getActList.FirstOrDefault().activityNo.ToString() == "---")
                    {
                        if (getActList.FirstOrDefault().chanel_Id != "")
                        {
                            int genNumber = int.Parse(getActivityDoc(getActList.FirstOrDefault().chanel_Id).FirstOrDefault().docNo);

                            result[0] += getActList.FirstOrDefault().trade == "term" ? "W" : "S";
                            result[0] += getActList.FirstOrDefault().shortBrand.Trim();
                            result[0] += getActList.FirstOrDefault().chanelShort.Trim();
                            result[0] += getActList.FirstOrDefault().cusShortName.Trim();
                            result[0] += new ThaiBuddhistCalendar().GetYear(DateTime.Now).ToString().Substring(2, 2);
                            result[0] += string.Format("{0:0000}", genNumber);
                            result[1] = Activity_Model.activityType.MT.ToString();
                        }
                        else
                        {
                            int genNumber = int.Parse(getActivityDoc("region").FirstOrDefault().docNo);
                            result[0] += getActList.FirstOrDefault().trade == "term" ? "W" : "S";
                            result[0] += getActList.FirstOrDefault().shortBrand.Trim();
                            result[0] += getActList.FirstOrDefault().regionShort.Trim();
                            result[0] += getActList.FirstOrDefault().cusShortName.Trim();
                            result[0] += new ThaiBuddhistCalendar().GetYear(DateTime.Now).ToString().Substring(2, 2);
                            result[0] += string.Format("{0:0000}", genNumber);
                            result[1] = Activity_Model.activityType.OMT.ToString();
                        }
                    }
                    else
                    {
                        result[0] = getActList.FirstOrDefault().activityNo.ToString();
                        result[1] = UtilsAppCode.Session.User.empCompanyId == ConfigurationManager.AppSettings["companyId_OMT"] ? Activity_Model.activityType.OMT.ToString() : Activity_Model.activityType.MT.ToString();

                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> genNumberActivity");
                return null;
            }
        }

        public static int deleteActivityOfProductByActivityId(string activityId)
        {

            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteActivityOfProductByActivityId"
                    , new SqlParameter[] {new SqlParameter("@activityId",activityId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> deleteActivityOfProductByActivityId");
            }

            return result;
        }

        public static int deleteusp_deleteTB_Act_ActivityLayout(string activityId)
        {

            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteTB_Act_ActivityLayout"
                    , new SqlParameter[] {new SqlParameter("@activityId",activityId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> deleteusp_deleteTB_Act_ActivityLayout");
            }

            return result;
        }

        public static int deleteActivityOfEstimateByActivityId(string activityId)
        {

            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteActivityOfEstimateByActivityId"
                    , new SqlParameter[] {new SqlParameter("@activityId",activityId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> deleteActivityOfEstimateByActivityId");
            }

            return result;
        }

        public static int deleteActivityTB_Act_ActivityChoiceSelect(string activityId)
        {

            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteActivityTB_Act_ActivityChoiceSelect"
                    , new SqlParameter[] {new SqlParameter("@activityId",activityId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> deleteActivityTB_Act_ActivityChoiceSelect");
            }

            return result;
        }

        public static int deleteusp_deleteTB_Act_ActivityForm_DetailOther(string activityId)
        {

            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteTB_Act_ActivityForm_DetailOther"
                    , new SqlParameter[] {new SqlParameter("@activityId",activityId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> deleteusp_deleteTB_Act_ActivityForm_DetailOther");
            }

            return result;
        }


        protected static int insertActivityForm(ActivityForm model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertAllActivity"
                    , new SqlParameter[] {new SqlParameter("@id",model.id)
                     ,new SqlParameter("@statusId",model.statusId)
                    ,new SqlParameter("@activityNo",model.activityNo)
                    ,new SqlParameter("@documentDate",model.documentDate)
                    ,new SqlParameter("@activityPeriodST",model.activityPeriodSt)
                    ,new SqlParameter("@activityPeriodEND",model.activityPeriodEnd)
                    ,new SqlParameter("@activityName",model.activityName)
                    ,new SqlParameter("@objective",model.objective)
                    ,new SqlParameter("@companyId",model.companyId)
                    ,new SqlParameter("@delFlag",model.delFlag)
                    ,new SqlParameter("@createdDate",model.createdDate)
                    ,new SqlParameter("@createdByUserId",model.createdByUserId)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)
                    ,new SqlParameter("@master_type_form_id",model.master_type_form_id)
                    ,new SqlParameter("@remark",model.remark)
                    ,new SqlParameter("@benefit", (model.benefit == null ? "" :model.benefit))
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertAllActivity");
            }

            return result;
        }


        protected static int updateActivityForm(ActivityForm model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateActivityForm"
                    , new SqlParameter[] {new SqlParameter("@actId",model.id)
                    ,new SqlParameter("@activityPeriodST",model.activityPeriodSt)
                    ,new SqlParameter("@activityPeriodEND",model.activityPeriodEnd)
                    ,new SqlParameter("@costPeriodST",model.costPeriodSt)
                    ,new SqlParameter("@costPeriodEND",model.costPeriodEnd)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> updateActivityForm");
            }

            return result;
        }


        protected static int usp_insertTB_Act_ActivityForm_DetailOther(TB_Act_ActivityForm_DetailOther model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertTB_Act_ActivityForm_DetailOther"
                    , new SqlParameter[] {new SqlParameter("@id",model.Id)
                    ,new SqlParameter("@activityId",model.activityId)
                    ,new SqlParameter("@productBrandId",model.productBrandId)
                    ,new SqlParameter("@channelId",model.channelId)
                    ,new SqlParameter("@SubjectId",model.SubjectId)
                    ,new SqlParameter("@activityProduct",model.activityProduct)
                    ,new SqlParameter("@activityTel",model.activityTel)
                    ,new SqlParameter("@EO",model.EO)
                    ,new SqlParameter("@descAttach",model.descAttach)
                    ,new SqlParameter("@BudgetNumber",model.BudgetNumber)
                    ,new SqlParameter("@IO",model.IO)
                    ,new SqlParameter("@brand_select",model.brand_select)
                    ,new SqlParameter("@costCenter",model.costCenter)
                    ,new SqlParameter("@channelRegionName",model.channelRegionName)
                    ,new SqlParameter("@glNo",model.glNo)
                    ,new SqlParameter("@glName",model.glName)
                    ,new SqlParameter("@toName",model.toName)
                    ,new SqlParameter("@toAddress",model.toAddress)
                    ,new SqlParameter("@toContact",model.toContact)
                    ,new SqlParameter("@detailContact",model.detailContact)
                    ,new SqlParameter("@delFlag",model.delFlag)
                    ,new SqlParameter("@createdDate",model.createdDate)
                    ,new SqlParameter("@createdByUserId",model.createdByUserId)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertEstimateTBMMKT");
            }

            return result;
        }

        protected static int usp_insertusp_insertTB_Act_ActivityLayout(TB_Act_ActivityLayout model)
        {
            int result = 0;
            if (model.io == null)
            {
                model.io = "";
            }
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertTB_Act_ActivityLayout"
                    , new SqlParameter[] {new SqlParameter("@id",model.id)
                    ,new SqlParameter("@activityId",model.activityId)
                    ,new SqlParameter("@no",model.no)
                    ,new SqlParameter("@io",model.io)
                    ,new SqlParameter("@activity",model.activity)
                    ,new SqlParameter("@amount",model.amount)
                    ,new SqlParameter("@delFlag",model.delFlag)
                    ,new SqlParameter("@createdDate",model.createdDate)
                    ,new SqlParameter("@createdByUserId",model.createdByUserId)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> usp_insertusp_insertTB_Act_ActivityLayout");
            }

            return result;
        }

        protected static int insertEstimate(CostThemeDetailOfGroupByPriceTBMMKT model)
        {
            int result = 0;

            if (model.IO == null)
            {
                model.IO = "";
            }

            if (model.unitPrice == null)
            {
                model.unitPrice = 0;
            }

            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertCostThemeDetail"
                    , new SqlParameter[] {new SqlParameter("@id",model.id)
                    ,new SqlParameter("@activityId",model.activityId)
                    ,new SqlParameter("@activityTypeId",model.activityTypeId)
                    ,new SqlParameter("@productDetail",model.productDetail)
                    ,new SqlParameter("@IO",model.IO)
                    ,new SqlParameter("@remark",model.remark)
                    ,new SqlParameter("@total",decimal.Parse(string.Format("{0:0.00000}", model.total)))
                    ,new SqlParameter("@rowNo",model.rowNo)
                    ,new SqlParameter("@delFlag",model.delFlag)
                    ,new SqlParameter("@createdDate",model.createdDate)
                    ,new SqlParameter("@createdByUserId",model.createdByUserId)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)
                    ,new SqlParameter("@unit",Convert.ToInt32(model.unit))
                    ,new SqlParameter("@unitPrice", decimal.Parse(string.Format("{0:0.00000}", model.unitPrice)))
                    ,new SqlParameter("@QtyName",model.QtyName)
                    ,new SqlParameter("@productId",model.productId)
                    ,new SqlParameter("@typeTheme",(model.typeTheme == null ? "" : model.typeTheme))
            });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertEstimateTBMMKT");
            }

            return result;
        }

        protected static int insertActivityChoiceSelect(TB_Act_ActivityChoiceSelectModel model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertTB_Act_ActivityChoiceSelect"
                    , new SqlParameter[] {new SqlParameter("@id",model.id)
                    ,new SqlParameter("@actFormId",model.actFormId)
                    ,new SqlParameter("@select_list_choice_id",model.select_list_choice_id)
                    ,new SqlParameter("@delFlag",model.delFlag)
                    ,new SqlParameter("@createdDate",model.createdDate)
                    ,new SqlParameter("@createdByUserId",model.createdByUserId)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertActivityChoiceSelect");
            }

            return result;
        }

        protected static int insertProductCost(DataTable dt)
        {
            try
            {
                int rtn = 0;
                foreach (DataRow dr in dt.Rows)
                {

                    rtn += SqlHelper.ExecuteNonQueryTypedParams(AppCode.StrCon, "usp_insertProductCostdetail", dr);
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("insertProductCost >> " + ex.Message);
            }
        }

        protected static int insertCostThemeDetail(DataTable dt)
        {
            try
            {
                int rtn = 0;
                foreach (DataRow dr in dt.Rows)
                {

                    rtn += SqlHelper.ExecuteNonQueryTypedParams(AppCode.StrCon, "usp_insertCostThemeDetail", dr);
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw new Exception("insertCostThemeDetail >> " + ex.Message);
            }
        }


        public static int updateStatusGenDocActivity(string statusId, string activityId, string genNumDoc)
        {

            int result = 0;
            try
            {

                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_InsertNumDocAndStatusActFormByActId"
                    , new SqlParameter[] {new SqlParameter("@statusId",statusId)
                    ,new SqlParameter("@activityId",activityId)
                    ,new SqlParameter("@genActivityDoc",genNumDoc)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> updateStatusGenDocActivity");
            }

            return result;
        }




        public static List<TB_Act_ActivityFormDocNo_Model> getActivityDoc(string chanel_Id)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertDocNoByChanelId"
                , new SqlParameter("@chanel_Id", chanel_Id));

                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Act_ActivityFormDocNo_Model()
                             {
                                 docNo = d["docNo"].ToString()
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getAllProductCate => " + ex.Message);
                return new List<TB_Act_ActivityFormDocNo_Model>();
            }
        }


        public static string getStatusActivity(string actId)
        {
            string result = "";
            try
            {
                object obj = SqlHelper.ExecuteScalar(AppCode.StrCon, CommandType.StoredProcedure, "usp_getCheckStatusActivity"
                    , new SqlParameter[] { new SqlParameter("@actId", actId) });
                result = obj.ToString();
                return result;

            }
            catch (Exception ex)
            {
                return result = "";
                throw new Exception("getStatusActivity >>" + ex.Message);
            }

        }

        public static int updateIOActivity(Activity_TBMMKT_Model model, string activityId)
        {
            int rtn = 0;
            try
            {
                int insertIndex = 1;

                if (model.costThemeDetailOfGroupByPriceTBMMKT != null)
                {
                    foreach (var item in model.costThemeDetailOfGroupByPriceTBMMKT.ToList())
                    {
                        CostThemeDetailOfGroupByPriceTBMMKT costThemeDetail = new CostThemeDetailOfGroupByPriceTBMMKT();
                        costThemeDetail.id = item.id;// ไท่แน่ใจค่าจะมาหรือป่าว                       
                        costThemeDetail.IO = item.IO;
                        costThemeDetail.updatedByUserId = UtilsAppCode.Session.User.empId;
                        costThemeDetail.updatedDate = DateTime.Now;
                        rtn += usp_updateIOActivity(costThemeDetail);
                        insertIndex++;
                    }
                }
                return rtn;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> updateIOActivity");
                return rtn;
            }


        }

        protected static int usp_updateIOActivity(CostThemeDetailOfGroupByPriceTBMMKT model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_updateIOActivity"
                    , new SqlParameter[] {new SqlParameter("@id",model.id)
                    ,new SqlParameter("@IO",model.IO)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> usp_UpdateIOActivity");
            }

            return result;
        }

        public static List<ApproveFlowModel.flowApproveDetail> get_flowApproveDetail(string SubjectId, string activityId)
        {
            try
            {
                ApproveModel.approveModels models = new ApproveModel.approveModels();
                models = ApproveAppCode.getApproveByActFormId(activityId);
                ApproveFlowModel.approveFlowModel flowModel = ApproveFlowAppCode.getFlowId(SubjectId, activityId);
                models.approveFlowDetail = flowModel.flowDetail;

                return models.approveFlowDetail;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("get_flowApproveDetail => " + ex.Message);
                return new List<ApproveFlowModel.flowApproveDetail>();
            }
        }


        #region "travelling tbm"
        public static int deleteRequestEmpByActivityId(string activityId)
        {

            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteRequestEmpByActivityId"
                    , new SqlParameter[] {new SqlParameter("@activityId",activityId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> deleteRequestEmpByActivityId");
            }

            return result;
        }
        public static int deletePlaceDetailByActivityId(string activityId)
        {

            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deletePlaceDetailByActivityId"
                    , new SqlParameter[] {new SqlParameter("@activityId",activityId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> deletePlaceDetailByActivityId");
            }

            return result;
        }
        public static int deletePurposeByActivityId(string activityId)
        {

            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deletePurposeByActivityId"
                    , new SqlParameter[] {new SqlParameter("@activityId",activityId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> deletePlaceDetailByActivityId");
            }

            return result;
        }

        public static int ProcessInsertRequestEmp(int rtn, Activity_TBMMKT_Model model, string activityId)
        {
            int insertIndex = 1;
            if (model.requestEmpModel != null)
            {
                rtn += deleteRequestEmpByActivityId(activityId);
                foreach (var item in model.requestEmpModel.ToList())
                {
                    RequestEmpModel requestEmpModel = new RequestEmpModel();
                    // requestEmpModel.id = Guid.NewGuid().ToString();
                    requestEmpModel.activityId = activityId;
                    requestEmpModel.rowNo = insertIndex;
                    requestEmpModel.empId = item.empId;
                    requestEmpModel.delFlag = false;
                    requestEmpModel.createdByUserId = model.activityFormModel.createdByUserId;
                    requestEmpModel.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
                    requestEmpModel.updatedByUserId = UtilsAppCode.Session.User.empId;
                    requestEmpModel.updatedDate = DateTime.Now;

                    rtn += insertRequestEmp(requestEmpModel);

                    insertIndex++;
                }
            }
            return rtn;
        }
        public static int ProcessInsertPlaceDetail(int rtn, Activity_TBMMKT_Model model, string activityId)
        {
            int insertIndex = 1;
            if (model.placeDetailModel != null)
            {
                rtn += deletePlaceDetailByActivityId(activityId);
                foreach (var item in model.placeDetailModel.ToList())
                {
                    //     model.activityFormModel.activityPeriodSt 
                    //         = string.IsNullOrEmpty(item.departureDate) ? (DateTime?)null :
                    //BaseAppCodes.converStrToDate(item.departureDate);

                    PlaceDetailModel placeDetailModel = new PlaceDetailModel();
                    //placeDetailModel.id = Guid.NewGuid().ToString();
                    placeDetailModel.activityId = activityId;
                    placeDetailModel.rowNo = insertIndex;
                    placeDetailModel.place = item.place;
                    placeDetailModel.forProject = item.forProject;
                    placeDetailModel.period = item.period;
                    // placeDetailModel.departureDate = item.departureDate;
                    placeDetailModel.departureDate = string.IsNullOrEmpty(item.departureDateStr) ? (DateTime?)null :
               BaseAppCodes.converStrToDateTime(item.departureDateStr);
                    placeDetailModel.arrivalDate = string.IsNullOrEmpty(item.arrivalDateStr) ? (DateTime?)null :
               BaseAppCodes.converStrToDateTime(item.arrivalDateStr);
                    placeDetailModel.delFlag = false;
                    placeDetailModel.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate; ;
                    placeDetailModel.createdByUserId = model.activityFormModel.createdByUserId;
                    placeDetailModel.updatedDate = DateTime.Now;
                    placeDetailModel.updatedByUserId = UtilsAppCode.Session.User.empId;

                    rtn += insertPlaceDetail(placeDetailModel);

                    insertIndex++;
                }
            }
            return rtn;
        }
        public static int ProcessInsertPurpose(int rtn, Activity_TBMMKT_Model model, string activityId)
        {
            if (model.chkPurpose != null)
            {
                if (model.chkPurpose.Any())
                {
                    rtn += deletePurposeByActivityId(activityId);
                    foreach (var item in model.chkPurpose)
                    {

                        PurposeModel purposeModel = new PurposeModel();

                        purposeModel.activityId = activityId;
                        //purposeModel.rowNo = insertIndex;
                        purposeModel.id = item;
                        //  purposeModel.status =item.status;
                        purposeModel.delFlag = false;
                        purposeModel.createdByUserId = model.activityFormModel.createdByUserId;
                        purposeModel.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
                        purposeModel.updatedByUserId = UtilsAppCode.Session.User.empId;
                        purposeModel.updatedDate = DateTime.Now;

                        rtn += insertPurpose(purposeModel);
                    }
                }
            }

            return rtn;
        }

        protected static int insertRequestEmp(RequestEmpModel model)
        {
            int result = 0;

            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertRequestEmp"
                    , new SqlParameter[] {new SqlParameter("@activityId",model.activityId)
                    ,new SqlParameter("@rowNo",model.rowNo)
                    ,new SqlParameter("@empId ",model.empId)
                    ,new SqlParameter("@delFlag",model.delFlag)
                    ,new SqlParameter("@createdDate",model.createdDate)
                    ,new SqlParameter("@createdByUserId",model.createdByUserId)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)

                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertEstimateTBMMKT");
            }

            return result;
        }
        protected static int insertPlaceDetail(PlaceDetailModel model)
        {
            int result = 0;

            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertPlaceDetail"
                    , new SqlParameter[] {new SqlParameter("@activityId",model.activityId)
                    ,new SqlParameter("@rowNo",model.rowNo)
                    ,new SqlParameter("@place",model.place)
                    ,new SqlParameter("@forProject",model.forProject)
                    ,new SqlParameter("@period",model.period)
                    ,new SqlParameter("@departureDate",model.departureDate)
                    ,new SqlParameter("@arrivalDate",model.arrivalDate)
                    ,new SqlParameter("@delFlag",model.delFlag)
                    ,new SqlParameter("@createdDate",model.createdDate)
                    ,new SqlParameter("@createdByUserId",model.createdByUserId)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)

                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertPlaceDetail");
            }

            return result;
        }
        protected static int insertPurpose(PurposeModel model)
        {
            int result = 0;

            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertPurpose"
                    , new SqlParameter[] {new SqlParameter("@activityId",model.activityId)
                    //,new SqlParameter("@rowNo",model.rowNo)
                    ,new SqlParameter("@purposeId ",model.id)
                    ,new SqlParameter("@delFlag",model.delFlag)
                    ,new SqlParameter("@createdDate",model.createdDate)
                    ,new SqlParameter("@createdByUserId",model.createdByUserId)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)

                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertPurpose");
            }

            return result;
        }
        #endregion
    }

}