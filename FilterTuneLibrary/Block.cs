using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterTuneWPF_dll
{
    /// <summary>
    /// Range of int numbers, used to describe parts of string array, that meet specified conditions
    /// </summary>
    public class Block
    {
        public string Contents { get; set; }
        public int Start { get; set; }
        public int Finish { get; set; }

        public Block(int _st, int _fn)
        {            
            Start = _st;
            Finish = _fn;
        }

        public Block(int _st, int _fn, string _contents)
        {
            Contents = _contents;
            Start = _st;
            Finish = _fn;
        }

        public void AddContents(string newText)
        {
            Contents += newText;
        }
    }
}
