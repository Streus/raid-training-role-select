using Newtonsoft.Json;

class Receipt {
    [JsonProperty(PropertyName = "seed")]
    public string Seed { get; set; } = "";

    [JsonProperty(PropertyName = "form")]
    public Form Form { get; set; } = new();

    [JsonProperty(PropertyName = "members")]
    public Member[][] Members { get; set; } = Array.Empty<Member[]>();

    [JsonProperty(PropertyName = "guaranteeds")]
    public string[] Guaranteeds { get; set; } = Array.Empty<string>();

    public class Member {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; } = string.Empty;

        public Member(string id, string role) {
            ID = id;
            Role = role;
        }

        public override string ToString() => $"{ID} as {Role}";
    }
}