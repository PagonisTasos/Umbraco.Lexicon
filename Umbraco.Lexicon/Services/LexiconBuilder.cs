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

        private StringBuilder _sb = new StringBuilder(string.Empty);
        const string tab = "    ";
        private List<string> usings = new List<string>
        {
            "Umbraco.Cms.Web.Common;"
            , "Umbraco.Cms.Core.Composing;"
        };

        public LexiconBuilder(ILocalizationService localizationService, IWebHostEnvironment environment)
        {
            LocalizationService = localizationService;
            Environment = environment;
        }

        public void BuildLexicon()
        {

            AddUsings();
            BeginNamespace();
                AddLexiconComposer();
                AddLexiconClass();
            EndNamespace();

            SaveGeneratedFile();
        }

        private void AddUsings()
        {
            foreach (var @using in usings)
            {
                _sb.AppendLine($"using {@using}");
            }
            _sb.AppendLine("");
        }
        private void BeginNamespace()
        {
            _sb.AppendLine("namespace Umbraco.Lexicon");
            _sb.AppendLine("{");
        }
        private void EndNamespace()
        {
            _sb.AppendLine("}");
        }
        private void AddLexiconComposer()
        {
            _sb.AppendLine($"{tab}public class LexiconComposer: ComponentComposer<LexiconComponent> {{ }}");
            _sb.AppendLine($"{tab}");
            _sb.AppendLine($"{tab}public class LexiconComponent : IComponent");
            _sb.AppendLine($"{tab}{{");
            _sb.AppendLine($"{tab}{tab}private readonly IUmbracoHelperAccessor accessor;");
            _sb.AppendLine($"{tab}{tab}");
            _sb.AppendLine($"{tab}{tab}public LexiconComponent(IUmbracoHelperAccessor accessor) => this.accessor = accessor;");
            _sb.AppendLine($"{tab}{tab}public void Initialize() => Lexicon.Configure(this.accessor);");
            _sb.AppendLine($"{tab}{tab}public void Terminate() {{ }}");
            _sb.AppendLine($"{tab}}}");
        }
        private void AddLexiconClass()
        {
            var roots = LocalizationService.GetRootDictionaryItems();

            _sb.AppendLine($"{tab}public static partial class Lexicon");
            _sb.AppendLine($"{tab}{{");
            _sb.AppendLine($"{tab}{tab}private static IUmbracoHelperAccessor _accessor;");
            _sb.AppendLine($"{tab}{tab}private static string Get(string key) => _accessor.TryGetUmbracoHelper(out UmbracoHelper umbracoHelper) ? umbracoHelper.GetDictionaryValue(key) : null;");
            _sb.AppendLine($"{tab}{tab}public static void Configure(IUmbracoHelperAccessor accessor) => _accessor = accessor;");
            _sb.AppendLine($"{tab}{tab}");

            foreach (var root in roots)
            {
                Add_Lemma_or_Struct_for_dictionaryNode(root, $"{tab}{tab}");
            }

            _sb.AppendLine($"{tab}}}");
        }
        private void Add_Lemma_or_Struct_for_dictionaryNode(IDictionaryItem node, string tabs)
        {
            var lemma_key = node.ItemKey.Split(".").Last();
            var lemma_guid = node.Key;
            var lemma_children = LocalizationService.GetDictionaryItemChildren(lemma_guid);

            if (lemma_children.Any())
            {
                _sb.AppendLine($"{tabs}public struct {lemma_key}");
                _sb.AppendLine($"{tabs}{{");

                foreach (var child in lemma_children)
                {
                    Add_Lemma_or_Struct_for_dictionaryNode(child, tabs + tab);
                }

                _sb.AppendLine($"{tabs}}}");
            }
            else
            {
                _sb.AppendLine($"{tabs}public static string {lemma_key} => Get(\"{node.ItemKey}\");");
            }


        }
        public void SaveGeneratedFile()
        {
            var filepath = System.IO.Path.Combine(Environment.ContentRootPath, "Lexicon.generated.cs");
            System.IO.File.WriteAllText(filepath, _sb.ToString());
        }
    }

}
