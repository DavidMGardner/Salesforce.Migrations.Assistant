using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Salesforce.Migrations.Assistant.Library
{
    public class XmlOutput
    {
        // The internal XmlDocument that holds the complete structure.
        XmlDocument xd = new XmlDocument();

        // A stack representing the hierarchy of nodes added. nodeStack.Peek() will always be the current node scope.
        Stack<XmlNode> nodeStack = new Stack<XmlNode>();

        // Whether the next node should be created in the scope of the current node.
        bool nextNodeWithin;

        // The current node. If null, the current node is the XmlDocument itself.
        XmlNode currentNode;

        /// <summary>
        /// Returns the string representation of the XmlDocument.
        /// </summary>
        /// <returns>A string representation of the XmlDocument.</returns>
        public string GetOuterXml()
        {
            return xd.OuterXml;
        }

        /// <summary>
        /// Returns the XmlDocument
        /// </summary>
        /// <returns></returns>
        public XmlDocument GetXmlDocument()
        {
            return xd;
        }

        /// <summary>
        /// Changes the scope to the current node.
        /// </summary>
        /// <returns>this</returns>
        public XmlOutput Within()
        {
            nextNodeWithin = true;

            return this;
        }

        /// <summary>
        /// Changes the scope to the parent node.
        /// </summary>
        /// <returns>this</returns>
        public XmlOutput EndWithin()
        {
            if (nextNodeWithin)
                nextNodeWithin = false;
            else
                nodeStack.Pop();

            return this;
        }

        /// <summary>
        /// Adds an XML declaration with the most common values.
        /// </summary>
        /// <returns>this</returns>
        public XmlOutput XmlDeclaration() { return XmlDeclaration("1.0", "utf-8", ""); }

        /// <summary>
        /// Adds an XML declaration to the document.
        /// </summary>
        /// <param name="version">The version of the XML document.</param>
        /// <param name="encoding">The encoding of the XML document.</param>
        /// <param name="standalone">Whether the document is standalone or not. Can be yes/no/(null || "").</param>
        /// <returns>this</returns>
        public XmlOutput XmlDeclaration(string version, string encoding, string standalone)
        {
            XmlDeclaration xdec = xd.CreateXmlDeclaration(version, encoding, standalone);
            xd.AppendChild(xdec);

            return this;
        }

        /// <summary>
        /// Creates a node. If no nodes have been added before, it'll be the root node, otherwise it'll be appended as a child of the current node.
        /// </summary>
        /// <param name="name">The name of the node to create.</param>
        /// <returns>this</returns>
        public XmlOutput Node(string name)
        {
            XmlNode xn = xd.CreateElement(name);

            // If nodeStack.Count == 0, no nodes have been added, thus the scope is the XmlDocument itself.
            if (nodeStack.Count == 0)
            {
                xd.AppendChild(xn);

                // Automatically change scope to the root DocumentElement.
                nodeStack.Push(xn);
            }
            else
            {
                // If this node should be created within the scope of the current node, change scope to the current node before adding the node to the scope element.
                if (nextNodeWithin)
                {
                    nodeStack.Push(currentNode);

                    nextNodeWithin = false;
                }

                nodeStack.Peek().AppendChild(xn);
            }

            currentNode = xn;

            return this;
        }

        /// <summary>
        /// Sets the InnerText of the current node without using CData.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public XmlOutput InnerText(string text)
        {
            return InnerText(text, false);
        }

        /// <summary>
        /// Sets the InnerText of the current node.
        /// </summary>
        /// <param name="text">The text to set.</param>
        /// <returns>this</returns>
        public XmlOutput InnerText(string text, bool useCData)
        {
            if (useCData)
                currentNode.AppendChild(xd.CreateCDataSection(text));
            else
                currentNode.AppendChild(xd.CreateTextNode(text));

            return this;
        }

        /// <summary>
        /// Adds an attribute to the current node.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <returns>this</returns>
        public XmlOutput Attribute(string name, string value)
        {
            XmlAttribute xa = xd.CreateAttribute(name);
            xa.Value = value;

            currentNode.Attributes.Append(xa);

            return this;
        }
    }
}
