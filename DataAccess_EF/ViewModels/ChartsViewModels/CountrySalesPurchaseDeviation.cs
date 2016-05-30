using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_EF.ViewModels.ChartsViewModels
{
    /// <summary>
    /// Country vise sales/purchase total value deviation
    /// </summary>
    public class CountrySalesPurchaseDeviationVm
    {
        public string Country { get; set; }
        public decimal TotalValue { get; set; }
    }
}
