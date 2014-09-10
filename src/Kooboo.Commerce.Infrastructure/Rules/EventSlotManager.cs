using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public class EventSlotManager
    {
        public static readonly EventSlotManager Instance = new EventSlotManager();

        private Dictionary<string, EventSlot> _slotsByEventNames = new Dictionary<string, EventSlot>();
        private Dictionary<string, List<EventSlot>> _slotsByGroups = new Dictionary<string, List<EventSlot>>();

        public IEnumerable<string> GetGroups()
        {
            return _slotsByGroups.Keys;
        }

        public IEnumerable<EventSlot> GetSlots(string group)
        {
            List<EventSlot> slots;
            if (_slotsByGroups.TryGetValue(group, out slots))
            {
                return slots;
            }

            return Enumerable.Empty<EventSlot>();
        }

        public EventSlot GetSlot<TEvent>()
            where TEvent : IEvent
        {
            return GetSlot(typeof(TEvent).Name);
        }

        public EventSlot GetSlot(string eventName)
        {
            EventSlot slot;
            if (_slotsByEventNames.TryGetValue(eventName, out slot))
            {
                return slot;
            }

            return null;
        }

        public void Register<TEvent>(string group, string shortName = null)
            where TEvent : IEvent
        {
            Register(group, typeof(TEvent), shortName);
        }

        public void Register(string group, Type eventType, string shortName = null)
        {
            var slot = new EventSlot(eventType, shortName);
            slot.Initialize();

            _slotsByEventNames.Add(eventType.Name, slot);

            if (!_slotsByGroups.ContainsKey(group))
            {
                _slotsByGroups.Add(group, new List<EventSlot> { slot });
            }
            else
            {
                _slotsByGroups[group].Add(slot);
            }
        }
    }
}
