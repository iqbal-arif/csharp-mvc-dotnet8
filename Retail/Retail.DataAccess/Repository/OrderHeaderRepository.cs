using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Retail.DataAccess.Data;
using Retail.DataAccess.Repository.IRepository;
using Retail.Models;

namespace Retail.DataAccess.Repository
{
    public class OrderHeaderRepository: Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db)
            :base(db) 
        {
            _db = db;
        }

        
        public void Update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }

        //Updating Status
        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            //Retrive OrderHeader from DB based on ID and Update OrderStatus
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if (orderFromDb != null)
            {
                //Order Status
                orderFromDb.OrderStatus = orderStatus;

                //Payment Status
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }

            }
        }
        
        //Payment Intent ID  and Session Id
        //Session Id is generated at payment attempt, upon successful status then Payment Intent Id is generated
        public void UpdateStripePaymentID(int id, string sessionId, string paymentIntenId)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);

            if (!string.IsNullOrEmpty(sessionId))
            {
                orderFromDb.SessionId = sessionId;
            }
            if (!string.IsNullOrEmpty(paymentIntenId))
            {
                orderFromDb.PaymentIntentId = paymentIntenId;
                //and update the date of payment 
                orderFromDb.PaymentDate = DateTime.Now;
            }
        }
    }
}
