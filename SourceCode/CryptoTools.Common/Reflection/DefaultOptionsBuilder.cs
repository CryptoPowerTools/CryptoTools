using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTools.Common.Reflection
{
	/// <summary>
	/// Default Options Builder implements a common design pattern commonly used in JavaScript libraries. You have Default Options, Override / Extended Options and then a 
	/// resulting Merged Options which contains a merge of the Default Options with any Overridden Options taking precedence. This pattern make it easy to set an application level
	/// set of default options and still provide each instance the ability to customize the instance by overriding the defaults.
	/// 
	/// Implementation Note: When you build your Options class you must use a Nullable<> for any Property that cannot be set to nul. For example Int, Bool, Double, DateTime are some examples.
	/// 
	/// </summary>
	public class DefaultOptionsBuilder
	{
		static public T MergeOptions<T>(T defaultOptions, T overrideOptions) where T : new()
		{
			if (defaultOptions == null) throw new ArgumentNullException(nameof(defaultOptions), "Argument must not be null");
			if (overrideOptions == null) overrideOptions = new T();

			T merged = new T();

			if (defaultOptions != null && overrideOptions != null)
			{
				foreach (PropertyInfo pi in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
				{
					Type t = pi.PropertyType;

					if (t == typeof(int) || t == typeof(bool) || t == typeof(double) || t == typeof(long) || t == typeof(int) || t == typeof(int) || t == typeof(int))
					{
						// throw an exception since These primitives must be declared using Nullable<>
						throw new Exception("These primitives must be declared using Nullable<>");
					}

					var overrideValue = pi.GetValue(overrideOptions, null);
					if (overrideValue != null || !string.IsNullOrWhiteSpace((string)overrideValue))
					{
						// Override Not set so use the default value
						pi.SetValue(merged, overrideValue);
					}
					else
					{
						// Override IS SET so use the extended value
						pi.SetValue(merged, pi.GetValue(defaultOptions));
					}
				}
			}
			return merged;
		}


		private bool IsNullableType(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
		}

	}
}