using System.Web.Mvc;
using TrueMoney.Common.Enums;
using TrueMoney.Common.Extensions;
using TrueMoney.Models;
using TrueMoney.Services;

namespace TrueMoney.Web.Controllers
{
    using System;
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
        private readonly IUserService _userServicee;

        public PaymentController(IPaymentService paymentService, IDealService dealService, IUserService userService)
        {
            _paymentService = paymentService;
            _dealService = dealService;
            _userServicee = userService;
        }

        public async Task<ActionResult> VisaLoan(int dealId)
        {
            var model = new VisaPaymentViewModel();
            await UpdateDataForVisaLoan(model, dealId);

            return View("Visa", model);
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

            await UpdateDataForVisaLoan(formModel, formModel.DealId);

            return View("Visa", formModel);
        }

        public async Task<ActionResult> VisaPayout(int dealId)
        {
            var model = new VisaPaymentViewModel();
            await UpdateDataForVisaPayout(model, dealId);

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

            await UpdateDataForVisaPayout(formModel, formModel.DealId);
            return View("Visa", formModel);
        }

        private async Task UpdateDataForVisaPayout(VisaPaymentViewModel viewModel, int dealId)
        {
            var deal = await _dealService.GetById(dealId, User.Identity.GetUserId<int>());
            var nearByPayment = deal.Payments.FirstOrDefault(x => !x.IsPaid);
            var paymentCount = nearByPayment.Amount + nearByPayment.Liability - deal.ExtraMoney;
            viewModel.PaymentCount = paymentCount;
            viewModel.DealId = dealId;
            viewModel.CanSetPaymentCount = true;
            viewModel.FormAction = "VisaPayout";
            viewModel.OffererFullName = deal.Offers.First(x => x.IsApproved).OffererFullName;
            viewModel.CardNumber = deal.DealOwner.CardNumber.Replace(" ", "");
        }

        private async Task UpdateDataForVisaLoan(VisaPaymentViewModel viewModel, int dealId)
        {
            var deal = await _dealService.GetById(dealId, User.Identity.GetUserId<int>());
            var currentUser = await _userServicee.GetById(User.Identity.GetUserId<int>());
            if (!deal.IsCurrentUserLender)
            {
                throw new AccessViolationException($"User can't loan deal with id = {dealId}.");
            }

            viewModel.DealOwnerName = deal.Deal.OwnerFullName;
            viewModel.PaymentCount = deal.Deal.Amount;
            viewModel.DealId = dealId;
            viewModel.FormAction = "VisaLoan";
            viewModel.CardNumber = currentUser.CardNumber.Replace(" ", "");
        }
    }
}