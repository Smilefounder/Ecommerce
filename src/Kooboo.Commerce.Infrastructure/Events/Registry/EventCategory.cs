using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Registry
{
    public class EventCategory
    {
        public static readonly EventCategory Uncategorized = new EventCategory("Uncategorized", 0);

        public string Name { get; private set; }

        public int Order { get; private set; }

        public EventCategory(string name, int order)
        {
            Require.NotNullOrEmpty(name, "name");
            Name = name;
            Order = order;
        }

        public static EventCategory From(Type type)
        {
            var attr = type.GetCustomAttribute<CategoryAttribute>(true);
            if (attr == null)
            {
                foreach (var @interface in type.GetInterfaces())
                {
                    attr = @interface.GetCustomAttribute<CategoryAttribute>(true);
                    if (attr != null)
                    {
                        break;
                    }
                }
            }

            if (attr != null)
            {
                return new EventCategory(attr.Name, attr.Order);
            }

            return EventCategory.Uncategorized;
        }

        public override bool Equals(object obj)
        {
            var other = obj as EventCategory;
            return other != null && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
