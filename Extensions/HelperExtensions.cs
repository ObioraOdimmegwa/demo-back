using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Server
{
    public static class HelperExtensions
    {
        public static List<string> Words = new List<string>
        {
          "Blaze","unfailingly","about","reward","survive",
          "recreate","amazing","rabbit","eagle","protect",
          "shield","arrow","space","sunlight","decorate",
          "elevate","mute","real","travel","landscape",
          "windmill","castle","slide","suit","crave"
        };

        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static string ToBase64(this string value)
        {
            byte[] val = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(val).Replace("=",string.Empty);
        }
        public static List<int> GenerateUniqueRandomNumbers(int min = 0, int max = 24, int amount = 12)
        {
            Random rand = new Random();
            int currNumber = rand.Next(min,max);
            List<int> randomNumbers = new List<int>();

            for(int count = 1; count <= amount; count++)
            {
                while(randomNumbers.Contains(currNumber))
                    currNumber = rand.Next(min,max);
                randomNumbers.Add(currNumber);
            }
            return randomNumbers;
        }
        public static void SetPassPhrase(this User user)
        {
            string passphrase = "";
            List<int> uniqueNumbers = GenerateUniqueRandomNumbers(0,24,12);
            foreach(int i in uniqueNumbers)
                passphrase += Words[i] + " ";
            user.RecoveryPhrase = passphrase.Trim();
        }
       }
}