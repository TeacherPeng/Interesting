using System.Xml.Linq;

namespace Words
{
    internal class WordItem
    {
        public string? Word { get; set; }
        public string? Mean { get; set; }
        public string? Sentence { get; set; }
        public int Weight { get; set; } = 10;
        public bool Skip { get; set; } = false;

        public void InceaseWeight()
        {
            Weight++;
        }

        public void DecreaseWeight()
        {
            if (Weight > 1) Weight--;
        }

        public void ReadFromXml(XElement aXElement)
        {
            Word = aXElement.Attribute(nameof(Word))?.Value ?? "";
            Mean = aXElement.Attribute(nameof(Mean))?.Value ?? "";
            Sentence = aXElement.Attribute(nameof(Sentence))?.Value ?? "";
            Weight = int.Parse(aXElement.Attribute(nameof(Weight))?.Value ?? "10");
            Skip = bool.Parse(aXElement.Attribute(nameof(Skip))?.Value ?? "false");
        }
        public XElement CreateXElement(string aName)
        {
            return new XElement(aName, 
                new XAttribute(nameof(Word), Word ?? ""),
                new XAttribute(nameof(Mean), Mean ?? ""),
                new XAttribute(nameof(Sentence), Sentence ?? ""), 
                new XAttribute(nameof(Weight), Weight), 
                new XAttribute(nameof(Skip), Skip));
        }
    }
}
