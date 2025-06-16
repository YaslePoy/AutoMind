using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMind
{
    public class MultilinkedField
    {
        List<LinkedElement> elements;
        public MultilinkedField()
        {
            elements = new List<LinkedElement>();
        }
        public void AddLink(object e1, object e2)
        {
            if (!elements.Any(i => i.Data == e1)) elements.Add(new LinkedElement(e1));
            if (!elements.Any(i => i.Data == e2)) elements.Add(new LinkedElement(e2));
            Get(e1).LinkWith(Get(e2));
            Get(e2).LinkWith(Get(e1));
        }
        public LinkedElement Get(object data) => elements.FirstOrDefault(i => i.Data == data);
        public bool HasWayBeet(object from, object to, List<LinkedElement> ignore, ref List<object> way)
        {
            if (from == to)
            {
                way.Add(from);
                return true;
            }
            var eStart = Get(from);
            var eEnd = Get(to);
            if (eEnd.HasLink(eStart))
            {
                way.Add(eStart.Data);
                return true;
            }

            List<LinkedElement> subIgnore = new List<LinkedElement>();
            ignore.ForEach(i => subIgnore.Add(i));
            subIgnore.Add(eEnd);
            var ways = new List<List<object>>();
            foreach (var l in eEnd.Links)
            {
                if (ignore.Any(i => i.Data == l.Data))
                    continue;
                List<object> w = new List<object>();

                if (HasWayBeet(from, l.Data, subIgnore, ref w))
                {
                    w.Add(l.Data);
                    ways.Add(w);
                }
            }
            if (ways.Count > 0)
            {
                way = ways.MinBy(i => i.Count);
                return true;
            }
            return false;
        }

        public bool HasWaysToDestinaion(List<object> having, object destiantion, List<LinkedElement> ignore, out List<List<object>> way)
        {
            way = new List<List<object>>();
            var elements = having.Select(Get).ToList();
            var elementSum = elements.SelectMany(i => i.Links).ToList();
            return false;
        }
    }

    public class LinkedElement
    {
        public List<LinkedElement> Links;
        public object Data;
        public LinkedElement(object inside)
        {
            Data = inside;
            Links = new List<LinkedElement>();
        }
        public void LinkWith(LinkedElement e)
        {
            if (!Links.Contains(e)) Links.Add(e);
        }
        public bool HasLink(LinkedElement e) => Links.Any(i => i.Data == e.Data);
        public override string ToString()
        {
            return Data.ToString();
        }
    }
}
