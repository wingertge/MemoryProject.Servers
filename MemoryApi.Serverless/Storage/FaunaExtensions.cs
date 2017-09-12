using System;
using System.Collections.Generic;
using FaunaDB.Query;
using MemoryApi.Util;

namespace MemoryApi.Storage
{
    public static class FaunaExtensions
    {
        public static Expr ToFaunaObj(this object obj)
        {
            var fields = new Dictionary<string, Expr>();
            foreach (var prop in obj.GetType().GetProperties())
            {
                var propType = prop.PropertyType;
                var propValue = prop.GetValue(obj);
                if(propValue == null) continue;
                switch (Type.GetTypeCode(propType))
                {
                    case TypeCode.Byte:
                    case TypeCode.SByte:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.Decimal:
                    case TypeCode.Double:
                    case TypeCode.Single:
                    case TypeCode.String:
                    case TypeCode.Boolean:
                        fields[prop.Name] = prop.GetValue(obj) as dynamic;
                        continue;
                    case TypeCode.Object:
                        fields[prop.Name] = prop.GetValue(obj).ToFaunaObj();
                        continue;
                    case TypeCode.Char:
                        fields[prop.Name] = prop.GetValue(obj).ToString();
                        continue;
                    case TypeCode.DateTime:
                        fields[prop.Name] = ((DateTime)prop.GetValue(obj)).ToUnixTimeStamp();
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return Language.Obj(fields);
        }
    }
}