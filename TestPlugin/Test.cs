using System.Collections.Generic;
using PointBlank.API;
using PointBlank.API.Collections;
using PointBlank.API.Plugins;

namespace TestPlugin
{
    public class Test : PointBlankPlugin
    {
        public override TranslationList Translations => new TranslationList()
        {
            { "test", "This is a test" }
        };

        public override ConfigurationList Configurations => new ConfigurationList()
        {
            { "test", "hello" },
            { "lolz", "smth" },
            { "testList", new List<object>()
            {
                "test string 1",
                69,
                "test string 2"
            } },
            { "adding", "just a test" }
        };

        public override string Version => "1.0.0.1";

        public override string VersionURL => "http://pastebin.com/raw/uYUr6pPN";

        public override void Load()
        {
            PointBlankLogging.Log("Hello from test plugin load!");
            PointBlankLogging.Log("Translation test: " + Translate("test")); // Call the translation
            PointBlankLogging.Log("Configuration test: " + Configure<string>("test")); // Call the test configuration
            PointBlankLogging.Log("Configuration test 2: " + (string)(Configure<List<object>>("testList")[0]));
        }

        public override void Unload()
        {
            PointBlankLogging.Log("Hello from test plugin unload!");
        }
    }
}
