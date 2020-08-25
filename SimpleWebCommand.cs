using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Cysharp.Threading.Tasks;
using OpenMod.Unturned.Plugins;
using OpenMod.API.Plugins;
using System.Threading.Tasks;
using SDG.Unturned;
using OpenMod.Core.Helpers;

[assembly: PluginMetadata("SS.SimpleWebCommand", DisplayName = "SimpleWebCommand")]
namespace SimpleWebCommand
{
    public class SimpleWebCommand : OpenModUnturnedPlugin
    {
        private readonly IConfiguration ro_Configuration;
        private readonly System.Random ro_Random = new System.Random();
        private readonly ILogger<SimpleWebCommand> ro_Logger;

        public SimpleWebCommand(
            IConfiguration configuration, 
            ILogger<SimpleWebCommand> logger, 
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            ro_Configuration = configuration;
            ro_Logger = logger;
        }

        protected override async UniTask OnLoadAsync()
        {
            ro_Logger.LogInformation("Plugin loaded correctly!");
            ro_Logger.LogInformation("If you have any error you can contact the owner in discord: Senior S#9583");
            string z = ro_Configuration.GetSection("plugin_configuration:url_with_interval").Get<string>();
            if (z == "true")
            {
                var interval = ro_Configuration.GetSection("plugin_configuration:interval").Get<int>();
                AsyncHelper.Schedule("Interval Url", () => IntervalUrl(this, interval));
            }
        }

        public async Task IntervalUrl(IOpenModPlugin myPlugin, int interval)
        {
            while (myPlugin.IsComponentAlive)
            {
                await Task.Delay(TimeSpan.FromSeconds(interval));
                await UniTask.SwitchToMainThread();
                var z = ro_Configuration.GetSection("plugin_configuration:interval_urls").Get<string>().Split(',');
                int random = ro_Random.Next(z.Length - 1);
                if (random == 0 || random == 1)
                {
                    var url = z[0];
                    var description = z[1];
                    SendUrl(url, description);
                }
                if (random % 2 == 0)
                {
                    var url = z[random];
                    var description = z[random + 1];
                    SendUrl(url, description);
                }
                else
                {
                    var url = z[random - 1];
                    var description = z[random];
                    SendUrl(url, description);
                }
            }
        }

        private void SendUrl(string url, string description)
        {
            foreach (var client in Provider.clients) client.player.sendBrowserRequest(description, url);
        }

        protected override async UniTask OnUnloadAsync()
        {
            ro_Logger.LogInformation("Plugin unloaded correctly!");
        }
    }
}
