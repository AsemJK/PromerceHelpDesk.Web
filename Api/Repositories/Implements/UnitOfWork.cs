using PromerceCRM.API.Repository.Interfaces;

namespace PromerceCRM.API.Repository.Implements
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IGenericRepository<T> GenericRepository<T>() where T : class
        {
            IGenericRepository<T> repo = new GenericRepository<T>(_dbContext);
            return repo;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
