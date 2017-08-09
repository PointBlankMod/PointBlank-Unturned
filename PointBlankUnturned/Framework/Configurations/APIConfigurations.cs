using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Collections;
using PointBlank.API.Interfaces;

namespace PointBlank.Framework.Configurations
{
    internal class APIConfigurations : IConfigurable
    {
        public string ConfigurationDirectory => "";

        public ConfigurationList Configurations => new ConfigurationList()
        {
            { "WebPermissions", false },
            { "WebPermissionsSite", "http://127.0.0.1/index.php?serverName=TestServer&file={0}" },
            { "WebPermissionsInterval", 600 },
        };

        public Dictionary<Type, IConfigurable> ConfigurationDictionary => Enviroment.APIConfigurations;
    }
}
