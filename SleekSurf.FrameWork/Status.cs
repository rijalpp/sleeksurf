using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SleekSurf.FrameWork
{
    public enum Status
    {
        InActive = 0,//if the user is made inactive  , this will be the text in comment
        Activated = 1,//if the user is activated by email verification , this will be the text in comment
        InActiveByDefault,//this will be the default message on user creation, this will be the text in comment
        InActiveBySuperAdmin,//if the user is made inactive by superadmin, this will be the text in comment
        InActiveBySuperAdminForClient,//if the user is made in active by superadmin for client's user, this will be the text in comment
        InActiveByDeletion,//if the user is deleted, then it's inactive by deletion.
        InActiveByAccountExpiration,//if the account is expired and made incative to client, this will be the text in comment
    }
}
