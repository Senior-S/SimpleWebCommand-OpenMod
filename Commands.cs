using System;
using OpenMod.Core.Commands;
using System.Threading.Tasks;
using OpenMod.Unturned.Users;
using OpenMod.API.Commands;
using OpenMod.API.Users;
using Command = OpenMod.Core.Commands.Command;
using OpenMod.Core.Users;
using Cysharp.Threading.Tasks;

namespace SimpleWebCommand
{
    [Command("sendbrowserrequest")]
    [CommandAlias("sbrequest")]
    [CommandAlias("sbr")]
    [CommandSyntax("<player> <url> [description]>")]
    [CommandDescription("Command to send web request to players.")]
    public class CommandSendBrowserRequest : Command
    {
        private readonly IUserManager ro_UserManager;

        public CommandSendBrowserRequest(IServiceProvider serviceProvider, IUserManager userManager) : base(serviceProvider)
        {
            ro_UserManager = userManager;
        }

        protected override async Task OnExecuteAsync()
        {
            var user = (UnturnedUser)Context.Actor;
            if (Context.Parameters.Length <= 0)
            {
                throw new UserFriendlyException("Please specfiy a player!");
            }
            else if (Context.Parameters.Length <= 1)
            {
                throw new UserFriendlyException("Please specify a link!");
            }
            var targetname = await Context.Parameters.GetAsync<string>(0);
            var url = await Context.Parameters.GetAsync<string>(1);
            var target = (UnturnedUser)await ro_UserManager.FindUserAsync(KnownActorTypes.Player, targetname, UserSearchMode.FindByNameOrId);
            await UniTask.SwitchToMainThread();
            if (Context.Parameters.Length == 3)
            {
                var description = await Context.Parameters.GetAsync<string>(2);
                target.Player.sendBrowserRequest(description, url);
            }
            target.Player.sendBrowserRequest("", url);
        }
    }
}
