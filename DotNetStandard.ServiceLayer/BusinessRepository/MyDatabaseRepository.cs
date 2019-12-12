using DotNetStandard.Infrastructure.GenericRepository;
using DotNetStandard.ServiceLayer.DatabaseContexts;
using System;

namespace DotNetStandard.ServiceLayer.BusinessRepository
{
    public class MyDatabaseRepository : GenericRepository<EmployeesDbContext>, IMyDatabaseRepository
    {
        public MyDatabaseRepository(EmployeesDbContext employeesDbContext)
            : base(employeesDbContext)
        {
        }

        public DateTime? GetLastLoanDate(long lenderAccountRefId)
        {
            throw new NotImplementedException();
        }
    }
}
