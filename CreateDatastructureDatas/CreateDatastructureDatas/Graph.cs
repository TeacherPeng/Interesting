using System.Collections.Generic;
using System.Text;

namespace CreateDatastructureDatas
{
    class Graph
    {
        public class Arc
        {
            public Arc(int aAdj, int aWeight)
            {
                Adj = aAdj;
                Weight = aWeight;
            }
            public int Weight { get; set; }
            public int Adj { get; set; }
        }
        public class Vex
        {
            public Vex(string aId, int aWeight)
            {
                Id = aId;
                Weight = aWeight;
            }
            public string Id { get; set; }
            public int Weight { get; set; }
            public List<Arc> Arcs { get; } = new List<Arc>();
        }
        public List<Vex> Vexes { get; } = new List<Vex>();
        public void AddVex(string aId, int aWeight)
        {
            Vexes.Add(new Vex(aId, aWeight));
        }
        public void AddArc(int aStart, int aEnd, int aWeight)
        {
            Vexes[aStart].Arcs.Add(new Arc(aEnd, aWeight));
        }
        public override string ToString()
        {
            StringBuilder aStringBuilder = new StringBuilder();
            // 输出OJ格式顶点权值表
            // 输出OJ格式顶点名称表
            // 输出OJ格式顶点名称+权值表
            // 输出试题格式顶点权值表
            // 输出试题格式顶点名称表
            // 输出试题格式顶点名称+权值表
            // 输出OJ格式弧表
            // 输出OJ格式弧+权值表
            // 输出试题格式弧表
            // 输出试题格式弧+权值表
            return aStringBuilder.ToString();
        }
    }
}
