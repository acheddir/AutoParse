using System;
using System.Globalization;
using System.Reflection;
using System.Linq;

namespace AutoParse
{
    public static class AutoParse
    {
        public delegate bool TryParser<T>(string str, out T value);
        public delegate bool TryNumericParser<T>(string str, NumberStyles style, IFormatProvider provider, out T value);
        public delegate bool TryDateParser<T>(string str, IFormatProvider provider, DateTimeStyles style, out T value);
        public delegate bool TryDateParserExactSingleFormat<T>(string str, string format, IFormatProvider provider, DateTimeStyles style, out T value);
        public delegate bool TryDateParserExactMultipleFormat<T>(string str, string[] formats, IFormatProvider provider, DateTimeStyles style, out T value);

        private static T TryParse<T>(this string value, TryParser<T> parser)
            where T : struct
        {
            T val;
            parser.Invoke(value, out val);
            return val;
        }

        private static T? TryParseNullable<T>(this string value, TryParser<T> parser)
            where T : struct
        {
            T val;
            return parser.Invoke(value, out val) ? val : null as T?;
        }

        private static T TryParseOrDefault<T>(this string value, TryParser<T> parser, T @default)
            where T : struct
        {
            T val;
            return parser.Invoke(value, out val) ? val : @default;
        }

        private static T TryParse<T>(this string value, TryNumericParser<T> parser, NumberStyles style, IFormatProvider provider)
            where T : struct
        {
            ThrowExceptionIfNotNumeric<T>();

            T val;
            parser.Invoke(value, style, provider, out val);
            return val;
        }

        private static T? TryParseNullable<T>(this string value, TryNumericParser<T> parser, NumberStyles style, IFormatProvider provider)
            where T : struct
        {
            ThrowExceptionIfNotNumeric<T>();

            T val;
            return parser.Invoke(value, style, provider, out val) ? val : null as T?;
        }

        private static T TryParseOrDefault<T>(this string value, TryNumericParser<T> parser, NumberStyles style, IFormatProvider provider, T @default = default(T))
            where T : struct
        {
            ThrowExceptionIfNotNumeric<T>();

            T val;
            return parser.Invoke(value, style, provider, out val) ? val : @default;
        }

        private static T TryParse<T>(this string value, TryDateParser<T> parser, IFormatProvider provider, DateTimeStyles style)
            where T : struct
        {
            ThrowExceptionIfNotDate<T>();

            T val;
            parser.Invoke(value, provider, style, out val);
            return val;
        }

        private static T? TryParseNullable<T>(this string value, TryDateParser<T> parser, IFormatProvider provider, DateTimeStyles style)
            where T : struct
        {
            ThrowExceptionIfNotDate<T>();

            T val;
            return parser.Invoke(value, provider, style, out val) ? val : null as T?;
        }

        private static T TryParseOrDefault<T>(this string value, TryDateParser<T> parser, IFormatProvider provider, DateTimeStyles style, T @default = default(T))
            where T : struct
        {
            ThrowExceptionIfNotDate<T>();

            T val;
            return parser.Invoke(value, provider, style, out val) ? val : @default;
        }

        public static T TryParse<T>(this string value)
            where T : struct
        {
            var parser = GetParser<T>();
            return value.TryParse(parser);
        }

        public static T? TryParseNullable<T>(this string value)
            where T : struct
        {
            var parser = GetParser<T>();
            return value.TryParseNullable<T>(parser);
        }

        public static T TryParseOrDefault<T>(this string value, T @default = default(T))
            where T : struct
        {
            var parser = GetParser<T>();
            return value.TryParseOrDefault(parser, @default);
        }

        public static T TryParse<T>(this string value, NumberStyles style, IFormatProvider provider)
            where T : struct
        {
            var parser = GetNumericParser<T>();
            return value.TryParse(parser, style, provider);
        }

        public static T? TryParseNullable<T>(this string value, NumberStyles style, IFormatProvider provider)
            where T : struct
        {
            var parser = GetNumericParser<T>();
            return value.TryParseNullable<T>(parser, style, provider);
        }

        public static T TryParseOrDefault<T>(this string value, NumberStyles style, IFormatProvider provider, T @default = default(T))
            where T : struct
        {
            var parser = GetNumericParser<T>();
            return value.TryParseOrDefault(parser, style, provider, @default);
        }

        public static T TryParse<T>(this string value, IFormatProvider provider, DateTimeStyles style)
            where T : struct
        {
            var parser = GetDateParser<T>();
            return value.TryParse(parser, provider, style);
        }

        public static T? TryParseNullable<T>(this string value, IFormatProvider provider, DateTimeStyles style)
            where T : struct
        {
            var parser = GetDateParser<T>();
            return value.TryParseNullable<T>(parser, provider, style);
        }

        public static T TryParseOrDefault<T>(this string value, IFormatProvider provider, DateTimeStyles style, T @default = default(T))
            where T : struct
        {
            var parser = GetDateParser<T>();
            return value.TryParseOrDefault(parser, provider, style, @default);
        }

        #region Utils

        private static TryParser<T> GetParser<T>()
        {
            return
                Delegate.CreateDelegate(typeof (TryParser<T>), typeof (T), "TryParse") as
                    TryParser<T>;
        }

        private static TryNumericParser<T> GetNumericParser<T>()
        {
            return
                Delegate.CreateDelegate(typeof (TryNumericParser<T>), typeof (T), "TryParser") as
                    TryNumericParser<T>;
        }

        private static TryDateParser<T> GetDateParser<T>()
        {
            return
                Delegate.CreateDelegate(typeof(TryDateParser<T>), typeof(T), "TryParser") as
                    TryDateParser<T>;
        }

        private static TryDateParserExactSingleFormat<T> GetDateParserExactSingleFormat<T>()
        {
            return
                Delegate.CreateDelegate(typeof(TryDateParserExactSingleFormat<T>), typeof(T), "TryParser") as
                    TryDateParserExactSingleFormat<T>;
        }

        private static TryDateParserExactMultipleFormat<T> GetDateParserExactMultipleFormat<T>()
        {
            return
                Delegate.CreateDelegate(typeof(TryDateParserExactMultipleFormat<T>), typeof(T), "TryParser") as
                    TryDateParserExactMultipleFormat<T>;
        }

        public static void ThrowExceptionIfNotNumeric<T>() where T : struct
        {
            if (typeof(T) == typeof(char) || typeof(T) == typeof(DateTime) || typeof(T) == typeof(bool))
                throw new InvalidCastException("Invalid cast, use this to cast numeric types.");
        }

        public static void ThrowExceptionIfNotDate<T>() where T : struct
        {
            if (typeof(T) != typeof(DateTime))
                throw new InvalidCastException("Invalid cast, use this to cast dates.");
        }

        #endregion
    }
}