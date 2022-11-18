using DietBot.ComputerVision;
using DietBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DietBot.Dialogs;

public class DietDialog : ComponentDialog
{
    //private readonly IStatePropertyAccessor<Diet> _dietAccessor;
    private readonly IComputerVisionService _computerVisionService;

    public DietDialog(UserState userState, IComputerVisionService computerVisionService) : base(nameof(DietDialog))
    {
        _computerVisionService = computerVisionService;

        //_dietAccessor = userState.CreateProperty<Diet>("Diet");

        var waterfallSteps = new WaterfallStep[]
        {
            LabeImagelStepAsync,
            DietTypeStepAsync,
            SummaryStepAsync
        };

        AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
        AddDialog(new AttachmentPrompt(nameof(AttachmentPrompt), LabelImagePromptValidatorAsync));
        AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

        InitialDialogId = nameof(WaterfallDialog);
    }

    private static async Task<DialogTurnResult> LabeImagelStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        var promptOptions = new PromptOptions
        {
            Prompt = MessageFactory.Text("Please attach a label image."),
            RetryPrompt = MessageFactory.Text("The label must be a jpeg/png image file."),
        };

        return await stepContext.PromptAsync(nameof(AttachmentPrompt), promptOptions, cancellationToken);
    }

    private static async Task<DialogTurnResult> DietTypeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        stepContext.Values["labelImage"] = ((IList<Attachment>)stepContext.Result)?.FirstOrDefault();

        var promptOptions = new PromptOptions
        {
            Prompt = MessageFactory.Text("Thank you. Please choose your diet type now."),
            Choices = ChoiceFactory.ToChoices(Enum.GetNames<DietType>()),
        };

        return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
    }

    private async Task<DialogTurnResult> SummaryStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        stepContext.Values["dietType"] = ((FoundChoice)stepContext.Result).Value;

        await stepContext.Context.SendActivityAsync(
            MessageFactory.Text("Thanks. Please wait while the label image is analyzed."), cancellationToken);

        var labelImage = (Attachment)stepContext.Values["labelImage"];
        var isDietType = Enum.TryParse<DietType>((string)stepContext.Values["dietType"], out var dietType);

        if (labelImage is not null && isDietType)
        {
            try
            {
                var extractedText = await _computerVisionService.ExtractText(labelImage.ContentUrl, cancellationToken);
                //await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(label, "This is your label."), cancellationToken);
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(extractedText), cancellationToken);
            }
            catch
            {
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text("A label was saved but could not be displayed here."), cancellationToken);
            }
        }

        return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
    }

    private static async Task<bool> LabelImagePromptValidatorAsync(
        PromptValidatorContext<IList<Attachment>> promptContext,
        CancellationToken cancellationToken)
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
            //await promptContext.Context.SendActivityAsync(
            //    "No attachments received. Proceeding without a profile picture...",
            //    cancellationToken: cancellationToken);

            // We can return true from a validator function even if Recognized.Succeeded is false.
            return false;
        }
    }
}
