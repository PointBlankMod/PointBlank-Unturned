using System;
using System.Collections.Generic;
using PointBlank.API.Interfaces;

namespace PointBlank
{
    public static class PointBlankUnturnedEnvironment
    {
        public static Dictionary<Type, ITranslatable> ServiceTranslations = new Dictionary<Type, ITranslatable>(); // Translations for the services
        public static Dictionary<Type, IConfigurable> ApiConfigurations = new Dictionary<Type, IConfigurable>(); // Configurations for the API
    }
}
