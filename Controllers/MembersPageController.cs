using GymManager.Interfaces;
using GymManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManager.Controllers
{
    [Authorize]
    public class MembersPageController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersPageController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        // GET: /MembersPage
        public async Task<IActionResult> Index()
        {
            var members = await _memberService.ListMembers();
            return View(members);
        }

        // GET: /MembersPage/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var member = await _memberService.FindMember(id);
            if (member == null)
                return NotFound();

            // Get the classes this member is registered for
            var registrations = await _memberService.ListRegistrationsForMember(id);

            // Pass both member and registrations to the view using a ViewModel
            var viewModel = new MemberDetailsViewModel
            {
                Member = member,
                ClassRegistrations = registrations.ToList()
            };

            return View(viewModel);
        }


        // GET: /MembersPage/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /MembersPage/Create
        [HttpPost]
        public async Task<IActionResult> Create(MemberDto memberDto)
        {
            Console.WriteLine("addMemberPage");
            if (ModelState.IsValid)
            {
                var result = await _memberService.AddMember(memberDto);
                if (result.Status == ServiceResponse.ServiceStatus.Created)
                    return RedirectToAction(nameof(Index));
                foreach (var msg in result.Messages)
                    ModelState.AddModelError(string.Empty, msg);
            }
            return View(memberDto);
        }

        // GET: /MembersPage/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var member = await _memberService.FindMember(id);
            if (member == null)
                return NotFound();
            return View(member);
        }

        // POST: /MembersPage/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, MemberDto memberDto)
        {
            if (id != memberDto.MemberId)
                return NotFound();

            if (ModelState.IsValid)
            {
                var result = await _memberService.UpdateMember(memberDto);
                if (result.Status == ServiceResponse.ServiceStatus.Updated)
                    return RedirectToAction(nameof(Index));
                foreach (var msg in result.Messages)
                    ModelState.AddModelError(string.Empty, msg);
            }
            return View(memberDto);
        }

        // GET: /MembersPage/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var member = await _memberService.FindMember(id);
            if (member == null)
                return NotFound();
            return View(member);
        }

        // POST: /MembersPage/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _memberService.DeleteMember(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
