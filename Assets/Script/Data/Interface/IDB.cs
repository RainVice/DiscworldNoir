namespace DB
{
    public interface IDB
    {
        bool Insert(BaseData data);
        bool Delete(BaseData data);
        bool Update(BaseData data);
        bool Query(BaseData data);
    }
}