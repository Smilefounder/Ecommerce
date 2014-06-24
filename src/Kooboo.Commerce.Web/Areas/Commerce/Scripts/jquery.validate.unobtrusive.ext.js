$.validator.unobtrusive.reparse = function (selector) {
    var $container = $(selector);
    if ($container.is('form')) {
        removeFormValidation($container);
    }

    $container.find('form').each(function () {
        removeFormValidation(this);
    });

    $.validator.unobtrusive.parse($container);

    function removeFormValidation(form) {
        $(form).removeData('validator');
        $(form).removeData('unobtrusiveValidation');
    }
};

$.validator.unobtrusive.adapters.add('uniquefield', [], function (options) {
    var value = {
        url: '/Commerce/Product/ValidateFieldUniqueness',
        type: 'GET',
        data: {
            fieldName: options.element.name,
            fieldValue: function () {
                return $(options.element).val();
            },
            fieldType: function () {
                return $(options.element).data('field-type');
            },
            productId: function () {
                return $(options.form).find(':input[name="ProductId"]').val()
            }
        }
    };

    options.rules['remote'] = value;
    if (options.message) {
        options.messages['remote'] = options.message;
    }
});