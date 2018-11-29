using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CrudImplementations;
using Model;

namespace Chapter8Basis
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create an order object to use and add ToString method
            Order myOrder = new Order();
            myOrder.id = Guid.NewGuid();
            myOrder.product = "Zenbo";
            myOrder.amount = 2;
            Console.WriteLine(myOrder.ToString());

            Console.WriteLine("=========CreateSeparateServices=========");
            OrderController sep = CreateSeparateServices();
            //Use the sep OrderController: CreateOrder(), DeleteOrder() 
            // CreateOrder() methods: (1)Main(), (2)CreateSeparateServices, (3)OrderController() constructor, 
            // (4)sep.CreateOrder(myOrder) in OrderController, (5)saver.Save(order) in Saver<TEntity> : ISave<TEntity> class, 
            //(6) Save(TEntity entity) in class Save<TEntity> : ISave<TEntity> CrudImplementations 
            sep.CreateOrder(myOrder);

            // DeleteOrder() methods: (1)Main(), (2)CreateSeparateServices, (3)OrderController() constructor, 
            // (4)sep.DeleteOrder(myOrder) in OrderController, (5)deleter.Delete(order) in Deleter<TEntity> : IDelete<TEntity> class
            //(6) Delete(TEntity entity) in class  Deleter<TEntity> : IDelete<TEntity> in CrudImplementations
            sep.DeleteOrder(myOrder);

            // Use the sing OrderController: 
            Console.WriteLine("=========CreateSingleService=========");
            OrderController sing = CreateSingleService();

            // sing.CreateOrder methods: (1) Main, (2) CreateSingleService in Program class (3) OrderController(crud, crud, crud) constructor in OrderController class
            // (4) CreateOrder(Order order) in OrderController Class, (5)  saver.Save(order) in Crud<TEntity> : IRead<TEntity>, ISave<TEntity>, IDelete<TEntity> ,
            // (6)Save(TEntity entity) in Crud<TEntity> : IRead<TEntity>, ISave<TEntity>, IDelete<TEntity> class
            sing.CreateOrder(myOrder);

            // sing.DeleteOrder methods:  (1) Main, (2) CreateSingleService in Program class (3) OrderController(crud, crud, crud) constructor in OrderController class
            // (4) Delete(Order order) in OrderController Class, (5) deleter.Delete(order) in Crud<TEntity> : IRead<TEntity>, ISave<TEntity>, IDelete<TEntity> ,
            // (6)Delete(TEntity entity) in Crud<TEntity> : IRead<TEntity>, ISave<TEntity>, IDelete<TEntity> class
            sing.DeleteOrder(myOrder);

            //Use the GenericController: CreateEntity()
            Console.WriteLine("=========GenericController<Order>=========");
            GenericController<Order> generic = CreateGenericServices();

            // generic.CreateEntity methods: (1) Main, (2) CreateGenericServices() in program class, 
            // (3) Activator.CreateInstance(typeof(GenericController<Order>), reader, saver, deleter) in GenericController<TEntity> Class,
            // (4) CreateEntity in GenericController<TEntity>, (5) saver.Save(entity) in  GenericController<TEntity> 
            // (6) Save(TEntity entity) in Saver<TEntity> : ISave<TEntity> class 
            generic.CreateEntity(myOrder);


            /////////////////////////////////////


            // Use the GenericController for Items: 
            Item myItem = new Item();
            myItem.itemId = Guid.NewGuid();
            myItem.product = "Charging Cord";
            myItem.cost = 50;
            
            // Code Modification: 
            // Created a new CreateGenricServiceItem() method in the progam class, it is exactly like the 
            // CreateGenericService() method used for objects except for the fact that any references to Order was changed to Item
            GenericController<Item> genericItem = CreateGenericServicesItem();


            ItemController itemTest = CreateSingleServiceItem();
            itemTest.CreateItem(myItem);
            itemTest.DeleteItem(myItem);



            Console.WriteLine("Hit any key to quit");
            Console.ReadKey();
        }

        static OrderController CreateSeparateServices()
        {
            var reader = new Reader<Order>();
            var saver = new Saver<Order>();
            var deleter = new Deleter<Order>();
            return new OrderController(reader, saver, deleter);
        }

        static OrderController CreateSingleService()
        {
            var crud = new Crud<Order>();
            return new OrderController(crud, crud, crud);
        }

        static GenericController<Order> CreateGenericServices()
        {
            var reader = new Reader<Order>();
            var saver = new Saver<Order>();
            var deleter = new Deleter<Order>();
            // This must be declared using reflection...
            GenericController<Order> ctl = (GenericController<Order>)Activator.CreateInstance(typeof(GenericController<Order>), reader, saver, deleter);
            //This does not work 
            //GenericController<Order> ctl = new GenericController(reader, saver, deleter);
            return ctl;
        }

        // New method created to handle items
        static GenericController<Item> CreateGenericServicesItem()
        {
            var reader = new Reader<Item>();
            var saver = new Saver<Item>();
            var deleter = new Deleter<Item>();
            // This must be declared using reflection...
            GenericController<Item> ctl = (GenericController<Item>)Activator.CreateInstance(typeof(GenericController<Item>), reader, saver, deleter);
            //This does not work 
            //GenericController<Order> ctl = new GenericController(reader, saver, deleter);
            return ctl;
        }

        static ItemController CreateSingleServiceItem()
        {
            var crud = new Crud<Item>();
            return new ItemController(crud, crud, crud);
        }

    }
}
