using eActForm.BusinessLayer;
using eForms.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eActForm.Controllers
{
    public class ImportBudgetControlController : Controller
    {
        // GET: ImportBudgetControl
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult ImportFlie(ImportBudgetControlModel.BudgetControlModels model)
        {
            var resultAjax = new AjaxResult();
            try
            {
                string resultFilePath = "";
                int CountFile = model.InputFiles.Count();
                for (int i = 0; i < CountFile; i++)
                {
                    string genUniqueName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + UtilsAppCode.Session.User.empId;
                    string extension = Path.GetExtension(model.InputFiles[i].FileName);
                    string resultFileName = genUniqueName + extension;
                    resultFilePath = Server.MapPath(string.Format(ConfigurationManager.AppSettings["rootUploadfiles_BudgetControl"], resultFileName));
                    model.InputFiles[i].SaveAs(resultFilePath);
                }
                DataTable dt = new DataTable();
                dt = ExcelAppCode.ReadExcel(resultFilePath, "BG-L1", "A:F");
                List<ImportBudgetControlModel.BudgetControlModels> modelList = new List<ImportBudgetControlModel.BudgetControlModels>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ImportBudgetControlModel.BudgetControlModels modelBudget = new ImportBudgetControlModel.BudgetControlModels();
                    modelBudget.brandId = dt.Rows[i]["BG2021"].ToString();
                    modelBudget.amountTT = decimal.Parse(dt.Rows[i]["TT"].ToString());
                    modelBudget.amountCVM = decimal.Parse(dt.Rows[i]["CVM"].ToString());
                    modelBudget.amountMT = decimal.Parse(dt.Rows[i]["MT"].ToString());
                    modelBudget.amountONT = decimal.Parse(dt.Rows[i]["ONT"].ToString());
                    modelBudget.amountSSC = decimal.Parse(dt.Rows[i]["SSC"].ToString());
                    modelList.Add(modelBudget);

                }
                TempData["importFlowModel"] = model;
                resultAjax.Success = true;

            }
            catch (Exception ex)
            {
                resultAjax.Message = ex.Message;

            }
            return Json(resultAjax, "text/plain");
        }
    }
}