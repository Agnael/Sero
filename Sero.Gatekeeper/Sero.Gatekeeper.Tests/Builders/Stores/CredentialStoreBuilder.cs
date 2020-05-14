using Moq;
using Sero.Gatekeeper.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper.Tests
{
    public class CredentialStoreBuilder
    {
        public readonly IRoleStore RoleStore;
        public readonly List<Credential> CredentialList;

        public CredentialStoreBuilder(IRoleStore roleStore)
        {
            RoleStore = roleStore;
            CredentialList = new List<Credential>();
        }

        public CredentialStoreBuilder WithDefaultCredentials()
        {
            Credential cred1 =
                new CredentialBuilder("cred_01")
                    .WithBirthdate(1994, 10, 26)
                    .WithCreationDate(2000, 1, 2)
                    .AddRole(RoleData.Role_01_Admin)
                    .Build();

            Credential cred2 =
                new CredentialBuilder("cred_02")
                    .WithBirthdate(1998, 10, 26)
                    .WithCreationDate(1998, 1, 2)
                    .AddRole(RoleData.Role_02_User)
                    .Build();

            CredentialList.Add(cred1);
            CredentialList.Add(cred2);

            return this;
        }

        public CredentialStoreBuilder AddCredential(Credential credential)
        {
            CredentialList.Add(credential);
            return this;
        }

        public InMemoryCredentialStore Build()
        {
            var credentialStore = new InMemoryCredentialStore(CredentialList);
            new CredentialSeeder().Seed(RoleStore, credentialStore).Wait();
            return credentialStore;
        }
    }
}
