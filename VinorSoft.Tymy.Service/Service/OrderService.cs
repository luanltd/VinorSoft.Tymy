using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VinorSoft.Tymy.Service.Constants;
using VinorSoft.Tymy.Service.Entities;
using VinorSoft.Tymy.Service.Interface;
using VinorSoft.Tymy.Service.Model;

namespace VinorSoft.Tymy.Service.Service
{
    public class OrderService : IOrderService
    {
        protected TymyDbContext _dbContext;
        public OrderService(TymyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IList<Orders> GetAll_User()
        {
            return this._dbContext.Set<Orders>().Where(e => e.Active&&e.StaffID==LoginContext.Instance.CurrentUser.UserId).ToList();
        }
        public int TotalOrder()
        {
            return  this._dbContext.Orders.Count();
        }
        public IList<Orders> GetAllOrderDetail_User(DateTime? date)
        {
            if (date.HasValue)
            {
                return this._dbContext.Set<Orders>().Where(e => e.Active && e.OrderDate.Value.Date == date.Value.Date && e.StaffID == LoginContext.Instance.CurrentUser.UserId).Select(e => new Orders()
                {
                    ID = e.ID,
                    CustomerId = e.CustomerId,
                    Customers = new Customers()
                    {
                        ID = e.Customers.ID,
                        Phone = e.Customers.Phone,
                        CustomerName = e.Customers.CustomerName,
                    },
                    OrderDate = e.OrderDate,
                    Note = e.Note,
                    OrderNo = e.OrderNo,
                    StatusId = e.StatusId,
                    OrderDetails = e.OrderDetails.Where(a=>a.Active).Select(a=>new OrderDetails() { 
                        OrderId=a.OrderId,
                        ProductId=a.ProductId,
                        UnitPrice=a.UnitPrice,
                        Discount=a.Discount,
                        Active=a.Active,
                        Quantity=a.Quantity
                    }).ToList(),
                    SellerID = e.SellerID,
                    StaffID = e.StaffID,
                    Active = e.Active,
                    TableId = e.TableId,
                }).ToList();
            }
            else
            {
                return this._dbContext.Set<Orders>().Where(e => e.Active&&e.StaffID==LoginContext.Instance.CurrentUser.UserId).Select(e => new Orders()
                {
                    ID = e.ID,
                    CustomerId = e.CustomerId,
                    Customers = new Customers()
                    {
                        ID = e.Customers.ID,
                        Phone = e.Customers.Phone,
                        CustomerName = e.Customers.CustomerName,
                    },
                    OrderDate = e.OrderDate,
                    Note = e.Note,
                    OrderNo = e.OrderNo,
                    StatusId = e.StatusId,
                    OrderDetails = e.OrderDetails.Where(a => a.Active).Select(a => new OrderDetails()
                    {
                        OrderId = a.OrderId,
                        ProductId = a.ProductId,
                        UnitPrice = a.UnitPrice,
                        Discount = a.Discount,
                        Active = a.Active,
                        Quantity = a.Quantity
                    }).ToList(),
                    SellerID = e.SellerID,
                    StaffID = e.StaffID,
                    Active = e.Active,
                    TableId = e.TableId,
                }).ToList();
            }
        }

        public Orders GetById(string orderId)
        {
            var order = this._dbContext.Set<Orders>().Where(e => e.Active && e.ID == orderId).Select(e => new Orders()
            {

                ID = e.ID,
                CustomerId = e.CustomerId,
                Customers = e.Customers,
                Note = e.Note,
                OrderDate = e.OrderDate,
                OrderDetails = e.OrderDetails.Where(a => a.Active).Select(a => new OrderDetails()
                {
                    OrderId = a.OrderId,
                    ProductId = a.ProductId,
                    UnitPrice = a.UnitPrice,
                    Discount = a.Discount,
                    Active = a.Active,
                    Quantity = a.Quantity,
                    Products = new Products()
                    {
                        ID=a.Products.ID,
                        ProductName=a.Products.ProductName,
                    },
                }).ToList(),
                OrderNo = e.OrderNo,
                SellerID = e.SellerID,
                StaffID = e.StaffID,
                Staffs = e.Staffs,
                StatusId = e.StatusId,
                TableId = e.TableId,
                Tables = e.Tables,
                Active = e.Active
            }).FirstOrDefault();

            return order;
        }

        public int Save(Orders item)
        {
            var order = this._dbContext.Orders.Find(item.ID);
           // var totalOrder = this._dbContext.Orders.Count() + 1;
            if (order != null)
            {
                order.Note = item.Note;
                //order.OrderDate = item.OrderDate;
                order.OrderNo = item.OrderNo;
                order.SellerID = !string.IsNullOrEmpty(item.SellerID) ? item.SellerID : null;
                order.StaffID = !string.IsNullOrEmpty(item.StaffID) ? item.StaffID : null;
                order.CustomerId = !string.IsNullOrEmpty(item.CustomerId) ? item.CustomerId : null;
                order.StatusId = item.StatusId;
                order.TableId = item.TableId;
                order.OrderDetails =null ;
                var orderDetails = item.OrderDetails;
                order.Active = item.Active;
                _dbContext.Orders.Update(order);
                _dbContext.OrderDetails.AddRange(orderDetails);
                //if (orderDetails != null && orderDetails.Count > 0)
                //{
                //    foreach (var detail in orderDetails)
                //    {
                //        if(_dbContext.OrderDetails.Any(e =>e.Active&& e.OrderId == detail.OrderId && e.ProductId == detail.ProductId))
                //        {
                //            _dbContext.OrderDetails.Update(detail);

                //        }
                //        else
                //        {
                //            _dbContext.OrderDetails.Add(detail);

                //        }
                //    }
                //}
            }
            else
            {

                //kiểm tra nếu bàn bận khi thông báo không cho lưu
                var orderTable = this._dbContext.Set<Orders>().Where(e => e.Active && e.StatusId != (int)TymyConstants.Status.AddNew && e.OrderDate.Value.Date == DateTime.Now.Date).Select(e => e.TableId).Distinct().ToList();
                var tableIds = this._dbContext.Set<Tables>().Where(e => e.Active && !orderTable.Contains(e.ID)).Select(e=>e.ID).ToList();
                if (tableIds.Contains(item.TableId))
                {
                    var addOrder = new Orders();
                    //addOrder.ID = $"{DateTime.Now.Year}-{totalOrder}";
                    addOrder.ID = item.ID;
                    addOrder.Note = item.Note;
                    addOrder.OrderDate = item.OrderDate;
                    //addOrder.OrderNo = totalOrder;
                    addOrder.OrderNo = item.OrderNo;
                    addOrder.SellerID = !string.IsNullOrEmpty( item.SellerID)?item.SellerID:null;
                    addOrder.CustomerId = !string.IsNullOrEmpty(item.CustomerId) ? item.CustomerId : null; 
                    addOrder.StaffID = !string.IsNullOrEmpty(item.StaffID) ? item.StaffID : null;
                    addOrder.StatusId = item.StatusId;
                    addOrder.TableId = item.TableId;
                    addOrder.Active = true;
                    if (item.OrderDetails != null && item.OrderDetails.Count > 0)
                    {
                        foreach (var orderDetail in item.OrderDetails)
                        {
                            orderDetail.OrderId = addOrder.ID;
                        }
                    }
                    addOrder.OrderDetails = item.OrderDetails;
                    _dbContext.Orders.Add(addOrder);
                }
                else
                {
                    throw new Exception("Bàn đã được đặt chỗ");
                }
               
            }
            return _dbContext.SaveChanges();
        }
        public int Cancel(string orderId)
        {
            var order = this._dbContext.Orders.Find(orderId);
            if (order == null)
                throw new Exception($"Không tìm thấy đơn hàng có mã: {orderId}");
            order.Active = false;
            var orderDtails = this._dbContext.OrderDetails.Where(i => i.OrderId == orderId).ToList();
            if (orderDtails != null && orderDtails.Count > 0)
            {
                orderDtails.ForEach(e => e.Active = false);
                //foreach (var item in orderDtails)
                //{
                //    this._dbContext.OrderDetails.Update(item);
                //}
                this._dbContext.OrderDetails.UpdateRange(orderDtails);
            }
            this._dbContext.Orders.Update(order);
            return _dbContext.SaveChanges();
        }

        public int Delete(string orderId)
        {
            var order = this._dbContext.Orders.Find(orderId);

            if (order == null)
                throw new Exception($"Không tìm thấy đơn hàng có mã: {orderId}");

            var orderDtails = this._dbContext.OrderDetails.Where(i => i.OrderId == orderId).ToList();
            if (orderDtails != null && orderDtails.Count > 0)
            {
                foreach (var item in orderDtails)
                {
                    this._dbContext.OrderDetails.Remove(item);
                }
            }
            this._dbContext.Orders.Remove(order);
            return _dbContext.SaveChanges();
        }
    }
}

