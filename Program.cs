using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;
using static System.Console;

//Source : https://github.com/rajanadar/VaultSharp
//Key Value Secrets Engine - V2
//https://github.com/rajanadar/VaultSharp#key-value-secrets-engine---v2

namespace myApp
{
    class Program
    {
        static async Task Main()
        {
            string path = "1cmd/dev";
            string mountPoint = "secret";
            string token = "s.3QsBJawBPvfPDD9eRLVzEkJL";
            string address = "http://localhost:8200/";
            // Initialize one of the several auth methods.
            IAuthMethodInfo authMethod = new TokenAuthMethodInfo(token);

            // Initialize settings. You can also set proxies, custom delegates etc. here.
            var vaultClientSettings = new VaultClientSettings(address, authMethod);

            IVaultClient vaultClient = new VaultClient(vaultClientSettings);
            var vaultData = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(path, mountPoint: mountPoint);

            var newData = new Dictionary<string, object> { { "key9", "val9" } };

            //var data = vaultData.Data.Data.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            // var newData = new Dictionary<string, object> { { "key7", "val7" }, { "key8", 8 } };

            // newData.ToList().ForEach(x => data.Add(x.Key, x.Value));

            var writtenValue = await vaultClient.V1.Secrets.KeyValue.V2.PatchSecretAsync(path, newData, mountPoint: mountPoint);

            vaultData = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(path, mountPoint: mountPoint);
            foreach (var key in vaultData.Data.Data.Keys)
            {
                vaultData.Data.Data.TryGetValue(key, out object value);
                WriteLine($"Key:{key} -> {value}");
            }
            // Generate a dynamic Consul credential
            // Secret<ConsulCredentials> consulCreds = await vaultClient.V1.Secrets.Consul.GetCredentialsAsync(consulRole, consulMount);
            // string consulToken = consulCredentials.Data.Token;
            Task.WaitAll();
        }
    }
}
