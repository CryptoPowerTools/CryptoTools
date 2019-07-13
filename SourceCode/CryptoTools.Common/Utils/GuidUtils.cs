using System;

namespace CryptoTools.Common.Utils
{
	/// <summary>
	/// Useful wrapper around a Guid can be used in your application.
	/// </summary>
	public class GuidUtils
	{
		/// <summary>
		/// converts a nullable Guid to a guid
		/// </summary>
		/// <param name="nullableGuid"></param>
		/// <returns></returns>
		public static Guid ToGuid(Guid? nullableGuid)
		{
			if (nullableGuid == null || !nullableGuid.HasValue)
			{
				return Guid.Empty;
			}
			return nullableGuid.Value;

		}

		public static Guid ToGuid(string guid)
		{
			return Guid.Parse(guid);
		}

		public static bool IsEmpty(Guid guid)
		{
			return guid.Equals(Guid.Empty);
		}


		public static Guid NewId()
		{
			return Guid.NewGuid();
		}

		public static readonly Guid Empty = new Guid("{00000000-0000-0000-0000-000000000000}");

		public static Guid NewGuid()
		{
			return Guid.NewGuid();
		}


		public static string GetPrefixString(Guid guid, int count = 3)
		{
			string prefix = guid.ToString().Substring(0, count);
			return prefix;
		}

	}
}
