namespace DB
{
    public interface IDB
    {
        bool Insert(IData data);
        bool Delete(IData data);
        bool Update(IData data);
        bool Query(IData data);
    }
}