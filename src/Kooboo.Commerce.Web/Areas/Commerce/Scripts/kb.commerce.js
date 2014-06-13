// CMS register this loading function when document ready, but i want to use it before document ready
(function loading() {
    var show = function () {
        $(document.body).addClass('loading');
    }
    var hide = function () {
        $(document.body).removeClass('loading');
    }
    window.loading = { show: show, hide: hide };
})();

(function ($) {

    var kb = window.kb = window.kb || {};

    kb.registerNamespace = function (ns) {
        var parts = ns.split('.');
        var parent = window;
        $.each(parts, function () {
            parent[this] = parent[this] || {};
            parent = parent[this];
        })

        return parent;
    };

    kb.Events = function () {
        var _handlersByEventName = {};

        this.on = function (eventName, handler) {
            var handlers = _handlersByEventName[eventName];
            if (!handlers) {
                handlers = [];
                _handlersByEventName[eventName] = handlers;
            }

            handlers.push(handler);
        };

        this.fire = function (eventName, sender, args) {
            var deferred = $.Deferred();
            var handlers = _handlersByEventName[eventName];
            if (handlers) {
                var promises = [];

                $.each(handlers, function () {
                    var result = this(sender, args);
                    if (result && result.then && typeof (result.then) === 'function') {
                        promises.push(result);
                    }
                });

                if (promises.length > 0) {
                    $.when.apply($, promises)
                     .then(function () {
                         deferred.resolve();
                     });
                } else {
                    deferred.resolve();
                }
            } else {
                deferred.resolve();
            }

            return deferred.promise();
        }
    };

    kb.events = new kb.Events();

    kb.http = {
        safeGet: function (url, data) {
            return $.get(url, data)
                    .fail(function (xhr) {
                        var result = JSON.parse(xhr.responseText);
                        showError(result);
                    });
        },
        safePost: function (url, data) {
            return $.ajax({
                url: url,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(data)
            })
            .fail(function (xhr) {
                var result = JSON.parse(xhr.responseText);
                showError(result);
            });
        }
    };

    kb.utils = {
        currentInstanceName: function () {
            var query = window.location.search;
            if (query) {
                if (query[0] === '?') {
                    query = query.substr(1);
                }

                var instance = null;
                var parts = query.split('&');

                for (var i = 0, len = parts.length; i < len; i++) {
                    var part = parts[i];
                    if (part.indexOf('commerceName') === 0 || part.indexOf('instance') === 0) {
                        var indexOfAssign = parts[i].indexOf('=');
                        if (indexOfAssign > 0) {
                            instance = part.substr(indexOfAssign + 1);
                            break;
                        }
                    }
                }

                return instance;
            }

            return null;
        }
    };

    var math = kb.registerNamespace('kb.math');

    math.descartes = function (arrays) {
        if (arrays.length === 0) {
            return [];
        }

        if (arrays.length === 1) {
            var result = [];
            for (var i = 0, len = arrays[0].length; i < len; i++) {
                result.push([arrays[0][i]]);
            }

            return result;
        }

        return computeDescartes(arrays, 0);
    }

    function computeDescartes(arrays, start) {
        var result = [];

        var array1 = arrays[start];
        var array2 = null;

        if (start === arrays.length - 2) {
            array2 = arrays[start + 1];
        } else {
            array2 = computeDescartes(arrays, start + 1);
        }

        for (var i = 0, len1 = array1.length; i < len1; i++) {
            for (var j = 0, len2 = array2.length; j < len2; j++) {
                var array = [array1[i]];

                // check if array2[j] is an array
                if (array2[j].splice) {
                    for (var k = 0, lenk = array2[j].length; k < lenk; k++) {
                        array.push(array2[j][k]);
                    }
                } else {
                    array.push(array2[j]);
                }

                result.push(array);
            }
        }

        return result;
    }

    function showError(error) {
        window.loading.hide();
        info.show(error.message, false);
    }

    $.ajaxSetup({
        beforeSend: function (xhr) {
            var instance = kb.utils.currentInstanceName();
            if (instance) {
                xhr.setRequestHeader('X-Kooboo-Commerce-Instance', instance);
            }
        }
    });

    // Unobtrusive control initializations
    kb.registerNamespace('kb.ui.unobtrusive');

    kb.ui.unobtrusive.handlers = {};

    kb.ui.unobtrusive.initialize = function (container) {
        $(container).each(function () {
            $(this).find('[data-toggle]').each(function () {
                var $element = $(this);
                var types = $element.data('toggle').split(' ');
                $.each(types, function () {
                    var handler = kb.ui.unobtrusive.handlers[this];
                    if (handler && handler.init) {
                        handler.init($element);
                    }
                });
            });
        });
    };

    kb.ui.unobtrusive.handlers.datepicker = {
        init: function (element) {
            $(element).datepicker();
        }
    };

    kb.ui.unobtrusive.handlers.tinymce = {
        init: function (element) {
            var textarea = $(element);
            if (!textarea.attr('id')) {
                textarea.attr('id', 'Tinymce_' + new Date().getTime());
            }

            var tinyMCEConfig = $.extend({}, tinymce.getKoobooConfig(), {
                '$textarea': textarea,
                'elements': textarea.attr('id'),
                setup: function (ed) {
                    ed.on('change', function (ed, l) {
                        window.leaveConfirm.stop();
                    });
                    ed.on('FullscreenStateChanged', function (e) {
                        $(window.parent.document).find('iframe').toggleClass('fullscreen');
                    });
                    ed.on('BeforeSetContent', function (e) {
                        e.format = 'raw';
                    });
                }
            });

            tinyMCE.init(tinyMCEConfig);
        }
    };

    kb.ui.unobtrusive.handlers.fileupload = {
        init: function (element) {
            var $file = $(element);
            if ($file.attr('type') != 'file') {
                $file = $file.find('[type="file"]');
            }

            if ($file.length === 0) {
                throw 'Cannot find file input.';
            }

            var instance = kb.utils.currentInstanceName();
            var handlerUrl = '/Areas/Commerce/Handlers/UploadHandler.ashx?owner=' + instance + '&path=';

            $file.attr('data-url', handlerUrl);
            $file.fileupload({
                add: function (e, data) {
                    data.submit();
                },
                start: function (e, data) {
                    window.loading.show();
                },
                done: function (e, data) {
                    var results = eval(data.result);
                    var file = ($.isArray(results) && results[0]) || { error: 'emptyResult' };
                    if (file && file.url) {
                        var $text = $(element).closest('[data-toggle="fileupload"]').find(':text');
                        $text.val(file.url);
                        $text.trigger('change');
                    } else {
                        alert('Upload failed');
                    }
                },
                fail: function (e, data) {
                    console.log(arguments);
                    alert('Upload failed');
                },
                stop: function (e, data) {
                    window.loading.hide();
                }
            });
        }
    };

    kb.ui.unobtrusive.handlers.dropdown = {
        init: function (element) {
            $(element).on('click', function () {
                var dropdown = $(this).data('dropdown');
                var $dropdown = dropdown ? $(dropdown) : $(this).find('.dropdown');
                $dropdown.toggle('fast');
            });
        }
    };

})(jQuery);