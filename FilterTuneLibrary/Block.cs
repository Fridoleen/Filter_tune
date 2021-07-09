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
    public struct Block
    {
        public int Start { get; set; }
        public int Finish { get; set; }

        public Block(int _st, int _fn)
        {
            this.Start = _st;
            this.Finish = _fn;
        }
    }
}
