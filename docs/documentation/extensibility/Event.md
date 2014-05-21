# 事件 #

## 定义事件 ##

Kooboo Commerce中的事件由普通CLR对象定义，事件对象即事件消息裁体，类似于`EventArgs`。

自定义事件可从`Kooboo.Commerce.Events.Event`类继承，如果一个事件和业务逻辑相关，则应从`Kooboo.Commerce.Events.DomainEvent`类继承，以清晰的表示其为领域事件(`Activity`只能绑定到领域事件中)。

```csharp

	public class OrderPaid: Event 
	{
		public int OrderId { get; set; }
	}

```

领域事件类的名称本身是业务语言的一部分，应能表达业务信息。事件名称以过去式命名，因为事件都是表示已经发生的事情。不建议添加多余的`Event`后缀。

事件类可能被序列化，因此不能在事件类中直接嵌入复杂对象，尤其有循环引用的对象。

领域事件可以使用`Kooboo.Commerce.Events.CategoryAttribute`进行标注，以对事件进行分组，后台Activity管理界面中将根据该标注对领域事件进行分组显示。

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

## 特殊事件 ##

实体的创建、删除以及更新是非常常见的事件，因此这三类事件有被特殊对待，如果一个实体需要创建事件，除了用标准的方法在实体保存的地方触发相应事件外，还可以实现`Kooboo.Commerce.ComponentModel.INotifyCreated`接口，例如:

```csharp

	public class Order : INotifyCreated 
	{
		void INotifyCreated.NotifyCreated()
		{
			Event.Raise(new OrderCreated(this));
		}
	}

```

若实体实现了`INotifyCreated`，则在保存到Repository时其`NotifyCreated`方法会被自动调用，进而触发`OrderCreated`事件。

不使用通用的`EntityCreated`事件是因为`EntityCreated`太过于技术化，命名没有业务价值，也不方便Activity的绑定。

为了不污染实体，建议在实体中显式实现`INotifyCreated`。

类似的，实体还可以实现`INotifyUpdated`, `INotifyDeleting` 以及 `INotifyDeleted`。

不建议过多使用后三者，尤其`INotifyUpdated`，应该尽量使用更具业务含义的操作，用更具业务含义的操作也会使其它模块的开发变得更加简单，例如，如果Activity订阅到了`OrderUpdated`事件，那它仍然无法下手，因为Update有太多可能，例如Update可能是因为地址变更，也可能是因为订单状态变更，等。而如果Activity订阅到的是`BillingAddressChanged`，或`OrderStatusChanged`，则Activity的开发会相对更容易一些。

需要注意的是，子集成对象添加和删除并不会导致上面所述的这些事件的触发，子集合对象的添加和删除若需要触发事件，应在其父对象中用相应的业务方法进行体现。若对应DDD中的聚合模式，则可以认为，`INotifyXXX`接口仅作用于聚合根。