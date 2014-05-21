# 事件 #

## 定义事件 ##

Kooboo Commerce中的事件由普通CLR对象定义，事件对象即事件消息裁体，类似于`EventArgs`。

自定义事件可从`Kooboo.Commerce.Events.Event`类继承，如果一件事件和业务逻辑相关，则应从`Kooboo.Commerce.Events.DomainEvent`类继承，以清晰的表示其为领域事件(`Activity`只能绑定到领域事件中)。

	public class OrderPaid: Event 
	{
		public int OrderId { get; set; }
	}

领域事件类的名称本身是业务语言的一部分，应能表达业务信息。事件名称以过去式命名，因为事件都是表示已经发生的事情。不建议添加多余的`Event`后缀。

事件类可能被序列化，因此不能在事件类中直接嵌入复杂对象，尤其有循环引用的对象。

## 订阅事件 ##

要订阅一个事件，需要添加一个实现了 `Kooboo.Commerce.Events.IHandle<YourEventType>` 接口的类。例如

	public class OrderPaidEventHandler : IHandle<OrderPaid>
	{
		public void Handle(OrderPaid @event)
		{
			// 处理事件
		}
	}

当`OrderPaid`事件发生时，上面的`EventHandler`将会被执行。

如果要订阅多个事件，可以添加多个`EventHandler`，也可以在一个`EventHandler`中实现多个`IHandle<TEvent>`接口，例如

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

事件处理也支持事件继承，如果订阅了一个事件基类，则当所有子类事件触发时，该`EventHandler`都会被执行，例如:

    public abstract class OrderEvent : DomainEvent { }

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

## 模块集成 ##

当两个自治模块需要集成时，可以使用`ProcessManager`模式来集成，例如，当订单支付后，库存需要更新，当订单和库存为两个自治模块时，订单不依赖于库存，无法在订单支付的代码中更新库存，此时库存流程处理中可以订阅订单模块的事件来集成，例如:

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
