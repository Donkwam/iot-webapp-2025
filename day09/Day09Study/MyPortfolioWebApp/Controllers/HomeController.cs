using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPortfolioWebApp.Models;
using System.Diagnostics;

namespace MyPortfolioWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;
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
            // 정적 HTML을 DB 데이터로 동적처리
            // DB에서 데이터를 불러온 뒤 About, Skill 객체에 데이터 담아서 뷰로 넘겨줌
            var SkillCount = _context.Skill.Count();
            var Skill = await _context.Skill.ToListAsync();

            ViewBag.SkillCount = SkillCount; // ex. 7이 넘어감
            ViewBag.ColNum = (SkillCount / 2) + (SkillCount % 2); // 3(7/2) + 1(7%2)

            var model = new AboutModel();
            // model.About  // 나중에
            model.Skill = Skill;

            return View(model);
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
