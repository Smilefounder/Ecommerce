ko.bindingHandlers.checkboxlist = {
    init: function (element, valueAccessor) {
        ko.bindingHandlers.checkboxlist._setValue(element, ko.utils.unwrapObservable(valueAccessor()));

        $(element).on('click', ':checkbox', function () {
            valueAccessor()(ko.bindingHandlers.checkboxlist._getValue(element));
        });
    },
    update: function (element, valueAccessor) {
        ko.bindingHandlers.checkboxlist._setValue(element, ko.utils.unwrapObservable(valueAccessor()));
    },
    _setValue: function (element, value) {
        var values = (value || '').split(',');
        $(element).find(':checkbox').each(function () {
            var $checkbox = $(this);
            var checkboxValue = $checkbox.prop('value');
            if (!checkboxValue) {
                $checkbox.prop('checked', false);
            } else {
                $checkbox.prop('checked', _.contains(values, checkboxValue));
            }
        });
    },
    _getValue: function (element) {
        var values = [];
        $(element).find(':checkbox:checked').each(function () {
            values.push($(this).prop('value'));
        });
        return values.join(',');
    }
};

ko.bindingHandlers.radiolist = {
    init: function (element, valueAccessor) {
        ko.bindingHandlers.radiolist._setValue(element, ko.utils.unwrapObservable(valueAccessor()));
        $(element).on('click', ':radio', function () {
            valueAccessor()(ko.bindingHandlers.radiolist._getValue(element));
        });
    },
    update: function (element, valueAccessor) {
        ko.bindingHandlers.radiolist._setValue(element, ko.utils.unwrapObservable(valueAccessor()));
    },
    _setValue: function (element, value) {
        $(element).find(':radio').each(function () {
            $(this).prop('checked', $(this).prop('value') == value);
        });
    },
    _getValue: function (element) {
        return $(element).find(':radio:checked').val();
    }
};

ko.bindingHandlers.tagbox = {
    init: function (element, valueAccessor) {
        var initValue = ko.utils.unwrapObservable(valueAccessor());
        if (initValue && initValue.join) {
            $(element).val(initValue.join(','));
        } else {
            $(element).val(initValue);
        }

        $(element).tagbox({
            lowercase: false,
            onAdd: function (tag, tags) {
                valueAccessor()(tags);
            },
            onRemove: function (tag, tags) {
                valueAccessor()(tags);
            }
        });
    }
};

ko.bindingHandlers.tooltip = {
    init: function (element, valueAccessor) {
        $(element).tooltip({
            items: element.tagName.toLowerCase(),
            html: true,
            content: function () {
                return ko.utils.unwrapObservable(valueAccessor());
            }
        });
    },
    update: function (element, valueAccessor) {

    }
};

ko.bindingHandlers.select2 = {
    _updateIsCausedBySelect2: false,
    init: function (element, valueAccessor, allBindings) {
        var options = {
            minimumInputLength: 0,
            id: function (item) { return item.id },
            formatResult: function (item) {
                return item[options.textField];
            },
            formatSelection: function (item) {
                return item[options.textField];
            },
            idField: 'id',
            textField: 'text'
        };

        var value = ko.utils.unwrapObservable(valueAccessor());

        if (allBindings.has('select2Options')) {
            var select2Options = allBindings.get('select2Options');

            if (select2Options.onlyLeafSelection === undefined) {
                select2Options.onlyLeafSelection = true;
            }

            if (select2Options.ajax) {
                options.ajax = {
                    dataType: 'json',
                    quietMillis: 100,
                    data: function (term, page) {
                        return {
                            term: term,
                            page: page
                        };
                    },
                    results: function (data, page) {

                        // Remove 'id' field for non-selectable nodes
                        if (select2Options.onlyLeafSelection) {
                            $.each(data.items, function () {
                                removeIdIfHasChildren(this);
                            });

                            function removeIdIfHasChildren(item) {
                                if (item.children && item.children.length > 0) {
                                    delete item.id;

                                    $.each(item.children, function () {
                                        removeIdIfHasChildren(this);
                                    });
                                }
                            }
                        }

                        return { text: options.textField, results: data.items, more: data.more };
                    }
                };

                options.initSelection = function (element, callback) {
                    if (value) {
                        value = ko.toJS(value);
                        callback(value);
                    } else {
                        callback(null);
                    }
                }
            }

            $.extend(true, options, select2Options);
        }

        $(element).select2(options)
                  .on('change', function (e) {
                      ko.bindingHandlers.select2._updateIsCausedBySelect2 = true;

                      if (e.val.push) {
                          var items = [];
                          _.each(e.val, function (id) {
                              var item = {};
                              item[options.idField] = id;
                              items.push(item);
                          });
                          valueAccessor()(items);
                      } else {
                          var oldItem = valueAccessor();
                          oldItem[options.idField](e.val);
                          oldItem[options.textField](null);
                      }

                      ko.bindingHandlers.select2._updateIsCausedBySelect2 = false;
                  });
    },
    update: function (element, valueAccessor, allBindings) {
        if (!ko.bindingHandlers.select2._updateIsCausedBySelect2) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            if (value) {
                $(element).select2('val', ko.toJS(value));
            }
        }
    }
};

ko.bindingHandlers.time = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        //initialize datepicker with some optional options
        var options = allBindingsAccessor().timeOptions || {};
        $(element).timepicker(options);

        //handle the field changing
        ko.utils.registerEventHandler(element, "change", function () {
            var observable = valueAccessor();
            observable($(element).timepicker("getDate"));
        });

        //handle disposal (if KO removes by the template binding)
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).timepicker("destroy");
        });

    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        var current = $(element).timepicker("getDate");

        if (value - current !== 0) {
            $(element).timepicker("setDate", value);
        }
    }
};
ko.bindingHandlers.datetime = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        //initialize datepicker with some optional options
        var options = allBindingsAccessor().datetimeOptions || {};
        $(element).datetimepicker(options);

        //handle the field changing
        ko.utils.registerEventHandler(element, "change", function () {
            var observable = valueAccessor();
            observable($(element).datetimepicker("getDate"));
        });

        //handle disposal (if KO removes by the template binding)
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).datetimepicker("destroy");
        });

    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        var current = $(element).datetimepicker("getDate");

        if (value - current !== 0) {
            $(element).datetimepicker("setDate", value);
        }
    }
};
ko.bindingHandlers.date = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        //initialize datepicker with some optional options
        var options = allBindingsAccessor().dateOptions || {};
        $(element).datepicker(options);

        //handle the field changing
        ko.utils.registerEventHandler(element, "change", function () {
            var observable = valueAccessor();
            observable($(element).datepicker("getDate"));
        });

        //handle disposal (if KO removes by the template binding)
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).datepicker("destroy");
        });

    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        var current = $(element).datepicker("getDate");

        if (value - current !== 0) {
            $(element).datepicker("setDate", value);
        }
    }
};

ko.bindingHandlers.format = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor(),
            allBindings = allBindingsAccessor();
        var valueUnwrapped = ko.utils.unwrapObservable(value);
        if (valueUnwrapped) {
            // parse /Date(1390795296175)/
            if (valueUnwrapped.indexOf('Date') >= 0) {
                valueUnwrapped = parseInt(valueUnwrapped.match(/\d+/)[0])
            }
            var pattern = allBindings.pattern;
            var provider = allBindings.provider;
            if (typeof provider == "string") {
                switch (provider) {
                    case "date":
                        var d = new Date(valueUnwrapped);
                        valueUnwrapped = $.datepicker.formatDate(pattern || 'yy-mm-dd', d);
                        break;
                    case "time":
                        var t = new Date(valueUnwrapped);
                        valueUnwrapped = $.datepicker.formatTime(pattern || 'HH:mm', { hour: t.getHours(), minute: t.getMinutes(), second: t.getSeconds() });
                        break;
                    case "datetime":
                        var ps = (pattern || 'yy-mm-dd HH:mm').split(' ');
                        var dt = new Date(valueUnwrapped);
                        valueUnwrapped = $.datepicker.formatDate(ps[0], dt) + ' ' + $.datepicker.formatTime(ps[1], { hour: dt.getHours(), minute: dt.getMinutes(), second: dt.getSeconds() });
                        break;
                }
            }
            else if (typeof provider == "function") {
                valueUnwrapped = provider(pattern, valueUnwrapped);
            }
            else {
                valueUnwrapped = valueUnwrapped.toString(pattern)
            }
        }
        else {
            valueUnwrapped = '';
        }
        if ($(element).is('input')) {
            $(element).val(valueUnwrapped);
        } else {
            $(element).text(valueUnwrapped);
        }
    }
};

ko.bindingHandlers.cls = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var last_cls = $(element).data('lastClass');
        if (last_cls) {
            $(element).removeClass(last_cls);
        }
        var value = ko.utils.unwrapObservable(valueAccessor() || {});
        if (typeof value == "string") {
            $(element).data('lastClass', value);
            $(element).addClass(value);
        }
        else if (typeof value == "function") {
            var cls = value.call(null, bindingContext.$data);
            if (typeof cls == "string") {
                $(element).data('lastClass', cls);
                $(element).addClass(cls);
            }
        }
    }
};

ko.bindingHandlers.forindex = {
    makeTemplateValueAccessor: function (valueAccessor) {
        return function () {
            var options = valueAccessor();
            var data = ko.utils.unwrapObservable(options.data());
            var start = options.start || 0;
            var count = options.count || data.length;
            var fixCount = options.fixCount || false;

            var bindingValue = data.slice(start, start + count);

            if (fixCount && bindingValue.length > 0) {
                var mcount = count - bindingValue.length;
                while (mcount > 0) {
                    var obj = {};
                    for (var prop in bindingValue[0]) {
                        var propVal = eval("bindingValue[0]." + prop + "()");
                        var type = typeof (propVal);
                        switch (type) {
                            case "string":
                                obj[prop] = ko.observable("");
                                break;
                            case "number":
                                obj[prop] = ko.observable(0);
                                break;
                            case "boolean":
                                obj[prop] = ko.observable(false);
                                break;
                            case "object":
                                if (typeof propVal.length == "number") {
                                    obj[prop] = ko.observableArray([]);
                                }
                                else {
                                    obj[prop] = ko.observable(null);
                                }
                                break;
                            default:
                                obj[prop] = ko.observable(null);
                                break;
                        }
                    }
                    bindingValue.push(obj);
                    mcount--;
                }
            }

            return { 'foreach': bindingValue, 'templateEngine': ko.nativeTemplateEngine.instance };
        };
    },
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        return ko.bindingHandlers['template']['init'](element, ko.bindingHandlers['forindex'].makeTemplateValueAccessor(valueAccessor));
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        return ko.bindingHandlers['template']['update'](element, ko.bindingHandlers['forindex'].makeTemplateValueAccessor(valueAccessor), allBindingsAccessor, viewModel, bindingContext);
    }
};

ko.bindingHandlers.attr2 = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var options = valueAccessor();
        var attrName = options.name;
        var attrVal = ko.utils.unwrapObservable(options.value);
        var condition = ko.utils.unwrapObservable(options.on);
        if (condition) {
            $(element).attr(attrName, attrVal);
        } else {
            $(element).removeAttr(attrName);
        }
    }
};

ko.bindingHandlers.bool = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var observable = valueAccessor(),
            interceptor = ko.computed({
                read: function () {
                    var val = observable();
                    if (val) {
                        return val.toString();
                    }
                    return "";
                },
                write: function (newValue) {
                    observable(newValue === "true");
                }
            });

        ko.applyBindingsToNode(element, { value: interceptor });
    }
};

ko.mapping.makeChildObservalbe = function (obj) {
    var temp = obj;
    if (ko.isObservable(obj))
        temp = obj();
    if (typeof temp != "object")
        return;
    if (temp instanceof Array) {
        $.each(temp, function (i, n) {
            ko.mapping.makeChildObservalbe(n);
        });
    } else {
        for (prop in temp) {
            if (prop.indexOf('__') == 0)
                continue;
            var p = temp[prop];
            if (!ko.isObservable(p)) {
                temp[prop] = ko.observable(p);
            }
            ko.mapping.makeChildObservalbe(temp[prop]());
        }
    }
};

ko.mapping.viewModel = function (jsData, vmData) {
    if (jsData) {
        if (vmData) {
            if (!ko.isObservable(vmData)) {
                if (vmData instanceof Array) {
                    vmData = ko.observableArray(vmData);
                } else {
                    vmData = ko.observable(vmData);
                }
            }
            var data = ko.mapping.fromJS(jsData);
            ko.mapping.makeChildObservalbe(data);
            if (jsData instanceof Array) {
                ko.mapping.fromJS(data, null, vmData);
            } else {
                vmData(data);
            }
            return vmData;
        } else {
            var data = ko.mapping.fromJS(jsData);
            ko.mapping.makeChildObservalbe(data);
            return data;
        }
    }
};

var oldKOTOJS = ko.toJS;
ko.toJS = function (data) {
    var jsData = oldKOTOJS(data);
    for (f in jsData) {
        if (jsData.hasOwnProperty(f) && f.indexOf('__') == 0) {
            delete jsData[f];
        }
    }
    return jsData;
}

