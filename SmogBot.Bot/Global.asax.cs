using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Compilation;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Bot.Builder.Dialogs;
using SmogBot.Bot.Helpers;
using SmogBot.Bot.Tools;
using Tomaszkiewicz.BotFramework.Dialogs;
using Tomaszkiewicz.DapperExtensions;

namespace SmogBot.Bot
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());
            builder.RegisterAssemblyTypes(BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray());

            builder.RegisterType<SmogBot>()
                .As<Tomaszkiewicz.BotFramework.Bot>();

            builder.RegisterType<SqlConnectionFactory>()
                .AsSelf()
                .WithParameter("connectionString", ConfigurationManager.ConnectionStrings["Bot"].ConnectionString);

            RegisterMainMenu(builder);

            builder.RegisterType<NotificationsMenuItems>()
                .As<Dictionary<string, Func<IDialogContext, ResumeAfter<object>, Task>>>();

            var container = builder.Build();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        private static void RegisterMainMenu(ContainerBuilder builder)
        {
            builder.RegisterType<MainMenuItems>()
                .As<Dictionary<string, Func<IDialog<object>>>>();

            builder.RegisterType<ExceptionHandlerDialog<object>>()
                .Named<IDialog<object>>("dialogs");

            

            builder.RegisterType<MenuDialogDispatcher>()
                .Named<IDialog<object>>("dialogs");

            builder.RegisterDecorator<IDialog<object>>(x => x, "dialogs");
        }
    }
}