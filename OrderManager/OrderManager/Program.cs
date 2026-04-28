const int DeliveryDays = 3;

Order order = GetOrderFromUser();

string resultMessage = ConfirmOrder( order )
    ? GetSuccessMessage( order, DateTime.Today.AddDays( DeliveryDays ) )
    : "Заказ отменён.";

Console.WriteLine( $"\n{resultMessage}" );


Order GetOrderFromUser()
{
    return new Order(
        Product: ReadInput( "Введите название товара: ", s => !string.IsNullOrWhiteSpace( s ) ),
        Count: int.Parse( ReadInput( "Введите количество: ", s => int.TryParse( s, out int n ) && n > 0 ) ),
        Name: ReadInput( "Введите ваше имя: ", s => !string.IsNullOrWhiteSpace( s ) ),
        Address: ReadInput( "Введите адрес доставки: ", s => !string.IsNullOrWhiteSpace( s ) )
    );
}

string ReadInput( string prompt, Func<string, bool> validator )
{
    while ( true )
    {
        Console.Write( prompt );

        string input = Console.ReadLine()?.Trim() ?? "";

        if ( validator( input ) )
        {
            return input;
        }
        Console.WriteLine( "Некорректный ввод. Попробуйте снова.\n" );
    }
}

bool ConfirmOrder( Order order )
{
    Console.WriteLine( $"\nЗдравствуйте, {order.Name}, вы заказали {order.Count} {order.Product} " +
                      $"на адрес {order.Address}, все верно? (y/n)" );

    return ReadInput( "Введите 'y' или 'n': ", s => s == "y" || s == "n" ) == "y";
}

string GetSuccessMessage( Order order, DateTime deliveryDate )
{
    return $"{order.Name}! Ваш заказ {order.Product} в количестве {order.Count} оформлен!\n" +
           $"Ожидайте доставку по адресу {order.Address} к {deliveryDate:dd.MM.yyyy}";
}

record Order( string Product, int Count, string Name, string Address );