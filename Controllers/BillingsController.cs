
using MyField.Models;
using MyField.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Identity;
using MyField.Data;
using MyField.Services;
using Microsoft.EntityFrameworkCore;
using MyField.Interfaces;
using System.Web;
using SelectPdf;
using System.Numerics;

namespace MyField.Controllers
{
    public class BillingsController : Controller
    {
        private readonly Ksans_SportsDbContext _context;
        private readonly UserManager<UserBaseModel> _userManager;
        private readonly IPaymentService _paymentService;
        private readonly DeviceInfoService _deviceInfoService;
        private readonly PdfService _pdfService;
        private readonly IViewRenderService _viewRenderService;
        private readonly EmailService _emailService;
        private readonly IActivityLogger _activityLogger;


        public BillingsController(Ksans_SportsDbContext context,
            UserManager<UserBaseModel> userManager,
            IPaymentService paymentService,
            DeviceInfoService deviceInfoService,
            PdfService pdfService,
            IViewRenderService viewRenderService,
            EmailService emailService,
            IActivityLogger activityLogger)
        { 
            _emailService = emailService;
            _context = context;
            _userManager = userManager;
            _paymentService = paymentService;
            _deviceInfoService = deviceInfoService;
            _pdfService = pdfService;
            _viewRenderService = viewRenderService; 
            _activityLogger = activityLogger;   
        }

        public async Task<IActionResult> MyIndividualFineInvoicePreview(int invoiceId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var invoice = await _context.Invoices
                .Where(i => i.InvoiceId == invoiceId && i.CreatedById == user.Id)
                .Include(i => i.Payment)
                .Include(i => i.Fine)
                .ThenInclude(t => t.Offender)
                .FirstOrDefaultAsync();

            ViewBag.FullNames = user.FirstName + " " + user.LastName;

            return PartialView("_MyIndividualFineInvoicePartial", invoice);
        }

        public async Task<IActionResult> MyPlayerInvoicePDfPreview (int invoiceId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            int? clubId = user switch
            {
                ClubAdministrator clubAdmin => clubAdmin.ClubId,
                ClubManager clubManager => clubManager.ClubId,
                Player player => player.ClubId,
                _ => null
            };

            if (clubId == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var clubs = await _context.Club
                                      .FirstOrDefaultAsync(mo => mo.ClubId == clubId);

            var invoice = await _context.Invoices
                .Where(i => i.Payment.ClubId == clubId && i.InvoiceId == invoiceId)
                .Include(i => i.Payment)
                .Include(i => i.Transfer)
                .ThenInclude(t => t.Player)
                .Include(i => i.Transfer)
                .ThenInclude(t => t.SellerClub)
                .Include(i => i.Transfer)
                .ThenInclude(t => t.CustomerClub)
                .Include(i => i.Fine)
                .ThenInclude(t => t.Club)
                .Include(i => i.Fine)
                .ThenInclude(t => t.Offender)
                .FirstOrDefaultAsync();

            ViewBag.ClubName = clubs?.ClubName;

            return PartialView("_MyPlayerTransferInvoicePartial", invoice);
        }

        public async Task<IActionResult> MyClubFineInvoicePreview(int invoiceId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            int? clubId = user switch
            {
                ClubAdministrator clubAdmin => clubAdmin.ClubId,
                ClubManager clubManager => clubManager.ClubId,
                Player player => player.ClubId,
                _ => null
            };

            if (clubId == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var clubs = await _context.Club
                                      .FirstOrDefaultAsync(mo => mo.ClubId == clubId);

            var invoice = await _context.Invoices
                .Where(i => i.InvoiceId == invoiceId)
                .Include(i => i.Payment)
                .Include(i => i.Transfer)
                .ThenInclude(t => t.Player)
                .Include(i => i.Transfer)
                .ThenInclude(t => t.SellerClub)
                .Include(i => i.Transfer)
                .ThenInclude(t => t.CustomerClub)
                .Include(i => i.Fine)
                .ThenInclude(t => t.Club)
                .Include(i => i.Fine)
                .ThenInclude(t => t.Offender)
                .FirstOrDefaultAsync();

            ViewBag.ClubName = clubs?.ClubName;

            return PartialView("_MyClubFineInvoicePartial", invoice);
        }

        /* private byte[] GeneratePdf(string htmlContent)
         {
             var converter = new HtmlToPdf();
             var baseUrl = "file:///" + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot").Replace("\\", "/");
             var pdfDoc = converter.ConvertHtmlString(htmlContent, baseUrl);
             using (var ms = new MemoryStream())
             {
                 pdfDoc.Save(ms);
                 return ms.ToArray();
             }
         }

         public async Task<IActionResult> MyPlayerInvoicePDF(int invoiceId)
         {
             var user = await _userManager.GetUserAsync(User);

             if (user == null)
             {
                 return RedirectToAction("Error", "Home");
             }

             int? clubId = user switch
             {
                 ClubAdministrator clubAdmin => clubAdmin.ClubId,
                 ClubManager clubManager => clubManager.ClubId,
                 Player player => player.ClubId,
                 _ => null
             };

             if (clubId == null)
             {
                 return RedirectToAction("Error", "Home");
             }

             var myPlayerInvoice = await _context.Invoices
                 .Where(i => i.Payment.ClubId == clubId && i.InvoiceId == invoiceId)
                 .Include(i => i.Payment)
                 .Include(i => i.Transfer)
                 .ThenInclude(t => t.Player)
                 .Include(i => i.Transfer)
                 .ThenInclude(t => t.SellerClub)
                 .Include(i => i.Transfer)
                 .ThenInclude(t => t.CustomerClub)
                 .FirstOrDefaultAsync();

             if (myPlayerInvoice == null)
             {
                 return RedirectToAction("Error", "Home");
             }

             // Render the partial view to a string
             var htmlContent = await _viewRenderService.RenderToStringAsync("Billings/_MyPlayerTransferInvoicePartial", myPlayerInvoice);

             // Convert HTML content to PDF bytes
             var pdfBytes = GeneratePdf(htmlContent);

             // Return the PDF bytes directly
             return File(pdfBytes, "application/pdf", "MyPlayerInvoice.pdf");
         }
 */


        [HttpGet]
        public async Task<IActionResult> IndividualFineInvoice()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var individualInvoices = await _context.Invoices
                .Where(i => i.Payment.PaymentMadeById == userId)    
                .Include(i => i.Payment)
                .ToArrayAsync();

            return View(individualInvoices);
        }



        [HttpGet]
        public async Task<IActionResult> ClubInvoice()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (!(user is ClubAdministrator clubAdministrator) &&
                !(user is ClubManager clubManager) &&
                !(user is Player clubPlayer))
            {
                return RedirectToAction("Error", "Home");
            }

            var clubId = (user as ClubAdministrator)?.ClubId ??
                         (user as ClubManager)?.ClubId ??
                         (user as Player)?.ClubId;

            if (clubId == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var clubs = await _context.Club
                                      .FirstOrDefaultAsync(mo => mo.ClubId == clubId);


            var invoice = await _context.Invoices
                .Where(i => i.Payment.ClubId == clubId || i.Payment.PaymentMadeById == user.Id)
                .Include(i => i.CreatedBy)
                .Include(i => i.Transfer)
                .ThenInclude(t => t.CreatedBy)
                .Include(i => i.Fine)
                .ThenInclude(t => t.Club)
                .Include(i => i.Fine)
                .ThenInclude(t => t.Offender)
                .Include(i => i.Payment)
                .ThenInclude(t => t.PaymentMadeByClub)
                .Include(i => i.Payment)
                .ThenInclude(t => t.PaymentMadeBy)
                .ToListAsync();

            ViewBag.ClubName = clubs?.ClubName;

            return View(invoice);
        }


        [HttpGet]

        public async Task<IActionResult> Transactions()
        {
            var payments = await _context.Payments
                .Include(p => p.PaymentMadeBy)
                .OrderByDescending( p => p.PaymentDate)
                .ToListAsync();

            return View(payments);
        }

        public async Task<IActionResult> PaymentDetails(int? paymentId)
        {
            if (paymentId == null || _context.Payments == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Where(p => p.PaymentId == paymentId)
                .Include(p => p.PaymentMadeBy)
                .Include(p => p.DeviceInfo)
                .FirstOrDefaultAsync();

            var viewModel = new PaymentViewModel
            {
                PaymentId = payment.PaymentId,
                AmountPaid = payment.AmountPaid,
                FirstName = payment.PaymentMadeBy.FirstName,
                LastName = payment.PaymentMadeBy.LastName,
                PaymentDate = payment.PaymentDate,
                PaymentStatus = payment.Status,
                DeviceName = $"{payment?.DeviceInfo?.DeviceName}, {payment?.DeviceInfo?.DeviceModel}, {payment?.DeviceInfo?.OSName}, {payment?.DeviceInfo?.OSVersion}",
                Browser = $"{payment?.DeviceInfo?.Browser}, {payment?.DeviceInfo?.BrowserVersion}",
                DeviceLocation = $"({payment?.DeviceInfo?.Longitude}, {payment?.DeviceInfo?.Latitude}), {payment?.DeviceInfo?.City}, {payment?.DeviceInfo?.Region},  {payment?.DeviceInfo?.Country} "
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PayClubFIne(int fineId, int clubId)
        {
            var fine = await _context.Fines
                .Where(mo => mo.FineId == fineId)
                .FirstOrDefaultAsync();

            var club = await _context.Club
                .Where(mo => mo.ClubId == clubId)
                .FirstOrDefaultAsync();



            var viewModel = new PayClubFineViewModel
            {
                FineId = fineId,
                FineDetails = fine.FineDetails,
                FineAmount = fine.FineAmount,
                FineDuDate = fine.FineDuDate,
                ClubId = club.ClubId,
                ClubName = club.ClubName,
                ClubBadge = club.ClubBadge,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PayClubFIne(PayClubFineViewModel viewModel)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PayPlayerTransfer(int transferId)
        {
            var transfer = await _context.Transfer
                .Where(mo => mo.TransferId == transferId)
                .Include(s => s.Player)
                .Include(s => s.SellerClub)
                .Include(s => s.CustomerClub)
                .Include(s => s.PlayerTransferMarket)
                .FirstOrDefaultAsync();

            if (transfer == null)
            {
                return NotFound();
            }

            var viewModel = new PayPlayerTransferViewModel
            {
                TransferId = transferId,
                PlayerId = transfer.Player.Id,
                PlayerTransferMarketId = transfer.PlayerTransferMarketId,
                SellerClubId = transfer.SellerClubId,
                CustomerClubId = transfer.CustomerClubId,
                SellerClubName = transfer.SellerClub.ClubName,
                SellerClubBadge = transfer.SellerClub.ClubBadge,
                ProfilePicture = transfer.Player.ProfilePicture,
                BuyerClubName = transfer.CustomerClub.ClubName,
                BuyerClubBadge = transfer.CustomerClub.ClubBadge,
                FirstName = transfer.Player.FirstName,
                LastName = transfer.Player.LastName,
                PlayerAmount = transfer.Player.MarketValue,
                Position = transfer.Player.Position,
                JerseyNumber = transfer.Player.JerseyNumber,
                DateOfBirth = transfer.Player.DateOfBirth,
                PaymentStatus = transfer.paymentTransferStatus,
            };

            return View(viewModel);
        }

        public async Task<IActionResult> PayPlayerTransfer(PayPlayerTransferViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return Json(new { success = false, message = "User not found." });
                }


                var deviceInfo = await _deviceInfoService.GetDeviceInfo();

                _context.Add(deviceInfo);
                await _context.SaveChangesAsync();

                var newPayment = new Payment
                {
                    ReferenceNumber = GenerateTransferPaymentReferenceNumber(),
                    PaymentMethod = PaymentMethod.Credit_Card,
                    AmountPaid = viewModel.PlayerAmount,
                    PaymentDate = DateTime.Now,
                    PaymentMadeById = user.Id,
                    Status = PaymentPaymentStatus.Unsuccessful,
                    DeviceInfoId = deviceInfo.DeviceInfoId,
                };

                _context.Add(newPayment);
                await _context.SaveChangesAsync();

                var player = await _context.Player.FindAsync(viewModel.PlayerId);
                var playerTransfer = await _context.Transfer.FindAsync(viewModel.TransferId);

                if (player == null)
                {
                    return Json(new { success = false, message = "Player not found." });
                }

                if (playerTransfer == null)
                {
                    return Json(new { success = false, message = "Player transfer not found." });
                }

                int paymentId = newPayment.PaymentId;
                decimal totalPrice = viewModel.PlayerAmount;
                int transferId = viewModel.TransferId;
                var returnUrl = Url.Action("PayFastReturn", "Billings", new { paymentId, transferId, totalPrice }, Request.Scheme);
                returnUrl = HttpUtility.UrlEncode(returnUrl);
                var cancelUrl = "https://newcafeteriabykhaya.azurewebsites.net";

                string paymentUrl = GeneratePayFastPaymentUrl(paymentId, totalPrice, transferId, returnUrl, cancelUrl);

                await _activityLogger.Log($"Initiated payment for {player.FirstName} {player.LastName} transfer.", user.Id);

                return Redirect(paymentUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing the payment: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                return Json(new
                {
                    success = false,
                    message = "Failed to process payment.",
                    errorDetails = new
                    {
                        InnerException = ex.InnerException?.Message ?? "No inner exception available.",
                        StackTrace = ex.StackTrace
                    }
                });
            }
        }



        public async Task<IActionResult> PayFastReturn(int paymentId, int transferId, decimal totalPrice)
        {
            try
            {
                var payment = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId);

                if (payment == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Payment with PaymentId: {paymentId} not found.");
                    return Json(new { success = false, message = "Payment not found." });
                }

                var playerTransfer = await _context.Transfer
                    .Where(mo => mo.TransferId == transferId)
                    .Include(s => s.Player)
                    .Include(s => s.PlayerTransferMarket)
                    .Include(s => s.SellerClub)
                    .Include(s => s.CustomerClub)
                    .FirstOrDefaultAsync();

                if (playerTransfer == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Player transfer with TransferId: {transferId} not found.");
                    return Json(new { success = false, message = $"Player transfer not found with TransferId: {transferId}" });
                }

                decimal roundedAmountPaid = Math.Round(payment.AmountPaid, 2);
                decimal roundedTotalPrice = Math.Round(totalPrice, 2);

                if (Math.Abs(roundedAmountPaid - roundedTotalPrice) > 0.01m)
                {
                    System.Diagnostics.Debug.WriteLine($"Amount mismatch: Payment AmountPaid = {roundedAmountPaid}, totalPrice = {roundedTotalPrice}");
                    return Json(new { success = false, message = $"Invalid payment amount. AmountPaid: {roundedAmountPaid}, totalPrice: {roundedTotalPrice}" });
                }

                if (!_paymentService.ValidatePayment(payment))
                {
                    return Json(new { success = false, message = "Payment validation failed." });
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    System.Diagnostics.Debug.WriteLine("User not authenticated.");
                    return Json(new { success = false, message = "User not authenticated." });
                }

                // Update player transfer and payment statuses
                playerTransfer.Player.ClubId = playerTransfer.CustomerClubId;
                playerTransfer.Player.ModifiedBy = user.Id;
                playerTransfer.Player.ModifiedDateTime = DateTime.Now;
                playerTransfer.PlayerTransferMarket.SaleStatus = SaleStatus.Unavailable;
                playerTransfer.paymentTransferStatus = PaymentTransferStatus.Payment_Completed;
                playerTransfer.Status = TransferStatus.Completed;
                payment.Status = PaymentPaymentStatus.Successful;

                _context.Update(payment);
                _context.Update(playerTransfer);
                await _context.SaveChangesAsync();

                // Generate invoice and update database
                var newInvoice = new Invoice
                {
                    PaymentId = payment.PaymentId,
                    TransferId = playerTransfer.TransferId,
                    InvoiceTimeStamp = DateTime.Now,
                    CreatedById = user.Id,
                    InvoiceNumber = GenerateInvoiceNumber(paymentId), 
                    IsEmailed = true
                };

                payment.ClubId = playerTransfer.CustomerClubId; // Ensure ClubId is set correctly

                _context.Update(payment);
                _context.Add(newInvoice);
                await _context.SaveChangesAsync();


                var viewName = "Billings/_MyPlayerTransferInvoicePartial";
                var viewData = new Invoice 
                {
                    Transfer = playerTransfer,
                    Payment = payment,
                    InvoiceNumber = newInvoice.InvoiceNumber 
                };

                string emailBody = await _viewRenderService.RenderToStringAsync(viewName, viewData);

                await _emailService.SendEmailAsync(user.Email, "Proof of Player Transfer", emailBody);

                TempData["Message"] = $"You have successfully bought {playerTransfer.Player.FirstName} {playerTransfer.Player.LastName} at an amount of {playerTransfer.Player.MarketValue} from {playerTransfer.SellerClub.ClubName}";

                return RedirectToAction("MyTransfersTabs", "Transfers");
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Failed to process payment: " + ex.Message,
                    errorDetails = new
                    {
                        InnerException = ex.InnerException?.Message,
                        StackTrace = ex.StackTrace
                    }
                });
            }
        }

        private string GeneratePayFastPaymentUrl(int paymentId, decimal amount, int transferId, string returnUrl, string cancelUrl)
        {


            string merchantId = "10033052";
            string merchantKey = "708c7udni72oo";

            int amountInCents = (int)(amount * 100);
            string amountString = amount.ToString("0.00").Replace(',', '.');

            string paymentUrl = $"https://sandbox.payfast.co.za/eng/process?merchant_id={merchantId}&merchant_key={merchantKey}&return_url={returnUrl}&cancel_url={cancelUrl}&amount={amountInCents}&item_name=Order+Payment&payment_id={paymentId}&transfer_id={transferId}&amount={amountString}";

            return paymentUrl;
        }




        /* private string GeneratePayFastPaymentUrl(int paymentId, decimal amount, int transferId, string returnUrl, string cancelUrl)
         {
             var playerTransfer = _context.Transfer
                    .Where(mo => mo.TransferId == transferId)
                    .Include(s => s.Player)
                    .Include(s => s.PlayerTransferMarket)
                    .Include(s => s.SellerClub)
                    .Include(s => s.CustomerClub)
                    .FirstOrDefault();

             string merchantId = "21098051";
             string merchantKey = "8oqhl4g4jjlft";
             int amountInCents = (int)(amount * 100);
             string amountString = amount.ToString("0.00").Replace(',', '.');

             string paymentUrl = $"https://www.payfast.co.za/eng/process?merchant_id={merchantId}&merchant_key={merchantKey}&return_url={returnUrl}&cancel_url={cancelUrl}&amount={amountInCents}&item_name={playerTransfer.Player.FirstName} {playerTransfer.Player.LastName} transfer from {playerTransfer.SellerClub.ClubName} to {playerTransfer.CustomerClub.ClubName}&payment_id={paymentId}&transfer_id={transferId}&amount={amountString}";

             return paymentUrl;
         }*/



        /*Pay club and individual fines*/

        [HttpGet]
        public async Task<IActionResult> PayFines(int fineId)
        {
            var fine = await _context.Fines
                .Where(mo => mo.FineId == fineId)   
                .Include(f => f.Club)
                .Include(f => f.Offender)
                .FirstOrDefaultAsync();

            if (fine == null)
            {
                return NotFound();
            }

            var viewModel = new PayFineViewModel
            {
                FineId = fineId,    
                FineDetails = fine.FineDetails,
                FineAmount = fine.FineAmount,
                FineDueDate = fine.FineDuDate,
                PaymentStatus = fine.PaymentStatus,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PayFines(PayFineViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Json(new { success = false, message = "User not found." });
                }

                var deviceInfo = await _deviceInfoService.GetDeviceInfo();

                _context.Add(deviceInfo);
                await _context.SaveChangesAsync();

                var newPayment = new Payment
                {
                    ReferenceNumber = GenerateFinePaymentReferenceNumber(),
                    PaymentMethod = PaymentMethod.Credit_Card,
                    AmountPaid = viewModel.FineAmount,
                    PaymentDate = DateTime.Now,
                    PaymentMadeById = user.Id,
                    Status = PaymentPaymentStatus.Unsuccessful,
                    DeviceInfoId = deviceInfo.DeviceInfoId
                };

                _context.Add(newPayment);
                await _context.SaveChangesAsync();

                var fine = await _context.Fines.FindAsync(viewModel.FineId);

                if (fine == null)
                {
                    return Json(new { success = false, message = "Fine not found." });
                }

                int paymentId = newPayment.PaymentId;
                decimal totalPrice = viewModel.FineAmount;
                int fineId = viewModel.FineId;

                var returnUrl = Url.Action("PayFinePayFastReturn", "Billings", new { paymentId, fineId, totalPrice }, Request.Scheme);
                returnUrl = HttpUtility.UrlEncode(returnUrl);
                var cancelUrl = "https://newcafeteriabykhaya.azurewebsites.net";

                string paymentUrl = GeneratePayFineFastPaymentUrl(paymentId, totalPrice, fineId, returnUrl, cancelUrl);

                await _activityLogger.Log($"Initiated payment for {fine?.Offender?.FirstName} {fine?.Offender?.LastName} {fine?.Club?.ClubName} fine.", user.Id);
                return Redirect(paymentUrl);

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Failed to redirect to payfast: " + ex.Message,
                    errorDetails = new
                    {
                        InnerException = ex.InnerException?.Message,
                        StackTrace = ex.StackTrace
                    }
                });
            }
        }


        public async Task<IActionResult> PayFinePayFastReturn(int paymentId, int fineId, decimal totalPrice)
        {
            try
            {
                var payment = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId);
                if (payment == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Payment with PaymentId: {paymentId} not found.");
                    return Json(new { success = false, message = "Payment not found." });
                }

                System.Diagnostics.Debug.WriteLine($"Payment found: {payment.PaymentId}, AmountPaid: {payment.AmountPaid}");

                var fine = await _context.Fines
                    .Where(mo => mo.FineId == fineId)
                    .Include(s => s.Club)
                    .Include(s => s.Offender)
                    .FirstOrDefaultAsync();

                if (fine == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Player transfer with TransferId: {fineId} not found.");
                    return Json(new { success = false, message = $"Player transfer not found with TransferId: {fineId}" });
                }

                System.Diagnostics.Debug.WriteLine($"Player transfer found: {fine.FineId}, PlayerId: {fine.OffenderId}");


                decimal roundedAmountPaid = Math.Round(payment.AmountPaid, 2);
                decimal roundedTotalPrice = Math.Round(totalPrice, 2);

                System.Diagnostics.Debug.WriteLine($"Original AmountPaid: {payment.AmountPaid}, roundedAmountPaid: {roundedAmountPaid}, Original totalPrice: {totalPrice}, roundedTotalPrice: {roundedTotalPrice}");

                if (Math.Abs(roundedAmountPaid - roundedTotalPrice) > 0.01m)
                {
                    System.Diagnostics.Debug.WriteLine($"Amount mismatch: Payment AmountPaid = {roundedAmountPaid}, totalPrice = {roundedTotalPrice}");

                    return Json(new
                    {
                        success = false,
                        message = $"Invalid payment amount. AmountPaid: {roundedAmountPaid}, totalPrice: {roundedTotalPrice}"
                    });
                }

                if (!_paymentService.ValidatePayment(payment))
                {
                    return Json(new { success = false, message = "Payment validation failed." });
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    System.Diagnostics.Debug.WriteLine("User not authenticated.");
                    return Json(new { success = false, message = "User not authenticated." });
                }
                var userId = user.Id;

                System.Diagnostics.Debug.WriteLine($"User found: {userId}");




                payment.PaymentMadeById = fine.OffenderId;
                payment.ClubId = fine.ClubId;   
                fine.PaymentStatus = PaymentStatus.Paid;
                payment.Status = PaymentPaymentStatus.Successful;
                fine.ModifiedById = userId;
                fine.ModifiedDateTime = DateTime.Now;
               

                _context.Update(fine);
                _context.Update(payment);
                await _context.SaveChangesAsync();

                var newInvoice = new Invoice
                {
                    PaymentId = payment.PaymentId,
                    FineId = fine.FineId,
                    InvoiceTimeStamp = DateTime.Now,
                    CreatedById = userId,
                };

                newInvoice.InvoiceNumber = GenerateInvoiceNumber(paymentId);
                newInvoice.IsEmailed = true;

                _context.Add(newInvoice);
                await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine("Player transfer and player updated successfully.");
                TempData["Message"] = $"You have successfully cleared your charges of {fine.FineAmount}. You will receive a confirmation email shortly with our clearence";

                if(fine.ClubId  != null)
                {
                    return RedirectToAction("MyClubFines", "Fines");
                }
                else
                {
                    return RedirectToAction("MyIndividualFines", "Fines");
                }

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Failed to process payment: " + ex.Message,
                    errorDetails = new
                    {
                        InnerException = ex.InnerException?.Message,
                        StackTrace = ex.StackTrace
                    }
                });
            }
        }

        private string GeneratePayFineFastPaymentUrl(int paymentId, decimal amount, int fineId, string returnUrl, string cancelUrl)
        {
            var fine = _context.Fines
                .Where(mo => mo.FineId == fineId)
                .Include(f => f.Offender)
                .Include(f => f.Club)
                .FirstOrDefault();

            string merchantId = "10033052";
            string merchantKey = "708c7udni72oo";

            int amountInCents = (int)(amount * 100);
            string amountString = amount.ToString("0.00").Replace(',', '.');

            // Replace unwanted characters such as \r\n with an empty string
            string fineDetails = fine?.FineDetails?.Replace("\r\n", "");

            string paymentUrl = $"https://sandbox.payfast.co.za/eng/process?merchant_id={merchantId}&merchant_key={merchantKey}&return_url={returnUrl}&cancel_url={cancelUrl}&amount={amountInCents}&item_name={fine?.Offender?.FirstName} {fine?.Offender?.LastName} {fine?.Club?.ClubName} fine charges for offence:{fineDetails}&payment_id={paymentId}&fine_id={fineId}&amount={amountString}";

            return paymentUrl;
        }

        /*Helper method to generate the Invoice number*/

        private string GenerateInvoiceNumber(int paymentId)
        {
            var payment = _context.Payments
                .Where(p => p.PaymentId == paymentId)
                .FirstOrDefault();  

            var year = DateTime.Now.Year.ToString().Substring(2);
            var month = DateTime.Now.Month.ToString("D2");
            var day = DateTime.Now.Day.ToString("D2");

            const string numbers = "0123456789";

            var random = new Random();
            var randomNumbers = new string(Enumerable.Repeat(numbers, 4)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return $"{year}{month}{day}{randomNumbers}";
        }


        private string GenerateTransferPaymentReferenceNumber()
        {
            var year = DateTime.Now.Year.ToString().Substring(2);
            var month = DateTime.Now.Month.ToString("D2");
            var day = DateTime.Now.Day.ToString("D2");

            const string numbers = "0123456789";
            const string transferLetters = "TF";

            var random = new Random();
            var randomNumbers = new string(Enumerable.Repeat(numbers, 4)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            var transLetters = transferLetters.ToString();  

            return $"{year}{month}{day}{randomNumbers}{transLetters}";
        }

        private string GenerateFinePaymentReferenceNumber()
        {
            var year = DateTime.Now.Year.ToString().Substring(2);
            var month = DateTime.Now.Month.ToString("D2");
            var day = DateTime.Now.Day.ToString("D2");

            const string numbers = "0123456789";
            const string fineLetters = "FP";

            var random = new Random();
            var randomNumbers = new string(Enumerable.Repeat(numbers, 4)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            var fineLets = fineLetters.ToString();  

            return $"{year}{month}{day}{randomNumbers}{fineLets}";
        }
    }
}
