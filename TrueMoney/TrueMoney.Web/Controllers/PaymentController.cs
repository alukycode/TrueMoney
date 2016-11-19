using System.Web.Mvc;
using TrueMoney.Common.Enums;
using TrueMoney.Models;
using TrueMoney.Services;

namespace TrueMoney.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using TrueMoney.Common;
    using TrueMoney.Services.Interfaces;

    [Authorize(Roles = RoleNames.User)]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<ActionResult> VisaLoan(string paymentName, decimal paymentCount, int payForId, int dealId)
        {
            if (!string.IsNullOrEmpty(paymentName)
                && paymentCount > 0)
            {

                return
                    View("Visa",
                        new VisaPaymentViewModel
                        {
                            PaymentCount = paymentCount,
                            PaymentName = paymentName,
                            DealId = dealId,
                            FormAction = "VisaLoan"
                        });
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VisaLoan(VisaPaymentViewModel formModel)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(formModel.PaymentName)
                && formModel.PaymentCount > 0)
            {
                var payRes = await _paymentService.LendMoney(formModel, User.Identity.GetUserId<int>());
                switch (payRes)
                {
                    case PaymentResult.Success:
                        return View("Success", formModel);
                    case PaymentResult.EmptyData:
                        ModelState.AddModelError("", "Заполните форму");
                        break;
                    case PaymentResult.Error:
                        ModelState.AddModelError("", "Server Error");
                        break;
                    case PaymentResult.NotEnoughtMoney:
                        ModelState.AddModelError("", "Недостаточно средств на счёте");
                        break;
                    case PaymentResult.PermissionError:
                        ModelState.AddModelError("", "Ошибка доступа к счёту");
                        break;
                }
            }

            return View("Visa", formModel);
        }

        public async Task<ActionResult> VisaPayout(string paymentName, int payForId, int dealId, decimal paymentCount = 0)
        {
            if (!string.IsNullOrEmpty(paymentName))
            {

                return
                    View("Visa",
                        new VisaPaymentViewModel
                        {
                            PaymentCount = paymentCount,
                            PaymentName = paymentName,
                            DealId = dealId,
                            CanSetPaymentCount = true,
                            FormAction = "VisaPayout"
                        });
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> VisaPayout(VisaPaymentViewModel formModel)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(formModel.PaymentName)
                && formModel.PaymentCount > 0)
            {
                var payRes = await _paymentService.Payout(formModel, User.Identity.GetUserId<int>());
                switch (payRes)
                {
                    case PaymentResult.Success:
                        return View("Success", formModel);
                    case PaymentResult.EmptyData:
                        ModelState.AddModelError("", "Заполните форму");
                        break;
                    case PaymentResult.Error:
                        ModelState.AddModelError("", "Server Error");
                        break;
                    case PaymentResult.NotEnoughtMoney:
                        ModelState.AddModelError("", "Недостаточно средств на счёте");
                        break;
                    case PaymentResult.PermissionError:
                        ModelState.AddModelError("", "Ошибка доступа к счёту");
                        break;
                    case PaymentResult.LessThenMinAmount:
                        ModelState.AddModelError("", "Сумма платежа меньше минимальной суммы. См. заголовок.");
                        break;
                }
            }

            return View("Visa", formModel);
        }
    }
}