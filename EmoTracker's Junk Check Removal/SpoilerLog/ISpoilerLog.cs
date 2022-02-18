namespace EmoTrackerJunkCheckRemoval.SpoilerLog
{
    public interface ISpoilerLog
    {
        bool IsThisSpoilerLog(string content);
        ISpoilerLog BuildSpoilerLog(string content);
        void SaveTracker(string filename);
    }
}
