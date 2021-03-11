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

        [Route("/")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/sandbox/run")]
        [HttpPost]
        public async Task<IActionResult> Run([FromBody] SandboxRunParameters parameters)
        {
            // TODO: Implement compilation.
            await Task.Delay(2000);

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
                    Output = "This is a sample output to test the execute button."
                };
            }

            return Json(result);
        }
    }
}