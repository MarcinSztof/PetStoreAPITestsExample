using System;
using System.Linq;

namespace PetStoreAPITests.Data
{
    public static class DateGenerationUtils
    {
        private static Random random = new Random();
        private static string charsAlphabetic = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static string charsAlphanumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static int GetRandomInt32(int min, int max) => random.Next(min, max);

        public static string GetRandomAlphabeticString(int length) =>
            new string(Enumerable.Repeat('\0', length).Select(x => charsAlphabetic[random.Next(0, charsAlphabetic.Length - 1)]).ToArray());

        public static string GetRandomAlphanumericString(int length) =>
            new string(Enumerable.Repeat('\0', length).Select(x => charsAlphanumeric[random.Next(0, charsAlphanumeric.Length - 1)]).ToArray());

    }
}
