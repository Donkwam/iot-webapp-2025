using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MyPortfolioWebApp.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace MyPortfolioWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context; // DB연동

        public HomeController(ApplicationDbContext context)
        {
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            // 정적 HTML을 DB 데이터로 동적 처리
            // DB에서 데이터를 불러온 뒤 About, Skill 객체에 데이터 담아서 뷰로 넘겨줌
            var skillCount = _context.Skill.Count();
            var skill = await _context.Skill.ToListAsync();

            var about = await _context.About.FirstOrDefaultAsync(); // FirstOrDefaultAsync - 데이터 없으면 Null

            ViewBag.SkillCount = skillCount;
            ViewBag.ColNum = (skillCount / 2) + (skillCount % 2);


            var model = new AboutModel();
            model.About = about;
            model.Skill = skill;

            return View(model);
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactModel model)
        {
            if (ModelState.IsValid) // Model에 들어간 네개 값이 제대로 들어갔으면
            {
                try
                {
                    var smtpClient = new SmtpClient("smtp.gmail.com") // Gmail을 사용하면
                    {
                        Port = 465, // 메일 SMTP 서비스포트 변경필요
                        Credentials = new NetworkCredential("ehdrhks0509@gmail.com", "비밀번호노출"),
                        EnableSsl = true,
                    };

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(model.Email), // 문의하기에 작성한 메일주소
                        Subject = model.Subject ?? "[제목없음]",
                        Body = $"보낸사람 : {model.Name} ({model.Email})\n\n메시지 : {model.Message}",
                        IsBodyHtml = false, // 메일 본문에 HTML태그를 사용여부
                    };
                    mailMessage.To.Add("ehdrhks0509@naver.com"); // 받을 메일 주소

                    await smtpClient.SendMailAsync(mailMessage); // 위에 생성된 메일객체를 전송!
                    ViewBag.Success = true;
                }
                catch (Exception ex)
                {
                    ViewBag.Success = false;
                    ViewBag.Error =  $"메일전송 실패! {ex.Message}";
                }
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
