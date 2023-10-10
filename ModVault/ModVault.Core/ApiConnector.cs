using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModVault.Core
{
    public interface ApiConnector
    {
        public Task<Mod?> TryGetMod(string id);

    }
}
