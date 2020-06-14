namespace Wikiled.Common.Utilities.Resources.Config
{
    public class LocationConfig
    {
        public string Remote { get; set; }

        public string Local { get; set; }

        public override string ToString()
        {
            return $"Location: {Local} Remote: {Remote}";
        }
    }
}
