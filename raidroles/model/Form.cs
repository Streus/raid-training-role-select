using Newtonsoft.Json;

class Form {
    [JsonProperty(PropertyName = "trainers")]
    public string[] Trainers { get; set; } = Array.Empty<string>();

    [JsonProperty(PropertyName = "guaranteed")]
    public string[] Guaranteed { get; set; } = Array.Empty<string>();

    [JsonProperty(PropertyName = "primary")]
    public _PrimaryRoleApplicants PrimaryRoleApplicants { get; set; } = new();

    [JsonProperty(PropertyName = "backup")]
    public _BackupRoleApplicants BackupRoleApplicants { get; set; } = new();

    [JsonProperty(PropertyName = "mechanics")]
    public _MechanicsApplicants MechanicsApplicants { get; set; } = new();

    public class _PrimaryRoleApplicants {
        [JsonProperty(PropertyName = "qHeal")]
        public string[] QHeal { get; set; } = Array.Empty<string>();

        [JsonProperty(PropertyName = "aHeal")]
        public string[] AHeal { get; set; } = Array.Empty<string>();

        [JsonProperty(PropertyName = "qDps")]
        public string[] QDps { get; set; } = Array.Empty<string>();

        [JsonProperty(PropertyName = "aDps")]
        public string[] ADps { get; set; } = Array.Empty<string>();

        [JsonProperty(PropertyName = "dps")]
        public string[] Dps { get; set; } = Array.Empty<string>();
    }

    public class _BackupRoleApplicants {
        [JsonProperty(PropertyName = "heal")]
        public string[] BkHeal { get; set; } = Array.Empty<string>();

        [JsonProperty(PropertyName = "boon")]
        public string[] BkBoon { get; set; } = Array.Empty<string>();

        [JsonProperty(PropertyName = "dps")]
        public string[] BkDps { get; set; } = Array.Empty<string>();
    }

    public class _MechanicsApplicants {
        [JsonProperty(PropertyName = "tank")]
        public string[] Tank { get; set; } = Array.Empty<string>();
    }
}
