using System;
using System.Collections.Generic;

namespace EmoTrackerJunkCheckRemoval.SpoilerLog
{
    public interface ISpoilerLog
    {
        bool IsThisSpoilerLog(string content);
        ISpoilerLog BuildSpoilerLog(string content);
        Dictionary<Item, List<string>> ItemsLocations { get; }
        void SaveTracker(string filename, IEnumerable<Item> junkItems);
    }

    public class Item
    {
        public Item(string id, string name, int order, Category category = null)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Order = order;
            Category = category;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public int Order { get; set; }

        public override bool Equals(object obj) => obj is Item item && Id == item.Id;
        public override int GetHashCode() => HashCode.Combine(Id);

        public static bool operator ==(Item left, Item right) => EqualityComparer<Item>.Default.Equals(left, right);
        public static bool operator !=(Item left, Item right) => !(left == right);
    }

    public class Category
    {
        public Category(string name, int order)
        {
            Name = name;
            Order = order;
        }

        public string Name { get; set; }
        public int Order { get; set; }
    }
}
