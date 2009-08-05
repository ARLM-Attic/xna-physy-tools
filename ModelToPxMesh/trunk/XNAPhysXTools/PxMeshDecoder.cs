/**************************
 * Created by:            *
 *      Vando Pereira     *
 * Date Created:          *
 *      04-08-2009        *
 * Last Revision:         *
 *      05-08-2009        *
 *************************/
using System;
using System.Collections.Generic;
using System.IO;

namespace XNAPhysXTools
{
    public class PxMeshDecoder
    {
        public static List<string> Load(string absoluteFileName)
        {
            var streams = new List<string>();

            var binReader = new BinaryReader(File.Open(absoluteFileName, FileMode.Open));


            //byte[] testArray = new byte[3];
            //int count = binReader.Read(testArray, 0, 3);

            int count = 1;

            if (count != 0)
            {
                int numOfStreams = binReader.ReadInt32();
                for (int i = 0; i < numOfStreams; i++)
                {
                    var size = binReader.ReadUInt32();
                    binReader.ReadInt32();
                    //var stream = new MemoryStream((int)size);
                    string name = Guid.NewGuid().ToString();
                    var s =File.OpenWrite(name);
                    byte[] byteStream = new byte[size];
                    byteStream = binReader.ReadBytes((int)size);

                    s.Write(byteStream,0,(int)size);
                    //stream.Flush();
                    s.Close();
                    //stream.Close();
                    streams.Add(name);
                }
            }

            return streams;
        }



    }
}
