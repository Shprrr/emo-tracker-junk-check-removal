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
            { "TowerTopRight", new[] { ("157:Wind%20Tribe%20House", "2:House%20Chests"), ("112:Wind%20Tribe%20House%20Open", "1:Later%20House%20Chests") } }
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
