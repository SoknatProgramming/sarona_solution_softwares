using System;
using Sarona_Solution_Softwares.Model.Domain;

namespace Sarona_Solution_Softwares
{
	public class SQLTestRequest : ITestRequest
	{
        public List<Model.Domain.TestRequest> GetAllAsync()
        {
            var getData = new List<TestRequest>
            {
               new TestRequest
               {
                   Id = Guid.NewGuid(),
                    UserName = "Sann Soknat",
                    Age = 27
               },
               new TestRequest
               {
                   Id = Guid.NewGuid(),
                   UserName = "Sat Rotana",
                   Age = 25
               },
               new TestRequest
               {
                   Id = Guid.NewGuid(),
                   UserName = "An Channean",
                   Age = 28
               }

            };
            return getData;
        }
    }
}

