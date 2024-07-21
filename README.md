# CRoaring.Net

A .Net wrapper for [CRoaring](https://github.com/RoaringBitmap/CRoaring) - a C implementation of [RoaringBitmap](https://github.com/RoaringBitmap/RoaringBitmap).

## Usage
```cs
using (var rb1 = new RoaringBitmap())
using (var rb2 = new RoaringBitmap())
{
	rb1.AddMany(1, 2, 3, 4, 5, 100, 1000);
	rb1.Optimize();
	
	rb2.AddMany(3, 4, 5, 7, 50);
	rb2.Optimize();

	using (var result = rb1.And(rb2))
	{
		Console.WriteLine(result.Contains(2));
		Console.WriteLine(result.Contains(4));
		Console.WriteLine(result.Contains(5));
	}
}
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