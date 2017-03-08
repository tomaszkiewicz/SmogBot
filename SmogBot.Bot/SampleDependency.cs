using System;

namespace SmogBot.Bot
{
    [Serializable]
    public class SampleDependency
    {
        private int _count;

        public string GetText()
        {
            return $"Sample dependency output generated {_count++} time.";
        }
    }
}