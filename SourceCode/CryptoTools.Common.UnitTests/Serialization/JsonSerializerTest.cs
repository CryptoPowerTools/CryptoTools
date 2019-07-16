using CryptoTools.Common.Serialization;
using CryptoTools.Common.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoTools.Common.UnitTests.Serialization
{



	internal class TestData
	{
		public int? IntValue { get; set; }
		public double? DoubleValue { get; set; }
		public bool? BoolValue { get; set; }
		public string StringValue { get; set; }
	}



	[TestClass]
	public class JsonSerializerTest
	{

		/// <summary>
		/// Provides basic usage of how to perform most operations using the serializer
		/// </summary>
		[TestMethod]
		public void JsonSerializer_BasicUsage()
		{
			TestData truthData = new TestData
			{
				BoolValue = true,
				IntValue = 777777777,
				DoubleValue = 777777.777777,
				StringValue = "My String Data 7777"
			};

			// Serilize and Deserialize
			string serializedData = JsonSerializer.Serialize(truthData);
			TestData deserializedData = JsonSerializer.DeserializeObject(serializedData, typeof(TestData)) as TestData;

			// Compare they should be the same
			CompareObjects comparer = new CompareObjects();
			Assert.IsTrue(comparer.Compare(truthData, deserializedData));



			// USING GENERICS! Serilize and Deserialize
			serializedData = JsonSerializer.Serialize(truthData);
			deserializedData = JsonSerializer.DeserializeObject<TestData>(serializedData);

			// Compare they should be the same
			comparer = new CompareObjects();
			Assert.IsTrue(comparer.Compare(truthData, deserializedData));


		}

	}
}
