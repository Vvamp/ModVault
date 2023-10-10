using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModVault.Core.Modrinth.Models
{
    public class License
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class ModeratorMessage
    {
        public string message { get; set; }
        public string? body { get; set; }
    }

    public class DonationUrl
    {
        public string? id { get; set; }
        public string? platform { get; set; }
        public string? url { get; set; }
    }
    public class Gallery
    {
        public string url { get; set; }
        public bool featured { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public string created { get; set; }
        public int? ordering { get; set; }
    }
    public class Project
    {
        public string slug { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string[] categories { get; set; }
        public string client_side { get; set; }
        public string server_side { get; set; }
        public string body { get; set; }
        public string[]? additional_categories { get; set; }
        public string? issues_url { get; set; }
        public string? source_url { get; set; }
        public string? wiki_url { get; set; }
        public string? discord_url { get; set; }
        public DonationUrl[]? donation_urls { get; set; }
        public string project_type { get; set; } // enum?
        public int downloads { get; set; }
        public string? icon_url { get; set; }
        public int? color { get; set; }
        public string id { get; set; }
        public string team { get; set; }
        public string? body_url { get; set; }
        public ModeratorMessage? moderator_message { get; set; }
        public string published { get; set; }
        public string updated { get; set; }
        public string? approved { get; set; }
        public int followers { get; set; }
        public string status { get; set; } // enum: approved, rejected, draft, unlisted, archived, processsing, unknown
        public License? license { get; set; }
        public string[]? versions { get; set; }
        public string[]? game_versions { get; set; }
        public string[]? loaders { get; set; }
        public Gallery[]? gallery { get; set; }


    }

}
