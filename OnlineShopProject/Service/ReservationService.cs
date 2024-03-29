﻿using OnlineShopProject.Data;
using OnlineShopProject.Enums;
using OnlineShopProject.Models;

namespace OnlineShopProject.Service
{
    public class ReservationService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer hostedTimer;
        private Timer reservationTimer;
        private readonly ILogger<ReservationService> _logger;

        public ReservationService(IServiceProvider serviceProvider, ILogger<ReservationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            hostedTimer = new Timer(StartReservation, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            return Task.CompletedTask;
        }

        private async void StartReservation(object state)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

            var orderProducts = dbContext.OrderProduct
            .Where(op =>
                DateTime.Compare(DateTime.UtcNow, op.CreatedAt.AddMinutes(10)) < 0 &&
                op.Reserved == null
            ).ToList();

            foreach (var orderProduct in orderProducts)
            {
                orderProduct.Reserved = ReservationStatus.Reserved;
                await dbContext.SaveChangesAsync();
                _logger.LogInformation($"Reservation for orderProduct {orderProduct.OrderId}-{orderProduct.ProductId} started");
                reservationTimer = new Timer(EndReservation, orderProduct, TimeSpan.FromMinutes(10), Timeout.InfiniteTimeSpan);
            }
        }

        private async void EndReservation(object state)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

            var orderProduct = (OrderProduct)state;
            var orderProductEntity = dbContext.OrderProduct.First(op => op.OrderId == orderProduct.OrderId 
                && op.ProductId == orderProduct.ProductId);
            orderProductEntity.Reserved = ReservationStatus.NotReserved;
            await dbContext.SaveChangesAsync();
            _logger.LogInformation($"Reservation for orderProduct {orderProduct.OrderId}-{orderProduct.ProductId} ended");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            hostedTimer?.Change(Timeout.Infinite, 0);
            reservationTimer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            hostedTimer?.Dispose();
            reservationTimer?.Dispose();
        }
    }
}
