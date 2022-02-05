using System.Drawing;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace ReBot.Core;

internal class PingResponder: IResponder<IMessageCreate>
{
    private readonly IDiscordRestChannelAPI _channelApi;

    public PingResponder(IDiscordRestChannelAPI channelApi)
    {
        _channelApi = channelApi;
    }

    public async Task<Result> RespondAsync(IMessageCreate gatewayEvent, CancellationToken ct = new CancellationToken())
    {
        if (gatewayEvent.Content != "ping")
        {
            return Result.FromSuccess();
        }

        var embed = new Embed(Description: "Pong!", Colour: Color.LawnGreen);
        var replyResult = await _channelApi.CreateMessageAsync
        (
            gatewayEvent.ChannelID,
            embeds: new[] { embed },
            ct: ct
        );

        return !replyResult.IsSuccess
            ? Result.FromError(replyResult)
            : Result.FromSuccess();
    }
}
