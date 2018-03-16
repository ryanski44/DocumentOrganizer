using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentOrganizerUI
{
    [Serializable]
    public class Filter
    {
        public string Regexes { get; set; }
        public string FilterOutputPath { get; set; }
    }
}
