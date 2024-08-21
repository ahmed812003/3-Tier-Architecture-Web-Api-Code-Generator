using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace CreateNewProjectScript.App
{
    public class Program
    {
        public static List<string> Dependencies = new List<string>
        {
            "Microsoft.AspNetCore.Authentication.JwtBearer",
            "Microsoft.AspNetCore.Identity.EntityFrameworkCore",
            "Microsoft.EntityFrameworkCore",
            "Microsoft.EntityFrameworkCore.Design",
            "Microsoft.EntityFrameworkCore.SqlServer",
            "Microsoft.EntityFrameworkCore.Tools",
            "Microsoft.VisualStudio.Web.CodeGeneration.Design",
            "System.IdentityModel.Tokens.Jwt"
        };

        static void Main(string[] args)
        {
            Console.Write("Enter Project Name : ");
            var projectName = Console.ReadLine();
            Console.Write("Enter Project Path : ");
            var path = Console.ReadLine();
            
            var MainProjectPath = Path.Combine(path, projectName);

            CreateFolder(MainProjectPath);

            CreateSolution(projectName, MainProjectPath); 
            
            CreateWebApiProject(projectName, MainProjectPath);
            AddProjectToSolution($"{projectName}.Api", MainProjectPath);
            
            CreateClassLib($"{projectName}.Entities", MainProjectPath);
            AddProjectToSolution($"{projectName}.Entities", MainProjectPath);
            
            CreateClassLib($"{projectName}.DataService", MainProjectPath);
            AddProjectToSolution($"{projectName}.DataService", MainProjectPath);

            var projectApiPath = Path.Combine(MainProjectPath, $"{projectName}.Api");

            var projectEntitesPath = Path.Combine(MainProjectPath , $"{projectName}.Entities");
            CleanClassLib(projectEntitesPath);
            CreateFolder(Path.Combine(projectEntitesPath, "Models"));
           
            var projectServicesPath = Path.Combine(MainProjectPath, $"{projectName}.DataService");
            CleanClassLib(projectServicesPath);
            CreateFolder(Path.Combine(projectServicesPath, "Data"));
            CreateFolder(Path.Combine(projectServicesPath, "Repositories\\Interfaces"));
            CreateFolder(Path.Combine(projectServicesPath , "Services"));
   
            ReWriteFile(projectApiPath , $"namespace {projectName}.Api", "Program");
            ReWriteFile(projectApiPath , "", "appsettings");
            ReWriteFile(Path.Combine(projectEntitesPath, "Models"), $"namespace {projectName}.Entities.Models", "AppUser");
            ReWriteFile(Path.Combine(projectServicesPath, "Data") , $"namespace {projectName}.DataService.Data", "AppDbContext");
            ReWriteFile(Path.Combine(projectServicesPath, "Repositories"), $"namespace {projectName}.DataService.Repositories", "GenericRepository");
            ReWriteFile(Path.Combine(projectServicesPath, "Repositories\\Interfaces"), $"namespace {projectName}.DataService.Repositories.Interfaces", "IGenericRepository");
            ReWriteFile(Path.Combine(projectServicesPath, "Services"), $"namespace {projectName}.DataService.Services", "EmailSender");

            InstallDependencies(projectApiPath);
            InstallDependencies(projectEntitesPath);
            InstallDependencies(projectServicesPath);

            
            Console.ReadKey();
        }

        private static void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Console.WriteLine($"Folder created at: {path}");
            }
            else
            {
                Console.WriteLine("Folder already exists.");
            }
        }

        private static void CreateSolution(string projectName, string path)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"new sln --name \"{projectName}\"",
                WorkingDirectory = path,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(processStartInfo);
            ArgumentNullException.ThrowIfNull(process);
            string result = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (string.IsNullOrEmpty(error))
            {
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine("Error creating Blank Sln");
                Console.WriteLine(error);
            }
        }

        private static void CreateWebApiProject(string projectName , string path)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"new webapi -n \"{projectName}.Api\" -f net8.0 --use-minimal-apis false",
                WorkingDirectory = path,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(processStartInfo);
            ArgumentNullException.ThrowIfNull(process);
            string result = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (string.IsNullOrEmpty(error))
            {
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine("Error creating Web API project:");
                Console.WriteLine(error);
            }         
        }

        private static void CreateClassLib(string projectName, string path)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"new classlib -n \"{projectName}\" -f net8.0",
                WorkingDirectory = path,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(processStartInfo);
            ArgumentNullException.ThrowIfNull(process);
            string result = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (string.IsNullOrEmpty(error))
            {
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine("Error creating Class lib project:");
                Console.WriteLine(error);
            }
        }

        private static void AddProjectToSolution(string projectName, string path)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"sln add .\\{projectName}\\{projectName}.csproj",
                WorkingDirectory = path,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(processStartInfo);
            ArgumentNullException.ThrowIfNull(process);
            var result = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (string.IsNullOrEmpty(error))
            {
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine("Error added project:");
                Console.WriteLine(error);
            }
        }
    
        private static void CleanClassLib(string path)
        {
            File.Delete(Path.Combine(path , "class1.cs"));
            Console.WriteLine("Class library cleaned successfully");
        }

        private static void ReWriteFile(string path, string fileNamespace, string fileName)
        {
            var FilePath = Path.Combine(Directory.GetCurrentDirectory());
            FilePath = Directory.GetParent(FilePath).Parent.Parent.FullName;
            FilePath = Path.Combine(FilePath, $"ImportantFiles\\{fileName}.txt");

            if (fileName != "appsettings")
            {
                path = Path.Combine(path, $"{fileName}.cs");
                var lines = File.ReadAllLines(FilePath);
                for(int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("namespace"))
                    {
                        lines[i] = fileNamespace;
                    }
                }
                
                File.WriteAllLines(FilePath, lines);

                var content = File.ReadAllText(FilePath);
                if (!File.Exists(path))
                {
                    using FileStream fs = File.Create(path);
                }
                if (File.Exists(path))
                    File.WriteAllText(path, content);
            }
            else
            {
                path = Path.Combine(path, $"{fileName}.json");
                var content = File.ReadAllText(FilePath);
                File.WriteAllText(path, content);
            }
            Console.WriteLine("file rewrited successfully");
        }

        private static void InstallDependencies(string path)
        {
            foreach (var depend in Dependencies)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = $"add . package {depend}",
                    WorkingDirectory = path,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                var process = Process.Start(processStartInfo);
                ArgumentNullException.ThrowIfNull(process);
                string result = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (string.IsNullOrEmpty(error))
                {
                    Console.WriteLine(result);
                }
                else
                {
                    Console.WriteLine("Error install package in project:");
                    Console.WriteLine(error);
                }
            }
        }

    }
}