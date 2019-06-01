using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SatisfactoryModTools
{
    class Program
    {

        public void GetTitle()
        {
            Console.Clear();
            Console.WriteLine("+----------------------------------------+\r");
            Console.WriteLine("| Satisfactory Mod Tools for Windows x64 |\r");
            Console.WriteLine("+----------------------------------------+\n");
        }

        static void Main(string[] args)
        {
            int exit = 0;
            while (exit == 0)
            {

                // Display the fancy text OwO
                Console.Title = "Satisfactory Mod Tools - Main menu";
                Program prgm = new Program();
                prgm.GetTitle();

                // Ask the user to choose an option.
                Console.WriteLine("Choose an option from the following list:\n");
                Console.WriteLine("  1) - Mod Packager");
                Console.WriteLine("  2) - Mod Manager (Soon...)");
                Console.WriteLine("\n  e) - Exit the program");
                Console.Write("\n~Tools>");

                // Mod object creation
                Packager packager = new Packager();

                // Use a switch statement to do the choice.
                switch (Console.ReadLine())
                {
                    case "1":
                        prgm.GetTitle();
                        Console.WriteLine("Type in the name of your Mod, and then press Enter");
                        Console.Write("~Packager>");
                        String ModName = Console.ReadLine();
                        packager.Package(ModName);
                        exit = 1;
                        break;
                    case "2":
                        break;
                    case "e":
                        exit = 1;
                        break;
                    default:
                        Console.WriteLine("Unknown command!");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
