using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using System.Diagnostics;

namespace SecretSharing.Test.Core.PrimeGeneration
{
    [TestClass]
    public class PrimeGenerationTests
    {
        [TestMethod]
        public void Test_GenerateRandomPrime()
        { 
            SecretSharingCore.PrimeGenerator pg = new SecretSharingCore.PrimeGenerator();

            Stopwatch sw=new Stopwatch();
            sw.Start();
            int iterate = 5;
            for (int i = 0; i < iterate; i++)
            {
                BigInteger bi = new BigInteger(pg.GenerateRandomPrime(33));
            }
            sw.Stop();
            var avg= sw.ElapsedMilliseconds / iterate;
            Console.WriteLine(avg);
        }
    }
}
