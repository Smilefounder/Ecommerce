 var ProfileDialog = {
    _currentDialogId: null,

    open: function (options) {
        var dialogId = options.dialogId || '__dialog_' + new Date().getTime().toString();
        var profile = options.profile || '.myProfile';
        var $dialog = $('<div id="' + dialogId + '" class="profile-dialog" style="display:none"></div>');
        $(profile).append($dialog);

        ProfileDialog._currentDialogId = dialogId;

        var settings = {
            close: function () {
                $dialog.remove();
            }
        };

        settings = $.extend(true, settings, options);

        if (options.content) {
            var content = options.content;

            if (typeof (options.content) == 'function') {
                content = options.content(function (html) {
                    $dialog.html(html);
                });
            }

            $dialog.html(content);
        } else if (options.url) {
            var width = options.width;
            var height = options.height;

            var url = options.url;

            var qs = 'dialogId=' + dialogId;
            if (url.indexOf('?') > 0) {
                url += '&' + qs;
            } else {
                url += '?' + qs;
            }

            var iframe = '<iframe src="' + url + '" frameborder="0" width="' + width + '" height="' + height + '"></iframe>';
            $dialog.html(iframe);
        }

        var scrollTop = $('body').scrollTop();
        if (scrollTop > 0) {
            var top = scrollTop + 60;

            if ($('.header h1').length) {
                top = top - $('.header h1').offset().top;
            }
            else {
                top = top - $dialog.parent('div').offset().top;
            }

            if (top > 0) {
                $dialog.css('top', top);
            }
        }

        if (options.center) {
            $dialog.css('left', ($dialog.parent('div').width() - $dialog.width() - 15) / 2);
        }

        $dialog.show();

        return $dialog;
    },
    close: function (dialogId) {
        if (dialogId != '') {
            if ($('#' + dialogId).length > 0) {
                $('#' + dialogId).remove();
            }
            else if (window.parent && window.parent.jQuery) {
                window.parent.jQuery('#' + dialogId).remove();
            }
        }
        else if (window.parent && window.parent.jQuery) {
            window.parent.jQuery('.profile-dialog').remove();
        }
    },
    closeCurrent: function () {
        if (ProfileDialog._currentDialogId) {
            ProfileDialog.close(ProfileDialog._currentDialogId);
        }
    }
};