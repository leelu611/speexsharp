﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace speexcmd
{
    //testing credentials master branch
    class Program
    {
        static void Main(string[] args)
        {
            Encode();
            Decode();

            Console.WriteLine("Finished!");
            Console.ReadKey();
        }

        private static void Encode()
        {
            Speex speex = new Speex(8);
            bool response = speex.Encode("gate10.decode.raw", "agmu1.spx");
        }

        private static void Decode()
        {
            Speex speex = new Speex(1);
            bool response = speex.Decode("agmu1.spx", "amug1.raw");
            Console.WriteLine("Decoded {0}", response ? "Success" : "Failed");
            
            //byte[] buffer = File.ReadAllBytes("speex.sample1.encoded.spx");
            //byte[] encodedBuffer = speex.Decode(buffer);

            //using (var fileStream = new FileStream("speex.sample1.decoded.raw", FileMode.Create, FileAccess.Write))
            //{
            //    fileStream.Write(encodedBuffer, 0, encodedBuffer.Length);
            //}
        }
    }
}
