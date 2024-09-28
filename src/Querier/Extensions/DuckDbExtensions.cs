using DuckDB.NET.Data;
using DuckDB.NET.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Extensions
{
    public static class DuckDbExtensions
    {
        public static DuckDBAppenderRow AppendValue(this DuckDBAppenderRow row, object value, int fieldIndex, string fieldName)
        {
            var type = value.GetType();
            if (value == null) return row.AppendNullValue();
            if (value.GetType() == typeof(DBNull)) return row.AppendNullValue();
            if (type == typeof(int)) return row.AppendValue((int?)value);
            if (type == typeof(bool)) return row.AppendValue((bool?)value);
            if (type == typeof(byte)) return row.AppendValue((byte?)value);
            if (type == typeof(char)) return row.AppendValue((char?)value);
            if (type == typeof(uint)) return row.AppendValue((uint?)value);
            if (type == typeof(nint)) return row.AppendValue((nint?)value);
            if (type == typeof(long)) return row.AppendValue((long?)value);
            if (type == typeof(Guid)) return row.AppendValue((Guid?)value);
            if (type == typeof(Enum)) return row.AppendValue((Enum?)value);
            if (type == typeof(short)) return row.AppendValue((short?)value);
            if (type == typeof(ulong)) return row.AppendValue((ulong?)value);
            if (type == typeof(nuint)) return row.AppendValue((nuint?)value);
            if (type == typeof(sbyte)) return row.AppendValue((sbyte?)value);
            if (type == typeof(float)) return row.AppendValue((float?)value);
            if (type == typeof(ushort)) return row.AppendValue((ushort?)value);
            if (type == typeof(byte[])) return row.AppendValue((byte[]?)value);
            if (type == typeof(double)) return row.AppendValue((double?)value);
            if (type == typeof(string)) return row.AppendValue((string?)value);
            if (type == typeof(decimal)) return row.AppendValue((decimal?)value);
            if (type == typeof(DateTime)) return row.AppendValue((DateTime?)value);
            if (type == typeof(TimeOnly)) return row.AppendValue((TimeOnly?)value);
            if (type == typeof(DateOnly)) return row.AppendValue((DateOnly?)value);
            if (type == typeof(TimeSpan)) return row.AppendValue((TimeSpan?)value);
            if (type == typeof(TimeSpan)) return row.AppendValue((TimeSpan?)value);
            if (type == typeof(BigInteger)) return row.AppendValue((BigInteger?)value);
            if (type == typeof(DateTimeOffset)) return row.AppendValue((DateTimeOffset?)value);
            if (type == typeof(DuckDBTimeOnly)) return row.AppendValue((DuckDBTimeOnly?)value);
            if (type == typeof(DuckDBDateOnly)) return row.AppendValue((DuckDBDateOnly?)value);
            if (type == typeof(IEnumerable<object>)) return row.AppendValue((IEnumerable<object>?)value);
            throw new ArgumentException($"Type of {value.GetType()} is not supported for `AppendValue`");
        }
    }
}
