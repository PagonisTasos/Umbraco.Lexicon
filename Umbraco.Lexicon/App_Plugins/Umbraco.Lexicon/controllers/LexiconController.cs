using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Lexicon;

namespace UmbracoLexikonTest.App_Plugins.Umbraco.Lexicon.controllers
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
#if DEBUG
            lexiconBuilder.Build();
            return Ok("");
#endif
            return Conflict("Cant perform this action in release mode."); 
        }
    }
}
