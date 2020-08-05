using eActForm.BusinessLayer;
using eActForm.Models;
using eForms.Presenter.MasterData;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using WebLibrary;
using System.Reflection;
using eForms.Models.MasterData;

namespace eActForm.Controllers
{
    [LoginExpire]
    public class ImportMasterHospitalController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(ImportExcel importExcel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    string folderKeepFile = "ImportMasterHospital";
                    string UploadDirectory = Server.MapPath("~") + "\\Uploadfiles\\" + folderKeepFile;


                    string extension = Path.GetExtension(importExcel.file.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(importExcel.file.FileName);
                    fileName = "impHosp_" + DateTime.Now.ToString("yyMMddHHmmss") + "_" + UtilsAppCode.Session.User.empId + extension;

                    string path = UploadDirectory + "\\" + fileName;

                    if (!System.IO.Directory.Exists(UploadDirectory))
                    {
                        System.IO.Directory.CreateDirectory(UploadDirectory);
                    }
                    importExcel.file.SaveAs(path);

                    var dt = new DataTable();
                    dt = ExcelAppCode.ReadExcel(path, "dataimport", "A:E");
                    var countDataHaveRows = dt.Rows.Count;

                    string[] region = { "1", "2", "3", "4", "5", "6", "7", "8", };


                    List<HospitalModel> typeList = new List<HospitalModel>();
                    typeList = QueryGetAllHospital.getHospitalType().ToList();

                    string hospitalId = "", hospitalName = "", hospTypeId = "", provinceId = "", regionId = "", delFlag = "";

                    // DataTable dtProvinces = getProvinces().Tables[0];
                    List<ProvinceModel> provinceList = new List<ProvinceModel>();
                    provinceList = ProvincePresenter.getProvince(AppCode.StrConAuthen);


                    List<HospitalModel> hospList = new List<HospitalModel>();
                    //valid data          

                    if (countDataHaveRows > 0)
                    {
                        //=====validate=======
                        bool validateValue = true;
                        string txtError = "";

                        for (int i = 0; i < countDataHaveRows; i++)
                        {
                            int rowInExcelAlert = (i + 2);

                            //a ต้องไม่เป็นค่าว่า เป็นคีย์ที่ใช้ในการ update หรือ insert(ขณะเช็ค ก็ไป get id มาเลย ถ้าไม่มี id เป็น ค่าว่าง )
                            if (dt.Rows[i][0].ToString() == "")
                            {
                                validateValue = false;
                                txtError += "บรรทัดที่ " + rowInExcelAlert + " กรุณาระบุ ชื่อสถานพยาบาล<br />";
                            }
                            else
                            {
                                hospitalId = getHospitalId(dt.Rows[i][0].ToString());
                                hospitalName = dt.Rows[i][0].ToString();
                                //txtError += hospitalId + "<br />";
                            }

                            DataRow[] dr;
                            //b ต้อง เป็น จังหวัดใน DB(ต้องหาวิธีแสดงชื่อจังหวัด)(ขณะเช็ค ก็ไป get id มาเลย)
                            if (dt.Rows[i][1].ToString() == "")
                            {
                                validateValue = false;
                                txtError += "บรรทัดที่ " + rowInExcelAlert + " กรุณาระบุ จังหวัด<br />";
                            }
                            else
                            {
                                var listProv = provinceList.Where(x => x.nameTH.Equals(dt.Rows[i][1].ToString().Trim())).ToList();
                                //dr = dtProvinces.Select("nameTH = '" + dt.Rows[i][1].ToString().Trim() + "'");
                                //if (dr.Count() > 0)
                                if (listProv.Count > 0)
                                {
                                    // provinceId = dr[0]["id"].ToString();
                                    provinceId = listProv[0].id;
                                }
                                else
                                {
                                    validateValue = false;
                                    txtError += "บรรทัดที่ " + rowInExcelAlert + " ชื่อจังหวัดไม่ตรงกับข้อมูล Master<br />";
                                }
                                //txtError += hospitalId + "<br />";
                            }

                            //c ต้องเป็น 1 - 8 เท่านั้น
                            if (dt.Rows[i][2].ToString() == "")
                            {
                                validateValue = false;
                                txtError += "บรรทัดที่ " + rowInExcelAlert + " กรุณาระบุ ภาคการขาย<br />";
                            }
                            else
                            {
                                if (region.Contains(dt.Rows[i][2].ToString().Trim()))
                                {
                                    regionId = dt.Rows[i][2].ToString().Trim();
                                }
                                else
                                {
                                    validateValue = false;
                                    txtError += "บรรทัดที่ " + rowInExcelAlert + " ภาคการขายระบุได้ 1-8 เท่านั้น<br />";
                                }
                            }

                            //D ต้องเป็น 100  75  50  เท่านั้น(ขณะเช็ค ก็ไป get id มาเลย)
                            if (dt.Rows[i][3].ToString() == "")
                            {
                                validateValue = false;
                                txtError += "บรรทัดที่ " + rowInExcelAlert + " กรุณาระบุ ประเภทสถานพยาบาล(%)<br />";
                            }
                            else
                            {
                                string per = dt.Rows[i][3].ToString().Trim();
                                var list = typeList.Where(x => Convert.ToString(x.percentage).Equals(per)).ToList();
                                if (list.Count > 0)
                                {
                                    hospTypeId = list[0].id.ToString();
                                }
                                else
                                {
                                    validateValue = false;
                                    txtError += "บรรทัดที่ " + rowInExcelAlert + " ประเภทสถานพยาบาลระบุได้ 100 ,75 ,50 เท่านั้น<br />";
                                }

                            }

                            //E ต้องเป็น 0 - 1 เท่านั้น
                            if (dt.Rows[i][4].ToString() == "0")//ปิด
                            {
                                delFlag = "1";
                            }
                            else if (dt.Rows[i][4].ToString() == "1")//เปิด
                            {
                                delFlag = "0";
                            }
                            else
                            {
                                validateValue = false;
                                txtError += "บรรทัดที่ " + rowInExcelAlert + " สถานะระบุได้ 0-1 เท่านั้น<br />";

                            }

                            hospList.Add(new HospitalModel()
                            {
                                id = hospitalId
                            ,
                                hospTypeId = hospTypeId
                            ,
                                hospNameTH = hospitalName
                            ,
                                hospNameEN = ""
                            ,
                                provinceId = provinceId
                            ,
                                region = regionId
                            ,
                                delFlag = delFlag
                            });
                        }

                        if (validateValue == false)
                        {
                            //ถ้ามี error ลบไฟล์ทิ้ง
                            System.IO.File.Delete(path);
                            ViewBag.Error = txtError;
                            return View();
                        }
                        else
                        {
                            //ไม่มี error insert data
                            for (int i = 0; i < hospList.Count; i++)
                            {
                                int result = QueryProcessHospital.updateHospital(hospList[i]);

                            }

                        }
                    }

                    ViewBag.Result = "Successfully Imported";
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("ImportMasterHospitalController.Index => " + ex.Message);
                ViewBag.Error = "ไม่สามารถอ่านไฟล์ได้กรุณาตรวจสอบชื่อชีต หรือ ติดต่อ Admin";
            }
            return View();
        }

        [HttpPost]
        public ActionResult exportHospital()
        {

            try
            {

                // DataSet ds = SqlHelper.ExecuteDataset(AppCode.StrCon, CommandType.StoredProcedure, "usp_exportAllHospital");
                DataTable dt = new DataTable();

                List<HospitalModel> getList = new List<HospitalModel>();
                getList = QueryGetAllHospital.exportHospital().ToList();

                dt = eForms.Presenter.BasePresenter.ToDataTable(getList);

                //DataView dv = ds.Tables[0].DefaultView;
                DataView dv = dt.DefaultView;
                dv.Sort = "percentage, hospNameTH asc";
                dt = dv.ToTable(false, "hospNameTH", "provName", "region", "percentage", "delFlag");

                dt.Columns["hospNameTH"].ColumnName = "ชื่อสถานพยาบาล";
                dt.Columns["provName"].ColumnName = "จังหวัด";
                dt.Columns["region"].ColumnName = "ภาคการขายที่";
                dt.Columns["percentage"].ColumnName = "ประเภทสถานพยาบาล(%)";
                dt.Columns["delFlag"].ColumnName = "สถานะ";
                //delFlag set เป็น status ที่ stored แล้ว
                //  dt = sortedDT.Copy();
                string fileNameExport = ("MasterHospital" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                ExcelAppCode.ExportExcelEpPlus(dt, "dataimport", fileNameExport, "systemExportAuthor", "systemExportSubject", this.HttpContext, "MasterHospital");
                return View("Index");
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("exportHospital => " + ex.Message);
                return View("Index");
            }
        }

        public ActionResult exportProvince()
        {

            try
            {
                //DataSet ds = getProvinces();

                List<ProvinceModel> provinceList = new List<ProvinceModel>();
                provinceList = ProvincePresenter.getProvince(AppCode.StrConAuthen);

                DataTable dt = new DataTable();
                dt = eForms.Presenter.BasePresenter.ToDataTable(provinceList);

                DataView dv = dt.DefaultView;
                dv.Sort = "nameTH asc";
                dt = dv.ToTable(false, "nameTH");
                dt.Columns["nameTH"].ColumnName = "ชื่อจังหวัด";
                string fileNameExport = ("MasterProvines" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                ExcelAppCode.ExportExcelEpPlus(dt, "Provines", fileNameExport, "systemExportAuthor", "systemExportSubject", this.HttpContext, "MasterProvines");
                return View("Index");
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("exportProvince => " + ex.Message);
                return View("Index");
            }
        }
        public string getHospitalId(string hospitalName)
        {
            //แก้stored ดึงให้หมดทุก delflag
            string hospitalId = "";
            List<HospitalModel> getList = new List<HospitalModel>();
            try
            {
                getList = QueryGetAllHospital.exportHospital().Where(x => x.hospNameTH.Replace(" ", "").Equals(hospitalName.Replace(" ", ""))).ToList();

                if (getList.Count > 0)
                {
                    hospitalId = getList[0].id.ToString();
                }

            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getAllHospital => " + ex.Message);
            }
            return hospitalId;
        }
     
    }
  }