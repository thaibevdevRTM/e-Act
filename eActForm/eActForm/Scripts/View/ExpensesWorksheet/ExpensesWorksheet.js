

let diffDays;

$(document).ready(function () {

    if ($("#ddlCountry option:selected").val() != '' && $("#ProductDetail_0").val() != '0') {
        //ให้วงเงิน สำหรับเดินทางต่างประเทศ

        console.log($("#ProductDetail_0").val() + 'callllll')

        setTimeout(function () {
            callMultiAllowance();
           
        }, 2000);

        setTimeout(function () {
            countDiffDate();
      
        }, 2500);
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
            console.log(parseFloat(response.Data.rate).toFixed(2) + 'rate')
            var div = document.getElementById('divRemarkRate');
            div.innerHTML = '*อ้างอิงจาก วันที่ ' + response.Data.txtDate;
        }
    });
}

$('#ddlCountry').change(function () {
    onChangeCountry();
});

function onChangeCountry() {
    setDataOrClearAllow("0");
    if ($("#activityFormModel_str_costPeriodSt").val() == '') {
        bootbox.alert('กรุณาเลือก วันที่เดินทาง')
        $("#ddlCountry").val('')
    }

    if ($("#ddlCountry option:selected").val() != '') {
        countDiffDate();
    } else {
        document.getElementById("chkAllowanceTBM").disabled = true;
        document.getElementById('divShowPerAllow').innerHTML = '';
        $("#ProductDetail_0").val(globAllowance);
        $("#txtUnitPrice_0").val(globAllowance);
        blurTxt(0);
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
    callFucResetValue();
})

$("#activityFormModel_str_costPeriodEnd").on("change.datetimepicker", ({ date, oldDate }) => {
    callFucResetValue();
})

function callFucResetValue() {
    getExchangeRate();
    setDataOrClearAllow('0');
    countDiffDate();
    onChangeCountry();
   
    blurTxt(0);
}


function countDiffDate() {
    diffDays = 0;
    var today = $("#activityFormModel_str_costPeriodEnd").val()
    today = new Date(today.split('/')[2], today.split('/')[1] - 1, today.split('/')[0]);
    var date2 = $("#activityFormModel_str_costPeriodSt").val()
    date2 = new Date(date2.split('/')[2], date2.split('/')[1] - 1, date2.split('/')[0]);

    var timeDiff = Math.abs(date2.getTime() - today.getTime());
    diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
    diffDays = diffDays + 1;
    console.log(diffDays)
    $("#txtUnit_0").val(diffDays);


    if ($("#ddlCountry option:selected").val() != '') {
        if (diffDays > 30) {

            document.getElementById('divShowPerAllow').innerHTML = "( คิดอัตราแบบเหมาจ่าย )";
            document.getElementById("chkAllowanceTBM").disabled = true;

            //ดึงข้อมูลเบี้ยเลี้ยงแบบ เหมา
            getAmountAllowance('month')
        } else {
            document.getElementById("chkAllowanceTBM").disabled = false;
            getAmountAllowance('day');

        }
    }

}



function getAmountAllowance(typeDays) {
    console.log($('#lblLvl').text() + 'lvl getAmountAllowance')

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

            let getAllowanceAmount = parseFloat(response.Data.Result.max_amount)


            console.log(getAllowanceAmount + 'max_amount')
            console.log($("#ProductDetail_0").val() + 'dayss')
            if (getAllowanceAmount > 0) {
                let calAmountAllow = parseFloat($("#txtAmountReceived").val()) * getAllowanceAmount

                console.log(parseFloat($("#txtAmountReceived").val()) + 'txtAmountReceived')

                if (typeDays == 'day') {


                    $("#ProductDetail_0").val(calAmountAllow.toFixed(2));
                    $("#txtUnitPrice_0").val(calAmountAllow.toFixed(2));

                    console.log($("#ProductDetail_0").val() + 'day')

                    let calTotalAllowanceDay = calAmountAllow * diffDays;
                    $("#txtTotal_0").val(calTotalAllowanceDay.toFixed(2));
                    $("#hdTotal_0").val(calTotalAllowanceDay);

                } else {
                    $("#ProductDetail_0").val(0);

                    calAvgAllowanceMon = calAmountAllow / 30;
                    calDiffOverDay = diffDays - 30
                    calSumOverAllowance = calAmountAllow + (calAvgAllowanceMon * calDiffOverDay);

                    $("#ProductDetail_0").val(calAvgAllowanceMon.toFixed(2));
                    $("#txtUnitPrice_0").val(calSumOverAllowance.toFixed(2));
                    $("#txtTotal_0").val(calSumOverAllowance.toFixed(2));
                    $("#hdTotal_0").val(calSumOverAllowance);
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

    console.log($("#ProductDetail_0").val() + 'sumrow')

    let getTotalCashEmp = parseFloat($("#ProductDetail_0").val()) * rowCount;
    getTotalCashEmp = getTotalCashEmp * (sumTotal / rowCount) / 100;

    document.getElementById('totalAllowance').innerHTML = getTotalCashEmp.toFixed(2);
    $("#totalAllowance").val(getTotalCashEmp);
    getPerCentAllowance = sumTotal / rowCount;
    document.getElementById('totalAllowancePerCent').innerHTML = getPerCentAllowance.toFixed(2);

    sumDataAllow();
}

function callMultiAllowance() {

    
    var rowCount = parseInt($('#tabelAllowance tr.tagCountRowinTable').length);
    console.log(rowCount + ' row callMultiAllowance')
    
    if (rowCount > 0) {
        sumRow();
        setDataOrClearAllow("1");

    } else {
        setDataOrClearAllow("0");
    }
}

function validateSubmit() {
    var requiredSub = "<b>กรุณาระบุ/Please input data :<br></b>";
    var validateSub = true;

    var totalAllowance = parseFloat($("#totalAllowance").val().replace(",", ""))
    console.log(totalAllowance)
    if (totalAllowance <= 0.00 || isNaN(totalAllowance)) {
        requiredSub += "&emsp;เลือก สัดส่วนเบี้ยเลี้ยง*<br>"; validateSub = false;
    }

    if (validateSub == true) {
        sumDataAllow();

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

        document.getElementById("chkAllowanceTBM").checked = false;
        $('#tabelAllowance tr.tagCountRowinTable input[type="checkbox"]').each(function () {
            $(this).prop('checked', false);
        });
        document.getElementById("txtUnitPrice_0").readOnly = false;
        document.getElementById("txtUnit_0").readOnly = false;
        $("#txtUnitPrice_0").val($("#ProductDetail_0").val());
        $("#txtUnit_0").val("0");
        $("#txtTotal_0").val("0.00");
        
        sumRow();
        countDiffDate();
        blurTxt(0);
        document.getElementById('divShowPerAllow').innerHTML = "";
    }

}

function sumDataAllow() {

    $("#txtUnit_0").val(daySelect);
    $("#txtUnitPrice_0").val(parseFloat($("#ProductDetail_0").val()).toFixed(2));


    var v_total = parseFloat($("#totalAllowance").val().replace(",", ""));
    $("#txtTotal_0").val(v_total.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
    $("#hdTotal_0").val(v_total.toFixed(2));


    var rowCount = parseInt($('#tabelAllowance tr.tagCountRowinTable').length);
    if (rowCount > 0) {
        document.getElementById('divShowPerAllow').innerHTML = "<a href=\"#\" onclick=\" $('#divAllowanceDetail').modal('show');\" style=\"width: 100 %\">(" + getPerCentAllowance.toFixed(2) + "%  << คลิกเพื่อดูรายละเอียด) </a>";
    }

}