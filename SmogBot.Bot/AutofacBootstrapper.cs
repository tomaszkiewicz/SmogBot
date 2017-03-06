using System.Reflection;
using Autofac;

namespace SmogBot.Bot
{
    public static class AutofacBootstrapper
    {
        public static IContainer Container { get; }

        static AutofacBootstrapper()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsSelf();

            builder.RegisterType<SampleDependency>()
                //.Keyed<SampleDependency>(FiberModule.Key_DoNotSerialize)
                .AsSelf().SingleInstance();

            Container = builder.Build();
        }
    }
}