using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnSale.Web.Models
{
    public class DepartmentResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<CityResponse> Cities { get; set; }
    }
}
