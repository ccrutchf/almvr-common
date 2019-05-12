using System;
using System.Collections.Generic;
using System.Text;

namespace AlmVR.Common.Models.Alm
{
    public class SwimLaneModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<CardModel> Cards { get; set; }
    }
}
