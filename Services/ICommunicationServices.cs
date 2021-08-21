using System.Threading.Tasks;

namespace Server
{
    public interface ICommunicationServices
    {
        /// <summary>
        /// Send text based email 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task SendEmail(Email email);
        
        /// <summary>
        /// Send html based email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task SendHtmlEmail(Email email);
    }
}