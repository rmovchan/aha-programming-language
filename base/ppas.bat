@echo off
SET THEFILE=base.dll
echo Linking %THEFILE%
C:\lazarus\fpc\2.6.0\bin\i386-win32\ld.exe -b pei-i386 -m i386pe  --gc-sections  -s --dll  --entry _DLLMainCRTStartup   --base-file base.$$$ -o base.dll link.res
if errorlevel 1 goto linkend
C:\lazarus\fpc\2.6.0\bin\i386-win32\dlltool.exe -S C:\lazarus\fpc\2.6.0\bin\i386-win32\as.exe -D base.dll -e exp.$$$ --base-file base.$$$ 
if errorlevel 1 goto linkend
C:\lazarus\fpc\2.6.0\bin\i386-win32\ld.exe -b pei-i386 -m i386pe  -s --dll  --entry _DLLMainCRTStartup   -o base.dll link.res exp.$$$
if errorlevel 1 goto linkend
C:\lazarus\fpc\2.6.0\bin\i386-win32\postw32.exe --subsystem console --input base.dll --stack 16777216
if errorlevel 1 goto linkend
goto end
:asmend
echo An error occured while assembling %THEFILE%
goto end
:linkend
echo An error occured while linking %THEFILE%
:end
