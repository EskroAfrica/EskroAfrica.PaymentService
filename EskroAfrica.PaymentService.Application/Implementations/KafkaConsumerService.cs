﻿using Confluent.Kafka;
using EskroAfrica.PaymentService.Application.Interfaces;
using EskroAfrica.PaymentService.Common.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EskroAfrica.PaymentService.Application.Implementations
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly AppSettings _appSettings;
        private readonly IServiceProvider _serviceProvider;

        public KafkaConsumerService(AppSettings appSettings, IServiceProvider serviceProvider)
        {
            _appSettings = appSettings;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var logger = _serviceProvider.GetRequiredService<ILogService>();
            var topics = new List<string>();

            using (var consumer = new ConsumerBuilder<string, string>(_appSettings.KafkaSettings.ConsumerConfig).Build())
            {
                consumer.Subscribe(topics);

                while (true)
                {
                    var consumeResult = consumer.Consume();
                }
            }
        }
    }
}
