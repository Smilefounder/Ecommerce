# 条件表达式 #

在 Activity 以及 Promotion 中都用到了条件表达式，用户可以对一个操作添加条件，并设置相关参数值，最终系统会生成相应的条件表达式在数据库中用于运行时判断条件是否满足。

每个条件表达式都要求一个特定的上下文（由一个CLR对象来表示），例如 Activity 模块中 Rule 的条件部分的上下文就是各个事件对象。而不同的上下文则决定了该条件表达式中有哪些参数是可用的，例如`OrderCreated`上下文中，`OrderId`, `OrderTotal`等是可用的参数，`BrandCreated`上下文中，`BrandId`, `BrandName`等是可用的参数。

## 客户端调用方式示例 ##

```csharp

	// 定义上下文类型
	public class OrderCreated
	{
		[Param]
		public int OrderId { get; set; }
		
		[Param]
		public string CustomerEmail { get; set; }
	}
	
	// 构建表达式
	var expression = "OrderId == 1024 OR CustomerEmail contains \"kooboo.com\"";
	// 创建上下文对象
	var context = new OrderCreated
	{
		OrderId = 5,
		CustomerEmail = "me@kooboo.com"
	}
	
	// 创建RuleEngine实例
	var ruleEngine = new RuleEngine();
	// 检查当前上下文是满可满足条件要求（本示例结果为 true）
	var success = ruleEngine.CheckCondition(expression, context);

```

## 通过Attribute定义可用参数 ##

最简单的方式是采用`Kooboo.Commerce.Rules.ParamAttribute`来标记上下文对象中的某个属性可用作条件表达式的参数，例如:

```csharp

	public class OrderCreated : DomainEvent
	{
		[Param]
		public int OrderId { get; set; }
	}
```

在`OrderId`上使用`ParamAttribute`标记后，`OrderCreated`上下文中的条件表达式就可以使用`OrderId`这个参数，因此可以在该上下文中使用例如这样的表达式: `OrderId == 500`。而使用`OrderId == 500 AND BrandId == 5`将会报错，因为当前上下文中`BrandId`参数不可用。

### 处理直接/间接对象引用 ###

因为事件要保证方便序列化，因此不会嵌套完整的业务对象，如果只通过`ParamAttribute`来定义可用参数显得不太方便。在这种情况下，可考虑使用`Kooboo.Commerce.Rules.ReferenceAttribute`定义对象间的间接引用（`OrderCreated`通过`OrderId`来关联`Order`即为**间接引用**，而`Product`通过`Brand`来关联`Brand`为**直接引用**）。

因此，上面的`OrderCreated`可以改造为:

```csharp

	public class OrderCreated : DomainEvent
	{
		[Reference(typeof(Order))]
		public int OrderId { get; set; }
	}
	
	public class Order 
	{
		[Param]
		public int Id { get; set; }
	
		[Param]
		public decimal Total { get; set; }
	}
```

这样，`OrderCreated`上下文中便可以使用`OrderId`, `OrderTotal`这两个参数，运行时会自动通过`IRepository<Order>`获取`Order`实例来获得`OrderId`及`OrderTotal`的值。

如果**间接引用**并非数据库实体间的引用，则可以通过`Kooboo.Commerce.Rules.IReferenceResolver`来自定义，实现者应在其`Resolve`方法中根据关联Id返回被引用对象的实例。然后还需要在`ReferenceAttribute`中设置`ReferenceResolver`，例如:

```csharp

	[Reference(typeof(MyNonDbObject), typeof(MyNonDbObjectReferenceResolver))]
	public int NonDbObjectId { get; set; }
```

如果是直接对象引用，也应当标记`ReferenceAttribute`，否则运行时将不会检查该引用对象中的可用参数。

当使用`ReferenceAttribute`时，会自动使用属性名作为被引用类型中的参数的前缀，例如:

```csharp

	pulibc class Order
	{
		[Reference]
		public OrderAddress BillingAddress { get; set; }

		[Reference]
		public OrderAddress ShippingAddress { get; set; }
	}

	public class OrderAddress 
	{
		[Param]
		public string City { get; set; }
	}

```

`Order`中使用了`ReferenceAttribute`，它们会分别默认使用`BillingAddress`和`ShippingAddress`作为前缀，因此在`Order`上下文中的可用参数为 `BillingAddressCity`和`ShippingAddressCity`，如果想自定义前缀，可以设置`RefernceAttribute.Prefix`属性，例如:

```csharp

	pulibc class Order
	{
		[Reference(Prefix = "Billing")]
		public OrderAddress BillingAddress { get; set; }

		[Reference(Prefix = "Shipping")]
		public OrderAddress ShippingAddress { get; set; }
	}
```

这样`Order`上下文的可用参数就变成了`BillingCity`和`ShippingCity`。

### 自定义参数可选值列表 ###

默认情况下参数值在UI中会显示TextBox输入框，如果参数只有固定几个值，可以通过`Kooboo.Commerce.Rules.IParameterValueSource`来定义参数可用值列表（枚举类型已默认处理，无需对其再自定义），实现者应在其`GetValues`中返回可用的参数值，然后在`ParamAttribute`中设置`ValueSource`为`IParameterValueSource`的类型。这样在UI中该参数值就会变成DropDownList。

## 动态构建可用参数 ##

除了使用Attribute，还可以使用代码的方式来动态构建上下文的可用参数，这在为开发者无法修改的上下文类型(如主系统中定义的上下文类型)添加参数时比较有用。实现者应实现`Kooboo.Commerce.Rules.IParameterProvider`接口，例如:

```csharp

	[Dependency(typeof(IParameterProvider), Key = "MyParameterProvider")]
	public class MyParameterProvider : IParameterProvider
	{
		public IEnumerable<ConditionParameter> GetParameters(Type dataContextType)
		{
			// 动态创建ConditionParameter并返回
			yield return new ConditionParameter(...);
		}
	}
```

注意使用Kooboo CMS的`DependencyAttribute`对自定义的`IParameterProvider`实现进行注册。