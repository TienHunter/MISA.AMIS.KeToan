using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; // thư viện
//using Dapper;
//using MysqlConnector;
namespace MISA.AMIS.KeToan.API.Controllers 
{
    [Route("api/[controller]")] // Attribute
    [ApiController] // attribute thể hiện 1  controller liên quan đến API
    public class UsersController : ControllerBase // dấu 2 chấm -> extends, implements
    {
        [HttpGet] 
        public string GetUserName()
        {
            return "Vũ Đình Tiến";
        }
    }
}
