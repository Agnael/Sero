using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Keeper.Ef
{
    public abstract class EfKeeper : IKeeper
    {
        private DbContext _dbContext;
        public DbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                    _dbContext = CreateDbContext();

                return _dbContext;
            }
        }

        public EfKeeper()
        {
        }

        protected abstract DbContext CreateDbContext();
        
        public void Commit()
        {
            DbContext.Database.CommitTransaction();
        }

        public Guid TransactionBegin()
        {
            DbContext.Database.BeginTransaction();
            return DbContext.Database.CurrentTransaction.TransactionId;
        }

        public async Task<Guid> TransactionBeginAsync()
        {
            await DbContext.Database.BeginTransactionAsync();
            return DbContext.Database.CurrentTransaction.TransactionId;
        }

        public void Rollback()
        {
            DbContext.Database.RollbackTransaction();
        }
    }
}
