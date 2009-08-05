/**************************
 * Created by:            *
 *      Vando Pereira     *
 * Date Created:          *
 *      04-08-2009        *
 * Last Revision:         *
 *      05-08-2009        *
 *************************/

using System.Collections.Generic;
using System.IO;

namespace XNAPhysXTools
{
    public class PxMeshEncoder
    {
        private string fileName;
        private BinaryWriter binWriter;

        private List<Stream> streams = new List<Stream>();


        public PxMeshEncoder(string absoluteFileName)
        {
            fileName = absoluteFileName;
        }

        public void AddStream(Stream stream)
        {
            streams.Add(stream);
        }

        /// <summary>
        /// Save the model to the hard drive, with previous specified name
        /// </summary>
        /// <returns>False if some errors occured</returns>
        public bool Save()
        {
            binWriter = new BinaryWriter(File.Open(fileName, FileMode.Create));
            binWriter.Write(streams.Count);
            foreach (var stream in streams)
            {
                binWriter.Write(stream.Length);

                byte[] streamBytes = new byte[stream.Length];

                stream.Position = 0;
                stream.Read(streamBytes, 0, (int)stream.Length);
                binWriter.Write(streamBytes);
            }
            binWriter.Flush();
            binWriter.Close();

            return true;
        }
    }
}