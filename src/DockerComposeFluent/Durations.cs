using System;
using System.Globalization;
using System.Text;

namespace DockerComposeFluent
{
    /// <summary>
    /// Reads and writes compose-spec durations: whole numbers with a unit (<c>us</c>, <c>ms</c>, <c>s</c>,
    /// <c>m</c> or <c>h</c>), combined without a separator, for example <c>1m30s</c> or <c>1h5m30s20ms</c>.
    /// <see href="https://github.com/compose-spec/compose-spec/blob/main/11-extension.md#specifying-durations"/>
    /// </summary>
    internal static class Durations
    {
        private const long TicksPerMicrosecond = 10;
        private const long MicrosecondsPerMillisecond = 1000;
        private const long MicrosecondsPerSecond = 1000 * MicrosecondsPerMillisecond;
        private const long MicrosecondsPerMinute = 60 * MicrosecondsPerSecond;
        private const long MicrosecondsPerHour = 60 * MicrosecondsPerMinute;

        /// <summary>
        /// Whether <paramref name="value"/> is a duration such as <c>90s</c> or <c>1m30s</c>.
        /// </summary>
        /// <param name="value">The text to check.</param>
        /// <returns><c>true</c> if every part is a whole number followed by a supported unit.</returns>
        internal static bool IsValid(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            int position = 0;
            while (position < value!.Length)
            {
                int digits = 0;
                while (position < value.Length && value[position] >= '0' && value[position] <= '9')
                {
                    position++;
                    digits++;
                }

                if (digits == 0)
                {
                    return false;
                }

                int unitStart = position;
                while (position < value.Length && (value[position] < '0' || value[position] > '9'))
                {
                    position++;
                }

                if (!IsUnit(value.Substring(unitStart, position - unitStart)))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Writes a time span as a compose-spec duration, for example <c>1m30s</c>. Units of zero are left out.
        /// </summary>
        /// <param name="value">A positive time span that is a whole number of microseconds.</param>
        /// <param name="parameterName">The name of the parameter being checked.</param>
        /// <returns>The duration text.</returns>
        internal static string Format(TimeSpan value, string parameterName)
        {
            if (value <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(parameterName, value, "A duration must be greater than zero.");
            }

            if (value.Ticks % TicksPerMicrosecond != 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, value, "A duration cannot be more precise than a microsecond.");
            }

            long remaining = value.Ticks / TicksPerMicrosecond;
            StringBuilder text = new StringBuilder();
            remaining = Append(text, remaining, MicrosecondsPerHour, "h");
            remaining = Append(text, remaining, MicrosecondsPerMinute, "m");
            remaining = Append(text, remaining, MicrosecondsPerSecond, "s");
            remaining = Append(text, remaining, MicrosecondsPerMillisecond, "ms");
            Append(text, remaining, 1, "us");

            return text.ToString();
        }

        private static bool IsUnit(string unit)
        {
            return unit == "us" || unit == "ms" || unit == "s" || unit == "m" || unit == "h";
        }

        private static long Append(StringBuilder text, long microseconds, long unitSize, string unit)
        {
            long count = microseconds / unitSize;
            if (count > 0)
            {
                text.Append(count.ToString(CultureInfo.InvariantCulture)).Append(unit);
            }

            return microseconds - count * unitSize;
        }
    }
}
