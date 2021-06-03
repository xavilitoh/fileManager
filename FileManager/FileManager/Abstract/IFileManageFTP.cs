

namespace FileManager.Abstract
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.AspNetCore.Http;

    public interface IFileManageFTP
    {
        string hostFtp { get; set; }
        string userFtp { get; set; }
        string passFtp { get; set; }

        /// <summary>
        /// Este metodo sube un file temporalmente al servidor para luego subirla por FTP al server de archivos youmalbum y al final borrar el file del servidor de la aplicacion
        /// </summary>
        /// <param name="identificador">identificador de la clase a la que pertenece el archivo.</param>
        /// <param name="app">aplicacion a la que pertenece el archivo.</param>
        /// <param name="subName">el nombre del registro al que pertenece la foto la foto.</param>
        /// <param name="file">el archivo que se desea subir en formato IFormFile.</param>
        /// <param name="index">secuancia de archivo.</param>
        string UploadFile(string identificador, string app, string subName, int index, IFormFile file);

        /// <summary>
        /// Este metodo sube un file temporalmente al servidor para luego subirla por FTP al server de archivos youmalbum y al final borrar el file del servidor de la aplicacion
        /// </summary>
         /// <param name="identificador">identificador de la clase a la que pertenece el archivo.</param>
        /// <param name="app">aplicacion a la que pertenece el archivo.</param>
        /// <param name="subName">el nombre del registro al que pertenece la foto la foto.</param>
        /// <param name="file">el archivo que se desea subir en formato byte[].</param>
        /// <param name="index">secuancia de archivo.</param>
        string UploadFile(string fileName, byte[] file);

        /// <summary>
        /// obtiene un arreglo de bytes de un archivo atravez de una url
        /// </summary>
        byte[] GetBytesFromUrl(string url);

        /// <summary>
        /// Escribe bytes en un archivo
        /// </summary>
        string WriteBytesToFile(string fileName, byte[] content, string path);

        /// <summary>
        /// Este metodo sube un archivo al servidor
        /// </summary>
        string UploadFile(IFormFile file, string subName);

        /// <summary>
        /// Este metodo sube un archivo al servidor
        /// </summary>
        string UploadFile(byte[] file, string fileName);

        /// <summary>
        /// Devuelve el nombre del archivo a subir conformado por un identificador, nombre de la app, el subName (nombre del registro perteneciente) y el file que se desea subir
        /// </summary>
        /// <param name="identificador">identificador de la clase a la que pertenece el archivo.</param>
        /// <param name="app">aplicacion a la que pertenece el archivo.</param>
        /// <param name="subName">el nombre del registro al que pertenece la foto la foto.</param>
        /// <param name="file">Archivo que se desea subir.</param>
        /// <param name="index">secuancia de archivo.</param>
        public string BuildFileName(string identificador, string app, string subName, int index, IFormFile file);

        /// <summary>
        /// Elimina un archivo del server FTP
        /// </summary>
        /// <param name="fileName">Nombre del archivo que se eliminara.</param>
        public string DeleteFile(string fileName);
    }
}
