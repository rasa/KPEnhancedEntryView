@echo off
cd %~dp0

echo Deleting existing PlgX folder
rmdir /s /q PlgX

echo Creating PlgX folder
mkdir PlgX

echo Copying files
xcopy KPEnhancedEntryView PlgX /s /e /exclude:PlgXExclude.txt

echo Compiling PlgX
"KeePass/KeePass.exe" /plgx-create "%~dp0PlgX" --plgx-prereq-os:Windows --plgx-prereq-kp:2.39

echo Releasing PlgX
mkdir "Releases/Build Outputs"
move /y PlgX.plgx "Releases\Build Outputs\KPEnhancedEntryView.plgx"

echo Cleaning up
rmdir /s /q PlgX
