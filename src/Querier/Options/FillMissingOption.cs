using Querier.Enums;

namespace Querier.Options
{
    public class FillMissingOption
    {
        public int Interval { get; set; } = 1;
        public string Unit { get; set; } = DateAddInterval.DAY;
    }
}
