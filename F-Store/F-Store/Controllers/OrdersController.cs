using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using F_Store.Data;
using F_Store.Models;
using F_Store.ViewModels;

namespace F_Store.Controllers
{
    public class OrdersController : Controller
    {
        // Conecta la base de datos
        F_StoreContext db = new F_StoreContext();
        
        // GET: Orders
        public ActionResult NewOrder()
        {
            var orderView = new OrderView()
            {
                Customer = new Customer(),
                Products = new List<ProductOrder>()
            };
            Session["orderView"] = orderView;

            // Llenado de Combobox
            var list = db.Customers.ToList();
            list.Add(new Customer { CustomerID = 0, FirstName = "[Seleccione un cliente]" });
            list = list.OrderBy(c => c.FullName).ToList();
            ViewBag.CustomerID = new SelectList(list, "CustomerID", "FullName");
            
            return View(orderView);
        }

        // POST
        [HttpPost]
        public ActionResult NewOrder(OrderView orderView)
        {
            // Recuperar orden de compra
            orderView = Session["orderView"] as OrderView;

            var customerID = int.Parse(Request["CustomerID"]);

            // Validación
            if (customerID == 0)
            {
                // Llenado de Combobox
                var list = db.Customers.ToList();
                list.Add(new Customer { CustomerID = 0, FirstName = "[Seleccione una Description]" });
                list = list.OrderBy(c => c.FullName).ToList();
                ViewBag.CustomerID = new SelectList(list, "CustomerID", "FullName");

                ViewBag.Error = "Debe seleccionar un cliente";

                return View(orderView);
            }

            // Validación
            var customer = db.Customers.Find(customerID);
            if (customer == null)
            {
                // Llenado de Combobox
                var list = db.Customers.ToList();
                list.Add(new Customer { CustomerID = 0, FirstName = "[Seleccione una Description]" });
                list = list.OrderBy(c => c.FullName).ToList();
                ViewBag.CustomerID = new SelectList(list, "CustomerID", "FullName");

                ViewBag.Error = "Cliente no existe";
            }

            // Validación
            if(orderView.Products.Count == 0)
            {
                // Llenado de Combobox
                var list = db.Customers.ToList();
                list.Add(new Customer { CustomerID = 0, FirstName = "[Seleccione una Description]" });
                list = list.OrderBy(c => c.FullName).ToList();
                ViewBag.CustomerID = new SelectList(list, "CustomerID", "FullName");

                ViewBag.Error = "Debe ingresar detalle";

                return View(orderView);
            }

            int orderID = 0;

            // Commit
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Insertar en base de datos
                    var order = new Order
                    {
                        CustomerID = customerID,
                        OrderDate = DateTime.Now,
                        OrderStatus = OrderStatus.Created
                    };
                    db.Orders.Add(order);
                    db.SaveChanges();

                    // Seleccionar el maximo ID de la Orden con Linq
                    orderID = db.Orders.ToList().Select(o => o.OrderID).Max();

                    foreach (var item in orderView.Products)
                    {
                        var orderDetail = new OrderDetail
                        {
                            ProductID = item.ProductID,
                            Description = item.Description,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            OrderID = orderID // Sacado de base de datos con Linq
                        };
                        db.OrderDetails.Add(orderDetail);
                        db.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Llenado de Combobox
                    var list = db.Customers.ToList();
                    list.Add(new Customer { CustomerID = 0, FirstName = "[Seleccione una Description]" });
                    list = list.OrderBy(c => c.FullName).ToList();
                    ViewBag.CustomerID = new SelectList(list, "CustomerID", "FullName");

                    transaction.Rollback();
                    ViewBag.Error = "ERROR:" + ex.Message;
                    return View(orderView);
                }
            }
            // Llenado de Combobox
            var listC = db.Customers.ToList();
            listC.Add(new Customer { CustomerID = 0, FirstName = "[Seleccione una Description]" });
            listC = listC.OrderBy(c => c.FullName).ToList();
            ViewBag.CustomerID = new SelectList(listC, "CustomerID", "FullName");

            ViewBag.Message = string.Format("La orden: {0}, se ha insertado correctamente", orderID);

            // Limpia el grid de los productos añadidos, porque no puede ir null
            orderView = new OrderView()
            {
                Customer = new Customer(),
                Products = new List<ProductOrder>()
            };
            Session["orderView"] = orderView;
            return View(orderView);

        }

        private void RedirectToAction()
        {
            throw new NotImplementedException();
        }

        public ActionResult AddProduct()
        {
            var list = db.Products.ToList();
            list.Add(new Product { ProductID = 0, Description = "[Seleccione una Description]" });
            list = list.OrderBy(c => c.Description).ToList();
            ViewBag.ProductID = new SelectList(list, "ProductID", "Description");

            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(ProductOrder productOrder)
        {
            var orderView = Session["orderView"] as OrderView;

            var productID = int.Parse(Request["ProductID"]);

            if (productID == 0)
            {
                var list = db.Products.ToList();
                list.Add(new Product { ProductID = 0, Description = "[Seleccione una Description]" });
                list = list.OrderBy(c => c.Description).ToList();
                ViewBag.ProductID = new SelectList(list, "ProductID", "Description");
                ViewBag.Error = "Debe seleccionar un producto";

                return View(productOrder);
            }

            var product = db.Products.Find(productID);
            if(product == null)
            {
                var list = db.Products.ToList();
                list.Add(new Product { ProductID = 0, Description = "[Seleccione una Description]" });
                list = list.OrderBy(c => c.Description).ToList();
                ViewBag.ProductID = new SelectList(list, "ProductID", "Description");
                ViewBag.Error = "Producto No existe";

                return View(productOrder);
            }

            //Unificar registros iguales del grid con LinQ
            productOrder = orderView.Products.Find(p => p.ProductID == productID);
            if (productOrder == null)
            {
                productOrder = new ProductOrder
                {
                    Description = product.Description,
                    Price = product.Price,
                    ProductID = product.ProductID,
                    Quantity = float.Parse(Request["Quantity"])
                };
                orderView.Products.Add(productOrder);
            }
            else
            {
                productOrder.Quantity += float.Parse(Request["Quantity"]);
            }

            // Carga el combobox de Customer, porque se borra al retornar la lista
            var listC = db.Customers.ToList();
            listC.Add(new Customer { CustomerID = 0, FirstName = "[Seleccione un cliente]" });
            listC = listC.OrderBy(c => c.FullName).ToList();
            ViewBag.CustomerID = new SelectList(listC, "CustomerID", "FullName");

            return View("NewOrder", orderView);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}