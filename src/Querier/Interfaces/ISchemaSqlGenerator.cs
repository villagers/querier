namespace Querier.Interfaces
{
    public interface ISchemaSqlGenerator
    {
        void Generate();
        void GenerateFillMissingDates();
    }
}
