using System.Diagnostics;
using System.Threading.Tasks;

namespace VivianSandbox.CompilerService
{
    public class Compiler
    {
        public async Task<string> CompileFile()
        {
            var process = new Process
            {
                StartInfo =
                {
                    // todo: setup project files inside docker instance
                    FileName = "dotnet.exe", 
                    Arguments = @"run --project G:\VivianLang\Vivian\samples\src\Playground.vivproj",
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };
        
            process.Start();
            var output = await process.StandardOutput.ReadToEndAsync();

            await Task.CompletedTask;
            return output;
        }
    }
}