using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SocialMediaAuthApp.Data;
using SocialMediaAuthApp.Hubs;
using SocialMediaAuthApp.Models;

namespace SocialMediaAuthApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly TaskDbContext _context;
        private readonly IHubContext<TaskHub> _hubContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            TaskDbContext context,
            IHubContext<TaskHub> hubContext,
            ILogger<HomeController> logger)
        {
            _context = context;
            _hubContext = hubContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Identity/Account/Login");
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var tasks = _context.Tasks
                                .Where(t => t.UserId == userId)
                                .ToList();  // 👈 EF Core will now translate this

            return View(tasks);
        }


        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] TaskModel task)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            task.UserId = userId; // 👈 Assign user ID

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveTaskUpdate", task);
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> EditTask([FromBody] TaskModel updatedTask)
        {
            var existingTask = await _context.Tasks.FindAsync(updatedTask.Id);
            if (existingTask == null)
            {
                return NotFound();
            }

            existingTask.Title = updatedTask.Title;
            existingTask.Description = updatedTask.Description;
            existingTask.IsCompleted = updatedTask.IsCompleted;

            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveTaskUpdate", updatedTask);

            return Ok();
        }



        [HttpPost]
        public async Task<IActionResult> DeleteTask([FromBody] int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("TaskDeleted", taskId);

            return Ok();
        }

        public IActionResult Privacy()
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
