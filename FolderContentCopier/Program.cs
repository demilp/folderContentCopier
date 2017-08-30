using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderContentCopier
{
    class Program
    {
        static string ext;
        static void Main(string[] args)
        {
            string source = ConfigurationManager.AppSettings["source"];
            string destination = ConfigurationManager.AppSettings["destination"];
            ext = ConfigurationManager.AppSettings["extension"];


            DirectoryCopy(source, destination, (bool.Parse(ConfigurationManager.AppSettings["copySubdirectories"])));
            /*if (bool.Parse(ConfigurationManager.AppSettings["copyFiles"]))
            {
                string[] files = ext =="" ? Directory.GetFiles(source) : Directory.GetFiles(source, "*." + ext);
                for (int i = 0; i < files.Length; i++)
                {
                    File.Copy(files[i], Path.Combine(destination, Path.GetFileName(files[i])));
                }
            }
            if (bool.Parse(ConfigurationManager.AppSettings["copySubdirectories"]))
            {
                foreach (string dirPath in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))

                    Directory.CreateDirectory(Path.Combine(destination, new DirectoryInfo(dirPath).Name));



                //Copy all the files & Replaces any files with the same name

                foreach (string newPath in Directory.GetFiles(source, "*.*",

                    SearchOption.AllDirectories))

                    File.Copy(newPath, Path.Combine(destination, Path.GetFileName(newPath)), true);
            }*/
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
            }
            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }
            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = ext == "" ? dir.GetFiles(): dir.GetFiles("*." + ext);
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }
            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
