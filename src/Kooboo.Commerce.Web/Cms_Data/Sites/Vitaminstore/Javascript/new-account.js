$(function () {
    var settngs = $.data($('form')[0], 'validator').settings;
    settngs.onkeyup = false;

    $.validator.unobtrusive.parse($('form'));

    $('.clubcard-yes').click(function () {
        $('.clubcard').show();
        $('.no-clubcard').hide();
    });

    $('.clubcard-no').click(function () {
        $('.clubcard').hide();
        $('.no-clubcard').show();
    });

    $('.clubcard-question').hover(function () {
        $('.clubcard-tip-diamond').show();
        $('.clubcard-tip').show();
    }, function () {
        $('.clubcard-tip-diamond').delay(1000).fadeOut(500);
        $('.clubcard-tip').delay(1000).fadeOut(500);
    });

    $('.clubcard-number').blur(function () {
        GetInfoFromClubCard();
    });

    $('.clubcard-number').keyup(function () {
        $('.clubcard-invalid').hide();
    });

    $('.clubcard-postcode').keyup(function () {
        GetInfoFromClubCard();
    });

    $favoriteCategories = $('#FavoriteCategories');
    $('.favorite-category a').click(function (e) {
        e.preventDefault();

        $(this).toggleClass('selected', 1000, 'easeOutSine');

        var selectedFavorites = '';
        $('.favorite-category a.selected').each(function () {
            selectedFavorites += '#' + $(this).attr('data-value');
        });

        $favoriteCategories.val(selectedFavorites);
    });

    $('.postcode').keyup(function () {
        GetAddress(false);
    });

    $('.housenumber').blur(function () {
        GetAddress(true);
    });

    $('.btn-save, .btn-create input').click(function () {
        if ($('form').valid()) {
            return validatePostcode();
        }

        $('.input-validation-error:first').focus();

        return false;
    });
});

function validatePostcode() {
    if ($.trim($('.country').val()) == 'NL' && !IsNLPostcode($('.postcode').val())) {
        $('.postcode').focus();
        alert('Ongeldige Postcode.');

        return false;
    }

    return $('.postcode').val() != '';
}

function IsEAN13Code(number) {
    number = number.replace(/ /g, '');
    var pattern = /^[1-9][0-9]{11,13}$/;

    return pattern.exec(number);
}

function GetInfoFromClubCard() {
    var cardNumber = $.trim($('.clubcard-number').val());
    var postcode = $.trim($('.clubcard-postcode').val());
    $invalidClubCard = $('.clubcard-invalid');

    if (cardNumber != '' && postcode != ''
        && IsEAN13Code(cardNumber) && IsNLPostcode(postcode)) {
        $loading = $('.clubcard-loading');
        $loading.show();

        $isValid = $('#ctl00_cpContent_hfClubCardValid');

        $.ajax({
            type: "POST",
            url: '/handlers/GetClubCardInfo.ashx',
            data: { cardnumber: cardNumber, postcode: postcode },
            success: function (data) {
                var json = $.parseJSON(data);

                if (json.NotFound) {
                    $isValid.val('');
                    $invalidClubCard.show();
                    $invalidClubCard.addClass('field-validation-error');
                }
                else if (json.Matched == false) {
                    $isValid.val('');

                    $invalidClubCard.show();
                    $invalidClubCard.addClass('field-validation-error');
                }
                else {
                    $invalidClubCard.hide();
                    $isValid.val('true');
                    BindClient(json);
                }

                $loading.hide();
            },
            error: function (xhr, textStatus, errorThrown) {
                $loading.hide();
            }
        });
    }
    else {
        $invalidClubCard.hide();
    }
}

function BindClient(data) {
    $(".gender").val(data.Gender);
    $('.firstname').val(data.FirstName);
    $('.middlename').val(data.MiddleName);
    $('.lastname').val(data.LastName);
    $('.birthdate').val(data.Birthday);
    $('.tel').val(data.Tel);
    $('.mobile').val(data.Mobile);
    $('.email').val(data.Email);
    $('#ctl00_cpContent_hfClubId').val(data.ValkClientID);
    $('#ctl00_cpContent_hfPoints').val(data.Bonuspoints);

    $('.street').val(data.Street);
    $('.housenumber').val(data.HouseNr);
    $('.housenumberExt').val(data.Toe);
    $('.postcode').val(data.PostCode);
    $('.city').val(data.City);
}

function IsNLPostcode(postcode) {
    var pattern = /^[1-9]{1}[0-9]{3} ?[A-Za-z]{2}$/;

    return pattern.exec(postcode);
}

function GetAddress(isHouseNr) {
    var postcode = $.trim($('.postcode').val());
    var houseNr = $.trim($('.housenumber').val());

    if (IsNLPostcode(postcode) && houseNr != '' && !isNaN(houseNr) && $.trim($('.country').val()) == 'NL') {
        postcode = postcode.replace(/ /g, '');
        var url = '/GetAddress.ashx?address=' + postcode + houseNr;
        $.get(url, function (result) {
            if (result != '') {
                var oResultData = eval('(' + result + ')');

                $('.street').val(oResultData.straatnaam);
                $('.city').val(oResultData.plaatsnaam);
            }
            else if (isHouseNr) {
                WrongHouseNr();
            }
        });
    }
}

function WrongHouseNr() {
    alert("Dit huisnummer klopt niet bij de ingevoerde postcode.");
}