using FilterTuneWPF_dll;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace FilterTuneLibrary
{

    public class TemplateList
    {
        public List<FilterTemplate> Templates;
        public TemplateList() 
        {
            Templates = new List<FilterTemplate>();
            Templates.Add(new FilterTemplate("Default"));
        }
        public IEnumerable<string> Names => Templates.Select(x => x.Name);

        /// <summary>
        /// Creates instance of class from FilterTemplate list
        /// </summary>
        /// <param name="tempList"></param>
        public TemplateList(List<FilterTemplate> tempList)
        {
            this.Templates = tempList;
        }

        /// <summary>
        /// Adds FilterTemplate to the list
        /// </summary>
        /// <param name="temp"></param>
        public void AddTemplate(FilterTemplate temp)
        {
            Templates.Add(temp);
        }

        /// <summary>
        /// Applies all templates in list to the contents of .filter file saved in FilterFile instance
        /// </summary>
        /// <param name="file"></param>
        public void ApplyTemplates(FilterFile file)
        {
            foreach (FilterTemplate temp in Templates) temp.ApplyTemplate(file);
        }

        /// <summary>
        /// Returns true if template was found and removed, elswise returns false
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public bool RemoveTemplate(FilterTemplate temp)
        {
            return Templates.Remove(temp);
        }
    }
}
