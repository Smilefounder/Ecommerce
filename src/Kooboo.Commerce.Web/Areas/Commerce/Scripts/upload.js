(function ($) {
    $.widget('media.upload', {
        options: {
            id: null,
            url: '/Commerce/MediaLibrary/Selection',
            instance: '',
            path: '',
            width: 180,
            height: 220,
            multiple: false,
            max_file_size: 5 * 1024 * 1024,
            accept_file_types: /(\.|\/)(gif|jpe?g|png)$/i,
            src: '',
            on_file_select: null
        },
        _create: function () {
            var self = this,
                ops = self.options,
                ele = self.element;
            if (typeof ops.id == 'function') { ops.id = ops.id(ele); }
            if (typeof ops.instance == "function") { ops.instance = ops.instance(ele); }
            if (typeof ops.path == "function") { ops.path = ops.path(ele); }
            if (typeof ops.width == "function") { ops.width = ops.width(ele); }
            if (typeof ops.height == "function") { ops.height = ops.height(ele); }
            if (typeof ops.multiple == "function") { ops.multiple = ops.multiple(ele); }
            if (typeof ops.src == "function") { ops.src = ops.src(ele); }

            ops.id = $(ele).attr('data-id') || ops.id;
            ops.instance = $(ele).attr('data-instance') || ops.instance;
            ops.path = $(ele).attr('data-path') || ops.path;
            ops.width = $(ele).attr('data-width') || ops.width;
            ops.height = $(ele).attr('data-height') || ops.height;
            ops.multiple = $(ele).attr('data-multiple') || ops.multiple;
            ops.src = $(ele).attr('data-src') || ops.src;

            $(ele).on('click', function () {
                self.open();
            })

            utils.onReceiveMessage("fileselected", function (msg) {
                if (msg.opener == ops.id) {
                    if (ops.on_file_select) {
                        msg.element = ele;
                        ops.on_file_select(msg);
                    }
                }
            });
        },
        _init: function () {
        },
        _setOption: function (key, value) {
            this.options = $.extend(this.options, { key: value });
        },
        open: function () {
            var self = this,
                ops = self.options,
                ele = self.element;

            var id = $(ele).attr('data-id') || ops.id;
            var instance = $(ele).attr('data-instance') || ops.instance;
            var path = $(ele).attr('data-path') || ops.path;
            var width = $(ele).attr('data-width') || ops.width;
            var height = $(ele).attr('data-height') || ops.height;
            var multiple = $(ele).attr('data-multiple') || ops.multiple;
            var src = $(ele).attr('data-src') || ops.src;

            var url = ops.url + '?instance=' + instance + '&opener=' + id + '&path=' + path + '&width=' + width + '&height=' + height + '&max_file_size=' + ops.max_file_size + '&multiple=' + multiple + '&accept_file_types=' + ops.accept_file_types + '&file=' + src;
            utils.openDialog('select file...', url, 800, 500);
        },
        destroy: function () {
            $.Widget.prototype.destroy.call(this);
        }
    });
})(jQuery);

(function ($) {
    $.widget('media.cropimage', {
        options: {
            crop_handler: '/Commerce/MediaLibrary/OpenImage',
            instance: '',
            path: '',
            width: 180,
            height: 220,
            src: '',
            keep_ratio: true,
            on_image_croped: null,
            on_cancel: null
        },
        _create: function () {
            var self = this,
                ops = self.options,
                ele = self.element;

            if (typeof ops.instance == "function") { ops.instance = ops.instance(ele); }
            if (typeof ops.path == "function") { ops.path = ops.path(ele); }
            if (typeof ops.width == "function") { ops.width = ops.width(ele); }
            if (typeof ops.height == "function") { ops.height = ops.height(ele); }
            if (typeof ops.src == "function") { ops.src = ops.src(ele); }
            if (typeof ops.keep_ratio == "function") { ops.keep_ratio = ops.keep_ratio(ele); }

            ops.instance = $(ele).attr('data-instance') || ops.instance;
            ops.path = $(ele).attr('data-path') || ops.path;
            ops.width = $(ele).attr('data-width') || ops.width;
            ops.height = $(ele).attr('data-height') || ops.height;
            ops.src = $(ele).attr('src') || $(ele).attr('data-src') || ops.src;
            ops.keep_ratio = $(ele).attr('data-keep-ratio') || ops.keep_ratio;

            $(ele).on('click', null, function () { self.open(); });

            utils.onReceiveMessage("cropimage", function (msg) {
                if (msg.instance == ops.instance && msg.path == ops.path) {
                    if (typeof ops.on_image_croped == "function") {
                        msg.element = ele;
                        ops.on_image_croped(msg);
                    }
                }
            });

            utils.onReceiveMessage("canceldialog", function (msg) {
                if (msg.instance == ops.instance && msg.path == ops.path) {
                    if (typeof ops.on_cancel == "function") {
                        msg.element = ele;
                        ops.on_cancel(msg);
                    }
                }
            });
        },
        _init: function () {
        },
        open: function () {
            var self = this,
                ops = self.options,
                ele = self.element;

            instance = $(ele).attr('data-instance') || ops.instance;
            path = $(ele).attr('data-path') || ops.path;
            width = $(ele).attr('data-width') || ops.width;
            height = $(ele).attr('data-height') || ops.height;
            src = $(ele).attr('src') || $(ele).attr('data-src') || ops.src;
            keep_ratio = $(ele).attr('data-keep-ratio') || ops.keep_ratio;
            if (!keep_ratio) {
                width = $(ele).width();
                height = $(ele).height();
            }

            var url = ops.crop_handler + '?file=' + src + '&width=' + width + '&height=' + height + '&instance=' + instance + '&path=' + path + '&keepRatio=' + keep_ratio;
            utils.openDialog('Crop Image', url, 500, 500);
        },
        destroy: function () {
            $(this.element).off('on', this.open);
            $.Widget.prototype.destroy.call(this);
        }
    });
})(jQuery);
