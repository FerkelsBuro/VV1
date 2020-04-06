namespace Domain.Models
{
    public class FileEvent
    {
        public FileEvent(WatchedFile watchedFile, Alphabet @event)
        {
            WatchedFile = watchedFile;
            Event = @event;
        }

        public WatchedFile WatchedFile { get; set; }
        public Alphabet Event { get; set; }
    }
}