
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
