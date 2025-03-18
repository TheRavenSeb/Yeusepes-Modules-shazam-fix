﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VRCOSC.App.Utils;

namespace YeusepesModules.SPOTIOSC.Utils.Requests
{
    public class SpotifyProfileRequest : SpotifyRequest
    {
        private const string UserProfileUrl = "https://api.spotify.com/v1/me";

        public SpotifyProfileRequest(HttpClient httpClient, string accessToken, string clientToken)
            : base(httpClient, accessToken, clientToken) { }

        public async Task<UserProfile> GetUserProfileAsync()
        {
            var request = CreateRequest(HttpMethod.Get, UserProfileUrl);
            var response = await SendAsync(request);

            Logger.Log($"User profile response: {response ?? "Response is null or empty"}");

            if (string.IsNullOrEmpty(response))
            {
                throw new Exception("Received null or empty response from the Spotify API.");
            }

            try
            {
                return JsonSerializer.Deserialize<UserProfile>(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException ex)
            {
                Logger.Log($"Deserialization error: {ex.Message}");
                Logger.Log($"Response content: {response}");
                throw;
            }
        }


        public class UserProfile
        {
            [JsonPropertyName("country")]
            public string Country { get; set; }

            [JsonPropertyName("display_name")]
            public string DisplayName { get; set; }

            [JsonPropertyName("email")]
            public string Email { get; set; }

            [JsonPropertyName("explicit_content")]
            public ExplicitContentSettings ExplicitContent { get; set; }

            [JsonPropertyName("external_urls")]
            public ExternalUrls ExternalUrls { get; set; }

            [JsonPropertyName("followers")]
            public Followers Followers { get; set; }

            [JsonPropertyName("href")]
            public string Href { get; set; }

            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("images")]
            public List<ImageObject> Images { get; set; }

            [JsonPropertyName("product")]
            public string Product { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("uri")]
            public string Uri { get; set; }
        }

        public class ExplicitContentSettings
        {
            [JsonPropertyName("filter_enabled")]
            public bool FilterEnabled { get; set; }

            [JsonPropertyName("filter_locked")]
            public bool FilterLocked { get; set; }
        }

        public class ExternalUrls
        {
            [JsonPropertyName("spotify")]
            public string Spotify { get; set; }
        }

        public class Followers
        {
            [JsonPropertyName("href")]
            public string Href { get; set; }

            [JsonPropertyName("total")]
            public int Total { get; set; }
        }

        public class ImageObject
        {
            [JsonPropertyName("url")]
            public string Url { get; set; }

            [JsonPropertyName("height")]
            public int? Height { get; set; }

            [JsonPropertyName("width")]
            public int? Width { get; set; }
        }
    }
}
