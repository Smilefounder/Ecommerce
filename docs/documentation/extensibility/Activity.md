# Activity #

`Activity`的扩展分为两部分，一是`Activity`的执行，二是自定义的配置界面。

## 实现IActivity接口##

```csharp

    public interface IActivity
    {
        /// <summary>
        /// Activity的惟一名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 是否允许异步执行。
        /// </summary>
        bool AllowAsyncExecution { get; }

        /// <summary>
        /// 该Activity是否可以绑定到指定的事件。
        /// </summary>
        bool CanBindTo(Type eventType);

        /// <summary>
        /// 执行Activity。
        /// </summary>
        void Execute(IEvent @event, ActivityContext context);

        /// <summary>
        /// 获取Activity的配置编辑器。
        /// </summary>
        ActivityEditor GetEditor(ActivityRule rule, AttachedActivityInfo info);
    }

```

其中`Execute`方法用于执行`Activity`的核心逻辑。`GetEditor`方法应返回`ActivityEditor`实例，Kooboo Commerce通过`ActivityEditor`的信息来加载`Activity`配置页面。如果`Activity`不需要配置，则返回`null`。

如果 Activity 在执行过程中发现业务规则不符合要求，应抛出`Kooboo.Commerce.BusinessRuleViolationException`异常，该异常抛出后，后续的逻辑将不再执行，该异常将会在UI中被特殊处理，其它的异常被当作普通的服务器执行异常。

## 实现Activity配置页面 ##

Activity配置页面会以Tab的形式加载到Activity设置页面中，实现者添加一个Razor View，并将`IActivity`的`GetEditor`方法中返回的`ActivityEditor`实例的`VirtualPath`属性设置为该View的虚拟路径，例如:

```csharp

	public class MyActivity : IActivity
	{
		public ActivityEditor GetEditor(ActivityRule rule, AttachedActivityInfo info)
        {
            return new ActivityEditor("~/Areas/MyActivity/Views/Config.cshtml");
        }
	}

```

在Commerce后台添加Activity时，会弹出Activity配置对话框，第一个tab为所有Activity通用的基础信息设置，第二个tab为自定义的Activity配置View。实现Activity配置View时，可通过javascript接口来和系统中的相关View进行交互，例如当用户点击保存时，同时保存一些自定义数据。

示例Activity配置View代码：

```html

	<script>

	$(function () {

		// 获取ActivityEditor实例
		var editor = ActivityEditor.current();

		editor.on('databound', function (sender, args) {
			// 基础配置页已绑定好数据，此时可以加载自定义数据
		});
		editor.on('saving', function (sender, args) {
			// 验证自定义配置View中的表单
		});
		editor.on('saved', function (sender, args) {
			// 基础信息已保存，保存自定义数据
		});
	
	});

	</script>

```

`ActivityEditor`中的相关事件的处理函数可以不返回值，也可以返回`promise`，如果返回`promise`，则主系统页面会等待该`promise`完成后再继续执行，例如，当用户点击保存后，Activity基础信息会先保存，保存完毕后，会调用`saved`事件的处理函数，该处理函数中可以通过ajax保存自定义数据，并返回`promise`，则主系统页面会在自定义数据保存成功后关闭对话框。

## 注册Activity扩展 ##

可使用`Kooboo.CMS.Common.Runtime.Dependency.DepencencyAttribute`来注册`IActivity`扩展，例如:

```csharp

    [Dependency(typeof(IActivity), Key = "Invoice Reminder")]
    public class InvoiceReminderActivity : IActivity
	{
		// ...
	}

```