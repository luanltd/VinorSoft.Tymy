using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VinorSoft.Tymy.Service.Entities;
using VinorSoft.Tymy.Service.Interface;
using VinorSoft.Tymy.Service.Model;

namespace VinorSoft.Tymy.Service.Service
{
   public class NotificationService:INotificationService
    {
        protected TymyDbContext _dbContext;

        public NotificationService(TymyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<Notifications> GetNotificationNotRead()
        {
            return this._dbContext.Set<Notifications>().Where(e => e.Active && !e.IsRead&&e.StaffId==LoginContext.Instance.CurrentUser.UserId).ToList();
        }
        public IList<Notifications> GetNotifications()
        {
            return this._dbContext.Set<Notifications>().Where(e => e.Active && e.StaffId == LoginContext.Instance.CurrentUser.UserId).OrderBy(e=>e.IsRead).ThenByDescending(e=>e.Created).Take(50).ToList();
        }

        public int Save(Notifications notifications)
        {
            this._dbContext.Notifications.Add(notifications);
            return _dbContext.SaveChanges();
        }

        public int UpdateIsRead(Guid id)
        {
            var notification = this._dbContext.Notifications.Find(id);
            notification.IsRead = true;
            _dbContext.Notifications.Update(notification);
            return this._dbContext.SaveChanges();
        }
        public int Cancel(Guid id)
        {
            var notification = this._dbContext.Notifications.Find(id);
            notification.Active = false;
            _dbContext.Notifications.Update(notification);
            return this._dbContext.SaveChanges();
        }
    }
}
