using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace APIClimaTempo.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet([FromServices]IConfiguration config)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string baseURL = config.GetSection("ClimaTempo_API:BaseURL").Value;
                string key = config.GetSection("ClimaTempo_API:Key").Value;
                string cidade = config.GetSection("ClimaTempo_API:CidadeID").Value;
                string urlMarine = config.GetSection("MarineTraffic_API:BaseURL").Value;
                string keyMarine = config.GetSection("MarineTraffic_API:Key").Value;
                string MINLAT = config.GetSection("MarineTraffic_API:MINLAT").Value;
                string MAXLAT = config.GetSection("MarineTraffic_API:MAXLAT").Value;
                string MINLON = config.GetSection("MarineTraffic_API:MINLON").Value;
                string MAXLON = config.GetSection("MarineTraffic_API:MAXLON").Value;
                HttpResponseMessage response = client.GetAsync(baseURL + "locale/"+cidade+"/days/15?token="+key).Result;

                response.EnsureSuccessStatusCode();
                string conteudo = response.Content.ReadAsStringAsync().Result;

                dynamic resultado = JsonConvert.DeserializeObject(conteudo);

                List<ClimaTempo> lClimaTempo = new List<ClimaTempo>();
                foreach (var item in resultado.data)
                {
                    ClimaTempo climaTempo = new ClimaTempo();

                    climaTempo.Data = item.date_br;
                    climaTempo.UmidadeMax = item.humidity.min;
                    climaTempo.UmidadeMin = item.humidity.max;
                    climaTempo.ProbabilidadeChuva = item.rain.probability;
                    climaTempo.VelocidadeMaximaVento = item.wind.velocity_max;
                    climaTempo.VelocidadeMinimaVento = item.wind.velocity_min;
                    climaTempo.SensacaoTermicaMaxima = item.thermal_sensation.max;
                    climaTempo.SensacaoTermicaMinima = item.thermal_sensation.min;
                    climaTempo.Tempo = item.text_icon.text.pt;
                    climaTempo.TemperaturaMaxima = item.temperature.max;
                    climaTempo.TemperaturaMinima = item.temperature.min;
                    climaTempo.NascerSol = item.sun.sunrise;
                    climaTempo.PorSol = item.sun.sunset;

                    lClimaTempo.Add(climaTempo);
                }
                response = client.GetAsync(urlMarine + "/" + keyMarine + "/MINLAT:" + MINLAT + "/MAXLAT:" + MAXLAT + "/MINLON:" + MINLON + "/MAXLON:" + MAXLON + "/timespan:2880/protocol:jsono").Result;
                response.EnsureSuccessStatusCode();
                conteudo = response.Content.ReadAsStringAsync().Result;
                resultado = JsonConvert.DeserializeObject(conteudo);
                List<MarineTraffic> lMarineTraffic = new List<MarineTraffic>();
                foreach (var item in resultado)
                {
                    MarineTraffic marineTraffic = new MarineTraffic();
                    marineTraffic.MMSI = item.MMSI;
                    marineTraffic.Speed = (item.SPEED / 10)*1.852;
                    marineTraffic.Data = item.TIMESTAMP;
                    marineTraffic.Data = marineTraffic.Data.AddHours(-3);
                    lMarineTraffic.Add(marineTraffic);
                }
                TempData["ClimaTempo"] = lClimaTempo;
                TempData["MarineTraffic"] = lMarineTraffic;
            }
        }
    }
}
