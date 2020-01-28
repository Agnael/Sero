//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Sero.Doorman.Services
//{
//    public class CredentialService : ICredentialService
//    {
//        public readonly ICredentialStore CredentialStore;
//        public readonly ICustomUserService CustomUserService;
//        public readonly IPermissionStore RoleStore;

//        public CredentialService(ICredentialStore credentialStore,
//                                ICustomUserService customUserService,
//                                IPermissionStore roleStore)
//        {
//            this.CredentialStore = credentialStore;
//            this.CustomUserService = customUserService;
//            this.RoleStore = roleStore;
//        }

//        public CredentialService(ICredentialStore credentialStore)
//        {
//            this.CredentialStore = credentialStore;
//        }

//        public async Task<bool> IsValidCredential(string email, string password)
//        {
//            Credential credential = await CredentialStore.FetchAsync(email);
//            return await this.IsValidCredential(credential, password);
//        }

//        public async Task<bool> IsValidCredential(Credential credential, string password)
//        {
//            string regeneratedHash = HashingUtil.GenerateHash(password, credential.PasswordSalt);

//            bool isValid = regeneratedHash.Equals(credential.PasswordHash);
//            return isValid;
//        }

//        public async Task CreateCredential(string email, string password, IEnumerable<string> roleList)
//        {
//            Credential credential = new Credential();
//            credential.Email = email;
//            credential.PasswordSalt = HashingUtil.GenerateSalt();
//            credential.PasswordHash = HashingUtil.GenerateHash(password, credential.PasswordSalt);
//            credential.CreationDate = DateTime.UtcNow;
//            credential.Permissions = new List<Permission>();

//            foreach(string roleName in roleList)
//            {
//                Permission role = await RoleStore.FetchAsync(roleName);

//                if (role == null)
//                    throw new UnexistingRoleException();

//                credential.Permissions.Add(role);
//            }

//            await CredentialStore.SaveAsync(credential);
//            await this.CustomUserService.OnCredentialCreated(credential);
//        }

//        public Task CreateCredential(string email, string password)
//        {
//            return this.CreateCredential(email, password, new string[] { "DefaultRole" });
//        }
//    }
//}
