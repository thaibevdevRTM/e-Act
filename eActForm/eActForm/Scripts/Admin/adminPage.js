var $adminPage = (function () {
    document.getElementById("ddlProductGrp").disabled = false
    document.getElementById("ddlProductBrand").disabled = true;
    'use strict';
   
    return {

        editProduct: function (id,productcode, cateid, groupid,brandid,size,pack,productname,smell) {
            console.log(brandid);

            document.getElementById("ddlProductCate").value = cateid;
            document.getElementById("ddlProductGrp").value = groupid;
            document.getElementById("ddlProductBrand").value = brandid;
            $("#txtProductCode").val(productcode);
            $("#txtSize").val(size);
            $("#txtPack").val(pack);
            $("#txtProductName").val(productname);
            $("#txtsmell").val(smell);
            
            
        },

        onchangeCate: function () {

            $("#ddlProductBrand").val('');
            $("#ddlProductfilter").val('');
            $("#ddlProductSize").val('');

            document.getElementById("ddlProductBrand").disabled = true;
            var ddlProductGrp = $("#ddlProductGrp");

            $.ajax({
                url: $adminPage.urlGetGroup,
                data: {
                    cateId: $("#ddlProductCate").val(),
                },
                dataType: "json",
                type: 'POST',
                success: function (response) {

                    if (response.Data.productGroup.length > 0) {
                        $("#ddlProductGrp option[value !='']").remove();
                        $.each(response.Data.productGroup, function () {
                            ddlProductGrp.append($("<option></option>").val(this['id']).html(this['groupName']));
                        });
                        document.getElementById("ddlProductGrp").disabled = false;
                    }
                    else {
                        document.getElementById("ddlProductGrp").disabled = true;
                    }

                }
            });
        },


        onchangeGroup: function () {
  
             var ddlProductBrand = $("#ddlProductBrand");

             $.ajax({
                 url: $adminPage.urlGetBrand,
                 data: {
                     p_groupId: $("#ddlProductGrp").val(),
                 },
                 dataType: "json",
                 type: 'POST',
                 success: function (response) {
                     if (response.Data.getProductname.length > 0) {
                         $("#ddlProductBrand option[value !='']").remove();
                         $.each(response.Data.getProductname, function () {
                             ddlProductBrand.append($("<option></option>").val(this['Value']).html(this['Text']));
                         });
                         //document.getElementById("ddlProductBrand").disabled = false;
                     }
                     else {
                         //document.getElementById("ddlProductBrand").disabled = true;
                     }
                 }
             });
         }



    }
})();

