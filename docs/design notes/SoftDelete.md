# Soft Delete (软删除/假删) #

本来有在Infrastructure中添加相应的支持，但是通过 Uid 的这篇文章：[http://www.udidahan.com/2009/09/01/dont-delete-just-dont/](http://www.udidahan.com/2009/09/01/dont-delete-just-dont/)，似乎Soft Delete并不是很重要，因为对于重要对象来说，我们可能不应该提供删除，而是提供更具业务含义的操作，而对于很简单的对象，也没有很大假删的意义，那就意味着对假删的支持是没有很大实际意义的？