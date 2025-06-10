using Empty.DTO;
using Empty.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Empty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class 빡빡이Controller : ControllerBase
    {
        private readonly DbCtx _db;

        public 빡빡이Controller(DbCtx ctx)
        {
            _db = ctx;
        }

        [HttpGet("찾기")]
        public ActionResult<studyDTO> Get([FromQuery] int id)
        {
            // List<studyzzzz> data = _db.studys.ToList();

            var data = _db.studys.FirstOrDefault(s => s.id == id);

            return new studyDTO
            {
                id = data.id,
                u_name = data.u_name,
                message = "왜찾아"
            };
        }

        [HttpPost("더하기")]
        public IActionResult Post(registerDTO data)
        {
            studyzzzz temp = new studyzzzz
            {
                u_name = data.u_name,
                u_pas = data.u_pas,
                age = data.age
            };
            try
            {
                _db.studys.Add(temp);
                _db.SaveChanges();
                return Ok("회원가입완료");

            }
            catch (Exception)
            {
                return Problem("서버 난리남", statusCode: 500);
            }
        }

    }
    public record registerDTO(
       string u_name,
       string u_pas,
       int age
    );
}




// record 이거 뭐하는새끼임

// const int