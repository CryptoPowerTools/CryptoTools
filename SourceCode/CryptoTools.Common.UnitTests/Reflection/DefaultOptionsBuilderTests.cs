using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CryptoTools.Common.Utils;
using System.Reflection;
using System.Diagnostics;
using CryptoTools.Common.Reflection;

namespace CryptoTools.Common.UnitTests.Reflection
{


	internal class TestClass
	{	
		public int? IntValue { get; set; }
		public double? DoubleValue { get; set; }
		public bool? BoolValue { get; set; }
		public string StringValue { get; set; }
	}



	[TestClass]
	public class DefaultOptionsBuilderTests
	{
		[TestMethod]
		public void ObjectUtils_MergeObjects_BasicUsage()
		{

		

			/////////////////////////////////////
			// Default Value
			/////////////////////////////////////
			TestClass defaultOptions = new TestClass
			{
				
				BoolValue = false,
				DoubleValue = 1.1,
				IntValue = 11, 
				StringValue = "1111"
			};


			///////////////////////////////////////////////
			// Override Test 1
			///////////////////////////////////////////////
			TestClass extendedOptions = new TestClass
			{
				DoubleValue = 2.2,
			};
			TestClass merged = Common.Reflection.DefaultOptionsBuilder.MergeOptions<TestClass>(defaultOptions, extendedOptions);
			Assert.IsTrue(merged.BoolValue == false);
			Assert.IsTrue(merged.DoubleValue == 2.2);
			Assert.IsTrue(merged.IntValue == 11);
			Assert.IsTrue(merged.StringValue == "1111");


			///////////////////////////////////////////////
			// Override Test 2
			///////////////////////////////////////////////
			extendedOptions = new TestClass
			{
				BoolValue = true,
				StringValue = "2222"
			};
			merged = Common.Reflection.DefaultOptionsBuilder.MergeOptions<TestClass>(defaultOptions, extendedOptions);
			Assert.IsTrue(merged.BoolValue == true);
			Assert.IsTrue(merged.DoubleValue == 1.1);
			Assert.IsTrue(merged.IntValue == 11);
			Assert.IsTrue(merged.StringValue == "2222");


			///////////////////////////////////////////////
			// Override Test 3
			///////////////////////////////////////////////
			extendedOptions = new TestClass
			{
				IntValue = 22				
			};
			merged = Common.Reflection.DefaultOptionsBuilder.MergeOptions<TestClass>(defaultOptions, extendedOptions);
			Assert.IsTrue(merged.BoolValue == false);
			Assert.IsTrue(merged.DoubleValue == 1.1);
			Assert.IsTrue(merged.IntValue == 22);
			Assert.IsTrue(merged.StringValue == "1111");




			// Perform Merge
			//merged = ObjectUtils.MergeObjects(o1, o2) as TestClass;


		}

		[TestMethod]
		public void ObjectUtils_MergeObjects_NullTest()
		{
			/////////////////////////////////////
			// Default Value
			/////////////////////////////////////
			TestClass defaultOptions = new TestClass
			{

				BoolValue = false,
				DoubleValue = 1.1,
				IntValue = 11,
				StringValue = "1111"
			};


			///////////////////////////////////////////////
			// Override Test 1
			///////////////////////////////////////////////
			TestClass extendedOptions = null;
			TestClass merged = DefaultOptionsBuilder.MergeOptions<TestClass>(defaultOptions, extendedOptions);
			Assert.IsTrue(merged.BoolValue == false);
			Assert.IsTrue(merged.DoubleValue == 1.1);
			Assert.IsTrue(merged.IntValue == 11);
			Assert.IsTrue(merged.StringValue == "1111");

		}
	}
}
