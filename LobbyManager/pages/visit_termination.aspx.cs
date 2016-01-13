using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace LobbyManager.pages
{
    /// <summary>
    /// Clase principal de la página de aprobación de visitantes.
    /// </summary>
    public partial class visit_termination : System.Web.UI.Page
    {
        String mainConnectionString = "SykesVisitorsDB";

        /// <summary>
        /// Establece si se deberá presentar el mensaje de finalización del proceso.
        /// </summary>
        public bool showMg = false;

        /// <summary>
        /// Establece si el destino procede a la página para agregar equipo.
        /// </summary>
        public bool addEQ = false;

        /// <summary>
        /// Establece si el destino procede a la lista de visitantes en espera.
        /// </summary>
        public bool returnToList = false;

        /// <summary>
        /// Contiene el ID del visitante.
        /// </summary>
        public int visitor = 0;

        /// <summary>
        /// Contiene el valor del parámetro de visitante a consultar.
        /// </summary>
        public String visitorID = "";

        /// <summary>
        /// Se ejecuta el iniciar la carga.
        /// </summary>
        /// <param name="sender">Objecto que llama a la acción</param>
        /// <param name="e">Evento Ejecutado</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            msgWarn.Visible = false;

            if (Request.QueryString["visitor"] != null)
            {
                visitorID = Request.QueryString["visitor"].ToString();
                if (!visitorID.Equals(""))
                {
                    try
                    {
                        visitor = Convert.ToInt16(visitorID);
                        if (!IsPostBack)
                        {
                            getVisitor();
                            getImages();
                        }
                    }
                    catch
                    {
                        visitor = 0;
                    }
                }
            }

            if (Request.QueryString["finish"] != null)
            {
                String finishFlag = Request.QueryString["finish"].ToString();
                if (finishFlag.Equals("true"))
                {
                    showMg = true;
                }
            }
        }

        /// <summary>
        /// Obtiene los datos del visitante seleccionado en la variable "visitor".
        /// </summary>
        public void getVisitor()
        {
            try
            {
                msgWarn.Visible = false;
                int vis_id = visitor;
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT vis_id, vis_name, vis_lastname, vis_doctype, vis_docnumber, vis_company, vis_phone, vis_reason, vis_description, vis_department, vis_internal_contact, vis_date, vis_status, vis_image_record, vis_with_equipment \n" +
                                      "FROM [tbl_vis_visitors] WHERE vis_id = @vis_id";
                    cmd.Parameters.AddWithValue("vis_id", vis_id);
                    //cmd.Parameters.AddWithValue("vis_image_record", 0);
                    
                    SqlDataReader dreader = cmd.ExecuteReader();
                    if (dreader.Read())
                    {
                        txt_name.Value = dreader["vis_name"].ToString().Trim();
                        txt_lastname.Value = dreader["vis_lastname"].ToString().Trim();
                        docTypeSelect.SelectedValue = dreader["vis_doctype"].ToString().Trim();
                        txt_docnumber.Value = dreader["vis_docnumber"].ToString().Trim();
                        txt_company.Value = dreader["vis_company"].ToString().Trim();
                        txt_phone.Value = dreader["vis_phone"].ToString().Trim();
                        reasonSelect.SelectedValue = dreader["vis_reason"].ToString().Trim();
                        txt_description.Value = dreader["vis_description"].ToString().Trim();
                        deptSelect.SelectedValue = dreader["vis_department"].ToString().Trim();
                        txt_contact.Value = dreader["vis_internal_contact"].ToString().Trim();
                        chk_addEQ.Checked = dreader["vis_with_equipment"].ToString().Trim().Equals("1");
                    }
                    dreader.Close();
                    conn.Close();
                }
            }
            catch (Exception a)
            {
                Response.Write(a.Message);
                msgWarn.Visible = true;
            }
        }

        /// <summary>
        /// Obtiene las imagenes del visitante seleccionado en la variable "visitor".
        /// </summary>
        public void getImages()
        {
            try
            {
                msgWarn.Visible = false;
                int vis_id = visitor;
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT img_id, img_visitor, img_front, img_back, img_profile FROM [tbl_img_images] WHERE img_visitor = @vis_id";
                    cmd.Parameters.AddWithValue("vis_id", vis_id);
                    //cmd.Parameters.AddWithValue("vis_image_record", 0);

                    SqlDataReader dreader = cmd.ExecuteReader();
                    if (dreader.Read())
                    {
                        ((HtmlImage)img_front).Src = @"data:image/bmp;base64," + dreader["img_front"].ToString().Trim();
                        ((HtmlImage)img_back).Src = @"data:image/bmp;base64," + dreader["img_back"].ToString().Trim();
                    }
                    dreader.Close();
                    conn.Close();
                }
            }
            catch (Exception a)
            {
                Response.Write(a.Message);
                msgWarn.Visible = true;
            }
        }

        /// <summary>
        /// Verifica registro de visitante.
        /// </summary>
        /// <param name="sender">Objeto que llama a la acción</param>
        /// <param name="e">Evento Ejecutado</param>
        public void UpdateVisitor(object sender, EventArgs e)
        {
            getVisitor();
            getImages();
            if (chk_addEQ.Checked)
            {
                addEQ = true;
            }
            else
            {
                showMg = true;
            }
        }

        /// <summary>
        /// Finaliza la visita.
        /// </summary>
        /// <param name="sender">Objeto que llama a la acción</param>
        /// <param name="e">Evento Ejecutado</param>
        public void finishVisit(object sender, EventArgs e)
        {
            try
            {
                msgWarn.Visible = false;
                int vis_id = visitor;
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "UPDATE [tbl_vis_visitors] SET vis_status = @vis_status, vis_checkout = GETDATE() \n" +
                                    "WHERE vis_id = @vis_id";
                    cmd.Parameters.AddWithValue("vis_id", vis_id);
                    cmd.Parameters.AddWithValue("vis_status", 0);

                    cmd.ExecuteNonQuery();

                    conn.Close();
                    //CleanForm();

                    returnToList = true;
                }
            }
            catch (Exception a)
            {
                Response.Write(a.Message);
                msgWarn.Visible = true;
            }
        }

        /// <summary>
        /// Finaliza la visita.
        /// </summary>
        /// <param name="sender">Objeto que llama a la acción</param>
        /// <param name="e">Evento Ejecutado</param>
        public void startVisit(object sender, EventArgs e)
        {
            try
            {
                msgWarn.Visible = false;
                int vis_id = visitor;
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "UPDATE [tbl_vis_visitors] SET " +
                                      "vis_visitor_card = @vis_visitor_card, vis_in_charge = @vis_in_charge, vis_status = @vis_status \n" +
                                      "WHERE vis_id = @vis_id";
                    cmd.Parameters.AddWithValue("vis_id", vis_id);
                    cmd.Parameters.AddWithValue("vis_visitor_card", txtBadge.Value);
                    cmd.Parameters.AddWithValue("vis_in_charge", txtEmp.Value);
                    cmd.Parameters.AddWithValue("vis_status", 2);

                    cmd.ExecuteNonQuery();
                    conn.Close();

                    returnToList = true;
                }
            }
            catch (Exception a)
            {
                Response.Write(a.Message);
                msgWarn.Visible = true;
            }
        }

        /// <summary>
        /// Limpia el formulario de ingreso de visitantes.
        /// </summary>
        public void CleanForm()
        {
            txt_name.Value = "";
            txt_lastname.Value = "";
            txt_docnumber.Value = "";
            txt_company.Value = "";
            txt_phone.Value = "";
            txt_description.Value = "";
            txt_contact.Value = "";
            chk_addEQ.Checked = false;
        }
        
        /// <summary>
        /// Ejecuta la importación de imágenes al formulario de ingreso de datos para nuevos visitantes
        /// </summary>
        /// <param name="sender">Objeto que llama a la acción</param>
        /// <param name="e">Evento Ejecutado</param>
        public void ImportImages(object sender, EventArgs e)
        {
            try
            {
                Bitmap bmp_front = new Bitmap(@"C:\MRobotics\LobbyManager\Client\IMG-A.bmp");
                Bitmap bmp_back = new Bitmap(@"C:\MRobotics\LobbyManager\Client\IMG-A-back.bmp");
                ((HtmlImage)img_front).Src = @"data:image/bmp;base64," + ConvertImageToBase64(bmp_front);
                ((HtmlImage)img_back).Src = @"data:image/bmp;base64," + ConvertImageToBase64(bmp_back);
                bmp_back.Dispose();
                bmp_front.Dispose();
                String rawDoc = File.ReadAllText(@"C:\MRobotics\LobbyManager\Client\IMG-A.txt");
                String[] fields = rawDoc.Split(',');
                txt_name.Value = fields[14];
                txt_lastname.Value = fields[16];
                txt_docnumber.Value = fields[0];
                images.Visible = true;
                fs_personalData.Visible = true;
                fs_visitDetails.Visible = true;
                File.Delete(@"C:\MRobotics\LobbyManager\Client\IMG-A.bmp");
                File.Delete(@"C:\MRobotics\LobbyManager\Client\IMG-A-back.bmp");
                File.Delete(@"C:\MRobotics\LobbyManager\Client\IMG-A.txt");
            }
            catch (Exception a)
            {
                images.Visible = false;
                fs_personalData.Visible = false;
                fs_visitDetails.Visible = false;
                txt_name.Value = "No se ha cargado el documento";
                txt_lastname.Value = "";
                txt_docnumber.Value = "";
                Response.Write(a.Message);
                msgWarn.Visible = true;
            }
        }

        /// <summary>
        /// Convierte un mapa de bits a formato string base64
        /// </summary>
        /// <param name="img">Mapa de Bits a convertir</param>
        /// <returns>String base64</returns>
        private static string ConvertImageToBase64(Bitmap img)
        {
            string _code = "";

            if (img != null)
            {
                Bitmap im = new Bitmap(img, img.Width / 4, img.Height / 4);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                im.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] byteImage = ms.ToArray();
                _code = Convert.ToBase64String(byteImage); //Get Base64
            }

            return _code;
        }
    }
}