using System.IO;

namespace EmoTrackerJunkCheckRemoval.SpoilerLog
{
    public class SpoilerLogFactory
    {
        public static ISpoilerLog Open(string spoilerLogFilename)
        {
            if (string.IsNullOrWhiteSpace(spoilerLogFilename)) return null;

            var spoilerLogText = File.ReadAllText(spoilerLogFilename);

            if (spoilerLogText.StartsWith("Spoiler for Minish Cap Randomizer"))
                return new SpoilerLogZeldaMinishCap();

            return null;
        }
    }
}