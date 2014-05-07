var http = {
    post: function (url, data) {
        var params = {
            __RequestVerificationToken: $(':hidden[name="__RequestVerificationToken"]').val()
        };

        $.extend(true, params, data);

        return $.post(url, params);
    }
};