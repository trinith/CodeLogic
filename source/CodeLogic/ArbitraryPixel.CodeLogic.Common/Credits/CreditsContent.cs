namespace ArbitraryPixel.CodeLogic.Common.Credits
{
    public static class CreditsContent
    {
        #region Pending Credits
        /*  This area for credits that haven't been included yet because they are part of a Work In Progress.
         *  
         * 
         * 
         */
        #endregion

        private static string attrTitle = "{C:White}{Font:Title}";
        private static string attrCredit = "{C:Gray}{Font:Credit}";
        private static string CreateTitle(string title) { return attrTitle + title + attrCredit; }

        public static string[] Credits = new string[]
            {
                "{Alignment:Centre}",
                "",
                CreateTitle("CodeLogic"),
                "Arbitrary Pixel",
                "Based on Mastermind, by Mordecai Meirowitz",
                "",
                CreateTitle("Design, Programming, and Graphics"),
                "Gary Texmo",
                "",
                CreateTitle("Sound"),
                "[www.freesound.org]",
                "waveplay",                 // ButtonPress, IndexValueChanged
                "kantouth",                 // SubmitSequence
                "pointparkcinema",          // WindowOpen, WindowClose
                "flathill",                 // Thunder1
                "deckycoss",                // AirplaneNormal (modified to create other airplane sounds)
                "",
                CreateTitle("Music"),
                "[www.soundimage.org]",
                "Eric Matyas",
                "",
                CreateTitle("Test Crew"),
                "Bill McDonald",
                "Charles Lee",
                "Edmond Louie",
                "Jamie Scales",
                "Justin Huskic",
                "Michael Writhe",
                "Tom McIvor",
                "",
                CreateTitle("Documentation Editing"),
                "April Taylor",
                "",
                CreateTitle("Additional Fonts"),
                "Mom's Typewriter by Christoph Mueller",
                "",
                CreateTitle("Additional Graphics"),
                "Tyler Weaver",
                "",
                "[www.flaticon.com]",
                "Freepik",
                "Google",
                "GraphicsBay",
                "Rami McMin",
                "Bogdan Rosu",
                "",
                CreateTitle("Special Thanks"),
                "Katrina Evanochko",
                "",
                "",
                "",
                "",
                CreateTitle("Built using..."),
                "MonoGame",
            };
    }
}
