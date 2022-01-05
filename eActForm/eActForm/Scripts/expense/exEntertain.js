var $exEntertain = (function () {
    document.getElementById("txtExMonth").addEventListener("change", getLimit);
    document.getElementById("txtEmpId").addEventListener("change", getLimit);
    

    function getLimit() {
        console.log("filejava");
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
       

    }
})();
