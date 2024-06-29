namespace UmsatzKategorisierung.Data
{
    public static class Categorizer
    {
        private static Dictionary<string, Category> categoryMapping = new Dictionary<string, Category>()
        {
            // Mappings for the 'Einkaufen' category
            { "e-center", Category.Einkaufen },
            { "baeckerei", Category.Einkaufen },
            { "k+u", Category.Einkaufen },
            { "sehne", Category.Einkaufen },
            { "lidl", Category.Einkaufen },
            { "kaufland", Category.Einkaufen },
            { "rewe", Category.Einkaufen },
            { "aktiv markt", Category.Einkaufen },
            { "pfannenbeschichtung24", Category.Einkaufen },
            { "prozis", Category.Einkaufen },
            { "metzgerei hanselmann", Category.Einkaufen },

            //Mappings for the Restaurant category
            { "hellofresh", Category.Restaurant },
            { "takeaway", Category.Restaurant },
            { "domino", Category.Restaurant },
            { "subway", Category.Restaurant },
            { "bertha s place", Category.Restaurant },
            { "mcdonalds", Category.Restaurant },
            { "chigo", Category.Restaurant },
            { "kfc", Category.Restaurant },

            //Mappings for the Miete category
            { "miete", Category.Wohnung },
            { "kaution", Category.Wohnung },
            { "rainer steinhilber", Category.Wohnung },
            { "vattenfall", Category.Wohnung },
            { "abfallgebühren", Category.Wohnung },
            { "abfallgebuehren", Category.Wohnung },
            { "landratsamt", Category.Wohnung },
            { "umzugshilfe", Category.Wohnung },

            //Mappings for the Bahn/Bus category
            { "db vertrieb", Category.Bahn_Bus },
            { "tuebus", Category.Bahn_Bus },
            { "bahnhof", Category.Bahn_Bus },
            { "stuttgarter strassenbahnen", Category.Bahn_Bus },

            //Mappings for the Health category
            { "apotheke", Category.Health },
            { "ina von der gracht", Category.Health },
            { "pvs reiss", Category.Health },

            //Mappings for the Tanken category
            { "aral", Category.Auto },
            { "esso", Category.Auto },
            { "omv", Category.Auto },
            { "shell", Category.Auto },
            { "mtb", Category.Auto },
            { "jet dankt", Category.Auto },
            { "tankstelle", Category.Auto },
            { "thomas bedrunka", Category.Auto },
            { "kfz", Category.Auto },
            { "landesoberkasse", Category.Auto },

            //Mappings for the Abos category
            { "e-plus service", Category.Abos },
            { "telekom", Category.Abos },
            { "audible", Category.Abos },
            { "amazon media eu", Category.Abos },
            { "kindle", Category.Abos },
            { "prime video", Category.Abos },
            { "amznprime", Category.Abos },
            { "netcup", Category.Abos },
            { "universitaet tuebingen", Category.Abos },
            { "rundfunk", Category.Abos },
            { "crunchyroll", Category.Abos },
            { "andreas schauer", Category.Abos },
            { "tsv heimerdingen", Category.Abos },
            { "mc shape", Category.Abos },

            //Mappings for the Bildung category
            { "stiftung medien in der bildung", Category.Bildung },
            { "universitaetskasse tuebingen", Category.Bildung },
            { "universitätskasse tuebingen", Category.Bildung },
            { "studierendenwerk", Category.Bildung },
            { "wiesingermedia", Category.Bildung },

            //Mappings for the Freizeit category
            { "dream bowl", Category.Event },
            { "dream-bowl", Category.Event },
            { "victory", Category.Event },
            { "paulaner", Category.Event },

            //Mappings for the Hobby category
            { "playstation", Category.Hobby },
            { "pe digital gmbh", Category.Hobby },
            { "mediamarkt", Category.Hobby },
            { "steampowered", Category.Hobby },
            { "sony", Category.Hobby },
            { "koelnmesse", Category.Hobby },
            { "lotto", Category.Hobby },
            { "fiverr", Category.Hobby },
            { "turn- und sportfreunde ditzingen", Category.Hobby },
            { "sportvg feuerbach", Category.Hobby },

            //Mappings for the Versicherung category
            { "generali", Category.Versicherung },
            { "hansemerkur", Category.Versicherung },
            { "aachenmuenchener", Category.Versicherung },
            { "huk-coburg", Category.Versicherung },

            //Mappings for the Bargeld category
            { "volksbank", Category.Banking },
            { "targobank", Category.Banking },
            { "bafög", Category.Banking },
            { "abschluss per", Category.Banking },
            { "abrechnung vom", Category.Banking },
            { "vr bank", Category.Banking },
            { "rabodirect", Category.Banking },
            { "auszahlung", Category.Banking },

            //Mappings for the Heimwerken category
            { "toom", Category.Heimwerken },
            { "ikea", Category.Heimwerken },
            { "bauhaus", Category.Heimwerken },
            { "hagebau", Category.Heimwerken },

            //Mappings for the Gehalt category
            { "alten", Category.Gehalt },
            { "daimler ag", Category.Gehalt },
            { "mercedes-benz", Category.Gehalt },

            //Mappings for the Sonstiges category
            { "amazon payments", Category.Sonstiges },
            { "zap-hosting", Category.Sonstiges },
            { "socialmatch", Category.Sonstiges },
            { "speeddating", Category.Sonstiges },
            { "mindfactory", Category.Sonstiges },
            { "gustini", Category.Sonstiges },
            { "ellation", Category.Sonstiges },
            { "nzxt", Category.Sonstiges },
            { "galaxus", Category.Sonstiges },
            { "amazon eu", Category.Sonstiges },
            { "paypal", Category.Sonstiges },
            { "butz", Category.Sonstiges },
            { "daniel", Category.Sonstiges },
            { "piechotta", Category.Sonstiges },
            { "christopher cura", Category.Sonstiges },
            { "stiftung", Category.Sonstiges },
            { "keller hairstyling", Category.Sonstiges },
            { "scheck", Category.Sonstiges },
            { "sixt", Category.Sonstiges },
            { "zinser", Category.Sonstiges },
            { "stadtkasse tübingen", Category.Sonstiges },
            { "post", Category.Sonstiges },
            { "b h24", Category.Sonstiges },
        };


        public static Category FindCategoryForTransaktion(Transaction data)
        {
            var receiver = data.ReceiverAccountName.ToLower();
            var bookingText = data.BookingText.ToLower();

            foreach (var entry in categoryMapping)
            {
                if (receiver.Contains(entry.Key) || bookingText.Contains(entry.Key))
                {
                    return entry.Value;
                }
            }

            Console.WriteLine($"No category found for: {data}");

            return Category.None;
        }
    }
}
