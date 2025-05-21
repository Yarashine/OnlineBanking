namespace NotificationService.Application.MappingProfilies;

using AutoMapper;
using NotificationService.Application.DTOs.Requests;
using NotificationService.Application.DTOs.Responses;
using NotificationService.Domain.Entities;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<Notification, NotificationResponse>();
        CreateMap<CreateNotificationRequest, Notification>();
    }
}