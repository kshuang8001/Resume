using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClockInProject.Service
{
    public class StockService
    {
        public Dictionary<int, string> GetAll()
        {
            return new Dictionary<int, string>() { { 0, "殖利率&nbsp;" }, { 1, "本益比&nbsp;" }, { 2, "股價淨值比&nbsp;" } };
        }
         
    }
}