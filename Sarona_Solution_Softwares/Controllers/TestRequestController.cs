using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sarona_Solution_Softwares.Model.Domain;
using Sarona_Solution_Softwares.Model.DomainDTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sarona_Solution_Softwares.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Reader")]
    [Authorize]
    public class TestRequestController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ITestRequest testRequest;

        public TestRequestController(IMapper mapper, ITestRequest testRequest)
        {
            this.mapper = mapper;
            this.testRequest = testRequest;
        }

        [HttpGet]
        public IActionResult GetData()
        {
            var testDomain = testRequest.GetAllAsync();


            return Ok(mapper.Map<List<TestRequestDto>>(testDomain));
        }
    }
}

