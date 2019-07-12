using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTools.Cryptography.Guids
{
	public interface IGuidAlgorithm
	{
		string NewGuid();
		bool Verify(string guid);
	}
}
