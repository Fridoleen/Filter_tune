using System.Collections.Generic;
using System.IO;

namespace FilterTuneWPF_dll
{

    /// <summary>
    /// Class that manages contents of *.filter file and can create new ones
    /// </summary>
    public class FilterFile
    {
        public string OriginalFilterPath { get; private set; }
        public string OriginalFileName { get; private set; }

        public string NewFilterPath { get; set; }

        public string[] FilterContent { get; set; }
        /// <summary>
        /// Creates instance of FilterFile associated with a specified file
        /// </summary>
        /// <param name="filePath"></param>
        public FilterFile(string filePath)
        {
            FileInfo fileInf = new FileInfo(filePath);

            if (fileInf.Exists)
            {
                FilterContent = File.ReadAllLines(filePath);
                OriginalFilterPath = fileInf.DirectoryName;
                OriginalFileName = fileInf.Name;
            }
        }

        /// <summary>
        /// Applies template to the contents of chosen filter
        /// </summary>
        /// <param name="fTemplate"></param>
        public void ApplyTemplate(FilterTemplate fTemplate)
        {
            List<Block> targetBlocks = new List<Block>(FindBlocks(0, fTemplate.Selectors));

            foreach (Block block in targetBlocks)
            {
                ReplaceValuesBlock(block.Start, block.Finish, fTemplate.Parameters);
            }
        }

        public void ApplyTemplates(IEnumerable<FilterTemplate> FTemplates)
        {
            foreach (var FTemplate in FTemplates)
            {
                ApplyTemplate(FTemplate);
            }
        }

        /// <summary>
        /// Returns text "Show" blocks, that are situated after [start] line and contain selectors
        /// </summary>
        /// <param name="start"></param>
        /// <param name="selectors"></param>
        /// <returns></returns>
        private List<Block> FindBlocks(int start, List<StringPair> selectors) // Done
        {
            List<Block> foundBlockList = new List<Block>();

            for (int i = start; i < FilterContent.Length; i++)
            {              
                if (FilterContent[i].Contains("Show"))
                {
                    int endBlock = FilterContent.Length - 1;

                    for (int j = i; j < FilterContent.Length - 1; j++)
                    {
                        if (FilterContent[j].Length < 3)
                        {
                            endBlock = j - 1;
                            break;
                        }
                    }

                    if (CheckBlock(i, endBlock, selectors))
                    {
                        foundBlockList.Add(new Block(i, endBlock));
                    }

                    i = endBlock + 1;
                }
            }

            return foundBlockList;
        }

        private List<Block> FindBlocks(List<StringPair> selectors)
        {
            return FindBlocks(0, selectors);
        }

        /// <summary>
        /// Check if lines with numbers from [start] to [finish] contain selectors
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="selectors"></param>
        /// <returns></returns>
        private bool CheckBlock(int start, int end, List<StringPair> selectors)
        {
            bool result = true;

            foreach (StringPair sel in selectors)
            {
                bool selectorIsPresent = false;

                for (int i = start; i < end; i++)
                {
                    if (FilterContent[i].IndexOf(sel.Name) > -1)
                    {
                        if (FilterContent[i].IndexOf(sel.Value) > -1)
                        {
                            selectorIsPresent = true;
                        }
                        else break;
                    }

                }

                result = result && selectorIsPresent;
            }

            return result;
        }

        /// <summary>
        /// Replaces parameter values for all parameters found in lines ranging numbers from start to finish
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <param name="parameters"></param>
        private void ReplaceValuesBlock(int start, int finish, List<StringPair> parameters)
        {
            for (int i = start; i < finish + 1; i++)
            {
                foreach (StringPair par in parameters)
                {
                    int nameParameterPosition = FilterContent[i].IndexOf(par.Name, 0);

                    if (nameParameterPosition > -1)
                    {
                        int commentaryPosition = FilterContent[i].IndexOf('#', nameParameterPosition);
                        int valueParameterPosition = nameParameterPosition + par.Name.Length + 1;

                        if (commentaryPosition > -1)                                              //if no comment in Line => erase all after parameter name and add parameter value
                        {
                            int commentSize = FilterContent[i].Length - commentaryPosition;

                            FilterContent[i] = FilterContent[i].Remove(valueParameterPosition, commentaryPosition - valueParameterPosition);
                            FilterContent[i].Insert(valueParameterPosition, par.Value + "\t");
                        }
                        else                                                         // else - erase from parameter name to comment beginning, add parameter
                        {
                            FilterContent[i] = FilterContent[i].Remove(valueParameterPosition);
                            FilterContent[i] += par.Value;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Creates new txt file in the folder of original .filter file and writes FilterContent there 
        /// </summary>
        /// <param name="newFilterPath"></param>
        public void CreateNewFilter(string newFilterPath) 
        {
            File.WriteAllLines(newFilterPath, FilterContent);
        }
    }
}
//In some cases BaseType parameter list is of the size of a MULTIPLE LINES - this may cause some problems
