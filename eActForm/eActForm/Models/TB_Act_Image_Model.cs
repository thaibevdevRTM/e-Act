﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eActForm.Models
{
    public class TB_Act_Image_Model 
    {
        public class ImageModels
        {
           public List<ImageModel> tbActImageList { get; set; }
        }
        public class ImageModel : ActBaseModel
        {
            public string id { get; set; }
            public string activityId { get; set; }
            public string imageType { get; set; }
            public byte[] _image { get; set; }
            public string _fileName { get; set; }
            public string remark { get; set; }
            public string typeFiles { get; set; }
        }

    }
}