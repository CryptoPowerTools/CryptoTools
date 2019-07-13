using System;

namespace CryptoTools.Common.Utils
{

	/// <summary>
	/// Contains and assortment of Exception helper methods that might be useful.
	/// </summary>
	public static class ExceptionUtils
	{

		/// <summary>
		/// Return a friendly name of the class that you can use in your exception messages.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		private static string GetClassName(object obj)
		{
			string formattedClassName;

			if (obj == null)
				return "";

			try
			{
				formattedClassName = obj.GetType().Name;
			}
			catch (Exception)
			{
				throw;
			}
			return formattedClassName;
		}

		/// <summary>
		/// Returns a friendly string that you can use at the beginning of your error message.
		/// </summary>
		/// <example>
		/// ErrorPrefix(this) + "Method failed  to start a process"
		/// 
		/// This would result in an error message like :
		///		
		///		Error occurred in [YourClass] - Method failed to start process
		/// </example>
		/// <param name="classReference"></param>
		/// <returns></returns>
		private static string FormatErrorPrefix(object classReference, string methodName)
		{
			if (classReference == null)
			{
				return "";
			}

			return FormatErrorPrefix(classReference.GetType(), methodName);
		}

		/// <summary>
		/// Returns a friendly string that you can use at the beginning of your error message.
		/// </summary>
		/// <example>
		/// ErrorPrefix(typeof(YourClass)) + "Method failed  to start a process"
		/// 
		/// This would result in an error message like :
		///		
		///		Error occurred in [YourClass] - Method failed to start process
		/// </example>
		/// <param name="objectType"></param>
		/// <returns></returns>
		private static string FormatErrorPrefix(Type objectType, string methodName)
		{
			string messagePrefix = string.Format("Error occurred in [{0}.{1}] - ", GetClassName(objectType), methodName);
			return messagePrefix;
		}

		/// <summary>
		/// Builds a nicely formated error description with the class and method name.
		/// </summary>
		/// <param name="className"></param>
		/// <param name="methodName"></param>
		/// <param name="e"></param>
		/// <returns></returns>
		private static string FormatErrorMessage(string className, string methodName, Exception e)
		{
			string message = FormatErrorPrefix(className, methodName);
			message += e == null ? "" : e.Message;
			return message;
		}

		public static string FormatErrorMessage(object className, string methodName, string errorMessage)
		{
			string message = FormatErrorPrefix(className, methodName);
			message += errorMessage;
			return message;
		}


		public static string FormatErrorMessage(object classReference, string methodName, Exception e, string customMessage = "")
		{
			string className = GetClassName(classReference);

			return FormatErrorMessage(className, methodName, e, customMessage);
		}



		public static string FormatErrorMessage(string className, string methodName, Exception e, string customMessage = "")
		{
			string message = FormatErrorPrefix(className, methodName);

			if (!string.IsNullOrWhiteSpace(customMessage))
			{
				message += " Message: " + customMessage;
			}

			if (e != null)
			{
				message += " Exception Message: " + e.Message;

				// Now if the exception has inner exceptions append those too.
				if (e.InnerException != null)
				{
					message += " Inner Exception Message: " + e.InnerException.Message;

					///TODO - Add some additional logic to iterate more inner exception if you have them
				}
			}



			return message;
		}

	}
}
