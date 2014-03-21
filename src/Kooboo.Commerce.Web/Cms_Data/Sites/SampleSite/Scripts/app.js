$(function () {
    //Fancybox
    $('#login-link').fancybox({
        showCloseButton: false,
        overlayColor: '#000',
        opacity: 0.3,
        autoDimensions: false,
        height: 260
    });

    $('.ajaxForm').each(function () {
        var form = $(this);
        form.ajaxForm({
            sync: true,
            beforeSerialize: function ($form, options) {
            },
            beforeSend: function () {
                var form = $(this);
                form.find("[type=submit]").addClass("disabled").attr("disabled", true);
            },
            beforeSubmit: function (arr, $form, options) {
            },
            success: function (responseData, statusText, xhr, $form) {
                form.find("[type=submit]").removeClass("disabled").removeAttr("disabled");
                if (!responseData.Success) {
                    var validator = form.validate();
                    //                            var errors = [];
                    for (var i = 0; i < responseData.FieldErrors.length; i++) {
                        var obj = {};
                        var fieldName = responseData.FieldErrors[i].FieldName;
                        if (fieldName == "") {
                            alert(responseData.FieldErrors[i].ErrorMessage);
                        }
                        obj[fieldName] = responseData.FieldErrors[i].ErrorMessage;
                        validator.showErrors(obj);
                    }
                }
                else {
                    if (responseData.RedirectUrl != null) {
                        location.href = responseData.RedirectUrl;
                    }
                    else {
                        location.reload();
                    }

                }
            },
            error: function () {
                var form = $(this);
                form.find("[type=submit]").removeClass('disabled').removeAttr('disabled');
            }

        });
    })

    function requestVerificationToken() {
        return $(':hidden[name="__RequestVerificationToken"]').val();
    }

    var commands = {
        orders: {
            createPayment: function (data) {
                var errors = [];

                if (!data.paymentMethodId) {
                    errors.push('Payment method is required.');
                }

                if (errors.length > 0) {
                    return $.Deferred().resolve({
                        Success: false,
                        Messages: errors
                    }).promise();
                }

                data.__RequestVerificationToken = requestVerificationToken();
                data.returnUrl = '/PaymentReturn';

                return $.post('/Kooboo-Submit/CreatePayment', data);
            },
            pay: function (data) {
                commands.orders
                        .createPayment(data)
                        .then(function (result) {
                            if (!result.Success) {
                                alert(result.Messages.join('\n'));
                            } else {
                                window.location.href = result.Model.RedirectUrl;
                            }
                        });
            }
        }
    };

    window.commands = commands;
})
