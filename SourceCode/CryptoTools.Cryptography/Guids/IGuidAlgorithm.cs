namespace CryptoTools.Cryptography.Guids
{
	public interface IGuidAlgorithm
	{
		string NewGuid();
		bool Verify(string guid);
	}
}
