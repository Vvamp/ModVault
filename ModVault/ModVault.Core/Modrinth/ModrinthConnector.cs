using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModVault.Core.Modrinth
{
    public class ModrinthConnector : ApiConnector
    {
        public async Task<Mod?> TryGetMod(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            var project = await new ModrinthApiCommunicator().GetProject(id);
            if (project == null) return null;

            var team = await new ModrinthApiCommunicator().GetTeamMembersByProject(project.id);
            var versions = await new ModrinthApiCommunicator().GetVersionsByProject(project.id);


            var ModFound = new Mod()
            {
                Name = project.title,
                Author = team?.members.FirstOrDefault(member => member.role.ToLower().Trim() == "owner")?.user.username,
                Source = ModSource.Modrinth,
                ReferenceId = project.id
            };

            if (versions != null && versions.versions.Count > 0)
            {
                foreach (var version in versions.versions)
                {
                    var primary_version_file = version.files.FirstOrDefault(f => f.primary);
                    if (primary_version_file == null)
                        primary_version_file = version.files.FirstOrDefault();

                    ModFound.VersionData.Add(new ModVersionData()
                    {
                        ModVersion = version.version_number,
                        VersionDate = DateTime.Parse(version.date_published),
                        GameVersions = version.game_versions.TryGetVersions(),
                        Loaders = ModVersionData.GetModLoaderByStrings(version.loaders),
                        PrimaryFile = new File()
                        {
                            url = primary_version_file.url,
                            filename = primary_version_file.filename,
                            hashes = primary_version_file.hashes
                        }
                    });
                }
            }

            return ModFound;

        }
    }
}
