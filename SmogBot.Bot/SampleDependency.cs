namespace SmogBot.Bot
{
    public class SampleDependency
    {
        private int _count;

        public string GetText()
        {
            return $"Sample dependency output generated {_count++} time.";
        }
    }
}