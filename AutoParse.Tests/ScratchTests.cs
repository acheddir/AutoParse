using System;
using NUnit.Framework;
using Should;

namespace AutoParse.Tests
{
    [TestFixture]
    public class ScratchTests
    {
        [Test]
        public void should_parse_bool()
        {
            // Arrange
            var boolStr = "true";

            // Act
            var boolValue = boolStr.TryParse<bool>();

            // Assert
            boolValue.ShouldEqual(true);
        }

        [Test]
        public void should_parse_char()
        {
            // Arrange
            var charStr = "a";

            // Act
            var charValue = charStr.TryParse<char>();

            // Assert
            charValue.ShouldEqual('a');
        }

        [Test]
        public void should_parse_numeric()
        {
            // Arrange
            var intStr = "0009998";
            var longStr = "0009998";
            var doubleStr = "0009998,0001";

            // Act
            var intValue = intStr.TryParse<int>();
            var longValue = longStr.TryParse<long>();
            var doubleValue = doubleStr.TryParse<double>();

            // Assert
            intValue.ShouldEqual(9998);
            longValue.ShouldEqual(9998);
            doubleValue.ShouldEqual(9998.0001);
        }

        [Test]
        public void should_parse_date()
        {
            // Arrange
            var dateStr = "01/01/2016 00:00:00.000";

            // Act
            var dateValue = dateStr.TryParse<DateTime>();

            // Assert
            dateValue.ShouldEqual(new DateTime(2016, 1, 1));
        }

        [Test]
        public void should_not_parse_bool()
        {
            // Arrange
            var boolStr = "trues";

            // Act
            var boolValue = boolStr.TryParse<bool>();

            // Assert
            boolValue.ShouldEqual(default(bool));
        }

        [Test]
        public void should_not_parse_char()
        {
            // Arrange
            var charStr = "ab";

            // Act
            var charValue = charStr.TryParse<char>();

            // Assert
            charValue.ShouldEqual(default(char));
        }

        [Test]
        public void should_not_parse_numeric()
        {
            // Arrange
            var intStr = "00A9998";
            var longStr = "00A9998";
            var doubleStr = "00A9998,0001";

            // Act
            var intValue = intStr.TryParse<int>();
            var longValue = longStr.TryParse<long>();
            var doubleValue = doubleStr.TryParse<double>();

            // Assert
            intValue.ShouldEqual(default(int));
            longValue.ShouldEqual(default(long));
            doubleValue.ShouldEqual(default(double));
        }

        [Test]
        public void should_not_parse_date()
        {
            // Arrange
            var dateStr = "03/01/2016A 00:00:00.000";

            // Act
            var dateValue = dateStr.TryParse<DateTime>();

            // Assert
            dateValue.ShouldEqual(new DateTime(0001, 1, 1));
        }

        [Test]
        public void should_parse_bool_to_default_on_failure()
        {
            // Arrange
            var boolStr = "trues";

            // Act
            var boolValue = boolStr.TryParseOrDefault<bool>(true);

            // Assert
            boolValue.ShouldEqual(true);
        }

        [Test]
        public void should_parse_char_to_default_on_failure()
        {
            // Arrange
            var charStr = "ab";

            // Act
            var charValue = charStr.TryParseOrDefault<char>('a');

            // Assert
            charValue.ShouldEqual('a');
        }

        [Test]
        public void should_parse_numeric_to_default_on_failure()
        {
            // Arrange
            var intStr = "00A9998";
            var longStr = "00A9998";
            var doubleStr = "00A9998,0001";

            // Act
            var intValue = intStr.TryParseOrDefault<int>(10);
            var longValue = longStr.TryParseOrDefault<long>(10L);
            var doubleValue = doubleStr.TryParseOrDefault<double>(10.0D);

            // Assert
            intValue.ShouldEqual(10);
            longValue.ShouldEqual(10L);
            doubleValue.ShouldEqual(10.0D);
        }

        [Test]
        public void should_parse_date_to_default_on_failure()
        {
            // Arrange
            var dateStr = "03/01/2016A 00:00:00.000";

            // Act
            var dateValue = dateStr.TryParseOrDefault<DateTime>(DateTime.Today);

            // Assert
            dateValue.ShouldEqual(DateTime.Today);
        }
    }
}