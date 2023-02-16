using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMind;

namespace AMTgBot
{
    public static class Bank
    {
        public static CalculatingEnvironment Full;
        public static void LoadFull()
        {
            Full = new CalculatingEnvironment();
            Full.AddEnviromentPack("total");
        }
    }
}
