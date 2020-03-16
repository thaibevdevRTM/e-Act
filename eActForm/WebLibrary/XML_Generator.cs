using System;
using System.Xml;

namespace WebLibrary
{
    /// <summary>
    /// Summary description for SiteMapFeedGenerator.
    /// </summary>
    public class XML_Generator
    {
        XmlTextWriter writer;
        public XML_Generator(System.IO.Stream stream, System.Text.Encoding encoding)
        {
            writer = new XmlTextWriter(stream, encoding);
            writer.Formatting = Formatting.Indented;
        }
        public XML_Generator(System.IO.TextWriter w)
        {
            writer = new XmlTextWriter(w);
            writer.Formatting = Formatting.Indented;
        }
        /// &lt;summary&gt;
        /// Writes the beginning of the SiteMap document
        /// &lt;/summary&gt;
        public void WriteStartDocument(string rootName)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement(rootName);
            //writer.WriteAttributeString("xmlns","");
        }
        /// &lt;summary&gt;
        /// Writes the end of the SiteMap document
        /// &lt;/summary&gt;
        public void WriteEndDocument()
        {
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }
        /// &lt;summary&gt;
        /// Closes this stream and the underlying stream
        /// &lt;/summary&gt;
        public void Close()
        {
            writer.Flush();
            writer.Close();
        }
        public void BeginWriteItem(string attributesName)
        {

            writer.WriteStartElement(attributesName);
        }
        public void WriteItem(string itemName, string values)
        {
            writer.WriteElementString(itemName, values);
        }
        public void EndWiteItem()
        {
            writer.WriteEndElement();
        }
        public string formatDate(DateTime d)
        {
            return d.ToString("s") + "+00:00";
        }
    }
}

/*
 				XML_Generator portal = new XML_Generator(Response.Output);
				portal.WriteStartDocument();
				portal.BeginWriteItem();
				portal.WriteItem("IsSuccess",response.IsSuccess.ToString());
				portal.WriteItem("Code",response.Code);
				portal.WriteItem("Description",response.Description);
				portal.WriteItem("TransactionID",response.TransactionID);
				portal.WriteItem("OrderRef",response.OrderRef);
				portal.EndWiteItem();
				portal.WriteEndDocument();
				portal.Close();
 
 */
