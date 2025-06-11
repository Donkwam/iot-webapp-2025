using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization; // 추가
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
            var query = _context.Board.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => EF.Functions.Like(b.Title, $"%{search}%"));
            }
            var totalCount = await query.CountAsync();
            var countList = 10;
            var totalPage = totalCount / countList;
            if (totalCount % countList > 0) totalPage++;
            if (totalPage < page) page = totalPage;
            var countPage = 10;
            var startPage = ((page - 1) / countPage) * countPage + 1;
            var endPage = startPage + countPage - 1;
            if (totalPage < endPage) endPage = totalPage;
            var boards = await query
                .OrderByDescending(b => b.PostDate)
                .Skip((page - 1) * countList)
                .Take(countList)
                .ToListAsync();
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

            board.ReadCount = (board.ReadCount ?? 0) + 1;
            _context.Board.Update(board);
            await _context.SaveChangesAsync();

            return View(board);
        }

        // GET: Board/Create
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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