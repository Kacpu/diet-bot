using Microsoft.Bot.Schema;

namespace DietBot.Models;

public class Diet
{
    public string Kind { get; set; }

    public Attachment Label { get; set; }
}
