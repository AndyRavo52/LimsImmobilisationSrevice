using System.Text.Json;
using System.Text.Json.Serialization;

namespace LimsImmobilisationService.Utils
{
    public class ApiResponse
    {
        public void HandleResponse<T>()
        {
            if (this?.Data != null)
            {
                var jsonString = JsonSerializer.Serialize(this.Data);
                var result = JsonSerializer.Deserialize<T>(jsonString);
                if (result != null)
                {
                    this.Data = result;
                }
            }
        }

        [JsonPropertyName("data")]
        public object? Data { get; set; }

        [JsonPropertyName("viewBag")]
        public Dictionary<string, object>? ViewBag { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }
    }
}