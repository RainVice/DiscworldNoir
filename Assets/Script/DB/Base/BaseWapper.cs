namespace DB
{
    public abstract class BaseWapper<R,T> : Wapper<R> where R : new()
    {
        protected T t;
        public abstract R Do();
    }
}