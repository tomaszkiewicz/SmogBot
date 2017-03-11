using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace SmogBot.Bot.Extensions
{
    public static class ObjectExtensions
    {
        public static void RestoreAllDependencies<T>(this T obj)
        {
            var fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var field in fields)
            {
                var attr = field.GetCustomAttributes<NonSerializedAttribute>();

                if (attr.Any())
                    field.SetValue(obj, GlobalConfiguration.Configuration.DependencyResolver.GetService(field.FieldType));
            }
        }
    }
}