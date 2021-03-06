﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using CallCopy.Media.Audio;

namespace speexcmd
{
    //testing credentials master branch
    class Program
    {
        static void Main(string[] args)
        {
             //WaveWriter wsw = new WaveWriter(1,1,4000,1,1);
             //byte[] silence = new byte[16];
             //wsw.Open("C:\\out.wav");
             //wsw.WriteHeader("Encoded with: agmu " );
             //Speex speex = new Speex();
             //byte[] buffer = File.ReadAllBytes("gate10.decode.raw");
             //byte[] speexBuffer = speex.Encode(buffer);
             //wsw.WritePacket(speexBuffer, 0, speexBuffer.Length);
             //wsw.Close();
            if (args.Length >= 2)
            {
                bool help = false, fullTest = false;
                String inputFile, outputFile;
                int quality = 5;
                int channels = 1;
                int rate = -1;
                int bandMode = BandMode.Narrow;
                string bMString = "Narrow";
                bool isEncoding = true;
                for (int i = 0; i < args.Length; i++) // Loop through array
                {
                    string argument = args[i];
                    switch (argument)
                    {
                        case "-h":
                            help = true;
                            break;
                        case "-n":
                            bandMode = BandMode.Narrow;
                            bMString = "Narrow";
                            break;
                        case "-w":
                            bandMode = BandMode.Wide;
                            bMString = "Wide";
                            break;
                        case "-u":
                            bandMode = BandMode.UltraWide;
                            bMString = "UltraWide";
                            break;
                        case "-ft":
                            fullTest = true;
                            isEncoding = true;
                            break;
                        case "--q":
                            if (args.Length >= i + 1)
                            {
                                try
                                {
                                    quality = Int32.Parse(args[i + 1]);
                                }
                                catch (Exception e)
                                {
                                    Console.Write("Usage: quality parameter missing, 5 is By default\n");
                                    quality = 5;
                                }
                            }
                            break;
                        case "--ch":
                            if (args.Length >= i + 1)
                            {
                                try
                                {
                                    channels = Int32.Parse(args[i + 1]);
                                    if (channels > 2) { channels = 1; }
                                }
                                catch (Exception e)
                                {
                                    Console.Write("Usage: channels parameter missing, 1 channel is By default\n");
                                    channels = 1;
                                }
                            }
                            break;

                        case "--rate":
                            if (args.Length >= i + 1)
                            {
                                try
                                {
                                    rate = Int32.Parse(args[i + 1]);
                                }
                                catch (Exception e)
                                {
                                    Console.Write("Usage: channels parameter missing, 1 channel is By default\n");
                                    rate = -1;
                                }
                            }
                            break;
                    }
                }

                inputFile = args[args.Length - 2];
                outputFile = args[args.Length - 1];
                if (!fullTest) isEncoding = outputFile.ToLowerInvariant().EndsWith(".spx");

                string stString = "mono";
                if (channels == 2)
                    stString = "stereo";
                Console.Write("************************************** \n");
                Console.Write("*************{0}************** \n", isEncoding ? "ENCODING" : "DECODING");
                Console.Write("Input File: " + inputFile + " \n");
                Console.Write("Output File: " + outputFile + " \n");
                if (isEncoding)
                {
                    Console.Write("channels : " + stString + " \n");
                    Console.Write("Quality : " + quality + " \n");
                    Console.Write("Band Mode : " + bMString + " \n"); 
                }
                Console.Write("************************************** \n");
                if (help)
                {
                    EncodeHelp();
                }
                if(isEncoding)
                {
                    if (fullTest)
                    {
                        TestEncodeFromBuffer(inputFile, outputFile, quality, bandMode, channels);
                    }
                    else
                    {
                        if (rate > 0)
                        {
                            EncodeFromFile(inputFile, outputFile, quality, bandMode, channels, rate);
                        }
                        else
                        {
                            EncodeFromFile(inputFile, outputFile, quality, bandMode, channels);
                        }
                    }

                }
                else
                {
                    Decode(inputFile, outputFile);
                }
            }
            else
            {
                EncodeHelp();
            }
            Console.WriteLine("Finished!");
            Console.ReadKey();
        }

        private static void EncodeHelp()
        {
            Console.Write("Usage: speexcmd [options] input_file output_file\n");
            Console.Write("\n");
            Console.Write("FOR ENCODING\n");
            Console.Write("Encodes input_file using Speex. It can read raw files.\n");
            Console.Write("\n");
            Console.Write("input_file can  be: filename.raw      Raw PCM file\n");
            Console.Write("output_file can be: filename.spx      Speex file\n");
            Console.Write("Options:\n");
            Console.Write(" -n       Narrowband (8 kHz) input file\n");
            Console.Write(" -w       Wideband (16 kHz) input file\n");
            Console.Write(" -u      \"Ultra-wideband\" (32 kHz) input file\n");
            Console.Write(" --q n     Encoding quality (0-10)\n");
            Console.Write(" --ch n    File channels (1-2)\n\n");
            Console.Write(" --rate n  Sampling rate for raw input\n");

            Console.Write("FOR DECODING\n");
            Console.Write("Decodes input_file spx file to raw file.\n");
            Console.Write("\n");
            Console.Write("input_file can  be: filename.spx      Speex file\n");
            Console.Write("output_file can be: filename.raw      Raw PCM file\n");
            Console.Write("Options:\n");
            Console.Write(" --ch n    File channels (1-2)\n\n");

            Console.Write(" -h       Help\n");
            Console.Write("********************************\n");
            Console.Write(" -ft      Full Test : input_file is a spx file, and output return a new raw file and a new spx file \n");
        }

        private static void EncodeFromFile(String input, String output, int quality, int bandMode, int channels)
        {
            Speex speex = new Speex();
            bool response = speex.EncodeFromFile(input, output, quality, bandMode, channels);
            Console.WriteLine("Encoded {0}", response ? "Success" : "Failed");
        }
        private static void EncodeFromFile(String input, String output, int quality, int bandMode, int channels,int rate)
        {
            Speex speex = new Speex();
            bool response = speex.EncodeFromFile(input, output, quality, bandMode, channels, rate);
            Console.WriteLine("Encoded {0}", response ? "Success" : "Failed");
        }

        private static void EncodeFromBuffer(String output, int quality, int bandMode, byte[] buffer, int buferSize, int channels)
        {
            Speex speex = new Speex();
            bool response = speex.EncodeFromBuffer(output, quality, bandMode, channels, buffer, buferSize);
            Console.WriteLine("Encoded {0}", response ? "Success" : "Failed");
        }

        private static void TestEncodeFromBuffer(String input, String output, int quality, int bandMode, int channels)
        {
            byte[] tempBuffer = new byte[3];
            Speex speex = new Speex();
            bool response = speex.TestEncodeFromBuffer(input, output, quality, bandMode, channels);
            Console.WriteLine("Encoded {0}", response ? "Success" : "Failed");
        }

        private static void Decode(string spxFileName ,string rawFileName)
        {
            Speex speex = new Speex();
            bool isSuccess = speex.Decode(spxFileName, rawFileName);
            Console.WriteLine("Decoded {0}", isSuccess ? "Success" : "Failed");
        }
    }
}
