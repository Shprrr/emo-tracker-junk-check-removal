using System;
using System.Collections.Generic;

namespace EmoTrackerJunkCheckRemoval.SpoilerLog
{
    public interface ISpoilerLog
    {
        bool IsThisSpoilerLog(string content);
        ISpoilerLog BuildSpoilerLog(string content);
        Dictionary<Item, int> ItemCount { get; }
        void SaveTracker(string filename);
    }

    public class Item
    {
        public Item(string id, string name, string category = null)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Category = category;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }

        public override bool Equals(object obj) => obj is Item item && Id == item.Id;
        public override int GetHashCode() => HashCode.Combine(Id);

        public static bool operator ==(Item left, Item right) => EqualityComparer<Item>.Default.Equals(left, right);
        public static bool operator !=(Item left, Item right) => !(left == right);
    }
}
