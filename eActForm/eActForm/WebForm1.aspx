
<style>
    .textRight {
        text-align: right;
    }

    .textLeft {
        text-align: left;
    }

    .line-breaks {
        white-space: pre-line;
    }

    .font12 {
        font-size: 12.5px;
    }
</style>



<div class="modal-body">
    <div id="divForm" style="width: 97%; margin-left: 20px;">
        <div id="divHeadCompany" runat="server" style="width: 100%; align-content: center; text-align: center;" class="fontDocV2">
            @Model.activityFormTBMMKT.companyName<br />
            @Model.activityFormTBMMKT.formName
        </div>


        <div>
            <div style="text-align: center;">
                <lable>ประจำเดือน : </lable>
            </div>
            <div style="text-align: right;">
                <lable>เลขที่เอกสาร   Exp 001 : </lable>
            </div>
        </div>
        <div>
            <div style="text-align: right;">
                <lable>เลขที่เอกสารใบยืมเงินทดรอง   ADV…..</lable>
            </div>
        </div>

        <div>
            <table style="width: 100%; margin-top: 30px;">
                <tr>
                    <td>ชื่อ / Name:
                    </td>
                    <td class="font12">ตำแหน่ง / Position:
                    </td>
                </tr>
                <tr>
                    <td class="font12">ระดับ / Level:
                    </td>
                    <td class="font12">หน่วยงาน / Dept.:
                    </td>
                </tr>
            </table>
        </div>
        <br />

        <div>
            Part 1 :   ค่าใช้จ่ายในการเดินทาง / Travel & Transportation Expenses
        </div>
        <div class="font12">
            - ประเทศหรืองานที่เดินทางไป / Country or For:
        </div>
        <br />
        <div>
            <table class="tablethin font12" style="width: 100%; text-align: center;" border="1" cellpadding="0" cellspacing="0">
                <tr style="background-color: #E5E2E2; height: 25px;">
                    <td style="text-align: center; width: 15%;">วันเดือนปี</td>
                    <td style="text-align: center; width: 30%;">รายการ</td>
                    <td style="text-align: center; width: 15%;">ลิตร</td>
                    <td style="text-align: center; width: 15%;">จำนวนเงิน</td>
                    <td style="text-align: center; width: 26%;">หมายเหตุ</td>
                </tr>
                <tr>
                    <td>1</td>
                    <td></td>
                    <td></td>
                    <td>&nbsp;</td>
                    <td></td>
                </tr>
                <tr>
                    <td>2</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>3</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="2" class="textRight">รวมค่าใช้จ่ายในการเดินทาง/ต่าใช้จ่ายประจำเดือน (ไม่รวมค่าเลี้ยงรับรองหรือการตรวจตลาด)
                Total Travel & Transporation Expenses (Excluding Entertainment Expenses and Market Visit)
                    </td>
                    <td class="textRight">
                        <label id="lblLite"></label>
                    </td>
                    <td class="textRight">
                        <label id="lblShowtotal"></label>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
        </div>
        <br />
        <br />
        <div>
            Part 2 : ค่าใช้จ่ายในการเลี้ยงรับรอง หรือการตรวจตลาด / Entertainment Expenses and Market Visit
        </div>
        <div class="font12">
            <lable style="color: red"> * หมายเหตุ:</lable>
            <lable>ค่าใช้จ่ายส่วนนี้ จะต้องผ่านการอนุมัติตามอำนาจการอนุมัติได้เท่านั้น *</lable>
        </div>
        <br />
        <div>
            <table class="tablethin font12" style="width: 100%; text-align: center;" border="1" cellpadding="0" cellspacing="0">
                <tr style="background-color: #E5E2E2; height: 25px;">
                    <td style="text-align: center; width: 15%;">วันที่</td>
                    <td style="text-align: center; width: 20%;">สถานที่</td>
                    <td style="text-align: center; width: 35%;">จุดประสงค์</td>
                    <td style="text-align: center; width: 20%;">ชื่อลูกค้า</td>
                    <td style="text-align: center; width: 15%;">รวมเป็นจำนวนเงิน</td>
                </tr>
                <tr>
                    <td>1</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>2</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>3</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="4" class="textRight">รวมค่าใช้จ่ายในการเลี้ยงรับรองและ/หรือการตรวจตลาด
                Total Entertainment Expenses and Market Visit
                    </td>
                    <td class="textRight"></td>
                </tr>
                <tr>
                    <td colspan="4" class="textRight">กรณีมีการเบิกเงินค่าใช้จ่ายล่วงหน้า จำนวนที่ขอเบิกไป  (บาท) /  Cash Advance (Baht)  =
                    </td>
                    <td class="textRight"></td>
                </tr>
                <tr>
                    <td colspan="4" class="textRight">รวมค่าใช้จ่ายที่ต้องการเบิกในครั้งนี้ (บาท) /  Total All Expenses  =
                    </td>
                    <td class="textRight"></td>
                </tr>
            </table>
        </div>

    </div>
</div>
