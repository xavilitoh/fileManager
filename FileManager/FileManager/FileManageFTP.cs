using FileManager.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace FileManager
{
    public class FileManageFTP : IFileManageFTP
    {
        private readonly IConfiguration configuration;

        public string hostFtp { get; set; }
        public string userFtp { get; set; }
        public string passFtp { get; set; }

        /// <summary>
        /// Este metodo construye el nombre de los achivos para subirlos al server 
        /// </summary>
        /// <param name="identificador">identificador de la clase a la que pertenece el archivo.</param>
        /// <param name="app">aplicacion a la que pertenece el archivo.</param>
        /// <param name="subName">el nombre del registro al que pertenece la foto la foto.</param>
        /// <param name="file">el archivo que se desea subir.</param>
        /// <param name="index">secuancia de archivo.</param>
        public string BuildFileName(string identificador, string app, string subName, int index, IFormFile file)
        {
            if (subName.Length > 20)
            {
                subName = subName.Substring(0, 20);
            }

            return $"{identificador}_{app}_{subName.Replace(" ", "").Replace(":", string.Empty).Replace("?", "").Replace("¿", string.Empty).Replace("!", string.Empty).Replace("~", string.Empty).Replace("*", string.Empty).Replace("/", string.Empty).Replace("`", string.Empty)}-{index}{Path.GetExtension(file.FileName)}";
        }



        /// <summary>
        /// Este metodo sube un file temporalmente al servidor para luego subirla por FTP al server de archivos youmalbum y al final borrar el file del servidor de la aplicacion
        /// </summary>
        public string UploadFile(string identificador, string app, string subName, int index, IFormFile file)
        {
            var fileName = BuildFileName(identificador, app, subName, index, file);

            fileName = UploadFile(file, fileName);

            string absoluteFileName = Path.GetFileName(fileName);

            WebRequest request = (FtpWebRequest)WebRequest.Create($"{hostFtp}/wwwroot/{absoluteFileName}");
            request.Method = WebRequestMethods.Ftp.UploadFile;

            request.Credentials = new NetworkCredential(userFtp, passFtp);

            using (FileStream fs = File.OpenRead(fileName))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(buffer, 0, buffer.Length);
                requestStream.Flush();
                requestStream.Close();
            }

            System.IO.File.Delete(fileName);

            return absoluteFileName;
        }

        public string DeleteFile(string fileName)
        {
            WebRequest request = (FtpWebRequest)WebRequest.Create($"{hostFtp}/wwwroot/{fileName}");
            request.Method = WebRequestMethods.Ftp.DeleteFile;

            request.Credentials = new NetworkCredential(userFtp, passFtp);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                return response.StatusDescription;
            }
        }

        /// <summary>
        /// Este metodo sube un archivo al servidor
        /// </summary>
        public string UploadFile(IFormFile file, string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/files/ProductsPictures/");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (file != null)
            {
                var myFile = Path.Combine(path, fileName);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (var filestream = new FileStream(myFile, FileMode.Create))
                {
                    file.CopyTo(filestream);
                }
                return myFile;
            }
            else
            {
                return Path.Combine(path, "noImage.png");
            }
        }

        public byte[] GetBytesFromUrl(string url)
        {
            byte[] b;
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            WebResponse myResp = myReq.GetResponse();

            Stream stream = myResp.GetResponseStream();
            //int i;
            using (BinaryReader br = new BinaryReader(stream))
            {
                //i = (int)(stream.Length);
                b = br.ReadBytes(500000);
                br.Close();
            }
            myResp.Close();
            return b;
        }

        public string WriteBytesToFile(string fileName, byte[] content, string path)
        {
            var myFile = Path.Combine(path, fileName);

            FileStream fs = new FileStream(myFile, FileMode.Create);
            BinaryWriter w = new BinaryWriter(fs);

            try
            {
                w.Write(content);
            }
            finally
            {
                fs.Close();
                w.Close();
            }

            return myFile;

        }
    }
}
