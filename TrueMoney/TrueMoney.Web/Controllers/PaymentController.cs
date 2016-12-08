using System.Web.Mvc;
using TrueMoney.Common.Enums;
using TrueMoney.Common.Extensions;
using TrueMoney.Models;
using TrueMoney.Services;

namespace TrueMoney.Web.Controllers
{
    using System.Linq;
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
                        PaymentName = $"Вы переводите деньги заёмщику (<b>{deal.Deal.OwnerFullName}</b>)" +
                                      $" в размере <b>{deal.Deal.Amount.Format()} р.</b> в контексте заявки <b>№ {deal.Deal.Id}</b>.",
                        PaymentCount = deal.Deal.Amount,
                        DealId = dealId,
                        FormAction = "VisaLoan",
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
                        var deal = await _dealService.GetById(formModel.DealId, User.Identity.GetUserId<int>());
                        formModel.DealOwnerName = deal.Deal.OwnerFullName;
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

        public async Task<ActionResult> VisaPayout(int dealId)
        {
            var deal = await _dealService.GetById(dealId, User.Identity.GetUserId<int>());
            var nearByPayment = deal.Payments.FirstOrDefault(x => !x.IsPaid);
            var paymentCount = nearByPayment.Amount + nearByPayment.Liability - deal.ExtraMoney;
            var model = new VisaPaymentViewModel
            {
                PaymentCount = paymentCount * (1 + NumericConstants.Tax),
                DealId = dealId,
                CanSetPaymentCount = true,
                FormAction = "VisaPayout",
                PaymentName = $"<p>Вы переводите деньги кредитору (<b>{deal.Offers.First(x => x.IsApproved).OffererFullName}</b>)"
                              + $" в размере <b>{paymentCount.Format()} р.</b> в контексте заявки <b>№ {deal.Deal.Id}</b>.</p>"
                              + "<p>Также с Вашего счёта будет списан налог за использования сервиса"
                              + $" в размере <b>{(paymentCount * NumericConstants.Tax).Format()} р.</b></p>"
            };

            return View("Visa", model);
        }

        [HttpPost]
        public async Task<ActionResult> VisaPayout(VisaPaymentViewModel formModel)
        {
            if (ModelState.IsValid)
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
                        ModelState.AddModelError("", "Сервис временно недоступен, повторите попытку позже");
                        break;
                    case PaymentResult.NotEnoughtMoney:
                        ModelState.AddModelError("", "Недостаточно средств на счёте");
                        break;
                    case PaymentResult.PermissionError:
                        ModelState.AddModelError("", "Ошибка доступа к счёту, проверьте введенные данные");
                        break;
                    case PaymentResult.LessThenMinAmount:
                        ModelState.AddModelError("", "Сумма платежа меньше минимальной суммы.");
                        break;
                }
            }

            return View("Visa", formModel);
        }
    }
}