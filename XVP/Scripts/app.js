var app = {

};

app.marketHome = {};


(function() {
    var homeModel = new Campaign();

    app.marketHome.getViewModel = function() {
        return homeModel;
    }

    app.marketHome.init = function () {
        app.marketHome.viewModel = homeModel;
        ko.bindingHandlers.datepicker = {
            init: function (element, valueAccessor, allBindingsAccessor) {
                var $el = $(element);

                //initialize datepicker with some optional options
                var options = allBindingsAccessor().datepickerOptions || {};
                $el.datepicker(options);

                //handle the field changing
                ko.utils.registerEventHandler(element, "change", function () {
                    var observable = valueAccessor();
                    observable($el.datepicker("getDate"));
                });

                //handle disposal (if KO removes by the template binding)
                ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                    $el.datepicker("destroy");
                });

            },
            update: function (element, valueAccessor) {
                var value = ko.utils.unwrapObservable(valueAccessor()),
                    $el = $(element),
                    current = $el.datepicker("getDate");

                if (value - current !== 0) {
                    $el.datepicker("setDate", value);
                }
            }
        };

       

        ko.applyBindings(homeModel);

        $('.datepicker').datepicker();
    };

})();