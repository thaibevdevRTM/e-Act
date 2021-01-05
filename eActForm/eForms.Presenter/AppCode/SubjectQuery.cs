using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLibrary;
using static eForms.Models.MasterData.SubjectModel;

namespace eForms.Presenter.AppCode
{
    public class SubjectQuery
    {
        public static List<TB_Reg_Subject_Model> getAllSubject(string strCon)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(strCon, CommandType.StoredProcedure, "usp_getAllSubject");
                var lists = (from DataRow d in ds.Tables[0].Rows
                             select new TB_Reg_Subject_Model()
                             {
                                 id = d["id"].ToString(),
                                 companyId = d["companyId"].ToString(),
                                 nameTH = d["nameTH"].ToString(),
                                 nameEn = d["nameEn"].ToString(),
                                 description = d["description"].ToString(),
                                 typeFormId = d["master_type_form_id"].ToString(),
                                 delFlag = bool.Parse(d["delFlag"].ToString()),
                                 createdDate = DateTime.Parse(d["createdDate"].ToString()),
                                 createdByUserId = d["createdByUserId"].ToString(),
                                 updatedDate = DateTime.Parse(d["updatedDate"].ToString()),
                                 updatedByUserId = d["updatedByUserId"].ToString(),
                             });
                return lists.ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.WriteError("getAllSubject => " + ex.Message);
                return new List<TB_Reg_Subject_Model>();
            }
        }
    }
}
