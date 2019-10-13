using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Potestas.Interfaces;

namespace Potestas.Processors.Serializers
{
    public class JsonSerializeProcessor<T> : SerializeProcessor<T> where T: IEnergyObservation
    {
        private readonly DataContractJsonSerializer _jsonSerializer;

        public JsonSerializeProcessor()
        {
            _jsonSerializer = new DataContractJsonSerializer(typeof(T));
        }

        public override void OnNext(T value)
        {
            base.OnNext(value);

            var content = string.Empty;

            if (Stream.Length > 0)
            {
                content = ReadAllStream().Result;
                var items = JsonConvert.DeserializeObject<List<T>>(content);
                items.Add(value);
                content = JsonConvert.SerializeObject(items);
            }
            else
            {
                content = JsonConvert.SerializeObject(value);
            }

            WriteToStream(content).Wait();
        }

        public override string Description => "Json serialize processor";
    }
}
