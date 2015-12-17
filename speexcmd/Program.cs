﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using CallCopy.Media.Audio;
using CommandLine;

namespace speexcmd
{
    //testing credentials master branch
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options) == false)
            {
                Console.WriteLine("Arguments do not parse correctly");
            }

            if (args.Length >= 2)
            {
                bool help = false, fullTest = false;
                String inputFile, outputFile;
                int quality = 5;
                int bandMode = BandMode.Narrow;

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
                            break;
                        case "-w":
                            bandMode = BandMode.Wide;
                            break;
                        case "-u":
                            bandMode = BandMode.UltraWide;
                            break;
                        case "-ft":
                            fullTest = true;
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
                    }
                }

                inputFile = args[args.Length - 2];
                outputFile = args[args.Length - 1];

                Console.Write("Input File: " + inputFile + " \n");
                Console.Write("Output File: " + outputFile + " \n");
                if (help)
                {
                    EncodeHelp();
                }
                else
                {
                    if (fullTest)
                    {
                        TestEncodeFromBuffer(inputFile, outputFile, quality, bandMode);
                    }
                    else
                    {
                        EncodeFromFile(inputFile, outputFile, quality, bandMode);
                    }

                }
                //Decode();
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
            Console.Write("Encodes input_file using Speex. It can read raw files.\n");
            Console.Write("\n");
            Console.Write("input_file can be:\n");
            Console.Write("  filename.raw        Raw PCM file\n");
            Console.Write("output_file can be:\n");
            Console.Write("  filename.spx      Speex file\n");
            Console.Write("Options:\n");
            Console.Write(" -n       Narrowband (8 kHz) input file\n");
            Console.Write(" -w       Wideband (16 kHz) input file\n");
            Console.Write(" -u      \"Ultra-wideband\" (32 kHz) input file\n");
            Console.Write(" --q n    Encoding quality (0-10)\n");
            Console.Write(" -h       Help\n");
            Console.Write("********************************\n");
            Console.Write(" -ft      Full Test : input_file is a spx file, and output return a new raw file and a new spx file \n");
        }
        private static void Encode()
        {
            Speex speex = new Speex();
            bool response = speex.Encode("gate10.decode.raw", 10, "agmu1.spx");
            Console.WriteLine("Encoded {0}", response ? "Success" : "Failed");
        }

        private static void EncodeFromFile(String input, String output, int quality, int bandMode)
        {
            Speex speex = new Speex();
            bool response = speex.EncodeFromFile(input, output, quality, bandMode, 1);
            Console.WriteLine("Encoded {0}", response ? "Success" : "Failed");
        }

        private static void EncodeFromBuffer(String output, int quality, int bandMode, byte[] buffer, int buferSize)
        {
            Speex speex = new Speex();
            bool response = speex.EncodeFromBuffer(output, quality, bandMode, 1, buffer, buferSize);
            Console.WriteLine("Encoded {0}", response ? "Success" : "Failed");
        }

        private static void TestEncodeFromBuffer(String input, String output, int quality, int bandMode)
        {
            byte[] tempBuffer = new byte[3];
            Speex speex = new Speex();
            bool response = speex.TestEncodeFromBuffer(input, output, quality, bandMode, 1);
            Console.WriteLine("Encoded {0}", response ? "Success" : "Failed");
        }

        private static void Decode()
        {
            Speex speex = new Speex();
            bool isSuccess = speex.Decode("agmu1.spx", "amug1.raw");
            Console.WriteLine("Decoded {0}", isSuccess ? "Success" : "Failed");
        }
    }


    class Options
    {
        [Option('q', "quality", Required = false, HelpText = "The quality that goes from 1 to 10", DefaultValue = 1)]
        public int Quality { get; set; }

        //[ParserState]
        //public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return "XXX.";
        }
    }
}
