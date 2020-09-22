var $exEntertain = (function () {
    document.getElementById("txtExMonth").addEventListener("change", getLimit);
    document.getElementById("txtEmpId").addEventListener("change", getLimit);
    

    function getLimit() {
        $exEntertain.clearValue();
        getLimitExpense();
    }



    return {


        clearValue: function () {
            document.getElementById('lblLimitEntertain').innerHTML = "0.00";
            document.getElementById('lblLimitBypass').innerHTML = "0.00";
            document.getElementById('lblLimitCarWash').innerHTML = "0.00";
            document.getElementById('lblLimitPhone').innerHTML = "0.00";

        },

       
        validateData: function (i) {
            var required = "กรุณา ระบุ :<br>";
            var validate = true;

            var rowIndex = $(i).closest('tr').index();
            if ($("#txtEmpId").val().length != 8) {
                required += "ระบุ รหัสพนักงาน*<br>"; validate = false;
            }
            if (validate == true) {
      
                $("#activityOfEstimateList_" + rowIndex + "__total").val("0")
                getTotal1();
            }
            else {
                document.getElementById("activityOfEstimateList_" + rowIndex + "__productId").selectedIndex = 0;
                bootbox.alert(required)
            }
        },

        //getDetailList: function (i) {
        //    console.log(i.id);
        //    $("#" + i.id).autocomplete({
        //        source: function (request, response) {
        //            $.ajax({
        //                url: "eAct/getOtherMasterByType",
        //                type: "POST",
        //                dataType: "json",
        //                data: {
        //                    type: 'CashLimitType',
        //                    subType: '',
        //                    text: $("#" + i.id).val(),
        //                },
        //                success: function (data) {
        //                    response($.map(data, function (item) {
        //                        return { label: item.displayVal, value: item.displayVal, id: item.id };
        //                    }))

        //                }
        //            })
        //        },
        //        minLength: 0,
        //        messages: {
        //            noResults: '',
        //            results: function (resultsCount) {
        //            }
        //        }, select: function (event, ui) {
        //            $("#" + i.id).val(ui.item.id)
        //        }
        //    }).focus(function () {
        //        $(this).data("uiAutocomplete").search($(this).val());
        //    });
        //},

       

    }
})();
