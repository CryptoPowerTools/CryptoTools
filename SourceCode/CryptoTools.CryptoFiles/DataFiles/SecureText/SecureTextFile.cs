using CryptoTools.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTools.CryptoFiles.DataFiles
{

	/// <summary>
	/// Secure Text Data File is specific implementation of CryptoDataFile that allows you to store plain text in an encrypted file.
	/// </summary>
	public class SecureTextFile : CryptoDataFile
	{	

		public SecureTextFile(string fullFileName) : base(new CryptoDataFileOptions { ContentFormat= new byte[] { 0xB, 0x0 } }, fullFileName)
		{
			FullFileName = fullFileName;
		}


		public void WriteAllText(string text)
		{
			Content = StringUtils.TextToBytes(text);
			Save();
			
		}

		public void WriteAllLines(string[] lines)
		{
			WriteAllText(StringUtils.StringArrayToString(lines, true));
		}

		public void WriteAllLines(IList<string> lines)
		{
			WriteAllLines(lines.ToArray());
		}


		public IList<string> ReadAllLines()
		{
			return StringUtils.StringToStringArray(ReadAllText());
		}		

		public string ReadAllText()
		{
			Load();
			string text = StringUtils.BytesToText(Content);
			return text;
		}


		
	}
}
