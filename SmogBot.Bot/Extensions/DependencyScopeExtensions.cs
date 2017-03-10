using System.Web.Http.Dependencies;

namespace SmogBot.Bot.Extensions
{
    public static class DependencyScopeExtensions
    {
        public static T Resolve<T>(this IDependencyScope scope)
        {
            return (T) scope.GetService(typeof (T));
        }
    }
}