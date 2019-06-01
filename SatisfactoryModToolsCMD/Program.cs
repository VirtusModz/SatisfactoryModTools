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
        static void Main(string[] args)
        {

            // Unreal data
            String PackagerPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Epic Games\UE_4.20\Engine\Binaries\Win64\UnrealPak.exe";
            String PakListPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Unreal Engine\AutomationTool\Logs\C+Program+Files+Epic+Games+UE_4.20\PakList_FactoryGame-WindowsNoEditor.txt";

            // Mod data
            String GamePath = null;
            String ModName = null;

            // Package data
            String ResponseFile = @"C:\temp\Response.txt";
            String ResponseParameters = null;

            // I/O Texts
            Console.WriteLine("Satisfactory Mod Tools for Windows\r");
            Console.WriteLine("----------------------------------\n\n");

            Console.WriteLine("/!\\ You need Unreal Engine 4.20.3 to make this tool work properly!\n\n");
            Console.WriteLine("-------------------------------------------------------------------\n\n");

            Console.WriteLine("Type in your Satisfactory game folder, and then press Enter");
            Console.WriteLine("The default one is: " + @"C:\Program Files\Epic Games\SatisfactoryEarlyAccess");
            GamePath = Convert.ToString(Console.ReadLine());

            Console.WriteLine("Type in the name of your Mod, and then press Enter");
            ModName = Convert.ToString(Console.ReadLine());

            // Creating Response File
            Console.WriteLine("Initialize Response File...");
            String fileName = @"C:\temp\Response.txt";

            try
            {
                // Check if file already exists. If yes, delete it.
                if (File.Exists(fileName))
                {
                    Console.WriteLine("Response File already exist, deleting the old one...");
                    File.Delete(fileName);
                }

                // Create a new file
                using (FileStream fs = File.Create(fileName))
                {
                    Console.WriteLine("Creating Response File...");
                    Console.WriteLine("Reading PakList File...");

                    String text = File.ReadAllText(PakListPath);
                    Console.WriteLine(text);

                    Console.WriteLine("Parsing required parameters...");

                     // Parse it

                    String[] lines = File.ReadAllLines(PakListPath);
                    IEnumerable<string> selectLines = lines.Where(line => line.Contains(@"\FactoryGame\Saved\Cooked\WindowsNoEditor\FactoryGame\Content\"));
                    foreach (var item in selectLines)
                    {
                        ResponseParameters += item + Environment.NewLine;
                    }

                    Console.WriteLine("Parsed with success!");
                    Console.WriteLine(ResponseParameters);

                    // Add the parsed data in the file

                    Console.WriteLine("Writing parsed data into the file...");
                    Byte[] content = new UTF8Encoding(true).GetBytes(ResponseParameters);
                    fs.Write(content, 0, content.Length);
                    Console.WriteLine("Done!");
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

            Console.Clear();

            try
            {
                Console.WriteLine("Packaging the mod...");
                // Determine whether the directory exists.
                if (Directory.Exists(" \"" + GamePath + @"\FactoryGame\Content\Paks\~mods"))
                {
                    // If there is already a directory, just create the mod inside
                    Console.WriteLine("That path exists already. Adding the mod inside...");
                }

                // Try to create the directory, the pak patch file, the sig patch file.
                String path = GamePath + @"\FactoryGame\Content\Paks\~mods\";
                DirectoryInfo di = Directory.CreateDirectory(path);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));
                Process.Start("\"" + PackagerPath + " \"", " \"" + path + ModName + "_p.pak\" -Create=\"" + ResponseFile + "\"");
                Console.WriteLine("Success!");
                Console.WriteLine("Adding custom SIG...");
                File.Copy(Path.Combine(GamePath + @"\FactoryGame\Content\Paks", "FactoryGame-WindowsNoEditor.sig"), Path.Combine(GamePath + @"\FactoryGame\Content\Paks\~mods", ModName + "_p.sig"), true);
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
