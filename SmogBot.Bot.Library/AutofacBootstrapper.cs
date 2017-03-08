using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using SmogBot.Bot.Dialogs;
using Tomaszkiewicz.DapperExtensions;

namespace SmogBot.Bot
{
    public static class AutofacBootstrapper
    {
        public static IContainer Container { get; }

        static AutofacBootstrapper()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<BasicProactiveEchoDialog>()
                //.Keyed<BasicProactiveEchoDialog>(FiberModule.Key_DoNotSerialize)
                .AsSelf();

            builder.RegisterType<SampleDependency>()
                //.Keyed<SampleDependency>(FiberModule.Key_DoNotSerialize)
                .AsSelf().SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsSelf();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("Tomaszkiewicz")))
                builder.RegisterAssemblyTypes(assembly);


            builder.Register(c => new SqlConnectionFactory(ConfigurationManager.ConnectionStrings["Bot"].ConnectionString));

            builder.Update(Conversation.Container);
            
            Container = Conversation.Container;

            //Container = builder.Build();
        }
    }
}