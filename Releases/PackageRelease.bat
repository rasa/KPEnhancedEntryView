@echo off
set version=0.2
set output=%~dp0v%version%\
set zipfile="%output%KPEnhancedEntryView-v%version%.zip"
set buildoutputs="%~dp0Build Outputs"

rd /s /q "%output%"

copy "%~dp0..\Readme.txt" %buildoutputs%
copy "%~dp0..\COPYING" %buildoutputs%

rem don't include pdb files
del %buildoutputs%\*.pdb

pushd %buildoutputs%
"%ProgramFiles%\7-Zip\7z.exe" a -tzip -mx9 -bd %zipfile% *
popd
copy "%~dp0..\Readme.txt" "%output%"

set version=
set output=
set zipfile=
set buildoutputs=