using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

using VivianSandbox.Models;
using System.Globalization;

namespace VivianSandbox.Controllers
{
    public class SpecController : Controller
    {
        public string SpecRoot => Path.Join(_environment.ContentRootPath, _relativeSpecRoot);

        private const string _relativeSpecRoot = "../modules/spec/spec";
        private const string _specIndex = "readme.md";

        private IWebHostEnvironment _environment;

        public SpecController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [Route("/spec")]
        public IActionResult Index()
        {
            return Redirect("/spec/" + _specIndex);
        }

        [Route("/spec/{**slug}")]
        public async Task<IActionResult> Page(string slug)
        {
            slug = string.IsNullOrEmpty(slug) ? "readme" : slug;
            string path = Path.Join(SpecRoot, slug);

            if (System.IO.File.Exists(path))
            {
                string title = Path.GetFileNameWithoutExtension(path);
                string content = await System.IO.File.ReadAllTextAsync(path);

                var page = new SpecPage
                {
                    Title = new CultureInfo("en-US").TextInfo.ToTitleCase(title),
                    Content = content,
                    IsRoot = slug == _specIndex
                };

                return View("Page", page);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
