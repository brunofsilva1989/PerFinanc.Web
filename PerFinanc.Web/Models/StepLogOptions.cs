namespace PerFinanc.Web.Models
{
    public class StepLogOptions
    {
        public bool Enabled { get; set; } = true;
        public string Directory { get; set; } = "Logs";
        public string FilePrefix { get; set; } = "app";
        public bool DateInFileName { get; set; } = true;
        public string MinLevel { get; set; } = "Information";
    }
}
