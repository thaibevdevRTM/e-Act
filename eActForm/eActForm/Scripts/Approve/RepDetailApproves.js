
function callPreviewRepDetailApprove(obj) {
    $.ajax({
        type: 'POST',
        url: '@Url.Action("repPreviewListView", "actFormRepDetail")',
        data: {
            actId: obj
        }
    }).done(function (htmlResponse) {
        $.ajax({
            type: 'POST',
            dataType: "html",
            url: '@Url.Action("repDetailGenPDF", "actFormRepDetail")',
            data: {
                gridHtml: htmlResponse
                , actId: '@actId'
            }
        }).done(function (htmlResponse) {
            $("#WaitDialog").hide();
            bootbox.confirm({
                message: "ดำเนินการเรียบร้อยครับ",
                buttons: {
                    confirm: {
                        label: 'OK',
                        className: 'btn-success'
                    }
                },
                callback: function (result) {
                    var url = '@Url.Action("Index", "ApproveListsRepDetail")';
                    window.location.href = url;
                }

            });
        });
    });
}