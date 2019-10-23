using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using Potestas.Interfaces;

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
                    T item;

                    if (jsonContent[0] == '[')
                    {
                        items = JsonConvert.DeserializeObject<List<T>>(jsonContent);
                        items.Add(value);
                    }
                    else
                    {
                        item = JsonConvert.DeserializeObject<T>(jsonContent);
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
    }
}
