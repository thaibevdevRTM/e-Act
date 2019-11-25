﻿<div id="divHeaderDetails" runat="server" style="width: 100%; padding-left: 15px; padding-right: 15px;">
    รูปแบบการดำเนินการ
    <table style="width: 100%; border-style: solid; border-width: 1px 0px 0px 0px; border-collapse: collapse;" id="tabelHeaderDetails" runat="server">
        <tr style="background-color: #2f74b5; color: white; height: 25px;">
            <th style="width: 5%; border-style: solid; border-width: 0px 1px 0px 1px; border-collapse: collapse; text-align: center;">ลำดับ</th>
            <th style="width: 15%; border-style: solid; border-width: 0px 1px 0px 0px; border-collapse: collapse; text-align: center;">IO</th>
            <th style="width: 65%; border-style: solid; border-width: 0px 1px 0px 0px; border-collapse: collapse; text-align: center;">กิจกรรม</th>
            <th style="width: 15%; border-style: solid; border-width: 0px 1px 0px 0px; border-collapse: collapse; text-align: center;">จำนวนเงินรวม</th>
        </tr>
        @{ var index = 0;}
        @foreach (var item in Model.list_TB_Act_ActivityLayout)
        {
            var rankNo = (index + 1);
            <tr>
                <td style="width: 5%; border-style: solid; border-width: 1px 1px 0px 1px; border-collapse: collapse; text-align: center;">
                    <input type="hidden" value="@item.id" id="list_TB_Act_ActivityLayout[@index].id" name="list_TB_Act_ActivityLayout[@index].id" class="cssTableTDTextCenter" />
                    <input type="text" value="@rankNo" id="list_TB_Act_ActivityLayout[@index].no" name="list_TB_Act_ActivityLayout[@index].no" class="cssTableTDTextCenter" readonly="readonly" />
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="@item.io" id="list_TB_Act_ActivityLayout[@index].io" name="list_TB_Act_ActivityLayout[@index].io" class="cssTableTDTextCenter" />
                </td>
                <td style="width: 65%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="@item.activity" id="list_TB_Act_ActivityLayout[@index].activity" name="list_TB_Act_ActivityLayout[@index].activity" class="cssTableTDTextLeft" />
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: right;">
                    <input type="text" value="@item.amount" id="list_TB_Act_ActivityLayout[@index].amount" name="list_TB_Act_ActivityLayout[@index].amount" class="cssTableTDDetailNumber" />
                </td>
            </tr>
        index++;
        }
        <tr>
            <td colspan="3" style="width: 85%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: right;">TOTAL&nbsp;
            </td>
            <td style="width: 15%; border-style: solid; border-width: 1px 1px 1px 0px; border-collapse: collapse; text-align: right;">0,000.00&nbsp;
            </td>
        </tr>
    </table>

</div>
