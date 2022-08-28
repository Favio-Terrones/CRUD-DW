using ABB.Catalogo.Entidades.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABB.Catalogo.Utiles.Helpers;
using System.Drawing;
using System.IO;

namespace ABB.Catalogo.AccesoDatos.Core
{
    public class ProductoDA
    {
        public Producto LlenarEntidad(IDataReader reader)
        {
            Producto producto = new Producto();
            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName = 'IdProducto'";
            if (reader.GetSchemaTable().DefaultView.Count.Equals(1))
            {
                if (!Convert.IsDBNull(reader["IdProducto"]))
                    producto.IdProducto = Convert.ToInt32(reader["IdProducto"]);

            }
            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName = 'IdCategoria'";
            if (reader.GetSchemaTable().DefaultView.Count.Equals(1))
            {
                if (!Convert.IsDBNull(reader["IdCategoria"]))
                    producto.IdCategoria = Convert.ToInt32(reader["IdCategoria"]);

            }
            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName = 'DescCategoria'";
            if (reader.GetSchemaTable().DefaultView.Count.Equals(1))
            {
                if (!Convert.IsDBNull(reader["DescCategoria"]))
                    producto.DescCategoria = Convert.ToString(reader["DescCategoria"]);

            }

            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName = 'NomProducto'";
            if (reader.GetSchemaTable().DefaultView.Count.Equals(1))
            {
                if (!Convert.IsDBNull(reader["NomProducto"]))
                    producto.NomProducto = Convert.ToString(reader["NomProducto"]);

            }

            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName = 'MarcaProducto'";
            if (reader.GetSchemaTable().DefaultView.Count.Equals(1))
            {
                if (!Convert.IsDBNull(reader["MarcaProducto"]))
                    producto.MarcaProducto = Convert.ToString(reader["MarcaProducto"]);

            }

            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName = 'ModeloProducto'";
            if (reader.GetSchemaTable().DefaultView.Count.Equals(1))
            {
                if (!Convert.IsDBNull(reader["ModeloProducto"]))
                    producto.ModeloProducto = Convert.ToString(reader["ModeloProducto"]);

            }

            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName = 'LineaProducto'";
            if (reader.GetSchemaTable().DefaultView.Count.Equals(1))
            {
                if (!Convert.IsDBNull(reader["LineaProducto"]))
                    producto.LineaProducto = Convert.ToString(reader["LineaProducto"]);

            }

            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName = 'GarantiaProducto'";
            if (reader.GetSchemaTable().DefaultView.Count.Equals(1))
            {
                if (!Convert.IsDBNull(reader["GarantiaProducto"]))
                    producto.GarantiaProducto = Convert.ToString(reader["GarantiaProducto"]);

            }

            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName = 'Precio'";
            if (reader.GetSchemaTable().DefaultView.Count.Equals(1))
            {
                if (!Convert.IsDBNull(reader["Precio"]))
                    producto.Precio = Convert.ToDecimal(reader["Precio"]);

            }

            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName = 'Imagen'";
            if (reader.GetSchemaTable().DefaultView.Count.Equals(1))
            {
                if (!Convert.IsDBNull(reader["Imagen"]))
                    producto.Imagen = (byte[])reader["Imagen"];
            }

            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName = 'DescripcionTecnica'";
            if (reader.GetSchemaTable().DefaultView.Count.Equals(1))
            {
                if (!Convert.IsDBNull(reader["DescripcionTecnica"]))
                    producto.DescripcionTecnica = Convert.ToString(reader["DescripcionTecnica"]);

            }
            return producto;
        }

        public List<Producto> ListarProductos()
        {
            List<Producto> ListaEntidad = new List<Producto>();
            Producto entidad = null;
            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["cnnSql"]].ConnectionString))
            {
                using (SqlCommand comando = new SqlCommand("paListarProductos", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    conexion.Open();
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        entidad = LlenarEntidad(reader);
                        ListaEntidad.Add(entidad);
                    }
                }
                conexion.Close();
            }
            return ListaEntidad;
        }

        public Producto InsertarProducto(Producto producto)
        {


            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["cnnSql"]].ConnectionString))
            {
                using (SqlCommand comando = new SqlCommand("paProducto_insertar", conexion))
                {

                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@IdCategoria", producto.IdCategoria);
                    comando.Parameters.AddWithValue("@NomProducto", producto.NomProducto);
                    comando.Parameters.AddWithValue("@MarcaProducto", producto.MarcaProducto);
                    comando.Parameters.AddWithValue("@ModeloProducto", producto.ModeloProducto);
                    comando.Parameters.AddWithValue("@LineaProducto", producto.LineaProducto);
                    comando.Parameters.AddWithValue("@GarantiaProducto", producto.GarantiaProducto);
                    comando.Parameters.AddWithValue("@Precio", producto.Precio);
                    comando.Parameters.AddWithValue("@Imagen", producto.Imagen);
                    comando.Parameters.AddWithValue("@DescripcionTecnica", producto.DescripcionTecnica);
                    conexion.Open();
                    producto.IdProducto = Convert.ToInt32(comando.ExecuteScalar());
                    conexion.Close();
                }
            }
            return producto;
        }
        public Producto buscarProducto(int IdProducto)
        {
            Producto entidad = null;
            try
            {

                using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["cnnSql"]].ConnectionString))
                {
                    using (SqlCommand comando = new SqlCommand("paProducto_BuscaProductoId", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@ParamProducto", IdProducto);
                        conexion.Open();
                        SqlDataReader reader = comando.ExecuteReader();
                        while (reader.Read())
                        {
                            entidad = LlenarEntidad(reader);

                        }
                        conexion.Close();
                    }

                }

                return entidad;
            }
            catch (Exception ex)
            {
                string innerException = (ex.InnerException == null) ? "" : ex.InnerException.ToString();
                //Logger.paginaNombre = this.GetType().Name;
                //Logger.Escribir("Error en Logica de Negocio: " + ex.Message + ". " + ex.StackTrace + ". " + innerException);
                return entidad;
            }

        }
    }
}

