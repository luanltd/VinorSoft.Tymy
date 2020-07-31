using System;
using System.Collections.Generic;
using System.Text;
using VinorSoft.Tymy.Service.Entities;

namespace VinorSoft.Tymy.Service.Interface
{
  public interface INotificationService
    {
        IList<Notifications> GetNotificationNotRead();
        IList<Notifications> GetNotifications();
        int Save(Notifications notifications);
        int UpdateIsRead(Guid id);
    }
}
