using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace SecretSharing.Test.Core
{
    [TestClass]
    public class BenalohLeichterTest
    {
        [TestMethod]
        public void Test_BenalohLeichter_DivideSecret()
        {
            //assign
            var secret = "12345678";
            var secretbytes = Encoding.UTF8.GetBytes(secret.ToCharArray());
            var benaloh = new SecretSharingCore.Algorithms.GeneralizedAccessStructure.BenalohLeichter();
            var access = new SecretSharing.OptimalThreshold.Models.AccessStructure("p1^p2^p3,p2^p3^p4,p1^p3^p4,p1^p2^p4");
            var  tryIntersect  = true;


            //arrange
            var optimisedAccess = SecretSharing.OptimalThreshold.ThresholdHelper.OptimiseAccessStructure(access, tryIntersect);
            var shares = benaloh.DivideSecret(secretbytes,optimisedAccess);

            //assert
            Assert.IsNotNull(shares);
            Assert.IsTrue(shares.Count > 0);
        }
        [TestMethod]
        [Ignore]
        public void Test_BenalohLeichter_ReconstructSecret()
        {
            //assign
            var secret = "12345678";
            var secretbytes = Encoding.UTF8.GetBytes(secret.ToCharArray());
            var benaloh = new SecretSharingCore.Algorithms.GeneralizedAccessStructure.BenalohLeichter();
            var access = new SecretSharing.OptimalThreshold.Models.AccessStructure("p2^p3,p1^p3^p4,p1^p2^p4");
            var tryIntersect = true;

            //arrange
            var optimisedAccess = SecretSharing.OptimalThreshold.ThresholdHelper.OptimiseAccessStructure(access, tryIntersect);
            var shares = benaloh.DivideSecret(secretbytes, optimisedAccess);

            foreach (var item in shares)
            {
                var reconSecret = Encoding.UTF8.GetString( benaloh.ReconstructSecret(item));
                
                Assert.AreEqual(secret, reconSecret,"secret and reconstructed secret are not the same");
            }
            
 
            //assert
            Assert.IsNotNull(shares);
            Assert.IsTrue(shares.Count > 0);
           
        }
    }
}
