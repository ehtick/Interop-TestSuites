@echo off
pushd %~dp0
"%VS120COMNTOOLS%..\IDE\mstest" /test:Microsoft.Protocols.TestSuites.MS_OXWSCORE.S02_ManageContactItems.MSOXWSCORE_S02_TC17_VerifyGetItemWithItemResponseShapeType_IncludeMimeContentBoolean /testcontainer:..\..\MS-OXWSCORE\TestSuite\bin\Debug\MS-OXWSCORE_TestSuite.dll /runconfig:..\..\MS-OXWSCORE\MS-OXWSCORE.testsettings /unique
pause