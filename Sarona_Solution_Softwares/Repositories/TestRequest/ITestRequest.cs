using System;
using Sarona_Solution_Softwares.Model.Domain;

namespace Sarona_Solution_Softwares
{
	public interface ITestRequest
	{
        List<TestRequest> GetAllAsync();
    }
}

