using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LobbyManager
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ImageCollector" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ImageCollector.svc or ImageCollector.svc.cs at the Solution Explorer and start debugging.
    /// <summary>
    /// Clase que permite la recolección de datos de imagen a partir del servicio de windows instalado en cada estación de ingreso de visitantes.
    /// </summary>
    public class ImageCollector : IImageCollector
    {
        String mainConnectionString = "SykesVisitorsDB";
        
        /// <summary>
        /// Guarda las imagenes recolectadas desde una estación de captura de datos.
        /// </summary>
        /// <param name="desk">ID de Escritorio</param>
        /// <param name="front">Imagen Frontal en String Base64</param>
        /// <param name="back">Imagen Posterior en String Base64</param>
        /// <param name="profile">Imagen Facial en String Base64</param>
        /// <param name="ocr">Archivo de extracción de datos.</param>
        public void SaveImages(String desk, String front, String back, String profile, String ocr)
        {
            string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "DELETE FROM tbl_temp_images \n" +
                                  "WHERE  temp_desk = @temp_desk";
                cmd.Parameters.AddWithValue("temp_desk", desk);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "INSERT INTO tbl_temp_images (temp_desk, temp_front, temp_back, temp_profile, temp_ocr) \n" +
                                  "values (@temp_desk, @temp_front, @temp_back, @temp_profile, @temp_ocr)";
                cmd.Parameters.AddWithValue("temp_desk", desk);
                cmd.Parameters.AddWithValue("temp_front", front);
                cmd.Parameters.AddWithValue("temp_back", back);
                cmd.Parameters.AddWithValue("temp_profile", profile);
                cmd.Parameters.AddWithValue("temp_ocr", ocr);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        /// <summary>
        /// Monitorea la impresión de viñetas para ingreso de equipo.
        /// </summary>
        /// <param name="desk">ID de Escritorio</param>
        /// <returns></returns>
        public String label(string desk)
        {
            String lbl = null;

            string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT TOP 1 lbl_desk, lbl_name, lbl_serial, lbl_equipment, lbl_desc, lbl_owner FROM tbl_lbl_labels where lbl_desk = @lbl_desk";
                cmd.Parameters.AddWithValue("lbl_desk", desk);
                SqlDataReader dreader = cmd.ExecuteReader();
                if (dreader.Read())
                {
                    String name = dreader["lbl_name"].ToString().Trim();
                    String serial = dreader["lbl_serial"].ToString().Trim();
                    String bcode = dreader["lbl_equipment"].ToString().Trim();
                    String desc = dreader["lbl_desc"].ToString().Trim();
                    String owner = dreader["lbl_owner"].ToString().Trim();

                    String[] line = { name, serial, bcode, desc, owner };
                    lbl = name + "|" + serial + "|" + bcode + "|" + desc + "|" + owner;
                }
                dreader.Close();

                if (lbl != null)
                {
                    cmd.CommandText = "DELETE FROM tbl_lbl_labels \n" +
                                      "WHERE  lbl_desk = @desk and lbl_name = @name and lbl_serial = @serial and lbl_equipment = @equipment and lbl_desc = @desc and lbl_owner = @owner";
                    cmd.Parameters.AddWithValue("desk", desk);
                    cmd.Parameters.AddWithValue("name", lbl.Split('|')[0]);
                    cmd.Parameters.AddWithValue("serial", lbl.Split('|')[1]);
                    cmd.Parameters.AddWithValue("equipment", lbl.Split('|')[2]);
                    cmd.Parameters.AddWithValue("desc", lbl.Split('|')[3]);
                    cmd.Parameters.AddWithValue("owner", lbl.Split('|')[4]);
                    cmd.ExecuteNonQuery();
                }
                
                conn.Close();
            }

            return lbl;
        }
    }
}
