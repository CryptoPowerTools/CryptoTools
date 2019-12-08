using CryptoTools.Common.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptoTools.Common.UnitTests.Logging
{
	[TestClass]
	public class LoggerTests
	{
		[TestMethod]
		public void BasicUsage()
		{
			// Create a Logger. Th                               is would generally be done inside the application startup code a single time for each application
			ILog log = new DebugLogger();
			Logger.SetLogger(log);

			// Create and instance of the logger. This could be done for each class as some logging frameworks, 
			// such as Log4Net allow you to filter on types.
			ILog Log = Logger.CreateInstance(typeof(LoggerTests));

			// You can now use the logger in your code
			Log.Debug("Debug Message");
			Log.Error("Error Message");
			Log.Fatal("Fatal Message");
			Log.Info("Info Message");
			Log.Warn("Warn Message");
		}
	}
}
