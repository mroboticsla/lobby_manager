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
    /// Images controller Web Service
    /// </summary>
    [ServiceContract]
    public interface IImageCollector
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        [OperationContract]
        void DoWork();

        /// <summary>
        /// Save images on temp table as a base 64 String
        /// </summary>
        /// <param name="desk"></param>
        /// <param name="front"></param>
        /// <param name="back"></param>
        /// <param name="profile"></param>
        [OperationContract]
        void SaveImages(String desk, String front, String back, String profile, String ocr);
    }
}
