using System;
using System.Collections.Generic;
using System.Text;

namespace AlmVR.Common.Models.Alm
{
    public class BoardModel
    {
        public string ID { get; set; }
        public IEnumerable<SwimLaneModel> SwimLanes { get; set; }
    }
}
