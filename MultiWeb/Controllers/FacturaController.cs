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
using MultiApp.Model;

namespace AdapterWeb.Controllers
{
    public class FacturaController : Controller
    {
        public async Task<IActionResult> Index()
        {
            IList<factura> fac;
            string cadena = "";
            try
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    var request = new System.Net.Http.HttpRequestMessage();
                    request.RequestUri = new Uri("http://WebApi/api/factura");
                   // request.RequestUri = new Uri("https://localhost:44383/api/factura");

                    var response = await client.SendAsync(request);
                    var fct = await response.Content.ReadAsStringAsync();
                    cadena = fct.ToString();
                    var details = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<factura>>(fct);

                    fac = details.ToList();
                }
                return View(fac);
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
        public async Task<IEnumerable<factura>>OnGet()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var request = new System.Net.Http.HttpRequestMessage();
              request.RequestUri = new Uri("http://WebApi/api/factura");
              // request.RequestUri = new Uri("https://localhost:44383/api/factura");

                var response = await client.SendAsync(request);
                var fct = await response.Content.ReadAsStringAsync();
                var details = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<factura>>(fct);

                return details;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormCollection form) 
        {
            factura fcr = new factura();
            fcr.fecha = Convert.ToDateTime(form["fecha"]);
            fcr.descripcion = form["descripcion"];
            fcr.valor = Convert.ToDecimal(form["valor"]);

            await OnInsert(fcr);

            Task<IEnumerable<factura>> factura = OnGet();
            IList<factura> fct = factura.Result.ToList();
            return View(fct);
        }

        public async Task<bool> Delete(string id)
        {
            bool rtn = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://WebApi/api/");

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("factura/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return true;
                }
            }


            return rtn;
        }

        public async Task<bool> OnInsert(factura Factura)
        {
            bool rtn = true;
           
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(Factura), Encoding.UTF8, "application/json");

                //using (var response = await httpClient.PostAsync("https://localhost:44383/api/factura", content))
               using (var response = await httpClient.PostAsync("http://WebApi/api/factura", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return rtn;
        }

        [HttpGet]
        public async Task<IActionResult> eliminar(string codigo)
        {
            await Delete(codigo);

            Task<IEnumerable<factura>> factura = OnGet();
            IList<factura> fct = factura.Result.ToList();
            return View("index",fct);
        }
    }
}
