﻿var $adminPage = (function () {
    document.getElementById("ddlProductGrp").disabled = false

    'use strict';
   
    return {

        editProduct: function (id, productcode, cateid, groupid, brandid, size, pack, productname) {

            document.getElementById("ddlProductCate").value = cateid;
            document.getElementById("ddlProductGrp").value = groupid;
            document.getElementById("ddlProductBrand").value = brandid;
            $("#txtProductCode").val(productcode);
            $("#txtSize").val(size);
            $("#txtPack").val(pack);
            $("#txtProductName").val(productname);

        },

        onchangeCate: function () {

            $("#ddlProductBrand").val('');
            $("#ddlProductfilter").val('');
            $("#ddlProductSize").val('');

            //document.getElementById("ddlProductBrand").disabled = true;
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
                        //document.getElementById("ddlProductGrp").disabled = false;
                    }
                    else {
                        //document.getElementById("ddlProductGrp").disabled = true;
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
                        document.getElementById("ddlProductBrand").disabled = false;
                    }
                    else {
                        document.getElementById("ddlProductBrand").disabled = true;
                    }
                }
            });
        },



        checkProduct: function () {

            $.ajax({
                url: $adminPage.urlCheckProduct,
                data: {
                    p_productCode: $("#txtProductCode").val(),
                },
                dataType: "json",
                type: 'POST',
                success: function (response) {
                    if (response.Success == true) {
                        $adminPage.callInsertProduct("คุณต้องการแก้ไขสินค้า ใช่ หรือ ไม่!", "update");
                    }
                    else {
                        $adminPage.callInsertProduct("คุณต้องการเพิ่ม ใช่ หรือ ไม่!", "insert");
                    }
                }
            });

        },

        callInsertProduct: function (msg, type) {
            bootbox.confirm({
                message: msg,
                buttons: {
                    confirm: {
                        label: 'Yes',
                        className: 'btn-success'
                    },
                    cancel: {
                        label: 'No',
                        className: 'btn-danger'
                    }
                },
                callback: function (result) {
                    if (result == true) {
                        console.log(result);
                        bootbox.dialog({ message: '<div class="text-center"><i class="fa fa-spin fa-spinner"></i> Loading...</div>' })

                        if (type == "insert") {
                            $.ajax({
                                url: $adminPage.urlAddProduct,
                                data: {
                                    p_cateId: $("#ddlProductCate").val(),
                                    p_groupId: $("#ddlProductGrp").val(),
                                    p_brandId: $("#ddlProductBrand").val(),
                                    p_size: $("#txtSize").val(),
                                    p_pack: $("#txtPack").val(),
                                    p_productName: $("#txtProductName").val(),
                                    p_productCode: $("#txtProductCode").val(),
                                },
                                dataType: "json",
                                type: 'POST',
                                success: function (response) {

                                }
                            });
                        }
                        else {
                            $.ajax({
                                url: $adminPage.urlUpdateProduct,
                                data: {
                                    p_cateId: $("#ddlProductCate").val(),
                                    p_groupId: $("#ddlProductGrp").val(),
                                    p_brandId: $("#ddlProductBrand").val(),
                                    p_size: $("#txtSize").val(),
                                    p_pack: $("#txtPack").val(),
                                    p_productName: $("#txtProductName").val(),
                                    p_productCode: $("#txtProductCode").val(),
                                },
                                dataType: "json",
                                type: 'POST',
                                success: function (response) {

                                }
                            });
                        }
                    }
                }

            });
        },



        onchangePrice: function (cusId, rowIndex) {

            var productId = $('#hdProductCode').val();
            var p_normalCost = $('#normalCost_' + rowIndex).val();
            var p_wholeSalesPrice = $('#wholeSalesPrice_' + rowIndex).val();
            var p_discount1 = $('#discount1_' + rowIndex).val();
            var p_discount2 = $('#discount2_' + rowIndex).val();
            var p_discount3 = $('#discount3_' + rowIndex).val();
            var p_saleNormal = $('#saleNormal_' + rowIndex).val();
            $.ajax({
                type: 'POST',
                url: $adminPage.urlOnchangePrice,
                data: {
                    customerId: cusId,
                    productCode: productId,
                    normalCost: p_normalCost,
                    wholeSalesPrice: p_wholeSalesPrice,
                    discount1: p_discount1,
                    discount2: p_discount2,
                    discount3: p_discount3,
                    saleNormal: p_saleNormal
                }
            }).done(function (response) {
                //CallChangefunc();
            });

        },

    }
})();

