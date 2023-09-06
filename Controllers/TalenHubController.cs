using L1_TalentHub.Models;
using L1_TalentHub.Models.Datos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace L1_TalentHub.Controllers
{
    public class TalenHubController : Controller
    {
        // GET: TalenHub
        public ActionResult Index()
        {           
            return View(Singleton.Instance.ListaRegistros);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LeeArchivo(IFormFile ArchivoCargado)
        {
            try
            {
                var Archivo = new StreamReader(ArchivoCargado.OpenReadStream());
                {
                    string info = Archivo.ReadToEnd();
                    foreach (string Fila in info.Split("\n"))
                    {
                        if (!(string.IsNullOrEmpty(Fila)))
                        {
                            string jsonAccion = Fila.Split(";")[0];
                            string jsonString = Fila.Split(";")[1];

                            Registros NuevoRegistro = JsonConvert.DeserializeObject<Registros>(jsonString);

                            if (jsonAccion == "INSERT")
                            {
                                Singleton.Instance.AVLNombres.Agregar(NuevoRegistro, NuevoRegistro.InsertarPorNombre);
                            }
                            else if (jsonAccion == "PATCH")
                            {
                                Singleton.Instance.AVLNombres.Actualizar(NuevoRegistro, NuevoRegistro.InsertarPorNombre, NuevoRegistro.InsertarPorDPI);
                            }
                            else
                            {
                                Singleton.Instance.AVLNombres.Borrar(NuevoRegistro, NuevoRegistro.InsertarPorNombre, NuevoRegistro.InsertarPorDPI);
                            }

                            Singleton.Instance.ListaRegistros.Add(NuevoRegistro);                          
                        }
                    }
                }
                TempData["Mensaje"] = "Los Datos han sido cargados correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Index");
            }
        }

        public ActionResult Buscar(string id)
        {
            try
            {
                Registros NuevoRegistro = new Registros
                {
                    Nombre = id
                };

                Singleton.Instance.RegistrosEncontrados = Singleton.Instance.AVLNombres.Busqueda(NuevoRegistro, NuevoRegistro.InsertarPorNombre);

                if (Singleton.Instance.RegistrosEncontrados == null)
                {
                    TempData["Mensaje"] = "La Persona No Existe o Coloco Mal Los Datos";
                    return RedirectToAction(nameof(Index));
                }
                
                //Se asegura que exista la carpeta en donde almacenara los resultados de las busquedas
                if (!Directory.Exists(@"outputs"))
                {
                    System.IO.Directory.CreateDirectory("outputs");
                }

                string ruta = @"outputs/" + Singleton.Instance.RegistrosEncontrados[0].Nombre + ".csv";//Nombre del archivo

                for (int i = 0; i < Singleton.Instance.RegistrosEncontrados.Count; i++)
                {
                    string json = JsonConvert.SerializeObject(Singleton.Instance.RegistrosEncontrados[i]) + "\n";
                    System.IO.File.AppendAllText(ruta, json);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Mensaje"] = "Coloque Datos Para Buscar";
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
