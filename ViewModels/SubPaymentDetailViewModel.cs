using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using VipcoPainting.Models;

namespace VipcoPainting.ViewModels
{
    public class SubPaymentDetailViewModel : SubPaymentDetail
    {
        public double CurrentCost { get; set; }
        public string PaymentDetailString { get; set; }

    }
}
