using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Numerics;
namespace SecretSharing.Test.Core.PVSS
{
    [TestClass]
    public class SchoenmakersTest
    {
         int n;
         int t;
         int fieldSize;
         SecretSharingCore.Algorithms.PVSS.Schoenmakers sch;
         SecretSharingCore.Algorithms.PKE.PublicKeyEncryption pke;
         List<Tuple<byte[], byte[]>> keypairs;



        [TestInitialize]
         public void Test_Schoenmakers_Initialise()
        {     
            fieldSize = 128;
            n = 20;
            t = 5; 
            
            sch = new SecretSharingCore.Algorithms.PVSS.Schoenmakers();
            sch.SelectPrimeAndGenerators(fieldSize);
            pke = new SecretSharingCore.Algorithms.PKE.PublicKeyEncryption();
            keypairs = new List<Tuple<byte[],byte[]>>();
            for (int i = 0; i < n; i++)
            {
                var pair = pke.GenerateKeyPair(sch.GetqB(), sch.GetGB());
                keypairs.Add(pair);
            }
            sch.SetPublicKeys(keypairs.Select(po=>po.Item2).ToList());
        }

        [TestMethod]
        public void Test_Schoenmakers_Distribute()
        {
            List<byte[]> Commitments = new List<byte[]>();
            byte[] secret = null;
            var shares = sch.Distribute(t, n, ref Commitments,ref secret);

            Assert.AreEqual(shares.Count, n);
        }

        [TestMethod]
        public void Test_Schoenmakers_SpecialDistribute()
        {
            List<byte[]> Commitments = new List<byte[]>();
            //user defined secret
            var sigma = "1234567890";
            var byteSigma = Encoding.UTF8.GetBytes(sigma);
            byte[] U = null;
            byte[] secret = null;
            var shares = sch.Distribute(t, n,byteSigma, ref Commitments,ref secret,ref U);

            Assert.AreEqual(shares.Count, n);
        }

        [TestMethod]
        public void Test_Schoenmakers_VerifyDistributed()
        {
            List<byte[]> Commitments = new List<byte[]>();
            byte[] secret=null;
            var shares = sch.Distribute(t, n, ref Commitments,ref secret);
            for (int i = 0; i < shares.Count; i++)
			{
                bool verifiedShare = sch.VerifyDistributedShare(i+1,shares[i],Commitments, keypairs[i].Item2);
                Assert.IsTrue(verifiedShare);
			}
            
        }
        [TestMethod]
        public void Test_Schoenmakers_PoolShares()
        {
            List<byte[]> Commitments = new List<byte[]>();
            byte[] secret = null;
            var shares = sch.Distribute(t, n, ref Commitments,ref secret);
            for (int i = 0; i < shares.Count; i++)
            {
                var share =  shares[i];
                sch.PoolShare(keypairs[i], ref share);
                Assert.IsTrue(share.IsPooled());
            }
        }
        [TestMethod]
        public void Test_Schoenmakers_VerifyPooledShares()
        {
            List<byte[]> Commitments = new List<byte[]>();
            byte[] secret = null;
            var shares = sch.Distribute(t, n, ref Commitments,ref secret);
            List<SecretSharingCore.Algorithms.PVSS.SchoenmakersShare> pooledShares = new List<SecretSharingCore.Algorithms.PVSS.SchoenmakersShare>();
            for (int i = 0; i < shares.Count; i++)
            {
                var share = shares[i];
                sch.PoolShare(keypairs[i], ref share);
                pooledShares.Add(share);
            }

            var allVerified = sch.VerifyPooledShares(t, pooledShares);
            Assert.IsTrue(allVerified);
        }
        [TestMethod]
        public void Test_Schoenmakers_ReconstructSecret()
        {
            List<byte[]> Commitments = new List<byte[]>();
            byte[] secret = null;
            var shares = sch.Distribute(t, n, ref Commitments,ref secret);
            for (int i = 0; i < shares.Count; i++)
            {
                var share = shares[i];
                sch.PoolShare(keypairs[i], ref share);

            }

            var recon = sch.Reconstruct(t, shares);
            Assert.IsNotNull(recon);
            var successfullyRecon = secret.SequenceEqual(recon);
            Assert.IsTrue(successfullyRecon);
        }
        [TestMethod]
        public void Test_Schoenmakers_SpecialReconstructSecret()
        {
            var bQ = new BigInteger(sch.GetqB());
            List<byte[]> Commitments = new List<byte[]>();
            //user defined secret
            var sigma = "1234567890";
            var byteSigma = Encoding.UTF8.GetBytes(sigma);
            BigInteger bSigma = new BigInteger(byteSigma);
            byte[] U = null;
            byte[] secret = null;
            var shares = sch.Distribute(t, n, byteSigma, ref Commitments,ref secret, ref U);
            BigInteger bU = new BigInteger(U);
            BigInteger bSecret = new BigInteger(secret);
            var pooledShares = new List<SecretSharingCore.Algorithms.PVSS.SchoenmakersShare>();
            for (int i = 0; i < shares.Count; i++)
            {
                var share = shares[i];
                sch.PoolShare(keypairs[i], ref share);
                pooledShares.Add(share);
            }

            var recon = sch.Reconstruct(t, pooledShares, U);
            Assert.IsNotNull(recon);
            var successfullyRecon = byteSigma.SequenceEqual(recon);
            Assert.IsTrue(successfullyRecon);
        }
    }
}
