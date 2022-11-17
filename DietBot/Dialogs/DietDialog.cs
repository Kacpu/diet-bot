﻿using DietBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Bot.Schema;
using System.Linq;

namespace DietBot.Dialogs;

public class DietDialog : ComponentDialog
{
    private readonly IStatePropertyAccessor<Diet> _dietAccessor;

    public DietDialog(UserState userState) : base(nameof(DietDialog))
    {
        _dietAccessor = userState.CreateProperty<Diet>("Diet");

        var waterfallSteps = new WaterfallStep[]
        {
            LabelStepAsync,
            SummaryStepAsync
        };

        AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
        AddDialog(new AttachmentPrompt(nameof(AttachmentPrompt), LabelPromptValidatorAsync));
        AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
        AddDialog(new TextPrompt(nameof(TextPrompt)));
        AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));

        InitialDialogId = nameof(WaterfallDialog);
    }

    private static async Task<DialogTurnResult> LabelStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        var promptOptions = new PromptOptions
        {
            Prompt = MessageFactory.Text("Please attach a label."),
            RetryPrompt = MessageFactory.Text("The attachment must be a jpeg/png image file."),
        };

        return await stepContext.PromptAsync(nameof(AttachmentPrompt), promptOptions, cancellationToken);
    }

    private async Task<DialogTurnResult> SummaryStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        stepContext.Values["label"] = ((IList<Attachment>)stepContext.Result)?.FirstOrDefault();

        await stepContext.Context.SendActivityAsync(MessageFactory.Text("Thanks. Label uploaded."), cancellationToken);

        var label = (Attachment)stepContext.Values["label"];

        if (label is not null)
        {
            try
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(label, "This is your label."), cancellationToken);
            }
            catch
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("A label was saved but could not be displayed here."), cancellationToken);
            }
        }

        return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
    }

    private static async Task<bool> LabelPromptValidatorAsync(PromptValidatorContext<IList<Attachment>> promptContext, CancellationToken cancellationToken)
    {
        if (promptContext.Recognized.Succeeded)
        {
            var attachments = promptContext.Recognized.Value;
            var validImages = new List<Attachment>();

            foreach (var attachment in attachments)
            {
                if (attachment.ContentType == "image/jpeg" || attachment.ContentType == "image/png")
                {
                    validImages.Add(attachment);
                }
            }

            promptContext.Recognized.Value = validImages;

            // If none of the attachments are valid images, the retry prompt should be sent.
            return validImages.Any();
        }
        else
        {
            await promptContext.Context.SendActivityAsync("No attachments received. Proceeding without a profile picture...");

            // We can return true from a validator function even if Recognized.Succeeded is false.
            return true;
        }
    }
}
