using eActForm.BusinessLayer;
using eActForm.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class ImportMaterial_PRODUCT_POS_PREMIUMController : Controller
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
                string folderKeepFile = "ImportMatProductPosPremium";
                string UploadDirectory = Server.MapPath("~") + "\\Uploadfiles\\" + folderKeepFile + "\\";
                string path = UploadDirectory + importExcel.file.FileName;
                importExcel.file.SaveAs(path);

                var dt = new DataTable();
                dt = ExcelAppCode.ReadExcel(path, "dataimport", "A:H");
                var countDataHaveRows = dt.Rows.Count;

                if (countDataHaveRows > 0)
                {
                    //====add column to use insert db=========
                    dt.Columns.Add("tB_Act_master_list_choice_id", typeof(String));
                    dt.Columns.Add("tB_Act_master_list_choice_id_InOutStock", typeof(String));
                    //==END==add column to use insert db======

                    //=====validate=======
                    bool validateValue = true;
                    string txtError = "";
                    for (int i = 0; i < countDataHaveRows; i++)
                    {
                        int rowInExcelAlert = (i + 2);
                        if (dt.Rows[i]["Material"].ToString() == "")
                        {
                            validateValue = false;
                            txtError += "กรุณาระบุ Material บรรทัดที่ " + rowInExcelAlert + "<br />";
                        }
                        if (dt.Rows[i]["Sub Group"].ToString() == "")
                        {
                            validateValue = false;
                            txtError += "กรุณาระบุ Sub Group บรรทัดที่ " + rowInExcelAlert + "<br />";
                        }
                        if (dt.Rows[i]["Status Mat"].ToString() == "")
                        {
                            validateValue = false;
                            txtError += "กรุณาระบุ Status Mat บรรทัดที่ " + rowInExcelAlert + "<br />";
                        }

                        string idFormPOS_Premium = "24BA9F57-586A-4A8E-B54C-00C23C41BFC5";
                        var keyValue_subGroupIdByRow = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(idFormPOS_Premium, "product_pos_premium").Where(x => x.name == dt.Rows[i]["Sub Group"].ToString().ToUpper()).FirstOrDefault();
                        if (keyValue_subGroupIdByRow != null)
                        {
                            dt.Rows[i]["tB_Act_master_list_choice_id"] = keyValue_subGroupIdByRow.id;
                        }
                        else
                        {
                            validateValue = false;
                            txtError += "รูปแบบข้อมูลที่กรอกไม่ถูกต้อง Sub Group บรรทัดที่ " + rowInExcelAlert + "<br />";
                        }
                        var keyValue_statusMatIdByRow = QueryGet_TB_Act_master_list_choice.get_TB_Act_master_list_choice(idFormPOS_Premium, "in_or_out_stock").Where(x => x.name == dt.Rows[i]["Status Mat"].ToString().ToUpper()).FirstOrDefault();
                        if (keyValue_statusMatIdByRow != null)
                        {
                            dt.Rows[i]["tB_Act_master_list_choice_id_InOutStock"] = keyValue_statusMatIdByRow.id;
                        }
                        else
                        {
                            validateValue = false;
                            txtError += "รูปแบบข้อมูลที่กรอกไม่ถูกต้อง Status Mat บรรทัดที่ " + rowInExcelAlert + "<br />";
                        }

                    }
                    if (validateValue == false)
                    {
                        ViewBag.Error = txtError;
                        return View();
                    }
                    else
                    {

                    }
                    //===END==validate=======

                }

                ViewBag.Result = "Successfully Imported";
            }
            return View();
        }
    }
}