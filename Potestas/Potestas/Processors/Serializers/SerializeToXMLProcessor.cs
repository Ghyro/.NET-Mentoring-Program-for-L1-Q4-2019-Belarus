using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Potestas.Interfaces;
using Potestas.Observations;

namespace Potestas.Processors.Serializers
{
    public class SerializeToXMLProcessor<T> : SerializeProcessor<T> where T : IEnergyObservation
    {
        //
    }
}
