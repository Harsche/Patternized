namespace PrototypePattern {
    public class FilePaths {
        private const string HOME_DIRECTORY_SYMBOL = "~/";

        public static readonly string resources_sprites = "Sprites";
        public static readonly string resources_artist = $"{resources_sprites}/Artist";
        public static readonly string resources_astrologer = $"{resources_sprites}/Astrologer";
        public static readonly string resources_blacksmith = $"{resources_sprites}/Blacksmith";
        public static readonly string resources_blacksmith2 = $"{resources_sprites}/Blacksmith 2";
        public static readonly string resources_citizen = $"{resources_sprites}/Citizen";
        public static readonly string resources_herbalist = $"{resources_sprites}/Herbalist";
        public static readonly string resources_hunter = $"{resources_sprites}/Hunter";
        public static readonly string resources_jeweler = $"{resources_sprites}/Jeweler";
        public static readonly string resources_jeweler2 = $"{resources_sprites}/Jeweler 2";
        public static readonly string resources_sage = $"{resources_sprites}/Sage";
        public static readonly string resources_warlord = $"{resources_sprites}/Warlord";

        public static string GetPathToResource(string defaultPath, string resourceName) {
            if (resourceName.StartsWith(HOME_DIRECTORY_SYMBOL))
                return resourceName.Substring(HOME_DIRECTORY_SYMBOL.Length);

            return $"{defaultPath}/{resourceName}";
        }
    }
}