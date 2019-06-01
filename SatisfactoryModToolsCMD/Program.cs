using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SatisfactoryModToolsCMD
{
    class Program
    {
        public void title()
        {
            Console.WriteLine("+----------------------------------------+\r");
            Console.WriteLine("| Satisfactory Mod Tools for Windows x64 |\r");
            Console.WriteLine("+----------------------------------------+\n");

            Console.WriteLine("+--------------------------------------------------------------------+\r");
            Console.WriteLine("| /!\\ You need Unreal Engine 4.20.3 to make this tool work properly! |\r");
            Console.WriteLine("+--------------------------------------------------------------------+\n\n");
        }

        static void Main(string[] args)
        {

            Program prgm = new Program();

            // Unreal Data //
            String DUnrealPath = @"C:\Program Files\Epic Games\UE_4.20";
            String UnrealPath = null;
            String PakListPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Unreal Engine\AutomationTool\Logs\C+Program+Files+Epic+Games+UE_4.20\PakList_FactoryGame-WindowsNoEditor.txt";

            // Mod Data //
            String DGamePath = @"C:\Program Files\Epic Games\SatisfactoryEarlyAccess";
            String GamePath = null;
            String ModName = null;

            // Package Data //
            String ResponseFile = @"C:\temp\Response.txt";
            String ResponseParameters = null;

            prgm.title();

            // Game path processing //
            Console.WriteLine("Type in your Satisfactory game directory path (Blank for default value), and then press Enter");
            Console.WriteLine("Default value: " + DGamePath);
            GamePath = Convert.ToString(Console.ReadLine());
            if(String.IsNullOrEmpty(GamePath))
            {
                GamePath = DGamePath;
            }

            // Engine path processing //
            Console.WriteLine("Type in your UnrealEngine directory path (Blank for default value), and then press Enter");
            Console.WriteLine("Default value: " + DUnrealPath);
            UnrealPath = Convert.ToString(Console.ReadLine());
            if (String.IsNullOrEmpty(UnrealPath))
            {
                UnrealPath = DUnrealPath;
            }

            String PackerPath = UnrealPath + @"\Engine\Binaries\Win64\UnrealPak.exe";

            Console.WriteLine("Type in the name of your Mod, and then press Enter");
            ModName = Convert.ToString(Console.ReadLine());

            Console.Clear();
            prgm.title();

            // Response File Section //
            String fileName = @"C:\temp\Response.txt";

            try
            {
                // Check if file already exists. If yes, delete it. //
                if (File.Exists(fileName))
                {
                    Console.WriteLine("Response File already exist, removing the old one...");
                    File.Delete(fileName);
                }

                // Create a new file //
                using (FileStream fs = File.Create(fileName))
                {
                    Console.WriteLine("Creating Response File...");
                    Console.WriteLine("Reading PakList File...");
                    Console.WriteLine("Parsing PakList File...");

                    // Parse it //

                    String[] lines = File.ReadAllLines(PakListPath);
                    IEnumerable<string> selectLines = lines.Where(line => line.Contains(@"\FactoryGame\Saved\Cooked\WindowsNoEditor\FactoryGame\Content\"));
                    foreach (var item in selectLines)
                    {
                        ResponseParameters += item + Environment.NewLine;
                    }

                    Console.WriteLine("Parsed with success!");
                    Console.WriteLine(ResponseParameters);

                    // Add the parsed data in the file //

                    Console.WriteLine("Writing parsed data into the new file...");
                    Byte[] content = new UTF8Encoding(true).GetBytes(ResponseParameters);
                    fs.Write(content, 0, content.Length);
                    Console.WriteLine("Done!");
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

            try
            {
                // Determine whether the directory exists. //
                if (Directory.Exists(" \"" + GamePath + @"\FactoryGame\Content\Paks\~mods"))
                {
                    // If there is already a directory, just create the mod inside //
                    Console.WriteLine("Inserting Mod in ~mods directory");
                }

                // Try to create the directory, the pak patch file, the sig patch file. //
                String path = GamePath + @"\FactoryGame\Content\Paks\~mods\";

                DirectoryInfo di = Directory.CreateDirectory(path);
                Console.WriteLine("~mods directory was successfully created!");

                // Packaging / SIG Creation //

                Console.WriteLine("Creating PAK file...");
                Process.Start("\"" + PackerPath + "\"", " \"" + path + ModName + "_p.pak\" -Create=\"" + ResponseFile + "\"");
                Console.WriteLine("Creating SIG file...");

                File.Copy(Path.Combine(GamePath + @"\FactoryGame\Content\Paks", "FactoryGame-WindowsNoEditor.sig"), Path.Combine(GamePath + @"\FactoryGame\Content\Paks\~mods", ModName + "_p.sig"), true);
                Console.WriteLine("Success!");
                Console.WriteLine("Success!");

            } catch (Exception Ex)
            {
                Console.Write(Ex.ToString() + "\n");
            }

            Console.Write("Press any key to close the app...");
            Console.ReadKey();
        }
    }
}
