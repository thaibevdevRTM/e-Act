var $adminPage = (function () {
    document.getElementById("ddlProductGrp").disabled = false

    'use strict';
   
    return {

        editProduct: function (id, productcode, cateid, groupid, brandid, size, pack, productname, smellid,unit) {

            $.ajax({
                url: $adminPage.urlgetProductSmell,
                data: {
                    productGroupId: groupid,
                },
                dataType: "json",
                type: 'POST',
                success: function (response) {
                    if (response.Data.length > 0) {
                        $("#ddlProductSmell option[value !='']").remove();
                        $.each(response.Data, function () {
                            $("#ddlProductSmell").append($("<option></option>").val(this['id']).html(this['nameTH']));
                            document.getElementById("ddlProductSmell").disabled = false;
                            document.getElementById("ddlProductSmell").value = smellid;
                        });
                    }
                    else {
                        $("#ddlProductSmell option[value !='']").remove();
                        document.getElementById("ddlProductSmell").disabled = true;
                        document.getElementById("ddlProductSmell").value = smellid;
                    }
                }
            });


            $.ajax({
                url: $adminPage.urlGetGroup,
                data: {
                    cateId: cateid,
                },
                dataType: "json",
                type: 'POST',
                success: function (response) {

                    if (response.Data.productGroup.length > 0) {
                        $("#ddlProductGrp option[value !='']").remove();
                        $.each(response.Data.productGroup, function () {
                            $("#ddlProductGrp").append($("<option></option>").val(this['id']).html(this['groupName']));
                            document.getElementById("ddlProductGrp").value = groupid;
                        });
                    }
                    else {
                        $("#ddlProductGrp option[value !='']").remove();
                        document.getElementById("ddlProductGrp").value = groupid;
                    }
                  

                }
            });
            
            document.getElementById("ddlProductCate").value = cateid;
            document.getElementById("ddlProductBrand").value = brandid;
            $("#txtProductCode").val(productcode);
            $("#txtSize").val(size);
            $("#txtUnit").val(unit);
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


        onDelProduct: function (productId) {
            $.ajax({
                url: $adminPage.urlDelProduct ,
                data: {
                    productId: productId,
                },
                dataType: "json",
                type: 'POST',
                success: function (response) {
                    window.location.href = $adminPage.urlIndexAdmin;
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

            $.ajax({
                url: $adminPage.urlgetProductSmell,
                data: {
                    productGroupId: $("#ddlProductGrp").val(),
                },
                dataType: "json",
                type: 'POST',
                success: function (response) {
                    if (response.Data.length > 0) {
                        $("#ddlProductSmell option[value !='']").remove();
                        $.each(response.Data, function () {
                            $("#ddlProductSmell").append($("<option></option>").val(this['id']).html(this['nameTH']));
                            document.getElementById("ddlProductSmell").disabled = false;
                        });
                    }
                    else {
                        $("#ddlProductSmell option[value !='']").remove();
                        document.getElementById("ddlProductSmell").disabled = true;
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
                        console.log($("#ddlProductSmell").val())
                        $adminPage.callInsertOrUpdateProduct("คุณต้องการแก้ไขสินค้า ใช่ หรือ ไม่!", "update");

                    }
                    else {
                        $adminPage.callInsertOrUpdateProduct("คุณต้องการเพิ่ม ใช่ หรือ ไม่!", "insert");
                    }
                }
            });

        },

        callInsertOrUpdateProduct: function (msg, type) {
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
                                    cateId: $("#ddlProductCate").val(),
                                    groupId: $("#ddlProductGrp").val(),
                                    brandId: $("#ddlProductBrand").val(),
                                    size: $("#txtSize").val(),
                                    pack: $("#txtPack").val(),
                                    unit: $("#txtUnit").val(),
                                    productName: $("#txtProductName").val(),
                                    productCode: $("#txtProductCode").val(),
                                    smellId: $("#ddlProductSmell").val(),
                                },
                                dataType: "json",
                                type: 'POST',
                                success: function (response) {
                                    window.location.href = $adminPage.urlIndexAdmin;
                                }
                            });
                        }
                        else {
                            $.ajax({
                                url: $adminPage.urlUpdateProduct,
                                data: {
                                    cateId: $("#ddlProductCate").val(),
                                    groupId: $("#ddlProductGrp").val(),
                                    brandId: $("#ddlProductBrand").val(),
                                    size: $("#txtSize").val(),
                                    pack: $("#txtPack").val(),
                                    unit: $("#txtUnit").val(),
                                    productName: $("#txtProductName").val(),
                                    productCode: $("#txtProductCode").val(),
                                    smellId: $("#ddlProductSmell").val(),
                                },
                                dataType: "json",
                                type: 'POST',
                                success: function (response) {
                                    window.location.href = $adminPage.urlIndexAdmin;
                                }
                            });
                        }
                    }
                }

            });
        },
      
        setComma: function (price) {

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
                    normalCost: p_normalCost.replace(",", ""),
                    wholeSalesPrice: p_wholeSalesPrice.replace(",", ""),
                    discount1: p_discount1.replace(",", ""),
                    discount2: p_discount2.replace(",", ""),
                    discount3: p_discount3.replace(",", ""),
                    saleNormal: p_saleNormal.replace(",", ""),
                }
            }).done(function (response) {
                //CallChangefunc();
            });

        },

    }
})();

