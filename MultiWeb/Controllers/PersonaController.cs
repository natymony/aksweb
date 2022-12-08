using AdapterWeb.Models;
using BuilderWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.Xml;
using Newtonsoft.Json;
using System.Text;
using MultiApp.Models;

namespace AdapterWeb.Controllers
{
    public class PersonaController : Controller
    {
        public async Task<IActionResult> Index()
        {
            IList<Persona> per;
            string cadena = "";
            try
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    var request = new System.Net.Http.HttpRequestMessage();
                    request.RequestUri = new Uri("http://WebApi/api/persona");
                    //request.RequestUri = new Uri("https://localhost:49199/api/persona");

                    var response = await client.SendAsync(request);
                    var prs = await response.Content.ReadAsStringAsync();
                    cadena = prs.ToString();
                    var details = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Persona>>(prs);

                    per = details.ToList();
                }
                return View(per);
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", new { message = "cadena: "+ cadena + "error:"+ e.Message + " " +e.InnerException });
            }
           
        }
        public ActionResult Error(string message)
        {
            var model = new ErrorModel();
            model.Error = message;
            return View(model); // make sure your error view is named as "Error". Else u need to specify the view name in the return command.
        }
        public async Task<IEnumerable<Persona>>OnGet()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var request = new System.Net.Http.HttpRequestMessage();
              request.RequestUri = new Uri("http://WebApi/api/persona");
              // request.RequestUri = new Uri("https://localhost:49199/api/persona");

                var response = await client.SendAsync(request);
                var prs = await response.Content.ReadAsStringAsync();
                var details = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Persona>>(prs);

                return details;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormCollection form)
        {
            Persona prs = new Persona();
            prs.cedula = form["cedula"];
            prs.nombre = form["nombre"];
            prs.apellido = form["apellido"];
            prs.direccion = form["direccion"];
            prs.telefono = form["telefono"];
            
            await OnInsert(prs);

            Task<IEnumerable<Persona>> persona = OnGet();
            IList<Persona> per = persona.Result.ToList();
            return View(per);
        }

        public async Task<bool> OnInsert(Persona persona)
        {
            bool rtn = true;
           
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(persona), Encoding.UTF8, "application/json");

               // using (var response = await httpClient.PostAsync("https://localhost:49165/api/persona", content))
                using (var response = await httpClient.PostAsync("http://WebApi/api/persona", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    
                }
            }
            return rtn;
        }
    }
}
