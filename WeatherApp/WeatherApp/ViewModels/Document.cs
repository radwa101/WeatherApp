using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherApp.ViewModels
{
    public class Document
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public DateTime ResendDate { get; set; }
        public int ResendId { get; set; }

        public int UltraFileItemOrder
        {
            get
            {
                switch (Name)
                {
                    case "One":
                        return 2;
                    case "Two":
                        return 1;
                    case "Three":
                        return 3;
                }
                return 1;
            }
        }

        public string PaymentType { get; set; }
        public string Email { get; set; }

        public bool IsUserRegistered { get; set; }

        public Dictionary<string, string> PaymentOptions = new Dictionary<string, string>();

        public bool ShowAdHocPayment
        {
            get
            { 
                if(PaymentOptions.ContainsKey("AdHocPayment"))
                {
                    return PaymentOptions["AdHocPayment"] == "Y";
                }
                return false;    
            }
        }

        public bool ShowFullPayment
        {
            get
            {
                if (PaymentOptions.ContainsKey("FullPayment"))
                {
                    return PaymentOptions["FullPayment"] == "Y";
                }
                return false;
            }
        }

        public bool ShowDirectDebit
        {
            get
            {
                if (PaymentOptions.ContainsKey("DirectDebit"))
                {
                    return PaymentOptions["DirectDebit"] == "Y";
                }
                return false;
            }
        }

        public HttpPostedFileBase[] files { get; set; }
    }
}