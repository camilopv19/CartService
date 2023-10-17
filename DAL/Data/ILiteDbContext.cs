using LiteDB;

namespace DAL.Data
{
    public interface ILiteDbContext
    {
        LiteDatabase Database { get; }
    }
}