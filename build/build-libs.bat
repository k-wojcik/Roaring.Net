vcpkg install --triplet=x64-windows
xcopy .\vcpkg_installed\x64-windows\bin\*.dll .\libs\x64-windows\ /I /S /Y
vcpkg install --triplet=x86-windows
xcopy .\vcpkg_installed\x86-windows\bin\*.dll .\libs\x86-windows\ /I /S /Y