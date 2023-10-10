using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModVault.Core.Modrinth.Models
{
    public class Dependency
    {
        public string? version_id { get; set; }
        public string? project_id { get; set; }
        public string? file_name { get; set;}
        public string dependency_type { get; set; } // enum: required, optional, incompatbile, embedded
    }


    public class File
    {
        public Dictionary<string, string> hashes { get; set; }
        public string url { get; set; }
        public string filename { get; set; }
        public bool primary { get; set; }
        public int size { get; set; }
        public string? file_type { get; set; } //enum: required-resource-pack, optional-resource-pack, null
    }

    public class Version
    {
        public string name { get; set; }
        public string version_number { get; set; }
        public string? changelog { get; set; }
        public Dependency[]? dependencies { get; set; }
        public string[] game_versions { get; set; }
        public string version_type { get; set; } // Enum: release, beta, alpha?
        public string[] loaders { get; set; }
        public bool featured { get; set; }
        public string? status { get; set; } //enum: listed, archived, draft, unlisted, scheduled, unknonw
        public string? requested_status { get; set; } // Enum: listed, archived, draft, unlisted
        public string id { get; set; }
        public string project_id { get; set; }
        public string author_id { get; set; }
        public string date_published { get; set; } // date in iso-8601
        public int downloads { get; set; }
        public File[]? files { get; set; }
    }

    public class Versions
    {
        public List<Version> versions = new List<Version>();
    }
}
