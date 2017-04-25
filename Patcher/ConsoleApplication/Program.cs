using System;
using System.IO;
using System.Linq;
using dnpatch.script;

namespace ConsoleApplication
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string directory = Environment.CurrentDirectory + "\\mods\\";
            DirectoryInfo d = new DirectoryInfo(directory);
            if (!File.Exists("Assembly-CSharp.dll.bak"))
            {
                Console.WriteLine("Backing up Assembly-CSharp.dll....");
                File.Copy("Assembly-CSharp.dll", "Assembly-CSharp.dll.bak", true);
                Console.WriteLine("Done!");
            }
            FileInfo[] files = d.GetFiles("*.json");
            var orderedFiles = files.OrderBy(f => f.Name);
            int i = 0;
            foreach (var file in orderedFiles)
            {
                Console.WriteLine("Applying Mod: "+file.Name);
                Script script = new Script(directory+file, i);
                script.Patch();
                if (orderedFiles.Count() != i + 1) script.Save("Assembly-CSharp.dll" + ++i);
                else script.Save("Assembly-CSharp.dll");
                Console.WriteLine("Done!");
            }
            if (!orderedFiles.Any()) File.Copy("Assembly-CSharp.dll.bak", "Assembly-CSharp.dll", true);

            Console.WriteLine("Deleting Temp Files...");
            for (int j = 0; j < orderedFiles.Count(); j++)
            {
                File.Delete("Assembly-CSharp.dll"+(j+1));
            }
            Console.WriteLine("Done!");
            Console.WriteLine("Press Enter to close this window.");
            Console.ReadKey();
        }
    }
}