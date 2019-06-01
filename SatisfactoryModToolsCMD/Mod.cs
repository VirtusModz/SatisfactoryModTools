using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryModTools
{

    // Mod data, will be used in further versions, not at the moment..
    public class Mod
    {
        public Mod()
        {
            return;
        }
    }

    // Mod Manager, inherits from Mod data, will also be used in further versions, not at the moment..
    public class Manager : Mod
    {
        public Manager()
        {
            return;
        }
    }

    // Mod Packager
    public class Packager
    {
        public Packager()
        {
            // Maybe a custom packaging engine in the future?
            Engine = "Unreal";
        }

        public void Package(String mod)
        {
            // Unreal Data
            String DUnrealPath = @"C:\Program Files\Epic Games\UE_4.20";
            String PakListPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Unreal Engine\AutomationTool\Logs\C+Program+Files+Epic+Games+UE_4.20\PakList_FactoryGame-WindowsNoEditor.txt";

            // Mod Data
            String DGamePath = @"C:\Program Files\Epic Games\SatisfactoryEarlyAccess";
            String ModName = mod;
       
            // Package Data
            String ResponseFile = @"C:\temp\Response.txt";
            String ResponseParameters = null;

            // Display the fancy text OwO
            Console.Title = "Satisfactory Mod Tools - Packager";
            Console.Clear();
            Console.WriteLine("+-----------------------------------------------------------+\r");
            Console.WriteLine("| /!\\ You'll need Unreal Engine 4.20.x to run the Packager! |\r");
            Console.WriteLine("+-----------------------------------------------------------+\n");

            // Game path adding
            Console.WriteLine("Type in your Satisfactory game directory path (Blank for default value), and then press Enter");
            Console.WriteLine("Default value: " + DGamePath);
            String GamePath = Convert.ToString(Console.ReadLine());
            if (String.IsNullOrEmpty(GamePath))
            {
                GamePath = DGamePath;
            }

            // Engine path adding
            Console.WriteLine("Type in your UnrealEngine directory path (Blank for default value), and then press Enter");
            Console.WriteLine("Default value: " + DUnrealPath);
            String UnrealPath = Convert.ToString(Console.ReadLine());
            if (String.IsNullOrEmpty(UnrealPath))
            {
                UnrealPath = DUnrealPath;
            }

            String PackerPath = UnrealPath + @"\Engine\Binaries\Win64\UnrealPak.exe";

            // Response file creation
            try
            {
                // Check if file already exists. If yes, delete it.
                if (File.Exists(ResponseFile))
                {
                    File.Delete(ResponseFile);
                }

                // Create a new file
                using (FileStream fs = File.Create(ResponseFile))
                {

                    // Keep the required lines

                    String[] lines = File.ReadAllLines(PakListPath);
                    IEnumerable<string> selectLines = lines.Where(line => line.Contains(@"\FactoryGame\Saved\Cooked\WindowsNoEditor\FactoryGame\Content\"));
                    foreach (var item in selectLines)
                    {
                        ResponseParameters += item + Environment.NewLine;
                    }

                    Console.WriteLine(ResponseParameters);

                    // Add the required data in the file

                    Byte[] content = new UTF8Encoding(true).GetBytes(ResponseParameters);
                    fs.Write(content, 0, content.Length);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(" \"" + GamePath + @"\FactoryGame\Content\Paks\~mods"))
                {
                    // If there is already a directory, just create the mod inside
                    Console.WriteLine("Inserting Mod in ~mods directory");
                }

                // Try to create the directory, the pak patch file, the sig patch file.
                String path = GamePath + @"\FactoryGame\Content\Paks\~mods\";

                DirectoryInfo di = Directory.CreateDirectory(path);
                Console.WriteLine("~mods directory was successfully created!");

                // Packaging & SIG Creation

                Process.Start("\"" + PackerPath + "\"", " \"" + path + ModName + "_p.pak\" -Create=\"" + ResponseFile + "\"");
                File.Copy(Path.Combine(GamePath + @"\FactoryGame\Content\Paks", "FactoryGame-WindowsNoEditor.sig"), Path.Combine(GamePath + @"\FactoryGame\Content\Paks\~mods", ModName + "_p.sig"), true);

                Console.WriteLine("Mod packaged! Custom sig file created! the mod is under (" + path + ")");
                Console.ReadLine();
            }
            catch (Exception Ex)
            {
                Console.Write(Ex.ToString() + "\n");
            }
        }

        private String Engine { get; }
    }
}
