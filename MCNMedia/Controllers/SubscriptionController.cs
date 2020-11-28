using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3.Model.Internal.MarshallTransformations;
using MCNMedia_Dev.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using MCNMedia_Dev.Repository;

namespace MCNMedia_Dev.Controllers
{
    
      public class SubscriptionController : Controller
      {
        PaymentDataAccessLayer paymentDAL = new PaymentDataAccessLayer();
        private int amount = 500;
       
        public ViewResult Subscribe() {

            ViewBag.PaymentAmount = amount;
            return View();
        }

        [HttpPost]
        public IActionResult Processing(string stripeToken, string stripeEmail)
        {
            //Add Online Payment Log
            StripePayment sPayment = new StripePayment();
            sPayment.SubscriberId = 1;
            sPayment.OrderId = stripeToken; // "1" + "-" + DateTime.Now.ToString("yyyyMMddHHmmss");
            sPayment.Amount = amount;
            sPayment.AmountInCents = (amount / 100);
            sPayment.OrderDate = DateTime.Now;
            sPayment.IsSuccess = false;
            paymentDAL.AddOnlinePaymentLog(sPayment);


            Dictionary<string, string> Metadata = new Dictionary<string, string>();
            Metadata.Add("Product", "RubberDuck");
            Metadata.Add("Quantity", "10");
            StripeConfiguration.ApiKey = "sk_test_51HsMgeBcBMLkoLiDqppjmP6GfFXnDOjuwsEFPvi2SM0WJZp3yh9zlQbsVDv3lcyUv2FA8aUDuDqJP8bJ6hUwNf1L00RC9aOiHS";
            var options = new ChargeCreateOptions
            {
                Amount = amount,
                Currency = "EUR",
                Description = "Package Description",
                Source = stripeToken,
                ReceiptEmail = stripeEmail,
                Metadata = Metadata
            };
            var service = new ChargeService();
            Charge charge = service.Create(options);
            switch (charge.Status)
            {
                case "succeeded":
                    //Update OnlinePaymentLog, Add PaymentLog
                    UpdateOnlinePaymentLog(charge.Status, true, 1, stripeToken);
                    break;
                case "failed":
                    UpdateOnlinePaymentLog(charge.Status, false, 1, stripeToken);
                    break;
            }
            return RedirectToAction(nameof(Subscribe));
        }
             
        public void UpdateOnlinePaymentLog(string sMessage,bool status,int subscriberId,string orderId)
        {
            StripePayment sPayment = new StripePayment();
            
            sPayment.SubscriberId = subscriberId;
            sPayment.OrderId = orderId; // "1" + "-" + DateTime.Now.ToString("yyyyMMddHHmmss");
            sPayment.IsSuccess = status;
            paymentDAL.UpdateOnlinePaymentLog(sPayment);
        }
    }
}