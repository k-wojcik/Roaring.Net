@echo off
REM CRoaring
if not exist lib/CRoaring/build mkdir lib/CRoaring/build
cd lib/CRoaring/build
cmake -DCMAKE_GENERATOR_PLATFORM=x64 ..
MSBuild RoaringBitmap.sln /p:Configuration=ALL_BUILD
MSBuild RoaringBitmap.sln /p:Configuration=RUN_TESTS
copy roaring.dll ../../../src/CRoaring.Net/
copy roaring.dll ../../../test/CRoaring.Net.Test/
cd ../../../

REM CRoaring.Net
cd src/CRoaring.Net
dotnet restore
dotnet build
cd ../../

REM CRoaring.Net.Test
cd test/CRoaring.Net.Test
dotnet restore
dotnet test
cd ../../