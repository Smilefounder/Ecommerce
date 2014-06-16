# 事件 #

## 定义事件 ##

Kooboo Commerce中的事件由普通CLR对象定义，事件对象即事件消息裁体，类似于`EventArgs`。

自定义事件可从`Kooboo.Commerce.Events.Event`类继承，如果一个事件和业务逻辑相关，则应从`Kooboo.Commerce.Events.BusinessEvent`类继承，以清晰的表示其为业务事件(`Activity`只能绑定到业务事件中)。

```csharp

	public class OrderPaid: Event 
	{
		public int OrderId { get; set; }
	}

```

业务事件类的名称本身是业务语言的一部分，应能表达业务信息。事件名称以过去式命名，因为事件都是表示已经发生的事情。

事件类可能被序列化，因此不能在事件类中直接嵌入复杂对象，尤其有循环引用的对象。

领域事件可以使用`Kooboo.Commerce.Events.EventAttribute`以及`Kooboo.Commerce.Events.CategoryAttribute`添加一些描述信息，例如分组，以及显示顺序。

**事件序列化**

很多Activity要求可以做到延后定时执行，为此，需要在事件触发时序列化事件对象并临时保存在数据库中。像价格计算之类的事件，需要引用临时存在的`PricingContext`，`PricingContext`不方便序列化，因此Event Handler要通过`PricingContext.GetCurrent()`的方式来获取当前的价格计算上下文，而不能从事件对象中获取。虽然我们可以通过某种其它方式来区别对象可序列化和不可序列化的事件，但这样显得有点复杂，对基础层的处理来说，把所有事件都默认当成可序列化的是最简单的(详情见Design Notes)。

## 订阅事件 ##

要订阅一个事件，需要添加一个实现了 `Kooboo.Commerce.Events.IHandle<YourEventType>` 接口的类。例如

```csharp

	public class OrderPaidEventHandler : IHandle<OrderPaid>
	{
		public void Handle(OrderPaid @event)
		{
			// 处理事件
		}
	}

```

当`OrderPaid`事件发生时，上面的`EventHandler`将会被执行。

如果要订阅多个事件，可以添加多个`EventHandler`，也可以在一个`EventHandler`中实现多个`IHandle<TEvent>`接口，例如

```csharp

	public class OrderEventHandlers : IHandle<OrderCreated>, IHandle<OrderPaid>
	{
		public void Handle(OrderCreated @event)
		{
			// Order created
		}

		public void Handle(OrderPaid @event)
		{
			// Order paid
		}
	}

```

事件处理也支持事件继承，如果订阅了一个事件基类，则当所有子类事件触发时，该`EventHandler`都会被执行，例如:

```csharp

    public abstract class OrderEvent : BusinessEvent { }

	public class OrderCreated : OrderEvent { }

	public class OrderPaid : OrderEvent { }

	public class OrderEventHandlers : IHandle<OrderEvent>
	{
		public void Handle(OrderEvent @event)
		{
			if (@event is OrderCreated) 
			{
				// Handle order created
			} else if (@event is OrderPaid) 
			{
				// Handle order paid
			}
		}
	}

```

## 模块集成 ##

当两个自治模块需要集成时，可以使用`ProcessManager`模式来集成，例如，当订单支付后，库存需要更新，当订单和库存为两个自治模块时，订单不依赖于库存，无法在订单支付的代码中更新库存，此时库存流程处理中可以订阅订单模块的事件来集成，例如:

```csharp

	public class InventoryProcessManager : IHandle<OrderPaid>, IHandle<OrderReturned>
	{
		public void Handle(OrderPaid @event)
		{
			// 减少库存
		}
	
		public void Handle(OrderReturned @event)
		{
			// 增加库存
		}
	}

```