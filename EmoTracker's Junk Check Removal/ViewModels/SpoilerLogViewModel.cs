using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using EmoTrackerJunkCheckRemoval.SpoilerLog;

namespace EmoTrackerJunkCheckRemoval.ViewModels
{
    internal class SpoilerLogViewModel : INotifyPropertyChanged
    {
        public static SpoilerLogViewModel Mock = new();
        static SpoilerLogViewModel()
        {
            Mock.ItemsCount.Add(new(new(new("i1", "Item 1"), 2)));
            Mock.ItemsCount.Add(new(new(new("i2", "Item 2"), 10)));
            Mock.ItemsCount.Add(new(new(new("i3", "Item 3"), 3)));
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
                foreach (var itemCount in spoilerLog.ItemsCount)
                {
                    ItemsCount.Add(new ItemCount(itemCount));
                }
        }

        internal class ItemCount
        {
            public ItemCount(KeyValuePair<Item, int> itemCount)
            {
                Item = itemCount.Key;
                Count = itemCount.Value;
            }

            public Item Item { get; }
            public int Count { get; }
        }
    }
}
