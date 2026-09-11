using Microsoft.Data.SqlClient; 
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AccesoDatos
{
    public class RegionServicio
    {
        private readonly string _cadenaConexion;

        // Constructor: recibe la cadena de conexión
        public RegionServicio(IConfiguration configuracion)
        {
            //_cadenaConexion = configuracion.GetConnectionString()!;
            _cadenaConexion = "Data Source = (localdb)\\mssqllocaldb; Initial Catalog = ValueTech; Integrated Security = True; Multiple Active Result Sets = True";
        }

       
        public List<Region> ObtenerTodasRegiones()
        {
            var lista = new List<Region>();
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_Region_Listar", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure; 

                    using (var lector = cmd.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Region
                            {
                                IdRegion = Convert.ToInt32(lector["IdRegion"]),
                                Nombre = lector["Nombre"].ToString()!
                            });
                        }
                    }
                }
            }
            return lista;
        }

        // 🔹 Obtener UNA región por ID
        public Region? ObtenerRegionPorId(int idRegion)
        {
            Region? region = null;
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_Region_ObtenerPorId", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdRegion", idRegion);

                    using (var lector = cmd.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            region = new Region
                            {
                                IdRegion = Convert.ToInt32(lector["IdRegion"]),
                                Nombre = lector["Nombre"].ToString()!
                            };
                        }
                    }
                }
            }
            return region;
        }

        public Comuna? ObtenerComuna(int idComuna)
        {
            Comuna? comuna = null;
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_Comuna_Obtener", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdComuna", idComuna);

                    using (var lector = cmd.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            comuna = new Comuna
                            {
                                IdRegion = Convert.ToInt32(lector["IdRegion"]),
                                IdComuna = Convert.ToInt32(lector["IdComuna"]),
                                Nombre = lector["Nombre"].ToString()!,
                                InformacionAdicional = new InformacionGeografica
                                {
                                    Superficie = (decimal)Convert.ToDouble(lector["Superficie"]),
                                    Poblacion = Convert.ToInt32(lector["Poblacion"]),
                                    Densidad = Convert.ToInt32(lector["Densidad"])
                                }

                            };
                        }
                    }
                }
            }
            return comuna;

        }

        public List<Comuna> ObtenerComunasPorRegion(int idRegion)
        {
            var lista = new List<Comuna>();
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_Comuna_ObtenerPorIdRegion", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdRegion", idRegion);

                    using (var lector = cmd.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            var xmlDatos = lector["InformacionAdicional"].ToString();
                            var xml = System.Xml.Linq.XDocument.Parse(xmlDatos);
                            var etiquetaInfo = xml.Element("Info");
                            var comuna = new Comuna
                            {
                                IdRegion = Convert.ToInt32(lector["IdRegion"]),
                                IdComuna = Convert.ToInt32(lector["IdComuna"]),
                                Nombre = lector["Nombre"].ToString()!,
                                InformacionAdicional = new InformacionGeografica
                                {
                                    Superficie = Convert.ToDecimal(etiquetaInfo.Element("Superficie").Value),
                                    Poblacion = Convert.ToInt32(etiquetaInfo.Element("Poblacion").Value),
                                    Densidad = Convert.ToDecimal(etiquetaInfo.Element("Poblacion").Attribute("Densidad").Value)
                                }
                            };
                            lista.Add(comuna);
                        }
                    }
                }
            }
            return lista;
        }

        public void EjecutarMerge(Comuna comuna)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_Comuna_Merge", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdComuna", comuna.IdComuna);
                    cmd.Parameters.AddWithValue("@Nombre", comuna.Nombre);
                    cmd.ExecuteNonQuery();
                }
            }
        } 
    }
}