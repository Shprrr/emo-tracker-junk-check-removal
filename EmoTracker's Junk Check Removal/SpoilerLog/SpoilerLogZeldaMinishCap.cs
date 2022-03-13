using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace EmoTrackerJunkCheckRemoval.SpoilerLog
{
    public class SpoilerLogZeldaMinishCap : ISpoilerLog
    {
        private static readonly Category MajorCategory = new("Major", 1);
        private static readonly Category QuestCategory = new("Quest", 2);
        private static readonly Category KinstoneCategory = new("Kinstone", 3);
        private static readonly Category UpgradeCategory = new("Upgrade", 4);
        private static readonly Category ScrollsCategory = new("Scrolls", 5);
        private static readonly Category GoalCategory = new("Goal", 6);
        //private static readonly Category DungeonCategory = new("Dungeon", 10);
        private static readonly Category DeepwoodShrineCategory = new("Deepwood Shrine", 11);
        private static readonly Category CaveOfFlamesCategory = new("Cave of Flames", 12);
        private static readonly Category FortressOfWindsCategory = new("Fortress of Winds", 13);
        private static readonly Category TempleOfDropletsCategory = new("Temple of Droplets", 14);
        private static readonly Category RoyalCryptCategory = new("Royal Crypt", 15);
        private static readonly Category PalaceOfWindsCategory = new("Palace of Winds", 16);
        private static readonly Category DarkHyruleCastleCategory = new("Dark Hyrule Castle", 17);
        private static readonly Category MoneyCategory = new("Money", 20);
        private static readonly Category RecoveryCategory = new("Recovery", 21);
        private static readonly Dictionary<string, SpoilerLog.Item> ItemDatabase = new()
        {
            { "SmithSword", new("Sword", "Sword", 1, MajorCategory) },
            //{ "GreenSword", new("GreenSword", "Sword", 0, null) }, // Progressive item
            //{ "RedSword", new("RedSword", "Sword", 0, null) }, // Progressive item
            //{ "BlueSword", new("BlueSword", "Sword", 0, null) }, // Progressive item
            //{ "FourSword", new("FourSword", "Sword", 0, null) }, // Progressive item
            //{ "Bombs", new("Bombs", "Bomb", 0, null) }, // Progressive item
            { "RemoteBombs", new("RemoteBombs", "Remote Bomb", 3, UpgradeCategory) },
            { "Bow", new("Bow", "Bow", 4, MajorCategory) },
            //{ "LightArrow", new("LightArrow", "Light Arrow", 0, null) }, // Progressive item
            { "Boomerang", new("Boomerang", "Boomerang", 5, MajorCategory) },
            //{ "MagicBoomerang", new("MagicBoomerang", "Magical Boomerang", 0, null) }, // Progressive item
            { "Shield", new("Shield", "Shield", 6, MajorCategory) },
            //{ "MirrorShield", new("MirrorShield", "Mirror Shield", 0, null) }, // Progressive item
            { "LanternOff", new("Lantern", "Lantern", 7, MajorCategory) },
            { "GustJar", new("GustJar", "Gust Jar", 8, MajorCategory) },
            { "PacciCane", new("PacciCane", "Cane of Pacci", 9, MajorCategory) },
            { "MoleMitts", new("MoleMitts", "Mole Mitts", 10, MajorCategory) },
            { "RocsCape", new("RocsCape", "Roc's Cape", 11, MajorCategory) },
            { "PegasusBoots", new("PegasusBoots", "Pegasus Boots", 12, MajorCategory) },
            //{ "FireRod", new("FireRod", "Fire Rod", 13, MajorCategory) }, // Unused
            { "Ocarina", new("Ocarina", "Ocarina of Wind", 14, MajorCategory) },
            { "GreenOrb", new("GreenOrb", "Green Clock (+4 minutes)", 15, null) }, // Unused Farore's Pearl
            { "BlueOrb", new("BlueOrb", "Blue Clock (+2 minutes)", 16, null) }, // Unused Nayru's Pearl
            { "RedOrb", new("RedOrb", "Red Clock (remove all time)", 17, null) }, // Unused Din's Pearl
            { "Trap", new("Trap", "Trap", 18, null) },
            { "Bottle1", new("Bottle", "Bottle", 19, MajorCategory) },
            { "Bottle2", new("Bottle", "Bottle", 20, MajorCategory) },
            { "Bottle3", new("Bottle", "Bottle", 21, MajorCategory) },
            { "Bottle4", new("Bottle", "Bottle", 22, MajorCategory) },
            //{ "SmithSwordQuest", new("SmithSwordQuest", "Smith's Sword", 23, QuestCategory) }, // Removed from rando
            //{ "BrokenPicoriBlade", new("BrokenPicoriBlade", "Broken Picori Blade", 24, QuestCategory) }, // Removed from rando
            { "DogFoodBottle", new("DogFoodBottle", "Dog Food", 25, QuestCategory) },
            { "LonLonKey", new("LonLonKey", "Lon Lon Ranch Spare Key", 26, QuestCategory) },
            { "WakeUpMushroom", new("WakeUpMushroom", "Wake-Up Mushroom", 27, QuestCategory) },
            { "HyruleanBestiary", new("HyruleanBestiary", "A Hyrulean Bestiary", 28, QuestCategory) },
            { "PicoriLegend", new("PicoriLegend", "Legend of the Picori", 29, QuestCategory) },
            { "MaskHistory", new("MaskHistory", "A History of Masks", 30, QuestCategory) },
            { "GraveyardKey", new("GraveyardKey", "Graveyard Key", 31, QuestCategory) },
            { "TingleTrophy", new("TingleTrophy", "Tingle Trophy", 32, QuestCategory) },
            { "CarlovMedal", new("CarlovMedal", "Carlov Medal", 33, QuestCategory) },
            //{ "ShellsX", new("ShellsX", "Shells", 34, MoneyCategory) }, // Removed from rando
            //{ "Shells30", new("Shells30", "Shells x30", 35, MoneyCategory) }, // Removed from rando
            { "EarthElement", new("EarthElement", "Earth Element", 36, QuestCategory) },
            { "EarthElement_Prizes", new("EarthElement", "Earth Element", 36, QuestCategory) },
            { "FireElement", new("FireElement", "Fire Element", 37, QuestCategory) },
            { "FireElement_Prizes", new("FireElement", "Fire Element", 37, QuestCategory) },
            { "WaterElement", new("WaterElement", "Water Element", 38, QuestCategory) },
            { "WaterElement_Prizes", new("WaterElement", "Water Element", 38, QuestCategory) },
            { "WindElement", new("WindElement", "Wind Element", 39, QuestCategory) },
            { "WindElement_Prizes", new("WindElement", "Wind Element", 39, QuestCategory) },
            { "GripRing", new("GripRing", "Grip Ring", 40, MajorCategory) },
            { "PowerBracelets", new("PowerBracelets", "Power Bracelets", 41, MajorCategory) },
            { "Flippers", new("Flippers", "Flippers", 42, MajorCategory) },
            //{ "HyruleMap", new("HyruleMap", "Map of Hyrule", 43, QuestCategory) }, // Removed from rando
            { "SpinAttack", new("SpinAttack", "Spin Attack", 44, ScrollsCategory) },
            { "RollAttack", new("RollAttack", "Roll Attack", 45, ScrollsCategory) },
            { "DashAttack", new("DashAttack", "Dash Attack", 46, ScrollsCategory) },
            { "RockBreaker", new("RockBreaker", "Rock Breaker", 47, ScrollsCategory) },
            { "SwordBeam", new("SwordBeam", "Sword Beam", 48, ScrollsCategory) },
            { "GreatSpin", new("GreatSpin", "Great Spin Attack", 49, ScrollsCategory) },
            { "DownThrust", new("DownThrust", "Downthrust", 50, ScrollsCategory) },
            { "PerilBeam", new("PerilBeam", "Peril Beam", 51, ScrollsCategory) },
            { "DungeonMap_Deepwood", new("DungeonMap_Deepwood", "Dungeon Map (DwS)", 1, DeepwoodShrineCategory) },
            { "Compass_Deepwood", new("Compass_Deepwood", "Compass (DwS)", 2, DeepwoodShrineCategory) },
            { "BigKey_Deepwood", new("BigKey_Deepwood", "Big Key (DwS)", 3, DeepwoodShrineCategory) },
            { "SmallKey_Deepwood", new("SmallKey_Deepwood", "Small Key (DwS)", 4, DeepwoodShrineCategory) },
            { "DungeonMap_24", new("DungeonMap_Deepwood", "Dungeon Map (DwS)", 1, DeepwoodShrineCategory) },
            { "Compass_24", new("Compass_Deepwood", "Compass (DwS)", 2, DeepwoodShrineCategory) },
            { "BigKey_24", new("BigKey_Deepwood", "Big Key (DwS)", 3, DeepwoodShrineCategory) },
            { "SmallKey_24", new("SmallKey_Deepwood", "Small Key (DwS)", 4, DeepwoodShrineCategory) },
            { "DungeonMap_FlameCave", new("DungeonMap_FlameCave", "Dungeon Map (CoF)", 1, CaveOfFlamesCategory) },
            { "Compass_FlameCave", new("Compass_FlameCave", "Compass (CoF)", 2, CaveOfFlamesCategory) },
            { "BigKey_FlameCave", new("BigKey_FlameCave", "Big Key (CoF)", 3, CaveOfFlamesCategory) },
            { "SmallKey_FlameCave", new("SmallKey_FlameCave", "Small Key (CoF)", 4, CaveOfFlamesCategory) },
            { "DungeonMap_25", new("DungeonMap_FlameCave", "Dungeon Map (CoF)", 1, CaveOfFlamesCategory) },
            { "Compass_25", new("Compass_FlameCave", "Compass (CoF)", 2, CaveOfFlamesCategory) },
            { "BigKey_25", new("BigKey_FlameCave", "Big Key (CoF)", 3, CaveOfFlamesCategory) },
            { "SmallKey_25", new("SmallKey_FlameCave", "Small Key (CoF)", 4, CaveOfFlamesCategory) },
            { "DungeonMap_Fortress", new("DungeonMap_Fortress", "Dungeon Map (FoW)", 1, FortressOfWindsCategory) },
            { "Compass_Fortress", new("Compass_Fortress", "Compass (FoW)", 2, FortressOfWindsCategory) },
            { "BigKey_Fortress", new("BigKey_Fortress", "Big Key (FoW)", 3, FortressOfWindsCategory) },
            { "SmallKey_Fortress", new("SmallKey_Fortress", "Small Key (FoW)", 4, FortressOfWindsCategory) },
            { "DungeonMap_26", new("DungeonMap_Fortress", "Dungeon Map (FoW)", 1, FortressOfWindsCategory) },
            { "Compass_26", new("Compass_Fortress", "Compass (FoW)", 2, FortressOfWindsCategory) },
            { "BigKey_26", new("BigKey_Fortress", "Big Key (FoW)", 3, FortressOfWindsCategory) },
            { "SmallKey_26", new("SmallKey_Fortress", "Small Key (FoW)", 4, FortressOfWindsCategory) },
            { "DungeonMap_Droplets", new("DungeonMap_Droplets", "Dungeon Map (ToD)", 1, TempleOfDropletsCategory) },
            { "Compass_Droplets", new("Compass_Droplets", "Compass (ToD)", 2, TempleOfDropletsCategory) },
            { "BigKey_Droplets", new("BigKey_Droplets", "Big Key (ToD)", 3, TempleOfDropletsCategory) },
            { "SmallKey_Droplets", new("SmallKey_Droplets", "Small Key (ToD)", 4, TempleOfDropletsCategory) },
            { "DungeonMap_27", new("DungeonMap_Droplets", "Dungeon Map (ToD)", 1, TempleOfDropletsCategory) },
            { "Compass_27", new("Compass_Droplets", "Compass (ToD)", 2, TempleOfDropletsCategory) },
            { "BigKey_27", new("BigKey_Droplets", "Big Key (ToD)", 3, TempleOfDropletsCategory) },
            { "SmallKey_27", new("SmallKey_Droplets", "Small Key (ToD)", 4, TempleOfDropletsCategory) },
            { "DungeonMap_Palace", new("DungeonMap_Palace", "Dungeon Map (PoW)", 1, PalaceOfWindsCategory) },
            { "Compass_Palace", new("Compass_Palace", "Compass (PoW)", 2, PalaceOfWindsCategory) },
            { "BigKey_Palace", new("BigKey_Palace", "Big Key (PoW)", 3, PalaceOfWindsCategory) },
            { "SmallKey_Palace", new("SmallKey_Palace", "Small Key (PoW)", 4, PalaceOfWindsCategory) },
            { "DungeonMap_28", new("DungeonMap_Palace", "Dungeon Map (PoW)", 1, PalaceOfWindsCategory) },
            { "Compass_28", new("Compass_Palace", "Compass (PoW)", 2, PalaceOfWindsCategory) },
            { "BigKey_28", new("BigKey_Palace", "Big Key (PoW)", 3, PalaceOfWindsCategory) },
            { "SmallKey_28", new("SmallKey_Palace", "Small Key (PoW)", 4, PalaceOfWindsCategory) },
            { "DungeonMap_DHC", new("DungeonMap_DHC", "Dungeon Map (DHC)", 1, DarkHyruleCastleCategory) },
            { "Compass_DHC", new("Compass_DHC", "Compass (DHC)", 2, DarkHyruleCastleCategory) },
            { "BigKey_DHC", new("BigKey_DHC", "Big Key (DHC)", 3, DarkHyruleCastleCategory) },
            { "SmallKey_DHC", new("SmallKey_DHC", "Small Key (DHC)", 4, DarkHyruleCastleCategory) },
            { "DungeonMap_29", new("DungeonMap_DHC", "Dungeon Map (DHC)", 1, DarkHyruleCastleCategory) },
            { "Compass_29", new("Compass_DHC", "Compass (DHC)", 2, DarkHyruleCastleCategory) },
            { "BigKey_29", new("BigKey_DHC", "Big Key (DHC)", 3, DarkHyruleCastleCategory) },
            { "SmallKey_29", new("SmallKey_DHC", "Small Key (DHC)", 4, DarkHyruleCastleCategory) },
            { "DungeonMap_Crypt", new("DungeonMap_Crypt", "Dungeon Map (RC)", 1, RoyalCryptCategory) },
            { "Compass_Crypt", new("Compass_Crypt", "Compass (RC)", 2, RoyalCryptCategory) },
            { "BigKey_Crypt", new("BigKey_Crypt", "Big Key (RC)", 3, RoyalCryptCategory) },
            { "SmallKey_Crypt", new("SmallKey_Crypt", "Small Key (RC)", 4, RoyalCryptCategory) },
            { "DungeonMap_30", new("DungeonMap_Crypt", "Dungeon Map (RC)", 1, RoyalCryptCategory) },
            { "Compass_30", new("Compass_Crypt", "Compass (RC)", 2, RoyalCryptCategory) },
            { "BigKey_30", new("BigKey_Crypt", "Big Key (RC)", 3, RoyalCryptCategory) },
            { "SmallKey_30", new("SmallKey_Crypt", "Small Key (RC)", 4, RoyalCryptCategory) },
            { "Rupee1", new("Rupee1", "Rupee x1", 52, MoneyCategory) },
            { "Rupee5", new("Rupee5", "Rupee x5", 53, MoneyCategory) },
            { "Rupee20", new("Rupee20", "Rupee x20", 54, MoneyCategory) },
            { "Rupee50", new("Rupee50", "Rupee x50", 55, MoneyCategory) },
            { "Rupee100", new("Rupee100", "Rupee x100", 56, MoneyCategory) },
            { "Rupee200", new("Rupee200", "Rupee x200", 57, MoneyCategory) },
            { "JabberNut", new("JabberNut", "Jabber Nut", 58, UpgradeCategory) },
            { "KinstoneX_YellowTotemProng", new("KinstoneX_YellowTotemProng", "Gold Kinstone Mysterious Statue", 59, KinstoneCategory) },
            { "KinstoneX_YellowCrown", new("KinstoneX_YellowCrown", "Gold Kinstone Source of the Flow", 60, KinstoneCategory) },
            { "KinstoneX_YellowTornadoProng", new("KinstoneX_YellowTornadoProng", "Gold Kinstone Mysterious Clouds", 61, KinstoneCategory) },
            { "KinstoneX_RedSpike", new("KinstoneX_Red", "Red Kinstone", 62, KinstoneCategory) },
            { "KinstoneX_RedCrack", new("KinstoneX_Red", "Red Kinstone", 62, KinstoneCategory) },
            { "KinstoneX_RedProng", new("KinstoneX_Red", "Red Kinstone", 62, KinstoneCategory) },
            { "KinstoneX_BlueL", new("KinstoneX_Blue", "Blue Kinstone", 63, KinstoneCategory) },
            { "KinstoneX_BlueS", new("KinstoneX_Blue", "Blue Kinstone", 63, KinstoneCategory) },
            { "KinstoneX_GreenSpike", new("KinstoneX_Green", "Green Kinstone", 64, KinstoneCategory) },
            { "KinstoneX_GreenSquare", new("KinstoneX_Green", "Green Kinstone", 64, KinstoneCategory) },
            { "KinstoneX_GreenSplit", new("KinstoneX_Green", "Green Kinstone", 64, KinstoneCategory) },
            { "Bombs5", new("Bombs5", "Bombs x5", 65, RecoveryCategory) },
            { "Bombs10", new("Bombs10", "Bombs x10", 66, RecoveryCategory) },
            { "Bombs30", new("Bombs30", "Bombs x30", 67, RecoveryCategory) },
            { "Arrows5", new("Arrows5", "Arrows x5", 68, RecoveryCategory) },
            { "Arrows10", new("Arrows10", "Arrows x10", 69, RecoveryCategory) },
            { "Arrows30", new("Arrows30", "Arrows x30", 70, RecoveryCategory) },
            { "SmallHeart", new("SmallHeart", "Recovery Heart", 71, RecoveryCategory) },
            { "Fairy", new("Fairy", "Fairy", 72, RecoveryCategory) },
            { "HeartContainer", new("HeartContainer", "Heart Container", 73, UpgradeCategory) },
            { "PieceOfHeart", new("PieceOfHeart", "Piece of Heart", 74, UpgradeCategory) },
            { "Wallet", new("Wallet", "Wallet", 75, UpgradeCategory) },
            { "BombBag", new("BombBag", "Bomb Bag", 2, MajorCategory) },
            { "LargeQuiver", new("LargeQuiver", "Quiver", 76, MajorCategory) },
            { "KinstoneBag", new("KinstoneBag", "Figurine", 77, GoalCategory) }, // Kinstone Bag replaced from rando
            { "Brioche", new("Brioche", "Brioche", 78, null) },
            { "Croissant", new("Croissant", "Croissant", 79, null) },
            { "PieSlice", new("PieSlice", "Slice of Pie", 80, null) },
            { "CakeSlice", new("CakeSlice", "Slice of Cake", 81, null) },
            { "ArrowButterfly_1", new("ArrowButterfly", "Joy Butterfly (Bow)", 82, UpgradeCategory) },
            { "DigButterfly_1", new("DigButterfly", "Joy Butterfly (Mole Mitts)", 83, UpgradeCategory) },
            { "SwimButterfly_1", new("SwimButterfly", "Joy Butterfly (Flippers)", 84, UpgradeCategory) },
            { "FastSpin", new("FastSpin", "Faster Spin Attack", 85, ScrollsCategory) },
            { "FastSplit", new("FastSplit", "Faster Split Gauge", 86, ScrollsCategory) },
            { "LongSpin", new("LongSpin", "Longer Great Spin Attack", 87, ScrollsCategory) }
        };

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
                if (pair.Item != "KinstoneBag" && pair.Subvalue != null) id += $"_{pair.Subvalue}";

                if (items.TryGetValue(id, out var itemKey))
                    spoilerLog.ItemsCount[itemKey]++;
                else
                {
                    if (!ItemDatabase.TryGetValue(id, out var item))
                        item = new SpoilerLog.Item(id, pair.Item, int.MaxValue); // Unknown item
                    items.Add(id, item);

                    // Several id gives the same item
                    if (spoilerLog.ItemsCount.ContainsKey(items[id]))
                        spoilerLog.ItemsCount[items[id]]++;
                    else
                        spoilerLog.ItemsCount.Add(items[id], 1);
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

        public Dictionary<SpoilerLog.Item, int> ItemsCount { get; } = new();

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
