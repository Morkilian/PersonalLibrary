namespace Morkilian.Helper
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class RandomExtensions
    {
        //https://stackoverflow.com/questions/108819/best-way-to-randomize-an-array-with-net
        public static void Shuffle<T>(this System.Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }

            //Usage example
            //var array = new int[] { 1, 2, 3, 4 };
            //var rng = new Random();
            //rng.Shuffle(array);
            //rng.Shuffle(array); // different order from first call to Shuffle
        }
    }

}