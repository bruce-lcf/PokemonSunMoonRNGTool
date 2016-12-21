using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMHatchingRNGTool
{
    public class TinyMTParameter
    {
        public TinyMTParameter(uint mat1, uint mat2, uint tmat)
        {
            this.mat1 = mat1;
            this.mat2 = mat2;
            this.tmat = tmat;
        }

        public uint mat1 { get; }
        public uint mat2 { get; }
        public uint tmat { get; }
    }
}
