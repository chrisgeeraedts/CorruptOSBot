using CorruptOSBot.Helpers;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace CorruptOSBot
{
    public static class SuggestionInterceptor
    {
        public static async Task NewSuggestionPosted(SocketMessage arg)
        {
            try
            {
                if (RootAdminManager.GetToggleState(nameof(SuggestionInterceptor)))
                {
                    var currentUser = ((SocketGuildUser)arg.Author);
                    if (!string.IsNullOrEmpty(currentUser.Nickname))
                    {
                        string title = string.Format("{0} suggested", currentUser.Nickname);
                        string description = arg.Content;

                        // Post the poll
                        var sent = await arg.Channel.SendMessageAsync(embed: EmbedHelper.CreateDefaultSuggestionEmbed(title, description, currentUser.GetAvatarUrl()));

                        // Add thumbs up emoji
                        var emojiUp = new Emoji("\uD83D\uDC4D");
                        await sent.AddReactionAsync(emojiUp);

                        // Add thumbs down emoji
                        var emojiDown = new Emoji("\uD83D\uDC4E");
                        await sent.AddReactionAsync(emojiDown);

                        // Add thumbs down emoji
                        var emojiQuestion = new Emoji("❓");
                        await sent.AddReactionAsync(emojiQuestion);
                    }
                }

            }
            catch (System.Exception)
            {

            }
            // delete the command posted
            await arg.DeleteAsync();

        }
    }
}
