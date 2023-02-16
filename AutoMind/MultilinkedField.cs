using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMind
{
    internal class MultilinkedField
    {
    }

    public class LinkedElement
    {
        public List<LinkedElement> Links;
        public object Data;

    }
    public class LinkChain : List<LinkedElement> { }
}
