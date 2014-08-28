using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Jobs
{
    public class TimeOfDay : IEquatable<TimeOfDay>, IComparable<TimeOfDay>
    {
        [JsonProperty]
        public int Hour { get; private set; }

        [JsonProperty]
        public int Mintue { get; private set; }

        public TimeOfDay() { }

        public TimeOfDay(int hour, int minute)
        {
            Hour = hour;
            Mintue = minute;
        }

        public bool Equals(TimeOfDay other)
        {
            return Hour == other.Hour && Mintue == other.Mintue;
        }

        public override bool Equals(object obj)
        {
            if (obj is TimeOfDay)
            {
                return Equals((TimeOfDay)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Hour.GetHashCode() * 397 ^ Mintue.GetHashCode();
        }

        public override string ToString()
        {
            return Hour.ToString("00") + ":" + Mintue.ToString("00");
        }

        public int CompareTo(TimeOfDay other)
        {
            if (Hour < other.Hour)
            {
                return -1;
            }

            if (Hour > other.Hour)
            {
                return 1;
            }

            if (Mintue < other.Mintue)
            {
                return -1;
            }

            if (Mintue > other.Mintue)
            {
                return 1;
            }

            return 0;
        }
    }

    public static class DateTimeExtensions
    {
        public static TimeOfDay TimeOfDay(this DateTime time)
        {
            return new TimeOfDay(time.Hour, time.Minute);
        }
    }
}