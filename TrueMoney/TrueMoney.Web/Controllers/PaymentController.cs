using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueMoney.Infrastructure.Enums;

namespace TrueMoney.Web.Controllers
{
    using System.Threading.Tasks;

    using TrueMoney.Infrastructure.Entities;
    using TrueMoney.Infrastructure.Services;
    using TrueMoney.Web.Models;

    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IUserService _userService;

        public PaymentController(IPaymentService paymentService, IUserService userService)
        {
            _paymentService = paymentService;
            _userService = userService;
        }

        public async Task<ActionResult> Visa(string paymentName, decimal paymentCount, int payForId, int dealId)
        {
            if (!string.IsNullOrEmpty(paymentName)
                && paymentCount > 0)
            {

                return
                    View(
                        new VisaPaymentViewModel
                        {
                            PaymentCount = paymentCount,
                            PaymentName = paymentName,
                            PayForId = payForId,
                            DealId = dealId
                        });
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> Visa(VisaPaymentViewModel formModel)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(formModel.PaymentName)
                && formModel.PaymentCount > 0)
            {
                var payRes = await _paymentService.LendMoney(
                    CurrentUser,
                    formModel.DealId,
                    formModel.PaymentCount,
                    new VisaDetails
                    {
                        CardNumber = formModel.CardNumber,
                        CvvCode = formModel.CvvCode,
                        Name = formModel.Name,
                        ValidBefore = formModel.ValidBefore
                    });
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

            return View(formModel);
        }
    }
}