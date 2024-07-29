using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier
{
    public class QueryTimeDimension<TEntity>
    {
        public required string Property { get; set; }
        public string? OrderBy { get; set; }
        public TimeDimensionPart? TimeDimensionPart { get; set; }
    }

    public enum TimeDimensionPart
    {
        Hour,
        Minute,
        Second,
        Microsecond,
        Millisecond,
        Nanosecond,
        Ticks,
        Day,
        DayOfYear,
        TimeOfDay,
        DayOfWeek,
        Month,
        Year,
        Date
    }
}
