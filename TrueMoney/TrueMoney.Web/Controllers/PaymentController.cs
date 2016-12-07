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
        private readonly IDealService _dealService;

        public PaymentController(IPaymentService paymentService, IDealService dealService)
        {
            _paymentService = paymentService;
            _dealService = dealService;
        }

        public async Task<ActionResult> VisaLoan(int dealId)
        {
            var deal = await _dealService.GetById(dealId, User.Identity.GetUserId<int>());
            return
                View("Visa",
                    new VisaPaymentViewModel
                    {
                        PaymentName = $"Вы переводите деньги в размере {deal.Deal.Amount} р. в контексте заявки № {deal.Deal.Id}",
                        PaymentCount = deal.Deal.Amount,
                        DealId = dealId,
                        FormAction = "VisaLoan"
                    });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VisaLoan(VisaPaymentViewModel formModel)
        {
            if (ModelState.IsValid)
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

        public ActionResult VisaPayout(int dealId)
        {
            var model = new VisaPaymentViewModel
            {
                //PaymentCount = paymentCount,
                //PaymentName = paymentName,
                DealId = dealId,
                CanSetPaymentCount = true,
                FormAction = "VisaPayout"
            };

            return View("Visa", model);
        }

        [HttpPost]
        public async Task<ActionResult> VisaPayout(VisaPaymentViewModel formModel)
        {
            if (ModelState.IsValid && formModel.PaymentCount > 0)
            {
                var payRes = await _paymentService.Payout(formModel);
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