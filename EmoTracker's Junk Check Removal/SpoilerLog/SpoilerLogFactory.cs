using System.IO;

namespace EmoTrackerJunkCheckRemoval.SpoilerLog
{
    public class SpoilerLogFactory
    {
        public ISpoilerLog[] spoilersTemplate = new[] { new SpoilerLogZeldaMinishCap() };

        public ISpoilerLog Open(string spoilerLogFilename)
        {
            if (string.IsNullOrWhiteSpace(spoilerLogFilename)) return null;

            var spoilerLogText = File.ReadAllText(spoilerLogFilename);

            foreach (var spoilersTemplate in spoilersTemplate)
            {
                if (spoilersTemplate.IsThisSpoilerLog(spoilerLogText))
                    return spoilersTemplate.BuildSpoilerLog(spoilerLogText);
            }

            return null;
        }
    }
}
