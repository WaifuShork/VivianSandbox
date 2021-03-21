using System.Diagnostics;
using System.Threading.Tasks;

namespace VivianSandbox.Controllers
{
    public class CompilerController
    {
        public CompilerController(string source)
        {
            Source = source;
        }

        public string Source { get; }
        public string Result { get; private set; }

        public async Task Compile()
        {
            var process = new Process
            {
                StartInfo =
                {
                    FileName = "dotnet.exe",
                    Arguments = $"build --project {Source}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };
            process.Start();
            Result = await process.StandardOutput.ReadToEndAsync();
            await System.Console.Out.WriteLineAsync(Result);
        }

        public async Task Execute()
        {
            var process = new Process
            {
                StartInfo =
                {
                    FileName = @"..\VivianProject\src\bin\Debug\Core.exe",
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };
            process.Start();
            Result = await process.StandardOutput.ReadToEndAsync();
            await System.Console.Out.WriteLineAsync(Result);
        }
    }
}