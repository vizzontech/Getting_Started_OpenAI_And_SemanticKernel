using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.ComponentModel;
using System.Text.Json.Serialization;

string _openAIModal = "gpt-3.5-turbo";
string _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User);

var builder = Kernel.CreateBuilder();
builder.Services.AddLogging(b => b.AddConsole().SetMinimumLevel(LogLevel.Trace));
Kernel kernel = builder
                .AddOpenAIChatCompletion(_openAIModal, _apiKey)
                .Build();


kernel.ImportPluginFromType<SpeakerSearchPlugin>();
var settings = new OpenAIPromptExecutionSettings() { ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions };
var chatService = kernel.GetRequiredService<IChatCompletionService>();
var chat = new ChatHistory();

while (true)
{
    Console.Write("Q: ");
    chat.AddUserMessage(Console.ReadLine());

    var r = await chatService.GetChatMessageContentAsync(chat, settings, kernel);
    Console.WriteLine(r);
    chat.Add(r);
}

class SpeakerSearchPlugin
{
    private readonly IList<Speaker> _speakers = new List<Speaker>()
    {
        new Speaker()
        {
            Name ="Dave Ramsey",
            Bio = "David Ramsey is an American financial author, radio host, television personality, and motivational speaker. His show and writings strongly focus on encouraging people to get out of debt.",
            WebSite = "http://www.daveramsey.com"
        },
        new Speaker()
        {
            Name ="Tony Robbins",
            Bio = "Tony Robbins is an American motivational speaker, personal finance instructor, and self-help author. He became well known from his infomercials and self-help books: Unlimited Power, Unleash the Power Within and Awaken the Giant Within.",
            WebSite = "http://www.tonyrobbins.com"
        },
        new Speaker()
        {
            Name ="Nick Vujicic",
            Bio = "Nick Vujicic is an Australian Christian evangelist and top motivational speaker born with Phocomelia, a rare disorder characterised by the absence of legs and arms.",
            WebSite = "http://www.nickvujicic.com"
        },
        new Speaker()
        {
            Name ="Eckhart Tolle",
            Bio = "Eckhart Tolle is a German-born resident of Canada, best known as the author of The Power of Now and A New Earth: Awakening to your Life’s Purpose. In 2011, he was listed by Watkins Review as the most spiritually influential person in the world.",
            WebSite = "http://www.eckharttolle.com"
        },
        new Speaker()
        {
            Name ="Louise Hay",
            Bio = "Louise Lynn Hay was an American motivational author and the founder of Hay House, she authored several New Thought self-help books, including the 1984 book, You Can Heal Your Life. Louise Hay died on the morning of August 30, 2017 at age 90.",
            WebSite = "http://www.louisehay.com"
        },
        new Speaker()
        {
            Name ="Chris Gardner",
            Bio = "Chris Gardner is an American entrepreneur, investor, stockbroker, motivational speaker, author, and philanthropist who, during the early 1980s, struggled with homelessness while raising his toddler son. Gardner’s book of memoirs, The Pursuit of Happyness, was published in May 2006.",
            WebSite = "http://www.chrisgardnermedia.com"
        },
        new Speaker()
        {
            Name ="Robert Kiyosaki",
            Bio = "Robert Kiyosaki is an American businessman, investor, self-help author, educator, motivational speaker, financial literacy activist, financial commentator, and radio personality. Kiyosaki is the founder of the Rich Dad Company, who provide financial and business education to people through books, videos, games, seminars, blogs, coaching, and workshops.",
            WebSite = "http://www.richdad.com"
        },
        new Speaker()
        {
            Name ="Eric Thomas",
            Bio = "Eric Thomas is an American motivational speaker, author and minister. After becoming known as a motivational speaker, Thomas founded a company to offer education consulting, executive coaching and athletic development, including athletes such as Lebron James.",
            WebSite = "http://www.etinspires.com"
        },
        new Speaker()
        {
            Name ="Les Brown",
            Bio = "Leslie Brown is an American motivational speaker, author, radio DJ, former television host, and former politician as a member of the Ohio House of Representatives. As a motivational speaker, he uses the catch phrase “it’s possible!” and teaches people to follow their dreams as he learned to do.",
            WebSite = "http://www.lesbrown.com"
        },
        new Speaker()
        {
            Name ="Suze Orman",
            Bio = "Suze Orman is an American author, financial advisor, motivational speaker, and television host. In 1987, she founded the Suze Orman Financial Group. Her program The Suze Orman Show began airing on CNBC in 2002 and won a Gracie Award for Outstanding Program Host on CNBC for it. She has written several books on the topic of personal finance.",
            WebSite = "http://www.suzeorman.com"
        },
    };


    [KernelFunction("get_speakers")]
    [Description("Gets a list of speakers")]
    [return: Description("List of speakers")]
    public IList<Speaker> GetSpeakers()
    {
        return _speakers;
    }


    [KernelFunction("get_speaker_by_id")]
    [Description("Get speaker by id")]
    [return: Description("A speaker")]
    public Speaker GetSpeaker(int id)
    {
        return _speakers.FirstOrDefault(x=> x.Id == id);
    }


    [KernelFunction("get_speaker_by_name")]
    [Description("Get speaker by name")]
    [return: Description("A speaker")]
    public Speaker GetSpeaker(string name)
    {
        return _speakers.FirstOrDefault(x => x.Name == name);
    }


    [KernelFunction("get_speaker_by_search_term")]
    [Description("Get speaker by search term")]
    [return: Description("List of speakers")]
    public List<Speaker> GetSpeakers(string searchTerm)
    {
     return  
            _speakers
            .Where(x => searchTerm.Contains(x.Name))
            .Where(x => searchTerm.Contains(x.Bio))
            .Where(x=> searchTerm.Contains(x.WebSite))
            .ToList();
    }
}


public class Speaker
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("bio")]
    public string Bio { get; set; }

    [JsonPropertyName("webSite")]
    public string WebSite { get; set; }
}
