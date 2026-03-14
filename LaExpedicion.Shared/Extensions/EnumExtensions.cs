using System.ComponentModel;
using System.Reflection;

namespace LaExpedicion.Shared.Extensions;

public static class EnumExtensions
{
    public static string GetEnumDescription(this Enum value)
    {
        // 1. Obtenemos el campo
        FieldInfo? field = value.GetType().GetField(value.ToString());
        
        if (field == null)
        {
            return value.ToString();
        }

        // 2. Buscamos el atributo de forma segura
        var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

        // 3. Si tiene el atributo devolvemos la descripción, si no, el nombre normal
        return attribute?.Description ?? value.ToString();
    }

}