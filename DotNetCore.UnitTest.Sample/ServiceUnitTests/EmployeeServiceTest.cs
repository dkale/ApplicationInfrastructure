using System.Collections.Generic;
using Xunit;

namespace DotNetCore.UnitTest.Sample.ServiceUnitTests
{
    public class EmployeeServiceTest : ServiceTestBase
    {
        public EmployeeServiceTest()
        {
            AddJsonTestDataFiles(new List<string> { "\\TestData\\employees.json" });
        }

        [Fact]
        public void IsEmployeeCrudSuccessful()
        {

        }
    }
}
