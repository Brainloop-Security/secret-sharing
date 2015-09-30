using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SecretSharing.Test.OptimalThreshold
{
    [TestClass]
    public class ThresholdHelperTest
    {
        [TestMethod]
        public void Test_OptimalThreshold_IsThresholdShareShorterTestPass()
        {
            //assign 
            var nothresholdAccess = "p1^p2^p3,p2^p3^p4,p1^p3^p4,p1^p2^p4,p4^p5^p6,p4^p5^p7,p4^p6^p7";
            var tryIntersect = true;
            //arrange
            SecretSharing.OptimalThreshold.Models.AccessStructure a = new SecretSharing.OptimalThreshold.Models.AccessStructure(nothresholdAccess);
            var thresholds = new List<SecretSharing.OptimalThreshold.Models.ThresholdSubset>();
            var remaining = new List<SecretSharing.OptimalThreshold.Models.QualifiedSubset>();
            SecretSharing.OptimalThreshold.ThresholdHelper.DetectThresholds(a,tryIntersect, out thresholds, out remaining);
            bool isEfficicent = SecretSharing.OptimalThreshold.ThresholdHelper.IsThresholdShareShorter(a, thresholds, remaining);


            //assert
            Assert.IsTrue(isEfficicent);
        }
        [TestMethod]
        public void Test_OptimalThreshold_IsThresholdShareShorterTestFail()
        {
            //assign 
            var nothresholdAccess = "P1^P2,P2^P3,P3^p4,p4^p5,p5^p6,p6^p7,p7^p8,p8^p1";
            var tryIntersect = true;
            //arrange
            SecretSharing.OptimalThreshold.Models.AccessStructure a = new SecretSharing.OptimalThreshold.Models.AccessStructure(nothresholdAccess);
            var thresholds = new List<SecretSharing.OptimalThreshold.Models.ThresholdSubset>();
            var remaining = new List<SecretSharing.OptimalThreshold.Models.QualifiedSubset>();
            SecretSharing.OptimalThreshold.ThresholdHelper.DetectThresholds(a,tryIntersect, out thresholds,out remaining);
            bool isEfficicent = SecretSharing.OptimalThreshold.ThresholdHelper.IsThresholdShareShorter(a,thresholds,remaining);

            //assert
            Assert.IsFalse(isEfficicent);
        }
        

    }
}
