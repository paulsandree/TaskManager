using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.dbcontext;
using TaskManager.models;

namespace TaskManager.controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly AppDbContext _context;

    public TaskController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<TaskItem>> GetTasks()
    {
        return Ok(_context.Tasks.ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<TaskItem> GetTask(int id)
    {
        var task = _context.Tasks.Find(id);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpPost]
    public ActionResult<TaskItem> CreateTask(TaskItem task)
    {
        task.CreatedDate = DateTime.Now;
        _context.Tasks.Add(task);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateTask(int id, TaskItem updatedTask)
    {
        var task = _context.Tasks.Find(id);
        if (task == null) return NotFound();
        
        task.Title = updatedTask.Title;
        task.Description = updatedTask.Description;
        task.IsCompleted = updatedTask.IsCompleted;
        task.DueDate = updatedTask.DueDate;
        task.ModifiedDate = updatedTask.ModifiedDate;
        task.Priority = updatedTask.Priority;
        _context.SaveChanges();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTask(int id)
    {
        var task = _context.Tasks.Find(id);
        if (task == null) return NotFound();
        
        _context.Tasks.Remove(task);
        _context.SaveChanges();
        
        return NoContent();
    }
}