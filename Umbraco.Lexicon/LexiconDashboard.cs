using System;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Dashboards;

namespace Umbraco.Lexicon
{
    [Weight(45)]
    public class LexiconDashboard : IDashboard
    {
        public string Alias => "lexicon builder";

        public string[] Sections => new[]
        {
            Umbraco.Cms.Core.Constants.Applications.Settings
        };

        public string View => "/App_Plugins/Umbraco.Lexicon/views/dashboard.html";

        public IAccessRule[] AccessRules
        {
            get
            {
                var rules = new IAccessRule[]
                {
                    new AccessRule {Type = AccessRuleType.Grant, Value = Umbraco.Cms.Core.Constants.Security.AdminGroupAlias}
                };
                return rules;
            }
        }
    }
}
