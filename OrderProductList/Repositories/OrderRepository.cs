using OrderProductList.Context;
using OrderProductList.Model;
using OrderProductList.Repositories.Interfaces;
using Dapper;

namespace OrderProductList.Repositories
{
    public class OrderRepository: IOrderRepository,IProductRepository
    {
        private readonly DapperContext _context;
        public OrderRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<double> AddProduct(List<Product> products, int cid)
        {
            int res=0;
            double grandtotal = 0;
            using(var connection=_context.CreateConnection())
            {
                if(products.Count>0)
                {
                   
                    foreach (var product in products)
                    {
                        product.orderId = cid;
                        product.totalAmount = product.productPrice * product.quantity;

                        var qry = @"insert into tblProduct(productName,productPrice,quantity,totalAmount,orderId)
                            values(@productName,@productPrice,@quantity,@totalAmount,@orderId)";
                        var res1 = await connection.ExecuteAsync(qry, product);
                        res = res + res1;
                        grandtotal+= product.totalAmount;


                    }
               
                }
                return grandtotal;
            }
            
        }

        public async Task DeleteOrder(int id)
        {
            var qry = "delete from tblOrder where orderId=@orderId";
            using(var connection=_context.CreateConnection())
            {
                await connection.ExecuteAsync(qry, new { orderId = id });
                await connection.ExecuteAsync("delete  from tblProduct where orderId=@orderId", new { orderId = id });
            }
       
        }

        public async Task<IEnumerable<Order>> GetAllOrder()
        {
            List<Order> orders = new List<Order>();
            var qry = "select * from tblOrder";
            using (var connection=_context.CreateConnection() )
            {
                var order=await connection.QueryAsync<Order>(qry);
                orders=order.ToList();
                foreach(var od in orders)
                {
                    var qry1 = "select * from tblProduct where orderId=@id";
                    var product=await connection.QueryAsync<Product>(qry1,new { id = od.orderId });
                    od.products = product.ToList();
                }
                return orders;
            }

        }

        public async Task<Order> GetOrderById(int id)
        {
            var order=new Order();
            var qry = "select * from tblOrder where orderId=@orderId";
            using (var connection=_context.CreateConnection())
            {
                 order=await connection.QueryFirstOrDefaultAsync<Order>(qry, new {orderId=id});
                
                    var qry1 = "select * from tblProduct where orderId=@id";
                    var product=await connection.QueryAsync<Product>(qry1, new {id=id});
                    order.products=product.ToList();
                
                return order;
            }
        }

        public async Task<int> InsertOrder(Order order)
        {
            var qry = @"insert into tblOrder(custName,shippingAddress,orderDate,finalAmount)
                        values(@custName,@shippingAddress,@orderDate,@finalAmount);
                             SELECT CAST(SCOPE_IDENTITY() as int)";
            using(var connection=_context.CreateConnection())
            {
                var res=await connection.QuerySingleAsync<int>(qry,order);
               if(res!=0)
                {
                    double fa=await AddProduct(order.products,res);
                    await connection.ExecuteAsync("update tblOrder set finalAmount=@finalAmount where orderId=@id", new { finalAmount = fa, id = res });
                }
                return res;
            }
           
         }

        public async Task<int> UpdateOrder(Order order)
        {
            var qry = @"update tblorder set custName=@custName,shippingAddress=@shippingAddress,
                    orderDate=@orderDate,finalAmount=@finalAmount where orderId=@orderId ";
            using(var connection=_context.CreateConnection())
            {
                var res = await connection.ExecuteAsync(qry, order);
               if(res != 0)
                {
                    await connection.ExecuteAsync("delete from tblProduct where orderId=@id", new { id = order.orderId });
                    await AddProduct(order.products, order.orderId);
                }
                return res;
            }
        }
    }
}
