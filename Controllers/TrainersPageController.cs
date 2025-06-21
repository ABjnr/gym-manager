using GymManager.Interfaces;
using GymManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManager.Controllers
{
    [Authorize]
    public class TrainersPageController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainersPageController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        // GET: /TrainersPage
        public async Task<IActionResult> Index()
        {
            var trainers = await _trainerService.ListTrainers();
            return View(trainers);
        }

        // GET: /TrainersPage/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var trainer = await _trainerService.FindTrainer(id);
            if (trainer == null)
                return NotFound();
            return View(trainer);
        }

        // GET: /TrainersPage/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /TrainersPage/Create
        [HttpPost]
        public async Task<IActionResult> Create(TrainerDto trainerDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _trainerService.AddTrainer(trainerDto);
                if (result.Status == ServiceResponse.ServiceStatus.Created)
                    return RedirectToAction(nameof(Index));
                foreach (var msg in result.Messages)
                    ModelState.AddModelError(string.Empty, msg);
            }
            return View(trainerDto);
        }

        // GET: /TrainersPage/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var trainer = await _trainerService.FindTrainer(id);
            if (trainer == null)
                return NotFound();
            return View(trainer);
        }

        // POST: /TrainersPage/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, TrainerDto trainerDto)
        {
            if (id != trainerDto.TrainerId)
                return NotFound();

            if (ModelState.IsValid)
            {
                var result = await _trainerService.UpdateTrainer(trainerDto);
                if (result.Status == ServiceResponse.ServiceStatus.Updated)
                    return RedirectToAction(nameof(Index));
                foreach (var msg in result.Messages)
                    ModelState.AddModelError(string.Empty, msg);
            }
            return View(trainerDto);
        }

        // GET: /TrainersPage/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var trainer = await _trainerService.FindTrainer(id);
            if (trainer == null)
                return NotFound();
            return View(trainer);
        }

        // POST: /TrainersPage/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _trainerService.DeleteTrainer(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
