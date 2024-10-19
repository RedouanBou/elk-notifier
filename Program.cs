using System;
using System.Collections.Generic;
using System.Timers;
using RestSharp;
using Newtonsoft.Json;
using NLog;
using DotNetEnv;

namespace ElkNotifier
{
    public class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static System.Timers.Timer _timer;
        private static List<AlertModel> _previousAlerts = new List<AlertModel>();
        private static string elasticsearchUrl;
        private static string webhookUrl;
        
        public static void Main(string[] args)
        {
            Env.Load();

            // Lees de URL's uit het .env-bestand
            elasticsearchUrl = Environment.GetEnvironmentVariable("ELASTICSEARCH_URL");
            webhookUrl = Environment.GetEnvironmentVariable("WEBHOOK_URL");

            Logger.Info("ELK Notifier has started.");
            
            _timer = new System.Timers.Timer(120000); // 2 minuten in milliseconden
            _timer.Elapsed += CheckForNewAlerts;
            _timer.AutoReset = true;
            _timer.Start();

            Console.WriteLine("Press Enter to exit the application...");
            Console.ReadLine();
        }

        private static void CheckForNewAlerts(Object source, ElapsedEventArgs e)
        {
            Logger.Info("Checking for new alerts...");

            List<AlertModel> newAlerts = GetAlertsFromElasticsearch();

            if (newAlerts != null && IsNewDataAvailable(newAlerts))
            {
                SendToWebhook(newAlerts);
                _previousAlerts = newAlerts;
            }
        }

        private static List<AlertModel> GetAlertsFromElasticsearch()
        {
            try
            {
                var client = new RestClient(elasticsearchUrl);
                var request = new RestRequest();
                request.Method = Method.Get;

                RestResponse response = client.Execute(request);

                if (!response.IsSuccessful)
                {
                    Logger.Error($"Error retrieving data:{response.ErrorMessage}");
                }

                var alerts = JsonConvert.DeserializeObject<List<AlertModel>>(response.Content);
                Logger.Info($"{alerts.Count} alerts received from Elasticsearch.");
                return alerts;
            }
            catch (Exception ex)
            {
                Logger.Error($"Er is een fout opgetreden: {ex.Message}");
            }

            return null;
        }

        private static bool IsNewDataAvailable(List<AlertModel> newAlerts)
        {
            return !JsonConvert.SerializeObject(newAlerts).Equals(JsonConvert.SerializeObject(_previousAlerts));
        }

        private static void SendToWebhook(List<AlertModel> alerts)
        {
            try
            {
                var client = new RestClient(webhookUrl);
                var request = new RestRequest();
                request.Method = Method.Post;

                request.AddJsonBody(alerts);

                RestResponse response = client.Execute(request);

                if (!response.IsSuccessful)
                {
                    Logger.Error($"Error sending to webhook:{response.ErrorMessage}");
                }
                
                Logger.Info("New data successfully sent to the webhook.");
            }
            catch (Exception ex)
            {
                Logger.Error($"An error occurred while sending to webhook:{ex.Message}");
            }
        }
    }
}