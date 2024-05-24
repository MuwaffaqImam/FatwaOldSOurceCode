namespace TeamWork.Core.Services
{
    public interface ICommonService<T>
    {
        int Insert(T model);
        void Update(T model);
        void Delete(int id);
        int GetCountRecord();
    }
}
