using ABB.Catalogo.Entidades.Core;
using ABB.Catalogo.LogicaNegocio.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;

namespace ABB.Catalogo.ClienteWeb.Controllers
{
    public class UsuariosController : Controller
    {
        string RutaApi = "https://localhost:44395/Api/"; //define la ruta del web api
        string jsonMediaType = "application/json"; // define el tipo de dat
        // GET: Usuarios
        public ActionResult Index()
        {
            string controladora = "Usuarios";
            string metodo = "Get";


            TokenResponse tokenrsp = new TokenResponse();
            tokenrsp = Respuest();

            List<Usuarios> listausuarios = new List<Usuarios>();
            using (WebClient usuario = new WebClient())
            {
                try
                {
                    usuario.Headers.Clear();//borra datos anteriores
                    usuario.Headers[HttpRequestHeader.Authorization] = "Bearer " + tokenrsp.Token;

                    //establece el tipo de dato de tranferencia
                    usuario.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                    //typo de decodificador reconocimiento carecteres especiales
                    usuario.Encoding = UTF8Encoding.UTF8;
                    string rutacompleta = RutaApi + controladora;
                    //ejecuta la busqueda en la web api usando metodo GET
                    var data = usuario.DownloadString(new Uri(rutacompleta));
                    // convierte los datos traidos por la api a tipo lista de usuarios
                    listausuarios = JsonConvert.DeserializeObject<List<Usuarios>>(data);
                }
                catch (Exception ex)
                {
                    
                }

            }

            return View(listausuarios);
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            Usuarios usuario = new Usuarios();// se crea una instancia de la clase usuario

            List<Rol> listarol = new List<Rol>();
            listarol = new RolLN().ListaRol();
            listarol.Add(new Rol() { IdRol = 0, DesRol = "[Seleccione Rol...]" });
            ViewBag.listaRoles = listarol;

            return View(usuario);

        }

        // POST: Usuarios/Create
        [HttpPost]
        public ActionResult Create(Usuarios collection)
        {
            string controladora = "Usuarios";
            try
            {
                TokenResponse tokenrsp = new TokenResponse();
                tokenrsp = Respuest();


                // TODO: Add insert logic here
                using (WebClient usuario = new WebClient())
                {
                    usuario.Headers.Clear();//borra datos anteriores
                   
                    usuario.Headers[HttpRequestHeader.Authorization] = "Bearer " + tokenrsp.Token;

                    //establece el tipo de dato de tranferencia
       
                    usuario.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                    //typo de decodificador reconocimiento carecteres especiales
                    usuario.Encoding = UTF8Encoding.UTF8;
                    //convierte el objeto de tipo Usuarios a una trama Json
                    var usuarioJson = JsonConvert.SerializeObject(collection);
                    string rutacompleta = RutaApi + controladora;
                    var resultado = usuario.UploadString(new Uri(rutacompleta), usuarioJson);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: Usuarios/Edit/5
        public ActionResult Edit(int id)
        {
            string controladora = "Usuarios";
            string metodo = "GetUserId";

            TokenResponse tokenrsp = new TokenResponse();
            tokenrsp = Respuest();

            Usuarios users = new Usuarios();

            using (WebClient usuario = new WebClient())
            {
                usuario.Headers.Clear();//borra datos anteriores

                usuario.Headers[HttpRequestHeader.Authorization] = "Bearer " + tokenrsp.Token;

                //establece el tipo de dato de tranferencia
                usuario.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                //typo de decodificador reconocimiento carecteres especiales
                usuario.Encoding = UTF8Encoding.UTF8;
                string rutacompleta = RutaApi + controladora + "?IdUsuario=" + id;
                //ejecuta la busqueda en la web api usando metodo GET
                var data = usuario.DownloadString(new Uri(rutacompleta));

                // convierte los datos traidos por la api a tipo lista de usuarios
                users = JsonConvert.DeserializeObject<Usuarios>(data);
            }
            List<Rol> listarol = new List<Rol>();
            listarol = new RolLN().ListaRol();
            listarol.Add(new Rol() { IdRol = 0, DesRol = "[Seleccione Rol...]" });
            ViewBag.listaRoles = listarol;
            return View(users);
        }


        // POST: Usuarios/Edit/5
        [HttpPost]
        public ActionResult Edit(int Id, Usuarios collection)
        {
            string controladora = "Usuarios";
            collection.IdUsuario = Id;
            collection.ClaveTxt = "";
            collection.Clave = null;
            collection.DesRol = "";
            string metodo = "Put";
            TokenResponse tokenrsp = new TokenResponse();
            try 
            {
                tokenrsp = Respuest();
                 
                using (var usuario = new HttpClient())
                {
                    usuario.DefaultRequestHeaders.Clear();//borra datos anteriores
                                                          //establece el token de autorizacion en la cabecera
                                                          //usuario.Headers[HttpRequestHeader.Authorization] = "Bearer " + tokenrsp.Token;
                                                          //establece el tipo de dato de tranferencia
                    usuario.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenrsp.Token);
                    usuario.BaseAddress = new Uri(RutaApi);
                    //usuario.Headers[HttpRequestHeader.ContentType] = jsonMediaType;     ACA SE COMENTO
                    //typo de decodificador reconocimiento carecteres especiales
                    //usuario.Encoding = Encoding.UTF8;                                   ACA SE COMENTO
                    //convierte el objeto de tipo Usuarios a una trama Json
                    //var usuarioJson = JsonConvert.SerializeObject(collection);           ACA SE COMENTO
                    //string rutacompleta = RutaApi + controladora + "?IdUsuario=" + Id;
                    //string rutacompleta = RutaApi + controladora + "?=" + Id;
                    string rutacompleta = RutaApi + controladora + "/" + Id;
                    var putTask = usuario.PutAsJsonAsync(rutacompleta, collection);
                    putTask.Wait();
                    var result = putTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    //var resultado = usuario.UploadString((rutacompleta), metodo, usuarioJson);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return RedirectToAction("Index");
                //return View();
            }
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int id)
        {
            string controladora = "Usuarios";
            string metodo = "GetUserId";
            Usuarios users = new Usuarios();
            TokenResponse tokenrsp = new TokenResponse();
            //llamada al web Api de Autorizacion.
            tokenrsp = Respuest();


            using (WebClient usuario = new WebClient())
            {
                usuario.Headers.Clear();//borra datos anteriores
                //establece el tipo de dato de tranferencia
                usuario.Headers[HttpRequestHeader.Authorization] = "Bearer " + tokenrsp.Token;
                usuario.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                //typo de decodificador reconocimiento carecteres especiales
                usuario.Encoding = UTF8Encoding.UTF8;
                string rutacompleta = RutaApi + controladora + "?IdUsuario=" + id;
                //ejecuta la busqueda en la web api usando metodo GET
                var data = usuario.DownloadString(new Uri(rutacompleta));

                // convierte los datos traidos por la api a tipo lista de usuarios
                users = JsonConvert.DeserializeObject<Usuarios>(data);
            }
            return View(users);
        }


       
        // POST: Usuarios/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Usuarios collection)
        {

            string controladora = "Usuarios";
            collection.IdUsuario = id;
            collection.ClaveTxt = "";
            collection.Clave = null;
            collection.DesRol = "";
            string metodo = "Delete";
            TokenResponse tokenrsp = new TokenResponse();

            try
            {
                tokenrsp = Respuest();

                using (WebClient usuario = new WebClient())
                {

                    usuario.Headers.Clear();//borra datos anteriores
                                            //establece el token de autorizacion en la cabecera
                                            //usuario.Headers[HttpRequestHeader.Authorization] = "Bearer " + tokenrsp.Token;
                                            //establece el tipo de dato de tranferencia
                    usuario.Headers[HttpRequestHeader.Authorization] = "Bearer " + tokenrsp.Token;
                    usuario.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                    //typo de decodificador reconocimiento carecteres especiales
                    usuario.Encoding = UTF8Encoding.UTF8;
                    //convierte el objeto de tipo Usuarios a una trama Json
                    var usuarioJson = JsonConvert.SerializeObject(collection);
                    string rutacompleta = RutaApi + controladora + "/" + id;
                    //string rutacompleta = RutaApi + controladora + "?=" + Id;
                    var resultado = usuario.UploadString(new Uri(rutacompleta), metodo, usuarioJson);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //var msg = ex.Message;
                return RedirectToAction("Index");
                //return View();
            }
        }


        private TokenResponse Respuest()
        {
            TokenResponse respuesta = new TokenResponse();
            string controladora = "Auth";
            string metodo = "Post";
            var resultado = "";
            UsuariosApi usuapi = new UsuariosApi();
            usuapi.Codigo = Convert.ToInt32(ConfigurationManager.AppSettings["Codigo"]);
            usuapi.UserName = ConfigurationManager.AppSettings["UserName"];
            usuapi.Clave = ConfigurationManager.AppSettings["Clave"];
            usuapi.Nombre = ConfigurationManager.AppSettings["Nombre"];
            usuapi.Rol = ConfigurationManager.AppSettings["Rol"];
            using (WebClient usuarioapi = new WebClient())
            {
                usuarioapi.Headers.Clear();//borra datos anteriores
                                           //establece el tipo de dato de tranferencia
                usuarioapi.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                //typo de decodificador reconocimiento carecteres especiales
                usuarioapi.Encoding = UTF8Encoding.UTF8;
                //convierte el objeto de tipo Usuarios a una trama Json
                var usuarioJson = JsonConvert.SerializeObject(usuapi);
                string rutacompleta = RutaApi + controladora;
                resultado = usuarioapi.UploadString(new Uri(rutacompleta), usuarioJson);
                respuesta = JsonConvert.DeserializeObject<TokenResponse>(resultado);
            }
            return respuesta;
        }

    }

}
