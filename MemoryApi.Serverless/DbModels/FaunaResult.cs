using System.Diagnostics.CodeAnalysis;

namespace MemoryApi.DbModels
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class FaunaResult<T>
    {
        public string @ref { get; set; }
        public long ts { get; set; }
        public T data { get; set; }
    }
}