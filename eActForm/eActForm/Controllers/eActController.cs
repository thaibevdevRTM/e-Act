using eActForm.BusinessLayer;
using eActForm.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebLibrary;

namespace eActForm.Controllers
{
    public partial class eActController : Controller
    {

        // GET: eAct
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getChanel(string Id)
        {
            var result = new AjaxResult();
            try
            {
                var getcustomer = QueryGetAllCustomers.getAllCustomers().Where(x => x.id == Id).FirstOrDefault();
                var getchanel = QueryGetAllChanel.getAllChanel().Where(x => x.id == getcustomer.chanel_Id).FirstOrDefault();
                result.Data = new
                {
                    Chanel_group = getchanel.chanelGroup,
                };
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, "text/plain");
        }


        public JsonResult getProductGroup(string cateId)
        {
            var result = new AjaxResult();
            try
            {
                var getProductGroup = QueryGetAllProductGroup.getAllProductGroup().Where(x => x.cateId.Trim() == cateId.Trim()).ToList();
                var resultData = new
                {
                    //    productGroup = getProductGroup.GroupBy(item => item.productGroup)
                    //.Select(group => new TB_Act_Product_Cate_Model.Product_Cate_Model
                    //{
                    //    id = group.First().id,
                    //    productGroup = group.First().productGroup,
                    //}).ToList(),
                    productGroup = getProductGroup.ToList(),
                };
                result.Data = resultData;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getProductSmell(string productGroupId)
        {
            var result = new AjaxResult();
            try
            {
                var lists = QueryGetAllProduct.getProductSmellByGroupId(productGroupId);
                result.Data = lists;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getProductBrand(string p_groupId)
        {
            var result = new AjaxResult();
            try
            {
                var getProductBrand = QueryGetAllBrand.GetAllBrand().Where(x => x.productGroupId.Trim() == p_groupId.Trim()).ToList();
                var resultData = new
                {
                    getProductname = getProductBrand.Select(x => new
                    {
                        Value = x.id,
                        Text = x.brandName
                    }).ToList(),
                };
                result.Data = resultData;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getddlSize(string Id)
        {
            var result = new AjaxResult();
            try
            {
                List<TB_Act_Product_Model.Product_Model> productModel = new List<TB_Act_Product_Model.Product_Model>();

                productModel = QueryGetAllProduct.getAllProduct().Where(x => x.brandId == Id).ToList();
                var resultData = new
                {
                    getProductSize = productModel.Select(x => new
                    {
                        Value = x.id,
                        Text = x.size
                    }).ToList(),
                };
                result.Data = resultData;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult getProductDetail(string brandId, string smellId)
        {
            var result = new AjaxResult();
            try
            {
                List<TB_Act_Product_Model.Product_Model> getProductDetail = new List<TB_Act_Product_Model.Product_Model>();
                if (smellId != "")
                {
                    getProductDetail = QueryGetAllProduct.getProductBySmellId(smellId);
                }
                else
                {
                    getProductDetail = QueryGetAllProduct.getProductByBrandId(brandId);
                }
                var resultData = new
                {
                    getProductsize = getProductDetail.GroupBy(item => item.size)
                     .Select(group => new TB_Act_Product_Model.Product_Model
                     {
                         id = group.First().brandId,
                         size = group.First().size,
                     }).OrderByDescending(x => x.size).ToList(),

                    getProductname = getProductDetail.Select(x => new
                    {
                        Value = x.id,
                        Text = x.productName
                    }).ToList(),
                };
                result.Data = resultData;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getddlProduct(string size, string brandId,string smellId)
        {
            var result = new AjaxResult();
            try
            {
                List<TB_Act_Product_Model.Product_Model> productModel = new List<TB_Act_Product_Model.Product_Model>();
                if (size != "")
                {
                    productModel = QueryGetAllProduct.getAllProduct().Where(x => ( x.brandId == brandId || x.smellId == smellId) && x.size == size).ToList();
                }
                else
                {
                    productModel = QueryGetAllProduct.getAllProduct().Where(x => x.brandId == brandId || x.smellId == smellId).ToList();
                }

                var resultData = new
                {
                    getProductname = productModel.Select(x => new
                    {
                        Value = x.id,
                        Text = x.productName
                    }).ToList(),
                };
                result.Data = resultData;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public static List<Attachment> genPdfFile(string GridHtml , string activityId)
        {
            GridHtml = GridHtml.Replace("\n", "");
        

            ContentType xlsxContent = new ContentType("application/pdf");
            MemoryStream msPreview = new MemoryStream();
            byte[] PreviewBytes = new byte[0];
            List<Attachment> files = new List<Attachment>();
            
            msPreview = GetFileReportTomail_Preview(GridHtml);
            PreviewBytes = msPreview.ToArray();

            var rootPath = System.Web.HttpContext.Current.Server.MapPath(string.Format(ConfigurationManager.AppSettings["rooPdftURL"], activityId));
            System.IO.File.WriteAllBytes(rootPath, PreviewBytes);


            if (PreviewBytes.Length != 0)
            {
                Attachment data_RepCashofSale = new Attachment(new MemoryStream(PreviewBytes), xlsxContent);
                data_RepCashofSale.ContentDisposition.FileName = "eActForm.pdf";
                files.Add(data_RepCashofSale);
            }

            return files;
        }

        public static MemoryStream GetFileReportTomail_Preview(string GridHtml)
        {
            MemoryStream ms = new MemoryStream();
            try
            {

                UserControl LoadControl = new UserControl();
                StringWriter sw = new StringWriter();
                HtmlTextWriter myWriter = new HtmlTextWriter(sw);
                LoadControl.RenderControl(myWriter);
                StringReader sr = new StringReader(sw.ToString());


                
                StringBuilder GridBuilder = new StringBuilder();
                GridBuilder.Append("<html>");
                GridBuilder.Append("<style>");
                GridBuilder.Append(".fontt{font-family:Angsana New;}");
                GridBuilder.Append("</style>");
                GridBuilder.Append("<body class=\"fontt\">");
                GridBuilder.Append(GridHtml);
                GridBuilder.Append("</body>");
                GridBuilder.Append("</html>");

                GridBuilder.Append(sw.ToString());


                string path = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Content\\" + "tablethin.css";
                string readText = System.IO.File.ReadAllText(path);

                Document pdfDoc = new Document(PageSize.A4, 25, 25, 10, 10);
                using (var writer = PdfWriter.GetInstance(pdfDoc, ms))
                {

                    pdfDoc.Open();
                    using (MemoryStream cssMemoryStream = new MemoryStream(Encoding.UTF8.GetBytes(readText)))
                    {
                        using (MemoryStream mss = new MemoryStream(Encoding.UTF8.GetBytes(GridBuilder.ToString().Replace(".png\">", ".png\"/>"))))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, mss, cssMemoryStream, Encoding.UTF8);
                        }
                        pdfDoc.Close();
                    }
                }
                return ms;
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError(ex.Message + ">> GetFileReportTomail_Preview");
                return ms;
            }

        }

    }
}