using System.Collections.Generic;
using System.IO;

namespace FilterTuneWPF_dll
{

    /// <summary>
    /// Manages contents of *.filter file and can create new ones
    /// </summary>
    public class FilterFile
    {
        public string OriginalFilterPath { get; private set; }
        public string OriginalFileName { get; private set; }
        public string NewFilterPath { get; set; }

        private List<Block> blocks;

        /// <summary>
        /// Creates instance of FilterFile associated with a specified file
        /// </summary>
        /// <param name="filePath"></param>
        public FilterFile(string filePath)
        {
            string[] FilterContent;
            FileInfo fileInf = new FileInfo(filePath);

            if (fileInf.Exists) // SINGLE RESPONSIBILITY PRINCIPLE VIOLATED???
            {
                FilterContent = File.ReadAllLines(filePath);
                OriginalFilterPath = fileInf.DirectoryName;
                OriginalFileName = fileInf.Name;
                blocks = new List<Block>();
                ParseBlocks(FilterContent);
            }
        } 


        //Check how \n works in string so that there is no double symbol \n
        private void ParseBlocks(string[] FileContent)
        {
            var block = new Block(0, 0);

            for (int i = 0; i < FileContent.Length; i++)
            {
                if (FileContent[i] == "\n")
                {
                    if (block.Start == i) block.Start++;
                    else
                    {
                        blocks.Add(block);
                        block = new Block(i + 1, i + 1);
                    }
                }
                else
                {
                    block.AddContents(FileContent[i] + "\n");
                    block.Finish++;
                }
            }
        }

        /// <summary>
        /// Applies template to the contents of chosen filter
        /// </summary>
        /// <param name="fTemplate"></param>
        public void ApplyTemplate(FilterTemplate fTemplate)
        {
            List<int> targetBlocks = new List<int>(FindBlocks(fTemplate.Selectors));

            foreach (int blockNumber in targetBlocks)
            {
                SetValuesAtBlock(blockNumber, fTemplate.Parameters);
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
        /// Returns numbers of all blocks with "Show", contain selectors
        /// </summary>
        /// <param name="selectors"></param>
        /// <returns></returns>
        private List<int> FindBlocks(List<StringPair> selectors) 
        {
            List<int> foundBlockList = new List<int>();

            for(int i = 0; i < blocks.Count; i++)
            {              
                if (blocks[i].Contents.Substring(0, 10).Contains("Show"))
                {
                    if (CheckBlock(i, selectors))
                    {
                        foundBlockList.Add(i);
                    }
                }
            }

            return foundBlockList;
        }

        /// <summary>
        /// Check if lines with numbers from [start] to [finish] contain selectors
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="selectors"></param>
        /// <returns></returns>
        private bool CheckBlock(int blockNumber, List<StringPair> selectors)
        {
            bool result = true;
            string[] BlockLines = blocks[blockNumber].Contents.Split('\n');

            foreach (StringPair sel in selectors)
            {
                bool selectorIsPresent = false;

                for (int i = 0; i < BlockLines.Length; i++)
                {
                    if (BlockLines[i].IndexOf(sel.Name) > -1)
                    {
                        if (BlockLines[i].IndexOf(sel.Value) > -1)
                        {
                            selectorIsPresent = true;
                        }
                        else break;
                    }
                }

                result = result && selectorIsPresent;
                if (!result) break;
            }

            return result;
        }

        /// <summary>
        /// Replaces parameter values for all parameters found in lines ranging numbers from start to finish
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <param name="parameters"></param>
        private void SetValuesAtBlock(int blockNumber, List<StringPair> parameters)
        {
            string modifiedBlock = blocks[blockNumber].Contents;

            foreach (StringPair par in parameters)
            {
                int paramPosition = modifiedBlock.IndexOf(par.Name, 0);

                if (paramPosition > -1)
                {
                    int comm = CommentStart(paramPosition, modifiedBlock);
                    int valueStartPosition = paramPosition + par.Name.Length + 1;

                    // erase all line contents after parameter till \n or comment and add parameter value
                    if (comm> -1)                                              
                    {
                        modifiedBlock.Remove(valueStartPosition, comm);
                        modifiedBlock.Insert(valueStartPosition, par.Value);
                    }
                    else                                                         
                    {
                        modifiedBlock.Remove(valueStartPosition, modifiedBlock.IndexOf('\n', valueStartPosition));
                        modifiedBlock.Insert(valueStartPosition, par.Value);
                    }
                }
                else
                {
                    modifiedBlock += $"{par.Name} {par.Value}\n";
                }                
            }

            blocks[blockNumber].Contents = modifiedBlock;
        }

        private int CommentStart(int start, string str)
        {
            int index = str.IndexOf('\n', start);
            index = (index > -1) ? index : str.Length;
            return str.IndexOf('#', start, index - start);
        }

        /// <summary>
        /// Creates new txt file in the folder of original .filter file and writes FilterContent there 
        /// </summary>
        /// <param name="newFilterPath"></param>
        public void CreateNewFilter(string newFilterPath) 
        {
            var FilterContent = new List<string>();

            foreach(var block in blocks)
            {
                FilterContent.Add(block.Contents);
            }

            File.WriteAllLines(newFilterPath, FilterContent);
        }
    }
}
//In some cases BaseType parameter list is of the size of a MULTIPLE LINES - this may cause some problems
