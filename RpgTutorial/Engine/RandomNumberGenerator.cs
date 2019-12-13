using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class RandomNumberGenerator
    {
        private static readonly Random _simpleRNG = new Random();
        private static readonly RNGCryptoServiceProvider _complexRNG = new RNGCryptoServiceProvider();

        public static int SimpleNumberBetween(int minimumValue, int maximumValue) =>
            _simpleRNG.Next(minimumValue, maximumValue + 1);

        public static int NumberBetween(int minimumValue, int maximumValue)
        {
            // get randomness
            byte[] randomNumber = new byte[1];
            _complexRNG.GetBytes(randomNumber);
            double asciiValueOfRandomNumber = Convert.ToDouble(randomNumber[0]);

            // We are using Math.Max and subtracting minor error to ensure the multiplier
            // will always be in [0.0,1), since 1 will cause a problem
            double multiplier = Math.Max(0, (asciiValueOfRandomNumber / 255d) - 0.0000000001d);

            // inc the range by 1 to account for the rounding of Math.Floor
            int range = maximumValue - minimumValue + 1;

            // apply randomness to our range
            double randomValueInRange = Math.Floor(multiplier * range);

            return (int) (minimumValue + randomValueInRange);
        }

        public static int RollDice(int numberOfDice, int sizeOfDice)
        {
            if (numberOfDice < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfDice), "You must roll at least 1 die");
            }

            int sum = 0;
            for (int i = 1; i < numberOfDice; i++)
            {
                sum += NumberBetween(1, sizeOfDice);
            }

            return sum;
        }
    }

}
