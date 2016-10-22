using System.Web.Mvc;
using TrueMoney.Common.Enums;
using TrueMoney.Models;
using TrueMoney.Services;

namespace TrueMoney.Web.Controllers
{
    using System.Threading.Tasks;

    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
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
                    await CurrentUserId(),
                    formModel.DealId,
                    formModel.PaymentCount
                    // todo: create model
                    //new VisaDetails
                    //{
                    //    CardNumber = formModel.CardNumber,
                    //    CvvCode = formModel.CvvCode,
                    //    Name = formModel.Name,
                    //    ValidBefore = formModel.ValidBefore
                    //}
                    );
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