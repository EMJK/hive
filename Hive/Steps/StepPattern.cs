using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hive.Steps
{
    class StepPattern
    {
        public  StepPatternSegment PatternRoot { get; }

        public StepPattern(StepPatternSegment root)
        {
            PatternRoot = root;
        }
    }
}
