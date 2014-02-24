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

})(jQuery);