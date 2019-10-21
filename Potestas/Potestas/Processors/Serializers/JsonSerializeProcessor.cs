using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Potestas.Interfaces;

namespace Potestas.Processors.Serializers
{
    public class JsonSerializeProcessor<T> : SerializeProcessor<T> where T: IEnergyObservation
    {
        private readonly DataContractJsonSerializer _jsonSerializer;

        public JsonSerializeProcessor(Stream stream) : base(stream)
        {
            _jsonSerializer = new DataContractJsonSerializer(typeof(T));
        }

        public override void OnNext(T value)
        {
            base.OnNext(value);

            using (var reader = new StreamReader(Stream))
            using (var writer = new StreamWriter(Stream))
            {
                string content;
                if (Stream.Length > 0)
                {
                    content = this.ReadAllStream(reader).Result;

                    List<T> items;
                    T item;

                    if (content[0] == '[')
                    {
                        items = JsonConvert.DeserializeObject<List<T>>(content);
                        items.Add(value);
                    }
                    else
                    {
                        item = JsonConvert.DeserializeObject<T>(content);
                        items = new List<T>
                        {
                            item,
                            value
                        };
                    }

                    content = JsonConvert.SerializeObject(items, Formatting.Indented);
                }
                else
                {
                    content = JsonConvert.SerializeObject(value, Formatting.Indented);
                }

                WriteToStream(writer, content).Wait();
            }
        }

        public override string Description => "Json serialize processor";
    }
}
