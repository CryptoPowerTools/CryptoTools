using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CryptoTools.Cryptography.Guids;

namespace CryptoTools.Cryptography.UnitTests.Guids
{
	[TestClass]
	public class GuidTests
	{
		[TestMethod]
		public void CryptoGuid_Algorithms_BasicUsage()
		{
			IGuidAlgorithm algorithm = null;

			// DotNetGuid Algorithm
			algorithm = new DotNetGuidAlgorithm();
			string guid = algorithm.NewGuid();
			bool isValid = algorithm.Verify(guid);
			
			// Md5 Algorithm
			algorithm = new Md5GuidAlgorithm();
			guid = algorithm.NewGuid();
			isValid = algorithm.Verify(guid);
			
			// Sha256 Algorithm
			algorithm = new Sha256GuidAlgorithm();
			guid = algorithm.NewGuid();
			isValid = algorithm.Verify(guid);
		}

		[TestMethod]
		public void CryptoGuid_BasicUsage()
		{
			//// Application Set Defaults
			CryptoGuid.SetDefaultAlgorithm(new DotNetGuidAlgorithm()); // Set Once in the Application ... throw exception on 2nd time.
			
			//// Instance Based w/ No Factor Defaults
			string guid = CryptoGuid.NewGuid();
			bool isValid = CryptoGuid.Verify(guid);

		}


		[TestMethod]
		public void CryptoGuid_FullTests()
		{
			string guid = null;
			CryptoGuid.EnforceSingleSetDefaultAlgorithm = false;

			CryptoGuid.SetDefaultAlgorithm(new Sha256GuidAlgorithm());
			guid = CryptoGuid.NewGuid();
			Assert.IsTrue(CryptoGuid.Verify(guid));

			CryptoGuid.SetDefaultAlgorithm(new Md5GuidAlgorithm());
			guid = CryptoGuid.NewGuid();
			Assert.IsTrue(CryptoGuid.Verify(guid));

			CryptoGuid.SetDefaultAlgorithm(new DotNetGuidAlgorithm());
			guid = CryptoGuid.NewGuid();
			Assert.IsTrue(CryptoGuid.Verify(guid));

		}
	}
}
