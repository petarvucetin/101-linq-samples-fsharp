
//Where - Simple 1
//    public void Linq1() 
//    { 
//        int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 }; 
//      
//        var lowNums = 
//            from n in numbers 
//            where n < 5 
//            select n; 
//      
//        Console.WriteLine("Numbers < 5:"); 
//        foreach (var x in lowNums) 
//        { 
//            Console.WriteLine(x); 
//        } 
//    } 

let numbers = [ 5; 4; 1; 3; 9; 8; 6; 7; 2; 0]

let lowNumbers = 
    numbers //from
        |> Seq.filter (fun z-> z < 5) //where
        |> Seq.map    (fun t-> t) //select

let Linq1 = 
    printfn "Numbers < 5:"

    for i in lowNumbers do
        printfn "%d" i 

//Where - Simple 2
//  public void Linq2() 
//    { 
//        List<Product> products = GetProductList(); 
//      
//        var soldOutProducts = 
//            from p in products 
//            where p.UnitsInStock == 0 
//            select p; 
//      
//        Console.WriteLine("Sold out products:"); 
//        foreach (var product in soldOutProducts) 
//        { 
//            Console.WriteLine("{0} is sold out!", product.ProductName); 
//        } 
//    } 
 
type Product = {ProductName: string; UnitsInStock:int;}
 
let GetProductList = 
    [
        {ProductName = "A"; UnitsInStock = 0} 
        {ProductName = "B"; UnitsInStock = 5}
        {ProductName = "C"; UnitsInStock = 10}
    ]
let soldOutProducts = 
    GetProductList //from
    |> Seq.filter(fun z-> z.UnitsInStock = 0) //where
    //select is ommitted

let Linq2 = 
    for product in soldOutProducts do
        printfn "%s is sold out!" product.ProductName 


//Where - Simple 3
// public void Linq3() 
//    { 
//        List<Product> products = GetProductList(); 
//      
//        var expensiveInStockProducts = 
//            from p in products 
//            where p.UnitsInStock > 0 && p.UnitPrice > 3.00M 
//            select p; 
//      
//        Console.WriteLine("In-stock products that cost more than 3.00:"); 
//        foreach (var product in expensiveInStockProducts) 
//        { 
//            Console.WriteLine("{0} is in stock and costs more than 3.00.", product.ProductName); 
//        } 
//    } 

type Product2 = {ProductName: string; UnitsInStock:int; UnitPrice:decimal}
 
let GetProductList2 = 
    [
        {ProductName = "A"; UnitsInStock = 0; UnitPrice  = 1.0m} 
        {ProductName = "B"; UnitsInStock = 5; UnitPrice  = 2.0m}
        {ProductName = "C"; UnitsInStock = 10; UnitPrice = 4.0m}
    ]
let expensiveInStockProducts = 
    GetProductList2 //from
    |> Seq.filter(fun z-> z.UnitsInStock > 0 && z.UnitPrice > 3.0m) //where
    //select is ommitted

let Linq3 = 
    printfn "In-stock products that cost more than 3.00:"

    for product in expensiveInStockProducts do
        printfn "%s is in stock and costs more than 3.00!" product.ProductName 

//Where - Drilldown
// public void Linq4() 
//    { 
//        List<Customer> customers = GetCustomerList(); 
//      
//        var waCustomers = 
//            from c in customers 
//            where c.Region == "WA" 
//            select c; 
//      
//        Console.WriteLine("Customers from Washington and their orders:"); 
//        foreach (var customer in waCustomers) 
//        { 
//            Console.WriteLine("Customer {0}: {1}", customer.CustomerID, customer.CompanyName); 
//            foreach (var order in customer.Orders) 
//            { 
//                Console.WriteLine("  Order {0}: {1}", order.OrderID, order.OrderDate); 
//            } 
//        } 
//    } 

type Order = {ID:int; OrderDate:System.DateTime }
type Customer = {CustomerId:int; CompanyName: string; Region:string; Orders:Order[]}
 
let GetCustomers = 
    [
        {CustomerId = 0; CompanyName = "A"; Region="WA"; 
            Orders = [|
                        {ID=1;OrderDate=new System.DateTime(2015,1,1)}
                        {ID=2;OrderDate=new System.DateTime(2015,1,2)}
                      |]
        };
        {CustomerId = 1; CompanyName = "B"; Region="CA"; 
            Orders = [|
                        {ID=1;OrderDate=new System.DateTime(2015,1,1)}
                      |]

        };
    ]
let waCustomers = 
    GetCustomers //from
    |> Seq.filter(fun z-> z.Region = "WA") //where
    //select is ommitted

let Linq4 = 
    printfn "Customers from Washington and their orders:"

    for customer in waCustomers do
        printfn "Customer %d: %s" customer.CustomerId customer.CompanyName
        for order in customer.Orders do
            printfn "   Order %d: %s" order.ID (order.OrderDate.ToShortDateString())

//Where - Indexed
//public void Linq5() 
//{ 
//    string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" }; 
//      
//    var shortDigits = digits.Where((digit, index) => digit.Length < index); 
//      
//    Console.WriteLine("Short digits:"); 
//    foreach (var d in shortDigits) 
//    { 
//        Console.WriteLine("The word {0} is shorter than its value.", d); 
//    } 
//}

