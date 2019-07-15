using Newtonsoft.Json;
using System;
using System.IO;

namespace Formula.Core.Serialization
{
	/// <summary>
	/// Provides a centralized place for all serizlization code with EASY to use methods. This also abstract any dependencies 
	/// from your application should you need to change the implementation down the road or future versions
	/// of .NET
	/// </summary>
	public class JsonSerializer
	{
		public static string SerializeObject<T>(T obj)
		{
			return JsonConvert.SerializeObject(obj, Formatting.Indented);

		}

		public static T DeserializeObject<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json);
		}
		public static object DeserializeObject(string json, Type type)
		{
			return JsonConvert.DeserializeObject(json, type);
		}

		public static string Serialize(object obj)
		{
			string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
			return json;
		}


		public static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
		{
			TextWriter writer = null;
			try
			{
				var contentsToWriteToFile = SerializeObject<T>(objectToWrite);
				writer = new StreamWriter(filePath, append);
				writer.Write(contentsToWriteToFile);
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				if (writer != null)
					writer.Close();
			}
		}


		/// <summary>
		/// Reads an object instance from an Json file.
		/// <para>Object type must have a parameterless constructor.</para>
		/// </summary>
		/// <typeparam name="T">The type of object to read from the file.</typeparam>
		/// <param name="filePath">The file path to read the object instance from.</param>
		/// <returns>Returns a new instance of the object read from the Json file.</returns>
		public static T ReadFromJsonFile<T>(string filePath) where T : new()
		{
			TextReader reader = null;
			try
			{
				reader = new StreamReader(filePath);
				var fileContents = reader.ReadToEnd();
				return DeserializeObject<T>(fileContents);
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				if (reader != null)
					reader.Close();
			}
		}



	}
}
