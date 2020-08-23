using System;
using System.Collections.Generic;
using System.Linq;

namespace ListOfCountries.Models
{
    public class Region
    {
        public string Name { get; set; }
        public int Population { get; set; }
        public IEnumerable<string> Subregions { get; set; }
        public IEnumerable<string> Countries { get; set; }
    }
}
