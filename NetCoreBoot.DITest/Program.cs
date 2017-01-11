using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreBoot.Common;

namespace NetCoreBoot.DITest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string input = "4a7d1ed414474e4033ac29ccb8653d9b";
            string encry = "57d3031d6fc4a34d";
            string encry1 = "12345678";
            string result = Des.Encrypt(input, encry1);
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
