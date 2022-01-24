using Umbraco.Cms.Core.Services;
using System.Linq;
using System.Text;
using Umbraco.Cms.Core.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;

namespace Umbraco.Lexicon
{
    public class LexiconBuilder
    {
        private ILocalizationService LocalizationService { get; }
        private IWebHostEnvironment Environment { get; }

        private StringBuilder stringBuilder = new StringBuilder(string.Empty);
        private const string oneTab = "    ";
        public LexiconBuilder(ILocalizationService localizationService, IWebHostEnvironment environment)
        {
            LocalizationService = localizationService;
            Environment = environment;
        }

        public void Build()
        {
            var roots = LocalizationService.GetRootDictionaryItems();

            stringBuilder.AppendLine("using Umbraco.Cms.Web.Common;");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("namespace Umbraco.Lexicon");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine("    public partial class Lexicon");
            stringBuilder.AppendLine("    {");
            stringBuilder.AppendLine("        public Lexicon(UmbracoHelper umbracoHelper)");
            stringBuilder.AppendLine("        {");
            stringBuilder.AppendLine("            UmbracoHelper = umbracoHelper;");
            stringBuilder.AppendLine("        }");
            stringBuilder.AppendLine("        ");
            stringBuilder.AppendLine("        private UmbracoHelper UmbracoHelper { get; }");

            foreach (var root in roots)
            {
                WriteLemma_or_childrenStructs_for_dictionaryNode(root, oneTab + oneTab);
            }

            stringBuilder.AppendLine("    }");
            stringBuilder.AppendLine("}");

            var filepath = System.IO.Path.Combine(Environment.ContentRootPath, "Lexicon.generated.cs");
            System.IO.File.WriteAllText(filepath, stringBuilder.ToString());
        }

        public void WriteLemma_or_childrenStructs_for_dictionaryNode(IDictionaryItem node, string tabs)
        {
            var lemma_key = node.ItemKey.Split(".").Last();

            var lemma_guid = node.Key;
            var lemma_children = LocalizationService.GetDictionaryItemChildren(lemma_guid);

            if (lemma_children.Any())
            {
                stringBuilder.AppendLine($"{tabs}public class {lemma_key}Class");
                stringBuilder.AppendLine($"{tabs}{{");

                stringBuilder.AppendLine($"{tabs}{oneTab}public {lemma_key}Class(UmbracoHelper umbracoHelper)");
                stringBuilder.AppendLine($"{tabs}{oneTab}{{");
                stringBuilder.AppendLine($"{tabs}{oneTab}     UmbracoHelper = umbracoHelper;");
                stringBuilder.AppendLine($"{tabs}{oneTab}}}");
                stringBuilder.AppendLine($"{tabs}{oneTab}");
                stringBuilder.AppendLine($"{tabs}{oneTab}private UmbracoHelper UmbracoHelper {{ get; }}");
                foreach (var child in lemma_children)
                {
                    WriteLemma_or_childrenStructs_for_dictionaryNode(child, tabs + oneTab);
                }

                stringBuilder.AppendLine($"{tabs}}}");
                stringBuilder.AppendLine($"{tabs}public {lemma_key}Class {lemma_key} => new {lemma_key}Class(UmbracoHelper);");
            }
            else
            {
                stringBuilder.AppendLine($"{tabs}public string {lemma_key} => UmbracoHelper.GetDictionaryValue(\"{node.ItemKey}\");");
            }


        }
    }
}
