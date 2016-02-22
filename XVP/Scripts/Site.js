jQuery(function ($) {

    $(document).ajaxSend(function(event, request, settings) {
        $('#loading-indicator').show();
        $(".modal-backdrop").show();
    });

    $(document).ajaxComplete(function(event, request, settings) {
        $('#loading-indicator').hide();
        $(".modal-backdrop").hide();
    });

});
  


       
