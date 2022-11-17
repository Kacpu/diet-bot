using DietBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace DietBot.Dialogs;

public class DietDialog : ComponentDialog
{
    private readonly IStatePropertyAccessor<Diet> _dietAccessor;

    public DietDialog(UserState userState) : base(nameof(DietDialog))
    {
        _dietAccessor = userState.CreateProperty<Diet>("Diet");

        var waterfallSteps = new WaterfallStep[]
        {

        };

        AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
        //AddDialog(new AttachmentPrompt(nameof(AttachmentPrompt), PicturePromptValidatorAsync));
        AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

        InitialDialogId = nameof(WaterfallDialog);
    }
}
