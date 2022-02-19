using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace EmoTrackerJunkCheckRemoval.SpoilerLog
{
    public class SpoilerLogZeldaMinishCap : ISpoilerLog
    {
        public bool IsThisSpoilerLog(string content) => content.StartsWith("Spoiler for Minish Cap Randomizer");

        public ISpoilerLog BuildSpoilerLog(string content)
        {
            var spoilerLog = new SpoilerLogZeldaMinishCap();
            var contentLines = content.Split(Environment.NewLine);

            // Read content of spoiler log file.
            bool inLocationContentsSection = false;
            for (var i = 0; i < contentLines.Length; i++)
            {
                var line = contentLines[i];
                if (line == "Location Contents:")
                {
                    inLocationContentsSection = true;
                    continue;
                }

                if (inLocationContentsSection)
                {
                    if (string.IsNullOrEmpty(line)) // End of section
                        break;

                    var parts = line.Split(": ");
                    if (parts[0].StartsWith("Area")) // Music section
                    {
                        i += 3;
                        continue;
                    }

                    if (string.IsNullOrEmpty(contentLines[i + 1])) // Item with subvalue ?
                        spoilerLog.pairLocationItems.Add(new PairLocationItem(parts[0], parts[1]));
                    else
                    {
                        var subvalueParts = contentLines[i + 1].Split(": ");
                        spoilerLog.pairLocationItems.Add(new PairLocationItem(parts[0], parts[1], subvalueParts[1]));
                        i++;
                    }
                    i++;
                    continue;
                }
            }

            // Compile informations.
            Dictionary<string, SpoilerLog.Item> items = new();
            foreach (var pair in spoilerLog.pairLocationItems)
            {
                var id = pair.Item;
                if (pair.Subvalue != null) id += $"_{pair.Subvalue}";

                if (items.TryGetValue(id, out var itemKey))
                    spoilerLog.ItemCount[itemKey]++;
                else
                {
                    items.Add(id, new SpoilerLog.Item(id, pair.Item));
                    spoilerLog.ItemCount.Add(items[id], 1);
                }
            }

            return spoilerLog;
        }

        private readonly List<PairLocationItem> pairLocationItems = new();

        private class PairLocationItem
        {
            public PairLocationItem(string location, string item, string subvalue = null)
            {
                Location = location ?? throw new ArgumentNullException(nameof(location));
                Item = item ?? throw new ArgumentNullException(nameof(item));
                Subvalue = subvalue;
            }

            public string Location { get; set; }
            public string Item { get; set; }
            public string Subvalue { get; set; }
        }

        public Dictionary<SpoilerLog.Item, int> ItemCount { get; } = new();

        public void SaveTracker(string filename)
        {
            var settings = JsonConvert.DeserializeObject<EmoTrackerSettings>(File.ReadAllText(Path.Combine(App.GetEmoTrackerFolder(), "application_settings.json")));
            Tracker tracker = new()
            {
                ignore_all_logic = settings.tracking_ignore_all_logic,
                display_all_locations = settings.tracking_display_all_locations,
                always_allow_chest_manipulation = settings.tracking_always_allow_clearing_locations
            };
            tracker.item_database.Add(new ProgressiveItem("95:progressive:Fusions", 1));
            tracker.location_database.locations.Add(new("8:Crenel%20Climbing%20Wall%20Chest", false, new Section("0:Wall%20Chest", 0)));
            tracker.location_database.locations.Add(new("34:Crenel%20Climbing%20Wall%20Cave", false, new Section("0:Crenel%20Climbing%20Wall%20Cave", 0)));
            tracker.location_database.locations.Add(new("126:Lady%20Next%20to%20Cafe", false, new Section("0:Lady%20Next%20to%20Cafe", 0)));
            File.WriteAllText(filename, JsonConvert.SerializeObject(tracker, new JsonSerializerSettings { DateFormatString = "yyyy'-'MM'-'dd' 'HH':'mm':'ss" }));
        }

        private class EmoTrackerSettings
        {
#pragma warning disable 0649 // Deserialization
            public bool tracking_ignore_all_logic;
            public bool tracking_always_allow_clearing_locations;
            public bool tracking_display_all_locations;
#pragma warning restore 0649
        }

        private class Tracker
        {
            public string package_uid = "tmcrando_maptracker_cube";
            public string package_variant_uid = "standard";
            public string package_version = "1.8.0.8";
            public DateTime creation_time = DateTime.Now;
            public bool ignore_all_logic;
            public bool display_all_locations;
            public bool always_allow_chest_manipulation;
            public List<Item> item_database = new();
            public Locations location_database = new();
        }

        private abstract class Item
        {
            public string item_reference = "";

            protected Item(string itemReference) => item_reference = itemReference ?? throw new ArgumentNullException(nameof(itemReference));
        }

        private class ToggleItem : Item
        {
            public bool active;

            public ToggleItem(string itemReference, bool active) : base(itemReference) => this.active = active;
        }

        private class ProgressiveItem : Item
        {
            public int stage_index;

            public ProgressiveItem(string itemReference, int stageIndex) : base(itemReference) => stage_index = stageIndex;
        }

        private class ProgressiveToggleItem : Item
        {
            public int stage_index;
            public bool active;

            public ProgressiveToggleItem(string itemReference, int stageIndex, bool active) : base(itemReference)
            {
                stage_index = stageIndex;
                this.active = active;
            }
        }

        private class ConsumableItem : Item
        {
            public int acquired_count;
            public int consumed_count;
            public int max_count;
            public int min_count;

            public ConsumableItem(string itemReference, int acquiredCount, int consumedCount, int maxCount, int minCount) : base(itemReference)
            {
                acquired_count = acquiredCount;
                consumed_count = consumedCount;
                max_count = maxCount;
                min_count = minCount;
            }
        }

        private class Locations
        {
            public List<Location> locations = new();
        }

        private class Location
        {
            public string location_reference = "";
            public bool modified_by_user;
            public List<Section> sections = new();

            public Location(string locationReference, bool modifiedByUser, params Section[] sections)
            {
                location_reference = locationReference ?? throw new ArgumentNullException(nameof(locationReference));
                modified_by_user = modifiedByUser;
                this.sections = new List<Section>(sections);
            }
        }

        private class Section
        {
            public string section_reference = "";
            public int available_chest_count;

            public Section(string sectionReference, int availableChestCount)
            {
                section_reference = sectionReference ?? throw new ArgumentNullException(nameof(sectionReference));
                available_chest_count = availableChestCount;
            }
        }
    }
}
