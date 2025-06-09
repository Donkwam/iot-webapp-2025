using Empty.DTO;
using Empty.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Empty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ºýºýÀÌController : ControllerBase
    {
        private readonly DbCtx _db;

        public ºýºýÀÌController(DbCtx ctx)
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
                message = "¹¹ÀÓ¸¶"
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
                return Ok("È¸¿ø°¡ÀÔ¿Ï·á");

            }
            catch (Exception)
            {
                return Problem("¼­¹ö ³­¸®³²", statusCode: 500);
            }
        }

    }
    public record registerDTO(
       string nName,
       int aAge
    );
}




// record ÀÌ°Å ¹¹ÇÏ´Â»õ³¢ÀÓ

// const int