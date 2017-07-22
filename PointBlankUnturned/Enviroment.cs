using System;
using System.Collections.Generic;
using PointBlank.API.Interfaces;

namespace PointBlank
{
    public static class Enviroment
    {
        public static Dictionary<Type, ITranslatable> ServiceTranslations = new Dictionary<Type, ITranslatable>(); // Translations for the services
    }
}
