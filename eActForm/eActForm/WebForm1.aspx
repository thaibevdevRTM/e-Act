<table style="width: 100%; border-style: solid; border-width: 1px 1px 0px 1px; border-collapse: collapse;" id="tabel_report" runat="server">


    <tbody>
        <tr>


            <td>
                <div id="divHeadCompany" runat="server" style="width: 100%; align-content: center; text-align: center;">
                    บริษัท  ไทยเบฟมาร์เก็ตติ้ง  จำกัด<br />
                    15 หมู่ 14 ถนนวิภาวดี-รังสิต แขวงจอมพล เขตจตุจักร กรุงเทพฯ 10900"
                </div>
            </td>


        </tr>


        <tr>


            <td style="border-style: solid; border-width: 1px 0px 0px 0px; border-collapse: collapse; border-bottom: none; border-left: none; border-right: none;">


                <div id="div1" runat="server" style="width: 100%; align-content: center; text-align: center;">
                    ขออนุมัติงบประมาณกิจกรรม.....................<input class="" id="txtActivityId" name="activityFormModel.id" type="text" value="75c3b4a3-60b6-4737-999b-1eed012266e3">
                </div>


            </td>


        </tr>


        <tr>
            <td style="border-style: solid; border-width: 1px 0px 0px 0px; border-collapse: collapse; border-bottom: none; border-left: none; border-right: none;">
                <div id="div2" runat="server" style="width: 100%; align-content: left; text-align: left;">

                    <div style="display: inline-block;">
                        <label class="control-label" for="ddlRegion">
                            &nbsp;Condition Flow<span class="required">:&nbsp;</span>
                        </label>
                        <select class="" id="ddlChannelOrBrand" name="activityFormTBMMKT.selectedBrandOrChannel">
                            <option value=""></option>
                            <option value=""></option>
                            <option value="Channel">Channel</option>
                            <option selected="selected" value="Brand">Brand</option>
                        </select>

                        <script>
                            function processDropdownConditionFlow() {
                                if ($("#ddlChannelOrBrand").val() == "Channel") {
                                    $('#divChannel').css('display', 'inline-block');
                                    $("#divSubject").css('display', 'inline-block');
                                    $("#divBrand").hide();
                                }
                                else if ($("#ddlChannelOrBrand").val() == "Brand") {
                                    $('#divBrand').css('display', 'inline-block');
                                    $("#divSubject").css('display', 'inline-block');
                                    $("#divChannel").hide();
                                }
                                else {
                                    $("#divBrand").hide();
                                    $("#divChannel").hide();
                                    $("#divSubject").hide();
                                }
                            }

                            $(function () {
                                //ทำหลังLoadหน้่าเว็บเสร็จ
                                var p_subjectId = "8EFFC1B8-DA48-4FFA-A3EC-858F3983541C";
                                var p_productBrandId = "BC05AADC-A306-4D33-8383-521B8CAB2B2F";
                                var p_channelId = "";
                                if (p_subjectId != "") {
                                    $("#divSubject").css('display', 'inline-block');
                                    $("#ddlSubject").val(p_subjectId);
                                }
                                if (p_productBrandId != "") {
                                    $("#divBrand").css('display', 'inline-block');
                                    $("#ddlBrand").val(p_productBrandId);
                                }
                                if (p_channelId != "") {
                                    $("#divChannel").css('display', 'inline-block');
                                    $("#ddlChannel").val(p_channelId);
                                }
                            });

                            $("#ddlChannelOrBrand").change(function () {
                                processDropdownConditionFlow();
                            });
                        </script>

                    </div>
                    <div id="divChannel" style="display: none;">
                        <label class="control-label" for="ddlChannel">
                            &nbsp;เลือก Channel<span class="required">:&nbsp;</span>
                        </label>
                        <select class="" id="ddlChannel" name="activityFormTBMMKT.channelId">
                            <option value=""></option>
                            <option value="012A178F-CF7A-41A7-A6BA-712A56628310">HYPER     </option>
                            <option value="28850F77-0378-4E56-81FF-794BD5A2697D">On-Trade</option>
                            <option value="3C9BEA0A-3C3E-486B-A721-9BA3BBC070DD">Traditional-Trade</option>
                            <option value="3FC2482A-EC21-4410-AF75-214B913BC254">Modern-Trade</option>
                            <option value="45FFB6E3-8A6D-4E15-9F71-A6871370B4FB">SALES</option>
                            <option value="4DE33D97-3A2E-4FD9-A50E-25C4E36DB966">SalesSupport</option>
                            <option value="69FCB7DA-5E5E-4551-90BC-6F908D2A6A70">Cashvan</option>
                            <option value="6A007687-9B71-4C05-A432-9FD2A5C9FE9D">เสริมสุข
                            </option>
                            <option value="74F060E9-65A2-4B04-B23F-E8B382DB4347">Convenience</option>
                            <option value="89DF1E8A-3586-4C33-BCCB-6A8CA6A8579B">NPD-RKS</option>
                            <option value="9AA77E79-E585-4D2D-A503-2DA8E2367E5E">SUPER     </option>
                            <option value="A6B1B576-D2D2-4237-B439-9AD6CCCC31D9">Event</option>
                            <option value="D139F904-DC4C-4738-A84E-AF082A46577F">NPD-Kulov</option>
                            <option value="FC7AFD02-64B9-46DB-A5AC-FF64EC14C107">KEG</option>
                        </select>
                    </div>
                    <div id="divBrand" style="display: inline-block;">
                        <label class="control-label" for="ddlBrand">
                            &nbsp;เลือก Brand<span class="required"></span>
                        </label>
                        <select class="" id="ddlBrand" name="activityFormTBMMKT.BrandlId">
                            <option value=""></option>
                            <option value="6B623E91-AE6E-4621-A6CD-3F567D19BC70">100Plus                       </option>
                            <option value="5FBD7C30-3538-4798-A51A-7AB46A67DF1A">333</option>
                            <option value="E8D744E5-E63B-4A6C-A00E-EDF79F463656">anCnoc                        </option>
                            <option value="27541C34-47F5-4EC9-9B62-5AD61013C6FA">Archa                         </option>
                            <option value="5A3F44B0-B857-4E15-B6B8-708400F783B3">Bangyikhan                    </option>
                            <option value="352FC565-A533-49AA-A74B-755F92F0B11C">Blend285                      </option>
                            <option value="8DB7456E-C36E-4030-9C6E-01D36C5799FA">CAORUNN                       </option>
                            <option value="B438C726-78E5-4704-9AEC-7A9DF82AAB7F">Chaiya                        </option>
                            <option value="7CA5340A-747B-486C-81C5-D206B081D96A">Chang Soda</option>
                            <option value="B8FCCD44-DFC7-4D52-968A-A093F3592FA0">ChangClassic                  </option>
                            <option value="C6DD3778-AE83-4287-9A60-83257D2116D2">ChaoPraya                     </option>
                            <option value="FC2F7528-7EBC-4EB9-82A7-DE502EBA1504">ChiangChun                    </option>
                            <option value="23111FEC-6CC7-45BA-9F4F-543D96355BD3">ChooSibNiw                    </option>
                            <option value="A292B627-2FB7-4B0D-A0D4-050C6B31CB36">Crown99                       </option>
                            <option value="2395EA4D-5CD5-4DDB-A7B6-48EF819B99BB">Crystal                       </option>
                            <option value="9EC1CA68-591D-4B3A-9A65-6509C6ED965E">EST                           </option>
                            <option value="F878B8C1-33BF-411D-98D2-3245D38281D3">Federbrau                     </option>
                            <option value="1D8F1409-9A19-46AC-B1D3-D71919351716">Oishi</option>
                            <option value="3F730723-6A7B-42A4-820F-D30B344400C5">HongThong                     </option>
                            <option value="32459970-AACE-4E67-B58B-F6D786F8D7A1">JubJai                        </option>
                            <option value="8B115105-BCD0-41F0-9809-32FE2206F54D">Kulov                         </option>
                            <option value="CDF110AC-B576-49DB-B337-D05147CA8D89">MaeWangWaree                  </option>
                            <option value="66E2DD90-6B32-43D7-9A93-126F749CB51F">Mekhong                       </option>
                            <option value="69B92596-9195-4F2B-AE06-09DF094DA1F5">Meridian                      </option>
                            <option value="A3182E30-C2C5-495B-ACBD-8E082FB6DCDE">MungkornThong                 </option>
                            <option value="B79CFC04-5523-4096-A167-E8817D31D3E3">Niyomthai                     </option>
                            <option value="8185052B-1EB9-4AE4-9548-EB71E8B81DE2">Paitong                       </option>
                            <option value="02A34932-3BA4-47DB-8558-C02FF486173F">PhayaSeur                     </option>
                            <option value="C4923C86-C05B-493E-AC6A-DDD8AD03E313">PHRAYA                        </option>
                            <option value="68590D33-1E6D-4E7A-B928-BD31F905F512">Prayanak                      </option>
                            <option value="7CDA78AE-B8D7-4C10-AFD3-E3C06EC418C1">PULTENEY                      </option>
                            <option value="BC05AADC-A306-4D33-8383-521B8CAB2B2F">Rock Mountain                 </option>
                            <option value="94AB57BC-240C-4CFB-9C7F-AF3796750165">RuangKhao                     </option>
                            <option value="8E850360-0C10-40B5-B885-75397BA78133">Saigon                        </option>
                            <option value="860795ED-755B-4978-8905-D0541466A437">Sangsom                       </option>
                            <option value="B5BFF64F-C3CA-4699-B504-B82F3B1DABF9">Sarsi                         </option>
                            <option value="FD216A0A-7CC6-40A5-828E-C30A319F1FF9">SauDum                        </option>
                            <option value="A524420B-0830-46FE-9CAA-90E6ECC1EB59">Star Cooler                   </option>
                            <option value="FB2E5E74-F369-4586-BA54-88DAF2321AC2">Tapper                        </option>
                            <option value="8AFFCAD6-4644-425F-8665-3740584704D1">WhiteTiger                    </option>
                            <option value="BB57DBF4-C281-4F79-9481-2B8A4C53C723">Wrangyer                      </option>
                            <option value="3B936397-55EC-475B-9441-5BE7DE1F80F5">Chang Water                        </option>
                            <option value="5FBD7C30-3538-4798-A51A-7AB46A67DF01">Huntsman</option>
                            <option value="5FBD7C30-3538-4798-A51A-7AB46A67DF35">Black Dragon</option>
                        </select>
                    </div>
                    <div id="divSubject" style="display: inline-block;">
                        <label class="control-label" for="ddlBrand">
                            &nbsp;เรื่อง <span class="required"></span>
                        </label>
                        <select class="" id="ddlSubject" name="activityFormTBMMKT.SubjectId">
                            <option value=""></option>
                            <option value="2269FEA7-4C26-4065-B029-E7A204522065">4.Sport | ขออนุมัติ งบประมาณกิจกรรม</option>
                            <option value="3CB983AF-2AFE-4626-A4CA-D7EBE00BBF3A">3.Research | ขออนุมัติ งบประมาณกิจกรรม</option>
                            <option value="41EE36A0-5738-4301-B36A-9780001AFCAA">2.Event
 | ขออนุมัติ งบประมาณกิจกรรม</option>
                            <option value="8D0DB7BD-FC7B-4C0D-BECC-2C19FE0358F1">6.Other | ขออนุมัติ งบประมาณกิจกรรม</option>
                            <option value="8EFFC1B8-DA48-4FFA-A3EC-858F3983541C">1.Media | ขออนุมัติ งบประมาณกิจกรรม</option>
                            <option value="AC333866-AAEB-4B5F-8D27-7E116655ADB6">7.เบิกตัดPOS/Premium | ขออนุมัติ งบประมาณกิจกรรม</option>
                            <option value="FC61FDFB-D561-4D24-88FC-9AF8ABB5C7AE">5.Design/Creative | ขออนุมัติ งบประมาณกิจกรรม</option>
                        </select>
                    </div>

                </div>
            </td>
        </tr>
        <tr style="border: 1px solid black; border-collapse: collapse; border-bottom: none;">
            <td>
                <div id="divHeaderDetails" runat="server" style="width: 100%;">
                    <table style="width: 100%;" id="tabelHeaderDetails" runat="server">
                        <tbody>
                            <tr>
                                <td style="width: 15%; border-style: solid; border-width: 1px 0.5px 0px 0.5px; border-collapse: collapse; border-left: none; border-top: none;">&nbsp;วันที่เสนอกิจกรรม
                                </td>
                                <td colspan="3" style="border-style: solid; border-width: 1px 0px 0px 0.5px; border-collapse: collapse; border-right: none; border-top: none;">
                                    <input class="" data-val="true" data-val-date="The field documentDate must be a date." id="txtdateDoc" name="activityFormModel.documentDate" type="text" value="11/21/2019 12:00:00 AM">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%; border-style: solid; border-width: 1px 0.5px 0px 0.5px; border-collapse: collapse; border-left: none;">&nbsp;ชื่อผู้เสนอกิจกรรม
                                </td>
                                <td style="width: 55%; border-style: solid; border-width: 1px 0px 0px 0.5px; border-collapse: collapse; border-right: none;">พีรภพ  อินทรวิเชียร
                                </td>
                                <td style="width: 10%; border-style: solid; border-width: 1px 0px 0px 0px; border-collapse: collapse; border-right: none; border-left: none;">&nbsp;เบอร์โทรศัพท์
                                </td>
                                <td style="width: 20%; border-style: solid; border-width: 1px 0px 0px 0px; border-collapse: collapse; border-right: none; border-left: none;">
                                    <input class="" id="txtactivityTel" name="tB_Act_ActivityForm_DetailOther.activityTel" type="text" value="">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%; border-style: solid; border-width: 1px 0.5px 0px 0.5px; border-collapse: collapse; border-left: none;">&nbsp;สินค้า
                                </td>
                                <td style="width: 55%; border-style: solid; border-width: 1px 0px 0px 0.5px; border-collapse: collapse; border-right: none;">
                                    <input class="" id="txtactivityProduct" name="tB_Act_ActivityForm_DetailOther.activityProduct" type="text" value="">
                                </td>
                                <td style="width: 10%; border-style: solid; border-width: 1px 0px 0px 0px; border-collapse: collapse; border-right: none; border-left: none;">&nbsp;เอกสารเลขที่
                                </td>
                                <td style="width: 20%; border-style: solid; border-width: 1px 0px 0px 0px; border-collapse: collapse; border-right: none; border-left: none;">
                                    <input class="" id="txtActivityNo" name="activityFormModel.activityNo" readonly="readonly" type="text" value="S620151">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%; border-style: solid; border-width: 1px 0.5px 0px 0.5px; border-collapse: collapse; border-left: none;">&nbsp;ชื่อกิจกรรม
                                </td>
                                <td style="width: 55%; border-style: solid; border-width: 1px 0px 0px 0.5px; border-collapse: collapse; border-right: none;">
                                    <input class="" id="txtActivityName" name="activityFormModel.activityName" type="text" value="">
                                </td>
                                <td style="width: 10%; border-style: solid; border-width: 1px 0px 0px 0px; border-collapse: collapse; border-right: none; border-left: none;">&nbsp;EO
                                </td>
                                <td style="width: 20%; border-style: solid; border-width: 1px 0px 0px 0px; border-collapse: collapse; border-right: none; border-left: none;">
                                    <input class="" id="txtEO" name="tB_Act_ActivityForm_DetailOther.EO" type="text" value="">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%; border-style: solid; border-width: 1px 0.5px 0px 0.5px; border-collapse: collapse; border-left: none; border-bottom: none;">&nbsp;เริ่มกิจกรรม
                                </td>
                                <td colspan="3" style="width: 55%; border-style: solid; border-width: 1px 0px 0px 0.5px; border-collapse: collapse; border-right: none; border-bottom: none;">
                                    <input class="" data-val="true" data-val-date="The field activityPeriodSt must be a date." id="txtdateActivitySt" name="activityFormModel.activityPeriodSt" type="text" value="21-11-2019">&nbsp;ถึง&nbsp;<input class="" data-val="true" data-val-date="The field activityPeriodEnd must be a date." id="txtactivityPeriodEnd" name="activityFormModel.activityPeriodEnd" type="text" value="21-11-2019">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%; border-style: solid; border-width: 1px 0.5px 0px 0px; border-collapse: collapse; border-left: none; border-bottom: none; vertical-align: top;">&nbsp;วัตถุประสงค์
                                </td>
                                <td colspan="3" style="width: 55%; border-style: solid; border-width: 1px 0px 0px 0.5px; border-collapse: collapse; border-right: none; border-bottom: none;">
                                    <textarea class="" cols="20" id="txtObjective" name="activityFormModel.objective" rows="2"></textarea>
                                </td>
                            </tr>
                        </tbody>
                    </table>

                </div>
            </td>
        </tr>
        <tr>
            <td style="border-style: solid; border-width: 1px 0px 0px 0px; border-collapse: collapse; border-bottom: none; border-left: none; border-right: none;">
                <div style="height: 5px;"></div>
                <div style="width: 100%; text-align: left;">
                    &nbsp;ค่าใช้จ่ายในการทำกิจกรรม..........................จำนวนเงิน............บาท
                        <br />
                    &nbsp;รายละเอียดงบประมาณ
                </div>
            </td>
        </tr>
        <tr style="border-bottom: none;">
            <td>
                <div style="height: 5px;"></div>

                <div id="div3" runat="server" style="width: 100%; padding-left: 15px; padding-right: 15px;">
                    รูปแบบการดำเนินการ
    <table style="width: 100%; border-style: solid; border-width: 1px 0px 0px 0px; border-collapse: collapse;" id="Table1" runat="server">
        <tbody>
            <tr style="background-color: #2f74b5; color: white; height: 25px;">
                <th style="width: 5%; border-style: solid; border-width: 0px 1px 0px 1px; border-collapse: collapse; text-align: center;">ลำดับ</th>
                <th style="width: 15%; border-style: solid; border-width: 0px 1px 0px 0px; border-collapse: collapse; text-align: center;">IO</th>
                <th style="width: 65%; border-style: solid; border-width: 0px 1px 0px 0px; border-collapse: collapse; text-align: center;">กิจกรรม</th>
                <th style="width: 15%; border-style: solid; border-width: 0px 1px 0px 0px; border-collapse: collapse; text-align: center;">จำนวนเงินรวม</th>
            </tr>

            <tr>
                <td style="width: 5%; border-style: solid; border-width: 1px 1px 0px 1px; border-collapse: collapse; text-align: center;">
                    <input type="hidden" value="039d1464-186c-4589-8fa7-bc62a7fb777e" id="list_TB_Act_ActivityLayout[0].id" name="list_TB_Act_ActivityLayout[0].id" class="cssTableTDTextCenter">
                    <input type="text" value="1" id="list_TB_Act_ActivityLayout[0].no" name="list_TB_Act_ActivityLayout[0].no" class="cssTableTDTextCenter" readonly="readonly">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="" id="list_TB_Act_ActivityLayout[0].io" name="list_TB_Act_ActivityLayout[0].io" class="cssTableTDTextCenter">
                </td>
                <td style="width: 65%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="" id="list_TB_Act_ActivityLayout[0].activity" name="list_TB_Act_ActivityLayout[0].activity" class="cssTableTDTextLeft">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: right;">
                    <input type="text" value="0.00" id="list_TB_Act_ActivityLayout[0].amount" name="list_TB_Act_ActivityLayout[0].amount" class="cssTableTDDetailNumber">
                </td>
            </tr>
            <tr>
                <td style="width: 5%; border-style: solid; border-width: 1px 1px 0px 1px; border-collapse: collapse; text-align: center;">
                    <input type="hidden" value="6660f554-1f5b-42cb-8f01-48613674916f" id="list_TB_Act_ActivityLayout[1].id" name="list_TB_Act_ActivityLayout[1].id" class="cssTableTDTextCenter">
                    <input type="text" value="2" id="list_TB_Act_ActivityLayout[1].no" name="list_TB_Act_ActivityLayout[1].no" class="cssTableTDTextCenter" readonly="readonly">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="" id="list_TB_Act_ActivityLayout[1].io" name="list_TB_Act_ActivityLayout[1].io" class="cssTableTDTextCenter">
                </td>
                <td style="width: 65%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="" id="list_TB_Act_ActivityLayout[1].activity" name="list_TB_Act_ActivityLayout[1].activity" class="cssTableTDTextLeft">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: right;">
                    <input type="text" value="0.00" id="list_TB_Act_ActivityLayout[1].amount" name="list_TB_Act_ActivityLayout[1].amount" class="cssTableTDDetailNumber">
                </td>
            </tr>
            <tr>
                <td style="width: 5%; border-style: solid; border-width: 1px 1px 0px 1px; border-collapse: collapse; text-align: center;">
                    <input type="hidden" value="9c6114a3-fc8a-4485-aed9-9bcabcb33273" id="list_TB_Act_ActivityLayout[2].id" name="list_TB_Act_ActivityLayout[2].id" class="cssTableTDTextCenter">
                    <input type="text" value="3" id="list_TB_Act_ActivityLayout[2].no" name="list_TB_Act_ActivityLayout[2].no" class="cssTableTDTextCenter" readonly="readonly">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="" id="list_TB_Act_ActivityLayout[2].io" name="list_TB_Act_ActivityLayout[2].io" class="cssTableTDTextCenter">
                </td>
                <td style="width: 65%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="" id="list_TB_Act_ActivityLayout[2].activity" name="list_TB_Act_ActivityLayout[2].activity" class="cssTableTDTextLeft">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: right;">
                    <input type="text" value="0.00" id="list_TB_Act_ActivityLayout[2].amount" name="list_TB_Act_ActivityLayout[2].amount" class="cssTableTDDetailNumber">
                </td>
            </tr>
            <tr>
                <td style="width: 5%; border-style: solid; border-width: 1px 1px 0px 1px; border-collapse: collapse; text-align: center;">
                    <input type="hidden" value="0ea6417d-b17b-494a-a0d5-dcc61f353e86" id="list_TB_Act_ActivityLayout[3].id" name="list_TB_Act_ActivityLayout[3].id" class="cssTableTDTextCenter">
                    <input type="text" value="4" id="list_TB_Act_ActivityLayout[3].no" name="list_TB_Act_ActivityLayout[3].no" class="cssTableTDTextCenter" readonly="readonly">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="" id="list_TB_Act_ActivityLayout[3].io" name="list_TB_Act_ActivityLayout[3].io" class="cssTableTDTextCenter">
                </td>
                <td style="width: 65%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="" id="list_TB_Act_ActivityLayout[3].activity" name="list_TB_Act_ActivityLayout[3].activity" class="cssTableTDTextLeft">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: right;">
                    <input type="text" value="0.00" id="list_TB_Act_ActivityLayout[3].amount" name="list_TB_Act_ActivityLayout[3].amount" class="cssTableTDDetailNumber">
                </td>
            </tr>
            <tr>
                <td style="width: 5%; border-style: solid; border-width: 1px 1px 0px 1px; border-collapse: collapse; text-align: center;">
                    <input type="hidden" value="7de3d8f7-c8af-49f2-b8ea-ac4ebe6a9b31" id="list_TB_Act_ActivityLayout[4].id" name="list_TB_Act_ActivityLayout[4].id" class="cssTableTDTextCenter">
                    <input type="text" value="5" id="list_TB_Act_ActivityLayout[4].no" name="list_TB_Act_ActivityLayout[4].no" class="cssTableTDTextCenter" readonly="readonly">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="" id="list_TB_Act_ActivityLayout[4].io" name="list_TB_Act_ActivityLayout[4].io" class="cssTableTDTextCenter">
                </td>
                <td style="width: 65%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="" id="list_TB_Act_ActivityLayout[4].activity" name="list_TB_Act_ActivityLayout[4].activity" class="cssTableTDTextLeft">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: right;">
                    <input type="text" value="0.00" id="list_TB_Act_ActivityLayout[4].amount" name="list_TB_Act_ActivityLayout[4].amount" class="cssTableTDDetailNumber">
                </td>
            </tr>
            <tr>
                <td style="width: 5%; border-style: solid; border-width: 1px 1px 0px 1px; border-collapse: collapse; text-align: center;">
                    <input type="hidden" value="9f2a8b27-a117-43d8-bf64-b9750418c42d" id="list_TB_Act_ActivityLayout[5].id" name="list_TB_Act_ActivityLayout[5].id" class="cssTableTDTextCenter">
                    <input type="text" value="6" id="list_TB_Act_ActivityLayout[5].no" name="list_TB_Act_ActivityLayout[5].no" class="cssTableTDTextCenter" readonly="readonly">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="" id="list_TB_Act_ActivityLayout[5].io" name="list_TB_Act_ActivityLayout[5].io" class="cssTableTDTextCenter">
                </td>
                <td style="width: 65%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="" id="list_TB_Act_ActivityLayout[5].activity" name="list_TB_Act_ActivityLayout[5].activity" class="cssTableTDTextLeft">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: right;">
                    <input type="text" value="0.00" id="list_TB_Act_ActivityLayout[5].amount" name="list_TB_Act_ActivityLayout[5].amount" class="cssTableTDDetailNumber">
                </td>
            </tr>
            <tr>
                <td style="width: 5%; border-style: solid; border-width: 1px 1px 0px 1px; border-collapse: collapse; text-align: center;">
                    <input type="hidden" value="d54b6bf1-715d-4f8b-af0e-c3e9d61d9c83" id="list_TB_Act_ActivityLayout[6].id" name="list_TB_Act_ActivityLayout[6].id" class="cssTableTDTextCenter">
                    <input type="text" value="7" id="list_TB_Act_ActivityLayout[6].no" name="list_TB_Act_ActivityLayout[6].no" class="cssTableTDTextCenter" readonly="readonly">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="" id="list_TB_Act_ActivityLayout[6].io" name="list_TB_Act_ActivityLayout[6].io" class="cssTableTDTextCenter">
                </td>
                <td style="width: 65%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="" id="list_TB_Act_ActivityLayout[6].activity" name="list_TB_Act_ActivityLayout[6].activity" class="cssTableTDTextLeft">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: right;">
                    <input type="text" value="0.00" id="list_TB_Act_ActivityLayout[6].amount" name="list_TB_Act_ActivityLayout[6].amount" class="cssTableTDDetailNumber">
                </td>
            </tr>
            <tr>
                <td colspan="3" style="width: 85%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: right;">TOTAL&nbsp;
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 1px 0px; border-collapse: collapse; text-align: right;">0,000.00&nbsp;
                </td>
            </tr>
        </tbody>
    </table>

                </div>
            </td>
        </tr>
        <tr style="border-bottom: none;">
            <td>
                <div style="height: 5px;"></div>
                <div id="div4" runat="server" style="width: 100%; padding-left: 15px; padding-right: 15px;">
                    รายละเอียดงบประมาณ
    <table style="width: 100%; border-style: solid; border-width: 1px 0px 0px 0px; border-collapse: collapse;" id="Table2" runat="server">
        <tbody>
            <tr style="background-color: #2f74b5; color: white; height: 25px;">
                <th style="width: 15%; border-style: solid; border-width: 0px 1px 0px 1px; border-collapse: collapse; text-align: center;">IO</th>
                <th style="width: 50%; border-style: solid; border-width: 0px 1px 0px 0px; border-collapse: collapse; text-align: center;">รายละเอียด</th>
                <th style="width: 5%; border-style: solid; border-width: 0px 1px 0px 0px; border-collapse: collapse; text-align: center;">จำนวน</th>
                <th style="width: 15%; border-style: solid; border-width: 0px 1px 0px 0px; border-collapse: collapse; text-align: center;">ราคาต่อหน่วย</th>
                <th style="width: 15%; border-style: solid; border-width: 0px 1px 0px 0px; border-collapse: collapse; text-align: center;">จำนวนเงินรวม</th>
            </tr>

            <tr>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 1px; border-collapse: collapse; text-align: center;">
                    <input type="hidden" value="0D01CD78-B94A-492A-BFE3-25E8662BC100" id="costThemeDetailOfGroupByPriceTBMMKT[0].id" name="costThemeDetailOfGroupByPriceTBMMKT[0].id" class="cssTableTDTextCenter">
                    <input type="text" value="" id="costThemeDetailOfGroupByPriceTBMMKT[0].IO" name="costThemeDetailOfGroupByPriceTBMMKT[0].IO" class="cssTableTDTextCenter">
                </td>
                <td style="width: 50%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="" id="costThemeDetailOfGroupByPriceTBMMKT[0].productDetail" name="costThemeDetailOfGroupByPriceTBMMKT[0].productDetail" class="cssTableTDTextLeft">
                </td>
                <td style="width: 5%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="0" id="costThemeDetailOfGroupByPriceTBMMKT[0].unit" name="costThemeDetailOfGroupByPriceTBMMKT[0].unit" class="cssTableTDTextCenter">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="0.00000" id="costThemeDetailOfGroupByPriceTBMMKT[0].unitPrice" name="costThemeDetailOfGroupByPriceTBMMKT[0].unitPrice" class="cssTableTDDetailNumber">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: right;">
                    <input type="text" value="0.00000" id="costThemeDetailOfGroupByPriceTBMMKT[0].total" name="costThemeDetailOfGroupByPriceTBMMKT[0].total" class="cssTableTDDetailNumber">
                </td>
            </tr>
            <tr>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 1px; border-collapse: collapse; text-align: center;">
                    <input type="hidden" value="36C02BF0-F38D-49C6-B8E3-B0D27C7DD1D2" id="costThemeDetailOfGroupByPriceTBMMKT[1].id" name="costThemeDetailOfGroupByPriceTBMMKT[1].id" class="cssTableTDTextCenter">
                    <input type="text" value="" id="costThemeDetailOfGroupByPriceTBMMKT[1].IO" name="costThemeDetailOfGroupByPriceTBMMKT[1].IO" class="cssTableTDTextCenter">
                </td>
                <td style="width: 50%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="" id="costThemeDetailOfGroupByPriceTBMMKT[1].productDetail" name="costThemeDetailOfGroupByPriceTBMMKT[1].productDetail" class="cssTableTDTextLeft">
                </td>
                <td style="width: 5%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="0" id="costThemeDetailOfGroupByPriceTBMMKT[1].unit" name="costThemeDetailOfGroupByPriceTBMMKT[1].unit" class="cssTableTDTextCenter">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="0.00000" id="costThemeDetailOfGroupByPriceTBMMKT[1].unitPrice" name="costThemeDetailOfGroupByPriceTBMMKT[1].unitPrice" class="cssTableTDDetailNumber">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: right;">
                    <input type="text" value="0.00000" id="costThemeDetailOfGroupByPriceTBMMKT[1].total" name="costThemeDetailOfGroupByPriceTBMMKT[1].total" class="cssTableTDDetailNumber">
                </td>
            </tr>
            <tr>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 1px; border-collapse: collapse; text-align: center;">
                    <input type="hidden" value="7CFC1E8D-4D1F-445B-A246-158A68996AB3" id="costThemeDetailOfGroupByPriceTBMMKT[2].id" name="costThemeDetailOfGroupByPriceTBMMKT[2].id" class="cssTableTDTextCenter">
                    <input type="text" value="" id="costThemeDetailOfGroupByPriceTBMMKT[2].IO" name="costThemeDetailOfGroupByPriceTBMMKT[2].IO" class="cssTableTDTextCenter">
                </td>
                <td style="width: 50%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="" id="costThemeDetailOfGroupByPriceTBMMKT[2].productDetail" name="costThemeDetailOfGroupByPriceTBMMKT[2].productDetail" class="cssTableTDTextLeft">
                </td>
                <td style="width: 5%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="0" id="costThemeDetailOfGroupByPriceTBMMKT[2].unit" name="costThemeDetailOfGroupByPriceTBMMKT[2].unit" class="cssTableTDTextCenter">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="0.00000" id="costThemeDetailOfGroupByPriceTBMMKT[2].unitPrice" name="costThemeDetailOfGroupByPriceTBMMKT[2].unitPrice" class="cssTableTDDetailNumber">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: right;">
                    <input type="text" value="0.00000" id="costThemeDetailOfGroupByPriceTBMMKT[2].total" name="costThemeDetailOfGroupByPriceTBMMKT[2].total" class="cssTableTDDetailNumber">
                </td>
            </tr>
            <tr>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 1px; border-collapse: collapse; text-align: center;">
                    <input type="hidden" value="81C625BE-9A1D-40A8-A8AC-089A7CAB1E0F" id="costThemeDetailOfGroupByPriceTBMMKT[3].id" name="costThemeDetailOfGroupByPriceTBMMKT[3].id" class="cssTableTDTextCenter">
                    <input type="text" value="" id="costThemeDetailOfGroupByPriceTBMMKT[3].IO" name="costThemeDetailOfGroupByPriceTBMMKT[3].IO" class="cssTableTDTextCenter">
                </td>
                <td style="width: 50%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="" id="costThemeDetailOfGroupByPriceTBMMKT[3].productDetail" name="costThemeDetailOfGroupByPriceTBMMKT[3].productDetail" class="cssTableTDTextLeft">
                </td>
                <td style="width: 5%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="0" id="costThemeDetailOfGroupByPriceTBMMKT[3].unit" name="costThemeDetailOfGroupByPriceTBMMKT[3].unit" class="cssTableTDTextCenter">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="0.00000" id="costThemeDetailOfGroupByPriceTBMMKT[3].unitPrice" name="costThemeDetailOfGroupByPriceTBMMKT[3].unitPrice" class="cssTableTDDetailNumber">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: right;">
                    <input type="text" value="0.00000" id="costThemeDetailOfGroupByPriceTBMMKT[3].total" name="costThemeDetailOfGroupByPriceTBMMKT[3].total" class="cssTableTDDetailNumber">
                </td>
            </tr>
            <tr>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 1px; border-collapse: collapse; text-align: center;">
                    <input type="hidden" value="A0DC8AF0-6033-4A84-A9AB-880ECD8500ED" id="costThemeDetailOfGroupByPriceTBMMKT[4].id" name="costThemeDetailOfGroupByPriceTBMMKT[4].id" class="cssTableTDTextCenter">
                    <input type="text" value="" id="costThemeDetailOfGroupByPriceTBMMKT[4].IO" name="costThemeDetailOfGroupByPriceTBMMKT[4].IO" class="cssTableTDTextCenter">
                </td>
                <td style="width: 50%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="" id="costThemeDetailOfGroupByPriceTBMMKT[4].productDetail" name="costThemeDetailOfGroupByPriceTBMMKT[4].productDetail" class="cssTableTDTextLeft">
                </td>
                <td style="width: 5%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="0" id="costThemeDetailOfGroupByPriceTBMMKT[4].unit" name="costThemeDetailOfGroupByPriceTBMMKT[4].unit" class="cssTableTDTextCenter">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="0.00000" id="costThemeDetailOfGroupByPriceTBMMKT[4].unitPrice" name="costThemeDetailOfGroupByPriceTBMMKT[4].unitPrice" class="cssTableTDDetailNumber">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: right;">
                    <input type="text" value="0.00000" id="costThemeDetailOfGroupByPriceTBMMKT[4].total" name="costThemeDetailOfGroupByPriceTBMMKT[4].total" class="cssTableTDDetailNumber">
                </td>
            </tr>
            <tr>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 1px; border-collapse: collapse; text-align: center;">
                    <input type="hidden" value="AEC17F73-14DA-4891-A759-23EA76C6101D" id="costThemeDetailOfGroupByPriceTBMMKT[5].id" name="costThemeDetailOfGroupByPriceTBMMKT[5].id" class="cssTableTDTextCenter">
                    <input type="text" value="" id="costThemeDetailOfGroupByPriceTBMMKT[5].IO" name="costThemeDetailOfGroupByPriceTBMMKT[5].IO" class="cssTableTDTextCenter">
                </td>
                <td style="width: 50%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="" id="costThemeDetailOfGroupByPriceTBMMKT[5].productDetail" name="costThemeDetailOfGroupByPriceTBMMKT[5].productDetail" class="cssTableTDTextLeft">
                </td>
                <td style="width: 5%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="0" id="costThemeDetailOfGroupByPriceTBMMKT[5].unit" name="costThemeDetailOfGroupByPriceTBMMKT[5].unit" class="cssTableTDTextCenter">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="0.00000" id="costThemeDetailOfGroupByPriceTBMMKT[5].unitPrice" name="costThemeDetailOfGroupByPriceTBMMKT[5].unitPrice" class="cssTableTDDetailNumber">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: right;">
                    <input type="text" value="0.00000" id="costThemeDetailOfGroupByPriceTBMMKT[5].total" name="costThemeDetailOfGroupByPriceTBMMKT[5].total" class="cssTableTDDetailNumber">
                </td>
            </tr>
            <tr>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 1px; border-collapse: collapse; text-align: center;">
                    <input type="hidden" value="C85DDBF1-C057-4156-8FEF-DC5BB827467D" id="costThemeDetailOfGroupByPriceTBMMKT[6].id" name="costThemeDetailOfGroupByPriceTBMMKT[6].id" class="cssTableTDTextCenter">
                    <input type="text" value="" id="costThemeDetailOfGroupByPriceTBMMKT[6].IO" name="costThemeDetailOfGroupByPriceTBMMKT[6].IO" class="cssTableTDTextCenter">
                </td>
                <td style="width: 50%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="" id="costThemeDetailOfGroupByPriceTBMMKT[6].productDetail" name="costThemeDetailOfGroupByPriceTBMMKT[6].productDetail" class="cssTableTDTextLeft">
                </td>
                <td style="width: 5%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="0" id="costThemeDetailOfGroupByPriceTBMMKT[6].unit" name="costThemeDetailOfGroupByPriceTBMMKT[6].unit" class="cssTableTDTextCenter">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: center;">
                    <input type="text" value="0.00000" id="costThemeDetailOfGroupByPriceTBMMKT[6].unitPrice" name="costThemeDetailOfGroupByPriceTBMMKT[6].unitPrice" class="cssTableTDDetailNumber">
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: right;">
                    <input type="text" value="0.00000" id="costThemeDetailOfGroupByPriceTBMMKT[6].total" name="costThemeDetailOfGroupByPriceTBMMKT[6].total" class="cssTableTDDetailNumber">
                </td>
            </tr>
            <tr>
                <td colspan="4" style="width: 85%; border-style: solid; border-width: 1px 1px 0px 0px; border-collapse: collapse; text-align: right;">TOTAL&nbsp;
                </td>
                <td style="width: 15%; border-style: solid; border-width: 1px 1px 1px 0px; border-collapse: collapse; text-align: right;">0,000.00&nbsp;
                </td>
            </tr>
        </tbody>
    </table>

                </div>
            </td>
        </tr>
        <tr style="border-bottom: none;">
            <td>
                <div style="height: 20px;"></div>
                <div id="div5" runat="server" style="width: 100%; padding-left: 100px;">
                    จึงเรียนมาเพื่อโปรดพิจารณาอนุมัติการดำเนินการข้างต้น
                </div>
            </td>
        </tr>
        <tr>
            <td style="border-style: solid; border-width: 0px 0px 0px 0px; border-collapse: collapse;">
                <div style="height: 5px;"></div>
                <div style="width: 100%; text-align: left; padding-left: 15px;">
                    หมายเหตุ
                    <textarea class="cssTableTDTextLeft" cols="20" id="txtremark" name="activityFormModel.remark" rows="2"></textarea>
                    <br />
                </div>
            </td>
        </tr>

        <tr style="border-bottom: none;">
            <td>&nbsp;<br />
            </td>
        </tr>
    </tbody>
</table>

