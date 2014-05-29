jQuery.support.cors = true;
var utils = window.utils = {
    ajaxTimeOut: 50000,
    minDialogWidth: 450,
    minDialogHeight: 200,
    initCommonEvents: function () {
        utils.onReceiveMessage("opendialog", function (msg) { utils.onOpenDialog(msg.title, msg.url, msg.width, msg.height, msg.opener); });
        utils.onReceiveMessage("closedialog", function (msg) { utils.onCloseDialog(msg.dlgId); });
        utils.onReceiveMessage("showmessage", function (msg) { utils.onShowMessage(msg.title, msg.msg); });
        utils.onReceiveMessage("progress", function (msg) { utils.onProgress(msg.val); });
        utils.onReceiveMessage("overlay", function (msg) { utils.onOverlay(msg.enable); });
        utils.onReceiveMessage("reload", function (msg) { utils.reload(msg); });
        utils.onReceiveMessage("redirect", function (url, msg) { utils.redirect(url, msg); });
        utils.showLocalMessage();
    },
    getCookieValue: function (name) {
        if (document.cookie && document.cookie != '') {
            var cookies = document.cookie.split(';');
            for (var i = 0; i < cookies.length; i++) {
                var cookie = jQuery.trim(cookies[i]);
                if (cookie.substr(0, name.length + 1) == (name + '=')) {
                    return decodeURIComponent(cookie.substr(name.length + 1));
                }
            }
        }
        return null;
    },
    getQueryString: function (name) {
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(window.location.search);
        if (results == null)
            return "";
        else
            return decodeURIComponent(results[1].replace(/\+/g, " "));
    },
    getPathNQuery: function (url) {
        var idx = url.indexOf('?');
        if (idx < 0) {
            return { path: url, query: null };
        }
        return { path: url.substr(0, idx), query: url.substr(idx + 1) }
    },
    appendUrl: function (uri, paraName, paraValue, includeEmpty) {
        if (!paraName || !paraValue || paraName.length <= 0 || paraValue.length <= 0) {
            if (!includeEmpty) {
                return uri;
            }
        }
        var separator = uri.indexOf('?') !== -1 ? "&" : "?";
        var re = new RegExp("([?|&])" + paraName + "=.*?(&|$)", "i");
        if (uri.match(re)) {
            return uri.replace(re, '$1' + paraName + "=" + escape(paraValue) + '$2');
        }
        else {
            return uri + separator + paraName + "=" + escape(paraValue);
        }
    },
    getOrigin: function (location) {
        if (location.origin) {
            return location.origin;
        } else {
            return (window.location.protocol + "//" + window.location.host + ":" + window.location.port + "/" + window.location.pathname);
        }
    },
    isCrossDomain: function (url) {
        if (url.indexOf('://') >= 0) {
            var fidx = url.indexOf("/", url.indexOf("://") + 3);
            var host = url.substr(0, fidx).toLowerCase();
            var fromUrl = utils.getOrigin(window.location).toLowerCase();
            return fromUrl.indexOf(host) < 0;
        }
        return false;
    },
    str2dic: function (val, comma, equal, leftComma, rightComma) {
        if (val.length <= 0)
            return [];
        if (!comma) { comma = '&'; }
        if (!equal) { equal = '='; }
        if (!leftComma) { leftComma = ''; }
        if (!rightComma) { rightComma = ''; }
        var dic = new Array();
        var startIdx = 0;
        var equalIdx = 0;
        var leftIdx = 0, rightIdx = 0;
        var key = "", value = "";
        while (startIdx < val.length) {
            equalIdx = val.indexOf(equal, startIdx);
            key = val.substr(startIdx, equalIdx - startIdx);
            if (leftComma.length <= 0)
                leftIdx = equalIdx + 1;
            else
                leftIdx = val.indexOf(leftComma, equalIdx) + leftComma.length;
            if (rightComma.length <= 0)
                rightIdx = val.indexOf(comma, leftIdx);
            else
                rightIdx = val.indexOf(rightComma, leftIdx);
            if (rightIdx < 0)
                rightIdx = val.length;
            value = val.substr(leftIdx, rightIdx - leftIdx);
            var obj = { "Key": key, "Value": value };
            dic.push(obj);
            rightIdx = val.indexOf(comma, rightIdx);
            if (rightIdx < 0)
                rightIdx = val.length;
            startIdx = rightIdx + comma.length;
        }
        return dic;
    },
    dic2str: function (dic, comma, equal, leftComma, rightComma) {
        if (dic.length <= 0)
            return "";
        if (!comma) { comma = '&'; }
        if (!equal) { equal = '='; }
        if (!leftComma) { leftComma = ''; }
        if (!rightComma) { rightComma = ''; }
        var val = "";
        for (var i = 0; i < dic.length; i++) {
            if (val.length > 0)
                val += comma;
            val += dic[i].Key + equal + leftComma + dic[i].Value + rightComma;
        }
        return val;
    },
    newid: function () {
        var ran = Math.floor(Math.random() * 10000);
        if (ran < 1000)
            ran += 999;
        return new Date().getTime().toString() + ran.toString();
    },
    postMessage: function (event, msg, target, targetOrgin) {
        if (!targetOrgin) {
            targetOrgin = '*';
        }
        var dialogId = msg.dialogId || utils.getQueryString("dialogId");
        if (!target) {
            if (dialogId && window.dialogs) {
                var win = $.grep((window.dialogs || []), function (n, i) { return n.dialog == dialogId; })[0];
                if (win) {
                    target = win.opener;
                }
            }
            if (!target) {
                target = window.parent || window.top;
            }
            $(window.self).trigger(event, [msg]);
        }
        if (target.postMessage && !(target == window.self)) {
            target.postMessage(JSON.stringify({ event: event, msg: msg }), targetOrgin);
        } else {
            $(target).trigger(event, [msg]);
        }
    },
    onReceiveMessage: function (event, func) {
        $(window).on(event, function (e, msg) {
            if (func) {
                func(msg);
            }
        });
    },
    overlay: function (enable, container) {
        utils.postMessage("overlay", { enable: enable, container: container }, window.top);
        return false;
    },
    onOverlay: function (enable, container) {
        if (!container) {
            container = $('body');
        } else {
            $(container).css('position', 'relative');
        }
        var ol = $(container).find('div.overlay');
        if (!ol.is('div')) {
            ol = $('<div class="overlay"></div>');
            $(container).append(ol);
        }
        try {
            if (enable) {
                ol.show();
            } else {
                ol.hide();
            }
        } catch (ex) { }
    },
    progress: function (val) {
        utils.postMessage("progress", { val: val }, window.top);
        return false;
    },
    onProgress: function (val, container) {
        if (!container) {
            container = $('<div class="progressbar"></div>');
            $('body').append(container);
        }
        var pg = $(container).find('div.progress');
        if (!pg.is('div')) {
            pg = $('<div class="progress"><div class="bar"></div></div>');
            $(container).append(pg);
        }
        try {
            if (val && !isNaN(val)) {
                pg.find('div.bar').css('width', val.toString() + '%');
                pg.show();
            }
            else {
                progressbar.hide();
            }
        } catch (ex) { }
    },
    clearScreen: function () {
        utils.progress();
        utils.closeDialog(false);
    },
    showLocalMessage: function () {
        var str = window.localStorage.getItem("alert");
        if (str && str.length > 0) {
            var msg = JSON.parse(str);
            utils.showMessage(msg.title || 'alert info', msg.message);
            window.localStorage.removeItem("alert");
        }
    },
    showDialog: function (ele, title, funcOK, funcCancel, width, height, ops) {
        var options = {
            modal: true,
            title: title,
            zIndex: 1200,
            maxWidth: $(window).width() - 200,
            maxHeight: $(window).height() - 100,
            minWidth: utils.minDialogWidth,
            minHeight: utils.minDialogHeight,
            buttons: [
                {
                    "text": "OK",
                    "class": "btn btn-success",
                    "click": function () { if (funcOK) { if (!funcOK()) { $(this).dialog("close"); }; } else { $(this).dialog("close"); } }
                },
                {
                    "text": "Cancel",
                    "class": "btn",
                    "click": function () { if (funcCancel) { funcCancel(); } $(this).dialog("close"); }
                }
            ]
        };
        if (width) {
            options.width = parseInt(width);
        }
        if (height) {
            options.height = parseInt(height);
        }

        options = $.extend(options, ops);

        if ($.browser.msie && $.browser.version < 8.0) {
            options.open = function (event, ui) {
                $('select').css('visibility', 'hidden');
            };
            options.close = function (event, ui) {
                $('select').css('visibility', 'visible');
            };
        }
        $(ele).dialog(options);
    },
    openDialog: function (title, url, width, height) {
        utils.postMessage("opendialog", { title: title, url: url, width: width, height: height, opener: null }, window.top);
        return false;
    },
    onOpenDialog: function (title, url, width, height, opener) {
        if (!opener) {
            opener = window;
        }
        if (!window.dialogs) {
            window.dialogs = [];
        }
        var id = new Date().getTime();
        url = this.appendUrl(url, "dialogId", id);
        if (this.isCrossDomain(url))
            url = this.appendUrl(url, "cros", "1");

        var dialog = $('<div id="' + id + '"><iframe scrolling="no" frameborder="0" src="' + url + '"></iframe></div>');
        var options = {
            modal: true,
            title: title,
            zIndex: 1100,
            dialogClass: 'iframe-dialog',
            draggable: false,
            resizable: false,
            maxWidth: $(window).width() - 200,
            maxHeight: $(window).height() - 100,
            minWidth: utils.minDialogWidth,
            minHeight: utils.minDialogHeight,
            close: function (event, ui) { $('#' + id).remove(); }
        };

        if (width) {
            options.width = parseInt(width);
        }
        if (height) {
            options.height = parseInt(height);
        }

        if ($.browser.msie && $.browser.version < 8.0) {
            options.open = function (event, ui) {
                $('select').css('visibility', 'hidden');
            };
            options.close = function (event, ui) {
                $('select').css('visibility', 'visible');
                $('#' + id).remove();
            };
        }
        else {
            options.close = function (event, ui) {
                $('#' + id).remove();
            };
        }
        window.dialogs.push({ dialog: id, opener: opener });
        $(dialog).dialog(options);
    },
    closeDialog: function (dlgId) {
        utils.postMessage("closedialog", { dlgId: dlgId }, window.top);
        return false;
    },
    onCloseDialog: function (dlgId) {
        if (dlgId) {
            $('#' + dlgId).dialog('close').remove();
        }
        else {
            $('div.ui-dialog-content').dialog('close').remove();
        }
    },
    loadHtml: function (ele, url, funcSuccess, funcError) {
        $(ele).empty().load(url, function (response, status, xhr) {
            this.progress();
            if (status == "error") {
                if (funcError) {
                    funcError(xhr.statusText, url);
                }
            }
            else {
                if (funcSuccess) {
                    funcSuccess();
                }
            }
        });
    },
    getJsonp: function (url, data, funcSuccess, funcError, funcBeforeSend) {
        url = this.appendUrl(url, "v", $.now());
        if (typeof (data) == "function") {
            funcError = funcSuccess;
            funcSuccess = data;
            data = null;
        }
        $.ajax({
            url: url,
            type: 'GET',
            dataType: 'jsonp',
            //crossDomain: true,
            data: data,
            traditional: true,
            timeout: utils.ajaxTimeOut,
            beforeSend: function (xhr) {
                if (funcBeforeSend) {
                    funcBeforeSend(xhr);
                }
            },
            success: function (data, status, xhr) {
                if (funcSuccess) {
                    funcSuccess(data);
                }
            },
            error: function (xhr, status, error) {
                if (funcError) {
                    funcError(error, url);
                }
            }
        });
    },
    getJson: function (url, data, funcSuccess, funcError, funcBeforeSend) {
        if (typeof (data) == "function") {
            funcError = funcSuccess;
            funcSuccess = data;
            data = null;
        }
        return $.ajax({
            url: url,
            type: 'GET',
            dataType: 'json',
            data: data,
            traditional: true,
            timeout: utils.ajaxTimeOut,
            beforeSend: function (xhr) {
                if (funcBeforeSend) {
                    funcBeforeSend(xhr);
                }
            },
            success: function (data, status, xhr) {
                if (funcSuccess) {
                    funcSuccess(data);
                }
            },
            error: function (xhr, status, error) {
                if (funcError) {
                    funcError(error, url);
                }
            }
        });
    },
    postJson: function (url, data, funcSuccess, funcError, funcBeforeSend) {
        if (typeof (data) == "function") {
            funcError = funcSuccess;
            funcSuccess = data;
            data = null;
        }
        $.ajax({
            url: url,
            type: 'POST',
            dataType: 'json',
            data: JSON.stringify(data),
            timeout: utils.ajaxTimeOut,
            contentType: 'application/json; charset=utf-8',
            beforeSend: function (xhr) {
                if (funcBeforeSend) {
                    funcBeforeSend(xhr);
                }
            },
            success: function (data, status, xhr) {
                if (funcSuccess) {
                    funcSuccess(data);
                }
            },
            error: function (xhr, status, error) {
                if (funcError) {
                    funcError(error, url);
                }
            }
        });
    },
    showMessage: function (title, msg, funcClose) {
        utils.postMessage("showmessage", { title: title, msg: msg }, window.top);
        return false;
    },
    onShowMessage: function (title, msg, level, funcClose) {
        var id = new Date().getTime();
        var dialog = $('<div id="' + id + '" class="' + (level || 'info') + '"><p>' + msg + '</p></div>');
        var options = {
            modal: true,
            title: title,
            zIndex: 1200,
            minWidth: utils.minDialogWidth,
            minHeight: utils.minDialogHeight,
            buttons: [
                {
                    "text": "OK",
                    "class": "btn btn-success",
                    "click": function () { $(this).dialog("close"); }
                }
            ]
        };
        if ($.browser.msie && $.browser.version < 8.0) {
            options.open = function (event, ui) {
                $('select').css('visibility', 'hidden');
            };
            options.close = function (event, ui) {
                $('select').css('visibility', 'visible');
                $('#' + id).remove(); if (funcClose) { funcClose(); }
            };
        }
        else {
            options.close = function (event, ui) {
                $('#' + id).remove(); if (funcClose) { funcClose(); }
            };
        }
        $(dialog).dialog(options);
    },
    showConfirm: function (title, msg, funcOK, funcCancel) {
        var id = new Date().getTime();
        var dialog = $('<div id="' + id + '" class="info"><p>' + msg + '</p></div>');
        var options = {
            modal: true,
            title: title,
            zIndex: 1200,
            minWidth: utils.minDialogWidth,
            minHeight: utils.minDialogHeight,
            buttons: {
                "OK": function () { if (funcOK) { funcOK(); } $(this).dialog("close"); },
                "Cancel": function () { if (funcCancel) { funcCancel(); } $(this).dialog("close"); }
            }
        };
        if ($.browser.msie && $.browser.version < 8.0) {
            options.open = function (event, ui) {
                $('select').css('visibility', 'hidden');
            };
            options.close = function (event, ui) {
                $('select').css('visibility', 'visible');
                $('#' + id).remove();
            };
        }
        else {
            options.close = function (event, ui) {
                $('#' + id).remove();
            };
        }
        $(dialog).dialog(options);
    },
    reload: function (msg) {
        if (msg) {
            window.localStorage.setItem("alert", JSON.stringify(msg));
        }
        window.location.reload();
    },
    redirect: function (url, msg) {
        if (msg) {
            window.localStorage.setItem("alert", JSON.stringify(msg));
        }
        window.location.href = url;
    },
    alertMessage: function () {
        var str = window.localStorage.getItem("alert");
        if (str && str.length > 0) {
            var msg = JSON.parse(str);
            this.showMessage(msg.title || 'infomation', msg.message, 'info');
            window.localStorage.removeItem("alert");
        }
    },
    enableFormValidation: function (form, submitHandler) {
        if ($.validator && $.validator.unobtrusive && form) {
            $(form).data("validator", null)
            $.validator.unobtrusive.parse(form);
            var validator = $(form).data("validator");
            if (validator) {
                validator.settings.submitHandler = submitHandler || function () { return true; };
            }
        }
    },
}

$(function () {
    utils.initCommonEvents();
    window.onmessage = function (e) {
        var p = JSON.parse((e || event).data);
        $(window).trigger(p.event, [p.msg]);
    }
});

/////////////////////////////////////////////////////////////////////////////////////////
// prototype extensions
Array.prototype.clear = function () {
    this.splice(0, this.length);
}
Array.prototype.select = function (funcSelect) {
    var arr = this;
    var selectedArr = [];
    for (var i = arr.length; i--;) {
        selectedArr.unshift(funcSelect(arr[i]));
    }
    return selectedArr;
}
Array.prototype.unique = function (funcCompare) {
    var arr = this;
    var uniqueArr = [];
    for (var i = arr.length; i--;) {
        var item = arr[i];
        if (funcCompare) {
            var fs = $.grep(uniqueArr, function (n, i) { return funcCompare(n, item); })[0];
            if (!fs) {
                uniqueArr.unshift(item);
            }
        }
        else {
            if ($.inArray(item, uniqueArr) === -1) {
                uniqueArr.unshift(item);
            }
        }
    }
    return uniqueArr;
}

function stopEvent(e) {
    e.preventDefault();
    e.stopPropagation();
}