using Sproto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using uFrame.Serialization;

namespace CoreEngine.Network
{

    public class ProtoEvent
    {
        public SprotoTypeBase rep = null;
        public int tag = -1;

        public ProtoEvent(SprotoTypeBase rep, int tag)
        {
            this.rep = rep;
            this.tag = tag;
        }
    }
}
