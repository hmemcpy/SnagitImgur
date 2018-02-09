@echo off
set config=%1
if "%config%" == "" (
   set config=Release
)

set nuget=
if "%nuget%" == "" (
	set nuget=src\.nuget\nuget.exe
)

%nuget% restore src\SnagitImgur.sln
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild src\SnagitImgur.sln /t:Rebuild /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false
