# Promotion Policy #

PromotionPolicy表示一种促销策略，例如给商品打8折是一种促销策略，赚送一本杂志也是一种促销策略。添加Promotion时可定义该Promotion要满足的条件，该条件的定义由Conditions模块处理，这里只介绍PromotionPolicy的扩展。

PromotionPolicy的扩展同样分为两部分，业务逻辑执行部分，以及后台PromotionPolicy自定义数据配置的View。

## 实现自定义的PromotionPolicy ##

实现一个新的PromotionPolicy应实现`Kooboo.Commerce.Promotions.IPromotionPolicy`接口，接口定义如下:

```csharp

    /// <summary>
    /// 定义一种促销策略。
    /// </summary>
    public interface IPromotionPolicy
    {
        /// <summary>
        /// 策略名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 执行促销策略。
        /// </summary>
        /// <param name="context"></param>
        void Execute(PromotionContext context);

        /// <summary>
        /// 获取促销策略配置的编辑器。
        /// </summary>
        PromotionPolicyEditor GetEditor();
    }

```

其中`GetEditor`方法应返回`PromotionPolicyEditor`或`null`，当返回`null`时表示该PromotionPolicy不需要配置自定义数据。

`Execute`方法中应执行促销逻辑，例如，我们可以在总价格上添加折扣:

```csharp

	public class MyPromotionPolicy : IPromotionPolicy
	{
		public void Execute(PromotionContext context)
		{
			// 在Subtotal上添加5元折扣
			context.PricingContext.Subtotal.AddDiscount(5);
		}
	}

```

当PromotionPolicy需要自定义数据配置时，应在`GetEditor`方法中返回`PromotionPolicyEditor`实例，并将`VirtualPath`设置为对应的配置View。

实现配置View时，可通过JS API和主系统页面进行交互，例如:

```html

	<script>
	    (function ($) {
			// 获取PromotionPolicyEditor实例
	        var editor = PromotionPolicyEditor.current();
	
	        editor.on('load', function (sender, args) {
	            var promotionId = args.promotionId;
				// 根据promotionId从服务端加载自定义数据
	        });
	
	        editor.on('saving', function (sender, args) {
	            var promotionId = args.promotionId;
				
				// 将自定义表单数据保存到服务器，可以返回promise
				return $.post('....');
	
	        });
	    })(jQuery);
	</script>
```

和Activity类似，Promotion中的JS事件处理函数可以返回`promise`，当返回`promise`时，主系统页面会等待其完成后才会进行下一步操作。例如`saving`事件中，我们需要用ajax保存自定义数据到服务端，保存操作会有一定延迟，且有可能保存失败，因此我们可以在对应的事件处理函数中返回`promise`，这样主系统界面会一直等其完成后才会跳转到下一个页面。

## 注册PromotionPolicy ##

可使用`Kooboo.CMS.Common.Runtime.Dependency.DependencyAttribute`注册自定义的PromotionPolicy，例如:

```csharp

	[Dependency(typeof(IPromotionPolicy), Key = "MyPromotionPolicy")]
	public class MyPromotionPolicy : IPromotionPolicy
	{
		// ...
	}

```