using System;
using System.Collections.Generic;
using System.Text;

namespace CodeRunSpace
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine(
                @"
******************
Daily C# Code Run
****************** 
Day:"
            );
            int? dayNumber = Convert.ToInt32(Console.ReadLine());

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            Type type = Type.GetType($"Day{dayNumber}");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
            Object? day = Activator.CreateInstance(type);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            day?.ToString();
        }
    }
}
