using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PagedList;
using Microsoft.AspNetCore.Mvc;
using ListOfCountries.Models;
using Microsoft.Extensions.Caching.Memory;
using RestSharp.Extensions;
using ReflectionIT.Mvc.Paging;
using Microsoft.AspNetCore.Routing;

namespace ListOfCountries.Controllers
{
    public class HomeController : Controller
    {
        private IMemoryCache _cache;

        public HomeController(IMemoryCache cache)
        {
            _cache = cache;
        }

        public ActionResult Index(string filter, int page = 1)
        {
            var countries = GetCountries();
            var pageSize = 15;
            if (!String.IsNullOrEmpty(filter))
            {
                countries = countries.Where(s => s.Name.Contains(filter)
                                       || s.Region.Contains(filter) || s.Subregion.Contains(filter));
            }        

            var model = PagingList.Create(countries, pageSize, page);
            model.RouteValue = new RouteValueDictionary {
                { "filter", filter}
            };

            return View(model);
        }
        public ActionResult ShowCountry(string country)
        {
            return PartialView("CountryView", GetCountries().First(x => x.Name == country || x.Code == country));
        }

        public ActionResult ShowRegion(string region)
        {
            var countries = GetCountries().Where(x => x.Region == region);

            var Region = new Region()
            {
                Name = region,
                Population = countries.Sum(x => x.Population),
                Subregions = countries.Select(x => x.Subregion).Distinct(),
                Countries = countries.Select(x => x.Name)
            };
            return PartialView("RegionView", Region);
        }

        public ActionResult ShowSubregion(string subregion)
        {
            var countries = GetCountries().Where(x => x.Subregion == subregion);

            var Subregion = new Subregion()
            {
                Name = subregion,
                Population = countries.Sum(x => x.Population),
                Region = countries.Select(x => x.Region).First(x => x.HasValue()),
                Countries = countries.Select(x => x.Name)
            };

            return PartialView("SubRegionView", Subregion);
        }

        private IEnumerable<Country> GetCountries()
        {
            List<Country> countriesList;

            if (!_cache.TryGetValue(CacheKeys.Entry, out countriesList))
            {
                // Key not in cache, so get data.
                countriesList = DeserializeCountries.GetListOfCountriesFromApi();

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                // Save data in cache.
                _cache.Set(CacheKeys.Entry, countriesList, cacheEntryOptions);
            }

            return countriesList;
        }
    }
}
