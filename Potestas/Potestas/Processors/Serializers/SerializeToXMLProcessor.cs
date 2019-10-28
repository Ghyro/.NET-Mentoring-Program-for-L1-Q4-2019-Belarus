using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Potestas.Interfaces;
using Potestas.Observations;

namespace Potestas.Processors.Serializers
{
    public class SerializeToXMLProcessor<T> : SerializeProcessor<T> where T : IEnergyObservation
    {
        private readonly XmlSerializer _xmlSerializer;
        private readonly XmlSerializer _xmlSerializerCollection;
        private FlashObservation _firstItem;

        public SerializeToXMLProcessor()
        {
            _firstItem = new FlashObservation();
            _xmlSerializer = new XmlSerializer(typeof(FlashObservation), new XmlRootAttribute("Observations"));
            _xmlSerializerCollection = new XmlSerializer(typeof(List<FlashObservation>), new XmlRootAttribute("Observations"));
        }

        public override string Description => "SerializeToXMLProcessor";

        public override void OnNext(T value)
        {
            base.OnNext(value);

            using (var reader = new StreamReader(Stream))
            using (var writer = new StreamWriter(Stream))
            {
                var newObservation = (FlashObservation)(object)value;

                if (Stream.Length > 0)
                {
                    var items = (List<FlashObservation>)_xmlSerializerCollection.Deserialize(reader);

                    if (items.Count >= 2)
                    {
                        items.Add(newObservation);
                        Stream.SetLength(0);
                        _xmlSerializerCollection.Serialize(writer, items);
                    }
                    else if (items.Count == 0)
                    {
                        items.Add(_firstItem);
                        items.Add(newObservation);
                        Stream.SetLength(0);
                        _xmlSerializerCollection.Serialize(writer, items);
                    }                    
                }
                else
                {
                    _xmlSerializer.Serialize(writer, newObservation);
                    _firstItem = (FlashObservation)(object)value;
                }              
            }
        }
    }
}
