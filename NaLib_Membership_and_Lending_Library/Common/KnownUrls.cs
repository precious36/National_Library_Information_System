using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaLib_Membership_and_Lending_Lib.Common
{
    public class KnownUrls
    {

        public const string BaseUrl = "api/v1";
        public const string Member = BaseUrl+"/Member";
        public const string GetAllMembers = "get_all_members";
        public const string GetMemberById = "get_member/{id}";
        public const string CreateMember = "create_member";
        public const string UpdateMember = "update_member/{id}";
        public const string DeleteMember = "delete_member/{id}";

        public const string LendingTransactionBaseUrl = BaseUrl + "/lending-transactions";
        public const string GetAllLendingTransactions = "get_all_transations";
        public const string GetLendingTransactionById = "get_transaction/{id}";
        public const string CreateLendingTransaction = "create_transaction";
        public const string UpdateLendingTransaction = "update_transaction/{id}";
        public const string DeleteLendingTransaction = "delete_transaction/{id}";


        public const string LendingPreferenceBaseUrl = BaseUrl + "/lending-preferences";
        public const string GetAllLendingPreferences = "get_all_preferences";
        public const string GetLendingPreferenceById = "get_preference/{id}";
        public const string CreateLendingPreference = "create_preference";
        public const string UpdateLendingPreference = "updat_preference/{id}";
        public const string DeleteLendingPreference = "delete_preference/{id}";

        public const string DamageAndLossBaseUrl = BaseUrl + "/damage-and-loss";
        public const string GetAllDamageAndLoss = "get_all_damage_loss";
        public const string GetDamageAndLossById = "get_damage_loss/{id}";
        public const string CreateDamageAndLoss = "create_damage_loss";
        public const string UpdateDamageAndLoss = "update_damage_loss/{id}";
        public const string DeleteDamageAndLoss = "delete_damage_loss/{id}";
    }

}



