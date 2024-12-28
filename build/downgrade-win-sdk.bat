REM https://github.com/actions/runner-images/issues/10981

for /f "usebackq tokens=*" %%i in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -legacy -property resolvedInstallationPath`) do (
	set VSinstallDir=%%i
)

set VSCMD_ARG_HOST_ARCH=x64
set VSCMD_ARG_TGT_ARCH=x64
call "%VSinstallDir%\Common7\Tools\vsdevcmd\core\winsdk.bat"

SET "Win10Lib=C:\Program Files (x86)\Windows Kits\10\Lib"
robocopy "%Win10Lib%\10.0.22621.0\ucrt\arm" "%Win10Lib%\%WindowsSDKVersion%ucrt\arm" /E 1>NUL
IF %ERRORLEVEL% GEQ 8 (EXIT /B %ERRORLEVEL%) ELSE (SET ERRORLEVEL=0)

robocopy "%Win10Lib%\10.0.22621.0\um\arm" "%Win10Lib%\%WindowsSDKVersion%um\arm" /E 1>NUL
IF %ERRORLEVEL% GEQ 8 (EXIT /B %ERRORLEVEL%) ELSE (SET ERRORLEVEL=0)