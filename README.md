# Roaring.Net

[![push-master workflow status](https://img.shields.io/github/actions/workflow/status/k-wojcik/Roaring.Net/push-master.yml)](https://github.com/k-wojcik/Roaring.Net/actions/workflows/push-master.yml)
[![GitHub Release](https://img.shields.io/github/v/release/k-wojcik/Roaring.Net?include_prereleases&sort=semver)](https://github.com/k-wojcik/Roaring.Net/releases)
[![NuGet Version](https://img.shields.io/nuget/vpre/Roaring.Net)](https://www.nuget.org/packages/Roaring.Net)
[![Codecov](https://img.shields.io/codecov/c/github/k-wojcik/Roaring.Net?token=ITL13NZGAH)](https://codecov.io/github/k-wojcik/Roaring.Net)
[![CRoaring version](https://img.shields.io/badge/dynamic/json?url=https%3A%2F%2Fraw.githubusercontent.com%2Fk-wojcik%2FRoaring.Net%2Frefs%2Fheads%2Fmaster%2Fvcpkg.json&query=%24.overrides%5B0%5D.version&label=CRoaring)](https://github.com/RoaringBitmap/CRoaring/releases)

A .NET wrapper for [CRoaring](https://github.com/RoaringBitmap/CRoaring) - a C implementation of [RoaringBitmap](https://github.com/RoaringBitmap/RoaringBitmap).

## Support
### Supported runtimes
| Platform/Architecture |      **x86**       |      **x64**       |      **arm**       |     **arm64**      |
|:---------------------:|:------------------:|:------------------:|:------------------:|:------------------:|
|      **windows**      | :white_check_mark: | :white_check_mark: | :white_check_mark: | :white_check_mark: |
|       **linux**       |                    | :white_check_mark: | :white_check_mark: | :white_check_mark: |
|        **osx**        |                    | :white_check_mark: |                    | :white_check_mark: |

### Supported frameworks
* .NET 8
* .NET 6

## Usage
```cs
using var rb1 = new Roaring32Bitmap();
using var rb2 = new Roaring32Bitmap();

rb1.AddMany([1, 2, 3, 4, 5, 100, 1000]);
rb1.Optimize();

rb2.AddMany([3, 4, 5, 7, 50]);
rb2.Optimize();

using var result = rb1.And(rb2);

Console.WriteLine(result.Contains(2));
Console.WriteLine(result.Contains(4));
Console.WriteLine(result.Contains(5));
```

## Requirements
### Linux
- gcc
- [VCpkg](https://vcpkg.io/en/getting-started) and please do `vcpkg integrate install`. (CMake for CRoaring will be handled by VCpkg on your behalf.)
- run `build/build-libs.sh` to build a CRoaring library

### Windows
- [VS2022 or later](https://www.visualstudio.com/downloads/)
- [VCpkg](https://vcpkg.io/en/getting-started) and please do `vcpkg integrate install`. (CMake for CRoaring will be handled by VCpkg on your behalf.)
- run `build/build-libs.bat` to build a CRoaring library
