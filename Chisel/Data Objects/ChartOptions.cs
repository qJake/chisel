using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chisel
{
    public sealed class ChartOptions
    {
        public DataPointStyles PointStyle { get; set; }
        public DataLineStyles LineStyle { get; set; }
        public ChartUpdateModes UpdateMode { get; set; }

        public ChartOptions()
        {
            PointStyle = DataPointStyles.Fill;
            LineStyle = DataLineStyles.Line;
            UpdateMode = ChartUpdateModes.Static;
        }
    }
}
