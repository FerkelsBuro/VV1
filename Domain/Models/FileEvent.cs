namespace Domain.Models
{
    public class FileEvent
    {
        public FileEvent(string dateiName, Alphabet @event)
        {
            DateiName = dateiName;
            Event = @event;
        }

        public string DateiName { get; set; }
        public Alphabet Event { get; set; }
    }
}