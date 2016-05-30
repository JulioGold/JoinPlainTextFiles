using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NDesk.Options;

namespace JoinPlainTextFiles
{
    class Program
    {
        static int verbosity;

        static void Main(string[] args)
        {
            bool show_help = false;
            string directory = String.Empty;
            bool subdirectories = false;
            string delimiter = String.Empty;
            string searchpattern = String.Empty;

            var p = new OptionSet()
            {
                { "d|dir=", "Directory to read files.", v => directory = v },
                { "s|sub=", "Search including subdirectories.", v => subdirectories = v != null },
                { "dl|delimiter=", "File delimiter for each file.", v => delimiter = v },
                { "sp|searchpattern=", "Search pattern.", v => searchpattern = v },
                { "h|help",  "Show this message and exit", v => show_help = v != null }
                // TODO: Adicionar uma forma de indicar uma ordem dos arquivos, e também se algum arquivo deve ser executado por primeiro...
                // TODO: Adicionar uma opção de delimitador padrão de arquivo
                // TODO: Adicionar uma opção para poder colocar quebra de linha no delimitador
                // TODO: Adicionar uma opção para colocar o cabeçalho de cada arquivo, que tenha o nome e a data...
            };

            List<string> extra;

            try
            {
                extra = p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("greet: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `JoinPlainTextFiles --help' for more information.");
                return;
            }

            if (!String.IsNullOrEmpty(directory))
            {
                StringBuilder uniao = new StringBuilder();
                //string[] arquivos = Directory.GetFiles(directory, String.IsNullOrEmpty(searchpattern) ? "*.*" : searchpattern, subdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                DirectoryInfo ai = new DirectoryInfo(directory);
                FileInfo[] arquivos = ai.GetFiles(String.IsNullOrEmpty(searchpattern) ? "*.*" : searchpattern, subdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

                //Array.Sort(arquivos);

                foreach (FileInfo arquivo in arquivos)
                {
                    using (StreamReader sr = new StreamReader(arquivo.FullName))
                    {
                        if (String.IsNullOrEmpty(delimiter))
                        {
                            uniao.Append(sr.ReadToEnd());
                            uniao.AppendLine();
                        }
                        else
                        {
                            uniao.Append(sr.ReadToEnd());
                            uniao.AppendLine();
                            uniao.AppendLine(delimiter);
                        }
                    }
                }
                Console.Write(uniao.ToString());
            }

            if (show_help)
            {
                ShowHelp(p);
                return;
            }
        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: JoinPlainTextFiles [OPTIONS]");
            Console.WriteLine("Join plain text files into a unique string..");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}
