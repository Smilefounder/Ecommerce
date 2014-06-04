# PricingContext #

`Kooboo.Commerce.Orders.Pricing.PricingContext`是一个用来表示价格计算上下文的对象，里面包含了一些价格计算的中间过程，如果在价格计算过程中通过Addin修改了其内容，那最终订单的价格也会被修改。

当编写价格计算相关事件的Event Handler时，经常要获得当前`PricingContext`的实例，为了达到这个目的，最直观的办法就是把`PricingContext`添加为相应事件类的属性。

但事件有可能被序列化，而且九成的事件有可能会被序列化保存在数据库（为了延后定时执行Activity），为了序列化，就不合适把`PricingContext`添加为事件类的属性，因为`PricingContext`只有计算价格的那一小会儿时间内才有效，另外`PricingContext`类本身不方便序列化。

但作为价格计算相关的Event Handler，又会希望直接在事件类中“点”出来`PricingContext`，因为这对它们来说是最方便且一致的上下文数据访问方式。所以这有个矛盾。

一种考虑是通过某种方式来区分可序列化的和不可序列化的事件，但这对基础层的处理来说添加了复杂度，对基础层的处理来说，默认所有事件都是可序列化的是最简单的。

所以目前考虑的考虑是这样，采用比较简单的做法，在`PricingContext`上添加一个`GetCurrent()`的方法，且不把`PricingContext`添加在事件类中，如果Event Handler要访问`PricingContext`对象，则调用`PricingContext.GetCurrent()`，而不是从事件类中获取。