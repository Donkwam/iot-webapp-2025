using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPortfolioWebApp.Models;

namespace MyPortfolioWebApp.Controllers
{
    public class BoardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BoardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Board
        public async Task<IActionResult> Index(int page = 1, string search = "")
        {
            ViewData["Title"] = "게시판 목록";

            // 검색 조건 적용
            var query = _context.Board.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => EF.Functions.Like(b.Title, $"%{search}%"));
            }

            // 전체 게시글 수
            var totalCount = await query.CountAsync();
            var countList = 10; // 한 페이지에 보여줄 게시글 수
            var totalPage = totalCount / countList;
            if (totalCount % countList > 0) totalPage++;
            if (totalPage < page) page = totalPage;

            // 페이징 네비게이션 계산
            var countPage = 10; // 페이지 네비에 보여줄 최대 페이지 수
            var startPage = ((page - 1) / countPage) * countPage + 1;
            var endPage = startPage + countPage - 1;
            if (totalPage < endPage) endPage = totalPage;

            // 실제 데이터 가져오기 (최신순)
            var boards = await query
                .OrderByDescending(b => b.PostDate)
                .Skip((page - 1) * countList)
                .Take(countList)
                .ToListAsync();

            // ViewBag에 페이징 정보 전달
            ViewBag.StartPage = startPage;
            ViewBag.EndPage = endPage;
            ViewBag.Page = page;
            ViewBag.TotalPage = totalPage;
            ViewBag.Search = search;

            return View(boards);
        }

        // GET: Board/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var board = await _context.Board.FirstOrDefaultAsync(m => m.Id == id);
            if (board == null)
                return NotFound();

            // 조회수 증가 (null 방지)
            board.ReadCount = (board.ReadCount ?? 0) + 1;
            _context.Board.Update(board);
            await _context.SaveChangesAsync();

            return View(board);
        }

        // GET: Board/Create
        public IActionResult Create()
        {
            var board = new Board
            {
                Writer = "관리자",
                PostDate = DateTime.Now,
                ReadCount = 0
            };
            return View(board);
        }

        // POST: Board/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Writer,Title,Contents")] Board board)
        {
            if (ModelState.IsValid)
            {
                board.Writer = "관리자";
                board.PostDate = DateTime.Now;
                board.ReadCount = 0;

                _context.Add(board);
                await _context.SaveChangesAsync();
                TempData["success"] = "게시글이 저장되었습니다!";
                return RedirectToAction(nameof(Index));
            }
            return View(board);
        }

        // GET: Board/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var board = await _context.Board.FindAsync(id);
            if (board == null)
                return NotFound();

            return View(board);
        }

        // POST: Board/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Writer,Title,Contents")] Board board)
        {
            if (id != board.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingBoard = await _context.Board.FindAsync(id);
                    if (existingBoard == null)
                        return NotFound();

                    existingBoard.Email = board.Email;
                    existingBoard.Writer = board.Writer;
                    existingBoard.Title = board.Title;
                    existingBoard.Contents = board.Contents;
                    // PostDate, ReadCount는 원본 유지

                    await _context.SaveChangesAsync();
                    TempData["success"] = "게시글이 수정되었습니다!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoardExists(board.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(board);
        }

        // GET: Board/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var board = await _context.Board.FirstOrDefaultAsync(m => m.Id == id);
            if (board == null)
                return NotFound();

            return View(board);
        }

        // POST: Board/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var board = await _context.Board.FindAsync(id);
            if (board != null)
            {
                _context.Board.Remove(board);
                TempData["success"] = "게시글이 삭제되었습니다!";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BoardExists(int id)
        {
            return _context.Board.Any(e => e.Id == id);
        }
    }
}
