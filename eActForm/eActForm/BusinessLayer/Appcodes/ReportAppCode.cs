using eActForm.BusinessLayer.Appcodes;
using eActForm.BusinessLayer.QueryHandler;
using eActForm.Controllers;
using eActForm.Models;
using eForms.Models.Forms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Hosting;
using WebLibrary;

namespace eActForm.BusinessLayer
{
    public class ReportAppCode
    {

        public static SearchActivityModels getMasterDataForSearch()
        {
            try
            {
                SearchActivityModels models = new SearchActivityModels
                {
                    approveStatusList = ApproveAppCode.getApproveStatus(AppCode.StatusType.app),
                    productGroupList = QueryGetAllProductGroup.getAllProductGroup(),
                    customerslist = QueryGetAllCustomers.getAllCustomers().Where(x => x.cusNameEN != "").ToList(),
                    productTypelist = QuerygetAllProductCate.getAllProductType(),
                    activityGroupList = QueryGetAllActivityGroup.getAllActivityGroup()
                    .GroupBy(item => item.activitySales)
                    .Select(grp => new TB_Act_ActivityGroup_Model { id = grp.First().id, activitySales = grp.First().activitySales }).ToList()
                };

                return models;
            }
            catch (Exception ex)
            {
                throw new Exception("getMasterDataForSearch >>" + ex.Message);
            }
        }
        public static List<CompanyModel> getCompanyByRole(string typeFormId)
        {
            String compId = "";
            try
            {
                List<CompanyModel> companyList = new List<CompanyModel>();

                companyList = QueryGetAllCompany.getCompanyByTypeFormId(typeFormId);

                List<ActUserModel.UserAuthorized> lst = new List<ActUserModel.UserAuthorized>();
                lst = UserAppCode.GetUserAuthorizedsByCompany(UtilsAppCode.Session.User.empCompanyGroup);

                compId = lst.Count > 0 ? lst.FirstOrDefault().companyId : "";
                if (UtilsAppCode.Session.User.isAdminHCBP)
                {

                }
                else if (UtilsAppCode.Session.User.isAdminNUM || UtilsAppCode.Session.User.isAdminPOM || UtilsAppCode.Session.User.isAdminCVM)
                {
                    companyList = companyList.Where(x => x.companyId == compId).OrderBy(x => x.companyNameTH).ToList();
                }
                else
                { //user ปกติดูได้หรือป่าว

                }


                return companyList; //ถ้าเป็น superadmim ถึงจะดึงทั้ง 8 ถ้าไม่ดึงตัวเอง
            }
            catch (Exception ex)
            {
                throw new Exception("getCompanyByRole >>" + ex.Message);
            }
        }

        public static Activity_TBMMKT_Model mainReport(string activityId, string empId)
        {
            Activity_TBMMKT_Model activity_TBMMKT_Model = new Activity_TBMMKT_Model();
            try
            {
                ActivityFormTBMMKT activityFormTBMMKT = new ActivityFormTBMMKT();

                activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getDataForEditActivity(activityId);
                List<Master_type_form_Model> listMasterType = QueryGet_master_type_form.get_master_type_form(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id);
                activity_TBMMKT_Model.activityFormTBMMKT.companyName = QueryGet_master_company.get_master_company(activity_TBMMKT_Model.activityFormTBMMKT.companyId).FirstOrDefault().companyNameTH;
                activity_TBMMKT_Model.activityFormTBMMKT.formName = listMasterType.FirstOrDefault().nameForm;
                activity_TBMMKT_Model.activityFormTBMMKT.formNameEn = listMasterType.FirstOrDefault().nameForm_EN;
                activity_TBMMKT_Model.activityFormTBMMKT.formCompanyId = listMasterType.FirstOrDefault().companyId;
                activity_TBMMKT_Model.activityFormTBMMKT.chkUseEng = (activity_TBMMKT_Model.activityFormTBMMKT.languageDoc == ConfigurationManager.AppSettings["cultureEng"]);
                activity_TBMMKT_Model.master_Type_Form_Detail_Models = QueryGet_master_type_form_detail.get_master_type_form_detail(activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id, "report");

                activity_TBMMKT_Model.approveFlowDetail = ActivityFormTBMMKTCommandHandler.get_flowApproveDetail(activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.SubjectId, activityId, empId);

                if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formCR_IT_FRM_314"])
                {
                    activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getMasterChooseSystemCRFormIT314(activity_TBMMKT_Model);
                    activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getMasterChooseSystemCRFormIT314_page2(activity_TBMMKT_Model);

                    activity_TBMMKT_Model.approveModels = ApproveAppCode.getApproveByActFormId(activityId, empId);
                }
                else if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPurchaseTbm"] ||
                    activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formPaymentVoucherTbmId"])
                {
                    ObjGetDataDetailPaymentAll objGetDataDetailPaymentAll = new ObjGetDataDetailPaymentAll();
                    objGetDataDetailPaymentAll.activityId = activity_TBMMKT_Model.activityFormModel.id;
                    objGetDataDetailPaymentAll.payNo = activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.payNo;
                    activity_TBMMKT_Model.listGetDataDetailPaymentAll = QueryGetSelectMainForm.GetDetailPaymentAll(objGetDataDetailPaymentAll);


                    ObjGetDataLayoutDoc objGetDataLayoutDoc = new ObjGetDataLayoutDoc();
                    objGetDataLayoutDoc.typeKeys = "PVFormBreakSignatureNewPage";
                    objGetDataLayoutDoc.activityId = activityId;
                    activity_TBMMKT_Model.list_ObjGetDataLayoutDoc = QueryGetSelectMainForm.GetQueryDataMasterLayoutDoc(objGetDataLayoutDoc);
                }
                else if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formBgTbmId"])
                {
                    #region BG
                    try
                    {
                        List<BudgetTotal> returnAmountList = new List<BudgetTotal>();


                        var getAmount = QueryGetBudgetActivity.getBudgetAmountList(activity_TBMMKT_Model.activityFormTBMMKT.id);
                        foreach (var item in getAmount.Where(x => x.typeShowBudget == AppCode.typeShowBudget.subMain.ToString()))
                        {
                            BudgetTotal budgetTotalModel = new BudgetTotal();
                            budgetTotalModel.returnAmount = item.returnAmount;
                            budgetTotalModel.EO = item.EO;
                            budgetTotalModel.useAmount = item.useAmount;
                            //budgetTotalModel.totalBudget = item.budgetTotal;
                            budgetTotalModel.amount = item.budgetTotal;
                            budgetTotalModel.amountBalance = item.amountBalance;
                            budgetTotalModel.activityType = item.activityType;
                            var amount = item.budgetTotal > 0 ? item.budgetTotal * 100 : 1;
                            budgetTotalModel.amountBalancePercen = item.useAmount / amount;
                            budgetTotalModel.brandId = activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.productBrandId;
                            budgetTotalModel.brandName = QueryGetAllBrand.GetAllBrand().Where(x => x.digit_EO.Contains(item.EO.Substring(0, 4))).FirstOrDefault().brandName;
                            budgetTotalModel.channelName = !string.IsNullOrEmpty(activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId) ? QueryGetAllChanel.getAllChanel().Where(x => x.id.Equals(activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId)).FirstOrDefault().no_tbmmkt : "";
                            budgetTotalModel.yearBG = item.yearBG;
                            activity_TBMMKT_Model.budgetTotalList.Add(budgetTotalModel);

                        }
                        foreach (var item in getAmount.Where(x => x.typeShowBudget == AppCode.typeShowBudget.main.ToString()))
                        {
                            BudgetTotal budgetMainModel = new BudgetTotal();
                            budgetMainModel.totalBudget = item.budgetTotal;
                            budgetMainModel.totalBudgetChannel = item.budgetTotal;
                            budgetMainModel.amountBalanceTotal = item.amountBalance;
                            budgetMainModel.useAmountTotal = item.useAmount;
                            budgetMainModel.returnAmount = item.returnAmount;
                            budgetMainModel.yearBG = item.yearBG;
                            budgetMainModel.brandId = activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.productBrandId;
                            budgetMainModel.brandName = item.brandName;
                            budgetMainModel.channelName = !string.IsNullOrEmpty(activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId) ? QueryGetAllChanel.getAllChanel().Where(x => x.id.Equals(activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId)).FirstOrDefault().no_tbmmkt : "";
                            activity_TBMMKT_Model.budgetMainTotalList.Add(budgetMainModel);
                        }
                        foreach (var item in getAmount.Where(x => x.typeShowBudget == AppCode.typeShowBudget.actMain.ToString()))
                        {
                            BudgetTotal budgetMainModel = new BudgetTotal();
                            budgetMainModel.totalBudget = item.budgetTotal;
                            budgetMainModel.totalBudgetChannel = item.budgetTotal;
                            budgetMainModel.amountBalanceTotal = item.amountBalance;
                            budgetMainModel.useAmountTotal = item.useAmount;
                            budgetMainModel.returnAmount = item.returnAmount;
                            budgetMainModel.yearBG = item.yearBG;
                            budgetMainModel.EO = item.EO;
                            budgetMainModel.brandId = activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.productBrandId;
                            budgetMainModel.brandName = item.brandName;
                            budgetMainModel.activityType = !string.IsNullOrEmpty(item.activityType) ? BusinessLayer.QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.id == item.activityType).FirstOrDefault().activitySales : "";
                            budgetMainModel.channelName = !string.IsNullOrEmpty(activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId) ? QueryGetAllChanel.getAllChanel().Where(x => x.id.Equals(activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId)).FirstOrDefault().no_tbmmkt : "";
                            activity_TBMMKT_Model.budgetMainActTypelList.Add(budgetMainModel);
                        }
                        foreach (var item in getAmount.Where(x => x.typeShowBudget == AppCode.typeShowBudget.subActMain.ToString()))
                        {
                            BudgetTotal budgetMainModel = new BudgetTotal();
                            budgetMainModel.totalBudget = item.budgetTotal;
                            budgetMainModel.totalBudgetChannel = item.budgetTotal;
                            budgetMainModel.amountBalanceTotal = item.amountBalance;
                            budgetMainModel.useAmountTotal = item.useAmount;
                            budgetMainModel.returnAmount = item.returnAmount;
                            budgetMainModel.yearBG = item.yearBG;
                            budgetMainModel.EO = item.EO;
                            budgetMainModel.brandId = activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.productBrandId;
                            budgetMainModel.brandName = QueryGetAllBrand.GetAllBrand().Where(x => x.digit_EO.Contains(item.EO.Substring(0, 4))).FirstOrDefault().brandName;
                            budgetMainModel.activityType = !string.IsNullOrEmpty(item.activityType) ? BusinessLayer.QueryGetAllActivityGroup.getAllActivityGroup().Where(x => x.id == item.activityType).FirstOrDefault().activitySales : "";
                            budgetMainModel.channelName = !string.IsNullOrEmpty(activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId) ? QueryGetAllChanel.getAllChanel().Where(x => x.id.Equals(activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.channelId)).FirstOrDefault().no_tbmmkt : "";
                            activity_TBMMKT_Model.budgetTotalActTypeList.Add(budgetMainModel);
                        }

                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.WriteError("showDetailBudgetRpt => " + ex.Message);
                    }
                    #endregion
                }
                else if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["masterEmpExpense"])
                {
                    var estimateList = activity_TBMMKT_Model.activityOfEstimateList;
                    activity_TBMMKT_Model.activityOfEstimateList = estimateList.Where(x => x.activityTypeId == "1").ToList();
                    activity_TBMMKT_Model.activityOfEstimateList2 = estimateList.Where(x => x.activityTypeId == "2").ToList();
                    activity_TBMMKT_Model.masterRequestEmp = QueryGet_empDetailById.getEmpDetailById(activity_TBMMKT_Model.activityFormTBMMKT.empId);
                }
                else if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formExpTrvNumId"]
                    || activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formExpMedNumId"]
                    || activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formTrvHome"])
                {
                    List<TB_Act_Image_Model.ImageModel> lists = ImageAppCode.GetImage(activity_TBMMKT_Model.activityFormTBMMKT.id);
                    activity_TBMMKT_Model.tB_Act_ActivityForm_SelectBrandOrChannelModel.txt = lists.Count.ToString();
                    activity_TBMMKT_Model.tB_Act_ActivityForm_SelectBrandOrChannelModel.val = activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id;

                    activity_TBMMKT_Model.approveModels = ApproveAppCode.getApproveByActFormId(activity_TBMMKT_Model.activityFormTBMMKT.id, "");
                    if (activity_TBMMKT_Model.approveModels.approveDetailLists.Count > 0)
                    {
                        activity_TBMMKT_Model.approveModels.approveDetailLists = activity_TBMMKT_Model.approveModels.approveDetailLists.Where(x => x.approveGroupId == AppCode.ApproveGroup.Director).ToList();
                    }

                    if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formExpTrvNumId"]
                        || activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formTrvHome"])
                    {
                        #region expensesTrvDetailRpt
                        ApproveModel.approveModels models = new ApproveModel.approveModels();
                        models = ApproveAppCode.getApproveByActFormId(activity_TBMMKT_Model.activityFormTBMMKT.id, "");
                        //List<approveDetailModel> approveDetailLists = new List<approveDetailModel>();
                        decimal? calSum1 = 0;
                        decimal? calSum2 = 0;
                        if (models.approveDetailLists.Count > 0)
                        {
                            models.approveDetailLists = models.approveDetailLists.Where(x => x.approveGroupId == AppCode.ApproveGroup.Recorder).Where(x => x.statusId == Convert.ToString((int)AppCode.ApproveStatus.อนุมัติ)).ToList();

                            if (models.approveDetailLists.Count > 0)
                            {//อนุมัติแล้ว
                                CostDetailOfGroupPriceTBMMKT model2 = new CostDetailOfGroupPriceTBMMKT();

                                model2.costDetailLists = QueryGetActivityEstimateByActivityId.getWithListChoice(activity_TBMMKT_Model.activityFormModel.id, activity_TBMMKT_Model.activityFormModel.master_type_form_id, AppCode.GLType.GLSaleSupport);

                                for (int i = 0; i < model2.costDetailLists.Count; i++)
                                {
                                    if (model2.costDetailLists[i].listChoiceId == AppCode.Expenses.Allowance)
                                    {
                                        calSum1 += model2.costDetailLists[i].total;
                                    }
                                    else
                                    {
                                        calSum2 += model2.costDetailLists[i].total;
                                    }
                                }
                            }
                        }

                        CostDetailOfGroupPriceTBMMKT model = new CostDetailOfGroupPriceTBMMKT();
                        model.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT() { rowNo = 1, total = calSum1 });
                        model.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT() { rowNo = 2, total = calSum2 });
                        activity_TBMMKT_Model.expensesDetailModel = model;

                        CostDetailOfGroupPriceTBMMKT modelResult = new CostDetailOfGroupPriceTBMMKT();
                        CostDetailOfGroupPriceTBMMKT model22 = new CostDetailOfGroupPriceTBMMKT();
                        CostDetailOfGroupPriceTBMMKT modelSub = new CostDetailOfGroupPriceTBMMKT();

                        model22.costDetailLists = QueryGetActivityEstimateByActivityId.getWithListChoice(activity_TBMMKT_Model.activityFormModel.id, activity_TBMMKT_Model.activityFormModel.master_type_form_id, AppCode.GLType.GLSaleSupport);
                        modelSub.costDetailLists = QueryGetActivityEstimateByActivityId.getEstimateSub(activity_TBMMKT_Model.activityFormModel.id, AppCode.Expenses.hotelExpense);
                        modelSub.costDetailLists = modelSub.costDetailLists.Where(x => x.unitPrice > 0).ToList();
                        string multiPrice = "";
                        decimal? total = 0;
                        for (int i = 0; i < model22.costDetailLists.Count; i++)
                        {
                            if (model22.costDetailLists[i].total != 0)
                            {
                                total = model22.costDetailLists[i].total;
                                if (model22.costDetailLists[i].listChoiceId == AppCode.Expenses.hotelExpense && model22.costDetailLists[i].unit != 0 && model22.costDetailLists[i].unitPrice == 0)
                                {
                                    foreach (var item in modelSub.costDetailLists)
                                    {
                                        multiPrice = multiPrice + item.unitPriceDisplayReport + "|";
                                    }
                                    multiPrice = multiPrice.Substring(0, (multiPrice.Length - 1));
                                    //เป็นค่าที่พักหลายราคา
                                }

                                modelResult.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT()
                                {
                                    listChoiceId = model22.costDetailLists[i].listChoiceId,
                                    listChoiceName = model22.costDetailLists[i].listChoiceName,
                                    productDetail = model22.costDetailLists[i].productDetail,
                                    unit = model22.costDetailLists[i].unit,
                                    unitPrice = model22.costDetailLists[i].unitPrice + model22.costDetailLists[i].vat,
                                    unitPriceDisplay = multiPrice,
                                    total = model22.costDetailLists[i].total,
                                    displayType = model22.costDetailLists[i].displayType,
                                    subDisplayType = model22.costDetailLists[i].subDisplayType,
                                    updatedByUserId = model22.costDetailLists[i].updatedByUserId,
                                    createdByUserId = model22.costDetailLists[i].createdByUserId,
                                    statusEdit = model22.costDetailLists[i].createdByUserId == "" ? "" :
                                      (model22.costDetailLists[i].createdByUserId != model22.costDetailLists[i].updatedByUserId
                                      && model22.costDetailLists[i].total != total ? "*" : ""),
                                });

                            }
                        }

                        activity_TBMMKT_Model.expensesDetailModel = modelResult;

                        #endregion


                    }
                    else if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formExpMedNumId"])
                    {
                        activity_TBMMKT_Model.expensesDetailModel.costDetailLists = QueryGetActivityEstimateByActivityId.getByActivityId(activity_TBMMKT_Model.activityFormModel.id);
                        activity_TBMMKT_Model.expensesDetailModel.costDetailLists[0].hospName = QueryGetAllHospital.getAllHospital().Where(x => x.id.Contains(activity_TBMMKT_Model.expensesDetailModel.costDetailLists[0].hospId)).FirstOrDefault().hospNameTH;
                        activity_TBMMKT_Model = ReportAppCode.recordByHcRptAppCode(activity_TBMMKT_Model);
                    }

                }

                //===END==layout doc===========

                //===========Set Language By Document Dev date 20200310 Peerapop=====================
                //ไม่ต้องไปกังวลว่าภาษาหลักของWebที่Userใช้งานอยู่จะมีปัญหาเพราะ _ViewStart จะเปลี่ยนภาษาปัจจุบันที่Userใช้เว็บปรับCultureกลับให้เอง
                DocumentsAppCode.setCulture(activity_TBMMKT_Model.activityFormModel.languageDoc);


                //====END=======Set Language By Document Dev date 20200310 Peerapop==================
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("reportAppCode >> mainReport >>" + activityId + "___" + ex.Message);
            }
            return activity_TBMMKT_Model;
        }


        public static Activity_TBMMKT_Model reportPettyCashNumAppCode(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            ActivityFormTBMMKT activityFormTBMMKT = new ActivityFormTBMMKT();
            try
            {


                if (activity_TBMMKT_Model.activityFormTBMMKT.id != null)
                {
                    activity_TBMMKT_Model = ActivityFormTBMMKTCommandHandler.getDataForEditActivity(activity_TBMMKT_Model.activityFormTBMMKT.id);
                    activity_TBMMKT_Model.activityFormTBMMKT.companyName = QueryGet_master_company.get_master_company(activity_TBMMKT_Model.activityFormTBMMKT.companyId).FirstOrDefault().companyNameTH;
                    activity_TBMMKT_Model.activityFormTBMMKT.chkUseEng = (activity_TBMMKT_Model.activityFormTBMMKT.languageDoc == ConfigurationManager.AppSettings["cultureEng"]);
                    //===ดึงผู้อนุมัติทั้งหมด=เพือเอาไปใช้แสดงในรายงาน===
                    activity_TBMMKT_Model.approveFlowDetail = ActivityFormTBMMKTCommandHandler.get_flowApproveDetail(activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.SubjectId, activity_TBMMKT_Model.activityFormTBMMKT.id, activity_TBMMKT_Model.activityFormTBMMKT.empId);
                    //=END==ดึงผู้อนุมัติทั้งหมด=เพือเอาไปใช้แสดงในรายงาน===
                }

                activity_TBMMKT_Model.approveModels = ApproveAppCode.getApproveByActFormId(activity_TBMMKT_Model.activityFormTBMMKT.id, activity_TBMMKT_Model.activityFormTBMMKT.empId);
                activity_TBMMKT_Model.activityFormTBMMKT.formName = QueryGet_master_type_form.get_master_type_form(ConfigurationManager.AppSettings["formReportPettyCashNum"]).FirstOrDefault().nameForm;
                activity_TBMMKT_Model.activityFormTBMMKT.formNameEn = QueryGet_master_type_form.get_master_type_form(ConfigurationManager.AppSettings["formReportPettyCashNum"]).FirstOrDefault().nameForm_EN;


                CostDetailOfGroupPriceTBMMKT modelResult = new CostDetailOfGroupPriceTBMMKT
                {
                    costDetailLists = new List<CostThemeDetailOfGroupByPriceTBMMKT>()
                };

                CostDetailOfGroupPriceTBMMKT model2 = new CostDetailOfGroupPriceTBMMKT
                {
                    costDetailLists = new List<CostThemeDetailOfGroupByPriceTBMMKT>()
                };

                #region "ดึงข้อมูล GL "
                //ฟอร์มที่ใช้เป็นของ saleSupport
                List<GetDataGL> lstGL = new List<GetDataGL>();
                lstGL = QueryGetGL.getGLMasterByDivisionId(AppCode.Division.salesSupport);
                #endregion

                #region form HC

                if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formExpTrvNumId"]
                    || activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formTrvHome"])
                {
                    decimal? vat = 0, vatsum = 0;
                    #region "ค่าเดินทางของ NUM"

                    model2.costDetailLists = QueryGetActivityEstimateByActivityId.getWithListChoice(activity_TBMMKT_Model.activityFormModel.id, activity_TBMMKT_Model.activityFormModel.master_type_form_id, AppCode.GLType.GLSaleSupport);
                    for (int i = 0; i < model2.costDetailLists.Count; i++)
                    {
                        if (model2.costDetailLists[i].total != 0 && model2.costDetailLists[i].listChoiceId != AppCode.Expenses.Allowance)
                        {
                            //vat แสดงรวมค่าใช้จ่ายอื่นๆแสดงแยก

                            if (model2.costDetailLists[i].listChoiceId == AppCode.Expenses.hotelExpense && model2.costDetailLists[i].unitPrice == 0)
                            {
                                vat = model2.costDetailLists[i].vat;
                                //  vatsum += model2.costDetailLists[i].vat;

                            }
                            else
                            {
                                vat = (model2.costDetailLists[i].vat * model2.costDetailLists[i].unit);
                                //  vatsum += (model2.costDetailLists[i].vat * model2.costDetailLists[i].unit);
                            }
                            vatsum += vat;

                            modelResult.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT()
                            {
                                listChoiceId = model2.costDetailLists[i].listChoiceId,
                                listChoiceName = model2.costDetailLists[i].listChoiceName,
                                productDetail = model2.costDetailLists[i].listChoiceName + " " +
                               (model2.costDetailLists[i].displayType == AppCode.CodeHtml.LabelHtml
                               ? model2.costDetailLists[i].unit + "วัน (สิทธิเบิก " + model2.costDetailLists[i].productDetail + " บาท/วัน)"
                               : model2.costDetailLists[i].productDetail),
                                total = model2.costDetailLists[i].total - (vat),
                                //glCode = lstGL.Where(x => x.groupGL.Contains(model2.costDetailLists[i].listChoiceName) ).FirstOrDefault()?.GL,
                                glCode = QueryGetGL.getGL(lstGL, model2.costDetailLists[i].glCodeId, activity_TBMMKT_Model.activityFormModel.empId) //lstGL.Where(x => x.id == model2.costDetailLists[i].glCodeId).FirstOrDefault()?.GL,
                            });
                        }
                    }
                    if (vatsum > 0)
                    {
                        modelResult.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT()
                        {
                            listChoiceId = "",
                            listChoiceName = "vat",
                            productDetail = lstGL.Where(x => x.GL == AppCode.GLVat.gl).FirstOrDefault()?.groupGL,
                            total = vatsum,
                            glCode = lstGL.Where(x => x.GL == AppCode.GLVat.gl).FirstOrDefault()?.GL,
                        });
                    }

                    if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formExpTrvNumId"])
                    {
                        activity_TBMMKT_Model.totalCostThisActivity -= model2.costDetailLists.Where(X => X.listChoiceId == AppCode.Expenses.Allowance).FirstOrDefault().total;
                    }
                    #endregion
                }
                else if (activity_TBMMKT_Model.activityFormTBMMKT.master_type_form_id == ConfigurationManager.AppSettings["formExpMedNumId"])
                {
                    modelResult.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT()
                    {
                        listChoiceId = "",
                        listChoiceName = "",
                        productDetail = "ค่ารักษาพยาบาล",
                        total = activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.amountReceived,
                        displayType = "",
                        glCode = QueryGetGL.getGL(lstGL, AppCode.SSGLId.medical, activity_TBMMKT_Model.activityFormModel.empId)//lstGL.Where(x => AppCode.SSGLId.medical.Contains(x.id)).FirstOrDefault()?.GL,//glCode = lstGL.Where(x => x.id == ).FirstOrDefault()?.GL,
                    });
                    activity_TBMMKT_Model.totalCostThisActivity = activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.amountReceived;

                }

                int rowAdd = 8 - modelResult.costDetailLists.Count;
                for (int i = 0; i < rowAdd; i++)
                {
                    modelResult.costDetailLists.Add(new CostThemeDetailOfGroupByPriceTBMMKT()
                    {
                        listChoiceId = "",
                        listChoiceName = "",
                        productDetail = "",
                        total = 0,
                        displayType = "",
                        glCode = "",
                    });
                }

                #endregion

                modelResult.costDetailLists = modelResult.costDetailLists.ToList();
                activity_TBMMKT_Model.expensesDetailModel = modelResult;



                //===========Set Language By Document Dev date 20200310 Peerapop=====================
                //ไม่ต้องไปกังวลว่าภาษาหลักของWebที่Userใช้งานอยู่จะมีปัญหาเพราะ _ViewStart จะเปลี่ยนภาษาปัจจุบันที่Userใช้เว็บปรับCultureกลับให้เอง
                DocumentsAppCode.setCulture(activity_TBMMKT_Model.activityFormModel.languageDoc);
                //====END=======Set Language By Document Dev date 20200310 Peerapop==================

                //return View(activity_Model); // test

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(" ReportPettyCashNumAppCode >>" + ex.Message);
            }
            return activity_TBMMKT_Model;
        }

        public static Activity_TBMMKT_Model recordByHcRptAppCode(Activity_TBMMKT_Model activity_TBMMKT_Model)
        {
            try
            {
                ApproveModel.approveModels modelApprove = new ApproveModel.approveModels();
                modelApprove = ApproveAppCode.getApproveByActFormId(activity_TBMMKT_Model.activityFormTBMMKT.id, "");
                bool delFlag = false;//ยังไม่อนุมัตื
                modelApprove.approveDetailLists = modelApprove.approveDetailLists.Where(x => x.approveGroupId == AppCode.ApproveGroup.Recorder).Where(x => x.statusId == "3").ToList();
                if (modelApprove.approveDetailLists.Count > 0)
                {
                    delFlag = true;//อนุมัติแล้ว
                }
                //models = activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther;
                activity_TBMMKT_Model.tB_Act_ActivityForm_DetailOther.delFlag = delFlag;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("recordByHcRptAppCode >>" + activity_TBMMKT_Model.activityFormTBMMKT.id + "___" + ex.Message);
            }
            return activity_TBMMKT_Model;
        }
        public static Activity_Model previewApprove(string actId, string empId)
        {
            Activity_Model activityModel = new Activity_Model();
            try
            {
                activityModel.activityFormModel = QueryGetActivityById.getActivityById(actId).FirstOrDefault();
                activityModel.detailOtherModel = QueryGetActivityFormDetailOtherByActivityId.getByActivityId(actId).FirstOrDefault();
                activityModel.productcostdetaillist1 = QueryGetCostDetailById.getcostDetailById(actId);
                activityModel.activitydetaillist = QueryGetActivityDetailById.getActivityDetailById(actId);
                activityModel.productImageList = ImageAppCode.GetImage(actId).Where(x => x.extension != ".pdf").ToList();
                activityModel.activityFormModel.typeForm = BaseAppCodes.getactivityTypeByCompanyId(activityModel.activityFormModel.companyId);
                activityModel.approveModels = ApproveFlowAppCode.getFlowId(ConfigurationManager.AppSettings["subjectActivityFormId"], actId);

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("reportAppCode >> previewApprove >>" + actId + "___" + ex.Message);
            }
            return activityModel;
        }

    }
}