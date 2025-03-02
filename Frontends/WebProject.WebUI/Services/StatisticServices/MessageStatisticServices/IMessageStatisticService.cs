﻿namespace WebProject.WebUI.Services.StatisticServices.MessageStatisticServices
{
    public interface IMessageStatisticService
    {
        Task<int> GetTotalMessageCount();
        Task<int> GetReadedMessageCount();
        Task<int> GetNonReadedMessageCount();
    }
}
