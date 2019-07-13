

set DestinationFolder=c:\LocalNuGet\


xcopy /v /y ..\SourceCode\CryptoTools.Common\bin\Release\*.nupkg %DestinationFolder%
xcopy /v /y ..\SourceCode\CryptoTools.Cryptography\bin\Release\*.nupkg %DestinationFolder%
xcopy /v /y ..\SourceCode\CryptoTools.CryptoArchivers\bin\Release\*.nupkg %DestinationFolder%
xcopy /v /y ..\SourceCode\CryptoTools.CryptoFiles\bin\Release\*.nupkg %DestinationFolder%

pause
