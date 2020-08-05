using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
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
                else if (model.activityFormModel.mode == AppCode.Mode.edit.ToString() && ActFormAppCode.checkCanEditByUser(activityId))
                {
                    rtn = ProcessInsertEstimate(rtn, model, activityId);
                }
                else
                {
                    model.activityFormTBMMKT.id = activityId;
                    model.activityFormTBMMKT.statusId = 1;
                    model.activityFormTBMMKT.documentDate = BaseAppCodes.converStrToDatetimeWithFormat(model.activityFormModel.documentDateStr, ConfigurationManager.AppSettings["formatDateUse"]);
                    model.activityFormTBMMKT.activityPeriodSt = string.IsNullOrEmpty(model.activityFormModel.activityPeriodStStr) ? (DateTime?)null : BaseAppCodes.converStrToDatetimeWithFormat(model.activityFormModel.activityPeriodStStr, ConfigurationManager.AppSettings["formatDateUse"]);
                    model.activityFormTBMMKT.activityPeriodEnd = string.IsNullOrEmpty(model.activityFormModel.activityPeriodEndStr) ? (DateTime?)null : BaseAppCodes.converStrToDatetimeWithFormat(model.activityFormModel.activityPeriodEndStr, ConfigurationManager.AppSettings["formatDateUse"]);
                    model.activityFormTBMMKT.costPeriodSt = string.IsNullOrEmpty(model.activityFormModel.str_costPeriodSt) ? (DateTime?)null : BaseAppCodes.converStrToDatetimeWithFormat(model.activityFormModel.str_costPeriodSt, ConfigurationManager.AppSettings["formatDateUse"]);
                    model.activityFormTBMMKT.costPeriodEnd = string.IsNullOrEmpty(model.activityFormModel.str_costPeriodEnd) ? (DateTime?)null : BaseAppCodes.converStrToDatetimeWithFormat(model.activityFormModel.str_costPeriodEnd, ConfigurationManager.AppSettings["formatDateUse"]);
                    model.activityFormTBMMKT.activityNo = string.IsNullOrEmpty(model.activityFormModel.activityNo) ? "---" : model.activityFormModel.activityNo;
                    model.activityFormTBMMKT.createdByUserId = model.activityFormModel.createdByUserId != null ? model.activityFormModel.createdByUserId : UtilsAppCode.Session.User.empId;
                    model.activityFormTBMMKT.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
                    model.activityFormTBMMKT.updatedByUserId = UtilsAppCode.Session.User.empId;
                    model.activityFormTBMMKT.updatedDate = DateTime.Now;
                    model.activityFormTBMMKT.delFlag = false;
                    model.activityFormTBMMKT.companyId = model.activityFormTBMMKT.formCompanyId;
                    model.activityFormTBMMKT.remark = model.activityFormModel.remark;
                    model.activityFormTBMMKT.master_type_form_id = model.activityFormTBMMKT.master_type_form_id == null ? "" : model.activityFormTBMMKT.master_type_form_id;
                    model.activityFormTBMMKT.languageDoc = model.activityFormTBMMKT.languageDoc == null ? "" : model.activityFormTBMMKT.languageDoc;
                    model.activityFormTBMMKT.piorityDoc = model.activityFormTBMMKT.piorityDoc == null ? "" : model.activityFormTBMMKT.piorityDoc;
                    model.activityFormTBMMKT.statusNote = model.activityFormTBMMKT.statusNote == null ? "" : model.activityFormTBMMKT.statusNote;

                    rtn = insertActivityForm(model.activityFormTBMMKT);

                    rtn = ProcessInsertTB_Act_ActivityForm_DetailOther(rtn, model, activityId);
                    rtn = ProcessInsertTB_Act_ActivityForm_DetailOtherList(rtn, model, activityId);
                    rtn = ProcessInsertEstimate(rtn, model, activityId);

                    rtn = ProcessInsertTB_Act_ActivityChoiceSelect(rtn, model, activityId);

                    rtn = ProcessInsertRequestEmp(rtn, model, activityId);
                    rtn = ProcessInsertPlaceDetail(rtn, model, activityId);
                    rtn = ProcessInsertPurpose(rtn, model, activityId);
                    rtn = ProcessInsertProduct(rtn, model, activityId);
                    rtn = ProcessInsertCliamIO(rtn, model, activityId);

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
            if (model.activityOfEstimateList != null)
            {
                rtn += deleteActivityOfEstimateByActivityId(activityId);
                rtn += insertEstimateToStored(model.activityOfEstimateList, activityId, string.IsNullOrEmpty(model.activityFormModel.createdByUserId) ? model.activityFormTBMMKT.createdByUserId : model.activityFormModel.createdByUserId, model.activityFormModel.createdDate);
            }
            if (model.activityOfEstimateList2 != null)
            {
                rtn += insertEstimateToStored(model.activityOfEstimateList2, activityId, model.activityFormModel.createdByUserId, model.activityFormModel.createdDate);
            }

            return rtn;
        }

        public static int insertEstimateToStored(List<CostThemeDetailOfGroupByPriceTBMMKT> activityOfEstimateList, string activityId, string createdByUserId, DateTime? createdDate)
        {
            int rtn = 0;
            int insertIndex = 1;

            foreach (var item in activityOfEstimateList.ToList())
            {

                CostThemeDetailOfGroupByPriceTBMMKT costThemeDetail = new CostThemeDetailOfGroupByPriceTBMMKT();
                costThemeDetail.id = Guid.NewGuid().ToString();
                costThemeDetail.activityId = activityId;
                costThemeDetail.activityTypeId = item.activityTypeId;
                costThemeDetail.productDetail = item.productDetail;
                costThemeDetail.total = item.total == null ? 0 : item.total;
                costThemeDetail.normalCost = item.normalCost == null ? 0 : item.normalCost;
                costThemeDetail.IO = item.IO;
                costThemeDetail.rowNo = insertIndex;
                costThemeDetail.delFlag = false;
                costThemeDetail.createdByUserId = createdByUserId;
                costThemeDetail.createdDate = createdDate == null ? DateTime.Now : createdDate;
                costThemeDetail.updatedByUserId = UtilsAppCode.Session.User.empId;
                costThemeDetail.updatedDate = DateTime.Now;
                costThemeDetail.unit = item.unit;
                costThemeDetail.unitPrice = item.unitPriceDisplay == null ? 0 : decimal.Parse(item.unitPriceDisplay.Replace(",", ""));
                costThemeDetail.QtyName = item.QtyName;
                costThemeDetail.remark = item.remark == null ? "" : item.remark;
                costThemeDetail.productId = item.productId == null ? "" : item.productId;
                costThemeDetail.typeTheme = item.typeTheme;
                costThemeDetail.date = string.IsNullOrEmpty(item.dateInput) ? (DateTime?)null : BaseAppCodes.converStrToDatetimeWithFormat(item.dateInput, ConfigurationManager.AppSettings["formatDateUse"]);
                costThemeDetail.detail = item.detail;
                costThemeDetail.listChoiceId = item.listChoiceId;
                costThemeDetail.compensate = item.compensate;
                costThemeDetail.glCode = item.glCode;
                costThemeDetail.hospId = item.hospId;
                costThemeDetail.UseYearSelect = item.UseYearSelect == null ? "" : item.UseYearSelect;
                costThemeDetail.EO = item.EO == null ? "" : item.EO;
                rtn += insertEstimate(costThemeDetail);
                insertIndex++;
            }

            return insertIndex;
        }

        public static int processInsertTBProduct(int rtn, Activity_TBMMKT_Model model, string activityId)
        {
            int insertIndex = 1;
            List<ProductCostOfGroupByPrice> insertProductlist = new List<ProductCostOfGroupByPrice>();
            if (model.productcostdetaillist1 != null)
            {
                rtn += deleteActivityOfProductByActivityId(activityId);
                foreach (var item in model.productcostdetaillist1.ToList())
                {
                    ProductCostOfGroupByPrice productcostdetail = new ProductCostOfGroupByPrice();
                    productcostdetail.id = item.id;
                    productcostdetail.productGroupId = item.productGroupId;
                    productcostdetail.activityId = activityId;
                    productcostdetail.productId = item.productId;
                    productcostdetail.wholeSalesPrice = item.wholeSalesPrice;
                    productcostdetail.saleIn = item.saleIn;
                    productcostdetail.saleOut = item.saleNormal;
                    productcostdetail.disCount1 = item.disCount1;
                    productcostdetail.disCount2 = item.disCount2;
                    productcostdetail.disCount3 = item.disCount3;
                    productcostdetail.normalCost = item.normalCost;
                    productcostdetail.normalGp = item.normalGp;
                    productcostdetail.promotionGp = item.promotionGp;
                    productcostdetail.specialDisc = item.specialDisc;
                    productcostdetail.specialDiscBaht = item.specialDiscBaht;
                    productcostdetail.promotionCost = item.promotionCost;
                    productcostdetail.isShowGroup = item.isShowGroup;
                    productcostdetail.rowNo = insertIndex;
                    productcostdetail.delFlag = item.delFlag;
                    productcostdetail.createdByUserId = model.activityFormModel.createdByUserId;
                    productcostdetail.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
                    productcostdetail.updatedByUserId = UtilsAppCode.Session.User.empId;
                    productcostdetail.updatedDate = DateTime.Now;

                    rtn += insertTBProduct(productcostdetail);

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

                tB_Act_ActivityForm_DetailOther.SubjectId = string.IsNullOrEmpty(model.activityFormTBMMKT.SubjectId) ? model.tB_Act_ActivityForm_DetailOther.SubjectId : model.activityFormTBMMKT.SubjectId;
                tB_Act_ActivityForm_DetailOther.activityProduct = model.tB_Act_ActivityForm_DetailOther.activityProduct;
                tB_Act_ActivityForm_DetailOther.activityTel = model.tB_Act_ActivityForm_DetailOther.activityTel;
                tB_Act_ActivityForm_DetailOther.IO = model.tB_Act_ActivityForm_DetailOther.IO;
                tB_Act_ActivityForm_DetailOther.EO = model.tB_Act_ActivityForm_DetailOther.EO;
                tB_Act_ActivityForm_DetailOther.descAttach = model.tB_Act_ActivityForm_DetailOther.descAttach;
                tB_Act_ActivityForm_DetailOther.delFlag = false;
                tB_Act_ActivityForm_DetailOther.createdByUserId = string.IsNullOrEmpty(model.activityFormModel.createdByUserId) ? model.activityFormTBMMKT.createdByUserId : model.activityFormModel.createdByUserId;
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
                tB_Act_ActivityForm_DetailOther.fiscalYear = model.tB_Act_ActivityForm_DetailOther.fiscalYear;
                tB_Act_ActivityForm_DetailOther.APCode = model.tB_Act_ActivityForm_DetailOther.APCode;
                tB_Act_ActivityForm_DetailOther.payNo = model.tB_Act_ActivityForm_DetailOther.payNo;
                tB_Act_ActivityForm_DetailOther.activityIdNoSub = model.tB_Act_ActivityForm_DetailOther.activityIdNoSub;
                tB_Act_ActivityForm_DetailOther.totalnormalCostEstimate = model.tB_Act_ActivityForm_DetailOther.totalnormalCostEstimate == null ? 0 : model.tB_Act_ActivityForm_DetailOther.totalnormalCostEstimate;
                tB_Act_ActivityForm_DetailOther.totalvat = model.tB_Act_ActivityForm_DetailOther.totalvat == null ? 0 : model.tB_Act_ActivityForm_DetailOther.totalvat;
                tB_Act_ActivityForm_DetailOther.totalnormalCostEstimateWithVat = model.tB_Act_ActivityForm_DetailOther.totalnormalCostEstimateWithVat == null ? 0 : model.tB_Act_ActivityForm_DetailOther.totalnormalCostEstimateWithVat;
                tB_Act_ActivityForm_DetailOther.totalallPayByIO = model.tB_Act_ActivityForm_DetailOther.totalallPayByIO == null ? 0 : model.tB_Act_ActivityForm_DetailOther.totalallPayByIO;
                tB_Act_ActivityForm_DetailOther.totalallPayNo = model.tB_Act_ActivityForm_DetailOther.totalallPayNo == null ? 0 : model.tB_Act_ActivityForm_DetailOther.totalallPayNo;
                tB_Act_ActivityForm_DetailOther.totalallPayByIOBalance = model.tB_Act_ActivityForm_DetailOther.totalallPayByIOBalance == null ? 0 : model.tB_Act_ActivityForm_DetailOther.totalallPayByIOBalance;
                tB_Act_ActivityForm_DetailOther.orderOf = model.tB_Act_ActivityForm_DetailOther.orderOf;
                tB_Act_ActivityForm_DetailOther.regionalId = model.tB_Act_ActivityForm_DetailOther.regionalId;
                tB_Act_ActivityForm_DetailOther.departmentId = model.tB_Act_ActivityForm_DetailOther.departmentId == null ? "" : model.tB_Act_ActivityForm_DetailOther.departmentId;
                tB_Act_ActivityForm_DetailOther.other1 = model.tB_Act_ActivityForm_DetailOther.other1 == null ? "" : model.tB_Act_ActivityForm_DetailOther.other1;
                tB_Act_ActivityForm_DetailOther.other2 = model.tB_Act_ActivityForm_DetailOther.other2 == null ? "" : model.tB_Act_ActivityForm_DetailOther.other2;
                tB_Act_ActivityForm_DetailOther.hospPercent = model.tB_Act_ActivityForm_DetailOther.hospPercent;
                tB_Act_ActivityForm_DetailOther.amount = model.tB_Act_ActivityForm_DetailOther.amount;
                tB_Act_ActivityForm_DetailOther.amountLimit = model.tB_Act_ActivityForm_DetailOther.amountLimit;
                tB_Act_ActivityForm_DetailOther.amountCumulative = model.tB_Act_ActivityForm_DetailOther.amountCumulative;
                tB_Act_ActivityForm_DetailOther.amountBalance = model.tB_Act_ActivityForm_DetailOther.amountBalance;
                tB_Act_ActivityForm_DetailOther.amountReceived = model.tB_Act_ActivityForm_DetailOther.amountReceived;
                tB_Act_ActivityForm_DetailOther.departmentIdFlow = model.tB_Act_ActivityForm_DetailOther.departmentIdFlow == null ? "" : model.tB_Act_ActivityForm_DetailOther.departmentIdFlow;

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

            if (model.activityFormTBMMKT.list_0_select != null
                || model.activityFormTBMMKT.list_1_select != null
                || model.activityFormTBMMKT.list_2_select != null
                || model.activityFormTBMMKT.list_3_select != null
                || model.activityFormTBMMKT.list_0_multi_select != null
                || model.activityFormTBMMKT.list_1_multi_select != null
                || model.activityFormTBMMKT.list_2_multi_select != null
                || model.activityFormTBMMKT.list_3_multi_select != null
                || model.activityFormTBMMKT.list_4_multi_select != null
                || model.activityFormTBMMKT.list_5_multi_select != null
                || model.activityFormTBMMKT.list_6_multi_select != null
                || model.activityFormTBMMKT.list_7_multi_select != null
                || model.activityFormTBMMKT.list_8_multi_select != null
                || model.activityFormTBMMKT.brand_select != null
                || model.activityFormTBMMKT.list_chooseRequest_multi_select != null
                 )
            {
                rtn += deleteActivityTB_Act_ActivityChoiceSelect(activityId);
                okProcessInsert = true;
            }

            if (okProcessInsert == true)
            {
                if (model.activityFormTBMMKT.list_0_select != "")//สต๊อก[ฟอร์มPOS,premium]
                {
                    tB_Act_ActivityChoiceSelectModel.id = Guid.NewGuid().ToString();
                    tB_Act_ActivityChoiceSelectModel.actFormId = activityId;
                    tB_Act_ActivityChoiceSelectModel.select_list_choice_id = model.activityFormTBMMKT.list_0_select;
                    rtn += insertActivityChoiceSelect(tB_Act_ActivityChoiceSelectModel);
                }
                if (model.activityFormTBMMKT.list_1_select != "")//VAT[ฟอร์มใบสั่งจ่าย]
                {
                    tB_Act_ActivityChoiceSelectModel.id = Guid.NewGuid().ToString();
                    tB_Act_ActivityChoiceSelectModel.actFormId = activityId;
                    tB_Act_ActivityChoiceSelectModel.select_list_choice_id = model.activityFormTBMMKT.list_1_select;
                    rtn += insertActivityChoiceSelect(tB_Act_ActivityChoiceSelectModel);
                }
                if (model.activityFormTBMMKT.list_0_multi_select != null)//ฟอร์มIT314_(Procure to Pay)
                {
                    if (model.activityFormTBMMKT.list_0_multi_select.Length > 0)
                    {
                        for (int i = 0; i < model.activityFormTBMMKT.list_0_multi_select.Length; i++)
                        {
                            tB_Act_ActivityChoiceSelectModel.id = Guid.NewGuid().ToString();
                            tB_Act_ActivityChoiceSelectModel.actFormId = activityId;
                            tB_Act_ActivityChoiceSelectModel.select_list_choice_id = model.activityFormTBMMKT.list_0_multi_select[i];
                            rtn += insertActivityChoiceSelect(tB_Act_ActivityChoiceSelectModel);
                        }
                    }
                }
                if (model.activityFormTBMMKT.list_1_multi_select != null)//ฟอร์มIT314_(ERP System)
                {
                    if (model.activityFormTBMMKT.list_1_multi_select.Length > 0)
                    {
                        for (int i = 0; i < model.activityFormTBMMKT.list_1_multi_select.Length; i++)
                        {
                            tB_Act_ActivityChoiceSelectModel.id = Guid.NewGuid().ToString();
                            tB_Act_ActivityChoiceSelectModel.actFormId = activityId;
                            tB_Act_ActivityChoiceSelectModel.select_list_choice_id = model.activityFormTBMMKT.list_1_multi_select[i];
                            rtn += insertActivityChoiceSelect(tB_Act_ActivityChoiceSelectModel);
                        }
                    }
                }
                if (model.activityFormTBMMKT.list_2_multi_select != null)//ฟอร์มIT314_(Order to Cash)
                {
                    if (model.activityFormTBMMKT.list_2_multi_select.Length > 0)
                    {
                        for (int i = 0; i < model.activityFormTBMMKT.list_2_multi_select.Length; i++)
                        {
                            tB_Act_ActivityChoiceSelectModel.id = Guid.NewGuid().ToString();
                            tB_Act_ActivityChoiceSelectModel.actFormId = activityId;
                            tB_Act_ActivityChoiceSelectModel.select_list_choice_id = model.activityFormTBMMKT.list_2_multi_select[i];
                            rtn += insertActivityChoiceSelect(tB_Act_ActivityChoiceSelectModel);
                        }
                    }
                }
                if (model.activityFormTBMMKT.list_3_multi_select != null)//ฟอร์มIT314_(Forecast to Delivery)
                {
                    if (model.activityFormTBMMKT.list_3_multi_select.Length > 0)
                    {
                        for (int i = 0; i < model.activityFormTBMMKT.list_3_multi_select.Length; i++)
                        {
                            tB_Act_ActivityChoiceSelectModel.id = Guid.NewGuid().ToString();
                            tB_Act_ActivityChoiceSelectModel.actFormId = activityId;
                            tB_Act_ActivityChoiceSelectModel.select_list_choice_id = model.activityFormTBMMKT.list_3_multi_select[i];
                            rtn += insertActivityChoiceSelect(tB_Act_ActivityChoiceSelectModel);
                        }
                    }
                }
                if (model.activityFormTBMMKT.list_4_multi_select != null)//ฟอร์มIT314_(Finance&Accounting)
                {
                    if (model.activityFormTBMMKT.list_4_multi_select.Length > 0)
                    {
                        for (int i = 0; i < model.activityFormTBMMKT.list_4_multi_select.Length; i++)
                        {
                            tB_Act_ActivityChoiceSelectModel.id = Guid.NewGuid().ToString();
                            tB_Act_ActivityChoiceSelectModel.actFormId = activityId;
                            tB_Act_ActivityChoiceSelectModel.select_list_choice_id = model.activityFormTBMMKT.list_4_multi_select[i];
                            rtn += insertActivityChoiceSelect(tB_Act_ActivityChoiceSelectModel);
                        }
                    }
                }
                if (model.activityFormTBMMKT.list_5_multi_select != null)//ฟอร์มIT314_(Human Capital)
                {
                    if (model.activityFormTBMMKT.list_5_multi_select.Length > 0)
                    {
                        for (int i = 0; i < model.activityFormTBMMKT.list_5_multi_select.Length; i++)
                        {
                            tB_Act_ActivityChoiceSelectModel.id = Guid.NewGuid().ToString();
                            tB_Act_ActivityChoiceSelectModel.actFormId = activityId;
                            tB_Act_ActivityChoiceSelectModel.select_list_choice_id = model.activityFormTBMMKT.list_5_multi_select[i];
                            rtn += insertActivityChoiceSelect(tB_Act_ActivityChoiceSelectModel);
                        }
                    }
                }
                if (model.activityFormTBMMKT.list_6_multi_select != null)//ฟอร์มIT314_(Other)
                {
                    if (model.activityFormTBMMKT.list_6_multi_select.Length > 0)
                    {
                        for (int i = 0; i < model.activityFormTBMMKT.list_6_multi_select.Length; i++)
                        {
                            tB_Act_ActivityChoiceSelectModel.id = Guid.NewGuid().ToString();
                            tB_Act_ActivityChoiceSelectModel.actFormId = activityId;
                            tB_Act_ActivityChoiceSelectModel.select_list_choice_id = model.activityFormTBMMKT.list_6_multi_select[i];
                            rtn += insertActivityChoiceSelect(tB_Act_ActivityChoiceSelectModel);
                        }
                    }
                }
                if (model.activityFormTBMMKT.list_7_multi_select != null)//ฟอร์มIT314_(Change type / ประเภทของการเปลี่ยนแปลง)
                {
                    if (model.activityFormTBMMKT.list_7_multi_select.Length > 0)
                    {
                        for (int i = 0; i < model.activityFormTBMMKT.list_7_multi_select.Length; i++)
                        {
                            tB_Act_ActivityChoiceSelectModel.id = Guid.NewGuid().ToString();
                            tB_Act_ActivityChoiceSelectModel.actFormId = activityId;
                            tB_Act_ActivityChoiceSelectModel.select_list_choice_id = model.activityFormTBMMKT.list_7_multi_select[i];
                            rtn += insertActivityChoiceSelect(tB_Act_ActivityChoiceSelectModel);
                        }
                    }
                }
                if (model.activityFormTBMMKT.list_8_multi_select != null)//ฟอร์มIT314_(Change Authorizations / สิทธิ์การใช้งาน)
                {
                    if (model.activityFormTBMMKT.list_8_multi_select.Length > 0)
                    {
                        for (int i = 0; i < model.activityFormTBMMKT.list_8_multi_select.Length; i++)
                        {
                            tB_Act_ActivityChoiceSelectModel.id = Guid.NewGuid().ToString();
                            tB_Act_ActivityChoiceSelectModel.actFormId = activityId;
                            tB_Act_ActivityChoiceSelectModel.select_list_choice_id = model.activityFormTBMMKT.list_8_multi_select[i];
                            rtn += insertActivityChoiceSelect(tB_Act_ActivityChoiceSelectModel);
                        }
                    }
                }
                if (model.activityFormTBMMKT.list_2_select != null)
                {
                    if (model.activityFormTBMMKT.list_2_select != "")//เพื่อ[ฟอร์มPOS,premium]
                    {
                        tB_Act_ActivityChoiceSelectModel.id = Guid.NewGuid().ToString();
                        tB_Act_ActivityChoiceSelectModel.actFormId = activityId;
                        tB_Act_ActivityChoiceSelectModel.select_list_choice_id = model.activityFormTBMMKT.list_2_select;
                        rtn += insertActivityChoiceSelect(tB_Act_ActivityChoiceSelectModel);
                    }
                }
                if (model.activityFormTBMMKT.brand_select != null)
                {
                    if (model.activityFormTBMMKT.brand_select != "")//Brand/ผลิตภัณฑ์ [ฟอร์มPOS,premium]
                    {
                        tB_Act_ActivityChoiceSelectModel.id = Guid.NewGuid().ToString();
                        tB_Act_ActivityChoiceSelectModel.actFormId = activityId;
                        tB_Act_ActivityChoiceSelectModel.select_list_choice_id = model.activityFormTBMMKT.brand_select;
                        rtn += insertActivityChoiceSelect(tB_Act_ActivityChoiceSelectModel);
                    }
                }
                if (model.activityFormTBMMKT.list_3_select != null)
                {
                    if (model.activityFormTBMMKT.list_3_select != "")//Channel+Region [ฟอร์มPOS,premium]
                    {
                        tB_Act_ActivityChoiceSelectModel.id = Guid.NewGuid().ToString();
                        tB_Act_ActivityChoiceSelectModel.actFormId = activityId;
                        tB_Act_ActivityChoiceSelectModel.select_list_choice_id = model.activityFormTBMMKT.list_3_select;
                        rtn += insertActivityChoiceSelect(tB_Act_ActivityChoiceSelectModel);
                    }
                }
                if (model.activityFormTBMMKT.list_chooseRequest_multi_select != null)
                {
                    if (model.activityFormTBMMKT.list_chooseRequest_multi_select.Length > 0)
                    {
                        for (int i = 0; i < model.activityFormTBMMKT.list_chooseRequest_multi_select.Length; i++)
                        {
                            tB_Act_ActivityChoiceSelectModel.id = Guid.NewGuid().ToString();
                            tB_Act_ActivityChoiceSelectModel.actFormId = activityId;
                            tB_Act_ActivityChoiceSelectModel.select_list_choice_id = model.activityFormTBMMKT.list_chooseRequest_multi_select[i];
                            rtn += insertActivityChoiceSelect(tB_Act_ActivityChoiceSelectModel);
                        }
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
                string en = ConfigurationManager.AppSettings["cultureEng"];
                int index_each = 0;
                string sumTxtLabelRequired = "";

                activity_TBMMKT_Model.activityFormTBMMKT = QueryGetActivityByIdTBMMKT.getActivityById(activityId).FirstOrDefault(); // TB_Act_ActivityForm
                activity_TBMMKT_Model.activityFormModel = activity_TBMMKT_Model.activityFormTBMMKT;
                activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther = QueryGetActivityFormDetailOtherByActivityId.getByActivityId(activityId).FirstOrDefault(); // TB_Act_ActivityForm_DetailOther                
                activity_TBMMKT_Model.activityOfEstimateList = QueryGetActivityEstimateByActivityId.getByActivityId(activityId);  //TB_Act_ActivityOfEstimate
                activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel = QueryGet_TB_Act_ActivityChoiceSelect.get_TB_Act_ActivityChoiceSelectModel(activityId);
                activity_TBMMKT_Model.activityFormTBMMKT.chkUseEng = DocumentsAppCode.checkLanguageDoc(activity_TBMMKT_Model.activityFormTBMMKT.languageDoc, en, activity_TBMMKT_Model.activityFormTBMMKT.statusId);
                activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOtherList = QueryGet_TB_Act_ActivityForm_DetailOtherList.get_TB_Act_ActivityForm_DetailOtherList(activityId);

                if (activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Count > 0)
                {
                    if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPosTbmId"])
                    {
                        activity_TBMMKT_Model.activityFormTBMMKT.list_0_select = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "in_or_out_stock").FirstOrDefault().select_list_choice_id;
                        activity_TBMMKT_Model.activityFormTBMMKT.labelInOrOutStock = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "in_or_out_stock").FirstOrDefault().name;
                        var countlist_1_multi_select = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "product_pos_premium").Count();
                        activity_TBMMKT_Model.activityFormTBMMKT.list_1_multi_select = new string[countlist_1_multi_select];
                        index_each = 0;
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
                    else if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPaymentVoucherTbmId"])
                    {
                        var countlist_2_multi_select = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "attachPV").Count();
                        activity_TBMMKT_Model.activityFormTBMMKT.list_2_multi_select = new string[countlist_2_multi_select];
                        index_each = 0;
                        foreach (var item in activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "attachPV").ToList())
                        {
                            activity_TBMMKT_Model.activityFormTBMMKT.list_2_multi_select[index_each] = item.select_list_choice_id;
                            index_each++;
                        }
                    }
                    else if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formCR_IT_FRM_314"])
                    {
                        index_each = 0;
                        List<TB_Act_ActivityChoiceSelectModel> tempList = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "ChooseRequest").ToList();
                        if (tempList.Count > 0)
                        {
                            activity_TBMMKT_Model.activityFormTBMMKT.list_chooseRequest_multi_select = new string[tempList.Count];
                            foreach (var item in tempList)
                            {
                                activity_TBMMKT_Model.activityFormTBMMKT.list_chooseRequest_multi_select[index_each] = item.select_list_choice_id;
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
                        }

                        index_each = 0;
                        tempList = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "ProcuretoPay").ToList();
                        if (tempList.Count > 0)
                        {
                            activity_TBMMKT_Model.activityFormTBMMKT.list_0_multi_select = new string[tempList.Count];
                            foreach (var item in tempList)
                            {
                                activity_TBMMKT_Model.activityFormTBMMKT.list_0_multi_select[index_each] = item.select_list_choice_id;
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
                        }

                        index_each = 0;
                        tempList = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "ERPSystem").ToList();
                        if (tempList.Count > 0)
                        {
                            activity_TBMMKT_Model.activityFormTBMMKT.list_1_multi_select = new string[tempList.Count];
                            foreach (var item in tempList)
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
                        }

                        index_each = 0;
                        tempList = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "OrdertoCash").ToList();
                        if (tempList.Count > 0)
                        {
                            activity_TBMMKT_Model.activityFormTBMMKT.list_2_multi_select = new string[tempList.Count];
                            foreach (var item in tempList)
                            {
                                activity_TBMMKT_Model.activityFormTBMMKT.list_2_multi_select[index_each] = item.select_list_choice_id;
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
                        }


                        index_each = 0;
                        tempList = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "ForecasttoDelivery").ToList();
                        if (tempList.Count > 0)
                        {
                            activity_TBMMKT_Model.activityFormTBMMKT.list_3_multi_select = new string[tempList.Count];
                            foreach (var item in tempList)
                            {
                                activity_TBMMKT_Model.activityFormTBMMKT.list_3_multi_select[index_each] = item.select_list_choice_id;
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
                        }


                        index_each = 0;
                        tempList = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "FinanceAndAccounting").ToList();
                        if (tempList.Count > 0)
                        {
                            activity_TBMMKT_Model.activityFormTBMMKT.list_4_multi_select = new string[tempList.Count];
                            foreach (var item in tempList)
                            {
                                activity_TBMMKT_Model.activityFormTBMMKT.list_4_multi_select[index_each] = item.select_list_choice_id;
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
                        }


                        index_each = 0;
                        tempList = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "HumanCapital").ToList();
                        if (tempList.Count > 0)
                        {
                            activity_TBMMKT_Model.activityFormTBMMKT.list_5_multi_select = new string[tempList.Count];
                            foreach (var item in tempList)
                            {
                                activity_TBMMKT_Model.activityFormTBMMKT.list_5_multi_select[index_each] = item.select_list_choice_id;
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
                        }

                        index_each = 0;
                        tempList = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "Other").ToList();
                        if (tempList.Count > 0)
                        {
                            activity_TBMMKT_Model.activityFormTBMMKT.list_6_multi_select = new string[tempList.Count];
                            foreach (var item in tempList)
                            {
                                activity_TBMMKT_Model.activityFormTBMMKT.list_6_multi_select[index_each] = item.select_list_choice_id;
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
                        }


                        index_each = 0;
                        tempList = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "Changetype").ToList();
                        if (tempList.Count > 0)
                        {
                            activity_TBMMKT_Model.activityFormTBMMKT.list_7_multi_select = new string[tempList.Count];
                            foreach (var item in tempList)
                            {
                                activity_TBMMKT_Model.activityFormTBMMKT.list_7_multi_select[index_each] = item.select_list_choice_id;
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
                        }


                        index_each = 0;
                        tempList = activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel.Where(x => x.type == "ChangeAuthorizations").ToList();
                        if (tempList.Count > 0)
                        {
                            activity_TBMMKT_Model.activityFormTBMMKT.list_8_multi_select = new string[tempList.Count];
                            foreach (var item in tempList)
                            {
                                activity_TBMMKT_Model.activityFormTBMMKT.list_8_multi_select[index_each] = item.select_list_choice_id;
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
                        }

                    }
                }
                bool chk = AppCode.hcForm.Contains(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id);
                activity_TBMMKT_Model.requestEmpModel = QueryGet_ReqEmpByActivityId.getReqEmpByActivityId(activityId, activity_TBMMKT_Model.activityFormTBMMKT.chkUseEng, chk);
                activity_TBMMKT_Model.purposeModel = QueryGet_master_purpose.getPurposeByActivityId(activityId);
                activity_TBMMKT_Model.placeDetailModel = QueryGet_PlaceDetailByActivityId.getPlaceDetailByActivityId(activityId);
                activity_TBMMKT_Model.expensesDetailModel.costDetailLists = activity_TBMMKT_Model.activityOfEstimateList;

                if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formCR_IT_FRM_314"])
                {
                    activity_TBMMKT_Model.dataRequesterToShows = QueryGetSelectMainForm.GetDataRequesterToShow(activityId);
                }

                Decimal? totalCostThisActivity = 0;
                foreach (var item in activity_TBMMKT_Model.activityOfEstimateList)
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

                if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOtherList.Count > 0)
                {
                    if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPaymentVoucherTbmId"])
                    {
                        var countlist_1_multi_select = activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOtherList.Where(x => x.typeKeep == ConfigurationManager.AppSettings["typeEOPaymentVoucher"]).Count();
                        activity_TBMMKT_Model.activityFormTBMMKT.list_1_multi_select = new string[countlist_1_multi_select];
                        index_each = 0;
                        foreach (var item in activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOtherList.Where(x => x.typeKeep == ConfigurationManager.AppSettings["typeEOPaymentVoucher"]).ToList())
                        {
                            activity_TBMMKT_Model.activityFormTBMMKT.list_1_multi_select[index_each] = DocumentsAppCode.formatValueSelectEO_PVForm(item.activityIdEO, item.EO);
                            index_each++;
                        }

                        //=========จากที่SelectทุกTypeมา=หลังจากใช้เสร็จก็กรองเหลือแค่ที่ตนเองจะใช้งาน========
                        activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOtherList = activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOtherList.Where(x => x.typeKeep == ConfigurationManager.AppSettings["typePVSectionThreeToFive"]).ToList();
                        //====END======จากที่SelectทุกTypeมา==หลังจากใช้เสร็จก็กรองเหลือแค่ที่ตนเองจะใช้งาน====
                    }
                }

                //===========Get All EO In Doc=======================
                List<string> templistEoInDoc = new List<string>();
                if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formBgTbmId"] || activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPaymentVoucherTbmId"])
                {
                    if (activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.EO != "" && activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.EO != null)
                    {
                        templistEoInDoc.Add(activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.EO);
                    }
                    foreach (var itemGetEO in activity_TBMMKT_Model.activityOfEstimateList)
                    {
                        if (!templistEoInDoc.Contains(itemGetEO.EO))
                        {
                            templistEoInDoc.Add(itemGetEO.EO);
                        }
                    }
                    activity_TBMMKT_Model.listEoInDoc = templistEoInDoc;
                }
                //===END========Get All EO In Doc=======================


            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> getDataForEditActivityTBMMKT");
            }
            return activity_TBMMKT_Model;
        }


        public static Activity_TBMMKT_Model getDataForEditActivityByActNo(string actNo)
        {
            Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
            try
            {
                string en = ConfigurationManager.AppSettings["cultureEng"];

                activity_TBMMKT_Model.activityFormTBMMKT = QueryGetActivityByActNo.getCheckRefActivityByActNo(actNo).FirstOrDefault(); // TB_Act_ActivityForm
                activity_TBMMKT_Model.activityFormModel = activity_TBMMKT_Model.activityFormTBMMKT;
                activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther = QueryGetActivityFormDetailOtherByActivityId.getByActivityId(activity_TBMMKT_Model.activityFormTBMMKT.id).FirstOrDefault(); // TB_Act_ActivityForm_DetailOther                
                activity_TBMMKT_Model.activityOfEstimateList = QueryGetActivityEstimateByActivityId.getByActivityId(activity_TBMMKT_Model.activityFormTBMMKT.id);  //TB_Act_ActivityOfEstimate
                activity_TBMMKT_Model.tB_Act_ActivityChoiceSelectModel = QueryGet_TB_Act_ActivityChoiceSelect.get_TB_Act_ActivityChoiceSelectModel(activity_TBMMKT_Model.activityFormTBMMKT.id);
                activity_TBMMKT_Model.activityFormTBMMKT.chkUseEng = DocumentsAppCode.checkLanguageDoc(
                activity_TBMMKT_Model.activityFormTBMMKT.languageDoc
                , en
                , activity_TBMMKT_Model.activityFormTBMMKT.statusId);


                bool chk = AppCode.hcForm.Contains(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id);
                activity_TBMMKT_Model.requestEmpModel = QueryGet_ReqEmpByActivityId.getReqEmpByActivityId(activity_TBMMKT_Model.activityFormTBMMKT.id, activity_TBMMKT_Model.activityFormTBMMKT.chkUseEng, chk);
                activity_TBMMKT_Model.purposeModel = QueryGet_master_purpose.getPurposeByActivityId(activity_TBMMKT_Model.activityFormTBMMKT.id);
                activity_TBMMKT_Model.placeDetailModel = QueryGet_PlaceDetailByActivityId.getPlaceDetailByActivityId(activity_TBMMKT_Model.activityFormTBMMKT.id);
                activity_TBMMKT_Model.expensesDetailModel.costDetailLists = activity_TBMMKT_Model.activityOfEstimateList;

                Decimal? totalCostThisActivity = 0;

                if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["masterEmpExpense"])
                {
                    totalCostThisActivity = !string.IsNullOrEmpty(activity_TBMMKT_Model.activityFormTBMMKT.benefit) ? decimal.Parse(activity_TBMMKT_Model.activityFormTBMMKT.benefit) : 0;
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


        public static int insertActivityForm(ActivityFormTBMMKT model)
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
                    ,new SqlParameter("@reference", model.reference)
                    ,new SqlParameter("@languageDoc", (model.languageDoc == null ? "" :model.languageDoc))
                    ,new SqlParameter("@piorityDoc", (model.piorityDoc == null ? "" :model.piorityDoc))
                    ,new SqlParameter("@customerId", model.customerId)
                    ,new SqlParameter("@activityDetail", model.activityDetail)
                    ,new SqlParameter("@costPeriodST", model.costPeriodSt)
                    ,new SqlParameter("@costPeriodEND", model.costPeriodEnd)
                    ,new SqlParameter("@empId", model.empId)
                    ,new SqlParameter("@productCateId", model.productCateId)
                    ,new SqlParameter("@productGroupId", model.productGroupId)
                    ,new SqlParameter("@brandId", model.productBrandId)
                    ,new SqlParameter("@theme", model.theme)
                    ,new SqlParameter("@trade", model.trade)


                    ,new SqlParameter("@statusNote", model.statusNote)
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
                    ,new SqlParameter("@totalnormalCostEstimate",model.totalnormalCostEstimate)
                    ,new SqlParameter("@totalvat",model.totalvat)
                    ,new SqlParameter("@totalnormalCostEstimateWithVat",model.totalnormalCostEstimateWithVat)
                    ,new SqlParameter("@totalallPayByIO",model.totalallPayByIO)
                    ,new SqlParameter("@totalallPayNo",model.totalallPayNo)
                    ,new SqlParameter("@totalallPayByIOBalance",model.totalallPayByIOBalance)
                    ,new SqlParameter("@fiscalYear",model.fiscalYear)
                    ,new SqlParameter("@APCode",model.APCode)
                    ,new SqlParameter("@payNo",model.payNo)
                    ,new SqlParameter("@activityIdNoSub",model.activityIdNoSub)
                    ,new SqlParameter("@delFlag",model.delFlag)
                    ,new SqlParameter("@createdDate",model.createdDate)
                    ,new SqlParameter("@createdByUserId",model.createdByUserId)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)
                    ,new SqlParameter("@orderOf",(model.orderOf == null ? "" : model.orderOf))
                    ,new SqlParameter("@regionalId",(model.regionalId == null ? "" : model.regionalId))
                    ,new SqlParameter("@departmentId",model.departmentId)
                    ,new SqlParameter("@other1",model.other1)
                    ,new SqlParameter("@other2",model.other2)
                    ,new SqlParameter("@hospPercent", model.hospPercent)
                    ,new SqlParameter("@amount",model.amount)
                    ,new SqlParameter("@amountLimit",model.amountLimit)
                    ,new SqlParameter("@amountCumulative", model.amountCumulative)
                    ,new SqlParameter("@amountBalance",model.amountBalance)
                    ,new SqlParameter("@amountReceived", model.amountReceived)
                    ,new SqlParameter("@departmentIdFlow", model.departmentIdFlow)
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
                    ,new SqlParameter("@normalCost",decimal.Parse(string.Format("{0:0.00000}", model.normalCost)))
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
                    ,new SqlParameter("@date",model.date)
                    ,new SqlParameter("@detail",model.detail)
                    ,new SqlParameter("@listChoiceId",(model.listChoiceId == null ? "" : model.listChoiceId))
                    ,new SqlParameter("@compensate",model.compensate)
                    ,new SqlParameter("@glCode",(model.glCode == null ? "" : model.glCode))
                    ,new SqlParameter("@hospId",(model.hospId == null ? "" : model.hospId))
                    ,new SqlParameter("@UseYearSelect",model.UseYearSelect)
                    ,new SqlParameter("@EO",model.EO)
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

        protected static int insertTBProduct(ProductCostOfGroupByPrice model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertProductCostdetail"
                    , new SqlParameter[] {new SqlParameter("@id",model.id)
                    ,new SqlParameter("@activityId",model.activityId)
                    ,new SqlParameter("@productId",model.productId)
                    ,new SqlParameter("@productName",model.productName)
                    ,new SqlParameter("@wholeSalesPrice",model.wholeSalesPrice)
                    ,new SqlParameter("@saleIn",model.saleIn)
                    ,new SqlParameter("@saleOut", model.saleOut)
                    ,new SqlParameter("@disCount1",model.disCount1)
                    ,new SqlParameter("@disCount2",model.disCount2)
                    ,new SqlParameter("@disCount3",model.disCount3)
                    ,new SqlParameter("@normalCost",model.normalCost)
                    ,new SqlParameter("@normalGp",model.normalGp)
                    ,new SqlParameter("@promotionGp",model.promotionGp)
                    ,new SqlParameter("@specialDisc",model.specialDisc)
                    ,new SqlParameter("@specialDiscBaht",model.specialDiscBaht)
                    ,new SqlParameter("@promotionCost",model.promotionCost)
                    ,new SqlParameter("@rowNo",model.rowNo)
                    ,new SqlParameter("@isShowGroup",model.isShowGroup)
                    ,new SqlParameter("@Date",model.DateInput)
                    ,new SqlParameter("@place",model.place)
                    ,new SqlParameter("@detail",model.detail)
                    ,new SqlParameter("@total",model.total)
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

                if (model.activityOfEstimateList != null)
                {
                    foreach (var item in model.activityOfEstimateList.ToList())
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
                    requestEmpModel.empTel = item.empTel;
                    requestEmpModel.detail = item.detail == null ? "" : item.detail;
                    requestEmpModel.delFlag = false;
                    requestEmpModel.createdByUserId = string.IsNullOrEmpty(model.activityFormModel.createdByUserId) ? model.activityFormTBMMKT.createdByUserId : model.activityFormModel.createdByUserId;
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
               BaseAppCodes.converStrToDatetimeWithFormat(item.departureDateStr, ConfigurationManager.AppSettings["formatDatetimeUse"]);
                    placeDetailModel.arrivalDate = string.IsNullOrEmpty(item.arrivalDateStr) ? (DateTime?)null :
               BaseAppCodes.converStrToDatetimeWithFormat(item.arrivalDateStr, ConfigurationManager.AppSettings["formatDatetimeUse"]);
                    placeDetailModel.depart = item.depart;
                    placeDetailModel.arrived = item.arrived;
                    placeDetailModel.delFlag = false;
                    placeDetailModel.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate; ;
                    placeDetailModel.createdByUserId = string.IsNullOrEmpty(model.activityFormModel.createdByUserId) ? model.activityFormTBMMKT.createdByUserId : model.activityFormModel.createdByUserId;
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
                        purposeModel.createdByUserId = string.IsNullOrEmpty(model.activityFormModel.createdByUserId) ? model.activityFormTBMMKT.createdByUserId : model.activityFormModel.createdByUserId;
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
                    ,new SqlParameter("@detail ",model.detail)
                    ,new SqlParameter("@delFlag",model.delFlag)
                    ,new SqlParameter("@createdDate",model.createdDate)
                    ,new SqlParameter("@createdByUserId",model.createdByUserId)
                    ,new SqlParameter("@updatedDate",model.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.updatedByUserId)
                    ,new SqlParameter("@empTel",(model.empTel == null ? "" : model.empTel))
                });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertRequestEmp");
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
                    ,new SqlParameter("@depart",(model.depart == null ? "" : model.depart))
                    ,new SqlParameter("@arrived",(model.arrived == null ? "" : model.arrived))
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

        public static int ProcessInsertTB_Act_ActivityForm_DetailOtherList(int rtn, Activity_TBMMKT_Model model, string activityId)
        {
            TB_Act_ActivityForm_DetailOtherList tB_Act_ActivityForm_DetailOtherList = new TB_Act_ActivityForm_DetailOtherList();
            var indexEach = 0;
            if (model.activityFormTBMMKT.list_1_multi_select != null)
            {
                if (model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPaymentVoucherTbmId"])
                {
                    tB_Act_ActivityForm_DetailOtherList.typeKeep = ConfigurationManager.AppSettings["typeEOPaymentVoucher"];
                    rtn += delete_TB_Act_ActivityForm_DetailOtherList(activityId, tB_Act_ActivityForm_DetailOtherList.typeKeep);
                    for (int i = 0; i < model.activityFormTBMMKT.list_1_multi_select.Length; i++)
                    {
                        string txt_activityIdEO_And_EO = model.activityFormTBMMKT.list_1_multi_select[i]; //update for multi EO devdate 20200713 peerapop
                        string[] splitValue = txt_activityIdEO_And_EO.Split('|');
                        tB_Act_ActivityForm_DetailOtherList.activityId = activityId;
                        tB_Act_ActivityForm_DetailOtherList.rowNo = (i + 1);
                        tB_Act_ActivityForm_DetailOtherList.activityIdEO = splitValue[0]; //update for multi EO devdate 20200713 peerapop ของเดิม tB_Act_ActivityForm_DetailOtherList.activityIdEO  = model.activityFormTBMMKT.list_1_multi_select[i]
                        tB_Act_ActivityForm_DetailOtherList.EO = splitValue[1];
                        tB_Act_ActivityForm_DetailOtherList.IO = "";
                        tB_Act_ActivityForm_DetailOtherList.GL = "";
                        tB_Act_ActivityForm_DetailOtherList.select_list_choice_id_ChReg = "";
                        tB_Act_ActivityForm_DetailOtherList.productBrandId = "";
                        tB_Act_ActivityForm_DetailOtherList.createdByUserId = UtilsAppCode.Session.User.empId;
                        rtn += usp_insertTB_Act_ActivityForm_DetailOtherList(tB_Act_ActivityForm_DetailOtherList);
                    }
                }
            }

            if (model.tB_Act_ActivityForm_DetailOtherList != null)
            {
                if (model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPaymentVoucherTbmId"])
                {
                    tB_Act_ActivityForm_DetailOtherList.typeKeep = ConfigurationManager.AppSettings["typePVSectionThreeToFive"];

                    rtn += delete_TB_Act_ActivityForm_DetailOtherList(activityId, tB_Act_ActivityForm_DetailOtherList.typeKeep);
                    indexEach = 0;
                    foreach (var item in model.tB_Act_ActivityForm_DetailOtherList)
                    {
                        tB_Act_ActivityForm_DetailOtherList.activityId = activityId;
                        tB_Act_ActivityForm_DetailOtherList.rowNo = (indexEach + 1);
                        tB_Act_ActivityForm_DetailOtherList.activityIdEO = "";
                        tB_Act_ActivityForm_DetailOtherList.IO = item.IO;
                        tB_Act_ActivityForm_DetailOtherList.GL = item.GL;
                        tB_Act_ActivityForm_DetailOtherList.select_list_choice_id_ChReg = item.select_list_choice_id_ChReg;
                        tB_Act_ActivityForm_DetailOtherList.productBrandId = item.productBrandId;
                        tB_Act_ActivityForm_DetailOtherList.createdByUserId = UtilsAppCode.Session.User.empId;
                        rtn += usp_insertTB_Act_ActivityForm_DetailOtherList(tB_Act_ActivityForm_DetailOtherList);
                        indexEach++;
                    }
                }
            }

            return rtn;
        }
        public static int delete_TB_Act_ActivityForm_DetailOtherList(string activityId, string typeKeep)
        {

            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_deleteTB_Act_ActivityForm_DetailOtherList"
                    , new SqlParameter[] {new SqlParameter("@activityId",activityId)
                   ,new SqlParameter("@typeKeep",typeKeep)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> delete_TB_Act_ActivityForm_DetailOtherList");
            }

            return result;
        }

        protected static int usp_insertTB_Act_ActivityForm_DetailOtherList(TB_Act_ActivityForm_DetailOtherList model)
        {
            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertTB_Act_ActivityForm_DetailOtherList"
                    , new SqlParameter[] {new SqlParameter("@activityId",model.activityId)
                    ,new SqlParameter("@typeKeep",model.typeKeep)
                    ,new SqlParameter("@rowNo",model.rowNo)
                    ,new SqlParameter("@activityIdEO",model.activityIdEO)
                    ,new SqlParameter("@IO",model.IO)
                    ,new SqlParameter("@GL",model.GL)
                    ,new SqlParameter("@select_list_choice_id_ChReg",model.select_list_choice_id_ChReg)
                    ,new SqlParameter("@productBrandId",model.productBrandId)
                    ,new SqlParameter("@ByUserId",model.createdByUserId)
                    ,new SqlParameter("@EO",model.EO)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> usp_insertTB_Act_ActivityForm_DetailOtherList");
            }

            return result;
        }
        public static Activity_TBMMKT_Model getMasterChooseSystemCRFormIT314(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            activity_TBMMKT_Model.list_chooseRequest = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "ChooseRequest").OrderBy(x => x.orderNum).ToList();
            activity_TBMMKT_Model.list_0 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "ProcuretoPay").OrderBy(x => x.orderNum).ToList();
            activity_TBMMKT_Model.list_1 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "ERPSystem").OrderBy(x => x.orderNum).ToList();
            activity_TBMMKT_Model.list_2 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "OrdertoCash").OrderBy(x => x.orderNum).ToList();
            activity_TBMMKT_Model.list_3 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "ForecasttoDelivery").OrderBy(x => x.orderNum).ToList();
            activity_TBMMKT_Model.list_4 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "FinanceAndAccounting").OrderBy(x => x.orderNum).ToList();
            activity_TBMMKT_Model.list_5 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "HumanCapital").OrderBy(x => x.orderNum).ToList();
            activity_TBMMKT_Model.list_6 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "Other").OrderBy(x => x.orderNum).ToList();
            return activity_TBMMKT_Model;
        }

        public static Activity_TBMMKT_Model getMasterChooseSystemCRFormIT314_page2(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            activity_TBMMKT_Model.list_7 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "Changetype").OrderBy(x => x.orderNum).ToList();
            activity_TBMMKT_Model.list_8 = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "ChangeAuthorizations").OrderBy(x => x.orderNum).ToList();
            return activity_TBMMKT_Model;
        }


        public static int ProcessInsertProduct(int rtn, Activity_TBMMKT_Model model, string activityId)
        {
            int insertIndex = 1;
            List<ProductCostOfGroupByPrice> insertProductlist = new List<ProductCostOfGroupByPrice>();
            if (model.productcostdetaillist1 != null)
            {
                foreach (var item in model.productcostdetaillist1)
                {
                    foreach (var itemIn in item.detailGroup)
                    {
                        ProductCostOfGroupByPrice productcostdetail = new ProductCostOfGroupByPrice();
                        productcostdetail.id = itemIn.id;
                        productcostdetail.productGroupId = item.productGroupId;
                        productcostdetail.activityId = activityId;
                        productcostdetail.productId = itemIn.productId;
                        productcostdetail.wholeSalesPrice = item.wholeSalesPrice;
                        productcostdetail.saleIn = item.saleIn;
                        productcostdetail.saleOut = item.saleNormal;
                        productcostdetail.disCount1 = item.disCount1;
                        productcostdetail.disCount2 = item.disCount2;
                        productcostdetail.disCount3 = item.disCount3;
                        productcostdetail.normalCost = item.normalCost;
                        productcostdetail.normalGp = item.normalGp;
                        productcostdetail.promotionGp = item.promotionGp;
                        productcostdetail.specialDisc = item.specialDisc;
                        productcostdetail.specialDiscBaht = item.specialDiscBaht;
                        productcostdetail.promotionCost = item.promotionCost;
                        productcostdetail.isShowGroup = item.isShowGroup;
                        productcostdetail.rowNo = insertIndex;
                        productcostdetail.rsp = item.rsp;
                        productcostdetail.unitTxt = item.unitTxt;
                        productcostdetail.delFlag = itemIn.delFlag;
                        productcostdetail.createdByUserId = model.activityFormModel.createdByUserId;
                        productcostdetail.createdDate = model.activityFormModel.createdDate == null ? DateTime.Now : model.activityFormModel.createdDate;
                        productcostdetail.updatedByUserId = UtilsAppCode.Session.User.empId;
                        productcostdetail.updatedDate = DateTime.Now;
                        insertProductlist.Add(productcostdetail);
                    }
                    insertIndex++;
                }
            }
            DataTable dt = AppCode.ToDataTable(insertProductlist);
            rtn += deleteActivityOfProductByActivityId(activityId);
            rtn += insertProductCost(dt);

            return rtn;
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

        protected static int ProcessInsertCliamIO(int rtn, Activity_TBMMKT_Model model, string activityId)
        {
            try
            {
                rtn = SqlHelper.ExecuteNonQuery(AppCode.StrCon, CommandType.StoredProcedure, "usp_insertClaimIO"
                    , new SqlParameter[] {new SqlParameter("@actId",model.activityFormTBMMKT.id)
                     ,new SqlParameter("@claim",model.activityFormTBMMKT.actClaim)
                    ,new SqlParameter("@IO",model.activityFormTBMMKT.actIO)
                    ,new SqlParameter("@checkbox",model.activityFormTBMMKT.chkAddIO)
                    ,new SqlParameter("@delFlag",model.activityFormTBMMKT.delFlag)
                    ,new SqlParameter("@createdDate",model.activityFormTBMMKT.createdDate)
                    ,new SqlParameter("@createdByUserId",model.activityFormTBMMKT.createdByUserId)
                    ,new SqlParameter("@updatedDate",model.activityFormTBMMKT.updatedDate)
                    ,new SqlParameter("@updatedByUserId",model.activityFormTBMMKT.updatedByUserId)
                    });
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> insertCliamIO");
            }

            return rtn;
        }
    }

}