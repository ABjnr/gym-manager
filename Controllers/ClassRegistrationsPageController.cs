using GymManager.Interfaces;
using GymManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymManager.Controllers
{
    [Authorize]
    public class ClassRegistrationsPageController : Controller
    {
        private readonly IClassRegistrationService _registrationService;
        private readonly IMemberService _memberService;
        private readonly IGymClassService _gymClassService;

        public ClassRegistrationsPageController(
            IClassRegistrationService registrationService,
            IMemberService memberService,
            IGymClassService gymClassService)
        {
            _registrationService = registrationService;
            _memberService = memberService;
            _gymClassService = gymClassService;
        }

        public async Task<IActionResult> Index()
        {
            var registrations = await _registrationService.ListClassRegistrations();
            return View(registrations);
        }

        public async Task<IActionResult> Details(int id)
        {
            var registration = await _registrationService.FindClassRegistration(id);
            if (registration == null)
                return NotFound();
            return View(registration);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClassRegistrationDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _registrationService.AddClassRegistration(dto);
                if (result.Status == ServiceResponse.ServiceStatus.Created)
                    return RedirectToAction(nameof(Index));
                foreach (var msg in result.Messages)
                    ModelState.AddModelError(string.Empty, msg);
            }
            await PopulateDropdowns();
            return View(dto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var registration = await _registrationService.FindClassRegistration(id);
            if (registration == null)
                return NotFound();
            await PopulateDropdowns(registration.MemberId, registration.GymClassId);
            return View(registration);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ClassRegistrationDto dto)
        {
            if (id != dto.ClassRegistrationId)
                return NotFound();

            if (ModelState.IsValid)
            {
                var result = await _registrationService.UpdateClassRegistration(dto);
                if (result.Status == ServiceResponse.ServiceStatus.Updated)
                    return RedirectToAction(nameof(Index));
                foreach (var msg in result.Messages)
                    ModelState.AddModelError(string.Empty, msg);
            }
            await PopulateDropdowns(dto.MemberId, dto.GymClassId);
            return View(dto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var registration = await _registrationService.FindClassRegistration(id);
            if (registration == null)
                return NotFound();
            return View(registration);
        }

        // POST: /ClassRegistrationsPage/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _registrationService.DeleteClassRegistration(id);
            return RedirectToAction(nameof(Index));
        }


        private async Task PopulateDropdowns(int? selectedMemberId = null, int? selectedGymClassId = null)
        {
            var members = await _memberService.ListMembers();
            var memberList = members.Select(m => new
            {
                m.MemberId,
                FullName = m.FirstName + " " + m.LastName
            }).ToList();
            ViewBag.Members = new SelectList(memberList, "MemberId", "FullName", selectedMemberId);

            var gymClasses = await _gymClassService.ListGymClasses();
            ViewBag.GymClasses = new SelectList(gymClasses, "GymClassId", "Name", selectedGymClassId);
        }
    }
}
