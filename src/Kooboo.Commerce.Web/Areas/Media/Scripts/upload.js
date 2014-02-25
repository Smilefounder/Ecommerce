(function ($) {
    $.widget('media.upload', {
        options: {
            url: '/Media/Upload',
            owner: '',
            property: '',
            width: 180,
            height: 220,
            max_file_size: 5 * 1024 * 1024,
            accept_file_types: /(\.|\/)(gif|jpe?g|png)$/i,
            src: '',
            crop_image: true,
            on_file_select: null
        },
        _create: function () {
            var self = this,
                ops = self.options,
                ele = self.element;
            if (typeof ops.owner == "function") { ops.owner = ops.owner(ele); }
            if (typeof ops.property == "function") { ops.property = ops.property(ele); }
            if (typeof ops.width == "function") { ops.width = ops.width(ele); }
            if (typeof ops.height == "function") { ops.height = ops.height(ele); }
            if (typeof ops.src == "function") { ops.src = ops.src(ele); }
            if (typeof ops.crop_image == "function") { ops.crop_image = ops.crop_image(ele); }

            ops.owner = ops.owner || $(ele).attr('data-owner');
            ops.property = ops.property || $(ele).attr('data-property');
            ops.width = ops.width || $(ele).attr('data-width');
            ops.height = ops.height || $(ele).attr('data-height');
            ops.src = ops.src || $(ele).attr('data-src');
            ops.crop_image = ops.crop_image || $(ele).attr('data-crop-image');

            $(ele).on('click', function () {
                self.open();
            })

            utils.onReceiveMessage("fileselected", function (msg) {
                if (msg.owner == ops.owner && msg.property == ops.property) {
                    if (typeof ops.on_file_select == "function") {
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
            var ops = this.options;
            var url = ops.url + '?owner=' + ops.owner + '&path=' + ops.property + '&width=' + ops.width + '&height=' + ops.height + '&max_file_size=' + ops.max_file_size + '&accept_file_types=' + ops.accept_file_types + '&crop=' + ops.crop_image + '&file=' + ops.src;
            utils.openDialog('select file...', url, 800, 500);
        },
        destroy: function () {
            $.Widget.prototype.destroy.call(this);
        }
    });
})(jQuery);

(function ($) {
    $.widget('media.upload_image', {
        options: {
            upload_handler: '/Areas/Media/Handlers/UploadHandler.ashx',
            save_handler: '/Media/Upload/SaveImage',
            owner: '',
            property: '',
            width: 180,
            height: 220,
            max_file_size: 5 * 1024 * 1024,
            accept_file_types: /(\.|\/)(gif|jpe?g|png)$/i,
            crop_image: true,
            img_src: '',
            no_img_src: '/images/nophoto.gif',
        },
        _create: function () {
            var self = this,
                ops = self.options,
                ele = self.element;

            if (typeof ops.owner == "function") { ops.owner = ops.owner(ele); }
            if (typeof ops.property == "function") { ops.property = ops.property(ele); }
            if (typeof ops.width == "function") { ops.width = ops.width(ele); }
            if (typeof ops.height == "function") { ops.height = ops.height(ele); }
            if (typeof ops.img_src == "function") { ops.img_src = ops.img_src(ele); }
            if (typeof ops.crop_image == "function") { ops.crop_image = ops.crop_image(ele); }

            var id = utils.newid();
            ele.attr('id', id);
            var form = $('<form id="' + id + '_form" name="' + id + '_form" action="' + ops.upload_handler + '?owner=' + ops.owner + '" method="post" enctype="multipart/form-data"></form>');
            var img_div = $('<div></div>').css({ width: ops.width, height: ops.height, overflow: 'hidden' });
            var img = $('<img/>').attr('src', ops.img_src || ops.no_img_src);
            var inputfile = $('<input type="file" name="files[]" style="display: none;" />');

            form.append(img_div);
            img_div.append(img);
            form.append(inputfile);
            ele.append(form);

            img.on('click', function () { $('#' + id + '_form input:file').trigger('click'); });

            if (ops.crop_image) {
                self.jcrop_api = null;
                var original_img_div = $('<div id="' + id + '_original_img_div" class="hide original_image" style="overflow: hidden; padding: 0px;"></div>');
                var inner_div = $('<div style="z-index: 1; position: relative;"></div>');
                var original_img = $('<img src="" style="display: inline-block; min-width: 120px;" />');

                inner_div.append(original_img);
                original_img_div.append(inner_div);
                $('body').append(original_img_div);
                ele.attr('data-cropper', '#' + id + '_original_img_div');

                img.on('load', function () {
                    var src = $(this).prop('src');
                    var img_src = ops.img_src || ops.no_img_src;
                    if (src.indexOf(img_src) >= 0) { return false; }
                    $(this).css({ width: 'auto', height: 'auto' });
                    original_img.prop('src', src);
                    $(original_img).Jcrop({
                        onChange: function (coords) { self.showPreview(coords); },
                        onSelect: function (coords) { self.showPreview(coords); }
                    }, function () {
                        self.jcrop_api = this;
                        var dimension = self.jcrop_api.getBounds();
                        self.jcrop_api.setSelect([0, 0, ops.width, ops.height]);
                        self.jcrop_api.setOptions({ aspectRatio: ops.width / ops.height });
                        self.jcrop_api.focus();
                        img.css({ width: dimension[0].toString() + 'px', height: dimension[1].toString() + 'px' });
                    });
                    utils.showDialog(original_img_div, 'crop image', function () { self.saveCropImage(); }, null, $(this).width(), $(this).height() + 120);
                });
            }
            form.fileupload({
                maxFileSize: ops.max_file_size,
                acceptFileTypes: ops.accept_file_types,
                add: function (e, data) {
                    $(this).fileupload('process', data).done(function () {
                        data.submit();
                        $('#' + id + '_form input:file').val('');
                    });
                },
                done: function (e, data) {
                    var results = eval(data.result);
                    var file = ($.isArray(results) && results[0]) || { error: 'emptyResult' };
                    if (file && file.url) {
                        img.prop('src', file.url);
                    }
                },
                progress: function (e, data) {
                    var val = parseInt(data.loaded / data.total * 100, 10);
                    utils.progress(val, img);
                }
            });
        },
        _init: function () {
        },
        showPreview: function (coords) {
            if (parseInt(coords.w) > 0) {
                this.cropCoords = coords;
                var rx = this.options.width / coords.w;
                var ry = this.options.height / coords.h;
                var original_img = $($(this.element).attr('data-cropper'));
                var iw = original_img.width();
                var ih = original_img.height();
                var img = $(this.element).find('form img');
                img.css({
                    width: Math.round(rx * iw) + 'px',
                    height: Math.round(ry * ih) + 'px',
                    marginLeft: '-' + Math.round(rx * coords.x) + 'px',
                    marginTop: '-' + Math.round(ry * coords.y) + 'px'
                });
            }
        },
        calculateTrueCoords: function () {
            var original_img = $($(this.element).attr('data-cropper'));
            var tw = original_img.prop('width');
            var th = original_img.prop('height');
            var bw = original_img.width();
            var bh = original_img.height();

            var cx = this.cropCoords.x / bw * tw;
            var cy = this.cropCoords.y / bh * th;
            var cw = this.cropCoords.w / bw * tw;
            var ch = this.cropCoords.h / bh * th;
            return { x: cx, y: cy, w: cw, h: ch };
        },
        saveCropImage: function () {
            var self = this,
                ops = self.options,
                ele = self.element;
            var original_img = $(ele.attr('data-cropper')).find('img');
            var src = original_img.attr('src');
            var pnq = utils.getPathNQuery(src);
            var tcoords = self.calculateTrueCoords();
            var url = ops.save_handler + "?file=" + escape(pnq.path) + "&x=" + tcoords.x + '&y=' + tcoords.y + '&w=' + tcoords.w + '&h=' + tcoords.h;
            utils.getJson(url, function (data) {
                if (self.jcrop_api) {
                    self.jcrop_api.destroy();
                };
                var rsrc = utils.appendUrl(pnq.path, 'v', new Date().getTime());
                original_img.prop('src', rsrc).css({ width: 'auto', height: 'auto' });
                args = utils.getQueryString('args');
                dialogId = utils.getQueryString('dialogId');
                if (typeof window.onFileUploaded == "function") {
                    window.onFileUploaded(data, args, dialogId);
                }
                utils.postMessage("cropimage", { url: data, owner: ops.owner, property: ops.property, args: args, dialogId: dialogId });
                utils.closeDialog(dialogId);
            });
        },
        destroy: function () {
            if (this.jcrop_api) { this.jcrop_api.destroy(); }
            $.Widget.prototype.destroy.call(this);
        }
    });
})(jQuery);
