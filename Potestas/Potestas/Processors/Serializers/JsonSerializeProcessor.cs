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
                string jsonContent;
                if (Stream.Length > 0)
                {
                    jsonContent = ReadAllStream(reader).Result;

                    List<T> items;

                    var settings = new JsonSerializerSettings
                    {
                        Converters = {
                            new AbstractConverter<FlashObservation, IEnergyObservation>()
                        }
                    };

                    if (jsonContent[0] == '[')
                    {
                        items = JsonConvert.DeserializeObject<List<T>>(jsonContent, settings);
                        items.Add(value);
                    }
                    else
                    {
                        var item = JsonConvert.DeserializeObject<T>(jsonContent, settings);
                        items = new List<T>
                        {
                            item,
                            value
                        };
                    }

                    jsonContent = JsonConvert.SerializeObject(items, Formatting.Indented);
                }
                else
                {
                    jsonContent = JsonConvert.SerializeObject(value, Formatting.Indented);
                }

                WriteToStream(writer, jsonContent).Wait();
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
