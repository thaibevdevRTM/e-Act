using System;

namespace eForms.Presenter.AppCode
{
    public class AjaxResult
    {
        public AjaxResult()
        {
        }
        public AjaxResult(Exception result)
        {
            this.Success = false;
            this.Message = result.Message;
            this.Code = 0;
        }

        #region Member
        public bool Success { get; set; }
        public string Message { get; set; }
        public int Code { get; set; }
        public object MessageType { get; set; }
        public object Data { get; set; }
        public string MessageCode { get; set; }
        public string ActivityId { get; set; }
        #endregion
    }
}
