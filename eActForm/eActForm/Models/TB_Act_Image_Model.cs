using eActForm.BusinessLayer;
using System;
using System.Collections.Generic;

namespace eActForm.Models
{
    public class TB_Act_Image_Model
    {
        public class ImageModels
        {
            public List<ImageModel> tbActImageList { get; set; }

            public ImageModels()
            {
                tbActImageList = new List<ImageModel>();
            }
        }
        public class ImageModel : ActBaseModel
        {
            public ImageModel()
            {
                try
                {
                    _image = new byte[0];
                    extension = ".pdf";
                    delFlag = false;
                    createdDate = DateTime.Now;
                    updatedDate = DateTime.Now;
                }
                catch (Exception ex) { }
            }
            public string id { get; set; }
            public string activityId { get; set; }
            public string imageType { get; set; }
            public byte[] _image { get; set; }
            public string _fileName { get; set; }
            public string extension { get; set; }
            public string remark { get; set; }
            public string typeFiles { get; set; }
            public string sizeFiles { get; set; }
        }

    }
}