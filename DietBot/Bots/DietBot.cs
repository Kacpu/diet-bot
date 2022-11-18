using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Logging;
using DietBot.Dialogs;

namespace DietBot.Bots;

public class DietBot : ActivityHandler
{
    protected readonly Dialog _dialog;
    protected readonly BotState _conversationState;
    protected readonly BotState _userState;
    protected readonly ILogger _logger;

    public DietBot(ConversationState conversationState, UserState userState, DietDialog dialog, ILogger<DietBot> logger)
    {
        _dialog = dialog;
        _conversationState = conversationState;
        _userState = userState;
        _logger = logger;
    }

    public override async Task OnTurnAsync(
        ITurnContext turnContext,
        CancellationToken cancellationToken = default)
    {
        await base.OnTurnAsync(turnContext, cancellationToken);

        // Save any state changes that might have occurred during the turn.
        await _conversationState.SaveChangesAsync(turnContext, false, cancellationToken);
        await _userState.SaveChangesAsync(turnContext, false, cancellationToken);
    }

    protected override async Task OnMembersAddedAsync(
        IList<ChannelAccount> membersAdded,
        ITurnContext<IConversationUpdateActivity> turnContext,
        CancellationToken cancellationToken = default)
    {
        var welcomeText = "Hello and welcome!";
        foreach (var member in membersAdded)
        {
            if (member.Id != turnContext.Activity.Recipient.Id)
            {
                await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
            }
        }

        await _dialog.RunAsync(turnContext, _conversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken);
    }

    protected override async Task OnMessageActivityAsync(
        ITurnContext<IMessageActivity> turnContext,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Running dialog with Message Activity.");

        await _dialog.RunAsync(turnContext, _conversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken);
    }
}
