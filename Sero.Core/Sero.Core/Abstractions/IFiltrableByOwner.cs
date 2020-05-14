using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public interface IFiltrableByOwner
    {
        string OwnerId { get; }
        void SetRequiredOwnerId(string ownerCredentialId);
    }
}
