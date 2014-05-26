# Payment Processor #

PaymentProcessor用于处理支付请求，添加PaymentMethod时需要指定对应的PaymentProcessor。

## 添加自定义的PaymentProcessor ##

自定义的PaymentProcessor需要实现`Kooboo.Commerce.Payments.IPaymentProcessor`接口:

```csharp

    /// <summary>
    /// 定义支付处理接口。
    /// </summary>
    public interface IPaymentProcessor
    {
        /// <summary>
        /// 处理器惟一名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 处理支付请求，并返回结果。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ProcessPaymentResult Process(ProcessPaymentRequest request);
        
        /// <summary>
        /// 获取自定义配置数据的编辑器信息。
        /// </summary>
        PaymentProcessorEditor GetEditor(PaymentMethod method);
    }

```

用户选择PaymentMethod并支付订单时，相关数据会提交到服务器，进而调用相应的`IPaymentProcessor`的`Process`方法，PaymentProcessor的实现者应对支付请求进行处理，然后返回`ProcessPaymentResult`实例，并指定处理后的支付状态及其它信息（例如，可指定下一步要跳转的URL地址，用于需要跳转到第三方支付网关支付的场景）。

例如一个简单的实现可以是:

```csharp

	public ProcessPaymentResult Process(ProcessPaymentRequest request)
	{
		var paymentId = request.Payment.Id;
		var amount = request.Amount;
		var redirectUrl = "http://your_payment_gateway_url?amount=" 
			+ amount + "&paymentId=" + paymentId;

		return ProcessPaymentResult.Pending(redirectUrl);
	}

```

`GetEditor`返回用于配置该PaymentProcessor的自定义数据的View的信息，和Activity, Promotion等类似。在该View中，通过JS API与主系统界面进行交互。例如:

```html

	<script>
	    (function ($) {
			// 获取PaymentProcessorEditor实例
	        var editor = PaymentProcessorEditor.current();
	
	        editor.on('load', function (sender, args) {
	            var methodId = args.paymentMethodId;
				// 这里加载自定义数据并初始化表单
	        });
	
	        editor.on('saving', function (sender, args) {
	            var methodId = args.paymentMethodId;
	
				// 这里保存自定义数据，可返回promise
	        });
	    })(jQuery);
	</script>

```

和其它的客户端扩展类似，所有事件处理函数都可以返回`promise`，当返回`promise`时，主系统页面会等待其完成才会继续下一步操作。