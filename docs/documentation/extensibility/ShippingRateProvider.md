# Shipping Rate Provider #

`Kooboo.Commerce.Shipping.IShippingRateProvider`用于提供订单运费计算的扩展性，实现自定义的运费规则时应实现该接口:

```csharp

    /// <summary>
    /// 定义运费计算接口。
    /// </summary>
    public interface IShippingRateProvider
    {
        string Name { get; }

        /// <summary>
        /// 计算运费。
        /// </summary>
        decimal GetShippingRate(ShippingMethod method, ShippingRateCalculationContext context);

        /// <summary>
        /// 获取自定义配置的编辑器信息。
        /// </summary>
        ShippingRateProviderEditor GetEditor();
    }

```

其中`GetShippingRate`方法应执行运费计算逻辑，并将计算结果返回。

如果该插件需要自定义配置界面，则`GetEditor`方法应返回`ShippingRateProviderEditor`实例，并指定对应的View路径，否则返回`null`。

开发配置编辑的View时，可通过JS API与主系统页面进行交互，和其它插件的扩展点类似，例如:

```html

	<script>
		(function($) {
			// 获取ShippingRateProviderEditor实例
	        var editor = ShippingRateProviderEditor.current();
	
	        editor.on('load', function (sender, args) {
	            var shippingMethodId = args.shippingMethodId;
	            // 此处根据shippingMethodId加载自定义配置数据并自行初始化表单
	        });
	
	        editor.on('saving', function (sender, args) {
				var shippingMethodId = args.shippingMethodId;
				// 此时保存自定义数据，可返回promise
	        });
	
	    })(jQuery);
	</script>

```