namespace Roaring.Net.CRoaring;

/// <summary>
/// Stores the types of formats into which bitmaps can be serialized.
/// </summary>
public enum SerializationFormat
{
    /// <summary>
    /// Native CRoaring serialization format. <br/>
    /// This format can sometimes be more space efficient than the portable form, e.g. when the data is sparse.
    /// </summary>
    Normal,

    /// <summary>
    /// Universal serialization format compatible with Java and Go implementations. <br/>
    /// <a href="https://github.com/RoaringBitmap/RoaringFormatSpec">RoaringFormatSpec</a>
    /// </summary>
    Portable,

    /// <summary>
    /// "Frozen" serialization format imitates memory layout of CRoaring types. <br/>
    /// Deserialized bitmap is a constant view of the underlying buffer. <br/>
    /// This significantly reduces amount of allocations and copying required during deserialization. <br/>
    /// It can be used with memory mapped files. <br/>
    /// Bitmaps serialized in frozen format are supported by the <see cref="FrozenRoaring32Bitmap"/> class.
    /// </summary>
    Frozen,
}