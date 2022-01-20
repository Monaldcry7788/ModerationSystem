namespace ModerationSystem.Discord
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using MEC;
    using ModerationSystem.Discord.Discord;
    using UnityEngine.Networking;
    using Utf8Json;

    public static class DiscordHandler
    {
        public static IEnumerator<float> SendMessage(string content, string url)
        {
            UnityWebRequest webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            UploadHandlerRaw uploadHandler = new UploadHandlerRaw(JsonSerializer.Serialize(new Message(content)))
            {
                contentType = "application/json"
            };
            webRequest.uploadHandler = uploadHandler;

            yield return Timing.WaitUntilDone(webRequest.SendWebRequest());

            if (webRequest.isNetworkError || webRequest.isHttpError)
                Log.Error($"An error occurred while sending log message: {webRequest.responseCode}\n{webRequest.error}");
        }
    }
}