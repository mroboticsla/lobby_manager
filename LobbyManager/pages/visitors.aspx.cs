using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace LobbyManager.pages
{
    /// <summary>
    /// Clase principal de la página de registro de visitantes.
    /// </summary>
    public partial class visitors : System.Web.UI.Page
    {
        static String mainConnectionString = "SykesVisitorsDB";

        /// <summary>
        /// Establece si se deberá presentar el mensaje de finalización del proceso.
        /// </summary>
        public bool showMg = false;

        /// <summary>
        /// Establece si el destino procede a la página para agregar equipo.
        /// </summary>
        public bool addEQ = false;

        /// <summary>
        /// Contiene el ID del visitante.
        /// </summary>
        public int visitor = 0;

        Bitmap bmp_front = null;
        Bitmap bmp_back = null;
        Bitmap bmp_profile = null;

        /// <summary>
        /// Se ejecuta el iniciar la carga.
        /// </summary>
        /// <param name="sender">Objecto que llama a la acción</param>
        /// <param name="e">Evento Ejecutado</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            msgWarn.Visible = false;
            images.Visible = false;
            fs_personalData.Visible = false;
            fs_visitDetails.Visible = false;
            
            if (Request.QueryString["finish"]!= null)
            {
                String finishFlag = Request.QueryString["finish"].ToString();
                if (finishFlag.Equals("true"))
                {
                    showMg = true;
                }
            }
        }

        /// <summary>
        /// Inserta un nuevo registro de visitante.
        /// </summary>
        /// <param name="sender">Objeto que llama a la acción</param>
        /// <param name="e">Evento Ejecutado</param>
        public void InsertVisitor(object sender, EventArgs e)
        {
            try
            {
                msgWarn.Visible = false;
                int vis_id = -1;
                string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT isnull(MAX(vis_id), 0) + 1 AS com_total FROM [tbl_vis_visitors]";
                    SqlDataReader dreader = cmd.ExecuteReader();
                    if (dreader.Read())
                    {
                        vis_id = int.Parse(dreader["com_total"].ToString());
                    }
                    dreader.Close();
                    conn.Close();
                }
                
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "INSERT INTO [tbl_vis_visitors] (vis_id, vis_name, vis_lastname, vis_doctype, vis_docnumber, vis_company, vis_phone, vis_reason, vis_description, vis_department, vis_internal_contact, vis_date, vis_status, vis_image_record, vis_with_equipment) \n" +
                                      "values (@vis_id, @vis_name, @vis_lastname, @vis_doctype, @vis_docnumber, @vis_company, @vis_phone, @vis_reason, @vis_description, @vis_department, @vis_internal_contact, GETDATE(), @vis_status, @vis_image_record, @vis_with_equipment)";
                    cmd.Parameters.AddWithValue("vis_id", vis_id);
                    cmd.Parameters.AddWithValue("vis_name", txt_name.Value);
                    cmd.Parameters.AddWithValue("vis_lastname", txt_lastname.Value);
                    cmd.Parameters.AddWithValue("vis_doctype", docTypeSelect.SelectedValue);
                    cmd.Parameters.AddWithValue("vis_docnumber", txt_docnumber.Value);
                    cmd.Parameters.AddWithValue("vis_company", txt_company.Value);
                    cmd.Parameters.AddWithValue("vis_phone", txt_phone.Value);
                    cmd.Parameters.AddWithValue("vis_reason", reasonSelect.SelectedValue);
                    cmd.Parameters.AddWithValue("vis_description", txt_description.Value);
                    cmd.Parameters.AddWithValue("vis_department", deptSelect.SelectedValue);
                    cmd.Parameters.AddWithValue("vis_internal_contact", txt_contact.Text);
                    cmd.Parameters.AddWithValue("vis_status", 1);
                    cmd.Parameters.AddWithValue("vis_image_record", 0);
                    cmd.Parameters.AddWithValue("vis_with_equipment", (chk_addEQ.Checked) ? 1 : 0);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    //CleanForm();
                }

                int log_id = -1;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT isnull(MAX(log_id), 0) + 1 AS com_total FROM [tbl_log_events]";
                    SqlDataReader dreader = cmd.ExecuteReader();
                    if (dreader.Read())
                    {
                        log_id = int.Parse(dreader["com_total"].ToString());
                    }
                    dreader.Close();
                    conn.Close();
                }

                int vrs_notification_type = -1;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT vrs_notification_type FROM [tbl_vrs_visit_reasons] WHERE vrs_id = @vrs_id";
                    cmd.Parameters.AddWithValue("vrs_id", reasonSelect.SelectedValue);
                    SqlDataReader dreader = cmd.ExecuteReader();
                    if (dreader.Read())
                    {
                        vrs_notification_type = int.Parse(dreader["vrs_notification_type"].ToString());
                    }
                    dreader.Close();
                    conn.Close();
                }

                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "INSERT INTO [tbl_log_events] (log_id, log_notification, log_user, log_date, log_visitor_record) \n" +
                                      "values (@log_id, @log_notification, @log_user, GETDATE(), @log_visitor_record)";
                    cmd.Parameters.AddWithValue("log_id", log_id);
                    cmd.Parameters.AddWithValue("log_notification", vrs_notification_type);
                    cmd.Parameters.AddWithValue("log_user", Session["usr_device"].ToString());
                    cmd.Parameters.AddWithValue("log_visitor_record", vis_id);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                int img_id = -1;
                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "SELECT isnull(MAX(img_id), 0) + 1 AS com_total FROM [tbl_img_images]";
                    SqlDataReader dreader = cmd.ExecuteReader();
                    if (dreader.Read())
                    {
                        img_id = int.Parse(dreader["com_total"].ToString());
                    }
                    dreader.Close();
                    conn.Close();
                }

                using (var conn = new SqlConnection(connStr))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "INSERT INTO tbl_img_images (img_id, img_visitor, img_front, img_back, img_profile) \n" +
                                      "values (@img_id, @img_visitor, @img_front, @img_back, @img_profile)";
                    cmd.Parameters.AddWithValue("img_id", img_id);
                    cmd.Parameters.AddWithValue("img_visitor", vis_id);
                    cmd.Parameters.AddWithValue("img_front", txt_imgFront.Value);
                    cmd.Parameters.AddWithValue("img_back", txt_imgBack.Value);
                    cmd.Parameters.AddWithValue("img_profile", txt_imgProfile.Value);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    if (chk_addEQ.Checked)
                    {
                        visitor = vis_id;
                        addEQ = true;
                    }
                    else
                    {
                        showMg = true;
                    }

                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "showMsg", "showMsg();", true);
                    //ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), "showMsg", "showMsg();", true);
                }
            }
            catch (Exception a)
            {
                Response.Write(a.Message);
                msgWarn.Visible = true;
            }
            finally
            {
                CleanForm();
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
            txt_contact.Text = "";
            chk_addEQ.Checked = false;
            fs_images.Visible = true;
            fs_personalData.Visible = false;
            fs_visitDetails.Visible = false;
        }

        /// <summary>
        /// Ejecuta el ingreso de datos sin validar el escaneo de documentos.
        /// </summary>
        /// <param name="sender">Objeto que llama la acción</param>
        /// <param name="e">Evento Ejecutado</param>
        public void NoDocumentDemo(object sender, EventArgs e)
        {
            images.Visible = true;
            fs_images.Visible = false;
            fs_personalData.Visible = true;
            fs_visitDetails.Visible = true;
        }

        /// <summary>
        /// Ejecuta la importación de imágenes al formulario de ingreso de datos para nuevos visitantes
        /// </summary>
        /// <param name="sender">Objeto que llama a la acción</param>
        /// <param name="e">Evento Ejecutado</param>
        public void ImportImages(object sender, EventArgs e)
        {
            Boolean loaded = false;
            string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
            try
            {
                int counter = 0;
                while (counter < 5)
                {
                    using (var conn = new SqlConnection(connStr))
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "SELECT * FROM tbl_temp_images WHERE temp_desk = @temp_desk";
                        cmd.Parameters.AddWithValue("temp_desk", Session["usr_device"].ToString());
                        SqlDataReader dreader = cmd.ExecuteReader();
                        if (dreader.Read())
                        {
                            String front = dreader["temp_front"].ToString();
                            String back = dreader["temp_back"].ToString();
                            String profile = dreader["temp_profile"].ToString();
                            String rawDoc = dreader["temp_ocr"].ToString();

                            txt_imgFront.Value = front;
                            txt_imgBack.Value = back;
                            txt_imgProfile.Value = profile;

                            ((HtmlImage)img_front).Src = @"data:image/bmp;base64," + front;
                            ((HtmlImage)img_back).Src = @"data:image/bmp;base64," + back;

                            String[] fields = rawDoc.Split(',');
                            txt_name.Value = fields[14];
                            txt_lastname.Value = fields[16];
                            txt_docnumber.Value = fields[0];
                            images.Visible = true;
                            fs_images.Visible = false;
                            fs_personalData.Visible = true;
                            fs_visitDetails.Visible = true;

                            loaded = true;
                        }
                        dreader.Close();
                        conn.Close();
                        if (loaded) break;
                        counter++;
                        Thread.Sleep(1500);
                    }
                }
            }
            catch (Exception a)
            {
                Response.Write(a.Message);
            }
            finally
            {
                if (!loaded)
                {
                    images.Visible = false;
                    fs_personalData.Visible = false;
                    fs_visitDetails.Visible = false;
                    fs_images.Visible = true;
                    txt_name.Value = "No se ha cargado el documento";
                    txt_lastname.Value = "";
                    txt_docnumber.Value = "";
                    msgWarn.Visible = true;
                }
                else
                {
                    using (var conn = new SqlConnection(connStr))
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "DELETE FROM tbl_temp_images WHERE temp_desk = @temp_desk";
                        cmd.Parameters.AddWithValue("temp_desk", Session["usr_device"].ToString());
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Convierte un mapa de bits a formato string base64
        /// </summary>
        /// <param name="img">Mapa de Bits a convertir</param>
        /// <returns>String base64</returns>
        private static string ConvertImageToBase64(Bitmap img, int compresion)
        {
            string _code = "";

            if (img != null)
            {
                Bitmap im = new Bitmap(img, img.Width / compresion, img.Height / compresion);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                im.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] byteImage = ms.ToArray();
                _code = Convert.ToBase64String(byteImage); //Get Base64
            }

            return _code;
        }

        /// <summary>
        /// Web Service para listar Empleados en las pantallas de registro de visitantes.
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [WebMethod]
        public static string[] GetEmployees(string keyword)
        {
            List<string> employee = new List<string>();
            string query = string.Format("SELECT emp_name, emp_lastname FROM tbl_emp_employees WHERE emp_name LIKE '%{0}%' OR emp_lastname LIKE '%{0}%'", keyword);
            
            if (keyword.Split(' ').Length > 1)
            {
                query = string.Format("SELECT emp_name, emp_lastname FROM tbl_emp_employees WHERE (emp_name + ' ' + emp_lastname) LIKE '%{0}%{1}%' OR (emp_name LIKE '%{0}%' AND emp_lastname LIKE '%{1}%')", keyword.Split(' '));
            }
            
            string connStr = ConfigurationManager.ConnectionStrings[mainConnectionString].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = query;
                SqlDataReader dreader = cmd.ExecuteReader();
                while (dreader.Read())
                {
                    employee.Add(dreader.GetString(0).ToString().Trim() + " " + dreader.GetString(1).ToString().Trim());
                }
                dreader.Close();
                conn.Close();
            }
            return employee.ToArray();
        }
    }
}