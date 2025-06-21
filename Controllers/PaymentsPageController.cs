using GymManager.Interfaces;
using GymManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymManager.Controllers
{
    [Authorize]
    public class PaymentsPageController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IMemberService _memberService;

        public PaymentsPageController(IPaymentService paymentService, IMemberService memberService)
        {
            _paymentService = paymentService;
            _memberService = memberService;
        }

        public async Task<IActionResult> Index()
        {
            var payments = await _paymentService.ListPayments();
            return View(payments);
        }

        public async Task<IActionResult> Details(int id)
        {
            var payment = await _paymentService.FindPayment(id);
            if (payment == null)
                return NotFound();
            return View(payment);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateMembers();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaymentDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _paymentService.AddPayment(dto);
                if (result.Status == ServiceResponse.ServiceStatus.Created)
                    return RedirectToAction(nameof(Index));
                foreach (var msg in result.Messages)
                    ModelState.AddModelError(string.Empty, msg);
            }
            await PopulateMembers(dto.MemberId);
            return View(dto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var payment = await _paymentService.FindPayment(id);
            if (payment == null)
                return NotFound();
            await PopulateMembers(payment.MemberId);
            return View(payment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PaymentDto dto)
        {
            if (id != dto.PaymentId)
                return NotFound();

            if (ModelState.IsValid)
            {
                var result = await _paymentService.UpdatePayment(dto);
                if (result.Status == ServiceResponse.ServiceStatus.Updated)
                    return RedirectToAction(nameof(Index));
                foreach (var msg in result.Messages)
                    ModelState.AddModelError(string.Empty, msg);
            }
            await PopulateMembers(dto.MemberId);
            return View(dto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _paymentService.FindPayment(id);
            if (payment == null)
                return NotFound();
            return View(payment);
        }

        // POST: /PaymentsPage/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _paymentService.DeletePayment(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateMembers(int? selectedMemberId = null)
        {
            var members = await _memberService.ListMembers();
            var memberList = members.Select(m => new
            {
                m.MemberId,
                FullName = m.FirstName + " " + m.LastName
            }).ToList();
            ViewBag.Members = new SelectList(memberList, "MemberId", "FullName", selectedMemberId);
        }
    }
}
