using System;
using TeamWork.Repository.DataContext;

namespace TeamWork.Repository.Repository
{
    public class DbFactory : IDbFactory,IDisposable
    {
        private readonly TeamDataContext _dbContext;

        public DbFactory(TeamDataContext teamDataContext)
        {
            _dbContext = teamDataContext;
        }

        public TeamDataContext GetDataContext
        {
            get
            {
                return _dbContext;
            }
        }

        #region Dispose

        private bool isDisposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if(!isDisposed && disposing)
            {
                if(_dbContext != null)
                {
                    _dbContext.Dispose();
                }
            }

            isDisposed = true;
        }
        #endregion

    }
}
