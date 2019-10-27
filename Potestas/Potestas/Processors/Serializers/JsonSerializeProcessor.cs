using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Potestas.Interfaces;
using Potestas.Observations;

namespace Potestas.Processors.Serializers
{
    public class JsonSerializeProcessor<T> : SerializeProcessor<T> where T: IEnergyObservation
    {
        public override string Description => "JsonSerializeProcessor";

        public override void OnNext(T value)
        {
            base.OnNext(value);

            using (var reader = new StreamReader(Stream))
            using (var writer = new StreamWriter(Stream))
            {
                string content = string.Empty;

                var settings = new JsonSerializerSettings
                {
                    Converters = {
                        new AbstractConverter<FlashObservation, IEnergyObservation>()
                    }
                };

                if (Stream.Length > 0)
                {
                    content = ReadAllStream(reader).Result;

                    List<T> items;

                    if (content[0] == '[')
                    {
                        items = JsonConvert.DeserializeObject<List<T>>(content, settings);
                        items.Add(value);
                    }
                    else
                    {
                        var item = JsonConvert.DeserializeObject<T>(content, settings);
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

        private class AbstractConverter<TReal, TAbstract> : JsonConverter where TReal : TAbstract
        {
            public override bool CanConvert(Type objectType) => objectType == typeof(TAbstract);

            public override object ReadJson(JsonReader reader, Type type, object value, JsonSerializer jser) => jser.Deserialize<TReal>(reader);

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer jser) => jser.Serialize(writer, value);
        }
    }
}
