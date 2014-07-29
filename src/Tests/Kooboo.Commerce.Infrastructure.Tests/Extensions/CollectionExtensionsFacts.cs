using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kooboo.Commerce.Infrastructure.Tests.Extensions
{
    public class CollectionExtensionsFacts
    {
        public class CollectionUpdate
        {
            [Fact]
            public void should_invoke_callbacks()
            {
                var item1 = new Item { Name = "Item1" };
                var item2 = new Item { Id = 2, Name = "Item2" };
                var item3 = new Item { Id = 3, Name = "Item3" };
                var oldItems = new List<Item>
                {
                    item2, item3
                };
                var newItems = new List<Item>
                {
                    item1, new Item { Id = 2, Name = "Item2(new)" }
                };

                oldItems.Update(
                    from: newItems,
                    by: i => i.Id,
                    onUpdateItem: (oldItem, newItem) => { oldItem.Name = newItem.Name; },
                    onAddItem: item => item.Name += "(add)",
                    onRemoveItem: item => item.Name += "(remove)");

                Assert.Equal("Item1(add)", item1.Name);
                Assert.Equal("Item2(new)", item2.Name);
                Assert.Equal("Item3(remove)", item3.Name);
            }

            [Fact]
            public void should_merge_items()
            {
                var oldItems = new List<Item>
                {
                    new Item { Id = 1, Name = "Old1" },     // will be updated
                    new Item { Id = 2, Name = "Old2" },     // will be updated
                    new Item { Id = 10, Name = "Old10" }    // will be removed
                };

                var newItems = new List<Item>
                {
                    new Item { Id = 1, Name = "New1" },
                    new Item { Name = "New4" },             // will be added
                    new Item { Id = 2, Name = "New2" },
                    new Item { Id = 3, Name = "New3" },     // will be added
                    new Item { Name = "New8" }              // will be added
                };

                oldItems.Update(
                    from: newItems,
                    by: i => i.Id,
                    onUpdateItem: (oldItem, newItem) => oldItem.Name = newItem.Name);

                oldItems = oldItems.OrderBy(i => i.Id).ThenBy(i => i.Name).ToList();

                Assert.Equal(5, oldItems.Count);

                Assert.Equal(0, oldItems[0].Id);
                Assert.Equal("New4", oldItems[0].Name);

                Assert.Equal(0, oldItems[1].Id);
                Assert.Equal("New8", oldItems[1].Name);

                Assert.Equal(1, oldItems[2].Id);
                Assert.Equal("New1", oldItems[2].Name);

                Assert.Equal(2, oldItems[3].Id);
                Assert.Equal("New2", oldItems[3].Name);

                Assert.Equal(3, oldItems[4].Id);
                Assert.Equal("New3", oldItems[4].Name);
            }

            [Fact]
            public void can_add_multiple_new_items_with_default_id()
            {
                var oldItems = new List<Item>();
                var newItems = new List<Item>
                {
                    new Item { Name = "Item1" },
                    new Item { Name = "Item2" }
                };

                oldItems.Update(
                    from: newItems,
                    by: i => i.Id,
                    onUpdateItem: (oldItem, newItem) => { });

                Assert.Equal(2, oldItems.Count);
                Assert.Equal("Item1", oldItems[0].Name);
                Assert.Equal("Item2", oldItems[1].Name);
            }

            public class Item
            {
                public int Id { get; set; }
                public string Name { get; set; }
            }
        }
    }
}
