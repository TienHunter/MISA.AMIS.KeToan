using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.KeToan.BL;
using MISA.AMIS.KeToan.Common.Entities;
using MySqlConnector;

namespace MISA.AMIS.KeToan.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : BaseController<Department>
    {
        public DepartmentController(IBaseBL<Department> balseBL) : base(balseBL)
        {
        }
    }
}
