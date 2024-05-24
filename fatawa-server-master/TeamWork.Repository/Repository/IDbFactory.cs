using TeamWork.Repository.DataContext;

namespace TeamWork.Repository.Repository
{
    public interface IDbFactory
    {
        TeamDataContext GetDataContext { get; }
    }
}
