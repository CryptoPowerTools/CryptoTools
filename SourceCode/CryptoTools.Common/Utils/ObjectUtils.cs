using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTools.Common.Utils
{
	/// <summary>
	/// Useful utilities that can be applied to your objects such as Reflection, Serializing etc.
	/// </summary>
	public class ObjectUtils
	{
		/// <summary>
		/// Returns a Clone of the object by making a Deep Copy of the object vs a Shallow Copy. This should return
		/// an exact replica or clone of the properties and the sub object graph.
		/// 
		/// IMPORTANT: The source MUST implement ISerializable and each of its contained object as well or you should
		///				expect a serialization error to be thrown.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="other"></param>
		/// <returns></returns>
		public static T DeepCopy<T>(T source)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(ms, source);
				ms.Position = 0;
				return (T)formatter.Deserialize(ms);
			}
		}


		/// <summary>
		/// Copies the Public Insance based properties from a [source] object to a [destination] object.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="destination"></param>
		/// <returns></returns>
		public static object MergeObjects(object source, object destination)
		{
			if (source != null && destination != null)
			{
				var t = source.GetType();

				foreach (PropertyInfo pi in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
				{				
					/////////////////////////////////////////////////
					// Handle Strings
					/////////////////////////////////////////////////
					if (pi.PropertyType == typeof(string))
					{
						// Seting Property
						string v = (string)pi.GetValue(source);
						if (!string.IsNullOrWhiteSpace(v))
						{
							pi.SetValue(destination, v);
						}

					}

					/////////////////////////////////////////////////
					// Handle Nullables
					/////////////////////////////////////////////////
					if (Nullable.GetUnderlyingType(pi.PropertyType) != null)
					{
						var v = pi.GetValue(source, null);
						if (v != null)
						{
							pi.SetValue(destination, pi.GetValue(source));
						}
					}
					
					/////////////////////////////////////////////////
					// Handle objects
					/////////////////////////////////////////////////
					if (pi.GetValue(source, null) != null)
					{
						var v = pi.GetValue(source, null);
						if (v != null)
						{
							pi.SetValue(destination, pi.GetValue(source));
						}
					}					
				}
			}
			return destination;
		}

		private bool IsNullableType(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
		}

	}

}

