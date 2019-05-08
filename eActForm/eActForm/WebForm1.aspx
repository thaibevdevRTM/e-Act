<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="eActForm.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <table style="width:100%; margin-top:30px;">
                        <tr>
                            <td width="140px;"></td>
                            <td width="260px;" style="margin-left:10px;"></td>
                            <td width="20px;"></td>
                            <td width="150px;" style="text-align:right; font-size:12.5px;">Activity No.</td>
                            <td width="5px;"></td>
                            <td width="160px;" class="cssborderbottom" style="font-size:12.5px;"> @Html.DisplayFor(item => item.activityFormModel.activityNo) </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td style="text-align:right; font-size:12.5px;">Document Date</td>
                            <td></td>
                            <td class="cssborderbottom" style="font-size:12.5px;">@Model.activityFormModel.documentDate.Value.ToString("dd/MM/yyyy") </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td style="text-align:right; font-size:12.5px;">Reference</td>
                            <td></td>
                            <td class="cssborderbottom" style="font-size:12.5px;"> @Html.DisplayFor(item => item.activityFormModel.reference) </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td style="font-size:12.5px;">ชื่อกิจกรรม</td>
                            <td class="cssborderbottom" style="font-size:12.5px;">@Html.DisplayFor(item => item.activityFormModel.activityName) </td>
                            <td></td>
                            <td style="text-align:right; font-size:12.5px;">ลูกค้า(Customer Name)</td>
                            <td></td>
                            <td class="cssborderbottom" style="font-size:12.5px;">@Html.DisplayFor(item => item.activityFormModel.customerName) </td>
                        </tr>
                        <tr>
                            <td style="font-size:12.5px;">ประเภทกิจกรรม(Theme)</td>
                            <td class="cssborderbottom" style="font-size:12.5px;"> @Html.DisplayFor(item => item.activityFormModel.txttheme) </td>
                            <td></td>
                            <td style="text-align:right; font-size:12.5px;">ช่องทาง(Channel)</td>
                            <td></td>
                            <td class="cssborderbottom" style="font-size:12.5px;"> @Html.DisplayFor(item => item.activityFormModel.chanel) </td>
                        </tr>
                        <tr>
                            <td style="font-size:12.5px;">วัตถุประสงค์</td>
                            <td class="cssborderbottom" style="font-size:12.5px;">@Html.DisplayFor(item => item.activityFormModel.objective)</td>
                            <td></td>
                            <td style="text-align:right; font-size:12.5px;">กลุ่มสินค้า(Product Group)</td>
                            <td></td>
                            <td class="cssborderbottom" style="font-size:12.5px;"> @Html.DisplayFor(item => item.activityFormModel.productGroupText) </td>
                        </tr>
                        <tr>
                            <td style="height:10px;"></td>
                        </tr>
                        <tr>
                            <td style="font-size:12.5px;">ไม่ได้เกิดจาก รายการส่งเสริมการขาย (Trade Term)</td>
                            <td></td>
                            <td style="text-align:right; font-size:12.5px;">ระยะเวลากิจกรรม</td>
                            <td></td>
                            <td style="font-size:12.5px;">

                            </td>
                        </tr>
                        <tr>

                            <td style="font-size:12.5px;">รายการส่งเสริมการขาย (นอกTrade Term)</td>
                            <td></td>
                            <td style="text-align:right; font-size:12.5px;">(Activity Period)</td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td style="text-align:right; font-size:12.5px;">ระยะเวลาการให้ทุนพิเศษ</td>
                            <td></td>
                            <td style="font-size:12.5px;">
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td style="text-align:right; font-size:12.5px;">(Cost Period)</td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
</body>
</html>
