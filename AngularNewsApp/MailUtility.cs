using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AngularNewsApp
{
    public class MailUtility
    {
        //private SmtpClient smtpClient;

        public MailUtility()
        {
            //Instanciation du client
            //smtpClient = new SmtpClient("smtp.gmail.com", 587);
            ////On indique au client d'utiliser les informations qu'on va lui fournir
            //smtpClient.UseDefaultCredentials = false;
            ////Ajout des informations de connexion
            //smtpClient.Credentials = new System.Net.NetworkCredential("lenfant.chris@gmail.com", "c#55&@2soU/&quot;Vt");
            ////On indique que l'on envoie le mail par le réseau
            //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            ////On active le protocole SSL
            //smtpClient.EnableSsl = true;
       }

        public bool SendMail(string subject, string body)
        {
            string _sender = "lenfant.chris@hotmail.fr";
            string _password = "Mmajjbmt15!14";

            SmtpClient client = new SmtpClient("smtp-mail.outlook.com");

            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            NetworkCredential credentials = new NetworkCredential(_sender, _password);
            client.EnableSsl = true;
            client.Credentials = credentials;

            MailMessage message = new MailMessage(_sender, "lenfant.chris@gmail.com");
            message.Subject = subject;
            message.Body = body;




            try
            {
                client.Send(message);

            }
            catch (Exception ex)
            {
                return false;
            }



            //MailMessage mail = new MailMessage();
            ////Expéditeur
            //mail.From = new MailAddress("newsapp@hotmail.fr", "NewsApp");
            ////Destinataire
            //mail.To.Add(new MailAddress("lenfant.chris@hotmail.fr"));

            //mail.Subject = subject;

            //mail.Body = body;

            //try
            //{
            //    smtpClient.Send(mail);

            //}
            //catch(Exception ex)
            //{
            //    return false;
            //}


            return true;
        }
    }
}
