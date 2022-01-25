using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.BackOffice.Controllers;

namespace Umbraco.Lexicon
{
    public class LexiconController : UmbracoAuthorizedApiController
    {
        private readonly LexiconBuilder lexiconBuilder;
        private readonly IWebHostEnvironment webHostEnvironment;

        public LexiconController(ILocalizationService localizationService, IWebHostEnvironment webHostEnvironment)
        {
            this.lexiconBuilder = new LexiconBuilder(localizationService, webHostEnvironment);
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public ActionResult BuildLexicon()
        {
            if (webHostEnvironment.IsDevelopment())
            {
                lexiconBuilder.BuildLexicon();
                return Ok();
            }

            return Conflict("Can only generate the Lexicon.cs file when in development environment.");
        }
    }
}
