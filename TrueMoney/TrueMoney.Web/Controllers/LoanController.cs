﻿using System.Web.Mvc;

namespace TrueMoney.Web.Controllers
{
    using System.Threading.Tasks;

    using Infrastructure.Services;

    using TrueMoney.Web.Models;

    [Authorize]
    public class LoanController : Controller
    {
        private readonly ILoanService _loanService;
        private readonly IUserService _userService;

        private readonly IApplicationService _applicationService;

        public LoanController(ILoanService loanService, IUserService userService, IApplicationService applicationService)
        {
            _loanService = loanService;
            _userService = userService;
            _applicationService = applicationService;
        }
        public async Task<ActionResult> Index()
        {
            return new EmptyResult();
        }

        public async Task<ActionResult> Details(int id)
        {
            var currentUser = await _userService.GetCurrentUser();
            ViewBag.CurrentUser = currentUser;
            var model = await _loanService.GetById(id);
            if (model.IsTakePart(currentUser))
            {
                return View(model);
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> VisaPayment(string paymentName, float paymentCount, string desctinationAccountNumber)
        {
            if (!string.IsNullOrEmpty(paymentName) && !string.IsNullOrEmpty(desctinationAccountNumber)
                && paymentCount > 0)
            {

                return
                    View(
                        new VisaPaymentViewModel
                        {
                            PaymentCount = paymentCount,
                            PaymentName = paymentName,
                            DesctinationAccountNumber = desctinationAccountNumber
                        });
            }   

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> VisaPayment(VisaPaymentViewModel formModel)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(formModel.PaymentName) && !string.IsNullOrEmpty(formModel.DesctinationAccountNumber)
                && formModel.PaymentCount > 0)
            {
                //todo - payment

                return View("Success", formModel);
            }

            ModelState.AddModelError("Server", "Server Error");

            return View(formModel);
        }

        public string Test()
        {
            return "authorized";
        }

        [Authorize]
        public async Task<ActionResult> YouActivity()
        {
            var currentUser = await _userService.GetCurrentUser();
            ViewBag.CurrentUser = currentUser;
            var viewModel = new YouActivityViewModel
                                {
                                    MoneyApplications =
                                        await _applicationService.GetByUserId(currentUser.Id),
                                    Loans = await _loanService.GetByUser(currentUser.Id),
                                    Offers =
                                        await
                                        _applicationService.GetAllOffersByUser(currentUser.Id)
                                };
            return View(viewModel);
        }
    }
}