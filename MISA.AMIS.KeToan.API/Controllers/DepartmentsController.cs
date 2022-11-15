using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.KeToan.BL;
using MISA.AMIS.KeToan.Common.Entities;
using MySqlConnector;

namespace MISA.AMIS.KeToan.API.Controllers
{
    [ApiController]
    public class DepartmentsController : BaseController<Department>
    {
        public DepartmentsController(IBaseBL<Department> balseBL) : base(balseBL)
        {
        }
    }
}
