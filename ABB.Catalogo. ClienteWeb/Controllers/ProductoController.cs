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
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace ABB.Catalogo.ClienteWeb.Controllers
{
    public class ProductoController : Controller
    {

        string RutaApi = "https://localhost:44395/Api/"; //define la ruta del web api
        string jsonMediaType = "application/json"; // define el tipo de dat
        // GET: Producto
        public ActionResult Index()
        {
            string controladora = "productos";
            string metodo = "Get";
            List<Producto> listaproductos = new List<Producto>();
            using (WebClient producto = new WebClient())
            {
                try
                {
                    producto.Headers.Clear();//borra datos anteriores
                                             //establece el tipo de dato de tranferencia
                    producto.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                    //typo de decodificador reconocimiento carecteres especiales
                    producto.Encoding = UTF8Encoding.UTF8;
                    string rutacompleta = RutaApi + controladora;
                    //ejecuta la busqueda en la web api usando metodo GET
                    var data = producto.DownloadString(new Uri(rutacompleta));
                    // convierte los datos traidos por la api a tipo lista de usuarios
                    listaproductos = JsonConvert.DeserializeObject<List<Producto>>(data);

                }
                catch (Exception er)
                {

                }

            }

            return View(listaproductos);
        }

        // GET: Producto/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Producto/Create
        public ActionResult Create()
        {
            Producto producto = new Producto();// se crea una instancia de la clase producto

            List<Categoria> listacategoria = new List<Categoria>();
            listacategoria = new CategoriaLN().ListaCategoria();
            listacategoria.Add(new Categoria() { IdCategoria = 0, DescCategoria = "[Seleccione Categoria...]" });
            ViewBag.listaCategorias = listacategoria;

            return View(producto);
        }

        // POST: Producto/Create
        [HttpPost]
        public ActionResult Create(Producto collection)
        {
            string controladora = "productos";
            HttpPostedFileBase file = Request.Files[0];
            var length = file.InputStream.Length; //Length: 103050706
            byte[] fileData = null;
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                fileData = binaryReader.ReadBytes(file.ContentLength);
            }
            collection.Imagen = fileData;
            try
            {
                // TODO: Add insert logic here
                using (WebClient producto = new WebClient())
                {
                    producto.Headers.Clear();//borra datos anteriores
                                             //establece el tipo de dato de tranferencia
                    producto.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                    //typo de decodificador reconocimiento carecteres especiales
                    producto.Encoding = UTF8Encoding.UTF8;
                    //convierte el objeto de tipo Usuarios a una trama Json
                    var usuarioJson = JsonConvert.SerializeObject(collection);
                    string rutacompleta = RutaApi + controladora;
                    var resultado = producto.UploadString(new Uri(rutacompleta), usuarioJson);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult getImage(int id)
        {
            Producto producto = new ProductoLN().buscarProducto(id);
            byte[] byteImage = producto.Imagen;
            MemoryStream memoryStream = new MemoryStream(byteImage);
            Image image = Image.FromStream(memoryStream);
            memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Png);
            memoryStream.Position = 0;
            return File(memoryStream, "image/png");
        }

        // GET: Producto/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Producto/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Producto/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Producto/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
