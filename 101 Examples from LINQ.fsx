
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

type Order = {ID:int; OrderDate:System.DateTime;Total:decimal }
type Customer = {CustomerId:int; CompanyName: string; Region:string; Orders:Order[]}
 
let GetCustomers = 
    [
        {CustomerId = 0; CompanyName = "A"; Region="WA"; 
            Orders = [|
                        {ID=1;OrderDate=new System.DateTime(2015,1,1);Total=10.0m}
                        {ID=2;OrderDate=new System.DateTime(2015,1,2);Total=30.0m}
                      |]
        };
        {CustomerId = 1; CompanyName = "B"; Region="CA"; 
            Orders = [|
                        {ID=1;OrderDate=new System.DateTime(2015,1,1); Total=5.0m}
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

//  static IEnumerable<TSource> WhereIterator<TSource>(IEnumerable<TSource> source, Func<TSource, int, bool> predicate) {
//            int index = -1;
//            foreach (TSource element in source) {
//                checked { index++; }
//                if (predicate(element, index)) yield return element;
//            }
//        }

//C# Literal
let digits = [ "zero"; "one"; "two"; "three"; "four"; "five"; "six"; "seven"; "eight"; "nine" ];

type f<'T> = 'T -> int -> bool

let where (source:list<string>) (predicate:f<string>) = 
    let index = ref -1
    seq {
            for s in source do
              index := !index + 1
              if (predicate s index.Value) then yield s
        }

let shortDigits = where digits (fun x i -> x.Length < i)

let Linq5 = 
    printfn "Short digits:"

    for digit in shortDigits do
        printfn "The word %s is shorter than its value" digit

//F# Way
let where2 digits f = 
    digits 
        |> Seq.mapi( fun i x -> i,x ) // convert to item, index
        |> Seq.filter f  // where caluse
        |> Seq.iter ( fun z-> printfn "The word %A is shorter than its value" (snd z)) 

where2 digits (fun (i, (x:string)) -> x.Length < i)


//Select - Simple 1
//public void Linq6() 
//{ 
//    int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 }; 
//  
//    var numsPlusOne = 
//        from n in numbers 
//        select n + 1; 
//  
//    Console.WriteLine("Numbers + 1:"); 
//    foreach (var i in numsPlusOne) 
//    { 
//        Console.WriteLine(i); 
//    } 
//}

let numbers2 = [ 5; 4; 1; 3; 9; 8; 6; 7; 2; 0 ]; 

printfn "Numbers + 1:"

numbers2 |>  Seq.map ((+) 1) |> Seq.iter (printfn "%d")

//Select - Simple 2
//public void Linq7() 
//{ 
//    List<Product> products = GetProductList(); 
//  
//    var productNames = 
//        from p in products 
//        select p.ProductName; 
//  
//    Console.WriteLine("Product Names:"); 
//    foreach (var productName in productNames) 
//    { 
//        Console.WriteLine(productName); 
//    } 
//}

GetProductList |> Seq.map (fun z-> z.ProductName) |> Seq.toArray

//Select - Transformation
//public void Linq8() 
//{ 
//    int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 }; 
//    string[] strings = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" }; 
//  
//    var textNums = 
//        from n in numbers 
//        select strings[n]; 
//  
//    Console.WriteLine("Number strings:"); 
//    foreach (var s in textNums) 
//    { 
//        Console.WriteLine(s); 
//    } 
//}
let strings = [| "zero"; "one"; "two"; "three"; "four"; "five"; "six"; "seven"; "eight"; "nine" |]; 

numbers |> Seq.map(fun z-> strings.[z] ) |> Seq.toArray

//Select - Anonymous Types 1
//public void Linq9() 
//{ 
//    string[] words = { "aPPLE", "BlUeBeRrY", "cHeRry" }; 
//  
//    var upperLowerWords = 
//        from w in words 
//        select new { Upper = w.ToUpper(), Lower = w.ToLower() }; 
//  
//    foreach (var ul in upperLowerWords) 
//    { 
//        Console.WriteLine("Uppercase: {0}, Lowercase: {1}", ul.Upper, ul.Lower); 
//    } 
//}

let words = [ "aPPLE"; "BlUeBeRrY"; "cHeRry" ]; 
type anonymous = {Upper:string; Lower:string}

words |> Seq.map (fun z-> {Upper=z.ToUpper();Lower = z.ToLower()}) |> Seq.toArray

//Select - Anonymous Types 2
//public void Linq10() 
//{ 
//    int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 }; 
//    string[] strings = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" }; 
//  
//    var digitOddEvens = 
//        from n in numbers 
//        select new { Digit = strings[n], Even = (n % 2 == 0) }; 
//  
//    foreach (var d in digitOddEvens) 
//    { 
//        Console.WriteLine("The digit {0} is {1}.", d.Digit, d.Even ? "even" : "odd"); 
//    } 
//}

type anonymous2 = {Digit:string; Even:bool}
numbers |> Seq.map(fun z-> {Digit = strings.[z]; Even = (z % 2 = 0) } ) |> Seq.toArray

//Select - Anonymous Types 3

//public void Linq11() 
//{ 
//    List<Product> products = GetProductList(); 
//  
//    var productInfos = 
//        from p in products 
//        select new { p.ProductName, p.Category, Price = p.UnitPrice }; 
//  
//    Console.WriteLine("Product Info:"); 
//    foreach (var productInfo in productInfos) 
//    { 
//        Console.WriteLine("{0} is in the category {1} and costs {2} per unit.", productInfo.ProductName, productInfo.Category, productInfo.Price); 
//    } 
//}

type Product3 = {ProductName: string; Category:string; UnitPrice:decimal}
 
let GetProductList3 = 
    [
        {ProductName = "A"; Category = "1"; UnitPrice  = 1.0m} 
        {ProductName = "B"; Category = "5"; UnitPrice  = 2.0m}
        {ProductName = "C"; Category = "10"; UnitPrice = 4.0m}
    ]

type anonymous3 = {ProductName:string; Category:string; Price:decimal}
GetProductList3 |> Seq.map(fun z-> {ProductName = z.ProductName; Category = z.Category; Price = z.UnitPrice } ) |> Seq.toArray

//Select - Indexed
//public void Linq12() 
//{ 
//    int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 }; 
//  
//    var numsInPlace = numbers.Select((num, index) => new { Num = num, InPlace = (num == index) }); 
//  
//    Console.WriteLine("Number: In-place?"); 
//    foreach (var n in numsInPlace) 
//    { 
//        Console.WriteLine("{0}: {1}", n.Num, n.InPlace); 
//    } 
//}

type anonymous4 = {Num:int; InPlace: bool}
numbers 
    |> Seq.mapi (fun x i -> {Num = i; InPlace = ( x = i) } ) 
    |> Seq.iter (fun z-> printfn "%i:%b" z.Num z.InPlace )

//Select - Filtered
//public void Linq13() 
//{ 
//    int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 }; 
//    string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" }; 
//  
//    var lowNums = 
//        from n in numbers 
//        where n < 5 
//        select digits[n]; 
//  
//    Console.WriteLine("Numbers < 5:"); 
//    foreach (var num in lowNums) 
//    { 
//        Console.WriteLine(num); 
//    } 
//}

printfn "Numbers < 5:"
let lowNums = 
    numbers 
        |> Seq.filter(fun z -> z < 5) 
        |> Seq.map(fun z-> digits.[z])

for num in lowNums do
    printfn "%s" num

//SelectMany - Compound from 1
//public void Linq14() 
//{ 
//    int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 }; 
//    int[] numbersB = { 1, 3, 5, 7, 8 }; 
//  
//    var pairs = 
//        from a in numbersA 
//        from b in numbersB 
//        where a < b 
//        select new { a, b }; 
//  
//    Console.WriteLine("Pairs where a < b:"); 
//    foreach (var pair in pairs) 
//    { 
//        Console.WriteLine("{0} is less than {1}", pair.a, pair.b); 
//    } 
//}
let numbersA = [ 0; 2; 4; 5; 6; 8; 9 ]; 
let numbersB = [ 1; 3; 5; 7; 8 ]; 

let pairs =
    numbersA 
        |> List.collect(fun a-> 
            numbersB |> List.filter (fun b-> a < b) 
                     |> List.map (fun y -> (a,y)))

printfn "Pairs where a < b:"
for pair in pairs do
    printfn "%i is less than %i" (fst(pair)) (snd(pair))


//SelectMany - Compound from 2
//public void Linq15() 
//{ 
//    List<Customer> customers = GetCustomerList(); 
//  
//    var orders = 
//        from c in customers 
//        from o in c.Orders 
//        where o.Total < 500.00M 
//        select new { c.CustomerID, o.OrderID, o.Total }; 
//  
//    ObjectDumper.Write(orders); 
//}

type anonymous5 = {CustomerId:int; OrderId:int; Total:decimal;}
GetCustomers 
    |> List.collect(fun c-> 
        c.Orders |> Seq.filter(fun o -> o.Total < 500.0m) 
                 |> Seq.toList
                 |> List.map(fun o -> {CustomerId = c.CustomerId; OrderId = o.ID; Total = o.Total}) )


//Object dumper
let (|Date|_|) (o:System.Object) =
  if (o.GetType()) = typeof<System.DateTime> then Some("DATE")
  else None


let Write (s:string) (writer:System.IO.TextWriter) =
    match s with
        | null -> ()
        | _ -> (writer.Write s)
    s.Length

let WriteIdent level (writer:System.IO.TextWriter) = 
    for i in 1..level do writer.Write "  ";

let WriteValue (o:System.Object) (writer:System.IO.TextWriter) =
    match o with 
      | null -> Write "null" writer
      | _ -> match o with 
              | o when o.GetType() = typeof<System.DateTime> -> Write ((o:?>System.DateTime).ToShortDateString()) writer
              | o when o.GetType() = typeof<System.ValueType> || o.GetType() = typeof<string> -> Write (o.ToString()) writer
              | o when o.GetType() = typeof<System.Collections.IEnumerable> -> Write "..." writer
              | _ -> 0 

//    private void WriteValue(object o) {
//        if (o == null) {
//            Write("null");
//        }
//        else if (o is DateTime) {
//            Write(((DateTime)o).ToShortDateString());
//        }
//        else if (o is ValueType || o is string) {
//            Write(o.ToString());
//        }
//        else if (o is IEnumerable) {
//            Write("...");
//        }
//        else {
//            Write("{ }");
//        }
//    }

let writeObject prefix o = 
    match o with 
        | null | ValuType | string -> (WriteIndent) (WritePrefix) (WriteValue o) (WriteLine)


case: null, ValueType or String
case: IEnumerable
none of the above: Use Reflection and recurse every property

//use active pattern to recgonize the type
  //keep calling the pattern


//TODO: ObjectDumper.Write
//private void WriteObject(string prefix, object o) {
//        if (o == null || o is ValueType || o is string) {
//            WriteIndent();
//            Write(prefix);
//            WriteValue(o);
//            WriteLine();
//        }
//        else if (o is IEnumerable) {
//            foreach (object element in (IEnumerable)o) {
//                if (element is IEnumerable && !(element is string)) {
//                    WriteIndent();
//                    Write(prefix);
//                    Write("...");
//                    WriteLine();
//                    if (level < depth) {
//                        level++;
//                        WriteObject(prefix, element);
//                        level--;
//                    }
//                }
//                else {
//                    WriteObject(prefix, element);
//                }
//            }
//        }
//        else {
//            MemberInfo[] members = o.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
//            WriteIndent();
//            Write(prefix);
//            bool propWritten = false;
//            foreach (MemberInfo m in members) {
//                FieldInfo f = m as FieldInfo;
//                PropertyInfo p = m as PropertyInfo;
//                if (f != null || p != null) {
//                    if (propWritten) {
//                        WriteTab();
//                    }
//                    else {
//                        propWritten = true;
//                    }
//                    Write(m.Name);
//                    Write("=");
//                    Type t = f != null ? f.FieldType : p.PropertyType;
//                    if (t.IsValueType || t == typeof(string)) {
//                        WriteValue(f != null ? f.GetValue(o) : p.GetValue(o, null));
//                    }
//                    else {
//                        if (typeof(IEnumerable).IsAssignableFrom(t)) {
//                            Write("...");
//                        }
//                        else {
//                            Write("{ }");
//                        }
//                    }
//                }
//            }
//            if (propWritten) WriteLine();
//            if (level < depth) {
//                foreach (MemberInfo m in members) {
//                    FieldInfo f = m as FieldInfo;
//                    PropertyInfo p = m as PropertyInfo;
//                    if (f != null || p != null) {
//                        Type t = f != null ? f.FieldType : p.PropertyType;
//                        if (!(t.IsValueType || t == typeof(string))) {
//                            object value = f != null ? f.GetValue(o) : p.GetValue(o, null);
//                            if (value != null) {
//                                level++;
//                                WriteObject(m.Name + ": ", value);
//                                level--;
//                            }
//                        }
//                    }
//                }
//            }
//        }
//    }

//SelectMany - Compound from 3
//public void Linq16() 
//{ 
//    List<Customer> customers = GetCustomerList(); 
//  
//    var orders = 
//        from c in customers 
//        from o in c.Orders 
//        where o.OrderDate >= new DateTime(1998, 1, 1) 
//        select new { c.CustomerID, o.OrderID, o.OrderDate }; 
//  
//    ObjectDumper.Write(orders); 
//}

//SelectMany - from Assignment
//SelectMany - Multiple from
//SelectMany - Indexed