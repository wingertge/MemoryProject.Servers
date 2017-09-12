using System.Collections.Generic;
using System.Linq;

namespace MemoryCore.JsonModels
{
    public class ActionResult<T>
    {
        public bool Succeeded { get; set; }
        public T Data { get; set; }
        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();

        public ActionResult() { }

        public ActionResult(bool succeeded, T data = default(T))
        {
            Succeeded = succeeded;
            Data = data;
        }

        public ActionResult(bool succeeded, params (string key, string value)[] errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToDictionary(a => a.key, a => a.value);
        }
    }
}