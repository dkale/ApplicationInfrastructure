using DotNetStandard.Infrastructure.GenericRepository;
using System;

namespace DotNetStandard.ServiceLayer.BusinessRepository
{
    public interface IMyDatabaseRepository : IGenericRepository
    {
        DateTime? GetLastLoanDate(long lenderAccountRefId);
    }
}
