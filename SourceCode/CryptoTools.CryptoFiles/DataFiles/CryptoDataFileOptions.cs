using CryptoTools.Common.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTools.CryptoFiles.DataFiles
{
	/// <summary>
	/// CryptoDataFileOptions allows you to customize eash instance of the CryptoDataFile as required. This is useful since defining new File Format can be as easy as customizing
	/// the Options and creating a new instance.
	/// </summary>
	public class CryptoDataFileOptions
	{
		public byte[] FileFormat { get; set; }
		public int? FileVersion { get; set; }
		public byte[] ContentFormat { get; set; }
		public int? ContentVersion { get; set; }
		public byte[] EndFileFormat { get; set; }
		
		public CryptoDataFileOptions()
		{
		}

		public static CryptoDataFileOptions CreateMergedInstance(CryptoDataFileOptions options = null)
		{
			// Merge Options with Global Options
			CryptoDataFileOptions result = DefaultOptionsBuilder.MergeOptions<CryptoDataFileOptions>(GlobalCryptoFilesOptions.CryptoDataFileOptions, options);
			return result;
		}
	}

}

