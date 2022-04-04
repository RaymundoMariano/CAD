using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CAD.Domain
{
    public static class EnumService<T>
    {
        public static IList<SelectOption> GetOptions<T>()
        {
            var tipo = typeof(T);
            var result = Enum.GetNames(tipo).Select(
                item => new SelectOption
                {
                    Value = (int)Enum.Parse(tipo, item),
                    Description = GetDescription<T>(item)
                }).ToList();
            return result;
        }

        public static string GetDescription<T>(string enumDescription)
        {
            var descriptionAttribute = typeof(T)
                .GetField(enumDescription)
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;
            return descriptionAttribute != null
                ? descriptionAttribute.Description
                : enumDescription;
        }
    }

    public class SelectOption
    {
        public int Value { get; set; }
        public string Description { get; set; }
    }
}
