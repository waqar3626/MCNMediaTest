namespace AwesomeDal
{
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// The project wide.
    /// </summary>
    internal static class Serializer
    {
        /// <summary>
        /// Serializes a class instance.
        /// </summary>
        /// <param name="value">
        /// the instance of the class to be serialized.
        /// </param>
        /// <typeparam name="T">
        /// A specified type to serialize to XML.
        /// </typeparam>
        /// <returns>
        /// The <see cref="string" /> of XML from the serialized object.
        /// </returns>
        internal static XmlDocument Serialize<T>(this T value)
        {
            var xmlserializer = new XmlSerializer(typeof(T));
            var stringWriter = new StringWriter();
            var writer = XmlWriter.Create(stringWriter);
            xmlserializer.Serialize(writer, value);
            writer.Close();

            var xDoc = new XmlDocument();
            xDoc.LoadXml(stringWriter.ToString());
            return xDoc;
        }
    }
}
