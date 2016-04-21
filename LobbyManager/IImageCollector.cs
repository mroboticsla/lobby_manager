using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LobbyManager
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IImageCollector" in both code and config file together.
    /// <summary>
    /// Clase que permite la recolección de datos de imagen a partir del servicio de windows instalado en cada estación de ingreso de visitantes.
    /// </summary>
    [ServiceContract]
    public interface IImageCollector
    {
        /// <summary>
        /// Guarda las imagenes recolectadas desde una estación de captura de datos.
        /// </summary>
        /// <param name="desk">ID de Escritorio</param>
        /// <param name="front">Imagen Frontal en String Base64</param>
        /// <param name="back">Imagen Posterior en String Base64</param>
        /// <param name="profile">Imagen Facial en String Base64</param>
        /// <param name="ocr">Archivo de extracción de datos.</param>
        [OperationContract]
        void SaveImages(String desk, String front, String back, String profile, String ocr);

        /// <summary>
        /// Monitorea la impresión de viñetas para ingreso de equipo.
        /// </summary>
        /// <param name="desk">ID de Escritorio</param>
        [OperationContract]
        String label(string desk);
    }
}
