using GymManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymManager.Controllers
{
    [Authorize]
    public class GymClassesPageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GymClassesPageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /GymClassesPage
        public async Task<IActionResult> Index()
        {
            var classes = await _context.gymClasses.Include(g => g.Trainer).ToListAsync();
            return View(classes);
        }

        // GET: /GymClassesPage/Create
        public async Task<IActionResult> Create()
        {
            var trainers = await _context.Trainers
                .Select(t => new { t.TrainerId, FullName = t.FirstName + " " + t.LastName })
                .ToListAsync();
            ViewBag.Trainers = new SelectList(trainers, "TrainerId", "FullName");
            return View();
        }

        // POST: /GymClassesPage/Create
        [HttpPost]
        public async Task<IActionResult> Create(GymClassDto dto)
        {
            if (ModelState.IsValid)
            {
                var trainer = await _context.Trainers.FindAsync(dto.TrainerId);
                if (trainer == null)
                {
                    ModelState.AddModelError("TrainerId", "Trainer not found.");
                }
                else
                {
                    var g = new GymClass
                    {
                        Name = dto.Name,
                        TrainerId = dto.TrainerId,
                        Trainer = trainer,
                        ScheduleTime = dto.ScheduleTime,
                        MaxCapacity = dto.MaxCapacity
                    };
                    _context.gymClasses.Add(g);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            // Repopulate dropdown if model state is invalid
            var trainers = await _context.Trainers
                .Select(t => new { t.TrainerId, FullName = t.FirstName + " " + t.LastName })
                .ToListAsync();
            ViewBag.Trainers = new SelectList(trainers, "TrainerId", "FullName");
            return View(dto);
        }

        // GET: /GymClassesPage/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var gymClass = await _context.gymClasses.FindAsync(id);
            if (gymClass == null)
                return NotFound();

            var dto = new GymClassDto
            {
                GymClassId = gymClass.GymClassId,
                Name = gymClass.Name,
                TrainerId = gymClass.TrainerId,
                ScheduleTime = gymClass.ScheduleTime,
                MaxCapacity = gymClass.MaxCapacity
            };

            var trainers = await _context.Trainers
                .Select(t => new { t.TrainerId, FullName = t.FirstName + " " + t.LastName })
                .ToListAsync();
            ViewBag.Trainers = new SelectList(trainers, "TrainerId", "FullName", dto.TrainerId);

            return View(dto);
        }

        // POST: /GymClassesPage/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, GymClassDto dto)
        {
            if (id != dto.GymClassId)
                return NotFound();

            if (ModelState.IsValid)
            {
                var gymClass = await _context.gymClasses.FindAsync(id);
                if (gymClass == null)
                    return NotFound();

                gymClass.Name = dto.Name;
                gymClass.TrainerId = dto.TrainerId;
                gymClass.ScheduleTime = dto.ScheduleTime;
                gymClass.MaxCapacity = dto.MaxCapacity;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var trainers = await _context.Trainers
                .Select(t => new { t.TrainerId, FullName = t.FirstName + " " + t.LastName })
                .ToListAsync();
            ViewBag.Trainers = new SelectList(trainers, "TrainerId", "FullName", dto.TrainerId);

            return View(dto);
        }

        // GET: /GymClassesPage/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var gymClass = await _context.gymClasses
                .Include(g => g.Trainer)
                .FirstOrDefaultAsync(g => g.GymClassId == id);

            if (gymClass == null)
                return NotFound();

            var gymClassDto = new GymClassDto
            {
                GymClassId = gymClass.GymClassId,
                Name = gymClass.Name,
                TrainerId = gymClass.TrainerId,
                TrainerName = gymClass.Trainer.FirstName + " " + gymClass.Trainer.LastName,
                ScheduleTime = gymClass.ScheduleTime,
                MaxCapacity = gymClass.MaxCapacity
            };

            var enrolledMembers = await _context.ClassRegistrations
                .Where(r => r.GymClassId == id)
                .Include(r => r.Member)
                .Select(r => new ClassRegistrationDto
                {
                    ClassRegistrationId = r.ClassRegistrationId,
                    MemberId = r.MemberId,
                    MemberFullName = r.Member.FirstName + " " + r.Member.LastName,
                    RegistrationDate = r.RegistrationDate
                })
                .ToListAsync();

            var viewModel = new GymClassDetailsViewModel
            {
                GymClass = gymClassDto,
                EnrolledMembers = enrolledMembers
            };

            return View(viewModel);
        }



        // GET: /GymClassesPage/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var gymClass = await _context.gymClasses.Include(g => g.Trainer).FirstOrDefaultAsync(g => g.GymClassId == id);
            if (gymClass == null)
                return NotFound();
            return View(gymClass);
        }

        // POST: /GymClassesPage/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gymClass = await _context.gymClasses.FindAsync(id);
            if (gymClass != null)
            {
                _context.gymClasses.Remove(gymClass);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


    }
}
