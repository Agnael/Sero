using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Gatekeeper.Storage
{
    public class ResourceSeeder
    {
        public virtual async Task Seed(IResourceStore store)
        {
            string category = "Sero.Gatekeeper";

            bool isExistingResourcesResource = await store.IsExisting(GtkResourceCodes.Resources);
            if (isExistingResourcesResource)
            {
                await store.Create(
                    category,
                    GtkResourceCodes.Resources,
                    "Gatekeeper resources administration");
            }

            bool isExistingRolesResource = await store.IsExisting(GtkResourceCodes.Roles);
            if (isExistingRolesResource)
            {
                await store.Create(
                    category,
                    GtkResourceCodes.Roles,
                    "Gatekeeper role administration");
            }
        }
    }
}
