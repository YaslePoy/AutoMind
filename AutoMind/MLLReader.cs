using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AutoMind
{
    internal class MLLDocument : List<MLLList>
    {
        public MLLList this[string id] => this.FirstOrDefault(i => i.Name == id);
        public string raw;
        public MLLDocument(string data)
        {
            raw = data;
        }
        public void Parce()
        {
            var prepText = Utils.Clear(raw);
            var lists = prepText.Split('}').ToList();
            lists.Remove("");
            foreach (var l in lists)
            {
                Add(new MLLList(l));
            }
        }
        public override string ToString()
        {
            return string.Join(", ", this.Select(i => $"{i.Name}[{i.Count}]"));
        }
        public bool HasList(string id) => this.Exists(i => i.Name == id);
    }
    public class MLLList : List<MLLObject>
    {
        public readonly string Name;
        public string Raw;
        public MLLList(string raw)
        {
            Raw = raw;
            Name = raw.Split('{')[0];
            Update();
        }
        public void Update()
        {
            var dataPart = Raw.Split('{')[1].Trim();
            var objs = dataPart.Split("add").ToList();
            objs = objs.Select(i => i.Trim()).ToList();
            objs.RemoveAll(i => i == "");
            foreach (var item in objs)
            {
                Add(new MLLObject(item));
            }
        }
        public override string ToString()
        {
            return $"List of {Count} elements";
        }
        public List<T> ParceList<T>() where T : new()
        {
            var list = new List<T>();
            this.ForEach(i => list.Add(i.Parce<T>()));
            return list;
        }
        public List<T> ParceListAs<T>(Func<string, T> convert)
        {
            var list = new List<T>();
            this.ForEach(i => list.Add(i.Parce<T>(convert)));
            return list;
        }
    }

    public class MLLObject
    {
        public string Raw;
        public Dictionary<string, string> Data = new Dictionary<string, string>();
        public MLLObject(string raw)
        {
            Raw = raw;
            Update();
        }
        public void Update()
        {
            var props = Raw.Split(' ').ToList();
            props.ForEach(i => Data.Add(i.Split('=')[0], i.Split('=')[1]));

        }
        public T Parce<T>() where T : new()
        {
            T ret = new();
            var objType = typeof(T);
            foreach (var item in Data)
            {
                var field = objType.GetField(item.Key);
                switch (field.FieldType.Name)
                {
                    case "String":
                        field.SetValue(ret, item.Value);
                        break;
                    case "Double":
                        field.SetValue(ret, Utils.ParceDouble(item.Value));
                        break;
                    case "Integer":
                        field.SetValue(ret, int.Parse(item.Value));
                        break;
                }
            }
            return ret;
        }
        public T Parce<T>(Func<string, T> func)
        {
            return func(Raw);
        }
        public override string ToString()
        {
            return Raw;
        }
    }
}
