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
    public class ImageCollector : IImageCollector
    {
        String mainConnectionString = "SykesVisitorsDB";
        
        public void DoWork()
        {
        }
        
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
    }
}
