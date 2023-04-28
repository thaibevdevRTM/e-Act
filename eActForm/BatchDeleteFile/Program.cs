// See https://aka.ms/new-console-template for more information
using BatchDeleteFile;
using System;
using System.IO;

Console.WriteLine("Process Delete File");

StreamWriter strLogs = File.AppendText(AppCode.LogsFileName());
var getListFile = AppCode.getFileList(BatchDeleteFile.Properties.Resources.strCon);
var pathh = BatchDeleteFile.Properties.Resources.rootUploadfiles;
int countSuccess = 0 , UnSuccess = 0;

foreach (var item in getListFile)
{
    try
    {  
        var getStr = string.Format(pathh, item._fileName);
        System.IO.File.Delete(getStr);

        strLogs.WriteLine("== Delete Success " + getStr);
        strLogs.WriteLine("== Delete Success " + DateTime.Now.ToString("dd MM yyyy HH:mm:ss "));
        countSuccess++;
    }
    catch(Exception ex)
    {
        strLogs.WriteLine(" Error :" + ex.Message);
        strLogs.WriteLine("== END " + DateTime.Now.ToString("dd MM yyyy HH:mm:ss "));
        UnSuccess++;
    }

}

strLogs.WriteLine(" Count File Success:" + countSuccess + "Count File UnSuccess: " + UnSuccess);
strLogs.Flush();
strLogs.Close();

Console.WriteLine("Delete File Success !!");


