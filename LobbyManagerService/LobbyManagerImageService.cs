using LobbyManagerService.LobbyManagerWS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using bpac;

namespace LobbyManagerService
{
    public partial class LobbyManagerImageService : ServiceBase
    {
        private const string TEMPLATE_DIRECTORY = @"C:\Program Files\Brother bPAC3 SDK\Templates\";	// Template file path
        private const string TEMPLATE_SIMPLE = "BcdItem.lbx";	// Template file name
        private const string TEMPLATE_FRAME = "NamePlate2.LBX";		// Template file name
        
        private StringBuilder m_Sb;
        private bool m_bDirty;
        private System.IO.FileSystemWatcher m_Watcher;
        private string path = @File.ReadAllText(Path.Combine(Application.StartupPath, "path.txt"));

        private System.Timers.Timer timer;

        public LobbyManagerImageService()
        {
            InitializeComponent();
            m_Sb = new StringBuilder();
            m_bDirty = false;
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                LobbyManagerWS.ImageCollectorClient imgCollectorClient = new ImageCollectorClient();
                String desk = File.ReadAllText(Path.Combine(Application.StartupPath, "desk.txt"));
                String labelService = imgCollectorClient.label(desk);

                if (labelService != null)
                {
                    File.AppendAllText(Path.Combine(path, @"log\errors.txt"), Environment.NewLine + labelService);
                    String[] labelRaw = labelService.Split('|');
                    string templatePath = TEMPLATE_DIRECTORY;
                    templatePath += TEMPLATE_SIMPLE;

                    bpac.DocumentClass doc = new DocumentClass();
                    if (doc.Open(templatePath) != false)
                    {
                        String name = labelRaw[0];
                        doc.GetObject("objName").Text = name;
                        doc.GetObject("objSerial").Text = labelRaw[1];
                        doc.GetObject("objBarcode").Text = labelRaw[2];
                        doc.GetObject("objDesc").Text = labelRaw[3];
                        doc.GetObject("objOwner").Text = labelRaw[4];
                        // doc.SetMediaById(doc.Printer.GetMediaId(), true);
                        doc.StartPrint("", PrintOptionConstants.bpoDefault);
                        doc.PrintOut(1, PrintOptionConstants.bpoDefault);
                        doc.EndPrint();
                    }
                    else
                    {
                        File.AppendAllText(Path.Combine(path, @"log\errors.txt"), Environment.NewLine + "ERROR: DocOpen");
                    }
                }
                else
                {
                    //File.AppendAllText(Path.Combine(path, @"log\errors.txt"), Environment.NewLine + "ERROR: NULL");
                }
            }
            catch (Exception ex)
            {
                //Do exception
                File.AppendAllText(Path.Combine(path, @"log\errors.txt"), Environment.NewLine + "ERROR: " + ex.ToString());
            }
        }

        protected override void OnStart(string[] args)
        {
            m_Watcher = new System.IO.FileSystemWatcher();
            m_Watcher.Filter = "*.*";
            m_Watcher.Path = path + "\\";
            m_Watcher.IncludeSubdirectories = false;

            m_Watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                     | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            m_Watcher.Changed += new FileSystemEventHandler(OnChanged);
            m_Watcher.Created += new FileSystemEventHandler(OnChanged);
            //m_Watcher.Deleted += new FileSystemEventHandler(OnChanged);
            m_Watcher.EnableRaisingEvents = true;

            this.timer = new System.Timers.Timer(3000D);  // 30000 milliseconds = 30 seconds
            this.timer.AutoReset = true;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
            this.timer.Start();
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (!m_bDirty)
            {
                try
                {
                    m_Sb.Remove(0, m_Sb.Length);
                    m_Sb.Append(e.FullPath);
                    m_Sb.Append(" ");
                    m_Sb.Append(e.ChangeType.ToString());
                    m_Sb.Append("    ");
                    m_Sb.Append(DateTime.Now.ToString());

                    Thread.Sleep(2000);

                    //File.AppendAllText(Path.Combine(path, @"log\log.txt"), Environment.NewLine + "File: " + m_Sb.ToString());
                    LobbyManagerWS.ImageCollectorClient imgCollectorClient = new ImageCollectorClient();

                    Bitmap img_front = new Bitmap(Path.Combine(path, "IMG-A.bmp"));
                    Bitmap img_back = new Bitmap(Path.Combine(path, "IMG-A-back.bmp"));
                    Bitmap img_profile = new Bitmap(Path.Combine(path, "IMG-A-Face.bmp"));
                    
                    String front = ConvertImageToBase64(img_front, 5);
                    String back = ConvertImageToBase64(img_back, 5);
                    String profile = ConvertImageToBase64(img_profile, 1);
                    String ocr = File.ReadAllText(Path.Combine(path, "IMG-A.txt"));
                    String desk = File.ReadAllText(Path.Combine(Application.StartupPath, "desk.txt"));

                    img_front.Dispose();
                    img_back.Dispose();
                    img_profile.Dispose();

                    File.Delete(Path.Combine(path, "IMG-A.bmp"));
                    File.Delete(Path.Combine(path, "IMG-A-back.bmp"));
                    File.Delete(Path.Combine(path, "IMG-A-Face.bmp"));
                    File.Delete(Path.Combine(path, "IMG-A.txt"));

                    imgCollectorClient.SaveImages(desk, front, back, profile, ocr);
                    //File.AppendAllText(Path.Combine(path, @"log\log.txt"), Environment.NewLine + "Finish uploading!");
                    
                    //m_bDirty = true;
                }
                catch (IOException ex)
                {
                    //File.AppendAllText(Path.Combine(path, @"log\errors.txt"), Environment.NewLine + "ERROR: " + ex.ToString());
                }
                catch (ArgumentException ex)
                {
                    //File.AppendAllText(Path.Combine(path, @"log\errors.txt"), Environment.NewLine + "ERROR: " + ex.ToString());
                }
                catch (Exception ex)
                {
                    File.AppendAllText(Path.Combine(path, @"log\errors.txt"), Environment.NewLine + "ERROR: " + ex.ToString());
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

        protected override void OnStop()
        {
            m_Watcher.EnableRaisingEvents = false;
            m_Watcher.Dispose();

            this.timer.Stop();
            this.timer = null;
        }
    }
}
