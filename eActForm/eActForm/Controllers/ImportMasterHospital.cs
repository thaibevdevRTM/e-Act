using eActForm.BusinessLayer;
using eActForm.Models;
using eForms.Presenter.MasterData;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class ImportMasterHospital : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(ImportExcel importExcel)
        {
            if (ModelState.IsValid)
            {
                string folderKeepFile = "ImportMasterHospital";
                string UploadDirectory = Server.MapPath("~") + "\\Uploadfiles\\" + folderKeepFile + "\\";
                string path = UploadDirectory + importExcel.file.FileName;
                importExcel.file.SaveAs(path);

                var dt = new DataTable();
                dt = ExcelAppCode.ReadExcel(path, "dataimport", "A:L");
                var countDataHaveRows = dt.Rows.Count;

                if (countDataHaveRows > 0)
                {
                    //=====validate=======
                    bool validateValue = true;
                    string txtError = "";

                    for (int i = 0; i < countDataHaveRows; i++)
                    {
                        int rowInExcelAlert = (i + 2);

                        if (dt.Rows[i]["APCode"].ToString() == "")
                        {
                            validateValue = false;
                            txtError += "กรุณาระบุ APCode บรรทัดที่ " + rowInExcelAlert + "<br />";
                        }
                        if (dt.Rows[i]["Name1"].ToString() == "")
                        {
                            validateValue = false;
                            txtError += "กรุณาระบุ Name1 บรรทัดที่ " + rowInExcelAlert + "<br />";
                        }
                        if (dt.Rows[i]["InActive"].ToString() != "0" && dt.Rows[i]["InActive"].ToString() != "1")
                        {
                            validateValue = false;
                            txtError += "รูปแบบข้อมูลที่กรอกไม่ถูกต้อง InActive บรรทัดที่ " + rowInExcelAlert + "<br />";
                        }

                    }

                    if (validateValue == false)
                    {
                        ViewBag.Error = txtError;
                        return View();
                    }
                    else
                    {
                        for (int i = 0; i < countDataHaveRows; i++)
                        {
                            eForms.Models.MasterData.APModel aPModel = new eForms.Models.MasterData.APModel();
                            var varDelFlag = false;
                            if (dt.Rows[i]["InActive"].ToString() == "0") { varDelFlag = false; } else { varDelFlag = true; }
                            aPModel.APCode = dt.Rows[i]["APCode"].ToString();
                            aPModel.Name1 = dt.Rows[i]["Name1"].ToString();
                            aPModel.CoNo = dt.Rows[i]["CoNo"].ToString();
                            aPModel.HouseNo = dt.Rows[i]["HouseNo"].ToString();
                            aPModel.Street = dt.Rows[i]["Street"].ToString();
                            aPModel.Street4 = dt.Rows[i]["Street4"].ToString();
                            aPModel.District = dt.Rows[i]["District"].ToString();
                            aPModel.City = dt.Rows[i]["City"].ToString();
                            aPModel.PostCode = dt.Rows[i]["PostCode"].ToString();
                            aPModel.Tel = dt.Rows[i]["Tel"].ToString();
                            aPModel.FaxNo = dt.Rows[i]["FaxNo"].ToString();
                            aPModel.delFlag = varDelFlag;
                            aPModel.createdByUserId = UtilsAppCode.Session.User.empId;
                            aPModel.createdDate = DateTime.Now;
                            aPModel.updatedByUserId = UtilsAppCode.Session.User.empId;
                            aPModel.updatedDate = DateTime.Now;
                            APPresenter.insert_TB_Act_Master_AP(AppCode.StrCon, aPModel);
                        }
                        //===END==validate=======
                    }
                }

                ViewBag.Result = "Successfully Imported";
            }
            return View();
        }


        [HttpPost]
        public ActionResult ExportMat()
        {
            DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_get_TB_Act_Master_AP_Export");
            DataTable dt = ds.Tables[0];
            string fileNameExport = ("MasterAP" + DateTime.Now.ToString("yyyyMMddHHmmss"));
            ExcelAppCode.ExportExcelEpPlus(dt, "dataimport", fileNameExport, "systemExportAuthor", "systemExportSubject", this.HttpContext, "MasterAP");
            return View("Index");
        }

    }
}