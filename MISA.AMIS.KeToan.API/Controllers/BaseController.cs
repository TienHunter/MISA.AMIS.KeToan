using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.KeToan.BL;
using MISA.AMIS.KeToan.Common.Entities;
using MISA.AMIS.KeToan.Common.Enums;
using MISA.AMIS.KeToan.Common;
using MySqlConnector;

namespace MISA.AMIS.KeToan.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseController<T> : ControllerBase
    {
        #region Field

        private IBaseBL<T> _baseBL;

        #endregion

        #region Constructor

        public BaseController(IBaseBL<T> balseBL)
        {
            _baseBL = balseBL;
        }

        #endregion


        #region Method
        /// <summary>
        /// API trả về thông tin tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách thông tin tất cả bản ghi</returns>
        /// CreatedBy:VDTien (1/11/2022)
        [HttpGet]
        public IActionResult GetAllRecords()
        {
            try
            {
                var records = _baseBL.GetAllRecords();


                // Thành công: Trả về dữ liệu cho FE
                if (records != null)
                {
                    return StatusCode(StatusCodes.Status200OK, records);
                }

                // Thất bại: Trả về lỗi
                return StatusCode(StatusCodes.Status200OK, new List<T>());
            }
            //Try catch exception 
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = AMISErrorCode.Exception,
                    DevMsg = Resources.DevMsg_Exception,
                    UserMsg = Resources.UserMsg_Exception,
                    MoreInfo = Resources.MoreInfo_Exception,
                    TraceId = HttpContext.TraceIdentifier
                });
            }

        }

        /// <summary>
        /// API lấy thông tin 1 bản ghi theo id
        /// </summary>
        /// <returns>Thông tin bản ghi có id phù hợp</returns>
        /// CreatedBy: VDTien(1/11/2022)
        [HttpGet("{recordID}")]
        public IActionResult GetRecordByID([FromRoute] Guid recordID)
        {
            try
            {
                var record = _baseBL.GetRecordByID(recordID);
                //Xử lý kết quả trả về
                if (record != null)
                {
                    return StatusCode(StatusCodes.Status200OK, record);
                }
                return StatusCode(StatusCodes.Status404NotFound);

                //Thành công: trả về dữ liệu cho FE

                //Thất bại: trả về lỗi
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = AMISErrorCode.Exception,
                    DevMsg = Resources.DevMsg_Exception,
                    UserMsg = Resources.UserMsg_Exception,
                    MoreInfo = Resources.MoreInfo_Exception,
                    TraceId = HttpContext.TraceIdentifier
                });
            }

            //Try catch exception
        }
        #endregion
    }
}
