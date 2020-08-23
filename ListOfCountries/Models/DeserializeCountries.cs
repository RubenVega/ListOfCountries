using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ListOfCountries.Models
{
    public class DeserializeCountries
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Currency
        {
            [JsonProperty("code")]
            public string Code;

            [JsonProperty("name")]
            public string Name;

            [JsonProperty("symbol")]
            public string Symbol;
        }

        public class Language
        {
            [JsonProperty("iso639_1")]
            public string Iso6391;

            [JsonProperty("iso639_2")]
            public string Iso6392;

            [JsonProperty("name")]
            public string Name;

            [JsonProperty("nativeName")]
            public string NativeName;
        }

        public class Translations
        {
            [JsonProperty("de")]
            public string De;

            [JsonProperty("es")]
            public string Es;

            [JsonProperty("fr")]
            public string Fr;

            [JsonProperty("ja")]
            public string Ja;

            [JsonProperty("it")]
            public string It;

            [JsonProperty("br")]
            public string Br;

            [JsonProperty("pt")]
            public string Pt;
        }

        public class RegionalBloc
        {
            [JsonProperty("acronym")]
            public string Acronym;

            [JsonProperty("name")]
            public string Name;

            [JsonProperty("otherAcronyms")]
            public List<string> OtherAcronyms;

            [JsonProperty("otherNames")]
            public List<string> OtherNames;
        }

        public class MyArray
        {
            [JsonProperty("name")]
            public string Name;

            [JsonProperty("topLevelDomain")]
            public List<string> TopLevelDomain;

            [JsonProperty("alpha2Code")]
            public string Alpha2Code;

            [JsonProperty("alpha3Code")]
            public string Alpha3Code;

            [JsonProperty("callingCodes")]
            public List<string> CallingCodes;

            [JsonProperty("capital")]
            public string Capital;

            [JsonProperty("altSpellings")]
            public List<string> AltSpellings;

            [JsonProperty("region")]
            public string Region;

            [JsonProperty("subregion")]
            public string Subregion;

            [JsonProperty("population")]
            public int Population;

            [JsonProperty("latlng")]
            public List<double> Latlng;

            [JsonProperty("demonym")]
            public string Demonym;

            [JsonProperty("area")]
            public double Area;

            [JsonProperty("gini")]
            public double Gini;

            [JsonProperty("timezones")]
            public List<string> Timezones;

            [JsonProperty("borders")]
            public List<string> Borders;

            [JsonProperty("nativeName")]
            public string NativeName;

            [JsonProperty("numericCode")]
            public string NumericCode;

            [JsonProperty("currencies")]
            public List<Currency> Currencies;

            [JsonProperty("languages")]
            public List<Language> Languages;

            [JsonProperty("translations")]
            public Translations Translations;

            [JsonProperty("flag")]
            public string Flag;

            [JsonProperty("regionalBlocs")]
            public List<RegionalBloc> RegionalBlocs;

            [JsonProperty("cioc")]
            public string Cioc;
        }

        public class Root
        {
            [JsonProperty("MyArray")]
            public List<MyArray> MyArray;

            public IEnumerable<Country> GetCountries()
            {
                return this.MyArray.Select(x => new Country
                {
                    Name = x.Name,
                    Code = x.Alpha3Code,
                    CapitalCity = x.Capital,
                    Region = x.Region,
                    Subregion = x.Subregion,
                    Population = x.Population/1000,
                    Currencies = x.Currencies.Select(y => y.Name),
                    Languages = x.Languages.Select(y => y.Name),
                    BorderingCountries = x.Borders
                });
            }
        }

        public static List<Country> GetListOfCountriesFromApi(string endpoint = "https://restcountries.eu/rest/v2/all")
        {
            var client = new RestClient(endpoint);
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "restcountries-v1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "SIGN-UP-FOR-KEY");
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var errors = new List<string>();
                var data = JsonConvert.DeserializeObject<List<MyArray>>(response.Content,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs earg)
                        {
                            errors.Add(earg.ErrorContext.Member.ToString());
                            earg.ErrorContext.Handled = true;
                        }
                    });

                return (new Root { MyArray = data }).GetCountries().ToList();
            }
            else //web api sent error response 
            {
                return new List<Country> { new Country() };
            }
        }
    }
}