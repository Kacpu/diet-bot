using DietBot.ComputerVision;
using DietBot.Diets.Models;
using DietBot.Diets.Service;
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
    private readonly IDietService _dietService;

    public DietDialog(
        UserState userState,
        IComputerVisionService computerVisionService,
        IDietService dietService) : base(nameof(DietDialog))
    {
        _computerVisionService = computerVisionService;
        _dietService = dietService;

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
            RetryPrompt = MessageFactory.Text("The label must be a jpeg/png image file. Please attach a valid label image."),
        };

        return await stepContext.PromptAsync(nameof(AttachmentPrompt), promptOptions, cancellationToken);
    }

    private async Task<DialogTurnResult> DietTypeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        var labelImage = ((IList<Attachment>)stepContext.Result)?.FirstOrDefault();

        if (labelImage is null)
        {

        }

        try
        {
            var extractedText = await _computerVisionService.ExtractText(labelImage.ContentUrl, cancellationToken);
            var isFoodLabel = _dietService.IsFoodLabel(extractedText);
            stepContext.Values["extractedText"] = extractedText;

            if (!isFoodLabel)
            {
                var message = "Sorry, but I think that this is not food label. Please send proper image.";
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(message), cancellationToken);
                return await stepContext.ReplaceDialogAsync(InitialDialogId, cancellationToken: cancellationToken);
            }

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Thank you. Please choose your diet type now."),
                RetryPrompt = MessageFactory.Text("Please choose your diet type."),
                Choices = ChoiceFactory.ToChoices(Enum.GetNames<DietType>()),
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }
        catch (Exception ex)
        {
            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text("Sorry, something went wrong. Please try again."), cancellationToken);

            return await stepContext.ReplaceDialogAsync(InitialDialogId, cancellationToken: cancellationToken);
        }
    }

    private async Task<DialogTurnResult> SummaryStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        stepContext.Values["dietType"] = ((FoundChoice)stepContext.Result).Value;

        //await stepContext.Context.SendActivityAsync(
        //    MessageFactory.Text("Thanks. Please wait while the label image is analyzed."), cancellationToken);

        var extractedText = (string)stepContext.Values["extractedText"];
        var isDietType = Enum.TryParse<DietType>((string)stepContext.Values["dietType"], out var dietType);

        if (extractedText is not null && isDietType)
        {
            try
            {
                var resultMessage = await _dietService.AnalyzeFood(dietType, extractedText);
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(resultMessage), cancellationToken);
            }
            catch (Exception ex)
            {
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text("Sorry, something went wrong. Please try again."), cancellationToken);

                return await stepContext.ReplaceDialogAsync(InitialDialogId, cancellationToken: cancellationToken);
            }
        }

        await stepContext.Context.SendActivityAsync(
            MessageFactory.Text("You can check next food now."), cancellationToken);

        return await stepContext.ReplaceDialogAsync(InitialDialogId, cancellationToken: cancellationToken);

        //return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
    }

    private static Task<bool> LabelImagePromptValidatorAsync(
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
            return Task.FromResult(validImages.Any());
        }

        return Task.FromResult(false);
    }
}
