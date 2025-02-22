namespace Morkilian.Helper
{
    using System;

    public static class EnumExtensions
    {


        /// <summary>
        /// Sets the given flag(s) to this current enum to true.
        /// </summary>    
        /// <param name="flag">The flag to set to true.</param>    
        /// <remarks>
        /// //Bit operation OR => if one or the other is true, then the resultant is true
        ///Since our flag is already true, it is meant to force this bit to true and leave the rest untouched (if other bits of the "currentValue" were already true, then they'll be already true)
        ///If the bits in "currentValue" were already false, they'll stay false since every bit except the concerned(s) ones from the flag will be set to true
        /// </remarks>
        /// <example>
        ///     01101001 -> currentValue
        /// OR  00000100 -> flag
        ///     01101101 -> result
        /// </example>
        public static T SetFlag<T>(this Enum currentValue, T flag) where T : struct, IConvertible
        {
            int? intValue = Convert.ToInt32(currentValue);
            int? flagValue = Convert.ToInt32(flag);

            int result = intValue.Value | flagValue.Value;

            return (T)(object)result;
        }

        /// <summary>
        /// Removes the given flag(s) to this current enum, or in other words, set them to false, and then returns it.
        /// </summary>
        /// <param name="flag">The flag to set to false</param>    
        /// <remarks>
        /// The idea is to force the concerned bits to false (or 0), and here the idea is to invert every bit of the flag, and then make an AND operation with our current value
        /// true & true stays true
        /// true & false stays false
        /// false & false stays false
        /// </remarks>
        /// <example>
        ///     0001000 -> flag
        ///     1110111 -> flag inverted
        /// AND 0101110 -> current value
        ///     0100110 -> result
        /// </example>
        public static T Remove<T>(this Enum currentValue, T flag) where T : struct, IConvertible
        {
            int? intValue = Convert.ToInt32(currentValue);
            int? flagValue = Convert.ToInt32(flag);
            intValue &= ~flagValue;

            return (T)(object)intValue.Value;
        }

        /// <summary>
        /// Selects a random enum value flag that's turned on. Returns the 0 value by default.
        /// </summary>        
        public static T SelectRandom<T>(this Enum currentValue) where T : struct, IConvertible
        {
            System.Random rang = new System.Random();
            bool foundRandom = false;
            int safeCount = 0;
            int currentValueInt = (int)(object)currentValue;
            T selected = default;
            while (foundRandom == false && safeCount++ < 50)
            {
                T[] allValues = (T[])Enum.GetValues(typeof(T));
                selected = allValues[rang.Next(allValues.Length)];
                int selectedInt = (int)(object)selected;
                if ((currentValueInt & selectedInt) == selectedInt)
                {
                    foundRandom = true;
                    break;
                }
            }
            if (foundRandom == false)
            {
                Logger.LogError("Couldn't find a suitable random enum value! Returning default.", eLoggerTypes.Extensions);
                return (T)(object)0;
            }
            else return selected;
        }

        public static bool ContainsFlag<T>(this Enum currentValue, T flag) where T : struct, IConvertible
        {
            int? intValue = Convert.ToInt32(currentValue);
            int? flagValue = Convert.ToInt32(flag);
            return (intValue & flagValue) == flagValue;
        }

    }

}