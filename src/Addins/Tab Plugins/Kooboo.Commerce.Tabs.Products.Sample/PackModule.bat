del /s /q Bin\*.pdb
del /s /q Bin\*.xml

setlocal enabledelayedexpansion
for %%* in (.) do set CurrDirName=%%~nx*

7z\7z a -r %CurrDirName%.zip *.* -x@7z\ignores.txt

@pause
