using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using ABB.Catalogo.Entidades.Core;
using ABB.Catalogo.LogicaNegocio.Core;

namespace WebServicesAbb.Controllers
{
    [Authorize]
    public class ProductosController : ApiController
    {
        // GET: api/Productos
        [HttpGet]
        public IEnumerable<Producto> Get()
        {
            List<Producto> productos = new List<Producto>();
            productos = new ProductoLN().ListarProductos();

            return productos;
        }

        public Producto buscarProducto([FromUri] int IdProducto)
        {
            try
            {
                ProductoLN producto = new ProductoLN();
                return producto.buscarProducto(IdProducto);
            }
            catch (Exception ex)
            {
                string innerException = (ex.InnerException == null) ? "" : ex.InnerException.ToString();
                //Logger.paginaNombre = this.GetType().Name;
                //Logger.Escribir("Error en Logica de Negocio: " + ex.Message + ". " + ex.StackTrace + ". " + innerException);
                throw;
            }


        }
        // GET: api/Productos/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Productos
        public void Post([FromBody] Producto value)
        {
            Producto producto = new ProductoLN().InsertarProducto(value);
        }

        // PUT: api/Productos/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Productos/5
        public void Delete(int id)
        {
        }
    }
}
