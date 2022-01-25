using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Lexicon;

namespace Umbraco.Lexicon
{
    public class LexiconController : UmbracoAuthorizedApiController
    {
        private readonly LexiconBuilder lexiconBuilder;

        public LexiconController(ILocalizationService localizationService, IWebHostEnvironment webHostEnvironment)
        {
            this.lexiconBuilder = new LexiconBuilder(localizationService, webHostEnvironment);
        }

        [HttpPost]
        public ActionResult BuildLexicon()
        {
            lexiconBuilder.BuildLexicon();
            return Ok();
        }
    }
}
