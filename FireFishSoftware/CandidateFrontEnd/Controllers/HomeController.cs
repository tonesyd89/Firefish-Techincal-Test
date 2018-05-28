using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CandidateApi.Models;
using Newtonsoft.Json;

namespace CandidateFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            IEnumerable<Candidate> candidates = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60946");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string test = client.BaseAddress.ToString();
                HttpResponseMessage res = await client.GetAsync("/api/Candidate");


                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    candidates = JsonConvert.DeserializeObject<List<Candidate>>(response);
                }
            }
            return View(candidates);
        }

        public async Task<ActionResult> Details(string id)
        {
            Candidate candidate = new Candidate();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60946");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync($"/api/Candidate/{id}");

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    candidate = JsonConvert.DeserializeObject<Candidate>(response);
                }

            }
            return View(candidate);
        }

        //public async Task<ActionResult> Edit(string id, Candidate candidate)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://localhost:60946");
        //        client.DefaultRequestHeaders.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        HttpResponseMessage res = await client.GetAsync($"/api/Candidate/{candidate}");
        //    }
        //}
        }
}