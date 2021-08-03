using Newtonsoft.Json;
using Lapka.Identity.Application.Services;

namespace Lapka.Identity.Infrastructure.Services
{
    public class CurrentJsonSerializer : IJsonSerializer
    {
        public string Serialize(object instance)
            => JsonConvert.SerializeObject(instance);

        public TResult Deserialize<TResult>(string value)
            => JsonConvert.DeserializeObject<TResult>(value);
    }
}