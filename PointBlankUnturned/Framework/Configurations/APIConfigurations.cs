using System;
using System.Collections.Generic;
using PointBlank.API.Collections;
using PointBlank.API.Interfaces;

namespace PointBlank.Framework.Configurations
{
    internal class APIConfigurations : IConfigurable
    {
        public override string ConfigurationDirectory => "";

        public override ConfigurationList DefaultConfigurations => new ConfigurationList()
        {
            { "WebPermissions", false },
            { "WebPermissionsSite", "http://127.0.0.1/index.php?serverName=TestServer&file={0}" },
            { "WebPermissionsInterval", 600 },
        };

        public override Dictionary<Type, IConfigurable> ConfigurationDictionary => Enviroment.APIConfigurations;
    }
}
