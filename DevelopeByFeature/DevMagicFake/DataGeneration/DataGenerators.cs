// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGenerators.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The data generators.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace M.Radwan.DevMagicFake.DataGeneration
{
    /// <summary>
    /// The data generators.
    /// </summary>
    internal class DataGenerators
    {
        #region Methods

        /// <summary>
        /// The random alphanumeric.
        /// </summary>
        /// <param name="random">
        /// The random.
        /// </param>
        /// <returns>
        /// The random alphanumeric returned value.
        /// </returns>
        internal static string RandomAlphanumeric(Random random)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

        /// <summary>
        /// The random bool.
        /// </summary>
        /// <param name="random">
        /// The random.
        /// </param>
        /// <returns>
        /// The random bool return value.
        /// </returns>
        internal static bool RandomBool(Random random)
        {
            return random.Next(2) == 1;
        }

        /// <summary>
        /// The random byte.
        /// </summary>
        /// <param name="min">
        /// The min.
        /// </param>
        /// <param name="max">
        /// The max.
        /// </param>
        /// <param name="random">
        /// The random.
        /// </param>
        /// <returns>
        /// The random byte.
        /// </returns>
        internal static byte RandomByte(byte min, byte max, Random random)
        {

            return (byte)random.Next(min, max);
        }

        /// <summary>
        /// The random date time, from 1/1/2000 to Now
        /// </summary>
        /// <param name="random">
        /// The random.
        /// </param>
        /// <returns>
        /// The date time return value
        /// </returns>
        internal static DateTime RandomDateTime(Random random)
        {
            DateTime from = new DateTime(2000, 1, 1);
            DateTime to = DateTime.Now;
            return RandomDateTime(from, to, random);
        }

        /// <summary>
        /// The random date time.
        /// </summary>
        /// <param name="from">
        /// The from date.
        /// </param>
        /// <param name="to">
        /// The to date.
        /// </param>
        /// <param name="random">
        /// The random.
        /// </param>
        /// <returns>
        /// The date time return value
        /// </returns>
        internal static DateTime RandomDateTime(DateTime from, DateTime to, Random random)
        {
            var range = new TimeSpan(to.Ticks - from.Ticks);
            return from + new TimeSpan((long)(range.Ticks * random.NextDouble()));
        }

        /// <summary>
        /// The random day only, the time will be always 12:00:00.
        /// </summary>
        /// <param name="random">
        /// The random.
        /// </param>
        /// <returns>
        /// The day return value
        /// </returns>
        internal static DateTime RandomDay(Random random)
        {
            DateTime start = new DateTime(1995, 1, 1);

            int range = (DateTime.Today - start).Days;
            return start.AddDays(random.Next(range));
        }

        /// <summary>
        /// The random decimal.
        /// </summary>
        /// <param name="random">
        /// The random.
        /// </param>
        /// <returns>
        /// The random decimal return value.
        /// </returns>
        internal static decimal RandomDecimal(Random random)
        {
            // to random the fraction use (byte)random.Next(5) instead of scale=2
            byte scale = 2;
            bool sign = random.Next(2) == 1;

            // return new decimal(NextInt32(), NextInt32(), NextInt32(), sign, scale);
            // return new decimal(NextInt32(), NextInt32(), NextInt32(), false, scale);
            return new decimal(RandomInt(100, 10000000, random), 0, 0, false, 2);
        }

        /// <summary>
        /// The random decimal.
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <param name="random">
        /// The random.
        /// </param>
        /// <returns>
        /// The random decimal.
        /// </returns>
        internal static decimal RandomDecimal(decimal from, decimal to, Random random)
        {
            // I need to create it
            // now it just will call normal random decimal 
            return RandomDecimal(random);
        }

        /// <summary>
        /// The random double.
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <param name="random">
        /// The random.
        /// </param>
        /// <returns>
        /// The random double.
        /// </returns>
        internal static double RandomDouble(double from, double to, Random random)
        {
            // I need to create it I will just use anything now
            return Convert.ToDouble(RandomDecimal(random));
        }

        /// <summary>
        /// The random int.
        /// </summary>
        /// <param name="min">
        /// The minimum number.
        /// </param>
        /// <param name="max">
        /// The maximum number.
        /// </param>
        /// <param name="random">
        /// The random.
        /// </param>
        /// <returns>
        /// The random int return value.
        /// </returns>
        internal static int RandomInt(int min, int max, Random random)
        {
            return random.Next(min, max);
        }

        /// <summary>
        /// The random long.
        /// </summary>
        /// <param name="min">
        /// The minimum number.
        /// </param>
        /// <param name="max">
        /// The maximum number.
        /// </param>
        /// <param name="random">
        /// The random.
        /// </param>
        /// <returns>
        /// The random long return value.
        /// </returns>
        internal static long RandomLong(int min, int max, Random random)
        {
            return random.Next(min, max);
        }

        /// <summary>
        /// The random s byte.
        /// </summary>
        /// <param name="min">
        /// The min.
        /// </param>
        /// <param name="max">
        /// The max.
        /// </param>
        /// <param name="random">
        /// The random.
        /// </param>
        /// <returns>
        /// The random s byte.
        /// </returns>
        internal static sbyte RandomSByte(sbyte min, sbyte max, Random random)
        {
            return (sbyte)random.Next(min, max);
        }

        /// <summary>
        /// The random short.
        /// </summary>
        /// <param name="min">
        /// The minimum number.
        /// </param>
        /// <param name="max">
        /// The maximum number.
        /// </param>
        /// <param name="random">
        /// The random.
        /// </param>
        /// <returns>
        /// The random short return value.
        /// </returns>
        internal static short RandomShort(short min, short max, Random random)
        {
            return (short)random.Next(min, max);
        }

        /// <summary>
        /// The random string.
        /// </summary>
        /// <param name="size">
        /// The size of the length of the returned string.
        /// </param>
        /// <param name="random">
        /// The random.
        /// </param>
        /// <returns>
        /// The random string return value.
        /// </returns>
        internal static string RandomString(int size, Random random)
        {
            // to change the random to capital latter change 97 to be 65
            var builder = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                char ch = Convert.ToChar(Convert.ToInt32(Math.Floor((26 * random.NextDouble()) + 97)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        /// <summary>
        /// The random string.
        /// </summary>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <param name="random">
        /// The random.
        /// </param>
        /// <returns>
        /// The random string return value.
        /// </returns>
        internal static string RandomString(string[] items, Random random)
        {
            // Now I have to Random object of dynamic seed and one for fixed seed, this because I have get feedback that the data generated should be the same to support testing, 
            // because if we write unit test with data we don't want to fail because there are new data generated, so I will add new parameter to the method to support choosing between
            // the different randomizations
            // ToDo: I suggest to make this configurable so I will decide to generate data using dynamic seed or fixed seed based on client needs
            int index = random.Next(items.Length);
            string randomString = items[index];
            if (randomString == "null")
            {
                return null;
            }

            return randomString;
        }

        /// <summary>
        /// The random string.
        /// </summary>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <param name="random">
        /// The random.
        /// </param>
        /// <returns>
        /// The random string return value.
        /// </returns>
        internal static string RandomString(List<string> items, Random random)
       {
           return RandomString(items.ToArray<string>(), random);
        }

        /// <summary>
        /// The random u short.
        /// </summary>
        /// <param name="min">
        /// The min.
        /// </param>
        /// <param name="max">
        /// The max.
        /// </param>
        /// <param name="random">
        /// The random.
        /// </param>
        /// <returns>
        /// The random u short.
        /// </returns>
        internal static ushort RandomUShort(ushort min, ushort max, Random random)
        {
            return (ushort)random.Next(min, max);
        }

        /// <summary>
        /// The random values.
        /// </summary>
        /// <param name="range">
        /// The range.
        /// </param>
        /// <param name="length">
        /// The length.
        /// </param>
        /// <param name="random">
        /// The random.
        /// </param>
        /// <returns>
        /// The random values returned.
        /// </returns>
        internal static object RandomValues(string range, int length, Random random)
        {
            var chars = range;
            var result = new string(
                Enumerable.Repeat(chars, length)
                    .Select(s => s[random.Next(s.Length)])
                    .ToArray());

            return result;
        }

        #endregion
    }
}
