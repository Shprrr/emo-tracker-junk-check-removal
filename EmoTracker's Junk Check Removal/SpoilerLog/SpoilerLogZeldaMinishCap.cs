using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private static readonly Dictionary<string, (string location, string section)[]> LocationDatabase = new()
        {
            { "SmithHouse", new[] { ("129:Smith%27s%20House", "0:Intro%20Items") } },
            { "IntroItem1", new[] { ("129:Smith%27s%20House", "0:Intro%20Items") } },
            { "IntroItem2", new[] { ("129:Smith%27s%20House", "0:Intro%20Items") } },
            { "LinkMinishWaterHoleHeartPiece", new[] { ("134:Minish%20Flippers%20Hole", "0:Minish%20Flippers%20Hole") } },
            { "HyruleWellTop", new[] { ("123:School", "1:Pull%20the%20Statue") } },
            { "HyruleWellLeft", new[] { ("120:Town%20Digging%20Cave", "1:Town%20Basement%20Left") } },
            { "HyruleWellBottom", new[] { ("122:Hyrule%20Well", "0:Hyrule%20Well%20Bottom%20Chest") } },
            { "HyruleWellPillar", new[] { ("122:Hyrule%20Well", "1:Hyrule%20Well%20Center%20Chest") } },
            { "HyruleWellRight", new[] { ("109:Mayor%27s%20House%20Basement", "0:Mayor%27s%20House%20Basement") } },
            { "PreCastleCaveHeartPiece", new[] { ("93:North%20Field%20Cave%20Heart%20Piece", "0:North%20Field%20Cave%20Heart%20Piece") } },
            { "SwiftbladeScroll1", new[] { ("102:Swiftblade%27s%20Dojo", "0:Spin%20Attack") } },
            { "SwiftbladeScroll2", new[] { ("102:Swiftblade%27s%20Dojo", "1:Rock%20Breaker") } },
            { "SwiftbladeScroll3", new[] { ("102:Swiftblade%27s%20Dojo", "2:Dash%20Attack") } },
            { "SwiftbladeScroll4", new[] { ("102:Swiftblade%27s%20Dojo", "3:Down%20Thrust") } },
            { "GrimbladeHeartPiece", new[] { ("79:Grimblade", "1:Heart%20Piece") } },
            { "GrimbladeScroll", new[] { ("79:Grimblade", "0:Grimblade") } },
            { "CastleWaterLeft", new[] { ("80:Moat", "0:Moat") } },
            { "CastleWaterRight", new[] { ("80:Moat", "0:Moat") } },
            { "CafeLady", new[] { ("126:Lady%20Next%20to%20Cafe", "0:Lady%20Next%20to%20Cafe") } },
            { "HearthLedge", new[] { ("127:Hearth%20Ledge", "0:Hearth%20Ledge") } },
            { "HearthBackdoor", new[] { ("106:Hearth%20Back%20Door%20Heart%20Piece", "0:Hearth%20Back%20Door%20Heart%20Piece"), ("107:Hearth", "0:Hearth%20Back%20Door%20Heart%20Piece") } },
            { "SchoolTop", new[] { ("123:School", "0:Roof%20Chest") } },
            { "SchoolGardenLeft", new[] { ("124:School%20Gardens", "0:Garden%20Chests"), ("125:School%20Gardens%20Open", "0:Garden%20Chests") } },
            { "SchoolGardenMiddle", new[] { ("124:School%20Gardens", "0:Garden%20Chests"), ("125:School%20Gardens%20Open", "0:Garden%20Chests") } },
            { "SchoolGardenRight", new[] { ("124:School%20Gardens", "0:Garden%20Chests"), ("125:School%20Gardens%20Open", "0:Garden%20Chests") } },
            { "SchoolGardenHeartPiece", new[] { ("124:School%20Gardens", "1:Heart%20Piece"), ("125:School%20Gardens%20Open", "1:Heart%20Piece") } },
            { "TownDiggingTop", new[] { ("120:Town%20Digging%20Cave", "0:Cave%20Chests") } },
            { "TownDiggingRight", new[] { ("120:Town%20Digging%20Cave", "0:Cave%20Chests") } },
            { "TownDiggingLeft", new[] { ("120:Town%20Digging%20Cave", "0:Cave%20Chests") } },
            { "BakeryAttic", new[] { ("108:Bakery%20Attic%20Chest", "0:Bakery%20Attic%20Chest") } },
            { "StockWellAttic", new[] { ("104:Stockwell%27s%20Shop", "3:Attic%20Chest") } },
            { "SimulationChest", new[] { ("105:Eastern%20Shops", "1:Simon%27s%20Simulations") } },
            { "RemShoeShop", new[] { ("105:Eastern%20Shops", "0:Rem") } },
            { "Shop80Item", new[] { ("104:Stockwell%27s%20Shop", "0:Wallet%20Spot%20%2880%20Rupees%29") } },
            { "Shop300Item", new[] { ("104:Stockwell%27s%20Shop", "1:Boomerang%20Spot%20%28300%20Rupees%29") } },
            { "Shop600Item", new[] { ("104:Stockwell%27s%20Shop", "2:Quiver%20Spot%20%28600%20Rupees%29") } },
            { "ShopDogfoodItem", new[] { ("104:Stockwell%27s%20Shop", "4:Dog%20Food%20Bottle%20Spot") } },
            { "CarlovReward", new[] { ("110:Carlov", "0:Carlov") } },
            { "FigurineHouseLeft", new[] { ("105:Eastern%20Shops", "2:Figurine%20House") } },
            { "FigurineHouseMiddle", new[] { ("105:Eastern%20Shops", "2:Figurine%20House") } },
            { "FigurineHouseRight", new[] { ("105:Eastern%20Shops", "2:Figurine%20House") } },
            { "FigurineHouseHeartPiece", new[] { ("105:Eastern%20Shops", "3:Figurine%20House%20Heart%20Piece") } },
            { "JullietaBook", new[] { ("113:Julietta%27s%20House", "0:Bookshelf") } },
            { "WrightAtticBook", new[] { ("117:Dr.%20Left%27s%20House", "0:Dr.%20Left%27s%20House") } },
            { "FountainBig", new[] { ("114:Fountain", "0:Mulldozers") } },
            { "FountainSmall", new[] { ("114:Fountain", "1:Small%20Chest") } },
            { "FountainHeartPiece", new[] { ("114:Fountain", "2:Heart%20Piece") } },
            { "LibraryMinish", new[] { ("118:Library", "0:Yellow%20Library%20Minish") } },
            { "CuccoMinigame", new[] { ("119:Anju", "0:Heart%20Piece") } },
            { "TownBell", new[] { ("121:Bell", "0:Bell") } },
            { "FlipsCaveBig", new[] { ("115:Flippers%20Cave", "0:Scissor%20Beetles"), ("116:Flippers%20Cave%20Rupees", "0:Scissor%20Beetles") } },
            { "FlipsCaveSmall", new[] { ("115:Flippers%20Cave", "1:Frozen%20Chest"), ("116:Flippers%20Cave%20Rupees", "1:Frozen%20Chest") } },
            { "TingleTrophyItem", new[] { ("132:Tingle", "0:Tingle") } },

            { "HillsKeeseCave", new[] { ("202:Eastern%20Hills%20Bombable%20Wall", "0:Eastern%20Hills%20Bomb%20Wall") } },
            { "AboveHPHole", new[] { ("180:Lon%20Lon%20Minish%20Crack", "0:Lon%20Lon%20Minish%20Crack") } }, // Not sure
            { "LonLonPot", new[] { ("178:Malon%27s%20Pot", "0:Malon%27s%20Pot") } },
            { "LonLonCave", new[] { ("179:Lon%20Lon%20Cave", "0:Lon%20Lon%20Cave") } },
            { "LonLonCaveSecret", new[] { ("179:Lon%20Lon%20Cave", "1:Hidden%20Bomb%20Wall") } },
            { "LonLonHeartPiece", new[] { ("183:Bonk%20the%20Tree", "0:Minish%20Path%20Heart%20Piece"), ("184:Bonk%20the%20Tree%20Open", "0:Minish%20Path%20Heart%20Piece") } },
            { "MinishRupeeFairy", new[] { ("209:Minish%20Woods%20Great%20Fairy", "0:Minish%20Woods%20Great%20Fairy") } },

            { "TrilbyBombCave", new[] { ("74:Trilby%20Highlands%20Bomb%20Wall", "0:Trilby%20Highlands%20Bomb%20Wall") } },
            { "TrilbyMoleCaveLeft", new[] { ("76:Trilby%20Highlands%20Mitts%20Cave", "0:Trilby%20Cave") } },
            { "TrilbyMoleCaveRight", new[] { ("76:Trilby%20Highlands%20Mitts%20Cave", "0:Trilby%20Cave") } },
            { "BottleScrub", new[] { ("71:Trilby%20Business%20Scrub", "0:Trilby%20Business%20Scrub") } },

            { "JabberNut", new[] { ("218:Minish%20Village", "0:Barrel"), ("219:Minish%20Village%20Open", "0:Barrel") } },
            { "BelariBombs", new[] { ("220:Belari", "0:Belari"), ("221:Belari%20Open", "0:Belari") } },
            { "MinishMiddleFlipperHole", new[] { ("224:Minish%20Flippers%20Cave", "2:Middle") } },
            { "MinishRightFlipperHole", new[] { ("224:Minish%20Flippers%20Cave", "3:Right") } },
            { "MinishLeftFlipperHole", new[] { ("224:Minish%20Flippers%20Cave", "0:Left") } },
            { "MinishLeftFlipperHoleHeartPiece", new[] { ("224:Minish%20Flippers%20Cave", "1:Left%20Heart%20Piece") } },
            { "MinishLikeLikeDiggingCaveLeft", new[] { ("216:Like%20Like%20Cave", "0:Like%20Like%20Cave") } },
            { "MinishLikeLikeDiggingCaveRight", new[] { ("216:Like%20Like%20Cave", "0:Like%20Like%20Cave") } },
            { "MinishNorthHole", new[] { ("195:Minish%20Woods%20North%20Minish%20Hole", "0:Minish%20Woods%20North%20Minish%20Hole") } },
            { "MinishWitchHut", new[] { ("208:Syrup%27s%20Hut", "0:Witch%27s%20Item%20%2860%20Rupees%29") } },
            { "MinishHeartPieceTop", new[] { ("207:Northern%20Heart%20Piece", "0:Northern%20Heart%20Piece") } },
            { "MinishHeartPieceBottom", new[] { ("214:Shrine%20Heart%20Piece", "0:Shrine%20Heart%20Piece") } },
            { "MinishVillageHeartPiece", new[] { ("218:Minish%20Village", "1:Dock%20Heart%20Piece"), ("219:Minish%20Village%20Open", "1:Dock%20Heart%20Piece") } },

            { "CrenelVineHole", new[] { ("32:Crenel%20Minish%20Hole", "0:Minish%20Hole") } },
            { "CrenelMinishHouse", new[] { ("30:Crenel%20Minish%20Crack", "0:Minish%20Crack") } },
            { "CrenelCaveDownstairs", new[] { ("5:Bridge%20Cave", "0:Bridge%20Cave") } },
            { "CrenelHeartCaveLeft", new[] { ("29:Crenel%20Heart%20Piece%20Cave", "0:Chests") } },
            { "CrenelHeartCaveRight", new[] { ("29:Crenel%20Heart%20Piece%20Cave", "0:Chests") } },
            { "CrenelHeartCaveHeartPiece", new[] { ("29:Crenel%20Heart%20Piece%20Cave", "1:Heart%20Piece") } },
            { "CrenelFairyHeartPiece", new[] { ("7:Fairy%20Cave%20Heart%20Piece", "0:Fairy%20Cave") } },
            { "CrenelGripScrub", new[] { ("6:Crenel%20Business%20Scrub", "0:Crenel%20Business%20Scrub") } },
            { "GraybladeLeft", new[] { ("24:Grayblade", "1:Chests") } },
            { "GraybladeRight", new[] { ("24:Grayblade", "1:Chests") } },
            { "GraybladeHeartPiece", new[] { ("24:Grayblade", "2:Heart%20Piece") } },
            { "GraybladeScroll", new[] { ("24:Grayblade", "0:Grayblade") } },
            { "CrenelBombFairy", new[] { ("9:Crenel%20Wall%20Fairy", "0:Crenel%20Fairy") } },
            { "CrenelDigCaveHeartPiece", new[] { ("34:Crenel%20Climbing%20Wall%20Cave", "0:Crenel%20Climbing%20Wall%20Cave") } },
            { "CrenelBlockChest", new[] { ("10:Crenel%20Mines%20Cave", "0:Crenel%20Mines%20Cave") } },
            { "Melari", new[] { ("11:Melari", "0:Melari"), ("12:Melari%20Open", "0:Melari"), ("13:Mines%20Obs", "0:Melari"), ("14:Mines", "0:Melari") } },

            { "WildsSouthCave", new[] { ("53:South%20Lake%20Cave", "0:South%20Lake%20Cave") } },
            { "WildsDarknutCave", new[] { ("47:Darknut", "0:Darknut") } },
            { "WildsDekuCaveRight", new[] { ("54:North%20Cave", "0:North%20Cave") } }, // Not sure
            { "WildsMulldozerHole", new[] { ("49:Mulldozers", "0:Bow%20Chest"), ("50:Mulldozers%20Open", "0:Bow%20Chest") } },
            { "WildsDiggingCaveLeft", new[] { ("57:Castor%20Wilds%20Mitts%20Cave", "0:Castor%20Wilds%20Mitts%20Cave") } },
            { "WildsDiggingCaveRight", new[] { ("57:Castor%20Wilds%20Mitts%20Cave", "0:Castor%20Wilds%20Mitts%20Cave") } },
            { "WildsTopChest", new[] { ("55:Wilds%20Platform%20Chest", "0:Platform%20Chest") } }, // Not sure
            { "WildsTopRightCaveHeartPiece", new[] { ("58:Northeast%20Lake%20Cave", "0:Northeast%20Lake%20Cave") } },
            { "SwiftbladeTheFirstHeartPiece", new[] { ("52:Swiftblade%20the%20First", "1:Heart%20Piece") } },
            { "SwiftbladeTheFirstScroll", new[] { ("52:Swiftblade%20the%20First", "0:Swiftblade%20the%20First") } },
            { "RuinsBombCave", new[] { ("41:Bombable%20Wall", "0:Bombable%20Wall") } },
            { "RuinsMinishHome", new[] { ("42:Minish%20Hole", "0:Minish%20Hole") } },
            { "RuinsMinishCaveHeartPiece", new[] { ("38:Minish%20Wall%20Hole", "0:Minish%20Wall%20Hole%20Heart%20Piece") } },
            { "RuinsArmosKillLeft", new[] { ("40:Armos%20Kill", "0:Armos") } },
            { "RuinsArmosKillRight", new[] { ("40:Armos%20Kill", "0:Armos") } },

            { "StockwellDog", new[] { ("187:Fifi", "0:Fifi") } },
            { "HyliaNorthMinishHole", new[] { ("192:North%20Minish%20Hole", "0:North%20Minish%20Hole") } },
            { "HyliaMayorCabin", new[] { ("190:Lake%20Cabin", "0:Lake%20Cabin"), ("191:Lake%20Cabin%20Open", "0:Lake%20Cabin") } },
            { "WitchDiggingCave", new[] { ("223:Minish%20Woods%20North%20Digging%20Cave", "0:Minish%20Woods%20North%20Digging%20Cave") } },
            { "HyliaSunkenHeartPiece", new[] { ("193:Pond%20Heart%20Piece", "0:Diving%20for%20Love") } },
            { "HyliaBottomHeartPiece", new[] { ("194:Hylia%20Southern%20Heart%20Piece", "0:Hylia%20Southern%20Heart%20Piece") } },
            { "WavebladeHeartPiece", new[] { ("196:Waveblade", "1:Heart%20Piece") } },
            { "WavebladeScroll", new[] { ("196:Waveblade", "0:Waveblade") } },

            { "HyliaCapeCaveTopRight", new[] { ("198:North%20Hylia%20Cape%20Cave", "0:North%20Hylia%20Cape%20Cave"), ("199:Treasure%20Cave", "0:Treasure%20Cave") } },
            { "HyliaCapeCaveBottomLeft", new[] { ("198:North%20Hylia%20Cape%20Cave", "0:North%20Hylia%20Cape%20Cave"), ("199:Treasure%20Cave", "0:Treasure%20Cave") } },
            { "HyliaCapeCaveTopLeft", new[] { ("198:North%20Hylia%20Cape%20Cave", "0:North%20Hylia%20Cape%20Cave"), ("199:Treasure%20Cave", "0:Treasure%20Cave") } },
            { "HyliaCapeCaveTopMiddle", new[] { ("198:North%20Hylia%20Cape%20Cave", "0:North%20Hylia%20Cape%20Cave"), ("199:Treasure%20Cave", "0:Treasure%20Cave") } },
            { "HyliaCapeCaveEntrance", new[] { ("198:North%20Hylia%20Cape%20Cave", "0:North%20Hylia%20Cape%20Cave"), ("199:Treasure%20Cave", "0:Treasure%20Cave") } },
            { "HyliaCapeCaveBottomRight", new[] { ("198:North%20Hylia%20Cape%20Cave", "0:North%20Hylia%20Cape%20Cave"), ("199:Treasure%20Cave", "0:Treasure%20Cave") } },
            { "HyliaCapeCaveBottomMiddle", new[] { ("198:North%20Hylia%20Cape%20Cave", "0:North%20Hylia%20Cape%20Cave"), ("199:Treasure%20Cave", "0:Treasure%20Cave") } },
            { "HyliaPostCapeCaveHeartPiece", new[] { ("200:Lon%20Lon%20North%20Heart%20Piece", "0:Lon%20Lon%20North%20Heart%20Piece") } }, // Not sure
            { "HyliaPreCapeCaveHeartPiece", new[] { ("197:Hylia%20Cape%20Heart%20Piece", "0:Cape%20Heart%20Piece") } },

            { "ArrowFairy", new[] { ("85:Great%20Fairy", "0:Great%20Fairy") } },
            { "DampeKey", new[] { ("87:Dampe", "0:Dampe") } },
            { "RoyalValleyGraveHeartPiece", new[] { ("88:Northwest%20Grave", "0:Northwest%20Grave"), ("89:Northwest%20Grave%20Area", "0:Northwest%20Grave") } },
            { "RoyalValleyLostWoodsChest", new[] { ("86:Lost%20Woods%20Secret%20Chest", "0:Left%20Left%20Left%20Up%20Up%20Up") } },

            { "CryptGibdoLeft", new[] { ("233:Royal%20Crypt", "1:Other%20Gibdos"), ("329:Gibdos", "1:Other%20Gibdos") } },
            { "CryptGibdoRight", new[] { ("233:Royal%20Crypt", "0:Gibdos"), ("329:Gibdos", "0:First%20Gibdos") } },
            { "CryptLeft", new[] { ("233:Royal%20Crypt", "2:Left%20Path"), ("330:Left%20Path", "0:Left%20Path") } },
            { "CryptRight", new[] { ("233:Royal%20Crypt", "3:Right%20Path"), ("331:Right%20Path", "0:Right%20Path") } },
            { "KingGift", new[] { ("233:Royal%20Crypt", "4:King%20Gustaf"), ("332:King%20Gustaf", "0:King%20Gustaf") } },

            { "FallsBehindWall", new[] { ("162:Source%20of%20the%20Flow%20Cave", "1:Bombable%20Wall%20First%20Chest") } },
            { "FallsCliff", new[] { ("162:Source%20of%20the%20Flow%20Cave", "2:Bombable%20Wall%20Second%20Chest") } },
            { "FallsTopCaveBomb", new[] { ("163:Veil%20Falls%20Upper%20Cave", "0:Bomb%20Wall%20Chest"), ("164:Veil%20Falls%20Upper%20Cave%20Rupees", "0:Bomb%20Wall%20Chest"), ("165:Veil%20Falls%20Upper%20Cave%20RupObs", "0:Bomb%20Wall%20Chest") } },
            { "FallsTopCaveFree", new[] { ("163:Veil%20Falls%20Upper%20Cave", "1:Freestanding%20Chest"), ("164:Veil%20Falls%20Upper%20Cave%20Rupees", "1:Freestanding%20Chest"), ("165:Veil%20Falls%20Upper%20Cave%20RupObs", "1:Freestanding%20Chest") } },
            { "FallsUpperHeartPiece", new[] { ("171:Veil%20Falls%20Upper%20Waterfall", "0:Waterfall%20Heart%20Piece") } },

            { "FallsLowerCaveLeft", new[] { ("173:Veil%20Falls%20South%20Mitts%20Cave", "0:Veil%20Falls%20South%20Mitts%20Cave") } },
            { "FallsLowerCaveRight", new[] { ("173:Veil%20Falls%20South%20Mitts%20Cave", "0:Veil%20Falls%20South%20Mitts%20Cave") } },
            { "FallsLowerHeartPiece", new[] { ("176:Lower%20Veil%20Falls%20Heart%20Piece", "0:Lower%20Heart%20Piece") } },

            { "CloudsFreeChest", new[] { ("144:Right%20Chest", "0:Right%20Chest") } },
            { "CloudsNorthKill", new[] { ("142:Kill%20Piranhas%20%28North%29", "0:Kill%20Piranhas") } },
            { "CloudsSouthKill", new[] { ("143:Kill%20Piranhas%20%28South%29", "0:Kill%20Piranhas") } },
            { "CloudsSouthMiddle", new[] { ("149:Center%20Left", "0:Center%20Left") } },
            { "CloudsWestBottom", new[] { ("147:Top%20Left%20South%20Chest", "0:Top%20Left%20South%20Chest") } },
            { "CloudsWestLeft", new[] { ("145:Top%20Left%20North%20Chests", "0:Top%20Left%20North%20Chests"), ("146:Top%20Left%20North%20Chests%20Obscure", "0:Top%20Left%20North%20Chests") } },
            { "CloudsWestRight", new[] { ("145:Top%20Left%20North%20Chests", "0:Top%20Left%20North%20Chests"), ("146:Top%20Left%20North%20Chests%20Obscure", "0:Top%20Left%20North%20Chests") } },
            { "CloudsSouthLeft", new[] { ("148:Bottom%20Left%20Chest", "0:Bottom%20Left%20Chest") } },
            { "CloudsSouthRight", new[] { ("150:Center%20Right", "0:Center%20Right") } },

            { "TowerBottomLeft", new[] { ("157:Wind%20Tribe%20House", "2:House%20Chests"), ("112:Wind%20Tribe%20House%20Open", "0:Early%20House%20Chests") } },
            { "TowerBottomRight", new[] { ("157:Wind%20Tribe%20House", "2:House%20Chests"), ("112:Wind%20Tribe%20House%20Open", "0:Early%20House%20Chests") } },
            { "TowerF2", new[] { ("157:Wind%20Tribe%20House", "2:House%20Chests"), ("112:Wind%20Tribe%20House%20Open", "0:Early%20House%20Chests") } },
            { "GregalOne", new[] { ("157:Wind%20Tribe%20House", "0:Save%20Gregal"), ("112:Wind%20Tribe%20House%20Open", "2:Save%20Gregal") } },
            { "GregalTwo", new[] { ("157:Wind%20Tribe%20House", "1:Gregal%27s%20Gift"), ("112:Wind%20Tribe%20House%20Open", "3:Gregal%27s%20Gift") } },
            { "TowerRightBed", new[] { ("157:Wind%20Tribe%20House", "2:House%20Chests"), ("112:Wind%20Tribe%20House%20Open", "1:Later%20House%20Chests") } },
            { "TowerMiddleBed", new[] { ("157:Wind%20Tribe%20House", "2:House%20Chests"), ("112:Wind%20Tribe%20House%20Open", "1:Later%20House%20Chests") } },
            { "TowerLeftBed", new[] { ("157:Wind%20Tribe%20House", "2:House%20Chests"), ("112:Wind%20Tribe%20House%20Open", "1:Later%20House%20Chests") } },
            { "TowerTopLeft", new[] { ("157:Wind%20Tribe%20House", "2:House%20Chests"), ("112:Wind%20Tribe%20House%20Open", "1:Later%20House%20Chests") } },
            { "TowerTopRight", new[] { ("157:Wind%20Tribe%20House", "2:House%20Chests"), ("112:Wind%20Tribe%20House%20Open", "1:Later%20House%20Chests") } },

            { "DeepwoodWiggler", new[] { ("226:DWS", "11:Madderpillar%20Chest"), ("257:Madderpillar%20Chest", "0:Madderpillar%20Chest") } },
            { "DeepwoodPostWigglerHeartPiece", new[] { ("226:DWS", "12:Madderpillar%20Heart%20Piece"), ("258:Madderpillar%20Heart%20Piece", "0:Madderpillar%20Heart%20Piece") } },
            { "DeepwoodPreWigglerLeft", new[] { ("226:DWS", "10:Puffstool%20Room"), ("256:Puffstool%20Room", "0:Puffstool%20Room") } },
            { "DeepwoodPreWigglerRight", new[] { ("226:DWS", "10:Puffstool%20Room"), ("256:Puffstool%20Room", "0:Puffstool%20Room") } },
            { "DeepwoodPreWigglerHeartPiece", new[] { ("226:DWS", "9:Blue%20Warp%20Heart%20Piece"), ("255:Blue%20Warp%20Heart%20Piece", "0:Blue%20Warp%20Heart%20Piece") } },
            { "DeepwoodPreCompass", new[] { ("226:DWS", "6:Two%20Lamp%20Chest"), ("252:Two%20Lamp%20Chest", "0:Two%20Lamp") } },
            { "DeepwoodMulldozers", new[] { ("226:DWS", "5:Mulldozer%20Key"), ("251:Mulldozer%20Key", "0:Mulldozer%20Key") } },
            { "DeepwoodStatueRoom", new[] { ("226:DWS", "4:Two%20Statue%20Room"), ("250:Two%20Statue%20Room", "0:Two%20Statue%20Room") } },
            { "DeepwoodWestWing", new[] { ("226:DWS", "3:West%20Side%20Big%20Chest"), ("249:West%20Side%20Big%20Chest", "0:West%20Side%20Big%20Chest") } },
            { "DeepwoodPreBarrel", new[] { ("226:DWS", "2:Barrel%20Room%20Northwest"), ("248:Barrel%20Room%20Northwest", "0:Barrel%20Room%20Northwest") } },
            { "DeepwoodSlugTorches", new[] { ("226:DWS", "0:Slug%20Room"), ("246:Slug%20Room", "0:Slug%20Room") } },
            { "DeepwoodBasementNorth", new[] { ("226:DWS", "13:Basement%20Big%20Chest"), ("259:Basement%20Big%20Chest", "0:Basement%20Big%20Chest") } },
            { "DeepwoodBasementSwitch", new[] { ("226:DWS", "8:Basement%20Switch%20Chest"), ("254:Basement%20Switch%20Chest", "0:Basement%20Switch%20Chest") } },
            { "DeepwoodBasementEast", new[] { ("226:DWS", "7:Basement%20Switch%20Room%20Big%20Chest"), ("253:Basement%20Switch%20Room%20Big%20Chest", "0:Basement%20Switch%20Room%20Big%20Chest") } },
            { "DeepwoodUpstairsChest", new[] { ("226:DWS", "1:Upstairs%20Chest"), ("247:Upstairs%20Room", "0:Upstairs%20Chest") } },
            { "DeepwoodBossItem", new[] { ("226:DWS", "14:Green%20Chu"), ("260:Green%20Chu", "0:Green%20Chu") } },
            { "DeepwoodPrize", new[] { ("226:DWS", "14:Green%20Chu"), ("260:Green%20Chu", "0:Green%20Chu") } },

            { "CoFFlippedCart", new[] { ("227:COF", "5:Spiny%20Chu%20Pillar%20Chest"), ("228:COF%20Rupees", "6:Spiny%20Chu%20Pillar%20Chest"), ("270:Spiny%20Chu%20Pillar%20Chest", "0:Spiny%20Chu%20Pillar%20Chest") } },
            { "CoFHeartPiece", new[] { ("227:COF", "3:Bombable%20Wall%20Heart%20Piece"), ("228:COF%20Rupees", "4:Bombable%20Wall%20Heart%20Piece"), ("268:Bombable%20Wall%20Heart%20Piece", "0:Bomb%20Wall%20HP") } },
            { "CoFChuPit", new[] { ("227:COF", "4:Spiny%20Chu%20Fight"), ("228:COF%20Rupees", "5:Spiny%20Chu%20Fight"), ("269:Spiny%20Chu%20Fight", "0:Spiny%20Chu%20Fight") } },
            { "CoFPillBugsPillarChest", new[] { ("227:COF", "2:First%20Rollobite%20Room"), ("228:COF%20Rupees", "3:First%20Rollobite%20Room"), ("267:First%20Rollobite%20Room%20Pillar%20Chest", "0:First%20Rollobite%20Room%20Pillar%20Chest") } },
            { "CoFPillBugsHoleChest", new[] { ("227:COF", "2:First%20Rollobite%20Room"), ("228:COF%20Rupees", "3:First%20Rollobite%20Room"), ("266:First%20Rollobite%20Room%20Free%20Chest", "0:First%20Rollobite%20Room%20Free%20Chest") } },
            { "CoFSoutheastSmall", new[] { ("227:COF", "1:Big%20Chest%20Room"), ("228:COF%20Rupees", "2:Big%20Chest%20Room"), ("265:Big%20Chest%20Room%20Small%20Chest", "0:Big%20Chest%20Room%20Small%20Chest") } },
            { "CoFSoutheastBig", new[] { ("227:COF", "1:Big%20Chest%20Room"), ("228:COF%20Rupees", "2:Big%20Chest%20Room"), ("264:Big%20Chest%20Room%20Big%20Chest", "0:Big%20Chest%20Room%20Big%20Chest") } },
            { "CoFBasementTop", new[] { ("227:COF", "6:Pre%20Lava%20Basement%20Room"), ("228:COF%20Rupees", "7:Pre%20Lava%20Basement%20Room"), ("271:Pre%20Lava%20Basement%20Room%20Block%20Chest", "0:Pre%20Lava%20Basement%20Room%20Block%20Chest") } }, // Not sure
            { "CoFBasementBottom", new[] { ("227:COF", "6:Pre%20Lava%20Basement%20Room"), ("228:COF%20Rupees", "7:Pre%20Lava%20Basement%20Room"), ("272:Pre%20Lava%20Basement%20Room%20Ledge", "0:Pre%20Lava%20Basement%20Room%20Ledge") } }, // Not sure
            { "CoFBlades", new[] { ("227:COF", "7:Blade%20Chest"), ("228:COF%20Rupees", "8:Blade%20Chest"), ("273:COF%20Blade%20Chest", "0:Blade%20Chest") } },
            { "CoFSpinies", new[] { ("227:COF", "0:Spiny%20Beetle%20Fight"), ("228:COF%20Rupees", "0:Spiny%20Beetle%20Fight"), ("262:Spiny%20Beetle%20Fight", "0:Spiny%20Beetle%20Fight") } },
            { "CoFBasementLavaLeft", new[] { ("227:COF", "8:Lava%20Basement%20%28Left%2CRight%29"), ("228:COF%20Rupees", "9:Lava%20Basement%20%28Left%2CRight%29"), ("274:Lava%20Basement%20Left", "0:Lava%20Basement%20Left") } },
            { "CoFBasementLavaRight", new[] { ("227:COF", "8:Lava%20Basement%20%28Left%2CRight%29"), ("228:COF%20Rupees", "9:Lava%20Basement%20%28Left%2CRight%29"), ("275:Lava%20Basement%20Right", "0:Lava%20Basement%20Right") } },
            { "CoFBasementLavaBig", new[] { ("227:COF", "9:Lava%20Basement%20Big%20Chest"), ("228:COF%20Rupees", "10:Lava%20Basement%20Big%20Chest"), ("276:Lava%20Basement%20Big%20Chest", "0:Lava%20Basement%20Big%20Chest") } },
            { "CoFBossItem", new[] { ("227:COF", "10:Gleerok"), ("228:COF%20Rupees", "11:Gleerok"), ("277:Gleerok", "0:Gleerok") } },
            { "CoFPrize", new[] { ("227:COF", "10:Gleerok"), ("228:COF%20Rupees", "11:Gleerok"), ("277:Gleerok", "0:Gleerok") } },

            { "FortressEntrance", new[] { ("229:FOW", "0:Entrance%20Far%20Left"), ("230:FOW%20Rupees", "0:Entrance%20Far%20Left"), ("231:FOW%20Obscure", "0:Entrance%20Far%20Left"), ("232:FOW%20RupObs", "0:Entrance%20Far%20Left"), ("279:Far%20Left%20Entrance%20Room", "0:Far%20Left%20Entrance%20Room") } },
            { "FortressHeartPiece", new[] { ("229:FOW", "8:Right%20Side%20Heart%20Piece"), ("230:FOW%20Rupees", "10:Right%20Side%20Heart%20Piece"), ("231:FOW%20Obscure", "8:Right%20Side%20Heart%20Piece"), ("232:FOW%20RupObs", "10:Right%20Side%20Heart%20Piece"), ("293:Right%20Side%20Heart%20Piece", "0:Right%20Side%20Heart%20Piece") } },
            { "FortressOutsideF2Left", new[] { ("229:FOW", "2:Left%20Side%20Mitts%20Chests"), ("230:FOW%20Rupees", "3:Left%20Side%20Mitts%20Chests"), ("231:FOW%20Obscure", "2:Left%20Side%20Mitts%20Chests"), ("232:FOW%20RupObs", "3:Left%20Side%20Mitts%20Chests"), ("282:Left%20Side%202nd%20Floor%20Mitts%20Chest", "0:Left%20Side%202nd%20Floor%20Mitts%20Chest") } },
            { "FortressOutsideF2Middle", new[] { ("229:FOW", "10:Center%20Path%20Switch"), ("230:FOW%20Rupees", "12:Center%20Path%20Switch"), ("231:FOW%20Obscure", "10:Center%20Path%20Switch"), ("232:FOW%20RupObs", "12:Center%20Path%20Switch"), ("295:Center%20Path%20Switch", "0:Center%20Path%20Switch") } },
            { "FortressOutsideF2Right", new[] { ("229:FOW", "6:Right%20Side%20Mitts%20Chests"), ("230:FOW%20Rupees", "8:Right%20Side%20Mitts%20Chests"), ("231:FOW%20Obscure", "6:Right%20Side%20Mitts%20Chests"), ("232:FOW%20RupObs", "8:Right%20Side%20Mitts%20Chests"), ("290:Right%20Side%202nd%20Floor%20Mitts%20Chest", "0:Right%20Side%202nd%20Floor%20Mitts%20Chest") } },
            { "FortressOutsideF3Left", new[] { ("229:FOW", "2:Left%20Side%20Mitts%20Chests"), ("230:FOW%20Rupees", "3:Left%20Side%20Mitts%20Chests"), ("231:FOW%20Obscure", "2:Left%20Side%20Mitts%20Chests"), ("232:FOW%20RupObs", "3:Left%20Side%20Mitts%20Chests"), ("285:Left%20Side%203rd%20Floor%20Mitts%20Chest", "0:Left%20Side%203rd%20Floor%20Mitts%20Chest") } },
            { "FortressOutsideF3Right", new[] { ("229:FOW", "6:Right%20Side%20Mitts%20Chests"), ("230:FOW%20Rupees", "8:Right%20Side%20Mitts%20Chests"), ("231:FOW%20Obscure", "6:Right%20Side%20Mitts%20Chests"), ("232:FOW%20RupObs", "8:Right%20Side%20Mitts%20Chests"), ("291:Right%20Side%203rd%20Floor%20Mitts%20Chest", "0:Right%20Side%203rd%20Floor%20Mitts%20Chest") } },
            { "FortressOutsideBombWallBigChest", new[] { ("229:FOW", "11:Bombable%20Wall%20Big%20Chest"), ("230:FOW%20Rupees", "13:Bombable%20Wall%20Big%20Chest"), ("231:FOW%20Obscure", "11:Bombable%20Wall%20Big%20Chest"), ("232:FOW%20RupObs", "13:Bombable%20Wall%20Big%20Chest"), ("296:Bombable%20Wall%20Big%20Chest", "0:Bombable%20Wall%20Big%20Chest") } },
            { "FortressOutsideBombWallSmallChest", new[] { ("229:FOW", "12:Bombable%20Wall%20Small%20Chest"), ("230:FOW%20Rupees", "14:Bombable%20Wall%20Small%20Chest"), ("231:FOW%20Obscure", "12:Bombable%20Wall%20Small%20Chest"), ("232:FOW%20RupObs", "14:Bombable%20Wall%20Small%20Chest"), ("297:Bombable%20Wall%20Small%20Chest", "0:Bombable%20Wall%20Small%20Chest") } },
            { "FortressOutsideMinishHole", new[] { ("229:FOW", "14:Minish%20Dirt%20Room%20Key%20Drop"), ("230:FOW%20Rupees", "16:Minish%20Dirt%20Room%20Key%20Drop"), ("231:FOW%20Obscure", "14:Minish%20Dirt%20Room%20Key%20Drop"), ("232:FOW%20RupObs", "16:Minish%20Dirt%20Room%20Key%20Drop"), ("299:Minish%20Dirt%20Room%20Key%20Drop", "0:Minish%20Dirt%20Room%20Key%20Drop") } },
            { "FortressRightDrop", new[] { ("229:FOW", "7:Right%20Side%20Key%20Drop"), ("230:FOW%20Rupees", "9:Right%20Side%20Key%20Drop"), ("231:FOW%20Obscure", "7:Right%20Side%20Key%20Drop"), ("232:FOW%20RupObs", "9:Right%20Side%20Key%20Drop"), ("292:Right%20Side%20Key%20Drop", "0:Right%20Side%20Key%20Drop") } },
            { "FortressLeftDrop", new[] { ("229:FOW", "4:Left%20Side%20Key%20Drop"), ("230:FOW%20Rupees", "6:Left%20Side%20Key%20Drop"), ("231:FOW%20Obscure", "4:Left%20Side%20Key%20Drop"), ("232:FOW%20RupObs", "6:Left%20Side%20Key%20Drop"), ("287:Left%20Side%20Key%20Drop", "0:Left%20Side%20Key%20Drop") } },
            { "FortressClonePuzzle", new[] { ("229:FOW", "13:Clone%20Puzzle%20Key%20Drop"), ("230:FOW%20Rupees", "15:Clone%20Puzzle%20Key%20Drop"), ("231:FOW%20Obscure", "13:Clone%20Puzzle%20Key%20Drop"), ("232:FOW%20RupObs", "15:Clone%20Puzzle%20Key%20Drop"), ("298:Clone%20Puzzle%20Key%20Drop", "0:Clone%20Puzzle%20Key%20Drop") } },
            { "FortressEyegoreKill", new[] { ("229:FOW", "3:Eyegores"), ("230:FOW%20Rupees", "5:Eyegores"), ("231:FOW%20Obscure", "3:Eyegores"), ("232:FOW%20RupObs", "5:Eyegores"), ("286:Eyegores", "0:Eyegores") } },
            { "FortressPedestal", new[] { ("229:FOW", "9:Pedestal%20Chest"), ("230:FOW%20Rupees", "11:Pedestal%20Chest"), ("231:FOW%20Obscure", "9:Pedestal%20Chest"), ("232:FOW%20RupObs", "11:Pedestal%20Chest"), ("294:Pedestal%20Chest", "0:Pedestal%20Chest") } },
            { "FortressSkullFall", new[] { ("229:FOW", "15:Skull%20Room%20Chest"), ("230:FOW%20Rupees", "17:Skull%20Room%20Chest"), ("231:FOW%20Obscure", "16:Skull%20Room%20Chest"), ("232:FOW%20RupObs", "18:Skull%20Room%20Chest"), ("302:Skull%20Room%20Chest", "0:Skull%20Room%20Chest") } },
            { "FortressSkullRoomLeft", new[] { ("229:FOW", "5:Right%20Side%20Two%20Lever%20Room"), ("230:FOW%20Rupees", "7:Right%20Side%20Two%20Lever%20Room"), ("231:FOW%20Obscure", "5:Right%20Side%20Two%20Lever%20Room"), ("232:FOW%20RupObs", "7:Right%20Side%20Two%20Lever%20Room"), ("289:Two%20Lever%20Room%20Left%20Chest", "0:Left%20Chest") } },
            { "FortressSkullRoomRight", new[] { ("229:FOW", "5:Right%20Side%20Two%20Lever%20Room"), ("230:FOW%20Rupees", "7:Right%20Side%20Two%20Lever%20Room"), ("231:FOW%20Obscure", "5:Right%20Side%20Two%20Lever%20Room"), ("232:FOW%20RupObs", "7:Right%20Side%20Two%20Lever%20Room"), ("288:Two%20Lever%20Room%20Right%20Chest", "0:Right%20Chest") } },
            { "FortressWizrobes", new[] { ("229:FOW", "1:Wizzrobe%20Fight"), ("230:FOW%20Rupees", "1:Wizzrobe%20Fight"), ("231:FOW%20Obscure", "1:Wizzrobe%20Fight"), ("232:FOW%20RupObs", "1:Wizzrobe%20Fight"), ("281:Wizzrobe%20Fight", "0:Wizzrobe%20Fight") } },
            { "FortressBossItem", new[] { ("229:FOW", "16:Mazaal"), ("230:FOW%20Rupees", "18:Mazaal"), ("231:FOW%20Obscure", "17:Mazaal"), ("232:FOW%20RupObs", "19:Mazaal"), ("303:Mazaal", "0:Mazaal") } },
            { "FortressPrize", new[] { ("229:FOW", "17:FOW%20Reward"), ("230:FOW%20Rupees", "19:FOW%20Reward"), ("231:FOW%20Obscure", "18:FOW%20Reward"), ("232:FOW%20RupObs", "20:FOW%20Reward"), ("303:Mazaal", "0:Mazaal") } },

            { "DropletsMulldozers", new[] { ("234:TOD", "13:Dark%20Maze%20Bomb%20Wall"), ("235:TOD%20Rupees", "15:Dark%20Maze%20Bomb%20Wall"), ("236:TOD%20Obscure", "14:Dark%20Maze%20Bomb%20Wall"), ("237:TOD%20RupObs", "18:Dark%20Maze%20Bomb%20Wall"), ("326:Dark%20Maze%20Bombable%20Wall", "0:Dark%20Maze%20Bombable%20Wall") } },
            { "DropletsWaterPot", new[] { ("234:TOD", "3:Underwater%20Pot"), ("235:TOD%20Rupees", "3:Underwater%20Pot"), ("236:TOD%20Obscure", "3:Underwater%20Pot"), ("237:TOD%20RupObs", "3:Underwater%20Pot"), ("308:Underwater%20Pot", "0:Underwater%20Pot") } },
            { "DropletsSecondIceblock", new[] { ("234:TOD", "1:Small%20Key%20Locked%20Ice%20Block"), ("235:TOD%20Rupees", "1:Small%20Key%20Locked%20Ice%20Block"), ("236:TOD%20Obscure", "1:Small%20Key%20Locked%20Ice%20Block"), ("237:TOD%20RupObs", "1:Small%20Key%20Locked%20Ice%20Block"), ("306:Key%20Locked%20Ice%20Block", "0:Key%20Locked%20Ice%20Block") } },
            { "DropletsFirstIceblock", new[] { ("234:TOD", "0:First%20Ice%20Block"), ("235:TOD%20Rupees", "0:First%20Ice%20Block"), ("236:TOD%20Obscure", "0:First%20Ice%20Block"), ("237:TOD%20RupObs", "0:First%20Ice%20Block"), ("305:First%20Ice%20Block", "0:First%20Ice%20Block") } },
            { "DropletsEastFirst", new[] { ("234:TOD", "8:Right%20Path%20Ice%20Walkway%20Chests"), ("235:TOD%20Rupees", "10:Right%20Path%20Ice%20Walkway%20Chests"), ("236:TOD%20Obscure", "8:Right%20Path%20Ice%20Walkway%20Chests"), ("237:TOD%20RupObs", "12:Right%20Path%20Ice%20Walkway%20Chests"), ("317:Right%20Path%20Ice%20Walkway%20First%20Chest", "0:Right%20Path%20Ice%20Walkway%20First%20Chest") } },
            { "DropletsIceMaze", new[] { ("234:TOD", "8:Right%20Path%20Ice%20Walkway%20Chests"), ("235:TOD%20Rupees", "10:Right%20Path%20Ice%20Walkway%20Chests"), ("236:TOD%20Obscure", "8:Right%20Path%20Ice%20Walkway%20Chests"), ("237:TOD%20RupObs", "12:Right%20Path%20Ice%20Walkway%20Chests"), ("318:Right%20Path%20Ice%20Walkway%20Second%20Chest", "0:Right%20Path%20Ice%20Walkway%20Second%20Chest") } },
            { "DropletsOverhang", new[] { ("234:TOD", "4:Overhang%20Chest"), ("235:TOD%20Rupees", "4:Overhang%20Chest"), ("236:TOD%20Obscure", "4:Overhang%20Chest"), ("237:TOD%20RupObs", "4:Overhang%20Chest"), ("309:Overhang%20Chest", "0:Overhang%20Chest") } },
            { "DropletsBluChu", new[] { ("234:TOD", "10:Blue%20Chu"), ("235:TOD%20Rupees", "12:Blue%20Chu"), ("236:TOD%20Obscure", "11:Blue%20Chu"), ("237:TOD%20RupObs", "15:Blue%20Chu"), ("320:Blue%20Chu", "0:Blue%20Chu") } },
            { "DropletsBasement", new[] { ("234:TOD", "9:Basement%20Frozen%20Chest"), ("235:TOD%20Rupees", "11:Basement%20Frozen%20Chest"), ("236:TOD%20Obscure", "10:Basement%20Frozen%20Chest"), ("237:TOD%20RupObs", "14:Basement%20Frozen%20Chest"), ("321:Basement%20Frozen%20Chest", "0:Basement%20Frozen%20Chest") } },
            { "DropletsFrozenIcePlain", new[] { ("234:TOD", "6:Ice%20Puzzle%20Frozen%20Chest"), ("235:TOD%20Rupees", "8:Ice%20Puzzle%20Frozen%20Chest"), ("236:TOD%20Obscure", "6:Ice%20Puzzle%20Frozen%20Chest"), ("237:TOD%20RupObs", "10:Ice%20Puzzle%20Frozen%20Chest"), ("315:Ice%20Puzzle%20Frozen%20Chest", "0:Ice%20Puzzle%20Frozen%20Chest") } },
            { "DropletsFreeIcePlain", new[] { ("234:TOD", "5:Ice%20Puzzle%20Free%20Chest"), ("235:TOD%20Rupees", "7:Ice%20Puzzle%20Free%20Chest"), ("236:TOD%20Obscure", "5:Ice%20Puzzle%20Free%20Chest"), ("237:TOD%20RupObs", "9:Ice%20Puzzle%20Free%20Chest"), ("314:Ice%20Puzzle%20Free%20Chest", "0:Ice%20Puzzle%20Free%20Chest") } },
            { "DropletsDarkMazeRight", new[] { ("234:TOD", "12:Dark%20Maze"), ("235:TOD%20Rupees", "14:Dark%20Maze"), ("236:TOD%20Obscure", "13:Dark%20Maze"), ("237:TOD%20RupObs", "17:Dark%20Maze"), ("325:Dark%20Maze%20Top%20Right", "0:Dark%20Maze%20Top%20Right") } },
            { "DropletsDarkMazeLeft", new[] { ("234:TOD", "12:Dark%20Maze"), ("235:TOD%20Rupees", "14:Dark%20Maze"), ("236:TOD%20Obscure", "13:Dark%20Maze"), ("237:TOD%20RupObs", "17:Dark%20Maze"), ("324:Dark%20Maze%20Top%20Left", "0:Dark%20Maze%20Top%20Left") } },
            { "DropletsDarkMazeMiddle", new[] { ("234:TOD", "12:Dark%20Maze"), ("235:TOD%20Rupees", "14:Dark%20Maze"), ("236:TOD%20Obscure", "13:Dark%20Maze"), ("237:TOD%20RupObs", "17:Dark%20Maze"), ("323:Dark%20Maze%20Bottom", "0:Dark%20Maze%20Bottom") } },
            { "DropletsPostTwinFrozen", new[] { ("234:TOD", "11:Post%20Blue%20Chu%20Frozen%20Chest"), ("235:TOD%20Rupees", "13:Post%20Blue%20Chu%20Frozen%20Chest"), ("236:TOD%20Obscure", "12:Post%20Blue%20Chu%20Frozen%20Chest"), ("237:TOD%20RupObs", "16:Post%20Blue%20Chu%20Frozen%20Chest"), ("322:Post%20Blue%20Chu%20Frozen%20Chest", "0:Post%20Blue%20Chu%20Frozen%20Chest") } },
            { "DropletsPreviewFrozen", new[] { ("234:TOD", "7:Post%20Ice%20Puzzle%20Frozen%20Chest"), ("235:TOD%20Rupees", "9:Post%20Ice%20Puzzle%20Frozen%20Chest"), ("236:TOD%20Obscure", "7:Post%20Ice%20Puzzle%20Frozen%20Chest"), ("237:TOD%20RupObs", "11:Post%20Ice%20Puzzle%20Frozen%20Chest"), ("316:Post%20Ice%20Puzzle%20Frozen%20Chest", "0:Post%20Ice%20Puzzle%20Frozen%20Chest") } },
            { "DropletsIceWiggler", new[] { ("234:TOD", "2:Post%20Madderpillar%20Chest"), ("235:TOD%20Rupees", "2:Post%20Madderpillar%20Chest"), ("236:TOD%20Obscure", "2:Post%20Madderpillar%20Chest"), ("237:TOD%20RupObs", "2:Post%20Madderpillar%20Chest"), ("307:Post%20Madderpillar%20Chest", "0:Post%20Madderpillar%20Chest") } },
            { "DropletsBossItem", new[] { ("234:TOD", "14:Octo"), ("235:TOD%20Rupees", "16:Octo"), ("236:TOD%20Obscure", "15:Octo"), ("237:TOD%20RupObs", "19:Octo"), ("327:Octo", "0:Octo") } },
            { "DropletsPrize", new[] { ("234:TOD", "14:Octo"), ("235:TOD%20Rupees", "16:Octo"), ("236:TOD%20Obscure", "15:Octo"), ("237:TOD%20RupObs", "19:Octo"), ("327:Octo", "0:Octo") } },

            { "PalaceWizrobeKill", new[] { ("238:POW", "1:Wizzrobe%20Platform%20Fight"), ("239:POW%20Rupees", "1:Wizzrobe%20Platform%20Fight"), ("335:Wizzrobe%20Platform%20Fight", "0:Wizzrobe%20Platform%20Fight") } },
            { "PalaceFirstGrate", new[] { ("238:POW", "0:Firebar%20Grate"), ("239:POW%20Rupees", "0:Firebar%20Grate"), ("334:Firebar%20Grate", "0:Firebar%20Grate") } },
            { "PalaceBraceletPuzzleKey", new[] { ("238:POW", "2:Pot%20Puzzle%20Key"), ("239:POW%20Rupees", "2:Pot%20Puzzle%20Key"), ("336:Pot%20Puzzle%20Key", "0:Pot%20Puzzle%20Key") } },
            { "PalaceWideGap", new[] { ("238:POW", "3:Moblin%20Archer%20Chest"), ("239:POW%20Rupees", "4:Moblin%20Archer%20Chest"), ("338:Moblin%20Archer%20Chest", "0:Moblin%20Archer%20Chest") } },
            { "PalaceBallAndChainSoldiers", new[] { ("238:POW", "4:Flail%20Soldiers"), ("239:POW%20Rupees", "5:Flail%20Soldiers"), ("339:Flail%20Soldiers", "0:Flail%20Soldiers") } },
            { "PalaceFanLoop", new[] { ("238:POW", "5:Spark%20Chest"), ("239:POW%20Rupees", "6:Spark%20Chest"), ("340:Spark%20Chest", "0:Spark%20Chest") } },
            { "PalacePreBigDoor", new[] { ("238:POW", "6:Pre%20Big%20Key%20Door%20Big%20Chest"), ("239:POW%20Rupees", "7:Pre%20Big%20Key%20Door%20Big%20Chest"), ("341:Pre%20Big%20Key%20Door%20Big%20Chest", "0:Pre%20Big%20Key%20Door%20Big%20Chest") } },
            { "PalaceDarkBig", new[] { ("238:POW", "8:Dark%20Room%20Big"), ("239:POW%20Rupees", "9:Dark%20Room%20Big"), ("343:Dark%20Room%20Big%20Chest", "0:Dark%20Room%20Big%20Chest") } },
            { "PalaceDarkSmall", new[] { ("238:POW", "9:Dark%20Room%20Small"), ("239:POW%20Rupees", "10:Dark%20Room%20Small"), ("344:Dark%20Room%20Small%20Chest", "0:Dark%20Room%20Small%20Chest") } },
            { "PalaceManyRollers", new[] { ("238:POW", "7:Roller%20Chest"), ("239:POW%20Rupees", "8:Roller%20Chest"), ("342:Roller%20Chest", "0:Roller%20Chest") } },
            { "PalaceTwinWizrobes", new[] { ("238:POW", "11:Twin%20Wizzrobe%20Fight"), ("239:POW%20Rupees", "12:Twin%20Wizzrobe%20Fight"), ("345:Twin%20Wizzrobe%20Fight", "0:Twin%20Wizzrobe%20Fight") } },
            { "PalaceFirerobeTrio", new[] { ("238:POW", "10:Fire%20Wizzrobe%20Fight"), ("239:POW%20Rupees", "11:Fire%20Wizzrobe%20Fight"), ("346:Firerobe%20Fight", "0:Firerobe%20Fight") } },
            { "PalaceHeartPiece", new[] { ("238:POW", "12:Heart%20Piece"), ("239:POW%20Rupees", "13:Heart%20Piece"), ("347:Heart%20Piece", "0:Heart%20Piece") } },
            { "PalaceSwitchHit", new[] { ("238:POW", "13:Switch%20Chest"), ("239:POW%20Rupees", "14:Switch%20Chest"), ("348:Switch%20Chest", "0:Switch%20Chest") } },
            { "PalacePreBoss", new[] { ("238:POW", "14:Bombarossa%20Maze"), ("239:POW%20Rupees", "15:Bombarossa%20Maze"), ("349:Bombarossa%20Maze", "0:Bombarossa%20Maze") } },
            { "PalaceBlockMaze", new[] { ("238:POW", "15:Block%20Maze%20Room"), ("239:POW%20Rupees", "16:Block%20Maze%20Room"), ("350:Block%20Maze%20Room", "0:Block%20Maze%20Room") } },
            { "PalaceDetour", new[] { ("238:POW", "16:Block%20Maze%20Detour"), ("239:POW%20Rupees", "17:Block%20Maze%20Detour"), ("351:Block%20Maze%20Room%20Detour", "0:Block%20Maze%20Room%20Detour") } },
            { "PalaceBossItem", new[] { ("238:POW", "17:Gyorg"), ("239:POW%20Rupees", "18:Gyorg"), ("352:Gyorg", "0:Gyorg") } },
            { "PalacePrize", new[] { ("238:POW", "17:Gyorg"), ("239:POW%20Rupees", "18:Gyorg"), ("352:Gyorg", "0:Gyorg") } },

            { "CastleKing", new[] { ("240:DHC", "2:Stone%20King"), ("241:DHC%20Open", "2:Stone%20King"), ("242:DHC%20Ped", "5:Stone%20King"), ("356:Stone%20King", "0:Stone%20King"), ("367:Stone%20King%20Open", "0:Stone%20King"), ("379:Stone%20King%20Ped", "0:Stone%20King") } },
            { "CastleBasement", new[] { ("240:DHC", "1:Platform%20Chest"), ("241:DHC%20Open", "1:Platform%20Chest"), ("242:DHC%20Ped", "4:Platform%20Chest"), ("355:Platform%20Chest", "0:Platform%20Chest"), ("366:Platform%20Chest%20Open", "0:Platform%20Chest"), ("378:Platform%20Chest%20Ped", "0:Platform%20Chest") } },
            { "CastleClones", new[] { ("240:DHC", "0:Blade%20Chest"), ("241:DHC%20Open", "0:Blade%20Chest"), ("242:DHC%20Ped", "3:Blade%20Chest"), ("354:Blade%20Chest", "0:Blade%20Chest"), ("365:Blade%20Chest%20Open", "0:Blade%20Chest"), ("377:Blade%20Chest%20Ped", "0:Blade%20Chest") } },
            { "CastlePostThrone", new[] { ("240:DHC", "3:Post%20Throne%20Big%20Chest"), ("241:DHC%20Open", "3:Post%20Throne%20Big%20Chest"), ("242:DHC%20Ped", "6:Post%20Throne%20Big%20Chest"), ("357:Post%20Throne%20Big%20Chest", "0:Post%20Throne%20Big%20Chest"), ("368:Post%20Throne%20Big%20Chest%20Open", "0:Post%20Throne%20Big%20Chest"), ("380:Post%20Throne%20Big%20Chest%20Ped", "0:Post%20Throne%20Big%20Chest") } },
            { "CastleTopLeftTower", new[] { ("240:DHC", "7:Northwest%20Tower"), ("241:DHC%20Open", "7:Northwest%20Tower"), ("242:DHC%20Ped", "10:Northwest%20Tower"), ("358:Northwest%20Tower", "0:Northwest%20Tower"), ("369:Northwest%20Tower%20Open", "0:Northwest%20Tower"), ("381:Northwest%20Tower%20Ped", "0:Northwest%20Tower") } },
            { "CastleTopRightTower", new[] { ("240:DHC", "4:Northeast%20Tower"), ("241:DHC%20Open", "4:Northeast%20Tower"), ("242:DHC%20Ped", "7:Northeast%20Tower"), ("361:Northeast%20Tower", "0:Northeast%20Tower"), ("372:Northeast%20Tower%20Open", "0:Northeast%20Tower"), ("384:Northeast%20Tower%20Ped", "0:Northeast%20Tower") } },
            { "CastleLowerLeftTower", new[] { ("240:DHC", "6:Southwest%20Tower"), ("241:DHC%20Open", "6:Southwest%20Tower"), ("242:DHC%20Ped", "9:Southwest%20Tower"), ("359:Southwest%20Tower", "0:Southwest%20Tower"), ("370:Southwest%20Tower%20Open", "0:Southwest%20Tower"), ("382:Southwest%20Tower%20Ped", "0:Southwest%20Tower") } },
            { "CastleLowerRightTower", new[] { ("240:DHC", "5:Southeast%20Tower"), ("241:DHC%20Open", "5:Southeast%20Tower"), ("242:DHC%20Ped", "8:Southeast%20Tower"), ("360:Southeast%20Tower", "0:Southeast%20Tower"), ("371:Southeast%20Tower%20Open", "0:Southeast%20Tower"), ("383:Southeast%20Tower%20Ped", "0:Southeast%20Tower") } },
            { "CastleBigBlock", new[] { ("240:DHC", "8:Big%20Block%20Chest"), ("241:DHC%20Open", "8:Big%20Block%20Chest"), ("242:DHC%20Ped", "11:Big%20Block%20Chest"), ("362:Big%20Block%20Chest", "0:Big%20Block%20Chest"), ("373:Big%20Block%20Chest%20Open", "0:Big%20Block%20Chest"), ("385:Big%20Block%20Chest%20Ped", "0:Big%20Block%20Chest") } }
        };
        private static readonly Dictionary<string, IEnumerable<KeyValuePair<string, int>>> SectionsByLocationDatabase =
            LocationDatabase.SelectMany(l => l.Value, (ls, l) => (locationSpoiler: ls.Key, l.location, l.section))
            .GroupBy(l => l.location, l => l.section)
            .ToDictionary(l => l.Key, l => l.GroupBy(s => s, (s, chests) => new KeyValuePair<string, int>(s, chests.Count())));

        public bool IsThisSpoilerLog(string content) => content.StartsWith("Spoiler for Minish Cap Randomizer");

        public ISpoilerLog BuildSpoilerLog(string content)
        {
            var spoilerLog = new SpoilerLogZeldaMinishCap();
            var contentLines = content.Split(Environment.NewLine);

            List<PairLocationItem> pairLocationItems = new();

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
                        pairLocationItems.Add(new PairLocationItem(parts[0], parts[1]));
                    else
                    {
                        var subvalueParts = contentLines[i + 1].Split(": ");
                        pairLocationItems.Add(new PairLocationItem(parts[0], parts[1], subvalueParts[1]));
                        i++;
                    }
                    i++;
                    continue;
                }
            }

            // Compile informations.
            Dictionary<string, SpoilerLog.Item> items = new();
            foreach (var pair in pairLocationItems)
            {
                var id = pair.Item;
                if (pair.Item != "KinstoneBag" && pair.Subvalue != null) id += $"_{pair.Subvalue}";

                if (items.TryGetValue(id, out var itemKey))
                    spoilerLog.ItemsLocations[itemKey].Add(pair.Location);
                else
                {
                    if (!ItemDatabase.TryGetValue(id, out var item))
                        item = new SpoilerLog.Item(id, pair.Item, int.MaxValue); // Unknown item
                    items.Add(id, item);

                    // Several id gives the same item
                    if (spoilerLog.ItemsLocations.ContainsKey(items[id]))
                        spoilerLog.ItemsLocations[items[id]].Add(pair.Location);
                    else
                        spoilerLog.ItemsLocations.Add(items[id], new() { pair.Location });
                }
            }

            return spoilerLog;
        }

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

        public Dictionary<SpoilerLog.Item, List<string>> ItemsLocations { get; } = new();

        public void SaveTracker(string filename, IEnumerable<SpoilerLog.Item> junkItems)
        {
            var settings = JsonConvert.DeserializeObject<EmoTrackerSettings>(File.ReadAllText(Path.Combine(App.GetEmoTrackerFolder(), "application_settings.json")));
            Tracker tracker = new()
            {
                ignore_all_logic = settings.tracking_ignore_all_logic,
                display_all_locations = settings.tracking_display_all_locations,
                always_allow_chest_manipulation = settings.tracking_always_allow_clearing_locations
            };
            //TODO: Read settings from spoiler.
            tracker.item_database.Add(new ProgressiveItem("95:progressive:Fusions", 1));

            Dictionary<string, Dictionary<string, int>> junkLocations = new();
            foreach (var junkItem in junkItems)
            {
                foreach (var location in ItemsLocations[junkItem])
                {
                    if (!LocationDatabase.TryGetValue(location, out var locationsWithSections)) continue; // Unknown location

                    foreach (var locationWithSection in locationsWithSections)
                    {
                        if (!junkLocations.ContainsKey(locationWithSection.location))
                            junkLocations.Add(locationWithSection.location, new Dictionary<string, int>(SectionsByLocationDatabase[locationWithSection.location]));
                        junkLocations[locationWithSection.location][locationWithSection.section]--;
                    }
                }
            }
            tracker.location_database.locations = junkLocations.Select(jl => new Location(jl.Key, jl.Value.Select(jls => new Section(jls.Key, jls.Value)).ToArray())).ToList();

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
#pragma warning disable 0649 // Deserialization
            public bool modified_by_user;
#pragma warning restore 0649
            public List<Section> sections = new();

            public Location(string locationReference, params Section[] sections)
            {
                location_reference = locationReference ?? throw new ArgumentNullException(nameof(locationReference));
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
