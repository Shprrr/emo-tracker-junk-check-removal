using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using EmoTrackerJunkCheckRemoval.SpoilerLog;

namespace EmoTrackerJunkCheckRemoval.ViewModels
{
    internal class SpoilerLogViewModel : INotifyPropertyChanged
    {
        public static SpoilerLogViewModel Mock = new();
        static SpoilerLogViewModel()
        {
            Mock.ItemsCount.Add(new(new("i1", "Item 1", 1), 2));
            Mock.ItemsCount.Add(new(new("i2", "Item 2", 2), 10));
            Mock.ItemsCount.Add(new(new("i3", "Item 3", 3), 3));
        }

        private ObservableCollection<ItemCount> itemsCount = new();

        public ObservableCollection<ItemCount> ItemsCount
        {
            get => itemsCount;
            set { itemsCount = value; PropertyChanged(this, new PropertyChangedEventArgs(nameof(ItemsCount))); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetSpoilerLog(ISpoilerLog spoilerLog)
        {
            ItemsCount.Clear();
            if (spoilerLog != null)
                foreach (var itemCount in spoilerLog.ItemsLocations
                    .OrderBy(i => (i.Key.Category?.Order).GetValueOrDefault(int.MaxValue)).ThenBy(i => i.Key.Order).ThenBy(i => i.Key.Id))
                {
                    ItemsCount.Add(new ItemCount(itemCount.Key, itemCount.Value.Count));
                }
        }

        internal class ItemCount
        {
            public ItemCount(Item item, int count)
            {
                Item = item;
                Count = count;
            }

            public bool IsSelected { get; set; } = true;
            public Item Item { get; }
            public int Count { get; }
        }
    }
}
