namespace DB
{
    public interface Wapper<T>
    {
        // 开始执行
        T Do();
    }
}