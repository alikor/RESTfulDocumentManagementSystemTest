namespace Documents.Utitlies
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}