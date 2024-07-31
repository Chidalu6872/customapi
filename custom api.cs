dotnet new webapi -n FitnessTrackerAPI
cd FitnessTrackerAPI

dotnet add package Microsoft.EntityFrameworkCore.InMemory

public class Workout
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Duration { get; set; }
    public int CaloriesBurned { get; set; }
    public List<Exercise> Exercises { get; set; }
}

public class Exercise
{
    public int Id { get; set; }
    public int WorkoutId { get; set; }
    public string Name { get; set; }
    public int Reps { get; set; }
    public int Sets { get; set; }
    public Workout Workout { get; set; }
}

using Microsoft.EntityFrameworkCore;

public class FitnessTrackerContext : DbContext
{
    public DbSet<Workout> Workouts { get; set; }
    public DbSet<Exercise> Exercises { get; set; }

    public FitnessTrackerContext(DbContextOptions<FitnessTrackerContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Workout>().HasData(
            new Workout { Id = 1, Date = DateTime.Now, Duration = 60, CaloriesBurned = 500 }
        );

        modelBuilder.Entity<Exercise>().HasData(
            new Exercise { Id = 1, WorkoutId = 1, Name = "Push-up", Reps = 10, Sets = 3 }
        );
    }
}

public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<FitnessTrackerContext>(options =>
        options.UseInMemoryDatabase("FitnessTracker"));
    services.AddControllers();
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class WorkoutsController : ControllerBase
{
    private readonly FitnessTrackerContext _context;

    public WorkoutsController(FitnessTrackerContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Workout>>> GetWorkouts()
    {
        return await _context.Workouts.Include(w => w.Exercises).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Workout>> GetWorkout(int id)
    {
        var workout = await _context.Workouts.Include(w => w.Exercises).FirstOrDefaultAsync(w => w.Id == id);
        if (workout == null)
        {
            return NotFound();
        }
        return workout;
    }

    [HttpPost]
    public async Task<ActionResult<Workout>> PostWorkout(Workout workout)
    {
        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetWorkout), new { id = workout.Id }, workout);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutWorkout(int id, Workout workout)
    {
        if (id != workout.Id)
        {
            return BadRequest();
        }

        _context.Entry(workout).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorkout(int id)
    {
        var workout = await _context.Workouts.FindAsync(id);
        if (workout == null)
        {
            return NotFound();
        }

        _context.Workouts.Remove(workout);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}