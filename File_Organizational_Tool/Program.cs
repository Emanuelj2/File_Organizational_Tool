using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;

namespace File_Organizational_Tool
{
    class Program
    {
        //these are the categories and the types of files that are in them
        private static readonly Dictionary<string, string[]> FileCategories = new()
        {
            { "Images", new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg", ".ico", ".webp" } },
            { "Documents", new[] { ".pdf", ".doc", ".docx", ".txt", ".xlsx", ".xls", ".ppt", ".pptx", ".csv", ".odt" } },
            { "Audio", new[] {".mp3", ".wav", ".flac", ".aac", ".ogg", ".m4a", ".wma"}},
            { "Video", new[] { ".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv", ".webm", ".m4v" } },
            { "Archives", new[] { ".zip", ".rar", ".7z", ".tar", ".gz", ".iso" } },
            { "Code", new[] { ".cs", ".java", ".py", ".js", ".html", ".css", ".cpp", ".c", ".h", ".json", ".xml" } },
            { "Executables", new[] { ".exe", ".msi", ".dll", ".bat", ".sh" } }
        };

        static void Main()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔═══════════════════════════════════════╗");
            Console.WriteLine("║          FILE ORGANIZER TOOL          ║");
            Console.WriteLine("╚═══════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();

            try
            {
                String folderPath = GetFolderPath();

                if (!Directory.Exists(folderPath))
                {
                    ShowError("The folder does not exist");
                    return;
                }

                //display the options
                Console.WriteLine("\nChoose operation mode:");
                Console.WriteLine("1. Move files (original files will be moved)");
                Console.WriteLine("2. Copy files (original files remain)");
                Console.Write("\nEnter your choice (1 or 2): ");

                bool moveFiles = Console.ReadLine()?.Trim() == "1";
                string operation = moveFiles ? "Moving" : "Copying";

                Console.WriteLine("\nInculde subfolders? (y/n): ");
                bool includeSubfolders = Console.ReadLine()?.Trim().ToLower() == "y";

                Console.WriteLine($"\n{operation} files...\n");

                OrganizeFiles(folderPath, moveFiles, includeSubfolders);

                //YO YOUR FILES ARE ORGANIZED
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nAll files organized successfully!");
                Console.ResetColor();
            }   
            catch (Exception ex) 
            {
                ShowError($"An error occurred: {ex.Message}");
            }

            //exit
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        //STEP 1:
        //get the folder path that the user will be organizing
        static string GetFolderPath()
        {
            Console.WriteLine("Enter the folder path that you would like to organize:");
            string? path = Console.ReadLine()?.Trim().Trim('"');

            //if the directory is empty
            if(String.IsNullOrEmpty(path))
            {
                path = Directory.GetCurrentDirectory();
                Console.WriteLine($" using current path: {path}");
            }

            return path;
        }

        //STEP 2:
        //organize the files
        static void OrganizeFiles(string folderPath, bool moveFiles, bool includeSubfolders)
        {
            var searchOption = includeSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            string[] files = Directory.GetFiles(folderPath, "*.*", searchOption);

            //base case if there are no files in the directory
            if(files.Length == 0)
            {
                Console.WriteLine($"there are no files foundd in {folderPath}.");
                return;
            }

            Console.WriteLine($"found {files.Length} files\n"); //# fo files in the directory

            var stats = new Dictionary<string, int>();
            int skippedFiles = 0;
            int errorCount = 0;

            foreach(string filePath in files)
            {
                try
                {
                    string fileName = Path.GetFileName(filePath);
                    string extension = Path.GetExtension(filePath).ToLower();

                    // Skip if file is in a category folder already
                    string? parentFolder = Path.GetFileName(Path.GetDirectoryName(filePath));
                    if (FileCategories.ContainsKey(parentFolder))
                    {
                        skippedFiles++;
                        continue;
                    }

                    //Determine the category of the file
                    string category = GetFileCategory(extension);

                    // Create category folder if it doesn't exist
                    string categoryFolder = Path.Combine(folderPath, category);
                    Directory.CreateDirectory(categoryFolder);

                    //make the destination path
                    string destPath = Path.Combine(categoryFolder, fileName);

                    //handle duplicate files
                    destPath =  GetUniqueFilePath(destPath);

                    //move or copy duplicates
                    if(moveFiles)
                    {
                        File.Move(filePath, destPath);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("MOVED: ");
                    }
                    else 
                    {
                        File.Copy(filePath, destPath);
                        Console.ForegroundColor= ConsoleColor.Blue;
                        Console.WriteLine("COPIED: ");
                    }

                    Console.ResetColor();
                    Console.WriteLine($"{fileName} → {category}/");

                    //Updatee the stats
                    if (!stats.ContainsKey(category))
                        stats[category] = 0;
                    stats[category]++;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR: Could not process {Path.GetFileName(filePath)} - {ex.Message}");
                    Console.ResetColor();
                    errorCount++;
                }
            }

            //Display your summary
            Console.WriteLine("\n" + new string('─', 50));
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("SUMMARY:");
            Console.ResetColor();
            Console.WriteLine(new string('─', 50));

            foreach (var kvp in stats.OrderByDescending(x => x.Value))
            {
                Console.WriteLine($"{kvp.Key,-20} : {kvp.Value,5} file(s)");
            }

            if (skippedFiles > 0)
                Console.WriteLine($"{"Skipped",-20} : {skippedFiles,5} file(s)");

            if (errorCount > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{"Errors",-20} : {errorCount,5} file(s)");
                Console.ResetColor();
            }
            Console.WriteLine(new string('─', 50));
            Console.WriteLine($"{"Total Processed",-20} : {stats.Values.Sum(),5} file(s)");

        }

        //STEP 2.5
        //Determine the category of the file (think of a hashmap)
        //{key: value} --EXAMPLE--> { "Image": {.img, ... }}
        static string GetFileCategory(string extension)
        {
            foreach (var category in FileCategories)
            {
                if (category.Value.Contains(extension))
                {
                return category.Key;
                }
            }
            return "Others";
        }

        static string GetUniqueFilePath(string filePath)
        {
            if (!File.Exists(filePath))
                return filePath;

            string? directory = Path.GetDirectoryName(filePath);
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            int counter = 1;

            string newPath;
            do
            {
                string newFileName = $"{fileNameWithoutExt}_{counter}{extension}";
                newPath = Path.Combine(directory, newFileName);
                counter++;
            }
            while (File.Exists(newPath));

            return newPath;
        }

        //Show Error
        static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n {message}");
            Console.ResetColor ();
        }

    }
}