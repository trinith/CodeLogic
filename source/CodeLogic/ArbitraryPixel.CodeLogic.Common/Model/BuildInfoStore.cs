using System;
using System.Reflection;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public interface IBuildInfoStore
    {
        string Platform { get; }
        string AssemblyTitle { get; }
        string Version { get; }
        string Date { get; }
        string ProductName { get; }
        string BuildName { get; }
    }

    public class BuildInfoStore : IBuildInfoStore
    {
        public string Platform { get; private set; }
        public string AssemblyTitle { get; private set; }
        public string Version { get; private set; }
        public string Date { get; private set; }
        public string ProductName { get; private set; }
        public string BuildName { get; private set; }

        public BuildInfoStore(string platform)
        {
            this.Platform = platform;
            Assembly assembly = typeof(CodeLogicEngine).GetTypeInfo().Assembly;
            this.AssemblyTitle = assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;
            this.Version = assembly.GetName().Version.ToString();
            this.Date = DateTime.Now.Date.ToString("yyyy/MM/dd");

            string[] assemblyTitleTokens = this.AssemblyTitle.Split(new char[] { '-' });
            this.ProductName = assemblyTitleTokens[0].Trim();
            this.BuildName = assemblyTitleTokens[1].Trim();
        }
    }
}
