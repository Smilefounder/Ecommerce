(function ($) {
    $.validator.unobtrusive.adapters.add('Birthdate', function (options) {
        options.rules['Birthdate'] = true;
    });

    $.validator.addMethod('Birthdate', function (value, element, param) {
        var validator = this;
        var birthdate = element.value;
        var patrn = /^(0[1-9]|[12][0-9]|3[01])-(0[1-9]|1[0-2])-(\d{4})$/;
        var invalidYear = false;

        if (patrn.exec(birthdate)) {
            if (CheckYear(patrn.exec(birthdate)[3])) {
                return true;
            }
            
            invalidYear = true;
        }
        else {
            var patrn1 = /^(0[1-9]|[12][0-9]|3[01])\/?(0[1-9]|1[0-2])\/?(\d{4})$/;

            if (patrn1.exec(birthdate)) {
                element.value = patrn1.exec(birthdate)[1] + '-' + patrn1.exec(birthdate)[2] + '-' + patrn1.exec(birthdate)[3];

                if (CheckYear(patrn1.exec(birthdate)[3])) {
                    return true
                }

                invalidYear = true;
            }

            var patrn2 = /^(0?[1-9]|[12][0-9]|3[01])[\/|\-](0?[1-9]|1[0-2])[\/|\-](\d{4})$/;
            if (patrn2.exec(birthdate)) {
                element.value = DoubleGigits(patrn2.exec(birthdate)[1]) + '-' + DoubleGigits(patrn2.exec(birthdate)[2]) + '-' + patrn2.exec(birthdate)[3];

                if (CheckYear(patrn2.exec(birthdate)[3])) {
                    return true
                }

                invalidYear = true;
            }
        }

        if (invalidYear) {
            var errors = {};
            errors[element.name] = 'Only allow years after 1900';
            validator.showErrors(errors);
        }

        return false;
    }, '');

    function DoubleGigits(num) {
        if (num.length == 1 && num < 10) {
            return '0' + num;
        }
        else {
            return num;
        }
    }

    function CheckYear(year) {
        return year >= 1900;
    }

    $.validator.unobtrusive.adapters.add('ClubCardNumber', function (options) {
        options.rules['ClubCardNumber'] = true;
    });

    $.validator.addMethod('ClubCardNumber', function (value, element, param) {
        var number = element.value;
        number = number.replace(/ /g, '');
        var pattern = /^[1-9][0-9]{11,13}$/;

        if (pattern.exec(number)) {
            element.value = number;

            return true;
        }

        return false;
    }, 'Invalid ClubCardNumber');

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
})(jQuery);