$(document).ready(function () {


    $("#map-canvas").on("click", ".addButton", function () {

        var itemId = $(this).attr('id');

        $.ajax({
        type: "GET",
        url: '/search/getmediaitem?itemId=' + itemId,
        contentType: "application/json; charset=utf-8",
        data: { a: "testing" },
        dataType: "json",
        success: successFunc,
        error: errorFunc
        });

        function successFunc(data, status) {
            app.marketHome.getViewModel().addMediaItemJson(data);
        }

        function errorFunc() {
            alert('error');
        }

        disableAddToCart(this);
    });
});

    
