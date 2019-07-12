using System;

namespace CryptoTools.Common.Utils
{
	/// <summary>
	/// DateTime helper methods
	/// </summary>
	public static class DateTimeUtils
	{
		// NOTE: 
		// This is the ONLY place in the code where you assign the Empty value.
		// This value can be changed if there is a good reason to for example. This value in SQL Server 
		// will throw an error so this could be changed to '01/01/1753' using SqlDateTime.MinValue if there is good reason.
		// The good news is that is you use the Empty property to check equality in your app... you can safely 
		// change the default value of _empty with no side effects.
		

		/// <summary>
		/// Represents an empty DateTime. This is the ONLY place in the application where the concept of "Empty"
		/// resides.
		/// </summary>
		public static readonly DateTime Empty = DateTime.MinValue;
		
		/// <summary>
		/// Converts a nullable DataTime to a DateTime object. If the value is null it return the DateTime.Empty value
		/// </summary>
		/// <param name="nullableDateTime"></param>
		/// <returns></returns>
		public static DateTime ToDateTime(DateTime? nullableDateTime)
		{
			if (nullableDateTime == null)
			{
				return Empty;
			}
			return nullableDateTime.Value;
		}

		/// <summary>
		/// Determines if the date time is an empty value
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static bool IsEmpty(DateTime dateTime)
		{
			////////////////////////
			// NOTE: Depending on the specific application you can add any 
			//		 conditionals here to represent a empty date time.
			///////////////////////////////////////////////////////////

			if (dateTime == Empty)
			{
				return true;
			}
			return false;
		}

		
		/// <summary>
		/// Strips the millisecond precision of the date time argument.
		/// 
		/// This can be very useful when you need to compare date times, especially since
		/// deserializing a date or reading from Sql Server will almost always have a ticks precision 
		/// error.
		/// </summary>
		/// <param name="dateTime"></param>
		public static DateTime StripMillisecondPrecision(ref DateTime dateTime)
		{
			dateTime = new DateTime((dateTime.Ticks / TimeSpan.TicksPerSecond) * TimeSpan.TicksPerSecond);
			return dateTime;
		}

		public static DateTime GetUtcNow()
		{
			return DateTime.UtcNow;
		}

		public static DateTime CoherceToDateTime(object input)
		{
			DateTime dateTime = DateTime.Parse(input.ToString());
			return dateTime;
		}

	}
}
