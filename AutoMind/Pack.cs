﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMind
{
    public class Pack
    {
        public string Identifier;
        public string Name = "NN";
        public string Description;
        public string Short;

        public List<Property> Properties { get; set; }
        
        public List<Constant> Constants { get; set; }
        public List<Formula> Formulas { get; set; }
        public static bool operator ==(Pack p1, Pack p2)
        {
            return p1.Name + p1.Description + p1.Short == p2.Name + p2.Description + p2.Short;
        }

        public static bool operator !=(Pack p1, Pack p2)
        {
            return p1.Name + p1.Description + p1.Short != p2.Name + p2.Description + p2.Short;
        }
    }
}