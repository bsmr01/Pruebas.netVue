namespace Ibero.Services.Utilitary.Domain.Infrastructure.Abstract
{
    using Ibero.Services.Utilitary.Core.Models;
    using System.Threading.Tasks;

    public interface IExternalService
    {
        Task<Transaction> PUTExternalService
        (
           object registrationForm,
           string registrationUrl
        );

        Transaction PUTExternalSyncService
        (
           object registrationForm,
           string registrationUrl
        );

        Task<Transaction> POSTExternalService
        (
           object registrationForm,
           string registrationUrl,
           bool automatic = true
        );
        Task<Transaction> POSTExternalServiceJSON
        (
           string registrationForm,
           string registrationUrl,
           bool automatic = true
        );

        Task<Transaction> POSTExternalServiceToken
        (
         object registrationForm,
         string registrationUrl,
         string tokenRef,
         bool automatic
        );
        Task<string> GETExternalServiceTokenFile
       (
        string registrationUrl,
        string tokenRef,
        string direction,
        string NAmeFile
       );
        Task<Transaction> POSTExternalServiceFileToken
        (
         object registrationForm,
         string registrationUrl,
         string tokenRef,
         string NAmeFile,
         byte[] data
        );

        Task<Transaction> POSTExternalServiceRefeshToken
       (
        object registrationForm,
        string registrationUrl,
        string tokenRef,
        string clientid,
        string clientsecret
       );

      //  Task<Transaction> POSTExternalServiceUpdateCode
      //(
      // object registrationForm,
      // string registrationUrl,
      // string Code,
      // string clientid,
      // string clientsecret
      //);

        Task<string> GETExternalService
        (
           string registrationUrl
        );

        Task<string> GETExternalServiceToken
        (
           string registrationUrl,
           string tokenRef
        );     
    }
}