function Campaign() {
    var self = this;
    self.items = ko.observableArray();
    self.campaignId = ko.observable(0);
    self.campaignName = ko.observable("Default Campaign");
    self.campaignStart = ko.observable(new Date());
    self.campaignEnd = ko.observable(new Date());
    self.total = ko.computed(function () {
        var total = 0;
        $.each(
            self.items(),
            function() {
                total += round(this.price(), 2);
            });
        return total;
    });

    this.addMediaItemJson = function (jsonItem) {
        var item = new MediaItem();
        item.name(jsonItem.Name);
        item.price(jsonItem.Price);
        item.image(jsonItem.Image);
        item.description(jsonItem.Description);
        item.publisher(jsonItem.Publisher);
        item.type(jsonItem.Type);
        item.id(jsonItem.Id);
        self.items.push(item);
        self.save();
    }

    this.checkout = function () {
        var mediaItemsList = $.map(self.items(), function (item) {
            return item.id() ? {
                Id: item.id(),
                Name: item.name(),
                Price: item.price(),
                Image: item.image(),
                Description: item.description(),
                Publisher: item.publisher(),
                Type: item.type()
            } : undefined
        });
        var campaignData = { Id: self.campaignId(), Name: self.campaignName(), MediaItems: mediaItemsList }
       
        $.ajax({
            type: "POST",
            url: '/campaign/CreateFromJson',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(campaignData),
            dataType: "json",
            success: successFunc,
            error: errorFunc
        });

        function successFunc(data, status) {
            $('#ModalCommissioned').modal('toggle');
        }

        function errorFunc(data, status) {
            alert("Json result returned: " + data);
        }

        disableAddToCart(this);
       
    };

    this.save = function() {
        var savedData = ko.toJSON(self);
        localStorage.setItem('savedCampaign', savedData);
    }

    this.load = function () {
        if (localStorage && localStorage.getItem('savedCampaign')) {
            var retrievedData = JSON.parse(localStorage.getItem('savedCampaign'));
            ko.mapping.fromJS(retrievedData, {}, self);
        }
    }



    this.removeMediaItemJson = function(item) {
        self.items.remove(item);
    };

}



function round(value, exp) {
    if (typeof exp === 'undefined' || +exp === 0)
        return Math.round(value);

    value = +value;
    exp = +exp;

    if (isNaN(value) || !(typeof exp === 'number' && exp % 1 === 0))
        return NaN;

    // Shift
    value = value.toString().split('e');
    value = Math.round(+(value[0] + 'e' + (value[1] ? (+value[1] + exp) : exp)));

    // Shift back
    value = value.toString().split('e');
    return +(value[0] + 'e' + (value[1] ? (+value[1] - exp) : -exp));
}

function MediaItem() {
    this.name = ko.observable(name || 'defaultName');
    this.price = ko.observable(0.00);
    this.image = ko.observable("http://thumbs.dreamstime.com/z/blank-billboard-city-15056776.jpg");
    this.description = ko.observable();
    this.publisher = ko.observable();
    this.type = ko.observable();
    this.id = ko.observable();
}

