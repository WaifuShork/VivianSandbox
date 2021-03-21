using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Mvc;

namespace VivianSandbox.Controllers
{
    public class SandboxController : Controller
    {
        public struct SandboxRunParameters
        {
            public string Main { get; set; }
        }
        public struct SandboxRunResult
        {
            public string Output { get; set; }
        }

        [Route("/sandbox")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/sandbox/run")]
        [HttpPost]
        public async Task<IActionResult> Run([FromBody] SandboxRunParameters parameters)
        {
            // D:\Vivian\Vivian\std\src\Core.vivproj
            // ..\VivianProject\src\Core.vivproj
            // ..\VivianProject\src\Main.viv
            // TODO: Implement compilation.
            await System.IO.File.WriteAllTextAsync(@"..\VivianProject\src\Main.viv", parameters.Main);
            var compiler = new CompilerController(@"..\VivianProject\src\Core.vivproj");
            await compiler.Compile();
            await compiler.Execute();
            
            SandboxRunResult result;
            if (string.IsNullOrEmpty(parameters.Main))
            {
                result = new SandboxRunResult
                {
                    Output = "No code was supplied to compile."
                };
            }
            else
            {
                result = new SandboxRunResult
                {
                    Output = compiler.Result
                };
            }
            return Json(result);
        }
    }
}