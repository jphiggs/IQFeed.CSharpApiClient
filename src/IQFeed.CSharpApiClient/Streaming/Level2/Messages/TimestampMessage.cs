﻿using System;
using System.Globalization;
using IQFeed.CSharpApiClient.Extensions;

namespace IQFeed.CSharpApiClient.Streaming.Level2.Messages
{
    // T,[YYYYMMDD HH:mm:SS]<LF>
    public class TimestampMessage
    {
        static char[] chr = { '\r' };
        public const string TimestampMessageDateTimeFormat = "yyyyMMdd HH:mm:ss";

        public TimestampMessage(DateTime timestamp)
        {
            Timestamp = timestamp;
        }

        public DateTime Timestamp { get; }

        public static TimestampMessage Parse(string message)
        {
            var values = message.SplitFeedMessage();
            
            DateTime.TryParseExact(values[1].TrimEnd(chr), TimestampMessageDateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var timestamp);
            return new TimestampMessage(timestamp);
        }

        public override bool Equals(object obj)
        {
            return obj is TimestampMessage message &&
                   Timestamp == message.Timestamp;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return 227403579 + Timestamp.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"{nameof(Timestamp)}: {Timestamp}";
        }
    }
}