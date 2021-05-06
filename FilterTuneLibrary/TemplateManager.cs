using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using FilterTuneLibrary;

namespace FilterTuneWPF_dll
{
    // IMPLEMENT EXCEPTIONS CONCERNING FILES !!!

    /// <summary>
    /// Contains functionality for extracting TemplateLists from and saving to a file, specified in constructor
    /// </summary>
    public class TemplateManager
    {
        public string TempFilePath { get; private set; }
        private static XmlSerializer formatter;

        /// <summary>
        /// Creates instance of TemplateManager associated with specific file
        /// </summary>
        /// <param name="filePath"></param>
        public TemplateManager(string filePath)
        {
            TempFilePath = filePath;
            formatter = new XmlSerializer(typeof(TemplateList));
        }

        /// <summary>
        /// Deserializes list of FilterTemplates from XML file, if it exists, elsewise returns empty container
        /// </summary>
        public TemplateList GetTemplates()    
        {
            FileInfo file = new FileInfo(TempFilePath);

            if (!file.Exists || file.Length == 0)
            {
                TemplateList templateList = new TemplateList();
                return templateList;
            }
            else
            {
                using (FileStream fs = new FileStream(TempFilePath, FileMode.OpenOrCreate))
                {
                    return (TemplateList)formatter.Deserialize(fs);
                }
            }
        }
        
        /// <summary>
        /// Saves Templates list to XML file
        /// </summary>
        public void SaveTemplates(TemplateList templateList)
        {
            using( FileStream fs = new FileStream(TempFilePath, FileMode.Create))
            {
                formatter.Serialize(fs, templateList);
            }
        }
    }
}
