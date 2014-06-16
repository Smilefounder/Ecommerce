
var dataState = {
    New: 1,
    Modified: 2,
    Deleted: 3,
    None: 0
}

function ObjectModel(options) {
    var self = this;
    self.options = $.extend({}, {
        trackDataChange: true         /*,
        getQueryUrl: function(self) { return ""; },
        getSaveUrl: function(self) { return ""; },
        onBeforeSend: function(xhf) {},
        onReceiveData: function(self, data) { return data; }
        onQuerySuccess: function(self, data) {},
        onQueryError: function(self, error) {},
        onSaveSuccess: function(self, info) {},
        onSaveError: function(self, error) {},
        onInit: function(self) {},
        onBind: function (self, data) {},
        onPropertyChanged: function (self, name, value, obj) { },
        validate: function(self) { return true; }        */
    }, options);

    self.data = ko.observable(null);
    self.trackChange = function (obj) {
        if (obj.dataState)
            return;
        for (var pn in obj) {
            self.trackPropertyChange(pn, obj);
        }
        obj.dataState = dataState.None;
    };

    self.trackPropertyChange = function (prop, obj) {
        var value = obj[prop];
        if (ko.isObservable(value)) {
            var val = value();
            if (val instanceof Array) {
                for (var i = 0; i < val.length; i++) {
                    self.trackChange(val[i]);
                }
            }
            value.subscribe(function (newValue) {
                if (self.options.onPropertyChanged) {
                    self.options.onPropertyChanged(self, prop, newValue, obj);
                }
                if (obj.dataState == dataState.None) {
                    obj.dataState = dataState.Modified;
                }
            }, self);
        }
    };
    self.bindData = function (odata, onBind) {
        if (self.options.bindData) {
            self.options.bindData(self, odata);
        } else {
            ko.mapping.viewModel(odata, self.data);
        }
        if (self.options.trackDataChange) {
            self.trackChange(self.data());
        }
        if (onBind) {
            onBind(self, self.data);
        } else if (self.options.onBind) {
            self.options.onBind(self, self.data);
        }
    };
    self.getData = function (url, jdata, onSuccess, onError, onReceiveData, onSend) {
        if (!url) url = self.options.getQueryUrl(self);
        return utils.getJson(url, jdata, function (odata) {
            if (onReceiveData) {
                odata = onReceiveData(self, odata) || odata;
            }
            else if (self.options.onReceiveData) {
                odata = self.options.onReceiveData(self, odata) || odata;
            }
            if (!odata) {
                var error = "no data!";
                if (onError) {
                    onError(error);
                } else if (self.options.onQueryError) {
                    self.options.onQueryError(self, error);
                } else {
                    utils.showMessage('error occurs', error + '\r\n' + url, 'error');
                }
                return false;
            }
            if (odata.status && odata.message && odata.status > 0) {
                if (onError) {
                    onError(self, odata.message);
                } else if (self.options.onQueryError) {
                    self.options.onQueryError(self, odata.message);
                } else {
                    utils.showMessage('error occurs', odata.message + '\r\n' + url, 'error');
                }
                return false;
            }

            self.bindData(odata.data || odata);

            if (onSuccess) {
                onSuccess(self, self.data);
            } else if (self.options.onQuerySuccess) {
                self.options.onQuerySuccess(self, self.data);
            }
        }, function (error, url) {
            if (onError) {
                onError(self, error);
            } else if (self.options.onQueryError) {
                self.options.onQueryError(self, error);
            } else {
                utils.showMessage('error occurs', error + '\r\n' + url, 'error');
            }
        }, onSend || self.options.onBeforeSend);
    };
    self.validate = function () {
        if (self.options.validate) {
            return self.options.validate(self);
        }
        return true;
    };
    self.saveData = function (url, beforeSave, onSuccess, onError, onReceiveData, onSend) {
        if (!self.validate()) {
            return false;
        }
        if (!url) url = self.options.getSaveUrl(self);
        var cdata = ko.toJS(self.data);
        if (beforeSave) {
            cdata = beforeSave(self, cdata);
        } else if (self.options.beforeSave) {
            cdata = self.options.beforeSave(self, cdata);
        }
        if (!cdata) { return false; }
        utils.postJson(url, cdata, function (info) {
            if (onReceiveData) {
                info = onReceiveData(self, info) || info;
            }
            else if (self.options.onReceiveData) {
                info = self.options.onReceiveData(self, info) || info;
            }
            if (info.data) {
                self.bindData(info.data, null);
            }
            if (info.status && info.message && info.status > 0) {
                if (onError) {
                    onError(self, info.message);
                } else if (self.options.onSaveError) {
                    self.options.onSaveError(self, info.message);
                } else {
                    utils.showMessage('error occurs', info.message + '\r\n' + url, 'error');
                }
                return false;
            }
            if (onSuccess) {
                onSuccess(self, info);
            } else if (self.options.onSaveSuccess) {
                self.options.onSaveSuccess(self, info);
            } else {
                utils.showMessage('info', info.message, 'info');
            }
        },
        function (error) {
            utils.showMessage('error occurs', error + '\r\n' + url, 'error');
            if (onError) {
                onError(self, error);
            } else if (self.options.onSaveError) {
                self.options.onSaveError(self, error);
            }
        }, onSend || self.options.onBeforeSend);
    };

    if (self.options.onInit) {
        self.options.onInit(self);
    }
};

function ListModel(options) {
    var self = this;
    self.options = $.extend({}, {
        trackDataChange: true,
        clearChangesWhenBindData: true        /*,
        getQueryUrl: function(self) { return ""; },
        getSaveUrl: function(self) { return ""; },
        onBeforeSend: function(xhf) {},
        onReceiveData: function(self, data) { return data; }
        onQuerySuccess: function(self, data) {},
        onQueryError: function(self, error) {},
        onSaveSuccess: function(self, info) {},
        onSaveError: function(self, error) {},
        onInit: function(self) {},
        createItem: function (self) { return {}; },
        canAdd: function(self) { return true; },
        canEdit: function(self, item) { return true; },
        canRemove: function(self, item) { return true; },
        onAdd: function(self, item) {},
        onEdit: function(self, item) {},
        onRemove: function(self, item) {},
        onBind: function (self, data) {},
        onPropertyChanged: function (self, name, value, obj) { },
        validate: function(self) { return true; }       */
    }, options);

    self.data = ko.observableArray([]);
    self.editingItem = ko.observable(null);
    self.selectedItem = ko.observable(null);
    self.selectedItems = ko.observableArray([]);
    self.newItems = ko.observableArray([]);
    self.modifiedItems = ko.observableArray([]);
    self.deletedItems = ko.observableArray([]);

    self._lastAddedItem = null;
    self._lastEditedItem = null;
    self._lastDeletedItem = null;
    self.trackChange = function (obj) {
        if (obj.dataState)
            return;
        for (var pn in obj) {
            self.trackPropertyChange(pn, obj);
        }
        obj.dataState = dataState.None;
    };

    self.trackPropertyChange = function (prop, obj) {
        var value = obj[prop];
        if (ko.isObservable(value)) {
            var val = value();
            if (val instanceof Array) {
                for (var i = 0; i < val.length; i++) {
                    self.trackChange(val[i]);
                }
            }
            value.subscribe(function (newValue) {
                if (self.options.onPropertyChanged) {
                    self.options.onPropertyChanged(self, prop, newValue, obj);
                }
                if (obj.dataState == dataState.None) {
                    obj.dataState = dataState.Modified;
                    self.modifiedItems.push(obj);
                }
            }, self);
        }
    };
    self.selectItem = function (item) {
        self.selectedItems.push(item);
    };

    self.unselectItem = function (item) {
        self.selectedItems.remove(item);
    };

    self.clearSelection = function () {
        self.selectedItems.clear();
    };

    self.isNew = function (item) {
        return item && item.dataState == dataState.New;
    };

    self.isModified = function (item) {
        return item && item.dataState == dataState.Modified;
    };

    self.isDeleted = function (item) {
        return item && item.dataState == dataState.Deleted;
    };

    self.addItem = function (item) {
        if (self.options.canAdd) {
            if (!self.options.canAdd()) {
                return false;
            }
        }
        if (!item) item = self.options.createItem(self);
        item.dataState = dataState.New;
        self.newItems.push(item);
        self.data.push(item);
        self.editingItem(item);
        self._lastAddedItem = item;
        if (self.options.onAdd) {
            self.options.onAdd(self, item);
        }
    };

    self.undoAddItem = function () {
        if (self._lastAddedItem) {
            var item = self._lastAddedItem;
            self.newItems.remove(item);
            self.data.remove(item);
            self._lastAddedItem = null;
        }
    };

    self.editItem = function (item) {
        if (self.options.canEdit) {
            if (!self.options.canEdit(self, item)) {
                return false;
            }
        }
        self.editingItem(item);
        self._lastEditedItem = ko.toJS(item);
        if (self.options.onEdit) {
            self.options.onEdit(self, item);
        }
    };

    self.undoEditItem = function () {
        if (self._lastEditedItem) {
            var item = self.editingItem();
            self.modifiedItems.remove(item);
            for (var pn in self._lastEditedItem) {
                if (typeof (item[pn]) == "function") {
                    item[pn](self._lastEditedItem[pn]);
                }
            }
            item.dataState = dataState.None;
            self.modifiedItems.remove(item);
            self._lastEditedItem = null;
        }
    };

    self.removeItem = function (item) {
        if (self.options.canRemove) {
            if (!self.options.canRemove(self, item)) {
                return false;
            }
        }
        utils.showConfirm("info", "confirm delete this item?", function () {
            self.data.remove(item);
            if (self.isNew(item)) {
                self.newItems.remove(item);
            } else {
                self.deletedItems.push(item);
            }
            self._lastDeletedItem = item;
            if (self.editingItem() == item) {
                self.editingItem(null);
            }
            self.unselectItem(item);
            if (self.options.onRemove) {
                self.options.onRemove(self, item);
            }
        });
    };

    self.undoRemoveItem = function () {
        if (self._lastDeletedItem) {
            var item = self._lastDeletedItem;
            if (item.IsNew) {
                self.newItems.push(item);
            }
            else {
                item.dataState = dataState.None;
                self.data.push(item);
            }
            self._lastDeletedItem = null;
        }
    };
    self.getChangedData = function () {
        return {
            newItems: ko.mapping.toJS(self.newItems),
            modifiedItems: ko.mapping.toJS(self.modifiedItems),
            deletedItems: ko.mapping.toJS(self.deletedItems)
        };
    };
    self.bindData = function (odata, bindData, onBind) {
        if (self.options.clearChangesWhenBindData) {
            self.newItems().clear();
            self.modifiedItems().clear();
            self.deletedItems().clear();
        }
        if (bindData) {
            bindData(self, odata);
        } else if (self.options.bindData) {
            self.options.bindData(self, odata);
        } else {
            ko.mapping.viewModel(odata, self.data);
        }
        if (self.options.trackDataChange) {
            for (var i = 0; i < self.data().length; i++) {
                var obj = self.data()[i];
                self.trackChange(obj);
            }
        }
        if (onBind) {
            onBind(self, self.data);
        } else if (self.options.onBind) {
            self.options.onBind(self, self.data);
        }
    };
    self.getData = function (url, jdata, onSuccess, onError, onReceiveData, onSend) {
        if (!url) url = self.options.getQueryUrl(self);
        utils.getJson(url, jdata, function (odata) {
            if (onReceiveData) {
                odata = onReceiveData(self, odata) || odata;
            }
            else if (self.options.onReceiveData) {
                odata = self.options.onReceiveData(self, odata) || odata;
            }
            if (!odata) {
                var error = "no data!";
                if (onError) {
                    onError(self, error);
                } else if (self.options.onQueryError) {
                    self.options.onQueryError(self, error);
                } else {
                    utils.showMessage('error occurs', error + '\r\n' + url, 'error');
                }
                return false;
            }
            if (odata.status && odata.message && odata.status > 0) {
                if (onError) {
                    onError(self, odata.message);
                } else if (self.options.onQueryError) {
                    self.options.onQueryError(self, odata.message);
                } else {
                    utils.showMessage('error occurs', odata.message + '\r\n' + url, 'error');
                }
                return false;
            }

            self.bindData(odata.data || odata);

            if (onSuccess) {
                onSuccess(self, self.data);
            } else if (self.options.onQuerySuccess) {
                self.options.onQuerySuccess(self, self.data);
            }
        }, function (error, url) {
            if (onError) {
                onError(self, error);
            } else if (self.options.onQueryError) {
                self.options.onQueryError(self, error);
            } else {
                utils.showMessage('error occurs', error + '\r\n' + url, 'error');
            }
        }, onSend || self.options.onBeforeSend);

    };
    self.validate = function () {
        if (self.options.validate) {
            return self.options.validate(self);
        }
        return true;
    };
    self.saveData = function (url, beforeSave, onSuccess, onError, onReceiveData, onSend) {
        if (!self.validate()) {
            return false;
        }
        if (!url) url = self.options.getSaveUrl(self);
        var cdata = self.getChangedData();
        if (beforeSave) {
            cdata = beforeSave(self, cdata);
        } else if (self.options.beforeSave) {
            cdata = self.options.beforeSave(self, cdata);
        }
        if (!cdata) { return false; }
        utils.postJson(url, cdata, function (info) {
            if (onReceiveData) {
                info = onReceiveData(self, info) || info;
            }
            else if (self.options.onReceiveData) {
                info = self.options.onReceiveData(self, info) || info;
            }
            if (info.data) {
                self.bindData(info.data, null);
            }
            if (info.status && info.message && info.status > 0) {
                if (onError) {
                    onError(info.message);
                } else if (self.options.onSaveError) {
                    self.options.onSaveError(self, info.message);
                } else {
                    utils.showMessage('error occurs', info.message + '\r\n' + url, 'error');
                }
                return false;
            }
            if (onSuccess) {
                onSuccess(self, info);
            } else if (self.options.onSaveSuccess) {
                self.options.onSaveSuccess(self, info);
            } else {
                utils.showMessage('info', info.message, 'info');
            }
            self.newItems().clear();
            self.modifiedItems().clear();
            self.deletedItems().clear();
        },
        function (error) {
            utils.showMessage('error occurs', error + '\r\n' + url, 'error');
            if (onError) {
                onError(error);
            } else if (self.options.onSaveError) {
                self.options.onSaveError(self, error);
            }
        }, onSend || self.options.onBeforeSend);
    };

    if (self.options.onInit) {
        self.options.onInit(self);
    }
};

function PagedListModel(options) {
    var self = this;
    self.options = $.extend({}, {
        pageIndex: 0,
        pageSize: 50    /*,
        onGotoPage: function (pi, ps) {}    */
    }, options);
    ListModel.prototype.constructor.call(self, options);
    self.pageInfo = ko.observable({
        pageIndex: ko.observable(self.options.pageIndex),
        pageSize: ko.observable(self.options.pageSize),
        startPosition: ko.observable(1),
        endPosition: ko.observable(1),
        totalItemCount: ko.observable(0),
        totalPageCount: ko.observable(0)
    });
    self.filters = ko.observable(null);
    self.getPages = function (pageCount) {
        if (!pageCount)
            pageCount = 10;
        var pinfo = self.pageInfo();
        if (pinfo) {
            var spi = pinfo.pageIndex() - Math.ceil(pageCount / 2);
            if (spi < 0) { spi = 0 }
            var epi = pinfo.pageIndex() + Math.ceil(pageCount / 2);
            if (epi > pinfo.totalPageCount() - 1) { epi = pinfo.totalPageCount() - 1; }
            var pages = [];
            for (var i = spi; i <= epi; i++) {
                pages.push(i);
            }
            return pages;
        }
        return [];
    };
    self.isFirstPage = function () {
        return self.pageInfo().pageIndex() <= 0;
    };
    self.isLastPage = function () {
        return self.pageInfo().pageIndex() >= self.pageInfo().totalPageCount() - 1;
    };
    self.isCurrentPage = function (p) {
        return self.pageInfo().pageIndex() == p;
    };
    self.gotoPage = function (op) {
        var pidx = self.pageInfo().pageIndex();
        if (typeof (op) == "number") {
            pidx = op;
        } else if (typeof (op) == "object") {
            if (op.page) {
                pidx = op.page;
            } else if (op.offset) {
                pidx += op.offset;
            }
        }
        if (pidx > self.pageInfo().totalPageCount() - 1) {
            pidx = self.pageInfo().totalPageCount() - 1;
        }
        if (pidx < 0) {
            pidx = 0;
        }
        if (pidx == self.pageInfo().pageIndex()) {
            return false;
        }
        if (self.options.onGotoPage) {
            return onGotoPage(pidx, self.pageInfo().pageSize());
        } else {
            self.getData(pidx);
        }
    };

    self.bindData = function (odata, onBind) {
        if (odata.pageInfo) {
            if (self.options.appendPagingData) {
                if (self.pageInfo()) {
                    odata.pageInfo.startPosition = self.pageInfo().startPosition || 1;
                } else {
                    odata.pageInfo.startPosition = 1;
                }
            }
            ko.mapping.viewModel(odata.pageInfo, self.pageInfo);
        }
        if (odata.filters) {
            ko.mapping.viewModel(odata.filters, self.filters);
        }
        self.__proto__.bindData.call(self, odata, function (udata) {
            if (self.options.appendPagingData && self.data() && self.data().length > 0) {
                for (var i = 0; i < udata.length; i++) {
                    self.data.push(ko.mapping.viewModel(udata[i]));
                }
            } else {
                ko.mapping.viewModel(udata, self.data);
            }
            if (onBind) {
                onBind(self, self.data);
            } else if (self.options.onBind) {
                self.options.onBind(self, self.data);
            }
        });
    };

    self.getData = function (pi, ps, url, onSuccess, onError, onReceiveData, onSend) {
        if (!url) url = self.options.getQueryUrl(self);
        url = utils.appendUrl(url, 'pi', pi || self.options.pageIndex);
        url = utils.appendUrl(url, 'ps', ps || self.options.pageSize);
        var filterData = null;
        if (self.filters) {
            filterData = ko.mapping.toJS(self.filters);
            for (var n in filterData) {
                if (filterData[n] && typeof (filterData[n].toJSON) == "function") {
                    filterData[n] = filterData[n].toJSON();
                }
            }
        }
        self.__proto__.getData.call(self, url, filterData, onSuccess, onError, onReceiveData, onSend);
    };
};

PagedListModel.prototype = new ListModel();

function afterRender(eles, data) {
    var rowSuffix = $.now().toString();
    var form = $(eles).eq(0).closest('form');
    form.find('input:not([type="hidden"]), select').each(function () {
        $(this).attr('id', $(this).attr('id') + rowSuffix);
        $(this).attr('name', $(this).attr('name') + rowSuffix);
        $(this).next().attr('data-valmsg-for', $(this).next().attr('data-valmsg-for') + rowSuffix);
    });
    utils.enableFormValidation(form);
}