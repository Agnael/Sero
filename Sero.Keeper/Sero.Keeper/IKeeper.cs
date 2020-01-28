using System;
using System.Threading.Tasks;

namespace Sero.Keeper
{
    public interface IKeeper
    {
        Guid TransactionBegin();
        Task<Guid> TransactionBeginAsync();
        void Commit();
        void Rollback();
    }
}