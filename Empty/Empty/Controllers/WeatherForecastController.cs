using Empty.DTO;
using Empty.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Empty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ������Controller : ControllerBase
    {
        private readonly DbCtx _db;

        public ������Controller(DbCtx ctx)
        {
            _db = ctx;
        }

        [HttpGet("getgetget")]
        public ActionResult<studyDTO> Get([FromQuery] int id)
        {
            // List<studyzzzz> data = _db.studys.ToList();

            var data = _db.studys.FirstOrDefault(s => s.Id == id);

            return new studyDTO
            {
                Id = data.Id,
                Name = data.Name,
                message = "���Ӹ�"
            };
        }

        [HttpPost("ptest")]
        public IActionResult Post(registerDTO data)
        {
            studyzzzz temp = new studyzzzz
            {
                Name = data.nName,
                Age = data.aAge
            };
            try
            {
                _db.studys.Add(temp);
                _db.SaveChanges();
                return Ok("ȸ�����ԿϷ�");

            }
            catch (Exception)
            {
                return Problem("���� ������", statusCode: 500);
            }
        }

    }
    public record registerDTO(
       string nName,
       int aAge
    );
}




// record �̰� ���ϴ»�����

// const int