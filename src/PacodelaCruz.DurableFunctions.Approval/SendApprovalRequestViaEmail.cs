using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.SendGrid;
using SendGrid.Helpers.Mail;
using PacodelaCruz.DurableFunctions.Models;
using Microsoft.Extensions.Logging;

namespace PacodelaCruz.DurableFunctions.Approval
{
    public static class SendApprovalRequestViaEmail
    {
        /// <summary>
        /// Sends an email with an Approval Request via SendGrid
        /// </summary>
        /// <param name="requestMetadata"></param>
        /// <param name="message"></param>
        /// <param name="log"></param>
        [FunctionName("SendApprovalRequestViaEmail")]
        public static void Run([ActivityTrigger]
                                ApprovalRequestMetadata requestMetadata, 
                                [SendGrid] out SendGridMessage message, 
                                ILogger log)
        {
            message = new SendGridMessage();
            message.AddTo(Environment.GetEnvironmentVariable("SendGrid:To"));
            message.AddContent("text/html", string.Format(Environment.GetEnvironmentVariable("SendGrid:ApprovalEmailTemplate"), requestMetadata.ApplicationName, requestMetadata.ApplicantId, requestMetadata.ReferenceUrl, requestMetadata.InstanceId, Environment.GetEnvironmentVariable("Function:BasePath")));
            message.SetFrom(Environment.GetEnvironmentVariable("SendGrid:From"));
            message.SetSubject(String.Format(Environment.GetEnvironmentVariable("SendGrid:SubjectTemplate"), requestMetadata.ApplicationName, requestMetadata.ApplicantId));
            log.LogInformation($"Message '{message.Personalizations[0].Subject}' sent!");
        }
    }
}
