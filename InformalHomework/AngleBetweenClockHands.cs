// Brillan Morgan

using System;
using System.Collections.Generic;
using System.Text;

namespace InformalHomework
{
    public static class ClockAngleDemo
    {
        private static Random RNG = new Random((int)DateTime.Now.Ticks);

        public static void Run()
        {
            const int RANDOM_TIMES_COUNT = 5;

            var times = new List<SimpleTime>
            {
                new SimpleTime(1,30),
                new SimpleTime(3,30),
                new SimpleTime(6,15),
                new SimpleTime(2,10),
                new SimpleTime(9,45),
                new SimpleTime(7,05),
                new SimpleTime(12,40),
                new SimpleTime(12,00),
            };

            // Note to self:
            // Remember when you make a collection to contain reference types
            // if inserting new objects into that collection, must call new
            // for the new objects.

            for (var i = 1; i<= RANDOM_TIMES_COUNT; ++i)
            {
                var randHour = RNG.Next(1, 12 + 1);
                var randMinute = RNG.Next(0, 59 + 1);

                times.Add(
                    new SimpleTime(randHour, randMinute));
            }

            Console.WriteLine("Let's calculate the angle between the two" +
                "hands of an analog clock based on the time shown in integer" +
                "hours and minutes.\n");

            foreach (var time in times)
            {
                var angle =
                    AngleBetweenClockHands.Calculate(time.Hour, time.Minute);

                Console.WriteLine(
                    $"{time.Hour}:{time.Minute:00} == {angle:N1} degrees.\n");
            }

            // Nice site to quickly test results:
            // https://kyle1668.github.io/Clock-Angle-Calculator/
        }


        private class SimpleTime
        {
            public SimpleTime(int hour, int minute)
            {
                Hour = hour;
                Minute = minute;
            }

            public int Hour { get; set; }
            public int Minute { get; set; }

        }
    }

    public static class AngleBetweenClockHands
    {
        // AngleBetweenClockHands.Calculate
        // Gives result as smallest positive angle (or zero)

        // Valid inputs:
        // hours shall be from 1 to 12, inclusive
        // mins shall be between 0 to 59, inclusive
        public static decimal Calculate(int hour, int minute)
        {
            const decimal MINS_PER_HOUR = 60.0M;

            const decimal HOURS_PER_REVOLUTION = 12.0M;

            const decimal DEGREES_IN_CIRCLE = 360.0M;

            if (hour < 1 || hour > 12 || minute < 0 || minute > 59)
                throw new ArgumentException($"{hour}:{minute} is an invalid time.");

            // Express hour 12 as hour 0 for purposes of angle calculation
            // to be consistent with how minutes are expressed
            var zeroBasedHours = hour == 12 ? 0 : hour;
            // Now zeroBasedHours is from 0 to 11

            // Position of the hour hand on range [0, 12) 
            var hourHandOnHourScale =
                zeroBasedHours + ( minute / MINS_PER_HOUR );

            // Position of the hour hand on range [0, 60) 
            var hourHandOnMinScale =
                hourHandOnHourScale * ( MINS_PER_HOUR / HOURS_PER_REVOLUTION );

            // Note that minute is already expressed on range [0, 60)
            // So now hourHandOnMinScale and minute are expressed on the same scale

            var posDiffInMins = minute > hourHandOnMinScale ?
                minute - hourHandOnMinScale : hourHandOnMinScale - minute;

            var fractionOfOneRevolution = posDiffInMins / MINS_PER_HOUR;

            var angleInDegrees = fractionOfOneRevolution * DEGREES_IN_CIRCLE;

            // 360 degrees is correct, but this method returns the smallest angle
            // so zero is more consistent
            var smallestPosAngleInDegrees =
                angleInDegrees == DEGREES_IN_CIRCLE ? 0 : angleInDegrees;

            return smallestPosAngleInDegrees;
        }
    }
}
