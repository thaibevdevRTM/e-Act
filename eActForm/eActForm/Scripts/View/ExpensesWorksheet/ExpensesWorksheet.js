

let diffDays;
let chkSubmit = false;

$(document).ready(function () {
    document.getElementById("txtAmountReceived").readOnly = true;

    if ($("#ddlCountry option:selected").val() != '' && $("#ProductDetail_0").val() != '0') {
        setTimeout(function () {
            callMultiAllowance();
        }, 2000);
    }
    else {
        document.getElementById("chkAllowanceTBM").disabled = true;
    }


});

function getExchangeRate() {

    $.ajax({
        type: 'POST',
        url: urlExchangeRate,
        data: {
            st_date: $("#activityFormModel_str_costPeriodSt").val(),
        }
    }).done(function (response) {
        if (response.Data != null) {
            $("#txtAmountReceived").val(parseFloat(response.Data.rate).toFixed(2))
            var div = document.getElementById('divRemarkRate');
            div.innerHTML = '*อ้างอิงจาก วันที่ ' + response.Data.txtDate;
        }
    });
}

$('#ddlCountry').change(function () {
    onChangeCountry();

});

$('#ddlTravelling').change(function () {


    let getTxtselect = $("#ddlTravelling option:selected").text();
    if (getTxtselect.includes('Domestic') || getTxtselect == '') {
        $('#ddlCountry').selectpicker('val', '')
        document.getElementById("txtAmountReceived").readOnly = true;
        document.getElementById("ddlCountry").disabled = true;
    }
    else {
        document.getElementById("txtAmountReceived").readOnly = false;
        document.getElementById("ddlCountry").disabled = false;
        getMasterCoutry();
    }
    onChangeCountry();
});



async function onChangeCountry() {
    await setDataOrClearAllow("0");
    if ($("#ddlCountry option:selected").val() != '') {
       await callFucResetValue();

    } else {

        document.getElementById("chkAllowanceTBM").disabled = true;
        document.getElementById('divShowPerAllow').innerHTML = '';
        $("#ProductDetail_0").val(globAllowance);
        $("#txtUnitPrice_0").val(globAllowance);
        $("#ProductDetail_1").val(perDayTH);
        $("#txtUnitPrice_1").val(perDayTH);
        await callFucResetValue();
        blurTxt(0);
        blurTxt(1);
    }

}


$('#ddlCountry').selectpicker({
    liveSearch: true,
    showSubtext: true
});



$('.clsdatepicker').datetimepicker({
    format: 'dd/mm/yyyy',
    weekStart: 1,
    todayBtn: true,
    autoclose: true,
    todayHighlight: true,
    startView: 2,
    minView: 2,
    forceParse: 0
});

$("#activityFormModel_str_costPeriodSt").on("change.datetimepicker", ({ date, oldDate }) => {
    onChangeCountry();
})

$("#activityFormModel_str_costPeriodEnd").on("change.datetimepicker", ({ date, oldDate }) => {
    onChangeCountry();
})

async function callFucResetValue() {

    if ($("#ddlCountry option:selected").val() != '') {
        await getExchangeRate();
        $("#txtUnitPrice_1").val(0.00);
    }
    else {
        $("#txtAmountReceived").val('');
    }


    //await setDataOrClearAllow('0');
    await countDiffDate();

    //await blurTxt(1);

    setTimeout(function () {
         sumall();
         getValues();
    }, 2000)
    

    

}


function countDiffDate() {
    diffDays = 0;

    var start = $('#activityFormModel_str_costPeriodSt').val();
    var end = $('#activityFormModel_str_costPeriodEnd').val();

    if (start != '' && end != '') {

        var fromTime = new Date(convertFormatDateTimetoEn(start));
        //console.log(fromTime + 'st')

        var toTime = new Date(convertFormatDateTimetoEn(end));
        //console.log(toTime + 'end')

        var diff = (toTime - fromTime) / 1000;
        diff = Math.abs(Math.floor(diff));

        var days = Math.floor(diff / (24 * 60 * 60));
        var leftSec = diff - days * 24 * 60 * 60;
        console.log(days + ' days')
        var hrs = Math.floor(leftSec / (60 * 60));
        var leftSec = leftSec - hrs * 60 * 60;

        //console.log(days + ' days')
        //console.log(hrs + ' hrs')

        if ($("#ddlCountry option:selected").val() == '') {
            if (hrs >= 6 && hrs < 12) {
                days = days + 0.5
            }
            else if (hrs >= 12) {
                days = days + 1
            }
        } else {
            days = days + 1
        }
        
        $("#txtUnit_0").val(days);
        console.log(days + 'total days')
        diffDays = days;

        if ($("#ddlCountry option:selected").val() != '') {

            if (diffDays > 30) {

                document.getElementById('divShowPerAllow').innerHTML = "( คิดอัตราแบบเหมาจ่าย )";
                document.getElementById("chkAllowanceTBM").disabled = true;

                //ดึงข้อมูลเบี้ยเลี้ยงแบบ เหมา
                getAmountAllowance('month')
            } else {

                document.getElementById("chkAllowanceTBM").disabled = false;

                getAmountAllowance('day');
                document.getElementById("txtUnitPrice_0").readOnly = true;
                document.getElementById("txtUnit_0").readOnly = true;
            }

        }
    }
}

function convertFormatDateTimetoEn(p_date) {

    var splitGetTime = p_date.split(' ')[1];
    var arrTime = splitGetTime.split(':')
    var gethour = parseInt(arrTime[0])
    var getmin = parseInt(arrTime[1])

    var splitGetDate = p_date.split(' ')[0]
    //console.log(splitGetTime + ' time')

    var result = new Date(splitGetDate.split('/')[2], splitGetDate.split('/')[1] - 1, splitGetDate.split('/')[0], gethour, getmin);

    return result;

     //var endDate = $("#activityFormModel_str_costPeriodEnd").val()
        //endDate = new Date(endDate.split('/')[2], endDate.split('/')[1] - 1, endDate.split('/')[0]);
        //var stDate = $("#activityFormModel_str_costPeriodSt").val()
        //stDate = new Date(stDate.split('/')[2], stDate.split('/')[1] - 1, stDate.split('/')[0]);

        //var timeDiff = Math.abs(stDate.getTime() - endDate.getTime());
        //diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
        //diffDays = diffDays + 1;

}

function getAmountAllowance(typeDays) {
    $.ajax({
        type: 'POST',
        url: urlAllowanceOverDays,
        data: {
            countryId: $("#ddlCountry option:selected").val(),
            lvl: $('#lblLvl').text(),
            typeDay: typeDays
        }
    }).done(function (response) {
        if (response.Data.Result != null) {

            //console.log($("#txtAmountReceived").val() + 'rate')
            //console.log(diffDays + ' dayss' )
            let getRate = parseFloat($("#txtAmountReceived").val());

            let getAllowanceAmount = parseFloat(response.Data.Result.max_amount)

            let exChangePerDayUs = getRate * perDayUs

            $("#ProductDetail_1").val(exChangePerDayUs.toFixed(2));
            sumHotelOV();
           // $("#txtUnitPrice_1").val(exChangePerDayUs.toFixed(2));

            if (getAllowanceAmount > 0) {
                let calAmountAllow = getRate * getAllowanceAmount

                //console.log(getRate + ' getRate')
                //console.log(getAllowanceAmount + ' getAllowanceAmount')

                if (typeDays == 'day') {

                    $("#ProductDetail_0").val(calAmountAllow.toFixed(2));
                    $("#txtUnitPrice_0").val(calAmountAllow.toFixed(2));

                    let calTotalAllowanceDay = calAmountAllow * diffDays;
                    $("#txtTotal_0").val(calTotalAllowanceDay.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
                    $("#hdTotal_0").val(calTotalAllowanceDay);
               
                    //console.log(calTotalAllowanceDay + ' p_day')
                } else {
                    $("#ProductDetail_0").val(0);

                    calAvgAllowanceMon = calAmountAllow / 30;
                    calDiffOverDay = diffDays - 30
                    calSumOverAllowance = calAmountAllow + (calAvgAllowanceMon * calDiffOverDay);

                    $("#ProductDetail_0").val(calAvgAllowanceMon.toFixed(2));
                    $("#txtUnitPrice_0").val(0);
                    $("#txtTotal_0").val(calSumOverAllowance.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
                    $("#hdTotal_0").val(calSumOverAllowance);

                    //console.log(calSumOverAllowance + ' p_month')
                }

            
            }

        }
        
    });

}

function sumRow() {

    let sumTotal = 0.00;
    let countRowChk = 0;

    var rowCount = parseInt($('#tabelAllowance tr.tagCountRowinTable').length);
    var grid = document.getElementById("tabelAllowance");
    var checkBoxes = grid.getElementsByTagName("INPUT");

    for (y = 0; y < rowCount; y++) {
        let result = 0;
        let checkRow = false;

        for (var i = 0; i < checkBoxes.length; i++) {

            if (checkBoxes[i].checked) {

                let getRowofCheck = checkBoxes[i].id.replace(/[^0-9]/g, '');
                if (getRowofCheck == y) {

                    result = result + 25;
                    checkRow = true;
                }

            }
        }

        if (checkRow == true) {
            countRowChk = countRowChk + 1;
        }

        document.getElementById('lblSumAllowance_' + y).innerHTML = result;
        sumTotal = sumTotal + result;
    }

    daySelect = countRowChk;

    let getTotalCashEmp = parseFloat($("#ProductDetail_0").val()) * rowCount;
    getTotalCashEmp = getTotalCashEmp * (sumTotal / rowCount) / 100;

    document.getElementById('totalAllowance').innerHTML = getTotalCashEmp.toFixed(2);
    $("#totalAllowance").val(getTotalCashEmp);
    getPerCentAllowance = sumTotal / rowCount;
    document.getElementById('totalAllowancePerCent').innerHTML = getPerCentAllowance.toFixed(2);
    $("#tB_Act_ActivityForm_DetailOther_amountCumulative").val(getPerCentAllowance.toFixed(2))

    sumDataAllow();
}

function checkCancel() {

    if (chkSubmit == false) {
        setDataOrClearAllow("0");
    }
}



function callMultiAllowance() {
    var rowCount = parseInt($('#tabelAllowance tr.tagCountRowinTable').length);
    //console.log(rowCount + 'count row')
    if (rowCount > 0) {
        chkSubmit = true;
        setDataOrClearAllow("1");
        countDiffDate();
        setTimeout(function () {
            sumRow();
        }, 1000);
        

    } else {
        setDataOrClearAllow("0");
        countDiffDate();

        setTimeout(function () {
            sumall();
            getValues();
        }, 2000);
    }
}

function validateSubmit() {
    var requiredSub = "<b>กรุณาระบุ/Please input data :<br></b>";
    var validateSub = true;

    var totalAllowance = parseFloat($("#totalAllowance").val().replace(",", ""))
    if (totalAllowance <= 0.00 || isNaN(totalAllowance)) {
        requiredSub += "&emsp;เลือก สัดส่วนเบี้ยเลี้ยง*<br>"; validateSub = false;
    }

    if (validateSub == true) {
        sumDataAllow();
        chkSubmit = true;
        $('#divAllowanceDetail').modal('hide');
        
    }
    else {
        bootbox.alert(requiredSub)
    }
}

function setDataOrClearAllow(val) {
    if (val == "1") {
        document.getElementById("txtUnitPrice_0").readOnly = true;
        document.getElementById("txtUnit_0").readOnly = true;
        document.getElementById("chkAllowanceTBM").checked = true;

    } else {
        chkSubmit = false;
        document.getElementById("chkAllowanceTBM").checked = false;
        $('#tabelAllowance tr.tagCountRowinTable input[type="checkbox"]').each(function () {
            $(this).prop('checked', false);
        });
        document.getElementById("txtUnitPrice_0").readOnly = false;
        document.getElementById("txtUnit_0").readOnly = false;
        $("#txtUnitPrice_0").val($("#ProductDetail_0").val());
        $("#txtUnit_0").val("0");
        $("#txtTotal_0").val("0.00");
        $("#hdTotal_0").val(0.00);
        $("#tB_Act_ActivityForm_DetailOther_amountCumulative").val(100)
        document.getElementById('divShowPerAllow').innerHTML = "";
        sumall();
        getValues();
    }

}

function sumDataAllow() {

    $("#txtUnit_0").val(daySelect);
    $("#txtUnitPrice_0").val(parseFloat($("#ProductDetail_0").val()).toFixed(2));
    $("#txtUnitPrice_0").val(0);

    var v_total = parseFloat($("#totalAllowance").val().replace(",", ""));
    $("#txtTotal_0").val(v_total.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
    $("#hdTotal_0").val(v_total.toFixed(2));


    var rowCount = parseInt($('#tabelAllowance tr.tagCountRowinTable').length);
    if (rowCount > 0) {
        document.getElementById('divShowPerAllow').innerHTML = "<a href=\"#\" onclick=\" $('#divAllowanceDetail').modal('show');\" style=\"width: 100 %\">(" + getPerCentAllowance.toFixed(2) + "%  << คลิกเพื่อดูรายละเอียด) </a>";
    }
    sumall();
    getValues();
}

