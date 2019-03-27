using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using PacodelaCruz.DurableFunctions.Models;

namespace PacodelaCruz.DurableFunctions.Approval
{
    public static class SendApprovalRequestViaSlack
    {
        /// <summary>
        /// Activity Function. 
        /// Sends an Approval Request to a Slack App using an interactive button template. 
        /// More information at https://api.slack.com/docs/message-buttons
        /// </summary>
        /// <param name="requestMetadata"></param>
        /// <returns></returns>
        [FunctionName("SendApprovalRequestViaSlack")]
        public static async Task<string> Run(
                                            [ActivityTrigger] ApprovalRequestMetadata requestMetadata, 
                                            ILogger log)
        {
            string approvalRequestUrl = Environment.GetEnvironmentVariable("Slack:ApprovalUrl", EnvironmentVariableTarget.Process);
            string approvalMessageTemplate = Environment.GetEnvironmentVariable("Slack:ApprovalMessageTemplate", EnvironmentVariableTarget.Process);
            Uri uri = new Uri(requestMetadata.ReferenceUrl);
            string approvalMessage = string.Format(approvalMessageTemplate, requestMetadata.ReferenceUrl, requestMetadata.ApprovalType, requestMetadata.InstanceId, requestMetadata.ApplicantId, requestMetadata.ApplicationName);
            string resultContent;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(approvalRequestUrl);
                var content = new StringContent(approvalMessage, UnicodeEncoding.UTF8, "application/json");
                var result = await client.PostAsync(approvalRequestUrl, content);
                resultContent = await result.Content.ReadAsStringAsync();
                if (result.StatusCode != HttpStatusCode.OK)
                {
                    throw new HttpRequestException(resultContent);
                }
            }
            log.LogInformation($"Message regarding {requestMetadata.ApplicationName} sent to Slack!");
            return resultContent;
        }
    }
}
