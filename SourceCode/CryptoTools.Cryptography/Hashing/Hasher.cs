using System;
using CryptoTools.Common;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptoTools.Cryptography.Hashing
{
    /// <summary>
    /// Creates Hashes using the .NET Cryptography Api. This is a Facade class using the Defaults or you can customize it by passing in 
    /// a custom HasherOptions object.
    /// </summary>
    public class Hasher
	{
		#region Private Fields
		private HashAlgorithm _algorithm;
		#endregion

		#region Public Properties
		public HasherOptions Options { get; private set; }
		#endregion

		#region Constructors

		/// <summary>
		/// Creates a Hasher with the default settings of SHA256 algorithm and default options
		/// </summary>
		public Hasher() : this(null ,null)	{}

		
		/// <summary>
		/// Creates a Hasher with custom options.
		/// </summary>
		/// <param name="options"></param>
		public Hasher(HasherOptions options) : this(null, options) {}

		/// <summary>
		/// Creates a Hasher with a specified algorithm and custom options
		/// </summary>
		/// <param name="algorithm"></param>
		/// <param name="options"></param>
		public Hasher(HashAlgorithm algorithm = null, HasherOptions options = null)
		{
			_algorithm = algorithm != null ? algorithm : SHA256.Create();
			Options = HasherOptions.CreateMergedInstance(options);
		}

		public static bool IsHashValid(string key, HashAlgorithm algorithm = null)
		{
			if (string.IsNullOrEmpty(key)) return false;

			HashAlgorithm a = algorithm == null ? SHA256.Create() : algorithm;
			Hasher hasher = new Hasher(a);
			string hash = hasher.Hash("test");

			if (key.Length != hash.Length)
				return false;

			return true;
		}

		/// <summary>
		/// Get the Length of the Hash string that is returned from the Hash() method. This will vary based on the current algorithm
		/// </summary>
		/// <param name="algorithm"></param>
		/// <returns></returns>
		public static int CalculateHashLength(HashAlgorithm algorithm = null)
		{			
			HashAlgorithm a = algorithm == null ? SHA256.Create() : algorithm;
			Hasher hasher = new Hasher(a);
			string hash = hasher.Hash("test");
			return hash.Length;			
		}

		/// <summary>
		/// Get the Length of the Hash Bytes that is returned from the HashToBytes() method. This will vary based on the current algorithm
		/// </summary>
		/// <param name="algorithm"></param>
		/// <returns></returns>
		public static int CalculateHashBytesLength(HashAlgorithm algorithm = null)
		{
			HashAlgorithm a = algorithm == null ? SHA256.Create() : algorithm;
			Hasher hasher = new Hasher(a);
			byte[] bytes = hasher.HashToBytes(new byte[] { 0x00 });
			return bytes.Length;
		}

		#endregion

		#region Public Hash Methods

		/// <summary>
		/// Hashes a String and returns a hash finger print based on the current Hash Algorithm
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public string Hash(string data)
		{
			string hash;
			using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
			{
				hash = Hash(stream);
			}
			return hash;
		}

		/// <summary>
		/// Hashes an Int32 and return a hash fingerprint based on the current Hash Algorithm
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public string Hash(int data)
		{
			byte[] bytes = BitConverter.GetBytes(data);
			return Hash(bytes);
		}



		/// <summary>
		/// Hashes a Stream and returns a hash finger print based on the current Hash Algorithm
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public string Hash(Stream stream)
		{
			string hash = "";
			Byte[] bytes = _algorithm.ComputeHash(stream);
			hash = BytesToHashSignature(bytes); 
			return hash;
		}

		/// <summary>
		/// Hashes an array of bytes (byte[]) and returns a hash finger print based on the current Hash Algorithm
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public string Hash(byte[] bytes)
		{
			string hash;
			using (MemoryStream stream = new MemoryStream(bytes))
			{
				hash = Hash(stream);
			}
			return hash;
		}

		/// <summary>
		/// Hashes an array of bytes and returns a hash byte array based on the current Hash Algorithm
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public byte[] HashToBytes(byte[] bytes)
		{
			byte[] buffer;
			using (MemoryStream stream = new MemoryStream(bytes))
			{
				buffer = HashToBytes(stream);
			}
			return buffer;
		}

		/// <summary>
		/// Hashes a Stream and returns a hash byte array based on the current Hash Algorithm
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public byte[] HashToBytes(Stream stream)
		{
			Byte[] bytes = _algorithm.ComputeHash(stream);
			return bytes;
		}

		/// <summary>
		/// Hashes a String and returns a hash byte array based on the current Hash Algorithm
		/// </summary>
		/// <param name="stringData"></param>
		/// <returns></returns>
		public byte[] HashToBytes(string stringData)
		{
			byte[] buffer;
			using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(stringData)))
			{
				buffer = HashToBytes(stream);
			}
			return buffer;
		}

		/// <summary>
		/// Hashes a String multiple Iterations determined by the argument iterations. 
		/// This is only recommended for small peices of data such as passwords, keys etc.
		/// For a small String, you can safely hash a few thousands of times if you want to create 
		/// a more secure hash that would be virtually impossible to guess.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="iterations"></param>
		/// <returns></returns>
		public string MultiHash(string message, int iterations)
		{
			string hash = Hash(message);

			for (int i = 0; i < iterations - 1; i++)
			{
				hash = Hash(hash);
			}
			return hash;
		}

		/// <summary>
		/// Enhanced secure hashing algorithm that mixes multi iterations and multi algorithm 
		/// types to create a more secure algorithm that should be more secure than a standard
		/// Shw256 Hash that can be easily hacked using a table / dictionary lookup attacks.
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public string SecureHash(string message, int executionMillisecondsApprox = 500)
		{

			// NOTE: Adjust the SpeedFactor as required.  This is estimated off a standard developer machine i7 Processor
			//		 You can use the Unit Test to fine tune this in the future as machine speed increases... if this code is running 
			//		 on a fast server the executionTime will obviously go down, but this should be fine for its intended purpose.
			const int SpeedFactor = 40;

			int iterations = executionMillisecondsApprox * SpeedFactor;
			string hash = message;
			string prevHash;

			// Create Additional Hashers for Complexity
			Hasher sha512 = new Hasher(SHA512.Create());
		
			Stopwatch sw = Stopwatch.StartNew();
			for (int i = 0; i < iterations - 1; i++)
			{
				hash = Hash(hash); // Hash using the current algorithm
				prevHash = sha512.Hash(hash); // Re-Hash using the SAH512 Algorithm
				hash = Hash(hash + prevHash); // Append the 2 hashes together and Re-Hash using current algorithm
			}
			Debug.WriteLine($"SecureHash execution time in milliseconds = {sw.ElapsedMilliseconds}");

			return hash;
		}
		
		public string HashFile(string fileName)
		{
			string hash = "";
			using (FileStream stream = File.OpenRead(fileName))
			{
				hash = Hash(stream);
			}
			return hash;
		}

		public string HashDirectoryTree(string directoryName)
		{
			string hash = "";
			Stack<string> stack;
			string[] files;
			string[] directories;
			string dir;

			stack = new Stack<string>();
			stack.Push(directoryName);

			while (stack.Count > 0)
			{
				// Pop a directory to process
				dir = stack.Pop();

				files = Directory.GetFiles(dir);
				Array.Sort<string>(files);
				foreach (string file in files)
				{
					string tempHashSignature = HashFile(file);
					// Calculate a new Hash Signature based on the previous Hash
					hash = Hash(tempHashSignature + hash);
					//Debug.WriteLine($"{hash} - {file}");
				}

				directories = Directory.GetDirectories(dir);
				Array.Sort<string>(directories);
				foreach (string directory in directories)
				{
					// Push each directory into stack
					stack.Push(directory);
				}
			}
			return hash;
		}

		/// <summary>
		/// Compares the Hash Signatures of 2 Directories and returns true if they contain the same contents or false if the contents different.
		/// </summary>
		/// <param name="directory1"></param>
		/// <param name="directory2"></param>
		/// <returns></returns>
		public bool CompareDirectoryHashSignatures(string directory1, string directory2)
		{
			bool isEquals = false;

			string hashSig1;
			string hashSig2;

			try
			{
				hashSig1 = HashDirectoryTree(directory1);
				hashSig2 = HashDirectoryTree(directory2);
				isEquals = hashSig1.Equals(hashSig2);
			}
			catch
			{
				isEquals = false;
			}

			return isEquals;
		}

		/// <summary>
		/// Returns and instance of the current Algorithm
		/// </summary>
		/// <returns></returns>
		public HashAlgorithm GetAlgorithm()
		{
			return _algorithm;
		}

		#endregion

		#region Private Methods
		/// <summary>
		/// Converts a byte array into a readable string hash fingerprint / signature
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		private string BytesToHashSignature(byte[] bytes)
		{
			StringBuilder builder = new StringBuilder();
			foreach (Byte b in bytes)
			{
				if (Options.HashFingerprintLowercase.Value)
					builder.Append(b.ToString("x2"));
				else
					builder.Append(b.ToString("X2"));
			}
			return builder.ToString();
		}
		#endregion		
	}
}
